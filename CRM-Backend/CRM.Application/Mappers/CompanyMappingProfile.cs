using AutoMapper;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Company.Request;
using CRM.Application.Dtos.Company.Response;
using CRM.Domain.Entities;
using CRM.Utilities.Static;

namespace CRM.Application.Mappers
{
    public class CompanyMappingProfile : Profile
    {
        public CompanyMappingProfile()
        {
            CreateMap<Company, CompanyResponseDto>()
                .ForMember(x => x.StateCompany, x => x.MapFrom(y=>y.State.Equals((int)StateTypes.Active)? "Activo": "Inactivo"))
                .ReverseMap();

            CreateMap<CompanyRequestDto, Company>();

            CreateMap<Company, SelectResponse>()
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Name))
                .ReverseMap();
        }
    }
}
