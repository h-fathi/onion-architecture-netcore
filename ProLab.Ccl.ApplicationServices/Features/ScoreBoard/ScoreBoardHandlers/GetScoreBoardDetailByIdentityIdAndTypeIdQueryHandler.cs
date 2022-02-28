using MassTransit;
using ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Queries;
using ProLab.Ccl.Domain.ServicesInterface;
using ProLab.Infrastructure.Core;
using ProLab.Infrastructure.Core.ExceptionHandling;
using ProLab.Infrastructure.Web.RabbitMq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices.Features.ScoreBoard.ScoreBoardHandlers
{
    public class GetScoreBoardDetailByIdentityIdAndTypeIdQueryHandler : IQueryHandler<GetScoreBoardDetailByIdentityIdAndTypeIdQuery, ApiResponse>
    {
        private readonly IScoreBoardService _scoreBoardService;
        private readonly IDomainExceptionManager _domainExceptionManager;
        private readonly IMessageQueueEndpoint _publishEndpoint;
        public GetScoreBoardDetailByIdentityIdAndTypeIdQueryHandler(IScoreBoardService scoreBoardService, IDomainExceptionManager domainExceptionManager, IMessageQueueEndpoint publishEndpoint)
        {
            _scoreBoardService = scoreBoardService;
            _domainExceptionManager = domainExceptionManager;
            _publishEndpoint = publishEndpoint;

        }

        public async Task<ApiResponse> Handle(GetScoreBoardDetailByIdentityIdAndTypeIdQuery request, CancellationToken cancellationToken)
        {
            var message = _domainExceptionManager.GetByCode("Ccl-109");
            
            var data = new
            {
                ReportId = Guid.NewGuid(),
                RouteKey = "ScoreBoard",
                Data = request
            };

            await _publishEndpoint.Send("ScoreBoard", data);


           // var resultValue = await _scoreBoardService.GetSummationDetailsByIdentity(request.IdentityId);


            return new ApiResponse<int>
            {
                Success = true,
                Result = 0,
                Message = message.Message
            };
        }


    }
}
