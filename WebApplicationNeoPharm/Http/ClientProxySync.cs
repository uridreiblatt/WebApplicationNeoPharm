
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Web;
//using System.Threading.Tasks;


//namespace  Elogy.InfraPriority.Http
//{
//    public class ClientProxySync : ClientProxy
//    {
//        #region Constructors

//        public ClientProxySync(string baseURLAppConfigKey, string resourceName) : base(baseURLAppConfigKey, resourceName)
//        {
//        }

//        public ClientProxySync(string baseURLAppConfigKey, string resourceName, TimeSpan cacheTimeoutTimespan) : base(baseURLAppConfigKey, resourceName, cacheTimeoutTimespan)
//        {
//        }

//        public ClientProxySync(Uri baseUrl, string resourceName) : base(baseUrl, resourceName)
//        {
//        }

//        public ClientProxySync(Uri baseUrl, string resourceName, TimeSpan cacheTimeoutTimespan) : base(baseUrl, resourceName, cacheTimeoutTimespan)
//        {
//        }

//        #endregion

//        public List<TResult> Get<TResult, TSpecification>(TSpecification spec, string specificResourceMethod = null)
//        {
//            List<TResult> result = null;

//            #region Gets base url, adds specific resource if spcified, adds get parameters and executes

//            HttpClient client = GetSingletonHttpClient();

//            string uri = null;
//            if (spec == null)
//            {
//                uri = specificResourceMethod;
//            }
//            //else
//            //{
//            //    uri = specificResourceMethod + spec..ToQueryString();
//            //}


//            HttpRequestMessage request = CreateRequest(uri, HttpMethod.Get);
//            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            HttpResponseMessage response = client.SendAsync(request).Result;

//            #endregion

//            #region Get result or handle exception

//            try
//            {
//                result = HandleResponseMessageExceptions<List<TResult>>(response, request).Result;
//            }
//            catch (AggregateException e)
//            {
//                // The task throws exceptions as AggregateException so this "hides" the businessExceptions
//                // This part gets the inner exception from the AggregateException and rethrows it as it was meant to be
//                if (e.InnerExceptions.Count == 1)
//                {
//                    throw e.InnerException;
//                }
//                throw;
//            }

//            #endregion

//            return result;
//        }
//        public TResult Post<TResult, TInput>(TInput input, string specificResourceMethod = null, bool postAsXml = false)
//        {
//            HttpClient client = GetSingletonHttpClient();

//            HttpRequestMessage request = CreateRequest(specificResourceMethod, HttpMethod.Post);

//            // IDAN: Now we can add url part to the resourceName without modifying the resourceName
//            HttpResponseMessage response;
//            string objJson = JsonSerializer.SerializeObject(input);
//            request.Content = new StringContent(objJson, Encoding.UTF8, "application/json");

//            response = client.SendAsync(request).Result;

//            //Exception Handling
//            try
//            {
//                TResult result = HandleResponseMessageExceptions<TResult>(response, request).Result;
//                return result;
//            }
//            catch (AggregateException e)
//            {
//                // The task throws exceptions as AggregateException so this "hides" the businessExceptions
//                // This part gets the inner exception from the AggregateException and rethrows it as it was meant to be
//                if (e.InnerExceptions.Count == 1)
//                {
//                    throw e.InnerException;
//                }
//                throw;
//            }
//        }
//        public TResult Put<TResult, TInput>(TInput input, string specificResourceMethod = null, bool postAsXml = false)
//        {
//            HttpClient client = GetSingletonHttpClient();

//            HttpRequestMessage request = CreateRequest(specificResourceMethod, HttpMethod.Put);

//            HttpResponseMessage response;

//            string objJson = JsonConvert.SerializeObject(input);
//            request.Content = new StringContent(objJson, Encoding.UTF8, "application/json");
//            response = client.SendAsync(request).Result;

//            //Exception Handling
//            try
//            {
//                TResult result = HandleResponseMessageExceptions<TResult>(response, request).Result;
//                return result;
//            }
//            catch (AggregateException e)
//            {
//                // The task throws exceptions as AggregateException so this "hides" the businessExceptions
//                // This part gets the inner exception from the AggregateException and rethrows it as it was meant to be
//                if (e.InnerExceptions.Count == 1)
//                {
//                    throw e.InnerException;
//                }
//                throw;
//            }
//        }
//        public TResult Delete<TResult, TInput>(TInput input, string specificResourceMethod = null)
//        {
//            HttpClient client = GetSingletonHttpClient();

//            HttpRequestMessage request = CreateRequest(specificResourceMethod + input.ToQueryString(), HttpMethod.Delete);

//            HttpResponseMessage response = client.SendAsync(request).Result;

//            try
//            {
//                //Exception Handling
//                TResult result = HandleResponseMessageExceptions<TResult>(response, request).Result;
//                return result;
//            }
//            catch (AggregateException e)
//            {
//                // The task throws exceptions as AggregateException so this "hides" the businessExceptions
//                // This part gets the inner exception from the AggregateException and rethrows it as it was meant to be
//                if (e.InnerExceptions.Count == 1)
//                {
//                    throw e.InnerException;
//                }
//                throw;
//            }
//        }
//    }
//}

