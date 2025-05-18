using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Reservation.Request;
using CRM.Application.Dtos.Reservation.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Dtos.User.Response;

namespace CRM.Application.Interfaces
{
    public interface IReservationApplication
    {
        Task<BaseResponse<IEnumerable<ReservationResponseDto>>> ListReservation(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectReservation();
        Task<BaseResponse<ReservationResponseDto>> ReservationById(int userId);
        Task<BaseResponse<bool>> RegisterReservation(ReservationRequestDto requestDto);
        Task<BaseResponse<bool>> EditReservation(int reservationId, ReservationRequestDto requestDto);
        Task<BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>> ListReservationHourAvailble(DateOnly dateSelected, int UserId);
        Task<BaseResponse<bool>> RemoveReservation(int reservationId);
        Task<BaseResponse<IQueryable<ReservationHourAvailbleResponseDto>>> ListReservationHourAvailbleTest(DateOnly dateSelected, int UserId);

         
    }
}
