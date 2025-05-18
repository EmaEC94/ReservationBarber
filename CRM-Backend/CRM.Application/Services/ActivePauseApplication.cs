using AutoMapper;
using CRM.Application.Commons.Bases.Request;
using CRM.Application.Commons.Bases.Response;
using CRM.Application.Commons.Ordering;
using CRM.Application.Dtos.ActivePause.Request;
using CRM.Application.Dtos.ActivePause.Response;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Infrastructure.Persistences.Interfaces;
using CRM.Utilities.Static;
using Microsoft.EntityFrameworkCore;
using WatchDog;

namespace CRM.Application.Services
{
    public class ActivePauseApplication : IActivePauseApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFileStoreLocalApplication _fileStore;

        public ActivePauseApplication(IOrderingQuery orderingQuery, IMapper mapper, IUnitOfWork unitOfWork, IFileStoreLocalApplication fileStore)
        {
            _orderingQuery = orderingQuery;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _fileStore = fileStore;
        }
       

        public async Task<BaseResponse<IEnumerable<ActivePauseResponseDto>>> ListActivePause(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ActivePauseResponseDto>>();
            try
            {
                var activePauses = _unitOfWork.ActivePause.GetAllQueryable()                   
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            activePauses = activePauses.Where(x => x.Titulo!.Contains(filters.TextFilter));
                            break;                       
                    }
                }
                if (filters.StateFilter is not null)
                {
                    activePauses = activePauses.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!String.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    activePauses = activePauses.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }

                filters.Sort ??= "Id";
                bool shouldPaginate = !filters.Download.HasValue || !filters.Download.Value;
                var items = await _orderingQuery.Ordering(filters, activePauses, shouldPaginate).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await activePauses.CountAsync();
                response.Data = _mapper.Map<IEnumerable<ActivePauseResponseDto>>(items);
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {

                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchDog.WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterActivePause(ActivePauseRequestDto requestDto)
        {
            var response =  new BaseResponse<bool>();
            try
            {
                var activePause = _mapper.Map<ActivePause>(requestDto);
                if (requestDto.Image is not null)
                    activePause.Image = await _fileStore.SaveFile(AzureContainers.ACTIVE_PAUSE, requestDto.Image);

                await _unitOfWork.ActivePause.RegisterAsync(activePause);
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            catch (Exception ex)
            {
                
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);

            }
            return response;
        }
    }
}
