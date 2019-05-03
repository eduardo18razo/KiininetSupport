using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;


namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcCategoria : UserControl
    {
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();

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
                if (Request.Params["userType"] != null)
                {
                    TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(int.Parse(Request.Params["userType"]));

                    lnkbtnTipoUsuario.Text = tipoUsuario != null ? tipoUsuario.Descripcion : "N/A";

                    rptAreas.DataSource = Session["UserData"] == null
                        ? _servicioArea.ObtenerAreasTipoUsuario(int.Parse(Request.Params["userType"]), false)
                            .Where(w => !w.Sistema)
                        : _servicioArea.ObtenerAreasUsuario(((Usuario)Session["UserData"]).Id, false)
                            .Where(w => !w.Sistema);
                    rptAreas.DataBind();
                }
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
                if (Session["UserData"] == null)
                    Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + tipoUsuario + "&idArea=" + lnkbtn.CommandArgument);
                else
                    Response.Redirect("~/Users/FrmDashboardUser.aspx?userType=" + tipoUsuario + "&idArea=" + lnkbtn.CommandArgument);

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
                if (Session["UserData"] == null)
                    Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + tipoUsuario);
                else
                    Response.Redirect("~/Users/FrmDashboardUser.aspx");
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