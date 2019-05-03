using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServicePuesto;
using KiiniHelp.ServiceSistemaRol;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Usuarios
{
    public partial class UcAltaUsuarioMoral : UserControl, IControllerModal
    {
        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();

        private readonly ServiceRolesClient _servicioSistemaRol = new ServiceRolesClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServicePuestoClient _servicioPuesto = new ServicePuestoClient();

        private List<string> _lstError = new List<string>();

        public bool Alta
        {
            get { return Convert.ToBoolean(hfAlta.Value); }
            set
            {
                ddlTipoUsuario.Enabled = value;
                hfAlta.Value = value.ToString();
                if (!value)
                {
                    btnModalDatosGenerales.CssClass = "btn btn-primary";
                    btnModalOrganizacion.CssClass = "btn btn-primary";
                    btnModalUbicacion.CssClass = "btn btn-primary";
                    btnModalRoles.CssClass = "btn btn-primary";
                    btnModalGrupos.CssClass = "btn btn-primary";
                }
            }
        }

        public int IdUsuario
        {
            get { return Convert.ToInt32(hfIdUsuario.Value); }
            set
            {
                hfIdUsuario.Value = value.ToString();
                SetUser();
            }
        }

        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuario.SelectedValue); }
            set
            {
                ddlTipoUsuario.SelectedValue = value.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(null, null);
                ddlTipoUsuario.Enabled = false;
                UcConsultaOrganizacion.IdTipoUsuario = value;
                UcConsultaUbicaciones.IdTipoUsuario = value;
            }
        }


        #region Alerts
        private List<string> AlertaGeneral
        {
            set
            {
                panelAlertaGeneral.Visible = value.Any();
                if (!panelAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value.Select(s => new { Detalle = s }).ToList();
                rptErrorGeneral.DataBind();
            }
        }

        private List<string> AlertaDatosGenerales
        {
            set
            {
                panelAlertaModalDg.Visible = value.Any();
                if (!panelAlertaModalDg.Visible) return;
                rptErrorDg.DataSource = value;
                rptErrorDg.DataBind();
            }
        }

        private List<string> AlertaRoles
        {
            set
            {
                panelAlertaRoles.Visible = value.Any();
                if (!panelAlertaRoles.Visible) return;
                rptErrorRoles.DataSource = value;
                rptErrorRoles.DataBind();
            }
        }


        #endregion Alerts

        #region Metodos
        private void LlenaCombos(bool usuariosResidentes)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<TipoUsuario> lstTipoUsuario = usuariosResidentes ? _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true) : _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true);
                    Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
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

        private void LimpiarPantalla()
        {
            try
            {
                Session["UsuarioGrupo"] = null;
                txtAp.Text = string.Empty;
                txtAm.Text = string.Empty;
                txtNombre.Text = string.Empty;
                txtUserName.Text = string.Empty;
                Metodos.LimpiarCombo(ddlPuesto);
                chkVip.Checked = false;
                chkDirectoriActivo.Checked = false;
                btnModalDatosGenerales.CssClass = "btn btn-primary";
                btnModalOrganizacion.CssClass = "btn btn-primary disabled";
                btnModalUbicacion.CssClass = "btn btn-primary disabled";
                btnModalRoles.CssClass = "btn btn-primary disabled";
                btnModalGrupos.CssClass = "btn btn-primary disabled";
                foreach (ListItem item in chklbxRoles.Items)
                {
                    item.Selected = false;
                }
                upRoles.Update();
                AsociarGrupoUsuario.Limpiar();
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
        #region Validaciones

        private void ValidaCapturaDatosGenerales()
        {
            StringBuilder sb = new StringBuilder();

            if (txtAp.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Apellido Paterno es un campo obligatorio.</li>");
            if (txtAm.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Apellido Materno es un campo obligatorio.</li>");
            if (txtNombre.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Nombre es un campo obligatorio.</li>");
            if (txtNombre.Text.Trim() == string.Empty)
                sb.AppendLine("<li>Nombre de Usuario es un campo obligatorio.</li>");

            List<ParametrosTelefonos> lstParamTelefonos = _servicioParametros.TelefonosObligatorios(Convert.ToInt32(ddlTipoUsuario.SelectedValue));
            List<TelefonoUsuario> telefonoUsuario = new List<TelefonoUsuario>();
            foreach (RepeaterItem item in rptTelefonos.Items)
            {
                Label tipoTelefono = (Label)item.FindControl("lblTipotelefono");
                TextBox numero = (TextBox)item.FindControl("txtNumero");
                TextBox extension = (TextBox)item.FindControl("txtExtension");
                if (tipoTelefono != null && numero != null && extension != null)
                {
                    telefonoUsuario.Add(new TelefonoUsuario { IdTipoTelefono = Convert.ToInt32(tipoTelefono.Text.Trim()), Numero = numero.Text.Trim(), Extension = extension.Text.Trim() });
                }
            }

            foreach (TelefonoUsuario telefono in telefonoUsuario)
            {
                if (telefono.Numero.Trim() != string.Empty && telefono.Numero.Length < 10)
                {
                    sb.Append(string.Format("<li>El telefono {0} debe ser de 10 digitos.</li>", telefono.Numero));
                }
                ParametrosTelefonos parametroTipoTelefono = lstParamTelefonos.Single(s => s.IdTipoTelefono == telefono.IdTipoTelefono);
                if (telefonoUsuario.Count(c => c.IdTipoTelefono == telefono.IdTipoTelefono && c.Numero.Trim() != string.Empty) < parametroTipoTelefono.Obligatorios)
                {
                    sb.AppendLine(String.Format("<li>Debe capturar al menos {0} telefono(s) de {1}.</li>",
                        parametroTipoTelefono.Obligatorios, parametroTipoTelefono.TipoTelefono.Descripcion));
                    break;
                }
            }
            var sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            List<TextBox> lstCorreos = rptCorreos.Items.Cast<RepeaterItem>().Select(item => (TextBox)item.FindControl("txtCorreo")).Where(w => w.Text != string.Empty).ToList();
            foreach (TextBox txtMail in lstCorreos)
            {
                if (!Regex.IsMatch(txtMail.Text.Trim(), sFormato))
                {
                    if (Regex.Replace(txtMail.Text.Trim(), sFormato, String.Empty).Length != 0)
                    {
                        sb.AppendLine(string.Format("Correo {0} con formato invalido", txtMail.Text.Trim()));
                    }
                }
            }

            List<CorreoUsuario> correos = rptCorreos.Items.Cast<RepeaterItem>().Select(item => (TextBox)item.FindControl("txtCorreo")).Where(correo => correo != null & correo.Text.Trim() != string.Empty).Select(correo => new CorreoUsuario { Correo = correo.Text.Trim() }).ToList();

            //TODO: Implementar metodo unico
            TipoUsuario paramCorreos = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(false).SingleOrDefault(s => s.Id == int.Parse(ddlTipoUsuario.SelectedValue));
            if (paramCorreos != null && (correos.Count(c => c.Correo != string.Empty) < paramCorreos.CorreosObligatorios))
                sb.AppendLine(String.Format("<li>Debe captura al menos {0} correo(s).</li>.", paramCorreos.CorreosObligatorios));

            if (sb.ToString() != string.Empty)
            {
                sb.Append("</ul>");
                sb.Insert(0, "<ul>");
                sb.Insert(0, "<h3>Datos Generales</h3>");
                throw new Exception(sb.ToString());
            }
        }

        private void ValidaCapturaRoles()
        {
            StringBuilder sb = new StringBuilder();


            if (!chklbxRoles.Items.Cast<ListItem>().Any(item => item.Selected))
                sb.AppendLine("<li>Debe seleccionar un Rol.</li>");

            if (sb.ToString() != string.Empty)
            {
                sb.Append("</ul>");
                sb.Insert(0, "<ul>");
                sb.Insert(0, "<h3>Roles</h3>");
                throw new Exception(sb.ToString());
            }
        }

        private void ValidaCapturaGrupos()
        {
            StringBuilder sb = new StringBuilder();


            if (!AsociarGrupoUsuario.ValidaCapturaGrupos())
                sb.AppendLine("<li>Debe asignar al menos un Grupo.</li>");

            if (sb.ToString() != string.Empty)
            {
                sb.Append("</ul>");
                sb.Insert(0, "<ul>");
                sb.Insert(0, "<h3>Grupos</h3>");
                throw new Exception(sb.ToString());
            }
        }

        #endregion Validaciones

        #endregion Metodos

        private void SetUser()
        {
            try
            {
                Usuario user = _servicioUsuarios.ObtenerDetalleUsuario(IdUsuario);
                if (user != null)
                {
                    LlenaCombos(false);
                    ddlTipoUsuario.SelectedValue = user.IdTipoUsuario.ToString();
                    ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                    txtAp.Text = user.ApellidoPaterno;
                    txtAm.Text = user.ApellidoMaterno;
                    txtNombre.Text = user.Nombre;
                    Metodos.LlenaComboCatalogo(ddlPuesto, _servicioPuesto.ObtenerPuestosByTipoUsuario(IdTipoUsuario, true));
                    if (user.IdPuesto != null)
                        ddlPuesto.SelectedValue = user.IdPuesto.ToString();
                    chkVip.Checked = user.Vip;
                    chkDirectoriActivo.Checked = user.DirectorioActivo;
                    int contadorCasa, contadorCel, contadorOficina;
                    foreach (TelefonoUsuario telefonoUsuario in user.TelefonoUsuario)
                    {
                        foreach (RepeaterItem item in rptTelefonos.Items)
                        {
                            Label lblTipoTelefono = (Label)item.FindControl("lblTipotelefono");
                            if (telefonoUsuario.IdTipoTelefono == Convert.ToInt32(lblTipoTelefono.Text))
                            {
                                TextBox txtNumero = (TextBox)item.FindControl("txtNumero");
                                if (txtNumero.Text == string.Empty)
                                {
                                    TextBox txtExtencion = (TextBox)item.FindControl("txtExtension");
                                    txtNumero.Text = telefonoUsuario.Numero;
                                    txtExtencion.Text = telefonoUsuario.Extension;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (CorreoUsuario correoUsuario in user.CorreoUsuario)
                    {
                        foreach (RepeaterItem item in rptCorreos.Items)
                        {
                            TextBox txtCorreo = (TextBox)item.FindControl("txtCorreo");
                            if (txtCorreo.Text != string.Empty) continue;
                            txtCorreo.Text = correoUsuario.Correo;
                            break;
                        }
                    }
                    UcConsultaOrganizacion.OrganizacionSeleccionada = user.IdOrganizacion;
                    UcConsultaUbicaciones.UbicacionSeleccionada = user.IdUbicacion;
                    foreach (UsuarioRol usuarioRol in user.UsuarioRol)
                    {
                        foreach (ListItem item in chklbxRoles.Items)
                        {
                            if (item.Value == usuarioRol.RolTipoUsuario.IdRol.ToString())
                            {
                                item.Selected = true;
                                break;
                            }
                        }
                    }
                    chkKbxRoles_OnSelectedIndexChanged(chklbxRoles, null);
                    upRoles.Update();
                    List<GrupoUsuarioInventarioArbol> lst = user.UsuarioGrupo.Select(usuarioGrupo => new GrupoUsuarioInventarioArbol
                    {
                        IdGrupoUsuario = usuarioGrupo.IdGrupoUsuario,
                        IdRol = usuarioGrupo.IdRol,
                        SubGrupoUsuario = usuarioGrupo.SubGrupoUsuario,
                        GrupoUsuario = usuarioGrupo.GrupoUsuario
                    }).ToList();
                    AsociarGrupoUsuario.GruposAsignados = lst;
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
                AlertaDatosGenerales = new List<string>();
                AlertaRoles = new List<string>();

                ucAltaPuesto.OnAceptarModal += UcAltaPuestoOnOnAceptarModal;
                ucAltaPuesto.OnCancelarModal += UcAltaPuestoOnOnCancelarModal;

                UcConsultaOrganizacion.Modal = true;
                UcConsultaOrganizacion.OnSeleccionOrganizacionModal += ucOrganizacion_OnAceptarModal;
                UcConsultaOrganizacion.ModalName = "#modalOrganizacion";
                UcConsultaOrganizacion.OnCancelarModal += ucOrganizacion_OnCancelarModal;

                UcConsultaUbicaciones.Modal = true;
                UcConsultaUbicaciones.ModalName = "#modalUbicacion";
                UcConsultaUbicaciones.OnSeleccionUbicacionModal += UcUbicacion_OnAceptarModal;
                UcConsultaUbicaciones.OnCancelarModal += UcUbicacion_OnCancelarModal;

                AsociarGrupoUsuario.AsignacionAutomatica = false;
                if (!IsPostBack)
                {
                    if (Request.QueryString["all"] != null && Request.QueryString["all"] != string.Empty)
                        LlenaCombos(false);
                    else
                        LlenaCombos(true);
                    Session["UsuarioTemporal"] = null;
                    Session["UsuarioGrupo"] = null;
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

        protected void chkKbxRoles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chklbxRoles.Items.Cast<ListItem>().Any(item => item.Selected && int.Parse(item.Value) == (int)BusinessVariables.EnumRoles.Administrador))
                {
                    foreach (ListItem item in chklbxRoles.Items.Cast<ListItem>())
                    {
                        item.Selected = int.Parse(item.Value) == (int)BusinessVariables.EnumRoles.Administrador;
                    }
                }
                List<object> lst = new List<object>();

                foreach (ListItem item in chklbxRoles.Items.Cast<ListItem>())
                {
                    AsociarGrupoUsuario.HabilitaGrupos(Convert.ToInt32(item.Value), item.Selected);
                }
                lst.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new TipoGrupo { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione });
                Session["UsuarioGrupo"] = new List<UsuarioGrupo>();
                btnModalGrupos.Visible = true;
                upGrupos.Update();
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

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                    rptTelefonos.DataSource = _servicioParametros.ObtenerTelefonosParametrosIdTipoUsuario(idTipoUsuario, false);
                    rptTelefonos.DataBind();

                    rptCorreos.DataSource = _servicioParametros.ObtenerCorreosParametrosIdTipoUsuario(idTipoUsuario, false);
                    rptCorreos.DataBind();

                    Metodos.LlenaListBoxCatalogo(chklbxRoles, _servicioSistemaRol.ObtenerRoles(idTipoUsuario, false));
                    AsociarGrupoUsuario.IdTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                    Session["UsuarioTemporal"] = new Usuario();
                    LimpiarPantalla();
                    divDatos.Visible = true;

                    Metodos.LlenaComboCatalogo(ddlPuesto, _servicioPuesto.ObtenerPuestosByTipoUsuario(IdTipoUsuario, true));
                    if (_servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(int.Parse(ddlTipoUsuario.SelectedValue)).EsMoral)
                    {
                        divPuesto.Visible = true;
                        btnModalOrganizacion.Text = "Organización";
                        btnModalUbicacion.Text = "Ubicación";
                    }
                    else
                    {
                        divPuesto.Visible = false;
                        btnModalOrganizacion.Text = "Actividad";
                        btnModalUbicacion.Text = "Direccion";
                    }
                }
                else
                {
                    LimpiarPantalla();
                    divDatos.Visible = false;
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

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un tipo de usuario.<br>");
                ValidaCapturaDatosGenerales();
                ValidaCapturaRoles();
                ValidaCapturaGrupos();
                Usuario usuario = new Usuario
                {
                    IdTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue),
                    ApellidoPaterno = txtAp.Text.Trim(),
                    ApellidoMaterno = txtAm.Text.Trim(),
                    Nombre = txtNombre.Text.Trim(),
                    DirectorioActivo = chkDirectoriActivo.Checked,
                    IdPuesto = ddlPuesto.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione ? (int?)null : Convert.ToInt32(ddlPuesto.SelectedValue),
                    Vip = chkVip.Checked,
                    NombreUsuario = txtUserName.Text.Trim(),
                    Password = ResolveUrl("~/ConfirmacionCuenta.aspx"),
                    Habilitado = true
                };
                List<ParametrosTelefonos> lstParamTelefonos = _servicioParametros.TelefonosObligatorios(Convert.ToInt32(ddlTipoUsuario.SelectedValue));
                int telefonosObligatoriosCasa = lstParamTelefonos.Count(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Casa);
                int telefonosObligatoriosCelular = lstParamTelefonos.Count(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular);
                int telefonosObligatoriosOficina = lstParamTelefonos.Count(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Oficina);
                int contadorCasa = 0;
                int contadorCelular = 0;
                int contadorOficina = 0;
                usuario.TelefonoUsuario = new List<TelefonoUsuario>();
                foreach (RepeaterItem item in rptTelefonos.Items)
                {
                    Label tipoTelefono = (Label)item.FindControl("lblTipotelefono");
                    TextBox numero = (TextBox)item.FindControl("txtNumero");
                    TextBox extension = (TextBox)item.FindControl("txtExtension");
                    bool obligatorio = false;
                    if (tipoTelefono != null && numero != null && extension != null)
                    {
                        switch (Convert.ToInt32(tipoTelefono.Text.Trim()))
                        {
                            case (int)BusinessVariables.EnumTipoTelefono.Casa:
                                if (contadorCasa < telefonosObligatoriosCasa)
                                {
                                    obligatorio = true;
                                    contadorCasa++;
                                }
                                break;
                            case (int)BusinessVariables.EnumTipoTelefono.Celular:
                                if (contadorCelular < telefonosObligatoriosCelular)
                                {
                                    obligatorio = true;
                                    contadorCelular++;
                                }
                                break;
                            case (int)BusinessVariables.EnumTipoTelefono.Oficina:
                                if (contadorOficina < telefonosObligatoriosOficina)
                                {
                                    obligatorio = true;
                                    contadorOficina++;
                                }
                                break;
                        }

                        usuario.TelefonoUsuario.Add(new TelefonoUsuario
                        {
                            IdTipoTelefono = Convert.ToInt32(tipoTelefono.Text.Trim()),
                            Numero = numero.Text.Trim(),
                            Extension = extension.Text.Trim(),
                            Obligatorio = numero.CssClass.Contains("obligatorio"),
                            Confirmado = false
                        });
                    }
                }
                usuario.CorreoUsuario = new List<CorreoUsuario>();
                foreach (TextBox correo in rptCorreos.Items.Cast<RepeaterItem>().Select(item => (TextBox)item.FindControl("txtCorreo")).Where(correo => correo != null & correo.Text.Trim() != string.Empty))
                {
                    usuario.CorreoUsuario.Add(new CorreoUsuario { Correo = correo.Text.Trim(), Obligatorio = correo.CssClass.Contains("obligatorio") });
                }

                usuario.IdOrganizacion = UcConsultaOrganizacion.OrganizacionSeleccionada;
                usuario.IdUbicacion = UcConsultaUbicaciones.UbicacionSeleccionada;

                #region Rol
                usuario.UsuarioRol = new List<UsuarioRol>();
                foreach (ListItem item in chklbxRoles.Items.Cast<ListItem>().Where(item => item.Selected))
                {
                    usuario.UsuarioRol.Add(new UsuarioRol { RolTipoUsuario = new RolTipoUsuario { IdRol = Convert.ToInt32(item.Value), IdTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue) } });
                }

                #endregion Rol

                #region Grupo

                usuario.UsuarioGrupo = new List<UsuarioGrupo>();

                foreach (RepeaterItem item in AsociarGrupoUsuario.GruposAsociados)
                {
                    Label lblIdGrupoUsuario = (Label)item.FindControl("lblIdGrupoUsuario");
                    Label lblIdRol = (Label)item.FindControl("lblIdTipoSubGrupo");
                    Label lblIdSubGrupoUsuario = (Label)item.FindControl("lblIdSubGrupo");
                    if (lblIdGrupoUsuario != null && lblIdRol != null && lblIdSubGrupoUsuario != null)
                    {
                        usuario.UsuarioGrupo.Add(new UsuarioGrupo
                        {
                            IdUsuario = Alta ? 0 : IdUsuario,
                            IdGrupoUsuario = Convert.ToInt32(lblIdGrupoUsuario.Text),
                            IdRol = Convert.ToInt32(lblIdRol.Text),
                            IdSubGrupoUsuario = lblIdSubGrupoUsuario.Text.Trim() == string.Empty ? (int?)null : Convert.ToInt32(lblIdSubGrupoUsuario.Text)
                        });
                    }
                }

                #endregion Grupos

                if (Alta)
                    _servicioUsuarios.GuardarUsuario(usuario);
                else
                {
                    _servicioUsuarios.ActualizarUsuario(IdUsuario, usuario);
                }

                LimpiarPantalla();
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

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ddlTipoUsuario.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
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
                AlertaGeneral = _lstError;
            }
        }
        protected void btnAddPuesto_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucAltaPuesto.EsAlta = true;
                ucAltaPuesto.IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAreas\");", true);
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

        protected void txtAp_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text.Trim().Length <= 0)
                    return;
                string username = txtUserName.Text.Trim() == string.Empty ? (txtNombre.Text.Substring(0, 1).ToLower() + txtAp.Text.Trim().ToLower()).Replace(" ", string.Empty) : txtUserName.Text.Trim();
                int limite = 2;
                if (_servicioUsuarios.ValidaUserName(username))
                {
                    for (int i = 1; i < limite; i++)
                    {
                        string tmpUsername = username + i;
                        if (!_servicioUsuarios.ValidaUserName(tmpUsername))
                        {
                            username = tmpUsername;
                            break;
                        }
                        limite++;
                    }
                }
                txtUserName.Text = username;
                if (txtAp.ID == ((TextBox)sender).ID)
                {
                    txtAm.Focus();
                }
                else if (txtNombre.ID == ((TextBox)sender).ID)
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

        #region botones cerrar Cancelar
        protected void btnAceptarDatosGenerales_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaDatosGenerales();
                if (Alta)
                {
                    btnModalDatosGenerales.CssClass = "btn btn-success";
                    btnModalOrganizacion.CssClass = "btn btn-primary";
                    btnModalUbicacion.CssClass = "btn btn-primary";
                    btnModalUbicacion.Enabled = false;
                    btnModalRoles.CssClass = "btn btn-primary";
                    btnModalRoles.Enabled = false;
                    btnModalGrupos.CssClass = "btn btn-primary";
                    btnModalGrupos.Enabled = false;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDatosGenerales\");", true);
                upOrganizacion.Update();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaDatosGenerales = _lstError;
            }
        }
        protected void btnCerrarDatosGenerales_OnClick(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalDatosGenerales\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaDatosGenerales = _lstError;
            }
        }
        protected void btnAceptarRoles_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaRoles();
                if (Alta)
                {
                    btnModalRoles.CssClass = "btn btn-success";
                    btnModalGrupos.Enabled = true;
                    btnModalGrupos.CssClass = "btn btn-primary";
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalRoles\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaRoles = _lstError;
            }
        }
        protected void btnCerrarRoles_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Alta)
                {
                    btnModalRoles.CssClass = "btn btn-primary";
                    btnModalGrupos.Enabled = false;
                    btnModalGrupos.CssClass = "btn btn-primary";
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalRoles\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaRoles = _lstError;
            }
        }

        protected void btnAceptarGrupos_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!AsociarGrupoUsuario.ValidaCapturaGrupos()) return;
                btnModalGrupos.CssClass = "btn btn-success";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalGrupos\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);

            }
        }

        protected void btnCerrarGrupos_OnClick(object sender, EventArgs e)
        {
            try
            {
                //if (!AsociarGrupoUsuario.ValidaCapturaGrupos()) return;
                btnModalGrupos.CssClass = "btn btn-primary";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalGrupos\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);

            }
        }
        #endregion botones cerrar Cancelar

        #region Delegados


        private void UcAltaPuestoOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAreas\");", true);
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

        private void UcAltaPuestoOnOnAceptarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAreas\");", true);
                Metodos.LlenaComboCatalogo(ddlPuesto, _servicioPuesto.ObtenerPuestosByTipoUsuario(IdTipoUsuario, true));
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

        void UcUbicacion_OnAceptarModal()
        {
            try
            {
                if (Alta)
                {
                    btnModalUbicacion.CssClass = "btn btn-success";
                    btnModalRoles.CssClass = "btn btn-primary";
                    btnModalRoles.Enabled = true;
                    btnModalGrupos.CssClass = "btn btn-primary";
                    btnModalGrupos.Enabled = false;
                    btnModalRoles.CssClass = "btn btn-primary";
                }
                upRoles.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalUbicacion\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
            }

        }
        void UcUbicacion_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalUbicacion\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                //UcUbicacion.AlertaUbicacion = _lstError;
            }
        }
        void ucOrganizacion_OnAceptarModal()
        {
            try
            {
                if (Alta)
                {
                    btnModalOrganizacion.CssClass = "btn btn-success";
                    btnModalUbicacion.CssClass = "btn btn-primary";
                    btnModalUbicacion.Enabled = true;
                    btnModalRoles.CssClass = "btn btn-primary";
                    btnModalRoles.Enabled = false;
                    btnModalGrupos.CssClass = "btn btn-primary";
                    btnModalGrupos.Enabled = false;
                }
                upUbicacion.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalOrganizacion\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
            }
        }
        void ucOrganizacion_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalOrganizacion\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                //ucOrganizacion.AlertaOrganizacion = _lstError;

            }
        }
        #endregion Delegados

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
    }
}