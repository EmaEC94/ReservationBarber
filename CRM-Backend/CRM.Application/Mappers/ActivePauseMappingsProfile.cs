using AutoMapper;
using CRM.Application.Dtos.ActivePause.Request;
using CRM.Application.Dtos.ActivePause.Response;
using CRM.Domain.Entities;
using CRM.Utilities.Static;

namespace CRM.Application.Mappers
{
    public class ActivePauseMappingsProfile : Profile
    {
        public ActivePauseMappingsProfile()
        {
            CreateMap<ActivePause, ActivePauseResponseDto>()
                .ForMember(x => x.StateAtivePause, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
            .ReverseMap();

            CreateMap<ActivePauseRequestDto, ActivePause>();


        }
    }
}
