using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.Model
{



    public class ClsPrepTask
    {
        public ClsPrepTask()
        {
            PSNAME = "REQUIRED FROM API";
        }
        public string PSDATE { get; set; }
        public string PSNAME { get; set; }
        public string odatacontext { get; set; }
        public string PREPTASKNUM { get; set; }
        public string SERIALNAME { get; set; }
        public string PARTNAME { get; set; }
        public string PARTDES { get; set; }
        public string CODE { get; set; }
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
        public string STATDES { get; set; }
        public decimal USERID { get; set; }
        public string SNAME { get; set; }
        public string INITIAL { get; set; }
        public string RECEPTACLECODE { get; set; }
        public string RECEPTACLEDES { get; set; }
        public decimal PREPTASK { get; set; }
        public NEO_PRESCRIPTION_LBL_SUBFORM NEO_PRESCRIPTION_LBL_SUBFORM { get; set; }
    }

    public class NEO_PRESCRIPTION_LBL_SUBFORM
    {
        public decimal NA { get; set; }
        public decimal K { get; set; }
        public decimal CA { get; set; }
        public decimal P { get; set; }
        public decimal MG { get; set; }
        public decimal CL { get; set; }
        public decimal ZN { get; set; }
        public decimal CU { get; set; }
        public decimal MN { get; set; }
        public decimal CR { get; set; }
        public decimal SE { get; set; }
        public decimal FE { get; set; }
        public decimal I { get; set; }
        public decimal MO { get; set; }
        public decimal F { get; set; }
        public string VITAMIN { get; set; }
        public decimal VIT_QUANT { get; set; }
        public string VIT_UNITNAME { get; set; }
        public string DRUG { get; set; }
        public decimal DRUG_QUANT { get; set; }
        public string DRUG_UNITNAME { get; set; }
        public string VEINDES { get; set; }
        public decimal TOT_VOL { get; set; }
        public decimal DEX { get; set; }
        public decimal TOTMLTPN { get; set; }
        public decimal AMINO { get; set; }
        public decimal FAT { get; set; }
        public decimal ACET { get; set; }
        public string GIVMETHDES { get; set; }
        public string GIV_METH { get; set; }
        public decimal DRIP_RATE { get; set; }
        public decimal NEO_TIMECODE { get; set; }
        public string NEO_TIME { get; set; }
        public string NEO_TIME_TEXT { get; set; }
        public decimal MQUANT { get; set; }
        public string MUMAS { get; set; }
        public decimal MMQUANT { get; set; }
        public string MMQUANT_UNITNAME { get; set; }
        public float MUMASML { get; set; }
        public string MUMAS2 { get; set; }
        public decimal MMQUANT2 { get; set; }
        public string MMQUANT_UNITNAME2 { get; set; }
        public decimal MUMASML2 { get; set; }
        public string MUMAS3 { get; set; }
        public decimal MMQUANT3 { get; set; }
        public string MMQUANT_UNITNAME3 { get; set; }
        public decimal MUMASML3 { get; set; }
        public decimal QUANTPERCENT { get; set; }
        public decimal DILUTION { get; set; }
        public string VIT1 { get; set; }
        public string VIT_QUANTNAME1 { get; set; }
        public string VIT2 { get; set; }
        public string VIT_QUANTNAME2 { get; set; }
        public string VIT3 { get; set; }
        public string VIT_QUANTNAME3 { get; set; }
        public string MEMIS { get; set; }
        public string STORAGE { get; set; }
        public float TOTALMUMASML { get; set; }
        public float NEOTOMEMMIS { get; set; }
        public string STORAGE_B { get; set; }
        public string MANUAL { get; set; }
    }


}
