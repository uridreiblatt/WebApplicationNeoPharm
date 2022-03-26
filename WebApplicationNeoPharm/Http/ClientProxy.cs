
//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml.Serialization;
//using System.Collections.Specialized;
//using InfraPriority.Extentions;
//using InfraPriority.Log;

//namespace  Elogy.InfraPriority.Http
//{
//    public class ClientProxy : BaseClientProxy
//    {
//        #region Public proxy parameters

//        public MediaTypeFormatterCollection CustomFormatters { get; internal set; }
//        /// <summary>
//        /// Pass the bearer token not from the context
//        /// </summary>
//        public string BearerToken { get; set; }
//        public Dictionary<string, string> CustomRequestHeaders { get; set; }

//        #endregion

//        private string _baseURLAppConfigKey;
//        protected TimeSpan _cacheTimeoutTimespan = new TimeSpan(0, 0, 60, 0); // cache for 60 min
//        private static object syncRoot = new object();
//        // Alex - Specifies whether to use case insensitive serializer which is slower than the case sensitive serializer
//        // however in some cases (like adapters for example) the data send can be either case, so it's nesessary
//        // A new Optima specific proxy class was added that sets this to false as default
//        protected bool CaseInsensitive = true;

//        #region Constructors

//        public ClientProxy(string baseURLAppConfigKey, string resourceName) : this(resourceName)
//        {
//            if (string.IsNullOrWhiteSpace(baseURLAppConfigKey))
//            {
//                throw new ArgumentException("baseURLAppConfigKey is null or empty", nameof(baseURLAppConfigKey));
//            }

//            _baseURLAppConfigKey = baseURLAppConfigKey;
//            _baseURL = ConfigurationExtensionMethods.ReadConfigSetting(baseURLAppConfigKey); //   ConfigurationManager.AppSettings[baseURLAppConfigKey];

//            if (_baseURL == null)
//            {
//                throw new ApplicationException($"Could not load app settings key: {baseURLAppConfigKey}");

//            }
//        }

//        public ClientProxy(string baseURLAppConfigKey, string resourceName, TimeSpan cacheTimeoutTimespan) : this(baseURLAppConfigKey, resourceName)
//        {
//            _cacheTimeoutTimespan = cacheTimeoutTimespan;
//        }

//        public ClientProxy(Uri baseUrl, string resourceName, TimeSpan cacheTimeoutTimespan) : this(baseUrl, resourceName)
//        {
//            _cacheTimeoutTimespan = cacheTimeoutTimespan;
//        }
//        public ClientProxy(Uri baseUrl, string resourceName) : this(resourceName)
//        {
//            if (baseUrl == null)
//            {
//                throw new ArgumentNullException(nameof(baseUrl));
//            }

//            _baseURL = baseUrl.AbsoluteUri;

//            if (_baseURL.IsNullOrWhiteSpace())
//            {
//                throw new ApplicationException($"baseUrl.AbsoluteUri is null or empty; ");
//            }
//        }


//        private ClientProxy(string resourceName) : this()
//        {
//            _resourceName = resourceName;

//            //This will trust all certificates (even self signed in development IIS)
//            ServicePointManager.ServerCertificateValidationCallback +=
//                (sender, cert, chain, sslPolicyErrors) => true;
//        }

//        protected ClientProxy()
//        {
//        }

//        #endregion
//        // This allows to pass custom headers from the calling controller
//        private static void ApplyCustomHeaders(HttpRequestMessage request, NameValueCollection headers)
//        {
//            List<string> skipRequestHeaders = new List<string> { "Content-Length", "Content-Type" };
//            if (headers != null)
//            {
//                foreach (var key in headers.AllKeys)
//                {
//                    if (skipRequestHeaders.Contains(key))
//                    {
//                        continue;
//                    }
//                    try
//                    {

//                        request.Headers.Add(key, headers[key]);
//                    }
//                    catch (Exception ex)
//                    {
//                        Logger.Handle_Log(Logger.SeverityLevel.Error,ex, $"Failed adding header ({key}, {headers[key]} )");
//                        throw ex;
//                    }

