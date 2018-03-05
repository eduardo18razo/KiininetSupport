using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KiiniHelp.Test
{

    public partial class FrmTestTelerik : System.Web.UI.Page
    {
        public DataTable GetData()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID");
            table.Columns.Add("ParentID");
            table.Columns.Add("Value");
            table.Columns.Add("Text");

            table.Rows.Add(new String[] { "1", null, "World_Continents", "World Continents" });
            table.Rows.Add(new String[] { "2", null, "World_Oceans", "World Oceans" });

            table.Rows.Add(new String[] { "3", "1", "Asia", "Asia" });
            table.Rows.Add(new String[] { "4", "1", "Africa", "Africa" });
            table.Rows.Add(new String[] { "5", "1", "Australia", "Australia" });
            table.Rows.Add(new String[] { "6", "1", "Europe", "Europe" });
            table.Rows.Add(new String[] { "7", "1", "North_America", "North America" });
            table.Rows.Add(new String[] { "8", "1", "South_America", "South America" });

            table.Rows.Add(new String[] { "9", "2", "Arctic_Ocean", "Arctic Ocean" });
            table.Rows.Add(new String[] { "10", "2", "Atlantic_Ocean", "Atlantic Ocean" });
            table.Rows.Add(new String[] { "11", "2", "Indian_Ocean", "Indian Ocean" });
            table.Rows.Add(new String[] { "12", "2", "Pacific_Ocean", "Pacific Ocean" });
            table.Rows.Add(new String[] { "13", "2", "South_Ocean", "SouthOcean" });

            return table;
        } 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string[] cars = new string[] { "Porsche Carrera", "Ferrari F430", "Aston Martin DB9" };
                ddlUsuarioAsignacion.DataSource = cars;
                ddlUsuarioAsignacion.DataBind();
                RadDropDownTree1.DataFieldID = "ID";
                RadDropDownTree1.DataFieldParentID = "ParentID";
                RadDropDownTree1.DataValueField = "Value";
                RadDropDownTree1.DataTextField = "Text";
                RadDropDownTree1.DataSource = GetData();
                RadDropDownTree1.DataBind();
            }
        }
    }
}