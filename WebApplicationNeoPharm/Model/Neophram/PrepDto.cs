
using WebApplicationNeoPharm.React;
using System;
using System.Collections.Generic;
using System.Reflection;
using WebApplicationNeoPharm.Const;
using WebApplicationNeoPharm.Utils;

namespace WebApplicationNeoPharm.Model
{
    public class PrepDto
    {


        public RootobjectReactFormRes BuildForm(ClsPrepTask prep, ClsPrepStation stat, ClsPrepEquiment eqpmnt, ClsPrepUsers clsPrepUser)
        {

            RootobjectReactFormRes rootobjectReactFormRes = new RootobjectReactFormRes();


            #region Validate
            //add buisness error
            if (prep == null)
            {
                return null;
            }
            //if (stat == null)
            //{
            //    return null;
            //}
            if (eqpmnt == null)
            {
                return null;
            }


            #endregion
            rootobjectReactFormRes.rootobjectReactForm.app_label = "טופס הכנה אנטיביוטיקה";
            rootobjectReactFormRes.rootobjectReactForm.app_url = "Add pdf link";

            rootobjectReactFormRes.rootobjectReactForm.pages = BuildPage(prep, stat, eqpmnt, clsPrepUser);

            return rootobjectReactFormRes;

        }
        private List<Page> BuildPage(ClsPrepTask prep, ClsPrepStation stations, ClsPrepEquiment eqpmnts, ClsPrepUsers clsPrepUsers)

