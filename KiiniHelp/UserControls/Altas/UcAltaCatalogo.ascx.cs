using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaCatalogo : UserControl, IControllerModal
    {
        private readonly ServiceCatalogosClient _servicioCatalogo = new ServiceCatalogosClient();
        private List<string> _lstError = new List<string>();

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set { hfEsAlta.Value = value.ToString(); }
        }

        public int IdCatalogo
        {
            get { return Convert.ToInt32(hfIdCatalogo.Value); }
            set
            {
                Catalogos puesto = _servicioCatalogo.ObtenerCatalogos(false).Single(s => s.Id == value);
                txtDescripcionCatalogo.Text = puesto.Descripcion;
                hfIdCatalogo.Value = value.ToString();
            }
        }

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

        public void LimpiarCampos()
        {
            try
            {
                txtDescripcionCatalogo.Text = String.Empty;
                Session["registrosCatalogos"] = new List<CatalogoGenerico>();
                LlenaRegistros();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LlenaRegistros()
        {
            try
            {
                rptRegistros.DataSource = Session["registrosCatalogos"];
                rptRegistros.DataBind();
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
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Session["registrosCatalogos"] = new List<CatalogoGenerico>();
                    LlenaRegistros();
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

        protected void btnBorrarRegistro_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton remover = (LinkButton)sender;
                List<CatalogoGenerico> tmpList = Session["registrosCatalogos"] == null ? new List<CatalogoGenerico>() : (List<CatalogoGenerico>)Session["registrosCatalogos"];
                tmpList.RemoveAt(int.Parse(remover.CommandArgument));
                Session["registrosCatalogos"] = tmpList;
                LlenaRegistros();
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

        protected void btnAgregarRegistro_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton adder = (LinkButton)sender;
                var itemFooter = adder.NamingContainer;
                TextBox txtFooter = (TextBox)itemFooter.FindControl("txtRegistroNew");
                if (txtFooter != null)
                {
                    if (txtFooter.Text.Trim() == string.Empty)
                        throw new Exception("Ingrese una descripción.");
                    List<CatalogoGenerico> tmpList = Session["registrosCatalogos"] == null ? new List<CatalogoGenerico>() : (List<CatalogoGenerico>)Session["registrosCatalogos"];
                    tmpList.Add(new CatalogoGenerico { Descripcion = txtFooter.Text.Trim().ToUpper() });
                    Session["registrosCatalogos"] = tmpList;
                    LlenaRegistros();
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

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDescripcionCatalogo.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripción");
                Catalogos cat = new Catalogos();
                cat.Descripcion = txtDescripcionCatalogo.Text;
                cat.DescripcionLarga = txtDescripcionCatalogo.Text;
                cat.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;

                if (EsAlta)
                    _servicioCatalogo.CrearCatalogo(cat, true, (List<CatalogoGenerico>)Session["registrosCatalogos"]);
                LimpiarCampos();
                if (OnAceptarModal != null)
                    OnAceptarModal();
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

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos();
                if (OnLimpiarModal != null)
                    OnLimpiarModal();
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

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos();
                if (OnCancelarModal != null)
                    OnCancelarModal();
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