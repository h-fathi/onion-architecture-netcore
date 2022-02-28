using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Web.Logging
{

    public interface ICorrelationIdAccessor
    {
        string GetCorrelationId();
    }


}
