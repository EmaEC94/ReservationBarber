using AutoMapper;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Catalog.Request;
using CRM.Application.Dtos.Catalog.Response;
using CRM.Domain.Entities;
using CRM.Utilities.Static;

namespace CRM.Application.Mappers
{
    public class CatalogMappingsProfile : Profile
    {
        public CatalogMappingsProfile()
        {
            CreateMap<Catalog, CatalogResponseDto>()
                .ForMember(x => x.CatalogId, x => x.MapFrom(y => y.Id))
                .ForMember(x => x.StateCatalog, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo"))
                .ReverseMap();

            CreateMap<Catalog, SelectResponse>()
           .ForMember(x => x.Description, x => x.MapFrom(y => y.Name))
           .ReverseMap();


            CreateMap<Catalog, CatalogByIdResponseDto>()
              .ForMember(x => x.CatalogId, x => x.MapFrom(y => y.Id))
              .ReverseMap();

            CreateMap<CatalogRequestDto, Catalog>();
        }
    }
}
