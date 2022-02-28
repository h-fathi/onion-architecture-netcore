using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProLab.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProLab.Infrastructure.Web
{
    public class BaseController : ControllerBase
    {
        public readonly IMediator Mediator;

        public BaseController()
        {
            Mediator = EngineContext.Current.Resolve<IMediator>();
        }
    }
    
}
