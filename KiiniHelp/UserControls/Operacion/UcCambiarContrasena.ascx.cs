using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using KiiniHelp.ServiceSeguridad;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Operacion
{
    public partial class UcCambiarContrasena : UserControl, IControllerModal
    {
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();
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
                StringBuilder sb = new StringBuilder();
                if (txtContrasenaActual.Text.Trim() == string.Empty)
                    sb.AppendLine("<li>Contraseña antigua es campo obligatorio</li>");
                if (txtContrasenaNueva.Text.Trim() == string.Empty)
                    sb.AppendLine("<li>Contraseña nueva es campo obligatorio</li>");
                if (txtContrasenaNueva.Text.Trim() != txtConfirmaContrasenaNueva.Text.Trim())
                    sb.AppendLine("<li>Las contraseñas no coinciden</li>");
                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Datos Generales</h3>");
                    throw new Exception(sb.ToString());
                }
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
                AlertaGeneral = new List<string>();
                if (!IsPostBack)
                {
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

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
    }
}