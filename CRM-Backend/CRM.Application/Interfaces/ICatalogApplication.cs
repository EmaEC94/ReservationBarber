using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Catalog.Request;
using CRM.Application.Dtos.Catalog.Response;
using CRM.Application.Dtos.SubCatalog.Request;
using CRM.Application.Dtos.SubCatalog.Response;
using CRM.Application.Dtos.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface ICatalogApplication
    {
        Task<BaseResponse<IEnumerable<CatalogResponseDto>>> ListCatalog(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectCatalog();
        Task<BaseResponse<CatalogByIdResponseDto>> CatalogById(int catalogId);
        Task<BaseResponse<bool>> RegisterCatalog(CatalogRequestDto requestDto);
        Task<BaseResponse<bool>> EditCatalog(int catalogId, CatalogRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveCatalog(int catalogId);               
    }
}
