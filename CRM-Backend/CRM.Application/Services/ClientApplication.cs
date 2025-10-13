using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Client.Request;
using CRM.Application.Dtos.Client.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using BC = BCrypt.Net.BCrypt;

namespace CRM.Application.Services
{
    public class ClientApplication : IClientApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public ClientApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<ClientResponseDto>>> ListClient(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ClientResponseDto>>();
            try
            {
                var clients = _unitOfWork.Client.GetAllQueryable()
                    .Include(x => x.DocumentType)                  
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            clients = clients.Where(x => x.Name!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            clients = clients.Where(x => x.Email!.Contains(filters.TextFilter));
                            break;
                        case 3:
                            clients = clients.Where(x => x.DocumentNumber!.Contains(filters.TextFilter));
                            break;
                    }
                }
                if (filters.StateFilter is not null)
                {
                    clients = clients.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!String.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    clients = clients.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                bool shouldPaginate = !filters.Download.HasValue || !filters.Download.Value;
                var items = await _orderingQuery.Ordering(filters, clients, shouldPaginate).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await clients.CountAsync();
                response.Data = _mapper.Map<IEnumerable<ClientResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectClient()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();
            try
            {
                var clients = await _unitOfWork.Client.GetAlltAsync();
                if (clients is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(clients);
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
        public async Task<BaseResponse<ClientByIdResponseDto>> ClientById(int clientId)
        {
            var response = new BaseResponse<ClientByIdResponseDto>();
            try
            {
                var client = await _unitOfWork.Client.GetByIdAsync(clientId);
                if (client is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = _mapper.Map<ClientByIdResponseDto>(client);
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

        public async Task<BaseResponse<bool>> RegisterClient(ClientRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var client = _mapper.Map<Client>(requestDto);
                client.Password = BC.HashPassword(requestDto.Password);

                response.Data = await _unitOfWork.Client.RegisterAsync(client);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;  
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }


        public async Task<BaseResponse<bool>> EditClient(int clientId, ClientRequestDto requestDto)
        {

            var response = new BaseResponse<bool>();
            try
            {
                var client = _mapper.Map<Client>(requestDto);
                client.Id = clientId;
                response.Data = await _unitOfWork.Client.EditAsync(client);
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

        public async Task<BaseResponse<bool>> RemoveClient(int clientId)
        {
            var response = new BaseResponse<bool>();
            try
            {
                response.Data = await _unitOfWork.Client.RemoveAsync(clientId);
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
