
//using InfraPriority.Extentions;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;



//namespace  Elogy.InfraPriority.Http
//{

    
//    public abstract class BaseClientProxy
//    {
//        public BaseClientProxy()
//        {

//        }

//        #region Public Properties

//        public string UserName { get; set; }
//        public string Password { get; set; }
//        /// <summary>
//        /// Pass the authentication header value manually.
//        /// for example bearer token or basic encoded string.
//        /// </summary>
//        public string AuthenticationHeaderValue { get; set; }
//        /// <summary>
//        /// Allows multiple auth types
//        /// Defualt is the bearer token type
//        /// </summary>
//        public AuthenticationType AuthType = AuthenticationType.Bearer;

//#if DEBUG
//        //when debugging set long timeout to allow debugging
//        public TimeSpan OverrideProxyTimeoutTimespan
//        {
//            get;
//            set;
//        } = new TimeSpan(1, 0, 0); // 1 hour
//#else
//        public TimeSpan OverrideProxyTimeoutTimespan { get; set; } = new TimeSpan(0, 3, 0); // 180 seconds default
//#endif

//        public static bool IsIgnoreInvalidSSL { get; set; }

//        #endregion

//        #region Private Properties

//        private static object syncRoot = new object();

//        #endregion

//        #region Protected Properties

//        //why use static http client:
//        //https://channel9.msdn.com/Series/aspnetmonsters/ASPNET-Monsters-62-You-are-probably-using-HttpClient-wrong
//        protected static HttpClient _httpClient;
//        protected string _baseURL;
//        protected string _resourceName;
//        protected const string ForwardSlash = "/";

//        #endregion

//        #region Constructors

//        public BaseClientProxy(string baseUrl, string resourceName) : this(resourceName)
//        {
//            if (baseUrl.IsNullOrWhiteSpace())
//            {
//                throw new ArgumentException("baseUrl Is Null", nameof(baseUrl));
//            }

//            try
//            {
//                var baseUri = new Uri(baseUrl);
//                _baseURL = baseUri.AbsoluteUri;
//            }
//            catch (Exception exp)
//            {
//                throw new Exception("BaseClientProxy => baseUrl is not a valid URI;", exp);
//            }
//        }
//        public BaseClientProxy(Uri baseUrl, string resourceName) : this(resourceName)
//        {
//            if (baseUrl == null)
//            {
//                throw new ArgumentNullException(nameof(baseUrl));
//            }

//            _baseURL = baseUrl.AbsoluteUri;
//        }
//        private BaseClientProxy(string resourceName)
//        {
//            _resourceName = resourceName;

//#if DEBUG
//            OverrideProxyTimeoutTimespan = new TimeSpan(1, 0, 0);
//#else
//            OverrideProxyTimeoutTimespan = new TimeSpan(0, 3, 0);
//#endif
//        }

//        #endregion

//        #region Private Methods

//        private HttpRequestMessage CreateMessageWithHeaders<TInput>(HttpMethod method, string action, TInput objJson = null, bool isProtectWithSecretKey = false) where TInput : class
//        {
//            action = action.StartsWith("/") ? action : $"/{action}";
//            var requestMsg = new HttpRequestMessage(method, _baseURL + _resourceName + action);

//            //ICurrentContext context = GetCurrentContext();
//            //IUser currentContextUser = context.GetContextUser();

//            if (AuthType == AuthenticationType.Basic)
//            {
//                string headerValue = null;
//                if (!AuthenticationHeaderValue.IsNullOrWhiteSpace())
//                {
//                    headerValue = AuthenticationHeaderValue;
//                }
//                else if (!UserName.IsNullOrWhiteSpace() && !Password.IsNullOrWhiteSpace())
//                {
//                    headerValue = $"{UserName}:{Password}".ToBase64String();
//                }

//                if (!headerValue.IsNullOrWhiteSpace())
//                {
//                    requestMsg.Headers.Authorization = new AuthenticationHeaderValue(AuthType.ToString(), AuthenticationHeaderValue);
//                }
//            }
//            else if (!AuthenticationHeaderValue.IsNullOrWhiteSpace() && AuthType == AuthenticationType.Bearer)
//            {
//                requestMsg.Headers.Authorization = new AuthenticationHeaderValue(AuthType.ToString(), AuthenticationHeaderValue);
//            }
//            //else if (currentContextUser != null)
//            //{
//            //    //InsertCurrentUserCredentialsIntoMessageHeaders
//            //    HandleCurrentContextUserAuthenticationHeader(requestMsg, currentContextUser);
//            //}
//            else
//            {
//                // those fields are authenticated in the owin pipe
//                requestMsg.Headers.Add("username", UserName);
//                requestMsg.Headers.Add("password", Password);
//            }

