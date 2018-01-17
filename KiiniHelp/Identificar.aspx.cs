using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp
{
    public partial class Identificar : Page
    {
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //lblBrandingModal.Text = WebConfigurationManager.AppSettings["Brand"];
                Alerta = new List<string>();
                if (Request.Params["confirmacionalta"] != null)
                {
                    string[] values = Request.Params["confirmacionalta"].Split('_');
                    if (!_servicioUsuarios.ValidaConfirmacion(int.Parse(values[0]), values[1]))
                    {
                        Alerta = new List<string> { "Link Invalido !!!" };
                    }
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

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    List<Usuario> usuarios = _servicioUsuarios.BuscarUsuarios(txtUserName.Text.Trim());
                    if (usuarios.Any())
                    {
                        if (!_servicioUsuarios.ObtenerDetalleUsuario(usuarios.First().Id).Activo)
                        {
                            throw new Exception("Debe primero confirmar su cuenta");
                        }
                        Response.Redirect("~/FrmRecuperar.aspx?ldata=" + QueryString.Encrypt(usuarios.First().Id.ToString()));
                    }
                    else
                    {
                        throw new Exception("Usuario no valido !!!");
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