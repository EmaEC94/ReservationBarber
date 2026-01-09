using CRM.Application.Commons.Bases.Request;
using CRM.Application.Dtos.Reservation.Request;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationApplication _reservationApplication;
        private readonly IGenerateExcelApplicattion _generateExcelApplicattion;
        public ReservationController(IReservationApplication reservationApplication, IGenerateExcelApplicattion generateExcelApplicattion)
        {
            _reservationApplication = reservationApplication;
            _generateExcelApplicattion = generateExcelApplicattion;
        }

        [HttpGet]
        public async Task<IActionResult> ListReservation([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _reservationApplication.ListReservation(filters);
            if (filters.Download.HasValue && filters.Download.Value)
            {
                //var columnNames = ExcelColumnNames.GetColumnsreservation();
                // var fileBytes = _generateExcelApplicattion.GenerateToExcel(response.Data!, columnNames);
                //return File(fileBytes, ContentType.ContentTypeExcel);
            }
            return Ok(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectreservation()
        {
            var response = await _reservationApplication.ListSelectReservation();
            return Ok(response);
        }

        [HttpPost("Register")]
        //[Authorize]
        public async Task<IActionResult> RegisterReservation([FromBody] ReservationRequestDto requestDto)
        {
            var response = await _reservationApplication.RegisterReservation(requestDto);
            return Ok(response);
        }

        [HttpGet("{reservationId:int}")]
        public async Task<IActionResult> ReservationById(int reservationId)
        {
            var response = await _reservationApplication.ReservationById(reservationId);
            return Ok(response);
        }

        [HttpPut("Edit/{reservationId}")]
        public async Task<IActionResult> EditReservation(int reservationId, [FromForm] ReservationRequestDto requestDto)
        {
            var response = await _reservationApplication.EditReservation(reservationId, requestDto);
            return Ok(response);
        }

        [HttpDelete("Remove/{reservationId:int}")]
        public async Task<IActionResult> RemoveReservation(int reservationId)
        {
            var response = await _reservationApplication.RemoveReservation(reservationId);
            return Ok(response);
        }
   
        [HttpPost("ReservationAvaibleRequest")]
        public async Task<IActionResult> GetReservationAvailble([FromBody] ReservationAvaibleRequest request)
        {
            var response = await _reservationApplication.ListReservationHourAvailble(request.DaySelected, request.UserId);
             return Ok(response);
        }
    }
}
