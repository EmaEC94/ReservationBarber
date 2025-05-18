using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Reservation.Request;
using CRM.Application.Dtos.Reservation.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Dtos.User.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.FileStorage;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;


namespace CRM.Application.Services
{
    public class ReservationApplication : IReservationApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IAzureStorage _azureStorage;
        private readonly IOrderingQuery _orderingQuery;
        public ReservationApplication(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _orderingQuery = orderingQuery;
        }
        public async Task<BaseResponse<IEnumerable<ReservationResponseDto>>> ListReservation(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ReservationResponseDto>>();
            try
            {
                var reservation = _unitOfWork.Reservation.GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            reservation = reservation.Where(x => x.Tittle!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            reservation = reservation.Where(x => x.Message!.Contains(filters.TextFilter));
                            break;
                    }
                }
                if (filters.StateFilter is not null)
                {
                    reservation = reservation.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!String.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    reservation = reservation.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                bool shouldPaginate = !filters.Download.HasValue || !filters.Download.Value;
                var items = await _orderingQuery.Ordering(filters, reservation, shouldPaginate).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await reservation.CountAsync();
                response.Data = _mapper.Map<IEnumerable<ReservationResponseDto>>(items);
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
        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectReservation()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();
            try
            {
                var reservation = await _unitOfWork.Reservation.GetAlltAsync();
                if (reservation is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(reservation);
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
        public async Task<BaseResponse<ReservationResponseDto>> ReservationById(int reservationId)
        {
            var response = new BaseResponse<ReservationResponseDto>();
            try
            {
                var user = await _unitOfWork.Reservation.GetByIdAsync(reservationId);
                if (user is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = _mapper.Map<ReservationResponseDto>(user);
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
        public async Task<BaseResponse<bool>> RegisterReservation(ReservationRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            var requestReservation = _mapper.Map<Reservation>(requestDto);

            response.Data = await _unitOfWork.Reservation.RegisterAsync(requestReservation);

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

        public async Task<BaseResponse<bool>> EditReservation(int reservationId, ReservationRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var reservation = _mapper.Map<Reservation>(requestDto);
                reservation.Id = reservationId;
                response.Data = await _unitOfWork.Reservation.EditAsync(reservation);
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
        public async Task<BaseResponse<bool>> RemoveReservation(int reservationId)
        {
            var response = new BaseResponse<bool>();
            try
            {
                response.Data = await _unitOfWork.Reservation.RemoveAsync(reservationId);
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

        public void ListReservationHourAvailbleTest(DateOnly dateSelected, int UserId)
        {
            var response = new BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>();
            try
            {
                var result = _unitOfWork.Reservation.GetHourAvailble(dateSelected, UserId);
                response.Data = (IQueryable<ReservationHourAvailbleResponseDto>?)_mapper.Map<ReservationHourAvailbleResponseDto>(result);
 
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
           // return response;
        }

        Task<BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>> IReservationApplication.ListReservationHourAvailbleTest(DateOnly dateSelected, int UserId)
        {
            var response = new BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>();
            try
            {
                var result = _unitOfWork.Reservation.GetHourAvailble(dateSelected, UserId);

                if (result != null)
                {
                    //response.Data = _mapper.Map<List<ReservationHourAvailbleResponseDto >> (result.Result.ToList());
                    response.Data = _mapper.Map<IEnumerable<ReservationHourAvailbleResponseDto>>(result.Result.ToList()).AsQueryable();


                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_DELETE;
                    return Task.FromResult(response);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return Task.FromResult(response);

        }

        Task<BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>> IReservationApplication.ListReservationHourAvailble(DateOnly dateSelected, int UserId)
        {
            var response = new BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>();
            try
            {
                var result = _unitOfWork.Reservation.GetHourAvailble(dateSelected, UserId);

                if (result != null)
                {
                    //response.Data = _mapper.Map<List<ReservationHourAvailbleResponseDto >> (result.Result.ToList());
                    response.Data = _mapper.Map<IEnumerable<ReservationHourAvailbleResponseDto>>(result.Result.ToList()).AsQueryable();


                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_DELETE;
                    return Task.FromResult(response);
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return Task.FromResult(response);
        }
    }
}
