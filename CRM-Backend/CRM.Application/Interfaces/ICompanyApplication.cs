using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Company.Request;
using CRM.Application.Dtos.Company.Response;

namespace CRM.Application.Interfaces
{
    public interface ICompanyApplication
    {
        //Task<BaseResponse<IEnumerable<CompanyResponseDto>>> ListCompany(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectCompany();   
        Task<BaseResponse<bool>> RegisterCompany(CompanyRequestDto requestDto);
        //Task<BaseResponse<bool>> EditCompany(int companyId, CompanyRequestDto requestDto);
        //Task<BaseResponse<bool>> RemoveCompany(int companyId);
    }
}
