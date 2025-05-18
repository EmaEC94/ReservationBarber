using AutoMapper;
using CRM.Application.Dtos.DocumentType.Resposne;
using CRM.Domain.Entities;

namespace CRM.Application.Mappers
{
    public class DocumentTypeMappingProfile : Profile
    {
        public DocumentTypeMappingProfile()
        {

            CreateMap<DocumentType, DocumentTypeResponseDto>()
                .ForMember(x => x.DocumentTypeId, x => x.MapFrom(y => y.Id))
                .ReverseMap();

        }
    }
}
