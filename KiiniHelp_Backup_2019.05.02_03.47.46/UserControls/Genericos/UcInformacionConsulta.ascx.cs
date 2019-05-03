using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Cat.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Genericos
{
    public partial class UcInformacionConsulta : UserControl
    {
        readonly ServiceArbolAccesoClient _servicioArbolAcceso = new ServiceArbolAccesoClient();
        readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();
        private List<string> _lstError = new List<string>();
        public int IdArbol { get { return Convert.ToInt32(hfIdArbol.Value); } set { hfIdArbol.Value = value.ToString(); } }

        private List<string> AlertaGeneral
        {
            set
            {
                pnlAlertaGeneral.Visible = value.Any();
                if (!pnlAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _lstError = new List<string>();
                if (!IsPostBack)
                {
                    ArbolAcceso arbol = _servicioArbolAcceso.ObtenerArbolAcceso(IdArbol);
                    rptInformacionConsulta.DataSource = _servicioInformacionConsulta.ObtenerInformacionConsultaArbol(IdArbol);
                    rptInformacionConsulta.DataBind();
                    lbltitleArbol.Text = "CONSULTA > ";
                    if (arbol.Nivel1 != null)
                        lbltitleArbol.Text += arbol.Nivel1.Descripcion;
                    if (arbol.Nivel2 != null)
                        lbltitleArbol.Text += " > " + arbol.Nivel2.Descripcion;
                    if (arbol.Nivel3 != null)
                        lbltitleArbol.Text += " > " + arbol.Nivel3.Descripcion;
                    if (arbol.Nivel4 != null)
                        lbltitleArbol.Text += " > " + arbol.Nivel4.Descripcion;
                    if (arbol.Nivel5 != null)
                        lbltitleArbol.Text += " > " + arbol.Nivel5.Descripcion;
                    if (arbol.Nivel6 != null)
                        lbltitleArbol.Text += " > " + arbol.Nivel6.Descripcion;
                    if (arbol.Nivel7 != null)
                        lbltitleArbol.Text += " > " + arbol.Nivel7.Descripcion;
                }
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

        protected void btnInformacion_OnClick(object sender, EventArgs e)
        {
            //TODO: CAMBIAR METODOS
            //try
            //{
            //    Button btn = (Button)sender;
            //    if (btn != null)
            //    {
            //        InformacionConsulta ic = _servicioInformacionConsulta.ObtenerInformacionConsultaById(Convert.ToInt32(btn.CommandArgument));
            //        hfIdInformacion.Value = ic.Id.ToString();
            //        switch (ic.IdTipoInfConsulta)
            //        {
                            
            //            case (int)BusinessVariables.EnumTiposInformacionConsulta.EditorDeContenido:
            //                lblContenido.Text = string.Empty;
            //                foreach (InformacionConsultaDatos contenindo in ic.InformacionConsultaDatos.OrderBy(o => o.Orden))
            //                {
            //                    lblContenido.Text += contenindo.Descripcion;
            //                }
            //                divPropuetario.Visible = true;
            //                divInfoDocto.Visible = false;
            //                if (btn.CommandName == "0")
            //                {
            //                    _servicioInformacionConsulta.GuardarHit(IdArbol, ((Usuario)Session["UserData"]).Id);
            //                    btn.CommandName = "1";
            //                }
            //                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalMuestraInformacion\");", true);
            //                break;
            //            case (int)BusinessVariables.EnumTiposInformacionConsulta.DocumentoOffice:
            //                string nombreDocto = ic.InformacionConsultaDatos.OrderBy(o => o.Orden).Aggregate(string.Empty, (current, contenindo) => current + contenindo.Descripcion);
            //                ifDoctos.Attributes.Add("src", string.Format("../General/FrmMostrarDocumento.aspx?NombreDocumento={0}&TipoDocumento={1}", nombreDocto, ic.IdTipoDocumento));
            //                divPropuetario.Visible = false;
            //                divInfoDocto.Visible = true;
            //                if (btn.CommandName == "0")
            //                {
            //                    _servicioInformacionConsulta.GuardarHit(IdArbol, ((Usuario)Session["UserData"]).Id);
            //                    btn.CommandName = "1";
            //                }
            //                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalMuestraInformacion\");", true);
            //                break;
            //            case (int)BusinessVariables.EnumTiposInformacionConsulta.DireccionWeb:
            //                string url = ic.InformacionConsultaDatos.OrderBy(o => o.Orden).Aggregate(string.Empty, (current, contenindo) => current + contenindo.Descripcion);
            //                if (btn.CommandName == "0")
            //                {
            //                    _servicioInformacionConsulta.GuardarHit(IdArbol, ((Usuario)Session["UserData"]).Id);
            //                    btn.CommandName = "1";
            //                }
            //                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "OpenWindow(\"" + url + "\");", true);
            //                break;
            //        }
            //        rptDownloads.DataSource = ic.InformacionConsultaDocumento;
            //        rptDownloads.DataBind();

            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (_lstError == null)
            //    {
            //        _lstError = new List<string>();
            //    }
            //    _lstError.Add(ex.Message);
            //    AlertaGeneral = _lstError;
            //}
        }

        protected void btnCerrarModalInfo_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalMuestraInformacion\");", true);
                string url = ResolveUrl("~/FrmEncuesta.aspx?IdTipoServicio=" + (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion + "&IdTicket=" + IdArbol);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptEncuesta", "OpenWindow(\"" + url + "\");", true);
                
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