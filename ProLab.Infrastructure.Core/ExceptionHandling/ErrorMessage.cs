using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core.ExceptionHandling
{
    public record ErrorMessage
    {
        public string MemberName { get; set; }
        public string ErrorType { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string FaMessage { get; set; }
    }
}
