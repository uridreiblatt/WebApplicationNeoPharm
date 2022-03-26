using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplicationNeoPharm.Const;
using WebApplicationNeoPharm.Exceptions;
using WebApplicationNeoPharm.Model;
using WebApplicationNeoPharm.React;



//Station
namespace WebApplicationNeoPharm.Dal
{
    public class NamedClientModel
    {
        private readonly IHttpClientFactory _clientFactory;

        //public ClsRootobjectCustomr PullRequests { get; private set; }

        public NamedClientModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ClsRootobjectCustomr> GetCustomer()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "NEO_APICUST?$filter=( PHONE2 eq '053-8778799' or PHONENUM eq '053-8778799' )&$top=1");

            var client = _clientFactory.CreateClient("priority");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    return await System.Text.Json.JsonSerializer.DeserializeAsync<ClsRootobjectCustomr>(responseStream);
                }


            }
            else
            {
                return null;
            }

        }
        public async Task<ClsPrepTask> GetPrep()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                NeoPharmServicesConsts.CleanRoomPrep);

            var client = _clientFactory.CreateClient("priority");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    return await System.Text.Json.JsonSerializer.DeserializeAsync<ClsPrepTask>(responseStream);
                }

            }
            else
            {
                return null;
            }


        }

        public async Task<ClsPrepStation> GetStation(string urlParam)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "NEO_CURTYPE?$select=CODE,DES&$expand=NEO_STATIONPREP_SUBFORM");

            var client = _clientFactory.CreateClient("priority");
            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    using (var responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        return await System.Text.Json.JsonSerializer.DeserializeAsync<ClsPrepStation>(responseStream);
                    }

                }
                else
                {
                    throw new BusinessException(EMessages.EStationParseError);

                }
            }
            catch (Exception er)
            {

                throw new Exception("NamedClientModel => GetStation => " + er.Message);
            }





        }

        public async Task<ClsPrepEquiment> GetEquipment()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "NEO_ADDEQUIPMENT");

            var client = _clientFactory.CreateClient("priority");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    return await System.Text.Json.JsonSerializer.DeserializeAsync<ClsPrepEquiment>(responseStream);
                }

            }
            else
            {
                return null;
            }


        }

        public async Task<ClsPrepUsers> GetUsers()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                "NEO_USERSBAPI?$filter=INACTIVE ne 'Y'");

            var client = _clientFactory.CreateClient("priority");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    return await System.Text.Json.JsonSerializer.DeserializeAsync<ClsPrepUsers>(responseStream);
                }
            }
            else
            {
                return null;
            }


        }


        public async Task<RootobjectReactFormRes> PostPrep(ClsPostPrep clsPostPrep)
        {
            RootobjectReactFormRes rootobjectReactFormRes = new RootobjectReactFormRes();
            rootobjectReactFormRes.Source = "post prep->patch";

            var jsonRequest = System.Text.Json.JsonSerializer.Serialize(clsPostPrep);
            using (var client = _clientFactory.CreateClient("priority"))
            {
                //client.BaseAddress = new Uri("https://ngpapi.neopharmgroup.com/odata/Priority/tabula.ini/eld0999/");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
                //.DefaultRequestHeaders.Add("Accept", "application/json; charset=utf-8");
                var method = "PATCH";
                var httpVerb = new HttpMethod(method);
                var httpRequestMessage =
                    new HttpRequestMessage(httpVerb, "NEO_PREPTASK('PRP2100002')?$expand=NEO_PRESCRIPTION_LBL_SUBFORM")
                    {
                        Content = new StringContent(jsonRequest, System.Text.Encoding.UTF8, "application/json")
                    };
                try
                {
                    var response = await client.SendAsync(httpRequestMessage);

                    if (!response.IsSuccessStatusCode)
                    {
                        rootobjectReactFormRes.ErrorCode = int.Parse(response.StatusCode.ToString());
                        rootobjectReactFormRes.ErrorInnerException = await response.Content.ReadAsStringAsync();
                        rootobjectReactFormRes.ErrorMessage = "Error Post";
                    }                  


                }
                catch (Exception excp)
                {
                    rootobjectReactFormRes.ErrorCode = 205;
                    rootobjectReactFormRes.ErrorInnerException = excp.Message;
                    rootobjectReactFormRes.ErrorMessage = "Error Post";

                }
            }
            rootobjectReactFormRes.rootobjectReactForm = null ;


            return rootobjectReactFormRes;



        }

    }
}

     //api.patch("NEO_PREPTASK('PRP2100002')?$expand=NEO_PRESCRIPTION_LBL_SUBFORM",Submit_data,
     //            {method: "PATCH",
     //              headers: {
     //                 'Authorization':  basicAuth,
     //                'Access-Control-Allow-Origin': '*',
     //                "Accept": "application/json",
     //               "Content-type": "application/json"}}
     //          )
     //        }