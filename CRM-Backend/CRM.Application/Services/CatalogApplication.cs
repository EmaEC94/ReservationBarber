using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Catalog.Request;
using CRM.Application.Dtos.Catalog.Response;
using CRM.Application.Dtos.User.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.FileStorage;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class CatalogApplication : ICatalogApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _azureStorage;
        private readonly IOrderingQuery _orderingQuery;

        public CatalogApplication(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IAzureStorage azureStorage, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _azureStorage = azureStorage;
            _orderingQuery = orderingQuery;
        }
        public async Task<BaseResponse<IEnumerable<CatalogResponseDto>>> ListCatalog(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<CatalogResponseDto>>();
            try
            {
                var catalog = _unitOfWork.Catalog.GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            catalog = catalog.Where(x => x.Description!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            catalog = catalog.Where(x => x.Name!.Contains(filters.TextFilter));
                            break;

                    }
                }
                if (filters.StateFilter is not null)
                {
                    catalog = catalog.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!String.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    catalog = catalog.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                bool shouldPaginate = !filters.Download.HasValue || !filters.Download.Value;
                var items = await _orderingQuery.Ordering(filters, catalog, shouldPaginate).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await catalog.CountAsync();
                response.Data = _mapper.Map<IEnumerable<CatalogResponseDto>>(items);
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }

            return response;
        }
        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectCatalog()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();
            try
            {
                var catalog = await _unitOfWork.Catalog.GetAlltAsync();
                if (catalog is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(catalog);
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<CatalogByIdResponseDto>> CatalogById(int catalogId)
        {
            var response = new BaseResponse<CatalogByIdResponseDto>();
            try
            {
                var user = await _unitOfWork.Catalog.GetByIdAsync(catalogId);
                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = _mapper.Map<CatalogByIdResponseDto>(user);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterCatalog(CatalogRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var catalago = _mapper.Map<Catalog>(requestDto);
            catalago.State = 1;

            if(catalago is not null)
            {
                response.Data = await _unitOfWork.Catalog.RegisterAsync(catalago);
            }

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }
            return response;
        }
        public async Task<BaseResponse<bool>> EditCatalog(int catalogId, CatalogRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            Catalog catalog = new Catalog();
            try
            {
                catalog = _mapper.Map<Catalog>(requestDto);
                catalog.Id = catalogId;
                response.Data = await _unitOfWork.Catalog.EditAsync(catalog);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public  async Task<BaseResponse<bool>> RemoveCatalog(int catalogId)
        {
            var response = new BaseResponse<bool>();
            try
            {
                response.Data = await _unitOfWork.Catalog.RemoveAsync(catalogId);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }
    }
}
