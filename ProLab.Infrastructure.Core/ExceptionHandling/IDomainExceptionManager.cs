using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Core.ExceptionHandling
{
    public interface IDomainExceptionManager
    {
        ErrorMessage GetByCode(string code);
        IList<ErrorMessage> GetByType(string type);
    }
}
