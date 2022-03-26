
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using WebApplicationNeoPharm.Dal;
using WebApplicationNeoPharm.Model;

namespace WebApplicationNeoPharm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {


        //private readonly DataContext _context;

        protected readonly ILogger<UserController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(ILogger<UserController> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;

        }

        //GET api/values
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            throw new NotImplementedException();


        }
        [HttpGet]
        [Route("SyncUsers")]
        public IActionResult SyncUsers()
        {
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/ValuesController", "GET");
            NamedClientModel namedClientModel = new NamedClientModel(_httpClientFactory);
            ClsPrepUsers clsPrepUsers = namedClientModel.GetUsers().Result;
            return Ok(clsPrepUsers);


        }




    }
}
