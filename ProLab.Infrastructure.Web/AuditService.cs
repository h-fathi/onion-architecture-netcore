using Microsoft.AspNetCore.Http;
using ProLab.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Web
{
    public class AuditService: IAuditService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        public Task<string> GetClientIp()
        {
            var context = _httpContextAccessor.HttpContext.Request;
            var ip = context.Headers["X-Forwarded-For"];

            return Task.FromResult<string>(ip);
        }


        public Task<CurrentUserInfo> GetCurrentUser()
        {
            var context = _httpContextAccessor.HttpContext.Request;
            var userId = context.Headers["userId"];
            var identityId = context.Headers["IdentityId"];
            var tseAccountId = context.Headers["TseAccountId"];
            var tseAccountNumber = context.Headers["TseAccountNumber"];
            var tseAccountCode = context.Headers["TseAccountCode"];
            var permissionKeys = context.Headers["PermissionKeys"];

            var result = new CurrentUserInfo
            {
                IdentityId = Convert.ToInt32(identityId),
                UserId = Convert.ToInt32(userId),
                TseAccountId = Convert.ToInt32(tseAccountId),
                TseAccountNumber = tseAccountNumber,
                TseAccountCode = tseAccountCode,
                PermissionKeys = permissionKeys
            };

            return Task.FromResult(result);
        }
    }
}