//                }
//            }
//        }

//        /// <summary>
//        /// Get entities by specification
//        /// </summary>
//        /// <typeparam name="TResult"></typeparam>
//        /// <typeparam name="TSpecification"></typeparam>
//        /// <param name="spec"></param>
//        /// <param name="specificResourceMethod"></param>
//        /// <param name="isProtectWithSecretKey">This will add the silverbyte secret key to the request headers. used when aa action is protected by the "ProtectBySecretKey" attribute</param>
//        /// <param name="headers">This allows to pass custom headers from the calling controller</param>
//        /// <returns></returns>
//        public async Task<List<TResult>> Get<TResult, TSpecification>(TSpecification spec, string specificResourceMethod = null, bool isProtectWithSecretKey = false, NameValueCollection headers = null)
//        {
//            List<TResult> result = null;

//            #region Gets base url, adds specific resource if spcified, adds get parameters and executes

//            HttpClient client = GetSingletonHttpClient();

//            string uri = null;
//            if (spec == null)
//            {
//                uri = specificResourceMethod;
//            }
//            else
//            {
//                uri = specificResourceMethod + spec.ToQueryString();
//            }

//            HttpRequestMessage request = CreateRequest(uri, HttpMethod.Get, isProtectWithSecretKey: isProtectWithSecretKey);
//            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//            ApplyCustomHeaders(request, headers);
//            var cs = new CancellationTokenSource();
//            cs.CancelAfter(OverrideProxyTimeoutTimespan); // With throw taskcancellation exception when request took more than 500 ms
//            HttpResponseMessage response = await client.SendAsync(request, cs.Token);

//            #endregion

//            //Exception Handling
//            result = await HandleResponseMessageExceptions<List<TResult>>(response, request);

//            return result;
//        }

//        /// <summary>
//        /// Create entities
//        ///
//        /// Rafi 30.07.19: Post currently works only for complex objects and not primitive types!
//        /// If you need to post a primitive type (int/decimal/string) , please wrap it in a class
//        /// </summary>
//        /// <typeparam name="TResult"></typeparam>
//        /// <typeparam name="TInput"></typeparam>
//        /// <param name="input"></param>
//        /// <param name="specificResourceMethod"></param>
//        /// <param name="postAsXml">Handle object with Xml attr by Sand Client</param>
//        /// <param name="headers">This allows to pass custom headers from the calling controller</param>
//        /// <returns></returns>
//        public virtual async Task<TResult> Post<TResult, TInput>(TInput input, string specificResourceMethod = null, bool postAsXml = false, bool isProtectWithSecretKey = false, NameValueCollection headers = null)
//        {
//            HttpRequestMessage request = null;
//            HttpResponseMessage response = null;
//            TResult result;
//            try
//            {
//                HttpClient client = GetSingletonHttpClient();
//                request = CreateRequest(specificResourceMethod, HttpMethod.Post, isProtectWithSecretKey: isProtectWithSecretKey);
//                ApplyCustomHeaders(request, headers);
//                // IDAN: Now we can add url part to the resourceName without modifying the resourceName
//                SerializeObjectToRequestContent(input, request);
//                var cs = new CancellationTokenSource();
//                cs.CancelAfter(OverrideProxyTimeoutTimespan); // With throw taskcancellation exception when request took more than 500 ms
//                response = await client.SendAsync(request, cs.Token);

//                //Exception Handling
//                result = await HandleResponseMessageExceptions<TResult>(response, request);
//            }
//            catch
//            {
//                throw;
//            }
//            return result;
//        }

//        /// <summary>
//        /// Update entities
//        /// </summary>
//        /// <typeparam name="TResult"></typeparam>
//        /// <typeparam name="TInput"></typeparam>
//        /// <param name="input"></param>
//        /// <param name="specificResourceMethod"></param>
//        /// <param name="headers">This allows to pass custom headers from the calling controller</param>
//        /// <returns></returns>
//        public async Task<TResult> Put<TResult, TInput>(TInput input, string specificResourceMethod = null, bool postAsXml = false, bool isProtectWithSecretKey = false, NameValueCollection headers = null)
//        {
//            HttpClient client = GetSingletonHttpClient();

