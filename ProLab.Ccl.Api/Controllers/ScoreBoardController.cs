
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProLab.Infrastructure.Web;

namespace ProLab.Ccl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoreBoardController : BaseController
    {


        [HttpGet]
        public async Task<IActionResult> Test(int identityId, string url)
        {
            var response = await Mediator.Send(new GetScoreBoardDetailByIdentityIdAndTypeIdQuery(identityId, url));
            return Ok(response);
        }
    }
}
