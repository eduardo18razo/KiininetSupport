using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Sistema;
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
        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();
        
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

        public void LLenaDatosDeCorreo(string nombre,string apellidoPaterno,string apellidoMaterno,string correo)
        {
            try
            {
                txtNombreRapido.Text = nombre;
                txtApRapido.Text = apellidoPaterno;
                txtAmRapido.Text = apellidoMaterno;
                txtCorreoRapido.Text = correo;
                txtCorreoRapidoConfirmacion.Text = correo;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                //if (!string.IsNullOrEmpty(txtTelefonoCelularRapido.Text.Trim()))
                //    if (_servicioUsuario.BuscarUsuario(txtTelefonoCelularRapido.Text.Trim()) != null)
                //    {
                //        throw new Exception("Ya existe una cuenta con estos datos");
                //    }
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
                TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(value);
                if (tipoUsuario != null)
                {
                    if (tipoUsuario.TelefonoObligatorio)
                    {
                        lblObligatorio.Text = "*";
                        lblObligatorio.Style.Add("color", "red");
                    }
                    else
                    {
                        lblObligatorio.Text = "(opcional)";
                        lblObligatorio.Style.Remove("color");
                    }
                }
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
                txtCorreoRapidoConfirmacion.Text = string.Empty;
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
                if (txtNombreRapido.Text.Trim() == string.Empty)
                    throw new Exception("Nombre es un campo obligatorio.");

                if (txtCorreoRapido.Text.Trim() == string.Empty)
                    throw new Exception("Correo es un campo Obligatorio.");
                if (txtCorreoRapido.Text.Trim() == string.Empty)
                    throw new Exception("Debe confirmar el correo.");

                if (txtCorreoRapido.Text != txtCorreoRapidoConfirmacion.Text)
                    throw new Exception("Los correos no coinciden.");

                if (!BusinessCorreo.IsValidEmail(txtCorreoRapido.Text.Trim()) || txtCorreoRapido.Text.Trim().Contains(" "))
                {
                    throw new Exception(string.Format("Correo {0} con formato invalido", txtCorreoRapido.Text.Trim()));
                }
                if (!BusinessCorreo.IsValidEmail(txtCorreoRapidoConfirmacion.Text.Trim()) || txtCorreoRapidoConfirmacion.Text.Trim().Contains(" "))
                {
                    throw new Exception(string.Format("Correo {0} con formato invalido", txtCorreoRapidoConfirmacion.Text.Trim()));
                }

                TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(IdTipoUsuario);
                if (tipoUsuario != null)
                {
                    if (tipoUsuario.TelefonoObligatorio)
                        if (txtTelefonoCelularRapido.Text.Trim() == string.Empty)
                            throw new Exception("El Telefono es Obligatorio.");
                }
                if(txtTelefonoCelularRapido.Text.Trim() != string.Empty && txtTelefonoCelularRapido.Text.Length <10)
                    throw new Exception(string.Format("El telefono debe ser de 10 digitos."));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
                    TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(IdTipoUsuario);
                    if (tipoUsuario != null)
                    {
                        if (tipoUsuario.TelefonoObligatorio)
                        {
                            datosUsuario.TelefonoUsuario = new List<TelefonoUsuario>
                            {
                                new TelefonoUsuario
                                {
                                    IdTipoTelefono = (int) BusinessVariables.EnumTipoTelefono.Celular,
                                    Confirmado = false,
                                    Extension = string.Empty,
                                    Numero = txtTelefonoCelularRapido.Text.Trim(),
                                    Principal = true
                                }
                            };
                        }
                        else if (txtTelefonoCelularRapido.Text.Trim() != string.Empty)
                        {
                            datosUsuario.TelefonoUsuario = new List<TelefonoUsuario>
                            {
                                new TelefonoUsuario
                                {
                                    IdTipoTelefono = (int) BusinessVariables.EnumTipoTelefono.Celular,
                                    Confirmado = false,
                                    Extension = string.Empty,
                                    Numero = txtTelefonoCelularRapido.Text.Trim(),
                                    Principal = true
                                }
                            };
                        }
                    }


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