

//using InfraPriority.Const;
//using System.Collections.Generic;

//namespace  Elogy.InfraPriority.Http
//{
//    public class GuestProfileExternalProxy : BasicAuthorizationBaseProxy
//    {
//        public GuestProfileExternalProxy() : base(ElogyServicesConsts.ElogyServicesAppPath, ElogyServicesConsts.ElogyCustomerResource)
//        {
//        }

//        public GuestProfileExternalProxy(string Username, string Password) : base(ElogyServicesConsts.ElogyServicesAppPath, ElogyServicesConsts.ElogyCustomerResource)
//        {
//            _proxy.UserName = Username;
//            _proxy.Password = Password;
//            _proxy.AuthType = AuthenticationType.Basic;
//        }

       

       

//        public StationDto UpdateStation(StationUpdateSpec station)
//        {
//            return _proxy.Put<StationDto, StationUpdateSpec>(station, ElogyServicesConsts.DmzPutCustomer);
//        }
//        public StationDto AddStation(StationUpdateSpec station)
//        {
//            return _proxy.Post<StationDto, StationUpdateSpec>(station, ElogyServicesConsts.DmzAddCustomer);
//        }

//        public List<StationDto> GetStation(StationSpec spec)
//        {
//            return _proxy.Get<StationDto, StationSpec>(spec, ElogyServicesConsts.DmzGetCustomers);
//        }
       

//    }
//}
