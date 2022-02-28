using MediatR;
using ProLab.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Queries
{
    public class GetScoreBoardDetailByIdentityIdAndTypeIdQuery: IQuery<ApiResponse>
    {
        public GetScoreBoardDetailByIdentityIdAndTypeIdQuery(int identityId, string url )
        {
            IdentityId = identityId;
            Name = url;
        }


        public int IdentityId { get; set; }
        public bool AreYouIn { get; set; }
        public string Name { get; set; }
        public DateTime UtcDate { get; set; }
        public List<int> Datails { get; set; }
    }
}
