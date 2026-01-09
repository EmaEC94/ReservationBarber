using AutoMapper;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.SubCatalog.Request;
using CRM.Application.Dtos.SubCatalog.Response;
using CRM.Domain.Entities;
using CRM.Utilities.Static;

namespace CRM.Application.Mappers
{
    public class SubCatalogMappingsProfile : Profile
    {
        public SubCatalogMappingsProfile()
        {
            CreateMap<SubCatalog, SubCatalogResponseDto>()
                .ForMember(x => x.SubCatalogId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.StateSubCatalog, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();

            CreateMap<SubCatalog, SelectResponse>()
           .ForMember(x => x.Description, x => x.MapFrom(y => y.Name))
           .ReverseMap();


            CreateMap<SubCatalog, SubCatalogByIdResponseDto>()
              .ForMember(x => x.SubCatalogId, x => x.MapFrom(y => y.Id))
              .ReverseMap();

            CreateMap<SubCatalogRequestDto, SubCatalog>().ReverseMap();


        }
    }
}
