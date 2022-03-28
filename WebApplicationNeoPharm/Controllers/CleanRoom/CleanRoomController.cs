using System;

using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplicationNeoPharm.React;

namespace WebApplicationNeoPharm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CleanRoomController : ControllerBase
    {
        protected readonly ILogger<CleanRoomController> _logger;



        private readonly IHttpClientFactory _httpClientFactory;

        public CleanRoomController(ILogger<CleanRoomController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }



        // GET api/values
        [HttpGet]
        [Route("Prep")]
        public async Task<IActionResult> GetPrep(string prepBarCode)
        {
            CleanRoomSrv cleanRoomSrv = new CleanRoomSrv(_httpClientFactory);

            var prep = await cleanRoomSrv.GetPrep();
            return Ok(prep);

            // return StatusCode(500, new { Info = er.Message, Source = "Register.Post" });

        }
      

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RootobjectReactFormRes rootobjectReactFormRes)
        {
            CleanRoomSrv cleanRoomSrv = new CleanRoomSrv( _httpClientFactory);

            RootobjectReactFormRes  prep = await cleanRoomSrv.PatchDto(rootobjectReactFormRes);
            if (prep.ErrorCode!=200)  return StatusCode(500, new { Info = prep.ErrorCode, ErrorMessage = prep.ErrorMessage , Source = prep.Source , ErrorInnerException = prep.ErrorInnerException});
            return Ok("Success");

        }

       
    }
}
