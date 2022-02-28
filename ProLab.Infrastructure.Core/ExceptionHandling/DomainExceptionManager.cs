using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core.ExceptionHandling
{
    public class DomainExceptionManager : IDomainExceptionManager
    {
        protected List<ErrorMessage> ErrorMessages = new List<ErrorMessage>();

        public ErrorMessage this[string code]
        {
            get
            {
                return ErrorMessages.FirstOrDefault(_ => _.ErrorCode == code);
            }
        }
        public ErrorMessage GetByCode(string code)
        {
            return ErrorMessages.FirstOrDefault(_ => _.ErrorCode == code);
        }
        public IList<ErrorMessage> GetByType(string type)
        {
            return ErrorMessages.Where(_ => _.ErrorType == type).ToList();
        }
    }
}
