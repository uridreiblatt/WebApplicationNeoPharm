using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.Model
{
    

    public class ClsPrepEquiment
    {
        public string odatacontext { get; set; }
        public List<EquipmentValue> value { get; set; }
    }

    public class EquipmentValue
    {
        public string PARTNAME { get; set; }
        public string PARTDES { get; set; }
        public object INACTIVE { get; set; }
        public int PART { get; set; }
    }

}
