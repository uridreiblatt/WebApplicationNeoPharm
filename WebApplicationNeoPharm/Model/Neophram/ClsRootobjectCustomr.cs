using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.Model
{

    public class ClsRootobjectCustomr 
    {

        public string odatacontext { get; set; }

        public ValueCustomer[] value { get; set; }

    }


   

    public class ValueCustomer
    {
        


        public string CUSTNAME { get; set; }
        public string CUSTDES { get; set; }
        public string BRANCHNAME { get; set; }
        public string BRANCHDES { get; set; }
        public string WTAXNUM { get; set; }
        public string ADDRESS { get; set; }
        public string STATEA { get; set; }
        public object ZIP { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public object PHONE2 { get; set; }
        public string NAME { get; set; }
        public object INACTIVEFLAG { get; set; }
        public string PHONENUM { get; set; }
        public object CELLPHONE { get; set; }
        public object CINACTIVEFLAG { get; set; }
        public string STATDES { get; set; }
        public string USERLOGIN { get; set; }
        public int CUST { get; set; }
    }
}
