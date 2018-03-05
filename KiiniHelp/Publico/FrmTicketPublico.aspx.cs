using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.Publico
{
    public partial class FrmTicketPublico : Page
    {
        readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        readonly ServiceTicketClient _servicioTicket = new ServiceTicketClient();
        private List<string> _lstError = new List<string>();

        private List<string> AlertaGeneral
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
        public int? IdConsulta
        {
            get
            {
                int result;
                if (hfIdConsulta.Value != string.Empty)
                    result = Convert.ToInt32(hfIdConsulta.Value);
                else
                    result = (int)Session["IdConsultaTicket"];
                return result;
            }
            set
            {
                if (hfIdConsulta != null)
                {
                    hfIdConsulta.Value = value.ToString();
                    Session.Remove("IdConsultaTicket");
                }
                else
                    Session["IdConsultaTicket"] = value;
            }
        }
        public int? IdMascara
        {
            get
            {
                int result;
                if (hfIdMascara.Value != string.Empty)
                    result = Convert.ToInt32(hfIdMascara.Value);
                else
                    result = (int)ViewState["IdMascaraTicket"];
                return result;
            }
            set
            {
                if (hfIdMascara != null)
                {
                    hfIdMascara.Value = value.ToString();
                    Session.Remove("IdMascaraTicket");
                }
                else
                    ViewState["IdMascaraTicket"] = value;
            }
        }

        public int IdSla
        {
            get
            {
                int result;
                if (hfIdSla.Value != string.Empty)
                    result = Convert.ToInt32(hfIdSla.Value);
                else
                    result = (int)Session["IdSlaTicket"];
                return result;
            }
            set
            {
                if (hfIdSla != null)
                {
                    hfIdSla.Value = value.ToString();
                    Session.Remove("IdSlaTicket");
                }
                else
                    Session["IdSlaTicket"] = value;
            }
        }

        public int IdEncuesta
        {
            get
            {
                int result;
                if (hfIdEncuesta.Value != string.Empty)
                {
                    result = Convert.ToInt32(hfIdEncuesta.Value);
                    Session.Remove("IdEncuestaTicket");
                }
                else
                    result = (int)Session["IdEncuestaTicket"];
                return result;
            }
            set
            {
                if (hfIdEncuesta != null)
                    hfIdEncuesta.Value = value.ToString();
                else
                    Session["IdEncuestaTicket"] = value;
            }
        }

        public int IdCanal
        {
            get
            {
                int result;
                if (hfIdCanal.Value != string.Empty)
                    result = Convert.ToInt32(hfIdCanal.Value);
                else
                    result = (int)Session["hfIdCanal"];
                return result;
            }
            set
            {
                if (hfIdCanal != null)
                {
                    hfIdCanal.Value = value.ToString();
                    Session.Remove("hfIdCanal");
                }
                else
                    Session["hfIdCanal"] = value;
            }
        }


        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["IdArbol"] != null && Request.QueryString["Canal"] != null)
                {
                    ArbolAcceso arbol = _servicioArbolAcceso.ObtenerArbolAcceso(Convert.ToInt32(Request.QueryString["IdArbol"]));
                    Session["ArbolAcceso"] = arbol;
                    IdMascara = arbol.InventarioArbolAcceso.First().IdMascara ?? 0;
                    IdEncuesta = arbol.InventarioArbolAcceso.First().IdEncuesta ?? 0;
                    IdCanal = int.Parse(Request.QueryString["Canal"]);
                }
                else if (Request.QueryString["IdArbol"] != null && Request.QueryString["Canal"] != null)
                {
                    ArbolAcceso arbol = _servicioArbolAcceso.ObtenerArbolAcceso(Convert.ToInt32(Request.QueryString["IdArbol"]));
                    Session["ArbolAcceso"] = arbol;
                    IdMascara = arbol.InventarioArbolAcceso.First().IdMascara ?? 0;
                    IdEncuesta = arbol.InventarioArbolAcceso.First().IdEncuesta ?? 0;
                    IdCanal = (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                ucFormulario.OnAceptarModal += UcFormulario_OnAceptarModal;
                ucFormulario.OnCancelarModal += UcFormulario_OnCancelarModal;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void UcFormulario_OnCancelarModal()
        {
            try
            {
                Global.Terminar();
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }

        private void UcFormulario_OnAceptarModal()
        {
            try
            {
                Public master = (Public)Page.Master;
                if (master != null) 
                    Response.Redirect("~/Publico/FrmUserSelect.aspx?userTipe=" + master.TipoUsuario);
                else
                    Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }


        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                Global.Terminar();
                Response.Redirect("~/Default.aspx");
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGeneral = _lstError;
            }
        }
    }
}