using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Dtos.User.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.FileStorage;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;


namespace CRM.Application.Services
{
    public class UserApplication : IUserApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _azureStorage;
        private readonly IOrderingQuery _orderingQuery;
        public UserApplication(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IAzureStorage azureStorage, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _azureStorage = azureStorage;
            _orderingQuery = orderingQuery;
        }
        public async Task<BaseResponse<IEnumerable<UserResponseDto>>> ListUsers(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<UserResponseDto>>();
            try
            {
                var users = _unitOfWork.User.GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            users = users.Where(x => x.UserName!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            users = users.Where(x => x.Email!.Contains(filters.TextFilter));
                            break;

                    }
                }
                if (filters.StateFilter is not null)
                {
                    users = users.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!String.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    users = users.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                bool shouldPaginate = !filters.Download.HasValue || !filters.Download.Value;
                var items = await _orderingQuery.Ordering(filters, users, shouldPaginate).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await users.CountAsync();
                response.Data = _mapper.Map<IEnumerable<UserResponseDto>>(items);
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
        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectUser()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();
            try
            {
                var users = await _unitOfWork.User.GetAlltAsync();
                if (users is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(users);
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
        public async Task<BaseResponse<UserByIdResponseDto>> UserById(int userId)
        {
            var response = new BaseResponse<UserByIdResponseDto>();
            try
            {
                var user = await _unitOfWork.User.GetByIdAsync(userId);
                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = _mapper.Map<UserByIdResponseDto>(user);
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
        public async Task<BaseResponse<bool>> RegisterUser(UserRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var account = _mapper.Map<User>(requestDto);

            account.Password = BC.HashPassword("user123");

            if (requestDto.Image is not null)
            {
                account.Image = await _azureStorage.SaveFile(AzureContainers.USERS, requestDto.Image);
            }

            response.Data = await _unitOfWork.User.RegisterAsync(account);

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

        public async Task<BaseResponse<bool>> EditUser(int userId, UserRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var user = _mapper.Map<User>(requestDto);
                user.Id = userId;
                response.Data = await _unitOfWork.User.EditAsync(user);
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
        public async Task<BaseResponse<bool>> RemoveUser(int userId)
        {
            var response = new BaseResponse<bool>();
            try
            {
                response.Data = await _unitOfWork.User.RemoveAsync(userId);
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
