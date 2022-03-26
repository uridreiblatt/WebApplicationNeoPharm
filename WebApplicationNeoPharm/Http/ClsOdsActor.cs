//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace  Elogy.InfraPriority.Http
//{

//    public class AuditActionSpecification
//    {
//        public string ActionName { get; set; }
//        public string Request { get; set; }
//        public string  currUser { get; set; }
//        public string Source { get; set; }
//    }

//    public class MarketplaceActor<TRequest, TResponse>
//    {
//        //private LogsExternalProxy logProxy = new LogsExternalProxy();

//        private Func<TRequest, TResponse> job;
//        public delegate void After(object sender, TResponse e);
//        public MarketplaceActor(Func<TRequest, TResponse> f)
//        {
//            job = f;
//        }

//        public virtual async Task<TResponse> Act(string actionName, TRequest t, After a)
//        {
//            await AuditAction(actionName, t);
//            TResponse v = job(t);

//            a?.Invoke(this, v);

//            // Update the DB with the response in the future
//            return v;
//        }

//        public virtual async Task<TResponse> Act(string actionName, TRequest t)
//        {
//            await AuditAction(actionName, t);
//            return job(t);
//        }

//        private async Task AuditAction(string actionName, TRequest t)
//        {
//            try
//            {
//                //CurrentContextUser currUser = CurrentMongoHttpContext.GetCurrentUser();
//                //var Source = MongoLogger.GetHostingEnvironment();
//                var spec = new AuditActionSpecification { ActionName = actionName, Request = t?.ToJson(), currUser = currUser, Source = Source };
//                //await logProxy.AuditAction(spec);
//            }
//            catch (Exception ex)
//            {
//                // In case of exception do not fail the transaction. Just log and continue.
//                //Logger.TechnicalError(ex);
//            }
//        }
//    }
//}