        {
            List<Page> pages = new List<Page>();
            Page page = new Page();
            Field f;
            Field_Options field_Options;
            int pagePropertyCount = 0;
            int pageCount = 1;
            page.Page_label = "Stage  " + pageCount.ToString();
            Type t = prep.GetType();
            string sCode = "";
            bool addField = true;

            pagePropertyCount = 0;
            foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                addField = true;

                f = new Field();
                f.field_id = p.Name;
                f.field_label = "Na";
                if (!p.Name.Equals("odatacontext"))
                {
                    f.field_value = p.GetValue(prep)?.ToString();
                }
                f.field_placeholder = "Na";
                f.field_type = "text";
                f.field_disable = false;
                f.field_mandatory = "yes";

                switch (p.Name)
                {
                    case "PREPTASKNUM":

                        f.field_disable = true;
                        f.field_label = "מספר תעודת הכנה";
                        break;

                    case "SERIALNAME":
                        f.field_disable = true;
                        f.field_label = "פק\"ע";
                        break;

                    case "PARTNAME":
                        f.field_disable = true;
                        f.field_label = "מק\"ט";
                        break;
                    case "PSDATE":
                        f.field_disable = true;
                        f.field_label = "תאריך הכנה";
                        page.fields.Add(f);
                        pagePropertyCount += 1;

                        page.fields.Add(BuildHeaderTable(prep));
                        pagePropertyCount += 1;
                        page.fields.Add(BuildSubHeaderTable(prep));
                        pagePropertyCount += 1;
                        page.fields.Add(BuildtextInjection(NeoPharmServicesConsts.Injection1));
                        pagePropertyCount += 1;
                        addField = false;
                        break;
                    case "CODE":
                        f.field_disable = true;
                        f.field_label = "סוג בית מרקחת";
                        // handle station by code
                        sCode = f.field_value;
                        break;
                    case "MONITORING":
                        f.field_label = "בדיקת צלחת ניטור";
                        f.field_mandatory = "yes";
                        f.field_type = "checkbox";
                        break;
                    case "NORMALLABEL":
                        f.field_label = "פרטי מדבקה תקינים";
                        f.field_mandatory = "yes";
                        f.field_type = "checkbox";
                        break;
                    case "STABILITY":
                        f.field_label = "יציבות תקינה";
                        f.field_mandatory = "yes";
                        f.field_type = "checkbox";
                        break;

                    case "FLOWRATE":
                        f.field_label = "קצב זרימה";
                        f.field_mandatory = "yes";
                        f.field_type = "checkbox";
                        break;
                    case "WORKSTATIONNAME":
                        f.field_label = "מספר מנדף";
                        f.field_mandatory = "yes";
                        f.field_type = "select";
                        if (stations != null)
                            foreach (STATIONPREP_SUBFORM station in stations.value)
                            {
                                if (station.CODE.Equals(sCode))
                                {
                                    foreach (NEO_STATIONPREP_SUBFORM station_1 in station.NEO_STATIONPREP_SUBFORM)
                                    {
                                        field_Options = new Field_Options();
                                        field_Options.label = station_1.WORKSTATIONNAME;
                                        field_Options.value = station_1.WORKSTATIONTYPE.ToString();
                                        f.field_options.Add(field_Options);
                                    }
                                }

                            }

                        break;
                    case "WEIGHINGCONTROL":
                        f.field_label = "בקרת שקילה";
                        f.field_mandatory = "yes";
                        break;
                    case "UNITS":
                        f.field_label = "מס יחידות תקינות";
                        f.field_mandatory = "yes";
                        break;
                    case "SIGN":
                        f.field_label = "חתימת טכנאי";
                        f.field_mandatory = "yes";
                        f.field_type = "checkbox";
                        break;
                    //case "USERID2":
                    //    f.field_label = "מספר טכנאי";
                    //    f.field_mandatory = "yes";
                    //    f.field_type = "select";
                    //    foreach (ClsPrepUser  clsPrepUser in clsPrepUsers.value)
                    //    {

                    //        if (!String.IsNullOrEmpty(clsPrepUser.TECHNICIAN) && clsPrepUser.TECHNICIAN.Equals("Y"))
                    //            {
                    //            field_Options = new Field_Options();
                    //            field_Options.option_label = clsPrepUser.USERID.ToString();

                    //            f.field_options.Add(field_Options);
                    //        }

                    //    }
                    //    break;
                    case "SNAME2":
                        f.field_label = "שם טכנאי";
                        f.field_disable = false;
                        f.field_type = "select";
                        foreach (ClsPrepUser clsPrepUser in clsPrepUsers.value)
                        {
                            if (!String.IsNullOrEmpty(clsPrepUser.TECHNICIAN) && clsPrepUser.TECHNICIAN.Equals("Y"))
                            {
                                field_Options = new Field_Options();
                                field_Options.label = clsPrepUser.SNAME;
                                field_Options.value = clsPrepUser.USERID.ToString();

                                f.field_options.Add(field_Options);
                            }

                        }
                        break;
                    case "STATDES":
                        f.field_label = "סטאטוס";
                        f.field_mandatory = "yes";
                        field_Options = new Field_Options();
                        field_Options.label = "CLOSED";
                        f.field_options.Add(field_Options);
                        field_Options = new Field_Options();
                        field_Options.label = "ERROR";
                        f.field_options.Add(field_Options);
                        break;
                    //case "USERID":
                    //    f.field_label = "מספר רוקח";
                    //    f.field_mandatory = "yes";
                    //    f.field_type = "select";
                    //    foreach (ClsPrepUser clsPrepUser in clsPrepUsers.value)
                    //    {
                    //        if (!String.IsNullOrEmpty(clsPrepUser.PHARMACIST) && clsPrepUser.PHARMACIST.Equals("Y"))
                    //        {
                    //            field_Options = new Field_Options();
                    //            field_Options.option_label = clsPrepUser.USERID.ToString();

                    //            f.field_options.Add(field_Options);
                    //        }

                    //    }
                    //    break;
                    case "SNAME":
                        f.field_label = "שם רוקח";
                        f.field_disable = false;
                        f.field_mandatory = "yes";
                        f.field_type = "select";
                        foreach (ClsPrepUser clsPrepUser in clsPrepUsers.value)
                        {
                            if (!String.IsNullOrEmpty(clsPrepUser.PHARMACIST) && clsPrepUser.PHARMACIST.Equals("Y"))
                            {
                                field_Options = new Field_Options();
                                field_Options.label = clsPrepUser.SNAME;
                                field_Options.value = clsPrepUser.USERID.ToString();

                                f.field_options.Add(field_Options);
                            }

                        }
                        break;
                    case "INITIAL":
                        f.field_label = "התחלתי";
                        f.field_disable = true;
                        f.field_type = "checkbox";
                        break;
                    //case "RECEPTACLECODE":
                    //    f.field_label = "RECEPTACLECODE";
                    //    f.field_mandatory = "yes";
                    //    break;
                    //case "RECEPTACLEDES":
                    //    f.field_label = "RECEPTACLEDES";
                    //    f.field_mandatory = "yes";
                    //    break;
                    //case "PREPTASK":
                    //    f.field_label = "PREPTASK";
                    //    f.field_mandatory = "yes";
                    //    break;

                    default:
                        Console.WriteLine($"field value is {p.Name}.");
                        addField = false;
                        break;
                }
                if (addField)
                {
                    page.fields.Add(f);
                    pagePropertyCount += 1;
                }
                if (pagePropertyCount > NeoPharmServicesConsts.FieldsInPage)
                {
                    pageCount += 1;
                    pages.Add(page);
                    page = new Page();
                    page.Page_label = "Perp Page label " + pageCount.ToString();
                    pagePropertyCount = 0;


                }
            }

