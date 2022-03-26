using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationNeoPharm.Const
{
    public class NeoPharmServicesConsts
    {

        #region Key

        public const string ElogyAdminAuthKey = "admin";
        public const string ElogyAdminAuthKeyValue = "sdfsdljfsdlkjf^kjkjhgjgh";

        #endregion


        #region Clean Room

        public const string CleanRoomPrep = "NEO_PREPTASK('PRP2100002')?$expand=NEO_PRESCRIPTION_LBL_SUBFORM";
        public const string CleanRoomStation = "NEO_CURTYPE?$select=CODE,DES&$expand=NEO_STATIONPREP_SUBFORM";
        public const string CleanRoomEquipment = "NEO_ADDEQUIPMENT";

        public const int FieldsInPage = 6;

        #endregion



        #region Injection text

        public const string Injection1 =  " שקית: בוצע ערבוב של השקית על פי הנדרש בנוהל HC-PP-036. בוצעה בדיקה ויזואלית לתוכן השקית שנמצא תקין לשימוש מבחינת צבע,משקעים, אוויר והומוגניות .            ·          אינפיוזר: בוצע Prime לאינפיוזר. בוצעה בדיקה ויזואלית לתוכן האינפיוזרשנמצא תקין לשימוש מבחינת צבע, משקעים, אוויר והומוגניות. בוצע מאזן כמות תרופה. ·          מזרק: בוצעה בדיקה ויזואלית לתוכן המזרק שנמצא תקין לשימוש מבחינת צבע,משקעים, אוויר והומוגניות.  בוצע מאזן כמות תרופה.";






        #endregion







        #region Guests
        public const string DmzPutCustomer = "api/ExternalGuest";
        public const string DmzAddCustomer = "PutGuest";
        public const string DmzGetCustomers = "api/DmzGuest";



        #endregion

        
     
    


    }
}
