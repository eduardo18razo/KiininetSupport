using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Preview
{
    public partial class UcVisorConsultainformacion : UserControl, IControllerModal
    {
        private readonly ServiceInformacionConsultaClient _servicioInformacion = new ServiceInformacionConsultaClient();
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
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", @"ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        public int IdArbol
        {
            get { return int.Parse(hfIdArbolAcceso.Value); }
            set
            {
                hfIdArbolAcceso.Value = value.ToString();
                ArbolAcceso arbol = new ServiceArbolAccesoClient().ObtenerArbolAcceso(value);
                if (arbol != null)
                {
                    lbtnTipoUsuario.Text = arbol.TipoUsuario.Descripcion;
                    IdTipoUsuario = arbol.IdTipoUsuario;
                    lblDescripcionConsulta.Text = arbol.InventarioArbolAcceso[0].Descripcion;
                    lblTitle.Text = arbol.InventarioArbolAcceso[0].Descripcion;
                    lblDescripcion.Text = arbol.Descripcion;
                    MuestraPreview(_servicioInformacion.ObtenerInformacionConsultaById(arbol.InventarioArbolAcceso.First().InventarioInfConsulta.First().IdInfConsulta));
                }


            }
        }

        public int IdInformacionconsulta
        {
            get { return int.Parse(hfIdInformacinConsulta.Value); }
            set { hfIdInformacinConsulta.Value = value.ToString(); }
        }

        public int IdTipoUsuario
        {
            get { return int.Parse(hfIdTipoUsuario.Value); }
            set { hfIdTipoUsuario.Value = value.ToString(); }
        }

        public bool MuestraEvaluacion
        {
            get { return bool.Parse(hfEvaluacion.Value); }
            set { hfEvaluacion.Value = value.ToString(); }
        }
        public bool MeGusta
        {
            get { return bool.Parse(hfMeGusta.Value); }
            set
            {
                hfMeGusta.Value = value.ToString();
                SetLike();
            }
        }

        private void SetLike()
        {
            try
            {
                Image img;
                if (MeGusta)
                {
                    img = (Image)lbtnNotLike.FindControl("imgDontLike");
                    if (img != null)
                    {
                        img.CssClass = "dontlike";
                    }
                    img = (Image)lbtnNotLike.FindControl("imgLike");
                    if (img != null)
                    {
                        img.CssClass = "";
                    }
                }
                else
                {
                    img = (Image)lbtnNotLike.FindControl("imgLike");
                    if (img != null)
                    {
                        img.CssClass = "dontlike";
                    }
                    img = (Image)lbtnNotLike.FindControl("imgDontLike");
                    if (img != null)
                    {
                        img.CssClass = "";
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void MuestraPreview(InformacionConsulta datos)
        {
            try
            {
                IdInformacionconsulta = datos.Id;

                TextPreview.InnerHtml = datos.InformacionConsultaDatos.First().Datos;
                rptArchivos.DataSource = datos.InformacionConsultaDocumentos;
                rptArchivos.DataBind();
                int idUsuario = 0;
                if (Session["UserData"] != null)
                {
                    idUsuario = ((Usuario)Session["UserData"]).Id;
                    InformacionConsultaRate rate = datos.InformacionConsultaRate.SingleOrDefault(s => s.IdUsuario == idUsuario);
                    if (rate != null)
                    {
                        MeGusta = rate.MeGusta;
                    }
                    divEvaluacion.Visible = MuestraEvaluacion;
                }
                else
                    divEvaluacion.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IdArbol == 0)
                    throw new Exception("No se puede mostrar esta información.");
                if (!IsPostBack)
                    if (Session["UserData"] != null)
                        _servicioInformacion.GuardarHit(IdArbol, IdTipoUsuario, ((Usuario)Session["UserData"]).Id);
                    else
                        _servicioInformacion.GuardarHit(IdArbol, IdTipoUsuario, null);

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

        protected void btnPreviewDocument_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                BusinessFile.CopiarSitioTemporal(BusinessVariables.Directorios.RepositorioInformacionConsulta, BusinessVariables.Directorios.CarpetaTemporal, new List<string> { btn.CommandArgument });
                string url = string.Format("https://docs.google.com/viewer?url={0}{1}", BusinessVariables.Directorios.CarpetaTemporalSitio, btn.CommandArgument);
                url = Page.ResolveUrl("~/Preview/FrmPreview.aspx?urlGoogle=" + url);
                string script = string.Format("window.open('{0}','_blank')", url);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptViewer", script, true);
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

        protected void lbtnLike_OnClick(object sender, EventArgs e)
        {
            try
            {
                MeGusta = true;
                _servicioInformacion.RateConsulta(IdArbol, IdInformacionconsulta, ((Usuario)Session["UserData"]).Id, MeGusta);
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

        protected void lbtnNotLike_OnClick(object sender, EventArgs e)
        {
            try
            {
                MeGusta = false;
                _servicioInformacion.RateConsulta(IdArbol, IdInformacionconsulta, ((Usuario)Session["UserData"]).Id, MeGusta);
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
        protected void lbtnHome_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserData"] == null)
                    Response.Redirect("~/Default.aspx");
                else
                    Response.Redirect("~/Users/DashBoard.aspx");
            }
            catch
            {
            }
        }

        protected void lbtnTipoUsuario_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserData"] == null)
                    Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + IdTipoUsuario);
                else
                    Response.Redirect("~/Users/FrmDashboardUser.aspx");
            }
            catch
            {
            }
        }
    }
}