using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.Model
{
   

    public class ClsPrepUsers
    {
        public string odatacontext { get; set; }
        public List<ClsPrepUser> value { get; set; }
    }

    public class ClsPrepUser
    {
        public int USERID { get; set; }
        public string SNAME { get; set; }
        public string PHARMACIST { get; set; }
        public string TECHNICIAN { get; set; }
        public string INACTIVE { get; set; }
        public int USERB { get; set; }
    }

}