//            if (isProtectWithSecretKey)
//            {
//                requestMsg.Headers.Add(ElogyServicesConsts.ElogyAdminAuthKey, ElogyServicesConsts.ElogyAdminAuthKeyValue);
//            }

//            requestMsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

//            if (method == HttpMethod.Post || method == HttpMethod.Put)
//            {
//                requestMsg.Content = new StringContent(objJson.ToString(), Encoding.UTF8, "application/json");
//            }

//            return requestMsg;
//        }
//        private async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpRequestMessage request)
//        {
//            var cs = new CancellationTokenSource();
//            cs.CancelAfter(OverrideProxyTimeoutTimespan); // With throw taskcancellation exception when request took more than 500 ms
//            HttpResponseMessage response = await client.SendAsync(request, cs.Token);
//            return response;
//        }

//        #endregion

//        #region Protected Methods

        
        
//        protected HttpResponseException CheckUnAutherized(HttpRequestMessage request, HttpResponseMessage response)
//        {
//            //ICurrentContext currentConext = GetCurrentContext();
//            string errorText = $"Unauthorized request URL: {Convert.ToString(_baseURL)}{request}";

//            //if (currentConext.GetContextUser() != null)
//            //{
//            //    errorText += $" currentConext.GetContextUser(): {currentConext.GetContextUser().UserName}";
//            //}

//            if (UserName != null)
//            {
//                errorText += $" Proxy UserName: {UserName}";
//            }

//            var errorResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized)
//            {
//                ReasonPhrase = response?.ReasonPhrase,
//                Content = new StringContent(errorText)
//            };
//            return new HttpResponseException(errorResponse);
//        }

//        protected async Task<TResult> HandleResponseMessageExceptions<TResult>(HttpResponseMessage response, HttpRequestMessage request)
//        {
//            TResult result;

//            if (response.IsSuccessStatusCode)
//            {
//                result = await response.Content.ReadAsStringAsync<TResult>();
//            }
//            else if (response.StatusCode == HttpStatusCode.Unauthorized)
//            {
//                throw CheckUnAutherized(request, response);
//            }
//            else
//            {
//                #region General system exceptions (usually error 500) -> throw exception

//                throw new Exception($"Error invoking request {_baseURL}{request}. Status code: {response.StatusCode}. Response content: {await response.Content.ReadAsStringAsync()}");

//                #endregion
//            }
//            return result;
//        }

//        #endregion

//        #region Public Methods

//        public virtual async Task<List<TResult>> Get<TResult, TSpec>(TSpec spec, string specificResourceMethod = null, bool isProtectWithSecretKey = false) where TSpec : class
//        {

//            HttpClient client = GetSingletonHttpClient();

//            string actionUri = null;
//            if (spec == null)
//            {
//                actionUri = specificResourceMethod;
//            }
//            else
//            {
//                //actionUri = specificResourceMethod + spec.ToQueryString();
//            }

//            HttpRequestMessage request = CreateMessageWithHeaders<object>(HttpMethod.Get, actionUri, objJson: null, isProtectWithSecretKey: isProtectWithSecretKey);
//            HttpResponseMessage response = await SendRequest(client, request);

//            List<TResult> result = await HandleResponseMessageExceptions<List<TResult>>(response, request);

//            return result;
//        }
//        public virtual async Task<TResult> Post<TResult, TInput>(TInput input, string specificResourceMethod = null, bool isProtectWithSecretKey = false) where TInput : class
//        {
//            HttpClient client = GetSingletonHttpClient();

//            HttpRequestMessage request = CreateMessageWithHeaders(HttpMethod.Post, specificResourceMethod, input, isProtectWithSecretKey: isProtectWithSecretKey);
//            HttpResponseMessage response = await SendRequest(client, request);

//            TResult result = await HandleResponseMessageExceptions<TResult>(response, request);

//            return result;
//        }
//        //why use static http client:
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

//                        if (IsIgnoreInvalidSSL)
//                        {
//                            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
//                        }

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

//        #endregion

//        //#region Abstract Protected Methods

//        ////protected abstract ICurrentContext GetCurrentContext();
//        ////protected abstract void HandleCurrentContextUserAuthenticationHeader(HttpRequestMessage message, IUser currentContextUser);
//        ////protected abstract bool IsHttpContextHasIdentity();

//        //#endregion
//    }

//    public enum AuthenticationType
//    {
//        Bearer = 0,
//        Basic = 1
//    }
//}
