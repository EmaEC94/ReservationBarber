using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.SubCatalog.Request;
using CRM.Application.Dtos.SubCatalog.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.FileStorage;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CRM.Application.Services
{
    public class SubCatalogApplication : ISubCatalogApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _azureStorage;
        private readonly IOrderingQuery _orderingQuery;

        public SubCatalogApplication(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IAzureStorage azureStorage, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _azureStorage = azureStorage;
            _orderingQuery = orderingQuery;
        }     
        public async Task<BaseResponse<IEnumerable<SubCatalogResponseDto>>> ListSubCatalog(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<SubCatalogResponseDto>>();
            try
            {
                var subCatalog = _unitOfWork.SubCatalog.GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            subCatalog = subCatalog.Where(x => x.Description!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            subCatalog = subCatalog.Where(x => x.Name!.Contains(filters.TextFilter));
                            break;

                    }
                }
                if (filters.StateFilter is not null)
                {
                    subCatalog = subCatalog.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!String.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    subCatalog = subCatalog.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                bool shouldPaginate = !filters.Download.HasValue || !filters.Download.Value;
                var items = await _orderingQuery.Ordering(filters, subCatalog, shouldPaginate).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await subCatalog.CountAsync();
                response.Data = _mapper.Map<IEnumerable<SubCatalogResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectSubCatalog()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();
            try
            {
                var catalog = await _unitOfWork.SubCatalog.GetAlltAsync();
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

        public async Task<BaseResponse<SubCatalogByIdResponseDto>> SubCatalogById(int subCatalogId)
        {
            var response = new BaseResponse<SubCatalogByIdResponseDto>();
            try
            {
                var SubCatalog = await _unitOfWork.SubCatalog.GetByIdAsync(subCatalogId);
                if (SubCatalog is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = _mapper.Map<SubCatalogByIdResponseDto>(SubCatalog);
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
        public async Task<BaseResponse<bool>> RegisterSubCatalog(SubCatalogRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var subCatalago = _mapper.Map<SubCatalog>(requestDto);
            subCatalago.State = 1;
            response.Data = await _unitOfWork.SubCatalog.RegisterAsync(subCatalago);

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

        public async Task<BaseResponse<bool>> EditSubCatalog(int subCatalogId, SubCatalogRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var subCatalog = _mapper.Map<SubCatalog>(requestDto);
                subCatalog.Id = subCatalogId;
                response.Data = await _unitOfWork.SubCatalog.EditAsync(subCatalog);
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

        public async Task<BaseResponse<bool>> RemoveSubCatalog(int subCatalogId)
        {
            var response = new BaseResponse<bool>();
            try
            {
                response.Data = await _unitOfWork.SubCatalog.RemoveAsync(subCatalogId);
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
