using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArea;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Agentes
{
    public partial class UcBusquedaFormulario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private List<string> _lstError = new List<string>();

        private List<string> Alerta
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

        public int IdUsuarioSolicito
        {
            get { return int.Parse(hfIdUsuarioSolicito.Value); }
            set { hfIdUsuarioSolicito.Value = value.ToString(); }
        }

        public int IdArbolAcceso
        {
            get
            {
                if (string.IsNullOrEmpty(txtBusquedaFormulario.Text.Trim()))
                    throw new Exception("Ingrese un Formulario");
                return int.Parse(txtBusquedaFormulario.Text.Trim());
            }
        }

        private int IdAreaSeleccionada
        {
            get { return int.Parse(hfAreaSeleccionada.Value); }
            set { hfAreaSeleccionada.Value = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Request.Params["idArbol"] != null)
                    {

                    }
                    else if (Request.Params["idUsuarioSolicitante"] != null)
                    {
                        Session["IdUsuarioSolicita"] = Request.Params["idUsuarioSolicitante"];
                        IdUsuarioSolicito = int.Parse(Request.Params["idUsuarioSolicitante"]);
                    }
                    else
                        Response.Redirect("~/Agente/Bandeja.aspx");
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

        protected void txtBusquedaFormulario_OnDataSourceSelect(object sender, SearchBoxDataSourceSelectEventArgs e)
        {
            try
            {
                ObjectDataSource source = (ObjectDataSource)e.DataSource;
                RadSearchBox searchBox = (RadSearchBox)sender;
                if (e.SelectedContextItem.Key != IdAreaSeleccionada.ToString())
                {
                    source.SelectParameters["idUsuarioSolicita"].DefaultValue = Session["IdUsuarioSolicita"].ToString();
                    source.SelectParameters["idUsuarioLevanta"].DefaultValue = ((Usuario)Session["UserData"]).Id.ToString();
                    source.SelectParameters["idArea"].DefaultValue = e.SelectedContextItem.Key;
                    source.SelectParameters["keys"].DefaultValue = e.FilterString;
                    source.DataBind();
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

        protected void txtBusquedaFormulario_OnSearch(object sender, SearchBoxEventArgs e)
        {
            try
            {
                if (e.Value != null  && e.Value != "0" && !string.IsNullOrEmpty(e.Value.Trim()))
                {
                    Response.Redirect("~/Agente/FrmAgenteNuevoTicket.aspx?idUsuarioSolicitante=" + IdUsuarioSolicito + "&IdArbol=" + e.Value + "&Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&UsuarioSolicita=" + IdUsuarioSolicito);
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
    }
}