//            /* IDAN: Changing _resourceName may cause problems if we use the same proxy instance more that once with a not null specificResourceMethod
//            if (specificResourceMethod != null)
//            {
//                _resourceName += specificResourceMethod;
//            }
//            */
//            HttpRequestMessage request = CreateRequest(specificResourceMethod, HttpMethod.Put, isProtectWithSecretKey: isProtectWithSecretKey);
//            ApplyCustomHeaders(request, headers);
//            // IDAN: Now we can add url part to the resourceName without modifying the resourceName

//            SerializeObjectToRequestContent(input, request);

//            var cs = new CancellationTokenSource();
//            cs.CancelAfter(OverrideProxyTimeoutTimespan); // With throw taskcancellation exception when request took more than 500 ms
//            var response = await client.SendAsync(request, cs.Token);

//            //Exception Handling
//            TResult result = await HandleResponseMessageExceptions<TResult>(response, request);

//            return result;
//        }

//        private static void SerializeObjectToRequestContent<TInput>(TInput input, HttpRequestMessage request)
//        {
//            //Rafi - commented on 05.04.2020 due to channel adapters bug

//            if (typeof(TInput) == typeof(System.Object))
//            {
//                //This does not accept type TInput and supports serializing object types
//                string objJson = JsonConvert.SerializeObject(input);
//                request.Content = new StringContent(objJson, Encoding.UTF8, "application/json");
//            }
//            else
//            {
//                //this serializer works fastest with PREDECLARED types
//                // Set this to default case handling with null exclusion
//                var objJson = JsonSerializer.Serialize(input, Resolvers.StandardResolver.ExcludeNull);
//                request.Content = new ByteArrayContent(objJson);
//                request.Content.Headers.Add("Content-Type", "application/json; charset=utf-8");
//            }

//        }

//        /// <summary>
//        /// delete entities
//        /// </summary>
//        /// <typeparam name="TResult"></typeparam>
//        /// <typeparam name="TInput"></typeparam>
//        /// <param name="input"></param>
//        /// <param name="specificResourceMethod"></param>
//        /// <param name="headers">This allows to pass custom headers from the calling controller</param>
//        /// <returns></returns>
//        public async Task<TResult> Delete<TResult, TInput>(TInput input, string specificResourceMethod = null, bool isProtectWithSecretKey = false, NameValueCollection headers = null)
//        {
//            HttpClient client = GetSingletonHttpClient();

//            HttpRequestMessage request = CreateRequest(specificResourceMethod + input.ToQueryString(), HttpMethod.Delete, isProtectWithSecretKey: isProtectWithSecretKey);

//            var cs = new CancellationTokenSource();
//            cs.CancelAfter(OverrideProxyTimeoutTimespan); // With throw taskcancellation exception when request took more than 500 ms
//            HttpResponseMessage response = await client.SendAsync(request, cs.Token);
//            ApplyCustomHeaders(request, headers);
//            //Exception Handling
//            TResult result = await HandleResponseMessageExceptions<TResult>(response, request);

//            return result;
//        }

//        /// <summary>
//        /// If specification exists in cache -> return
//        /// else, get from web api and add to cache
//        /// <param name="headers">This allows to pass custom headers from the calling controller</param>
//        /// </summary>
//        public async Task<List<TResult>> CachedGet<TResult, TSpecification>(TSpecification spec, string specificResourceMethod = null, bool isForceCacheRefresh = false, bool isProtectWithSecretKey = false, NameValueCollection headers = null)
//        {
//            string specificationSerializedToString = CacheProvider.GetCacheKeyBySpecification(spec, typeof(TResult));

//            #region if exists in cache, return from cache

//            if (!isForceCacheRefresh)
//            {
//                object cacheResults = CacheProvider.Instance.GetItem(specificationSerializedToString);
//                if (cacheResults != null)
//                {
//                    //Task<List<TResult>> taskRes = Task.FromResult((List<TResult>)cacheResults);
//                    // return await taskRes;
//                    return (List<TResult>)cacheResults;
//                }
//            }

