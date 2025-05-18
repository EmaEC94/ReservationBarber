using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Commons.Select.Response;
using CRM.Application.Dtos.Company.Request;
using CRM.Application.Dtos.Company.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using System.Linq.Expressions;

namespace CRM.Application.Services
{
    public class CompanyApplication : ICompanyApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public CompanyApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }
        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectCompany()
        {
            var response =  new BaseResponse<IEnumerable<SelectResponse>>();
            try
            {
                var companies = await _unitOfWork.Company.GetAlltAsync();
                if(companies is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<IEnumerable<SelectResponse>>(companies);
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterCompany(CompanyRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var company = _mapper.Map<Company>(requestDto);
                response.Data = await _unitOfWork.Company.RegisterAsync(company);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }
            return response;
        }
    }
}
