using ProLab.Ccl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.DomainServices.RepositoryInterfaces
{
    public interface IScoreBoardRepository
    {
        Task<ICollection<ScoreBoardDetail>> GetDetailsByIdentityId(int identityId);
        Task UpdateScoreBoardDetail(int id = 51);

        Task Insert(int identityId);
    }
}
