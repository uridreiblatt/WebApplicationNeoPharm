using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplicationNeoPharm.MessageHandlers
{
    public class APIKeyMessageHandler : DelegatingHandler
    {
        private readonly string APIKeyToChek = "d3468ca2-c0cd-4a39-947d-94f77bf205c5"; // ConfigurationManager.AppSettings["x-api-key"].ToString();

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="httpRequestMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        //{
        //    bool validKey = false;
        //    IEnumerable<string> requestHeaders;
        //    var checkApiExists = httpRequestMessage.Headers.TryGetValues("APIKey", out requestHeaders);
        //    if (!checkApiExists) checkApiExists = httpRequestMessage.Headers.TryGetValues("x-api-key", out requestHeaders);
        //    if (checkApiExists)
        //    {
        //        requestHeaders = httpRequestMessage.Headers.GetValues("");              
        //        if (requestHeaders.FirstOrDefault().Equals(APIKeyToChek))
        //        {
        //            validKey = true;
        //        }
        //    }
        //    if (!validKey && httpRequestMessage.Method == HttpMethod.Get) validKey = true;
        //    if (!validKey)
        //    {
        //        return httpRequestMessage.CreateResponse(System.Net.HttpStatusCode.Forbidden, "Invalid API Key");
                

        //    }

        //    var response = await base.SendAsync(httpRequestMessage, cancellationToken);
        //    return response;
        //}
    }
}
