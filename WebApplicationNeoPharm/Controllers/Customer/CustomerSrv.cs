using System;


namespace WebApplicationNeoPharm.Controllers
{
    public class CustomerSrv
    {
        public readonly string url = "https://ngpapi.neopharmgroup.com/odata/Priority/tabula.ini/eld0999/NEO_APICUST?$filter=( PHONE2 eq '053-8778799' or PHONENUM eq '053-8778799' )&$top=1";
        

        public void getCustomer ()
        {
            throw new NotImplementedException();
        }
    }
}
