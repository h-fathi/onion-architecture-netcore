using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core
{
    public interface IAuditService
    {
        Task<string> GetClientIp();
        Task<CurrentUserInfo> GetCurrentUser();
    }
}
