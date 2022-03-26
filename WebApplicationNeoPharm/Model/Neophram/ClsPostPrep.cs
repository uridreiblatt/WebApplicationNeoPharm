using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.Model

{
    

    public class ClsPostPrep
    {

            
        public ClsPostPrep()
        {
            NEO_PREPADDEQUIP_SUBFORM = new List<NEO_PREPADDEQUIP_SUBFORM>();
        }
        public string MONITORING { get; set; }
        public string NORMALLABEL { get; set; }
        public string STABILITY { get; set; }
        public string FLOWRATE { get; set; }
        public string WORKSTATIONNAME { get; set; }
        public string WORKSTATIONDES { get; set; }
        public decimal WEIGHINGCONTROL { get; set; }
        public decimal UNITS { get; set; }
        public string SIGN { get; set; }
        public decimal USERID2 { get; set; }
        public string SNAME2 { get; set; }
        public decimal USERID { get; set; }
        public string SNAME { get; set; }
        public string INITIAL { get; set; }
        public List<NEO_PREPADDEQUIP_SUBFORM> NEO_PREPADDEQUIP_SUBFORM { get; set; }
    }

    public class NEO_PREPADDEQUIP_SUBFORM
    {
        public NEO_PREPADDEQUIP_SUBFORM(string partName)
        {
            PARTNAME = partName;
        }
        public string PARTNAME { get; set; }
    }

}
