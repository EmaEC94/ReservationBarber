using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.SubCatalog.Request;
using CRM.Application.Dtos.SubCatalog.Response;
using CRM.Application.Dtos.User.Request;
using CRM.Application.Dtos.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface ISubCatalogApplication
    {
        Task<BaseResponse<IEnumerable<SubCatalogResponseDto>>> ListSubCatalog(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectSubCatalog();
        Task<BaseResponse<SubCatalogByIdResponseDto>> SubCatalogById(int subCatalogId);
        Task<BaseResponse<bool>> RegisterSubCatalog(SubCatalogRequestDto requestDto);
        Task<BaseResponse<bool>> EditSubCatalog(int subCatalogId, SubCatalogRequestDto  requestDto);
        Task<BaseResponse<bool>> RemoveSubCatalog(int subCatalogId);
    }
}
