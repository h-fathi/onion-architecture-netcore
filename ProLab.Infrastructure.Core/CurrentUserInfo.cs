using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core
{
    public class CurrentUserInfo
    {
        public int UserId { get; set; }
        public int IdentityId { get; set; }
        public int TseAccountId { get; set; }
        public string TseAccountNumber { get; set; }
        public string TseAccountCode { get; set; }
        public string PermissionKeys { get; set; }


    }
}
