using System;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;


using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;


namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcServiceArea : UserControl
    {

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
                if (Request.Params["idArea"] != null)
                {
                    rptConsultas.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoTerminalAllTipificacion(int.Parse(Request.Params["idArea"]), ((Usuario)Session["UserData"]).IdTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion, null, null, null, null, null, null, null);
                    rptConsultas.DataBind();
                    rptServicios.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoTerminalAllTipificacion(int.Parse(Request.Params["idArea"]), ((Usuario)Session["UserData"]).IdTipoUsuario, (int)BusinessVariables.EnumTipoArbol.SolicitarServicio, null, null, null, null, null, null, null);
                    rptServicios.DataBind();
                    rptIncidentes.DataSource = new ServiceArbolAccesoClient().ObtenerArbolesAccesoTerminalAllTipificacion(int.Parse(Request.Params["idArea"]), ((Usuario)Session["UserData"]).IdTipoUsuario, (int)BusinessVariables.EnumTipoArbol.ReportarProblemas, null, null, null, null, null, null, null);
                    rptIncidentes.DataBind();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        protected void lbTipoUsuario_Click(object sender, EventArgs e)
        {
            try
            {
                string tipoUsuario = ((Usuario)Session["UserData"]).IdTipoUsuario.ToString();
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