using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Usuarios
{
    public partial class UcAltaUsuarioRapida : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private readonly ServiceUsuariosClient _servicioUsuario = new ServiceUsuariosClient();
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

        public void RegistraUsuario()
        {
            try
            {
                GuardarUsuario();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int IdUsuario
        {
            get { return int.Parse(hfIdUsuario.Value); }
            set { hfIdUsuario.Value = value.ToString(); }
        }

        private void Limpiar()
        {
            try
            {
                txtNombre.Text = string.Empty;
                txtAp.Text = string.Empty;
                txtAm.Text = string.Empty;
                txtCorreo.Text = string.Empty;
                txtTelefono.Text = string.Empty;
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
                Alerta = new List<string>();
                //btnGuardar.OnClientClick = "this.disabled = true; " + Page.ClientScript.GetPostBackEventReference(btnGuardar, null) + ";";
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

        private void ValidaCaptura()
        {
            List<string> sb = new List<string>();

            if (txtAp.Text.Trim() == string.Empty)
                sb.Add("Apellido Paterno es un campo obligatorio.");
            if (txtAm.Text.Trim() == string.Empty)
                sb.Add("Apellido Materno es un campo obligatorio.");
            if (txtNombre.Text.Trim() == string.Empty)
                sb.Add("Nombre es un campo obligatorio.");

            bool capturoTelefono = false, capturoCorreo = false;
            if (txtCorreo.Text.Trim() != string.Empty)
                capturoCorreo = true;
            if (txtTelefono.Text.Trim() != string.Empty)
                capturoTelefono = true;
            if (!capturoCorreo)
                sb.Add("Debe capturar un correo.");
            if (!capturoTelefono)
                sb.Add("Debe capturar un telefono.");
            //if (!capturoCorreo && !capturoTelefono)
            //    sb.Add("Debe capturar correo y/o telefono.");
            MailAddress m = new MailAddress(txtCorreo.Text);

            if (sb.Count > 0)
            {
                sb.Insert(0, "<h3>Datos Generales</h3>");
                _lstError = sb;
                throw new Exception("");
            }
        }

        private string GeneraNombreUsuario()
        {
            string result = null;
            try
            {
                string username = (txtNombre.Text.Substring(0, 1).ToLower() + txtAp.Text.Trim().ToLower()).Replace(" ", string.Empty);
                username = username.PadRight(30).Substring(0, 30).Trim();
                int limite = 10;
                if (_servicioUsuario.ValidaUserName(username))
                {
                    for (int i = 1; i < limite; i++)
                    {
                        string tmpUsername = username + i;
                        if (!_servicioUsuario.ValidaUserName(tmpUsername.PadRight(30).Substring(0, 30).Trim()))
                        {
                            username = tmpUsername;
                            break;
                        }
                        limite++;
                    }
                }
                result = username;
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
            return result;
        }
        private void GuardarUsuario()
        {
            try
            {
                ValidaCaptura();
                Usuario datosUsuario = new Usuario
                {
                    IdTipoUsuario = (int) BusinessVariables.EnumTiposUsuario.Cliente,
                    ApellidoPaterno = txtAp.Text.Trim(),
                    ApellidoMaterno = txtAm.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    DirectorioActivo = false,
                    Vip = false,
                    PersonaFisica = false,
                    NombreUsuario = GeneraNombreUsuario(),
                    Password = ResolveUrl("~/ConfirmacionCuenta.aspx"),
                    Habilitado = true
                };
                if (txtTelefono.Text.Trim() != string.Empty)
                    datosUsuario.TelefonoUsuario = new List<TelefonoUsuario>
                    {
                        new TelefonoUsuario
                        {
                            IdTipoTelefono = (int) BusinessVariables.EnumTipoTelefono.Celular,
                            Confirmado = false,
                            Extension = string.Empty,
                            Numero = txtTelefono.Text.Trim(),
                            Obligatorio = true
                        }
                    };
                if (txtCorreo.Text.Trim() != string.Empty)
                    datosUsuario.CorreoUsuario = new List<CorreoUsuario>
                    {
                        new CorreoUsuario
                        {
                            Correo = txtCorreo.Text.Trim(),
                            Obligatorio = true,
                        }
                    };
                IdUsuario = _servicioUsuario.RegistrarCliente(datosUsuario);
                Limpiar();
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
    }
}