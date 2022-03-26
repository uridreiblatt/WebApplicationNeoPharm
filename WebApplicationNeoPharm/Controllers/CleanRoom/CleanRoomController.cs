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
            CleanRoomSrv cleanRoomSrv = new CleanRoomSrv(_logger, _httpClientFactory);

            var prep = await cleanRoomSrv.GetPrep();
            return Ok(prep);
            //try
            //{
            //    _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/ValuesController", "GET");
            //    NamedClientModel namedClientModel = new NamedClientModel(_httpClientFactory);
            //    var v = await namedClientModel.GetPrep();
            //    if (v == null) return NotFound(new { Info = "no data found" });

            //}
            //catch (Exception er)
            //{

            //    _logger.LogError(er.Message, "GetPrep");
            //    return StatusCode(500, new { Info = er.Message, Source = "Register.Post" });
            //}


        }
        // GET api/values
        //[HttpGet]
        //[Route("Station")]
        //public async Task<IActionResult> GetStation()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/ValuesController", "GET");
        //        NamedClientModel namedClientModel = new NamedClientModel(_httpClientFactory);
        //        var v = await namedClientModel.GetStation("");
        //        if (v == null) return NotFound(new { Info = "no data found" });
        //        return Ok(v);
        //    }
        //    catch (Exception er)
        //    {

        //        _logger.LogError(er.Message, "GET");
        //        return StatusCode(500, new { Info = er.Message, Source = "Register.Post" });
        //    }


        //}
        // GET api/values
        //[HttpGet]
        //[Route("Equipment")]
        //public async Task<IActionResult> GetEquipment()
        //{
        //    try
        //    {
        //        _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/ValuesController", "GET");
        //        NamedClientModel namedClientModel = new NamedClientModel(_httpClientFactory);
        //        var v = await namedClientModel.GetEquipment();
        //        if (v == null) return NotFound(new { Info = "no data found" });
        //        return Ok(v);
        //    }
        //    catch (Exception er)
        //    {

        //        _logger.LogError(er.Message, "GET");
        //        return StatusCode(500, new { Info = er.Message, Source = "Register.Post" });
        //    }


        //}

        // GET api/values/5
        //[HttpGet("{id}")]
        //public ActionResult<string> Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RootobjectReactFormRes rootobjectReactFormRes)
        {
            CleanRoomSrv cleanRoomSrv = new CleanRoomSrv(_logger, _httpClientFactory);

            RootobjectReactFormRes  prep = await cleanRoomSrv.PatchDto(rootobjectReactFormRes);
            if (prep.ErrorCode!=200)  return StatusCode(500, new { Info = prep.ErrorCode, ErrorMessage = prep.ErrorMessage , Source = prep.Source , ErrorInnerException = prep.ErrorInnerException});
            return Ok("Success");

        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