            //handle last page & aquipment 
            f = new Field();
            f.field_id = "equipment";
            f.field_label = "ציוד נלווה";
            f.field_placeholder = f.field_label;
            f.field_type = "Multiselect";
            f.field_disable = false;
            f.field_mandatory = "yes";
            if (eqpmnts != null)
                foreach (EquipmentValue equipmentValue in eqpmnts.value)
                {
                    field_Options = new Field_Options();
                    field_Options.label = equipmentValue.PARTNAME + " - " + equipmentValue.PARTDES;
                    field_Options.value = equipmentValue.PARTNAME;
                    f.field_options.Add(field_Options);
                }
            page.fields.Add(f);
            page.fields.Add(BuildFotterTable(prep));


            pages.Add(page);

            return pages;
        }

        public ClsPostPrep BuildPost(RootobjectReactForm rootobjectReactForm)
        {

            ClsPostPrep clsPostPrep = new ClsPostPrep();

            

            foreach (var prop in clsPostPrep.GetType().GetProperties())
            {
                try
                {
                    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(clsPostPrep, null));
                    System.TypeCode typeCode = Type.GetTypeCode(prop.PropertyType);
                    switch (typeCode)
                    {
                        case TypeCode.String:
                            string s_val = GetFiledString(rootobjectReactForm, prop.Name);
                            prop.SetValue(clsPostPrep, s_val);
                            break;
                        case (TypeCode.Decimal):
                            decimal d_val = GetFiledDecimal(rootobjectReactForm, prop.Name);
                            prop.SetValue(clsPostPrep, d_val);
                            break;
                        case (TypeCode.Int32):
                            int i_val = GetFiledInt(rootobjectReactForm, prop.Name);
                            prop.SetValue(clsPostPrep, i_val);
                            break;
                        case (TypeCode.Object):
                            var v_val = GetFiledObj(rootobjectReactForm, prop.Name);
                            prop.SetValue(clsPostPrep, v_val);
                            break;
                        default:                          
                            break;
                    }
                }
                catch (Exception er)
                {

                    NeoPharmLog.Write(NeoPharmLog.SeverityLevel.Warn, er, "BuildPost");
                }


            }


