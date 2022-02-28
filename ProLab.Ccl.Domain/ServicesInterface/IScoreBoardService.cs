using ProLab.Ccl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Ccl.Domain.ServicesInterface
{
    public interface IScoreBoardService
    {
        Task<int> GetSummationDetailsByIdentity(int identityId);

        Task<int> InsertAsync(int identityId);
    }
}
