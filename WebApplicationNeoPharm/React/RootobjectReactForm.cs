using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplicationNeoPharm.React
{

    public class RootobjectReactFormRes
    {
        public RootobjectReactFormRes()
        {
            ErrorCode = 200;
            ErrorMessage = "Success";
            rootobjectReactForm = new RootobjectReactForm();

        }

        public string Source { get; set; } 
        public string ErrorMessage { get; set; }
        public string ErrorInnerException { get; set; }
        public int  ErrorCode { get; set; }

        public RootobjectReactForm rootobjectReactForm { get; set; }
    }

        public class RootobjectReactForm
    {

        public RootobjectReactForm()
        {
            pages = new List<Page>();
        }
        public string app_label { get; set; }
        public string app_url { get; set; }
        public List<Page> pages { get; set; }
    }

    public class Page
    {
        public Page()
        {
            fields = new List<Field>();
        }
        public string Page_label { get; set; }
        public List<Field> fields { get; set; }
       
    }

    public class Field
    {
        public Field()
        {
            field_options = new List<Field_Options>();
        }
        public string field_id { get; set; }
        public string field_label { get; set; }
        public string field_mandatory { get; set; }
        public string field_placeholder { get; set; }
        public string field_type { get; set; }
        public string field_value { get; set; }
        public bool field_disable { get; set; }
        public List<Field_Options> field_options { get; set; }
        public Table_options Table_options { get; set; }
    }

    public class Field_Options
    {
        public string label { get; set; }
        public string value { get; set; }
    }

    public class Table_options
    {
        public Table_options()
        {
            table_header = new List<string>();
            table_row = new List<Table_row>();
        }
        public List<string> table_header { get; set; }
        public List<Table_row> table_row { get; set; }

    }

    public class Table_row
    {
        public Table_row()
            {
            table_cell = new List<string>();
            }
        public List<string> table_cell { get; set; }

    }

}