            //clsPostPrep.MONITORING = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.NORMALLABEL = GetFiledValue(rootobjectReactForm, "NORMALLABEL");
            //clsPostPrep.STABILITY = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.FLOWRATE = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.WORKSTATIONNAME = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.WORKSTATIONDES = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.WEIGHINGCONTROL = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.UNITS = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.SIGN = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.USERID2 = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.SNAME2 = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.USERID = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.SNAME = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.INITIAL = GetFiledValue(rootobjectReactForm, "MONITORING");
            //clsPostPrep.NEO_PREPADDEQUIP_SUBFORM.Add(new NEO_PREPADDEQUIP_SUBFORM("Z-0008"));
            //clsPostPrep.NEO_PREPADDEQUIP_SUBFORM.Add(new NEO_PREPADDEQUIP_SUBFORM("Z-0009"));
            //clsPostPrep.NEO_PREPADDEQUIP_SUBFORM.Add(new NEO_PREPADDEQUIP_SUBFORM("Z-0027"));
            return clsPostPrep;
        }

        private string GetFiledString(RootobjectReactForm rootobjectReactForm, string fieldName)
        {
            bool dataFound = false;
            string sRet = "";
            foreach (Page p in rootobjectReactForm.pages)
            {
                foreach (Field f in p.fields)
                {
                    if (f.field_id.Equals(fieldName))
                    {
                        if (string.IsNullOrEmpty(f.field_value)) sRet = "";
                        else if (f.field_value.Equals("true")) sRet = "Y";
                        else sRet = f.field_value;
                        dataFound = true;
                        if (dataFound) break;
                    }
                }
                if (dataFound) break;
            }
            return sRet;
        }
        private decimal GetFiledDecimal(RootobjectReactForm rootobjectReactForm, string fieldName)
        {
            bool dataFound = false;
            decimal sRet = 0;
            foreach (Page p in rootobjectReactForm.pages)
            {
                foreach (Field f in p.fields)
                {
                    if (f.field_id.Equals(fieldName))
                    {

                        sRet = decimal.Parse(f.field_value);
                        dataFound = true;
                        if (dataFound) break;
                    }
                }
                if (dataFound) break;
            }
            return sRet;
        }

        private int GetFiledInt(RootobjectReactForm rootobjectReactForm, string fieldName)
        {
            bool dataFound = false;
            int sRet = 0;
            foreach (Page p in rootobjectReactForm.pages)
            {
                foreach (Field f in p.fields)
                {
                    if (f.field_id.Equals(fieldName))
                    {

                        sRet = int.Parse(f.field_value);
                        dataFound = true;
                        if (dataFound) break;
                    }
                }
                if (dataFound) break;
            }
            return sRet;
        }
        private List<NEO_PREPADDEQUIP_SUBFORM> GetFiledObj(RootobjectReactForm rootobjectReactForm, string fieldName)
        {
            bool dataFound = false;
            string tmpfieldName = "equipment";

            List<NEO_PREPADDEQUIP_SUBFORM> NEO_PREPADDEQUIP_SUBFORM = new List<NEO_PREPADDEQUIP_SUBFORM>();
            foreach (Page p in rootobjectReactForm.pages)
            {
                foreach (Field f in p.fields)
                {
                    if (f.field_id.Equals(tmpfieldName))
                    {

                        string sRet = f.field_value;
                        string[] sArr = sRet.Split(',');

                        for (int i = 0; i < sArr.Length; i++)

                        {
                            NEO_PREPADDEQUIP_SUBFORM.Add(new NEO_PREPADDEQUIP_SUBFORM(sArr[i]));
                        }
                        dataFound = true;
                        if (dataFound) break;
                    }
                }
                if (dataFound) break;
            }
            return NEO_PREPADDEQUIP_SUBFORM;
        }

        private Table_options Build_Header_Table(RootobjectReactForm rootobjectReactForm)
        {
            Table_options Table_options = new Table_options();



            return Table_options;
        }
        private Field BuildHeaderTable(ClsPrepTask prep)
        {
            Field f = new Field();
            f.field_id = "tableHeader";
            f.field_label = "Na";
            f.field_placeholder = "Na";
            f.field_type = "table";
            f.field_disable = false;
            f.field_mandatory = "yes";


            Table_options Table_options = new Table_options();
            Table_options.table_header.Add("שם תרופה");
            Table_options.table_header.Add("כמות ב ml");
            Table_options.table_header.Add("סוג ממס נפח סופי ב ml");
            Table_options.table_header.Add("סוג האינפיוזר ");
            Table_row tr = new Table_row();
            tr.table_cell.Add(prep.PARTDES);
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMASML.ToString());
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MEMIS);
            tr.table_cell.Add(prep.RECEPTACLEDES);
            Table_options.table_row.Add(tr);

            f.Table_options = Table_options;


            return f;
        }
        private Field BuildSubHeaderTable(ClsPrepTask prep)
        {
            Field f = new Field();
            f.field_id = "tableHeader";
            f.field_label = "Na";
            f.field_placeholder = "Na";
            f.field_type = "table";
            f.field_disable = false;
            f.field_mandatory = "yes";

            Table_options Table_options = new Table_options();
            Table_options.table_header.Add("שם החומר הפעיל");
            Table_options.table_header.Add("כמות");
            Table_options.table_header.Add("כמות ב ml");

            Table_row tr = new Table_row();
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMAS);
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MMQUANT.ToString());
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMASML.ToString());
            Table_options.table_row.Add(tr);
            if (!string.IsNullOrEmpty(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMAS2))
            {
                tr = new Table_row();
                tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMAS2);
                tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MMQUANT2.ToString());
                tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMASML2.ToString());
                Table_options.table_row.Add(tr);
            }
            if (!string.IsNullOrEmpty(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMAS3))
            {
                tr = new Table_row();
                tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMAS3);
                tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MMQUANT3.ToString());
                tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.MUMASML3.ToString());
                Table_options.table_row.Add(tr);
            }


            f.Table_options = Table_options;


            return f;
        }
        private Field BuildFotterTable(ClsPrepTask prep)
        {
            Field f = new Field();
            f.field_id = "tableFooter";
            f.field_label = "Na";
            f.field_placeholder = "Na";
            f.field_type = "table";
            f.field_disable = false;
            f.field_mandatory = "yes";

            Table_options Table_options = new Table_options();
            Table_options.table_header.Add("");
            Table_options.table_header.Add("(ml) כמות");
            Table_options.table_header.Add("להשלים");

            Table_row tr = new Table_row();
            tr.table_cell.Add("מילוי ממס (מל)");
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.NEOTOMEMMIS.ToString());
            tr.table_cell.Add("");
            Table_options.table_row.Add(tr);

            tr = new Table_row();
            tr.table_cell.Add("דילול (מל)");
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.DILUTION.ToString());
            tr.table_cell.Add("");
            Table_options.table_row.Add(tr);


            tr = new Table_row();
            tr.table_cell.Add("מילוי חומר פעיל לכלי קיבול");
            tr.table_cell.Add(prep.NEO_PRESCRIPTION_LBL_SUBFORM.TOTALMUMASML.ToString());
            tr.table_cell.Add("");
            Table_options.table_row.Add(tr);



            f.Table_options = Table_options;


            return f;
        }
        private Field BuildtextInjection(string textInjection)
        {

            //handle  instrunctions
            Field f = new Field();
            f.field_id = "Instructions";
            f.field_label = "הנחיות";
            f.field_placeholder = f.field_label;
            f.field_type = "instructions";
            f.field_disable = true;
            f.field_mandatory = "yes";
            //f.field_value = " שקית: בוצע ערבוב של השקית על פי הנדרש בנוהל HC-PP-036. בוצעה בדיקה ויזואלית לתוכן השקית שנמצא תקין לשימוש מבחינת צבע,משקעים, אוויר והומוגניות .            ·          אינפיוזר: בוצע Prime לאינפיוזר. בוצעה בדיקה ויזואלית לתוכן האינפיוזרשנמצא תקין לשימוש מבחינת צבע, משקעים, אוויר והומוגניות. בוצע מאזן כמות תרופה. ·          מזרק: בוצעה בדיקה ויזואלית לתוכן המזרק שנמצא תקין לשימוש מבחינת צבע,משקעים, אוויר והומוגניות.  בוצע מאזן כמות תרופה.";
            f.field_value = textInjection;
            return f;
        }
    }
}
