using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using KiiniHelp.ServiceArbolAcceso;
using KiiniNet.Entities.Cat.Operacion;
using Telerik.Web.UI;

namespace KiiniHelp.Test
{
    public partial class FrmMasterDetail : System.Web.UI.Page
    {
        private readonly ServiceArbolAccesoClient _servicioArbol = new ServiceArbolAccesoClient();

        private List<ArbolAcceso> GetDataSections()
        {
            try
            {
                List<ArbolAcceso> arboles = _servicioArbol.ObtenerArbolesAccesoAll(null, null, null, null, null, null, null, null, null, null);
                arboles = arboles.Where(w => w.EsTerminal == false).ToList();
                return arboles;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private List<ArbolAcceso> GetDataOptions(int? idArea, int? idNivel1, int? idNivel2, int? idNivel3, int? idNivel4, int? idNivel5, int? idNivel6, int? idNivel7)
        {
            try
            {
                List<ArbolAcceso> arboles = _servicioArbol.ObtenerArbolesAccesoAll(idArea, null, null, idNivel1, idNivel2, idNivel3, idNivel4, idNivel5, idNivel6, idNivel7).Where(w => w.EsTerminal).ToList();
                return arboles;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void RadGrid1_NeedDataSource(object source, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (!e.IsFromDetailTable)
                {
                    RadGrid1.DataSource = GetDataSections();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void RadGrid1_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                switch (e.DetailTableView.Name)
                {
                    case "Opcion":
                        {
                            int? idArea = int.Parse(dataItem.GetDataKeyValue("Id").ToString());
                            int? idNivel1 = dataItem.GetDataKeyValue("IdNivel1") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel1").ToString()) : (int?)null;
                            int? idNivel2 = dataItem.GetDataKeyValue("IdNivel2") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel2").ToString()) : (int?)null;
                            int? idNivel3 = dataItem.GetDataKeyValue("IdNivel3") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel3").ToString()) : (int?)null;
                            int? idNivel4 = dataItem.GetDataKeyValue("IdNivel4") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel4").ToString()) : (int?)null;
                            int? idNivel5 = dataItem.GetDataKeyValue("IdNivel5") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel5").ToString()) : (int?)null;
                            int? idNivel6 = dataItem.GetDataKeyValue("IdNivel6") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel6").ToString()) : (int?)null;
                            int? idNivel7 = dataItem.GetDataKeyValue("IdNivel7") != null ? int.Parse(dataItem.GetDataKeyValue("IdNivel7").ToString()) : (int?)null;


                            e.DetailTableView.DataSource = GetDataOptions(idArea, idNivel1, idNivel2, idNivel3, idNivel4, idNivel5, idNivel6, idNivel7);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetDataTable(string query, string selectParameter = "", string parameterValue = "")
        {
            String ConnString = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(ConnString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(query, conn);

            if (!string.IsNullOrEmpty(selectParameter))
            {
                command.Parameters.Add(new SqlParameter(selectParameter, parameterValue));
            }

            adapter.SelectCommand = command;

            DataTable myDataTable = new DataTable();

            conn.Open();
            try
            {
                adapter.Fill(myDataTable);
            }
            finally
            {
                conn.Close();
            }

            return myDataTable;
        }

        protected void RadGrid1_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if (RadGrid1.MasterTableView.Items.Count > 0)
                    {
                        RadGrid1.MasterTableView.Items[0].Expanded = true;
                        RadGrid1.MasterTableView.Items[0].ChildItem.NestedTableViews[0].Items[0].Expanded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}