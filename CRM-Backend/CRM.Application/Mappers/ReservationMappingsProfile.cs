using AutoMapper;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Reservation.Request;
using CRM.Application.Dtos.Reservation.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Dtos.User.Response;
using CRM.Domain.Entities;
using CRM.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Mappers
{
    public class ReservationMappingsProfile : Profile
    {
        public ReservationMappingsProfile(){
            CreateMap<Reservation, ReservationResponseDto>()
            .ForMember(x => x.RervationId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.State, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
            .ReverseMap();

            CreateMap<Reservation, SelectResponse>()
           .ForMember(x => x.Description, x => x.MapFrom(y => y.Title))
           .ReverseMap();

            CreateMap<Reservation, ReservationResponseDto>()
              .ForMember(x => x.RervationId, x => x.MapFrom(y => y.Id))
              .ReverseMap();

            CreateMap<ReservationRequestDto, Reservation>();

            CreateMap<ReservationHourAvailble, ReservationHourAvailbleResponseDto>().ReverseMap();
        }
    }
}
