using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebApplicationNeoPharm.Dal;
using WebApplicationNeoPharm.Model;
using WebApplicationNeoPharm.React;

namespace WebApplicationNeoPharm.Controllers
{
    public class CleanRoomSrv
    {
        protected readonly ILogger<CleanRoomSrv> _logger;



        private readonly IHttpClientFactory _httpClientFactory;
        private ILogger<CleanRoomController> logger;
        private IHttpClientFactory httpClientFactory;

        public CleanRoomSrv(ILogger<CleanRoomSrv> logger, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public CleanRoomSrv(ILogger<CleanRoomController> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public PrepDto PrepDto { get; private set; }

        public async Task<RootobjectReactFormRes> GetPrep()
        {
            RootobjectReactFormRes rootobjectReactFormRes = new RootobjectReactFormRes();
            try
            {

                _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/ValuesController", "GET");
                NamedClientModel namedClientModel = new NamedClientModel(_httpClientFactory);

                ClsPrepUsers clsPrepUser = await namedClientModel.GetUsers();

                ClsPrepEquiment ePrepEqpmnt = await namedClientModel.GetEquipment();

                ClsPrepStation clsPrepStation = await namedClientModel.GetStation("");


                ClsPrepTask prep = await namedClientModel.GetPrep();
                // stat = await namedClientModel.GetStation("");
              



                PrepDto prepDto = new PrepDto();

               
                rootobjectReactFormRes = prepDto.BuildForm(prep, clsPrepStation, ePrepEqpmnt, clsPrepUser);
                
            }
            catch (Exception exp)
            {

                _logger.LogError(exp.Message);
                rootobjectReactFormRes.ErrorCode = 100;
                rootobjectReactFormRes.ErrorMessage = exp.Message;
                rootobjectReactFormRes.ErrorInnerException = exp.InnerException?.ToString();

            }
            return rootobjectReactFormRes;


        }

        public async Task<RootobjectReactFormRes> PatchDto(RootobjectReactFormRes rootobjectReactFormReq)
        {
         
            _logger.LogInformation("Run endpoint {endpoint} {verb}", "/api/ValuesController", "PATCH");
            NamedClientModel namedClientModel = new NamedClientModel(_httpClientFactory);


            PrepDto prepDto = new PrepDto();
            ClsPostPrep clsPostPrep = prepDto.BuildPost(rootobjectReactFormReq.rootobjectReactForm);
            RootobjectReactFormRes rootobjectReactFormRes =  await namedClientModel.PostPrep(clsPostPrep);
            return rootobjectReactFormRes;
            //rootobjectReactFormRes.ErrorCode = 200;
            //rootobjectReactFormRes.ErrorMessage = "Success";

            //if (httpPostRes.Contains("Rejected"))
            //{
            //    httpPostRes = httpPostRes.Replace("<html><head><title>Request Rejected</title></head><body>", "");
            //    httpPostRes = httpPostRes.Replace("<br><br><a href='javascript:history.back();'>[Go Back]</a></body></html>", "");
            //    rootobjectReactFormRes.ErrorMessage = "Patch faild = > " + httpPostRes;
            //    rootobjectReactFormRes.ErrorCode = 204;
            //}



        }
    }
}
