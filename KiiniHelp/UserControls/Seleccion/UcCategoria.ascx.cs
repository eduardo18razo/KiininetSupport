using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArea;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;


namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcCategoria : UserControl
    {
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();

        private List<string> _lstError = new List<string>();

        public List<string> Alerta
        {
            set
            {
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lbTipoUsuario.Text = string.Format("{0}", int.Parse(Request.Params["userType"]) == (int)BusinessVariables.EnumTiposUsuario.Empleado ? "Empleado" : int.Parse(Request.Params["userType"]) == (int)BusinessVariables.EnumTiposUsuario.Cliente ? "Cliente" : "Proveedor");
                rptAreas.DataSource = _servicioArea.ObtenerAreasTipoUsuario(int.Parse(Request.Params["userType"]), false).Where(w => !w.Sistema);
                rptAreas.DataBind();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void btnAreaSelect_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkbtn = (LinkButton)sender;
                int tipoUsuario = int.Parse(Request.Params["userType"]);
                //Response.Redirect("~/Publico/FrmServiceArea.aspx?idArea=" + lnkbtn.CommandArgument);

                Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + tipoUsuario + "&idArea=" + lnkbtn.CommandArgument);

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }

        protected void lbTipoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                string tipoUsuario = Request.Params["userType"];
                Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + tipoUsuario);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                Alerta = _lstError;
            }
        }
    }
}