//            #endregion;

//            #region Else, Get from httpclient and add to cache

//            List<TResult> itemFromService = await Get<TResult, TSpecification>(spec, specificResourceMethod, isProtectWithSecretKey: isProtectWithSecretKey, headers);
//            //Anton, user and chain check removed

//            //Rafi 31-10-18 null values are not supported in the memory cache
//            if (itemFromService != null)
//            {
//                CacheProvider.Instance.SetItem(specificationSerializedToString, itemFromService, new DateTimeOffset(DateTime.Now + _cacheTimeoutTimespan));
//            }
//            return itemFromService;

//            #endregion
//        }

//        #region Private Helpers

//        //use static http client:
//        //https://channel9.msdn.com/Series/aspnetmonsters/ASPNET-Monsters-62-You-are-probably-using-HttpClient-wrong
//        public static HttpClient GetSingletonHttpClient()
//        {
//            if (_httpClient == null)
//            {
//                lock (syncRoot)
//                {
//                    //check again for null after locking
//                    if (_httpClient == null)
//                    {
//                        var handler = new HttpClientHandler()
//                        {
//                            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
//                        };
//                        _httpClient = new HttpClient(handler);
//                        _httpClient.DefaultRequestHeaders.Clear();
//                        //big timeout (1H) per proxy overrides the defautl 100 seconds
//                        //the more meaningful timeout is passed per request using OverrideProxyTimeoutTimespan
//                        _httpClient.Timeout = new TimeSpan(1, 0, 0);
//                    }
//                }
//            }

//            return _httpClient;
//        }

//        private HttpRequestMessage CreateMessageWithHeaders(HttpMethod method, string uri, bool isProtectWithSecretKey = false)
//        {
//            try
//            {
//                var message = new HttpRequestMessage(method, _baseURL + uri);

//                #region pass TraceLogCorrelationID in header (if exists)

//                object traceLogCorrelationID = CallContext.LogicalGetData("TraceLogCorrelationID");
//                if (traceLogCorrelationID != null)
//                {
//                    //pass tracelogcorrelationId in header (Owin on recieving side will use it)
//                    message.Headers.Add("TraceLogCorrelationID", traceLogCorrelationID.ToString());
//                }

//                #endregion

//                #region Pass autherization token in header

//                CurrentUser currUser = CurrentHttpContext.GetCurrentUser();

//                //1) First priority -> If username/password were passed by code to proxy -> this is the first priority! this means the HttpContext user's token will not be used
//                if (UserName != null && Password != null)
//                {
//                    // check the Auth type when passing username or password
//                    if (AuthType == AuthenticationType.Bearer)
//                    {
//                        //pass user/pass in header (for dmz applications calls)
//                        message.Headers.Add("username", UserName);
//                        message.Headers.Add("password", Password);
//                    }
//                    else if (AuthType == AuthenticationType.Basic)
//                    {
//                        message.Headers.Authorization = new AuthenticationHeaderValue("basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}")));
//                    }
//                }
//                else if (!BearerToken.IsNullOrWhiteSpace())
//                {
//                    #region pass bearer token hardcoded to proxy

//                    if (!BearerToken.ToLower().StartsWith("bearer"))
//                    {
//                        message.Headers.Add(AuthConsts.HeaderKeyAuthorization, "Bearer " + BearerToken);
//                    }
//                    else
//                    {
//                        message.Headers.Add(AuthConsts.HeaderKeyAuthorization, BearerToken);
//                    }

//                    #endregion
//                }
//                else if (UnitTestFakeIdentityHttpContext.IsFakeUnitTestIdentityInitialized)
//                {
//                    //Use fake authentication for unit tests
//                    message.Headers.Add(AuthConsts.HeaderKeyAuthorization, "Bearer " + UnitTestFakeIdentityHttpContext.FakeToken);
//                }
//                else if ((currUser != null) && (currUser.OwinToken != null))
//                {
//                    //If used authenticated, Pass the current authorization token in teh header
//                    message.Headers.Add(AuthConsts.HeaderKeyAuthorization, currUser.OwinToken);
//                }
//                else if (HttpContext.Current?.User != null && HttpContext.Current.User is ClaimsPrincipal)
//                {
//                    #region Add header by taking the token from the user claims (token possibly filled by wepapi owin handleheaders - pass user/pass in header)

