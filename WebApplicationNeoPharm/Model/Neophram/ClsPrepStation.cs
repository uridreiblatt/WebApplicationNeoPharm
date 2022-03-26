using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.Model
{


    public class ClsPrepStation
    {
        public string odatacontext { get; set; }
        public List< STATIONPREP_SUBFORM> value { get; set; }
    }

    public class STATIONPREP_SUBFORM
    {
        public string CODE { get; set; }
        public string DES { get; set; }
        public List< NEO_STATIONPREP_SUBFORM> NEO_STATIONPREP_SUBFORM { get; set; }
    }

    public class NEO_STATIONPREP_SUBFORM
    {
        public string WORKSTATIONNAME { get; set; }
        public string WORKSTATIONDES { get; set; }
        public int WORKSTATIONTYPE { get; set; }
    }

}
