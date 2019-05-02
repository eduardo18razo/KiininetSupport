using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSeguridad;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KiiniHelp.UserControls.Operacion
{
    public partial class UcCambiarContrasena : UserControl, IControllerModal
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
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

        private void ValidaCampos()
        {
            try
            {
                if (txtContrasenaActual.Text.Trim() == string.Empty)
                    throw new Exception("Contraseña antigua es campo obligatorio~");
                if (txtContrasenaNueva.Text.Trim() == string.Empty)
                    throw new Exception("Contraseña nueva es campo obligatorio~");
                if (txtContrasenaNueva.Text.Trim() != txtConfirmaContrasenaNueva.Text.Trim())
                    throw new Exception("Las contraseñas no coinciden");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void CondicioinPassword()
        {
            try
            {
                ParametrosGenerales parametrosGenerales = _servicioParametros.ObtenerParametrosGenerales();
                if (parametrosGenerales != null)
                {
                    ParametroPassword parametrosPassword = _servicioParametros.ObtenerParemtrosPassword();
                    if (parametrosPassword != null)
                    {
                        lblCaracteristicas.Visible = parametrosGenerales.StrongPassword;
                        listParamtros.Visible = parametrosGenerales.StrongPassword;
                        lblLongitud.Text = string.Format("Longitud minima de {0} caracteres", parametrosPassword.Min);
                        lblLongitudMayus.Text = string.Format("{0} Mayuscula", parametrosPassword.Mayusculas);
                        paramMayuscula.Visible = parametrosPassword.Mayusculas > 0;
                        paramNumero.Visible = parametrosPassword.Numeros;
                        paramEspecial.Visible = parametrosPassword.Especiales;
                    }
                }
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
                AlertaGeneral = new List<string>();
                if (!IsPostBack)
                {
                    CondicioinPassword();
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

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCampos();
                _servicioSeguridad.ValidaPassword(txtContrasenaNueva.Text.Trim());
                _servicioSeguridad.ChangePassword(((Usuario)Session["UserData"]).Id, txtContrasenaActual.Text.Trim(), txtContrasenaNueva.Text.Trim());
                if (OnAceptarModal != null)
                    OnAceptarModal();
                Response.Redirect(ResolveUrl("~/Users/DashBoard.aspx"));//Agregado
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                if(ex.Message.Contains("~"))
                    foreach (string msg in ex.Message.Split('~'))
                    {
                        _lstError.Add(msg);
                    }
                else
                {
                    _lstError.Add(ex.Message);
                }
                AlertaGeneral = _lstError;
            }
        }

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
    }
}