//                    Claim token = ((ClaimsPrincipal)HttpContext.Current.User).FindFirst(c => c.Type.ToString().ToLower() == SilverbyteClaimTypes.BearerToken.ToLower());
//                    if (token != null)
//                    {
//                        message.Headers.Add(AuthConsts.HeaderKeyAuthorization, "Bearer " + token.Value);
//                    }

//                    #endregion
//                }

//                #endregion

//                #region Protect with secret key header

//                if (isProtectWithSecretKey)
//                {
//                    message.Headers.Add(ElogyServicesConsts.ElogyAdminAuthKey, ElogyServicesConsts.ElogyAdminAuthKeyValue);
//                }

//                #endregion

//#if DEBUG
//                var stack = new System.Diagnostics.StackTrace(5, true).GetFrames().Take(10).ToList();
//                foreach (StackFrame frame in stack)
//                {
//                    Debug.WriteLine(frame.ToString());
//                }
//                //message.Headers.Add("StackTrace", stack.Replace(Environment.NewLine, Environment.NewLine + " ").Substring(0,500));
//#endif

//                if (!CustomRequestHeaders.IsNullOrEmpty())
//                {
//                    foreach (KeyValuePair<string, string> header in CustomRequestHeaders)
//                    {
//                        message.Headers.Remove(header.Key);
//                        message.Headers.Add(header.Key, header.Value);
//                    }
//                }

//                return message;
//            }
//            catch (Exception ex)
//            {
//                Logger.Write(Logger.SeverityLevel.Error, ex,$"Error in ClientProxy.CreateMessageWithHeaders | Exception {ex} | Method {Convert.ToString(method)}  | _baseURL {Convert.ToString(_baseURL)} | uri {Convert.ToString(uri)}");
//                throw;
//            }
//        }

//        private List<MediaTypeFormatter> caseSensitiveFormatters = new List<MediaTypeFormatter> { new Utf8JsonFormatter() };
//        protected async Task<TResult> HandleResponseMessageExceptions<TResult>(HttpResponseMessage response, HttpRequestMessage request)
//        {
//            TResult result;

//            if (response.IsSuccessStatusCode)
//            {
//                try
//                {
//                    if (CustomFormatters != null)
//                    {
//                        result = await response.Content.ReadAsAsync<TResult>(CustomFormatters);
//                    }
//                    else if (CaseInsensitive)
//                    {
//                        //Use the default serializer (usually set in SilverbyteWebApiConfig)
//                        result = await response.Content.ReadAsAsync<TResult>();
//                    }
//                    else
//                    {
//                        //Alex's fast case sensitive formatter
//                        result = await response.Content.ReadAsAsync<TResult>(caseSensitiveFormatters);
//                    }
//                }
//                catch (Exception exp)
//                {
//                    string responseContent = null;
//                    try
//                    {
//                        responseContent = await response.Content.ReadAsStringAsync();
//                    }
//                    catch { }
//                    var exeption = new Exception($"Error reading response content as {typeof(TResult).Name}", exp);
//                    exeption.Data.Add("Response", responseContent);
//                    exeption.Source = "ClientProxy => HandleResponseMessageExceptions";
//                    throw exeption;
//                }
//            }
//            else if (response.StatusCode == HttpStatusCode.BadRequest)
//            {
//                #region bad requests (error code 400) are rethrown as business exceptions

//                throw HandleBusinessExceptions(response);

//                #endregion
//            }
//            else if (response.StatusCode == HttpStatusCode.Unauthorized)
//            {
//                #region Unauthorized requests sign out of owin, then rethrow unauthrozied httpexception 401

//                throw CheckUnAutherized(request, response);

