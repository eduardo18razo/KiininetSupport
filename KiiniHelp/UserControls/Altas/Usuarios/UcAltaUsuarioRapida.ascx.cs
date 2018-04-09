using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
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

        public bool ValidaUsuarioExiste()
        {
            try
            {
                ValidaCaptura();
                if (_servicioUsuario.BuscarUsuario(txtCorreoRapido.Text.Trim()) != null)
                {
                    throw new Exception("Ya existe una cuenta con estos datos");
                }
                if (!string.IsNullOrEmpty(txtTelefonoCelularRapido.Text.Trim()))
                    if (_servicioUsuario.BuscarUsuario(txtTelefonoCelularRapido.Text.Trim()) != null)
                    {
                        throw new Exception("Ya existe una cuenta con estos datos");
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
        public void RegistraUsuario()
        {
            try
            {
                ValidaCaptura();
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

        public int IdTipoUsuario
        {
            get { return hfIdTipoUsuario.Value == string.Empty ? 0 : int.Parse(hfIdTipoUsuario.Value); }
            set
            {
                hfIdTipoUsuario.Value = value.ToString();
            }
        }

        private void Limpiar()
        {
            try
            {
                txtNombreRapido.Text = string.Empty;
                txtApRapido.Text = string.Empty;
                txtAmRapido.Text = string.Empty;
                txtCorreoRapido.Text = string.Empty;
                txtTelefonoCelularRapido.Text = string.Empty;
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
                if (Request.Params["userType"] != null)
                    IdTipoUsuario = int.Parse(Request.Params["userType"]);
                else
                    IdTipoUsuario = Session["TipoUsuario"] == null ? (int)BusinessVariables.EnumTiposUsuario.Cliente : int.Parse(Session["TipoUsuario"].ToString());
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
            try
            {

                if (txtApRapido.Text.Trim() == string.Empty)
                    throw new Exception("Apellido Paterno es un campo obligatorio.");
                if (txtAmRapido.Text.Trim() == string.Empty)
                    throw new Exception("Apellido Materno es un campo obligatorio.");
                if (txtNombreRapido.Text.Trim() == string.Empty)
                    throw new Exception("Nombre es un campo obligatorio.");

                bool capturoTelefono = false, capturoCorreo = false;
                if (txtCorreoRapido.Text.Trim() != string.Empty)
                    capturoCorreo = true;
                if (txtTelefonoCelularRapido.Text.Trim() != string.Empty)
                    capturoTelefono = true;
                if (!capturoCorreo)
                    throw new Exception("Debe capturar un correo.");
                if (!capturoTelefono)
                    txtTelefonoCelularRapido.Text = "";
                //sb.Add("Debe capturar un telefono.");
                if (!BusinessCorreo.IsValid(txtCorreoRapido.Text.Trim()) || txtCorreoRapido.Text.Trim().Contains(" "))
                {
                    throw new Exception(string.Format("Correo {0} con formato invalido", txtCorreoRapido.Text.Trim()));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GeneraNombreUsuario()
        {
            string result = null;
            try
            {
                string username = (txtNombreRapido.Text.Substring(0, 1).ToLower() + txtApRapido.Text.Trim().ToLower()).Replace(" ", string.Empty);
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
                Usuario datosUsuario = _servicioUsuario.GetUsuarioByCorreo(txtCorreoRapido.Text.Trim());
                if (datosUsuario == null)
                {
                    datosUsuario = new Usuario
                    {
                        IdTipoUsuario = IdTipoUsuario,
                        ApellidoPaterno = txtApRapido.Text.Trim(),
                        ApellidoMaterno = txtAmRapido.Text.Trim(),
                        Nombre = txtNombreRapido.Text.Trim(),
                        DirectorioActivo = false,
                        Vip = false,
                        PersonaFisica = false,
                        NombreUsuario = GeneraNombreUsuario(),
                        Password = ResolveUrl("~/ConfirmacionCuenta.aspx"),
                        Autoregistro = true,
                        Habilitado = true
                    };
                    //if (txtTelefonoCelularRapido.Text.Trim() != string.Empty)
                    datosUsuario.TelefonoUsuario = new List<TelefonoUsuario>
                        {
                            new TelefonoUsuario
                            {
                                IdTipoTelefono = (int) BusinessVariables.EnumTipoTelefono.Celular,
                                Confirmado = false,
                                Extension = string.Empty,
                                Numero = txtTelefonoCelularRapido.Text.Trim(),
                                Obligatorio = true
                            }
                        };
                    if (txtCorreoRapido.Text.Trim() != string.Empty)
                        datosUsuario.CorreoUsuario = new List<CorreoUsuario>
                        {
                            new CorreoUsuario
                            {
                                Correo = txtCorreoRapido.Text.Trim(),
                                Obligatorio = true,
                            }
                        };
                    IdUsuario = _servicioUsuario.RegistrarCliente(datosUsuario);
                }
                else
                {
                    IdUsuario = datosUsuario.Id;
                }

                Limpiar();

                if (OnAceptarModal != null)
                    OnAceptarModal();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