//                #endregion
//            }
//            else
//            {
//                if (response.StatusCode == HttpStatusCode.NotFound)
//                {
//                    Logger.Write(Logger.SeverityLevel.Error,null, $"URL not found! {request.RequestUri}");
//                }

//                #region General system exceptions (usually error 500) -> throw exception
//                string responseContent = null;
//                string requestContent = null;
//                try
//                {
//                    responseContent = response.Content != null ? await response.Content.ReadAsStringAsync() : "";
//                    requestContent = request.Content != null ? await request.Content.ReadAsStringAsync() : "";
//                }
//                catch { }
//                var content = JsonConvert.SerializeObject(responseContent);
//                var exeption = new Exception(responseContent);
//                exeption.Data.Add("Request", requestContent);
//                exeption.Data.Add("Response", responseContent);
//                exeption.Data.Add("Request URI", request.RequestUri);
//                exeption.Source = "ClientProxy => HandleResponseMessageExceptions";
//                throw exeption;
//                #endregion
//            }

//            return result;
//        }
//        protected BusinessException HandleBusinessExceptions(HttpResponseMessage response)
//        {
//            string paramHeader = response.Headers.FirstOrDefault(r => r.Key == BusinessException.paramsHeaderName).Value?.FirstOrDefault();
//            string StringKeyCode = response.Headers.FirstOrDefault(r => r.Key == BusinessException.stringKeyCodeHeaderName).Value?.FirstOrDefault();
//            string StringIsRevocable = response.Headers.FirstOrDefault(r => r.Key == BusinessException.stringIsRevocable).Value?.FirstOrDefault();
//            if (string.IsNullOrWhiteSpace(StringKeyCode))
//            {
//                string errorText = response.Content.ReadAsStringAsync().Result;
//                throw new Exception(errorText);
//            }
//            var error = (EMessages)Enum.Parse(typeof(EMessages), StringKeyCode);
//            JArray optinalParams = null;
//            BusinessException busEx = null;
//            if (!string.IsNullOrEmpty(paramHeader))
//            {
//                optinalParams = JArray.Parse(paramHeader);
//                busEx = new BusinessException(error, optinalParams.ToObject<object[]>());
//            }
//            else if (!string.IsNullOrEmpty(StringIsRevocable))
//            {
//                busEx = new BusinessException(error, bool.Parse(StringIsRevocable));
//            }
//            else
//            {
//                busEx = new BusinessException(error);
//            }
//            return busEx;
//        }
//        protected HttpRequestMessage CreateRequest(string uri, HttpMethod method, bool isProtectWithSecretKey = false)
//        {
//            //Anton, multi hotels in chain view support added , previous version refactored
//            string request = _resourceName;

//            uri = WebApiClientHelper.GetHotelIdFromUrl(uri);

//            request += uri;

//            HttpRequestMessage httpRequestMessage = CreateMessageWithHeaders(method, request, isProtectWithSecretKey);

//            return httpRequestMessage;
//        }
//        protected HttpException CheckUnAutherized(HttpRequestMessage request, HttpResponseMessage response)
//        {

//            if (HttpContext.Current != null && HttpContext.Current.User != null)
//            {
//                IOwinContext owinContext = null;
//                {
//                    try
//                    {
//                        owinContext = HttpContext.Current.GetOwinContext();
//                    }
//                    catch { }
//                }

//                if (owinContext != null)
//                {
//                    //if token expired, sign out to force relogin (renew token)
//                    owinContext.Authentication.SignOut();
//                }
//            }

//            string errorText = $"Unauthorized request URL: {Convert.ToString(_baseURL)}{request}";

//            if (CurrentHttpContext.GetCurrentUser() != null)
//            {
//                errorText += $" CurrentHttpContext.GetCurrentUser().UserName: {CurrentHttpContext.GetCurrentUser().UserName}";
//            }

//            if (UserName != null)
//            {
//                errorText += $" Proxy UserName: {UserName}";
//            }

//            if (response != null && response.ReasonPhrase != null)
//            {
//                errorText += $" ReasonPhrase: {response.ReasonPhrase}";
//            }

//            return new HttpException(401, errorText);
//        }

//        #endregion
//    }


//}