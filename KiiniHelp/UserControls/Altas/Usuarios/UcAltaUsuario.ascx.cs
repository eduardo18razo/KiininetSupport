using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServicePuesto;
using KiiniHelp.ServiceSistemaDomicilio;
using KiiniHelp.ServiceSistemaTipoTelefono;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Usuarios
{
    public partial class UcAltaUsuario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
        private readonly ServiceUsuariosClient _servicioUsuarios = new ServiceUsuariosClient();
        private readonly ServicePuestoClient _servicioPuesto = new ServicePuestoClient();
        private readonly ServiceTipoTelefonoClient _servicioTipoTelefono = new ServiceTipoTelefonoClient();
        private readonly ServiceDomicilioSistemaClient _servicioSistemaDomicilio = new ServiceDomicilioSistemaClient();
        private List<string> _lstError = new List<string>();
        UsuariosMaster _mp;

        public bool Alta
        {
            get { return Convert.ToBoolean(hfAlta.Value); }
            set
            {
                ddlTipoUsuario.Enabled = value;
                hfAlta.Value = value.ToString();
                hfGeneraUsuario.Value = value.ToString();

                btnCambiarImagen.Visible = false;
                FileUpload1.Enabled = false;

                DesbloqueaBotones();

                if (!value)
                {
                    btnModalOrganizacion.CssClass = "btn btn-primary";
                    btnModalUbicacion.CssClass = "btn btn-primary";
                }
            }
        }

        private bool EsDetalle
        {
            get { return bool.Parse(hdEsDetalle.Value); }
            set { hdEsDetalle.Value = value.ToString(); }
        }

        private bool Habilitado
        {
            get { return bool.Parse(hfHabilitado.Value); }
            set { hfHabilitado.Value = value.ToString(); }
        }

        private bool EditarDetalle
        {
            get { return bool.Parse(hfEditaDetalle.Value); }
            set { hfEditaDetalle.Value = value.ToString(); }
        }

        public int IdUsuario
        {
            get { return hfIdUsuario.Value.Trim() == string.Empty ? 0 : Convert.ToInt32(hfIdUsuario.Value); }
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
                ucAltaOrganizaciones.IdTipoUsuario = value;
                ucAltaUbicaciones.IdTipoUsuario = value;
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
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert",
                        "ErrorAlert('Error','" + error + "');", true);
                }
            }
        }

        private List<TelefonoUsuario> TelefonosUsuario
        {
            get
            {
                List<TelefonoUsuario> result = Session["TelefonosUsuario"] == null ? new List<TelefonoUsuario>() : (List<TelefonoUsuario>)Session["TelefonosUsuario"];

                return result;
            }
            set { Session["TelefonosUsuario"] = value; }
        }

        private List<CorreoUsuario> CorreosUsuario
        {
            get
            {
                List<CorreoUsuario> result = Session["CorreoUsuario"] == null ? new List<CorreoUsuario>() : (List<CorreoUsuario>)Session["CorreoUsuario"];
                //foreach (TextBox correo in rptCorreos.Items.Cast<RepeaterItem>().Select(item => (TextBox)item.FindControl("txtCorreo")).Where(correo => correo != null & correo.Text.Trim() != string.Empty))
                //{
                //    result.Add(new CorreoUsuario
                //    {
                //        Correo = correo.Text.Trim(),
                //        Obligatorio = correo.CssClass.Contains("obligatorio")
                //    });
                //}
                return result;
            }
            set { Session["CorreoUsuario"] = value; }
        }

        private List<HelperAsignacionRol> GruposUsuario
        {
            get
            {
                if (Session["GruposUsuario"] == null)
                    Session["GruposUsuario"] = new List<HelperAsignacionRol>();
                return (List<HelperAsignacionRol>)Session["GruposUsuario"];
            }
            set { Session["GruposUsuario"] = value; }
        }

        #region Metodos

        private void AgregarGrupo(List<HelperAsignacionRol> grupoAgregar)
        {
            try
            {
                foreach (HelperAsignacionRol asignacionNueva in grupoAgregar)
                {
                    if (GruposUsuario.Any(a => a.DescripcionRol == asignacionNueva.DescripcionRol))
                    {
                        HelperAsignacionRol rolExistente = GruposUsuario.Single(a => a.DescripcionRol == asignacionNueva.DescripcionRol);
                        if (rolExistente != null)
                        {
                            foreach (HelperAsignacionGrupoUsuarios gpoNuevo in asignacionNueva.Grupos)
                            {
                                if (rolExistente.Grupos.Any(a => a.IdGrupo == gpoNuevo.IdGrupo))
                                {
                                    HelperAsignacionGrupoUsuarios grupoExistente = rolExistente.Grupos.Single(a => a.IdGrupo == gpoNuevo.IdGrupo);
                                    if (gpoNuevo.SubGrupos != null)
                                        foreach (HelperSubGurpoUsuario subgrupoNuevo in gpoNuevo.SubGrupos)
                                        {
                                            if (!grupoExistente.SubGrupos.Any(a => a.Id == subgrupoNuevo.Id))
                                            {
                                                grupoExistente.SubGrupos.Add(new HelperSubGurpoUsuario
                                                {
                                                    Id = subgrupoNuevo.Id,
                                                    Descripcion = subgrupoNuevo.Descripcion
                                                });
                                            }
                                        }
                                }
                                else
                                {
                                    rolExistente.Grupos.Add(gpoNuevo);
                                }
                            }
                        }
                    }
                    else
                    {
                        GruposUsuario.Add(asignacionNueva);
                    }
                }

                if (GruposUsuario.Any(a => a.IdRol == (int)BusinessVariables.EnumRoles.Administrador))
                    GruposUsuario.RemoveAll(r => r.IdRol == (int)BusinessVariables.EnumRoles.AccesoAnalíticos);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void EliminarRol(int idRol)
        {
            try
            {
                GruposUsuario.Remove(GruposUsuario.Single(s => s.IdRol == idRol));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void EliminarGrupo(int idRol, int idGrupo, int? idSubGrupo, bool esSubGrupo)
        {
            try
            {
                if (esSubGrupo)
                {
                    HelperSubGurpoUsuario rolEliminar = GruposUsuario.Single(s => s.IdRol == idRol).Grupos.Single(s => s.IdGrupo == idGrupo).SubGrupos.Single(s => s.Id == idSubGrupo);
                    if (rolEliminar != null)
                    {
                        GruposUsuario.Single(s => s.IdRol == idRol).Grupos.Single(s => s.IdGrupo == idGrupo).SubGrupos.Remove(rolEliminar);
                    }
                }
                else
                {
                    HelperAsignacionGrupoUsuarios grupoEliminar = GruposUsuario.Single(s => s.IdRol == idRol).Grupos.Single(s => s.IdGrupo == idGrupo);
                    if (grupoEliminar != null)
                    {
                        GruposUsuario.Single(s => s.IdRol == idRol).Grupos.Remove(grupoEliminar);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaCombos(bool usuariosResidentes)
        {
            try
            {
                if (!IsPostBack)
                {
                    List<TipoUsuario> lstTipoUsuario = usuariosResidentes
                        ? _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true, false)
                        : _servicioSistemaTipoUsuario.ObtenerTiposUsuario(true);
                    Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
                    Metodos.LlenaComboCatalogo(ddlTipoTelefono, _servicioTipoTelefono.ObtenerTiposTelefono(true));
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

        private void LimpiarPantalla()
        {
            try
            {
                Session["UsuarioTemporal"] = null;
                Session["UsuarioGrupo"] = null;
                GruposUsuario = null;
                txtAp.Text = string.Empty;
                txtAm.Text = string.Empty;
                txtNombre.Text = string.Empty;
                txtUserName.Text = string.Empty;
                Metodos.LimpiarCombo(ddlPuesto);
                chkVip.Checked = false;
                chkDirectoriActivo.Checked = false;
                ucRolGrupo.Limpiar();
                ucAltaOrganizaciones.OrganizacionSeleccionada = new Organizacion();
                ucAltaUbicaciones.UbicacionSeleccionada = new Ubicacion();
                rptOrganizacion.DataSource = null;
                rptOrganizacion.DataBind();
                rptUbicacion.DataSource = null;
                rptUbicacion.DataBind();
                GruposUsuario = null;
                rptRoles.DataSource = GruposUsuario;
                rptRoles.DataBind();

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
                    imgPerfil.ImageUrl = user.Foto != null ? "~/DisplayImages.ashx?id=" + user.Id : "~/assets/images/profiles/profile-1.png";
                    lblFechaUltimoAcceso.Text = user.FechaUltimoAccesoExito;
                    txtAp.Text = user.ApellidoPaterno;
                    txtAm.Text = user.ApellidoMaterno;
                    txtNombre.Text = user.Nombre;
                    txtUserName.Text = user.NombreUsuario;
                    txtUserName.ReadOnly = true;

                    Metodos.LlenaComboCatalogo(ddlPuesto, _servicioPuesto.ObtenerPuestosByTipoUsuario(IdTipoUsuario, true));
                    if (user.IdPuesto != null)
                        ddlPuesto.SelectedValue = user.IdPuesto.ToString();
                    chkVip.Checked = user.Vip;
                    chkDirectoriActivo.Checked = user.DirectorioActivo;
                    chkPersonaFisica.Checked = user.PersonaFisica;

                    TelefonosUsuario = user.TelefonoUsuario == null ? new List<TelefonoUsuario>() : user.TelefonoUsuario.OrderBy(o => o.IdTipoTelefono).ToList();
                    LlenaTelefonosUsuarios();

                    CorreosUsuario = user.CorreoUsuario.ToList();
                    LlenaCorreosUsuarios();

                    ucAltaOrganizaciones.OrganizacionSeleccionada = user.Organizacion;
                    MuestraOrganizacion(new List<Organizacion>());


                    ucAltaUbicaciones.UbicacionSeleccionada = user.Ubicacion;
                    TipoUsuario tipoUsuario = _servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(Convert.ToInt32(ddlTipoUsuario.SelectedValue));
                    if (tipoUsuario.Domicilio)
                    {
                        divDomicilio.Visible = true;
                        divUbicacion.Visible = false;
                        if (user.Ubicacion.Campus.Domicilio.Any())
                        {
                            txtCalle.Text = user.Ubicacion.Campus.Domicilio.First().Calle;
                            txtNoExt.Text = user.Ubicacion.Campus.Domicilio.First().NoExt;
                            txtNoInt.Text = user.Ubicacion.Campus.Domicilio.First().NoInt;
                            txtCp.Text = user.Ubicacion.Campus.Domicilio.First().Colonia.CP.ToString();
                            txtCp_OnTextChanged(null, null);
                            ddlColonia.SelectedValue = user.Ubicacion.Campus.Domicilio.First().IdColonia.ToString();
                            ddlColonia_OnSelectedIndexChanged(ddlColonia, null);
                        }
                    }
                    else
                    {
                        divDomicilio.Visible = false;
                        divUbicacion.Visible = true;
                        MuestraUbicacion(new List<Ubicacion>());
                    }

                    List<HelperAsignacionRol> lstRoles = user.UsuarioGrupo.Select(s => new { s.IdRol, s.Rol.Descripcion }).Distinct().Select(typeAnonymous => new HelperAsignacionRol { IdRol = typeAnonymous.IdRol, DescripcionRol = typeAnonymous.Descripcion, Grupos = new List<HelperAsignacionGrupoUsuarios>() }).ToList();
                    foreach (UsuarioGrupo usuarioGrupo in user.UsuarioGrupo)
                    {

                        HelperAsignacionRol rolActivo = lstRoles.Single(s => s.IdRol == usuarioGrupo.IdRol);
                        HelperAsignacionGrupoUsuarios grupoToAdd;
                        if (rolActivo.Grupos.Any(s => s.IdGrupo == usuarioGrupo.IdGrupoUsuario))
                        {
                            grupoToAdd = rolActivo.Grupos[rolActivo.Grupos.FindIndex(s => s.IdGrupo == usuarioGrupo.IdGrupoUsuario)];
                        }
                        else
                        {
                            grupoToAdd = new HelperAsignacionGrupoUsuarios
                            {
                                IdGrupo = usuarioGrupo.IdGrupoUsuario,
                                IdTipoGrupo = usuarioGrupo.GrupoUsuario.IdTipoGrupo,
                                DescripcionGrupo = usuarioGrupo.GrupoUsuario.Descripcion,
                            };

                        }
                        if (usuarioGrupo.IdSubGrupoUsuario != null)
                        {
                            if (grupoToAdd.SubGrupos == null)
                                grupoToAdd.SubGrupos = new List<HelperSubGurpoUsuario>();
                            if (grupoToAdd.SubGrupos.All(a => a.Id != usuarioGrupo.IdSubGrupoUsuario))
                            {
                                if (grupoToAdd.SubGrupos == null)
                                    grupoToAdd.SubGrupos = new List<HelperSubGurpoUsuario>();
                                HelperSubGurpoUsuario subGpoToAdd = new HelperSubGurpoUsuario
                                {
                                    Id = (int)usuarioGrupo.IdSubGrupoUsuario,
                                    Descripcion = usuarioGrupo.SubGrupoUsuario.SubRol.Descripcion
                                };
                                grupoToAdd.SubGrupos.Add(subGpoToAdd);
                            }
                            rolActivo.Grupos = rolActivo.Grupos ?? new List<HelperAsignacionGrupoUsuarios>();
                        }
                        if (rolActivo.Grupos.All(a => a.IdGrupo != grupoToAdd.IdGrupo))
                            rolActivo.Grupos.Add(grupoToAdd);



                    }
                    GruposUsuario = lstRoles;
                    MostrarGruposSeleccionados();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LlenaTelefonosUsuarios()
        {
            try
            {
                rptTelefonos.DataSource = TelefonosUsuario;
                rptTelefonos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void LlenaCorreosUsuarios()
        {
            try
            {
                rptCorreos.DataSource = CorreosUsuario;
                rptCorreos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Validaciones

        private void ValidaCapturaDatosGenerales(bool domicilio)
        {
            List<string> sb = new List<string>();

            if (txtAp.Text.Trim() == string.Empty)
                sb.Add("Apellido Paterno es un campo obligatorio.");
            //if (txtAm.Text.Trim() == string.Empty)
            //    sb.Add("Apellido Materno es un campo obligatorio.");
            if (txtNombre.Text.Trim() == string.Empty)
                sb.Add("Nombre es un campo obligatorio.");
            if (txtNombre.Text.Trim() == string.Empty)
                sb.Add("Nombre de Usuario es un campo obligatorio.");


            #region Validacion Telefono

            TipoUsuario tipoUsuario = _servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(IdTipoUsuario);
            if (tipoUsuario.TelefonoObligatorio)
            {
                if (TelefonosUsuario.Count > 0)
                {
                    if (!TelefonosUsuario.Any(a => a.Principal && a.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular))
                        sb.Add(string.Format("Debe ingresar un telefono Celular"));
                }
                else
                {

                    if (ddlTipoTelefono.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        throw new Exception("Debe seleccionar un tipo de telefono");

                    if (txtTelefono.Text.Trim() == string.Empty)
                        throw new Exception("Debe ingresar un numero de telefono");

                    if (int.Parse(ddlTipoTelefono.SelectedValue) != (int)BusinessVariables.EnumTipoTelefono.Celular || txtTelefono.Text.Trim() == string.Empty)
                        throw new Exception("Debe ingresar un numero de telefono celular");
                }
            }

            foreach (TelefonoUsuario telefono in TelefonosUsuario)
            {
                if (telefono.Numero.Trim() != string.Empty && telefono.Numero.Length < 10)
                {
                    sb.Add(string.Format("El telefono {0} debe ser de 10 digitos.", telefono.Numero));
                }
            }

            #endregion Validacion Telefono

            #region Validacion Correos

            if (!CorreosUsuario.Any(a => a.Obligatorio))
            {
                if (CorreosUsuario.Count > 0)
                {
                    CorreosUsuario.First().Obligatorio = true;
                }
                else
                {
                    if (txtCorreoPrincipal.Text != string.Empty)
                    {
                        if (!BusinessCorreo.IsValidEmail(txtCorreoPrincipal.Text.Trim()) || txtCorreoPrincipal.Text.Trim().Contains(" "))
                        {
                            sb.Add(string.Format("Correo {0} con formato invalido", txtCorreoPrincipal.Text.Trim()));
                        }
                    }
                    else
                        sb.Add(string.Format("El Debe Ingresar un correo"));
                }
            }
            else
            {
                foreach (CorreoUsuario correo in CorreosUsuario)
                {
                    if (!BusinessCorreo.IsValidEmail(correo.Correo) || correo.Correo.Trim().Contains(" "))
                    {
                        sb.Add(string.Format("Correo {0} con formato invalido", correo.Correo.Trim()));
                    }
                }
            }

            #endregion Validacion Correos





            if (ucAltaOrganizaciones.OrganizacionSeleccionada.Id == 0)
                sb.Add(String.Format("Debe Seleccionar una organización."));
            if (ucAltaUbicaciones.UbicacionSeleccionada.Id == 0 && !domicilio)
                sb.Add(String.Format("Debe Seleccionar una Ubicación."));

            if (sb.Count > 0)
            {
                sb.Insert(0, "<h3>Datos Generales</h3>");
                _lstError = sb;
                throw new Exception("");
            }
        }

        private void ValidaCapturaDomicilio(int idTipoUsuario)
        {
            try
            {
                List<string> sb = Metodos.ValidaCapturaCatalogoCampus(idTipoUsuario, "Id", ddlColonia.SelectedValue == "" ? 0 : Convert.ToInt32(ddlColonia.SelectedValue), txtCalle.Text.Trim(), txtNoExt.Text.Trim(), txtNoInt.Text.Trim());
                if (sb.Count > 0)
                {
                    sb.Insert(0, "<h3>Datos Generales</h3>");
                    _lstError = sb;
                    throw new Exception("");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void ValidaCapturaRoles()
        {
            List<string> sb = new List<string>();

            if (!rptRoles.Items.Cast<RepeaterItem>().Any())
                sb.Add("Debe seleccionar un Rol Grupo de usuario.");

            if (sb.Count > 0)
            {
                sb.Insert(0, "<h3>Roles</h3>");
                _lstError = sb;
                throw new Exception("");
            }
        }

        private void ValidaCapturaGrupos()
        {
            List<string> sb = new List<string>();


            if (rptRoles.Items.Count <= 0)
                sb.Add("Debe asignar al menos un Grupo.");

            if (sb.Count > 0)
            {
                sb.Insert(0, "<h3>Grupos</h3>");
                _lstError = sb;
                throw new Exception("");
            }
        }

        #endregion Validaciones

        private void HabilitaDetalle()
        {
            divUltimoAcceso.Visible = Habilitado || EditarDetalle;
            btnEditar.Visible = !Habilitado || EditarDetalle;

            divBtnGuardar.Visible = Habilitado && !EditarDetalle;

            FileUpload1.Enabled = Habilitado && !EditarDetalle && !Alta;
            btnCambiarImagen.Visible = Habilitado && !EditarDetalle && !Alta;

            txtNombre.ReadOnly = !Habilitado;
            txtAp.ReadOnly = !Habilitado;
            txtAm.ReadOnly = !Habilitado;
            txtUserName.ReadOnly = true;
            ddlPuesto.Enabled = Habilitado && !EditarDetalle;
            btnAddPuesto.Visible = Habilitado && !EditarDetalle;

            chkVip.Enabled = Habilitado;
            chkDirectoriActivo.Enabled = Habilitado;
            chkPersonaFisica.Enabled = Habilitado;

            txtCalle.ReadOnly = !Habilitado;
            txtNoExt.ReadOnly = !Habilitado;
            txtNoInt.ReadOnly = !Habilitado;
            txtCp.ReadOnly = !Habilitado;
            ddlColonia.Enabled = Habilitado;

            ddlTipoTelefono.Enabled = Habilitado;
            txtTelefono.ReadOnly = !Habilitado;
            btnAddTelefono.Visible = Habilitado;
            foreach (RepeaterItem item in rptTelefonos.Items)
            {
                Label lblPrincipalRepeater = (Label)item.FindControl("lblPrincipal");

                DropDownList ddlTipoTelefonoRepeater = (DropDownList)item.FindControl("ddlTipoTelefonoRepeater");
                TextBox numeroRepeater = (TextBox)item.FindControl("txtNumero");
                TextBox extensionRepeater = (TextBox)item.FindControl("txtExtension");

                LinkButton lbtnEditarTelefono = (LinkButton)item.FindControl("lbtnEditarTelefono");
                LinkButton lbtnEliminarTelefono = (LinkButton)item.FindControl("lbtnEliminarTelefono");
                if (ddlTipoTelefonoRepeater != null && numeroRepeater != null && extensionRepeater != null && lblPrincipalRepeater != null && lbtnEditarTelefono != null && lbtnEliminarTelefono != null)
                {
                    lbtnEditarTelefono.Visible = Habilitado;
                    lbtnEliminarTelefono.Visible = Habilitado;
                    if (bool.Parse(lblPrincipalRepeater.Text))
                    {
                        lbtnEliminarTelefono.Visible = false;
                    }
                }
            }

            txtCorreoPrincipal.ReadOnly = !Habilitado;
            txtCorreoPrincipalConfirmacion.ReadOnly = !Habilitado;
            btnAddCorreo.Visible = Habilitado;
            lbtnCancelarEdicionTelefono.Visible = Habilitado;
            lbtnCancelarEdicionCorreo.Visible = Habilitado;
            foreach (RepeaterItem item in rptCorreos.Items)
            {
                Label lblPrincipalRepeater = (Label)item.FindControl("lblCorreoPrincipal");
                TextBox txt = (TextBox)item.FindControl("txtCorreo");
                LinkButton lbtnEditarCorreo = (LinkButton)item.FindControl("lbtnEditarCorreo");
                LinkButton lbtnEliminarCorreo = (LinkButton)item.FindControl("lbtnEliminarCorreo");
                if (txt != null && lbtnEditarCorreo != null && lbtnEliminarCorreo != null && lblPrincipalRepeater != null)
                {
                    lbtnEditarCorreo.Visible = Habilitado;
                    lbtnEliminarCorreo.Visible = Habilitado;
                    if (bool.Parse(lblPrincipalRepeater.Text))
                    {
                        lbtnEditarCorreo.Visible = Habilitado;
                        lbtnEliminarCorreo.Visible = false;
                    }
                }
            }

            foreach (RepeaterItem itemRol in rptRoles.Items)
            {
                Repeater rptGrupo = (Repeater)itemRol.FindControl("rptGrupos");
                if (rptGrupo != null)
                {
                    foreach (RepeaterItem gpoItem in rptGrupo.Items)
                    {
                        Repeater rptSubGpo = (Repeater)gpoItem.FindControl("rptSubGrupos");
                        bool btnVisible;
                        LinkButton btn = (LinkButton)gpoItem.FindControl("btnRemoveRol");
                        if (rptSubGpo != null)
                        {
                            if (rptSubGpo.Items.Count > 0)
                            {
                                btn.Visible = false;
                                btn = null;
                                foreach (RepeaterItem itemSubGpo in rptSubGpo.Items)
                                {
                                    LinkButton lnkbtnSubRol = (LinkButton)itemSubGpo.FindControl("btnRemoveRolSub");
                                    if (lnkbtnSubRol != null)
                                        lnkbtnSubRol.Visible = Habilitado && !EditarDetalle;
                                }
                            }
                        }
                        if (btn != null)
                            btn.Visible = Habilitado && !EditarDetalle;
                    }
                }
            }



            btnModalOrganizacion.Visible = Habilitado && !EditarDetalle;
            btnModalUbicacion.Visible = Habilitado && !EditarDetalle;
            btnModalRoles.Visible = Habilitado && !EditarDetalle;
        }

        private void DesbloqueaBotones()
        {
            btnAddCorreo.Visible = true;
            btnAddTelefono.Visible = true;
            chkVip.Enabled = true;
            chkDirectoriActivo.Enabled = true;
            chkPersonaFisica.Enabled = true;
        }

        #endregion Metodos

        #region page

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                FileUpload1.Attributes["onchange"] = "UploadFile(this)";

                ucAltaPuesto.OnTerminarModal += UcAltaPuestoOnOnAceptarModal;
                ucAltaPuesto.OnCancelarModal += UcAltaPuestoOnOnCancelarModal;

                ucAltaOrganizaciones.OnTerminarModal += ucOrganizacion_OnTerminarModal;
                ucAltaOrganizaciones.OnCancelarModal += ucOrganizacion_OnCancelarModal;

                ucAltaUbicaciones.OnTerminarModal += UcUbicacion_OnTerminarModal;
                ucAltaUbicaciones.OnCancelarModal += UcUbicacion_OnCancelarModal;

                ucRolGrupo.OnTerminarModal += UcRolGrupoOnTerminarModal;
                ucRolGrupo.OnCancelarModal += UcRolGrupoOnOnCancelarModal;
                ucRolGrupo.AsignacionAutomatica = false;

                faq5_1.CssClass = "panel-collapse collapse in";
                faq5_2.CssClass = "panel-collapse collapse in";
                faq5_3.CssClass = "panel-collapse collapse in";
                faq_domicilio.CssClass = "panel-collapse collapse in";
                faq5_4.CssClass = "panel-collapse collapse in";
                faq5_5.CssClass = "panel-collapse collapse in";
                _mp = (UsuariosMaster)Page.Master;
                if (!IsPostBack)
                {
                    Session["UsuarioTemporal"] = null;
                    Session["UsuarioGrupo"] = null;
                    GruposUsuario = null;
                    EditarDetalle = false;
                    if (Request.QueryString["Consulta"] != null)
                    {
                        hfConsultas.Value = bool.Parse(Request.QueryString["Consulta"]).ToString();
                    }
                    if (Request.QueryString["IdUsuario"] != null && Request.QueryString["Detail"] == null)
                    {
                        lblTitle.Text = "Editar Usuario";
                        EsDetalle = false;
                        Alta = false;
                        Habilitado = true;
                        IdUsuario = int.Parse(Request.QueryString["IdUsuario"]);
                    }
                    else if (Request.QueryString["Detail"] != null)
                    {
                        lblTitle.Text = "Mi Perfil";
                        EsDetalle = true;
                        Alta = false;
                        IdUsuario = ((Usuario)Session["UserData"]).Id;
                        /**/
                        btnModalOrganizacion.Visible = false;
                        btnModalUbicacion.Visible = false;
                        btnModalRoles.Visible = false;

                        Habilitado = false;
                    }
                    else
                    {
                        lblTitle.Text = "Nuevo Usuario";
                        EsDetalle = false;
                        Alta = true;
                        Habilitado = true;
                    }
                    HabilitaDetalle();
                    if (Request.QueryString["all"] != null && Request.QueryString["all"] != string.Empty)
                        LlenaCombos(false);
                    else
                        LlenaCombos(true);
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

        protected void btnCancelarEdicion_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (bool.Parse(hfConsultas.Value))
                {
                    Response.Redirect("~/Users/Administracion/Usuarios/FrmConsultaUsuarios.aspx");
                }
                else
                    Response.Redirect(Request.RawUrl);
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
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un tipo de usuario.<br>");
                TipoUsuario tipoUsuario = _servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(Convert.ToInt32(ddlTipoUsuario.SelectedValue));
                ValidaCapturaDatosGenerales(tipoUsuario.Domicilio);

                if (tipoUsuario.Domicilio)
                {
                    ValidaCapturaDomicilio(tipoUsuario.Id);
                }

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
                    PersonaFisica = chkPersonaFisica.Checked,
                    NombreUsuario = txtUserName.Text.Trim(),
                    Password = ResolveUrl("~/ConfirmacionCuenta.aspx"),
                    Autoregistro = false,
                    Habilitado = true
                };

                if (!TelefonosUsuario.Any())
                    if (ddlTipoTelefono.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione && txtTelefono.Text.Trim() != string.Empty && !TelefonosUsuario.Any())
                    {
                        switch (int.Parse(ddlTipoTelefono.SelectedValue))
                        {
                            case (int)BusinessVariables.EnumTipoTelefono.Casa:
                                TelefonosUsuario.Add(new TelefonoUsuario
                                {
                                    Principal = false,
                                    Numero = txtTelefono.Text.Trim(),
                                    Extension = string.Empty,
                                    IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Casa
                                });
                                break;
                            case (int)BusinessVariables.EnumTipoTelefono.Celular:
                                lblPrincipal.Text = (!TelefonosUsuario.Any(a => a.Principal)).ToString();
                                TelefonosUsuario.Add(new TelefonoUsuario
                                {
                                    Principal = !TelefonosUsuario.Any(a => a.Principal),
                                    Numero = txtTelefono.Text.Trim(),
                                    Extension = string.Empty,
                                    IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Celular
                                });
                                break;
                            case (int)BusinessVariables.EnumTipoTelefono.Oficina:
                                TelefonosUsuario.Add(new TelefonoUsuario
                                {
                                    Principal = false,
                                    Numero = txtTelefono.Text.Trim(),
                                    Extension = txtExtension.Text.Trim(),
                                    IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Oficina
                                });
                                break;
                        }
                    }
                usuario.TelefonoUsuario = TelefonosUsuario;
                LlenaTelefonosUsuarios();

                if (!CorreosUsuario.Any())
                {
                    if (txtCorreoPrincipal.Text.Trim() == string.Empty)
                        throw new Exception("Debe ingresar un correoo");

                    if (CorreosUsuario.Any(a => a.Correo == txtCorreoPrincipal.Text.Trim()))
                        throw new Exception("Este correo ya ha sido registrado");

                    if (!CorreosUsuario.Any(a => a.Obligatorio))
                    {
                        if (txtCorreoPrincipalConfirmacion.Text.Trim() == string.Empty)
                            throw new Exception("Debe confirmar el correo electronico.");
                        if (txtCorreoPrincipal.Text.Trim() != txtCorreoPrincipalConfirmacion.Text.Trim())
                            throw new Exception("Los correos no coinciden");
                    }

                    if (!BusinessCorreo.IsValidEmail(txtCorreoPrincipal.Text.Trim()) || txtCorreoPrincipal.Text.Trim().Contains(" "))
                    {
                        throw new Exception(string.Format("Correo {0} con formato invalido", txtCorreoPrincipal.Text.Trim()));
                    }

                    bool principal = !CorreosUsuario.Any(a => a.Obligatorio);

                    CorreosUsuario.Add(new CorreoUsuario { Correo = txtCorreoPrincipal.Text.Trim(), Obligatorio = principal });
                }
                usuario.CorreoUsuario = CorreosUsuario;
                LlenaCorreosUsuarios();

                Domicilio domicilio = null;
                usuario.IdOrganizacion = ucAltaOrganizaciones.OrganizacionSeleccionada.Id;
                if (tipoUsuario.Domicilio)
                {
                    domicilio = new Domicilio();
                    domicilio.IdColonia = int.Parse(ddlColonia.SelectedValue);
                    domicilio.Calle = txtCalle.Text.Trim();
                    domicilio.NoExt = txtNoExt.Text.Trim();
                    domicilio.NoInt = txtNoInt.Text.Trim();
                }
                else
                    usuario.IdUbicacion = ucAltaUbicaciones.UbicacionSeleccionada.Id;

                #region Rol

                usuario.UsuarioRol = new List<UsuarioRol>();
                //TODO: CAMBIAR ASIGNACION DE ROL
                foreach (HelperAsignacionRol item in GruposUsuario)
                {
                    usuario.UsuarioRol.Add(new UsuarioRol
                    {
                        RolTipoUsuario =
                            new RolTipoUsuario
                            {
                                IdRol = item.IdRol,
                                IdTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue)
                            }
                    });
                }

                #endregion Rol

                #region Grupo

                usuario.UsuarioGrupo = new List<UsuarioGrupo>();

                foreach (HelperAsignacionRol item in GruposUsuario)
                {
                    foreach (HelperAsignacionGrupoUsuarios grupoUsuarios in item.Grupos)
                    {
                        if (grupoUsuarios.SubGrupos != null && grupoUsuarios.SubGrupos.Count > 0)
                        {
                            foreach (HelperSubGurpoUsuario subGrupo in grupoUsuarios.SubGrupos)
                            {
                                usuario.UsuarioGrupo.Add(new UsuarioGrupo
                                {
                                    IdUsuario = Alta ? 0 : IdUsuario,
                                    IdGrupoUsuario = grupoUsuarios.IdGrupo,
                                    IdRol = item.IdRol,
                                    IdSubGrupoUsuario = subGrupo.Id
                                });
                            }
                        }
                        else
                        {
                            usuario.UsuarioGrupo.Add(new UsuarioGrupo
                            {
                                IdUsuario = Alta ? 0 : IdUsuario,
                                IdGrupoUsuario = grupoUsuarios.IdGrupo,
                                IdRol = item.IdRol,
                                IdSubGrupoUsuario = null
                            });
                        }
                    }

                }

                #endregion Grupos

                if (Alta)
                {
                    _servicioUsuarios.GuardarUsuario(usuario, domicilio);
                }
                else
                {
                    _servicioUsuarios.ActualizarUsuario(IdUsuario, usuario, domicilio);
                }

                LimpiarPantalla();
                Response.Redirect(bool.Parse(hfConsultas.Value) ? "~/Users/Administracion/Usuarios/FrmConsultaUsuarios.aspx" : Request.RawUrl);

                if (OnAceptarModal != null)
                    OnAceptarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                if (ex.Message != string.Empty)
                    _lstError.Add(ex.Message);
                Alerta = _lstError;
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
                Alerta = _lstError;
            }
        }

        #endregion page

        #region Delegados


        private void UcAltaPuestoOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAreas\");",
                    true);
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

        private void UcAltaPuestoOnOnAceptarModal()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlPuesto, _servicioPuesto.ObtenerPuestosByTipoUsuario(IdTipoUsuario, true));
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAreas\");",
                    true);
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

        #endregion Delegados

        #region Datos Generales

        protected void btnCambiarImagen_OnClick(object sender, EventArgs e)
        {
        }

        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                hfGeneraUsuario.Value = false.ToString();
                EditarDetalle = !((Usuario)Session["UserData"]).Administrador;
                EsDetalle = false;
                Alta = false;
                Habilitado = true;
                HabilitaDetalle();
                btnEditar.Visible = false;
                FileUpload1.Enabled = true;
                btnCambiarImagen.Visible = true;
                divBtnGuardar.Visible = true;
                btnCancelarEdicion.Visible = true;
                btnGuardar.Visible = true;
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

        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                _servicioUsuarios.GuardarFoto(IdUsuario, FileUpload1.FileBytes);
                imgPerfil.ImageUrl = "~/DisplayImages.ashx?id=" + IdUsuario;
                _mp.ActualizarFoto();
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

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                    TipoUsuario tipoUsuario = _servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(idTipoUsuario);
                    if (tipoUsuario != null)
                    {

                        ucAltaOrganizaciones.EsSeleccion = true;
                        ucAltaOrganizaciones.EsAlta = true;
                        ucAltaOrganizaciones.IdTipoUsuario = idTipoUsuario;

                        ucAltaUbicaciones.EsSeleccion = true;
                        ucAltaUbicaciones.EsAlta = true;

                        if (!tipoUsuario.Domicilio)
                            ucAltaUbicaciones.IdTipoUsuario = idTipoUsuario;
                        ucAltaUbicaciones.Title = "Establecer Ubicación";

                        TelefonosUsuario = _servicioParametros.ObtenerTelefonosParametrosIdTipoUsuario(idTipoUsuario, false);
                        LlenaTelefonosUsuarios();

                        CorreosUsuario = _servicioParametros.ObtenerCorreosParametrosIdTipoUsuario(idTipoUsuario, false);
                        LlenaCorreosUsuarios();

                        Session["UsuarioTemporal"] = new Usuario();
                        LimpiarPantalla();

                        ucRolGrupo.IdTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                        ucRolGrupo.CargaGrupoAccesoDefault();


                        divDomicilio.Visible = tipoUsuario.Domicilio;
                        divUbicacion.Visible = !tipoUsuario.Domicilio;

                        ParametrosUsuario parametrosUsuario = _servicioParametros.ObtenerParametrosUsuario(tipoUsuario.Id);
                        if (parametrosUsuario != null)
                        {
                            ucAltaOrganizaciones.OrganizacionDefault(parametrosUsuario.IdOrganizacion);
                        }

                        if (divUbicacion.Visible)
                        {
                            if (parametrosUsuario != null)
                            {
                                ucAltaUbicaciones.UbicacionDefault(parametrosUsuario.IdUbicacion);
                            }
                        }
                        Metodos.LlenaComboCatalogo(ddlPuesto, _servicioPuesto.ObtenerPuestosByTipoUsuario(IdTipoUsuario, true));
                        if (_servicioSistemaTipoUsuario.ObtenerTipoUsuarioById(int.Parse(ddlTipoUsuario.SelectedValue)).EsMoral)
                        {
                            divPuesto.Visible = true;
                            btnAddPuesto.Visible = true;
                            btnModalOrganizacion.Text = "Editar";
                            btnModalUbicacion.Text = "Editar";
                            btnModalRoles.Text = "Editar";
                        }
                        else
                        {
                            divPuesto.Visible = false;
                            btnAddPuesto.Visible = false;
                            btnModalOrganizacion.Text = "Actividad";
                            btnModalUbicacion.Text = "Direccion";
                        }


                    }

                }
                else
                {
                    LimpiarPantalla();
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

        protected void btnAddPuesto_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione el Tipo de usuario");
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
                Alerta = _lstError;
            }
        }

        protected void txtAp_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!bool.Parse(hfGeneraUsuario.Value))
                    return;
                if (txtNombre.Text.Trim().Length <= 0)
                    return;
                string username = (txtNombre.Text.Substring(0, 1).ToLower() + txtAp.Text.Trim().ToLower()).Replace(" ", string.Empty);
                username = username.PadRight(30).Substring(0, 30).Trim();
                int limite = 10;
                if (_servicioUsuarios.ValidaUserName(username))
                {
                    for (int i = 1; i < limite; i++)
                    {
                        string tmpUsername = username + i;
                        if (!_servicioUsuarios.ValidaUserName(tmpUsername.PadRight(30).Substring(0, 30).Trim()))
                        {
                            username = tmpUsername;
                            break;
                        }
                        limite++;
                    }
                }
                txtUserName.Text = username.PadRight(35).Substring(0, 30).Trim();
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
                Alerta = _lstError;
            }
        }

        #endregion Datos Generales

        #region Telefonos

        protected void ddlTipoTelefono_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = (DropDownList)sender;
                divExtension.Visible = false;
                divExtensionTitulo.Visible = false;
                if (int.Parse(ddl.SelectedValue) == (int)BusinessVariables.EnumTipoTelefono.Oficina)
                {
                    divExtension.Visible = true;
                    divExtensionTitulo.Visible = true;
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

        protected void btnAddTelefono_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoTelefono.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Debe seleccionar un tipo de telefono");

                if (txtTelefono.Text.Trim() == string.Empty)
                    throw new Exception("Debe ingresar un numero de telefono");

                if (txtTelefono.Text.Trim() != string.Empty && txtTelefono.Text.Length < 10)
                    throw new Exception(string.Format("El telefono debe ser de 10 digitos."));



                if (bool.Parse(lblNuevoTelefonoEdicion.Text))
                {
                    if (TelefonosUsuario.Any(a => a.IdTipoTelefono == int.Parse(ddlTipoTelefono.SelectedValue) && a.Numero == txtTelefono.Text.Trim() && a.Extension == txtExtension.Text.Trim()))
                        throw new Exception("Este numero ya ha sido registrado");

                    switch (int.Parse(ddlTipoTelefono.SelectedValue))
                    {
                        case (int)BusinessVariables.EnumTipoTelefono.Casa:
                            TelefonosUsuario.Add(new TelefonoUsuario
                            {
                                Principal = false,
                                Numero = txtTelefono.Text.Trim(),
                                Extension = string.Empty,
                                IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Casa
                            });
                            break;
                        case (int)BusinessVariables.EnumTipoTelefono.Celular:
                            lblPrincipal.Text = (!TelefonosUsuario.Any(a => a.Principal)).ToString();
                            TelefonosUsuario.Add(new TelefonoUsuario
                            {
                                Principal = !TelefonosUsuario.Any(a => a.Principal),
                                Numero = txtTelefono.Text.Trim(),
                                Extension = string.Empty,
                                IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Celular
                            });
                            break;
                        case (int)BusinessVariables.EnumTipoTelefono.Oficina:
                            TelefonosUsuario.Add(new TelefonoUsuario
                            {
                                Principal = false,
                                Numero = txtTelefono.Text.Trim(),
                                Extension = txtExtension.Text.Trim(),
                                IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Oficina
                            });

                            break;
                    }
                }
                else
                {
                    if (TelefonosUsuario.Any(a => a.IdTipoTelefono == int.Parse(ddlTipoTelefono.SelectedValue) && a.Numero == txtTelefono.Text.Trim() && a.Extension == txtExtension.Text.Trim()))
                        throw new Exception("Este numero ya ha sido registrado");
                    bool principal = false;
                    if (TelefonosUsuario.Any(telefono => telefono.Principal == bool.Parse(lblPrincipal.Text) && telefono.Numero == lblTelefonoAnteriorEdicion.Text && telefono.Extension == lblExtensionAnteriorEdicion.Text && telefono.IdTipoTelefono == int.Parse(lblTipoTelefonoAnteriorEdicion.Text)))
                    {
                        foreach (TelefonoUsuario telefono in TelefonosUsuario.Where(telefono => telefono.Principal == bool.Parse(lblPrincipal.Text) && telefono.Numero == lblTelefonoAnteriorEdicion.Text && telefono.Extension == lblExtensionAnteriorEdicion.Text && telefono.IdTipoTelefono == int.Parse(lblTipoTelefonoAnteriorEdicion.Text)))
                        {
                            principal = int.Parse(ddlTipoTelefono.SelectedValue) == (int)BusinessVariables.EnumTipoTelefono.Celular ? TelefonosUsuario.Any(a => a.Principal) ? false : true : false;
                            telefono.Principal = principal;
                            telefono.IdTipoTelefono = int.Parse(ddlTipoTelefono.SelectedValue);
                            telefono.Extension = string.Empty;
                            telefono.Numero = txtTelefono.Text.Trim();
                        }
                    }
                    else
                    {
                        principal = int.Parse(ddlTipoTelefono.SelectedValue) == (int)BusinessVariables.EnumTipoTelefono.Celular ? TelefonosUsuario.Any(a => a.Principal) ? false : true : false;
                        switch (int.Parse(ddlTipoTelefono.SelectedValue))
                        {
                            case (int)BusinessVariables.EnumTipoTelefono.Casa:
                                TelefonosUsuario.Add(new TelefonoUsuario
                                {
                                    Principal = false,
                                    Numero = txtTelefono.Text.Trim(),
                                    Extension = string.Empty,
                                    IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Casa
                                });
                                break;
                            case (int)BusinessVariables.EnumTipoTelefono.Celular:
                                lblPrincipal.Text = (!TelefonosUsuario.Any(a => a.Principal)).ToString();
                                TelefonosUsuario.Add(new TelefonoUsuario
                                {
                                    Principal = principal,
                                    Numero = txtTelefono.Text.Trim(),
                                    Extension = string.Empty,
                                    IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Celular
                                });
                                break;
                            case (int)BusinessVariables.EnumTipoTelefono.Oficina:
                                TelefonosUsuario.Add(new TelefonoUsuario
                                {
                                    Principal = false,
                                    Numero = txtTelefono.Text.Trim(),
                                    Extension = txtExtension.Text.Trim(),
                                    IdTipoTelefono = (int)BusinessVariables.EnumTipoTelefono.Oficina
                                });

                                break;
                        }
                    }
                    if (!TelefonosUsuario.Any(a => a.Principal) && !principal)
                    {
                        foreach (TelefonoUsuario telefono in TelefonosUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular))
                        {
                            telefono.Principal = true;
                            break;
                        }
                    }
                }


                TelefonosUsuario = TelefonosUsuario;
                LlenaTelefonosUsuarios();

                ddlTipoTelefono.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlTipoTelefono_OnSelectedIndexChanged(ddlTipoTelefono, null);
                txtTelefono.Text = string.Empty;
                txtExtension.Text = string.Empty;

                lblNuevoTelefonoEdicion.Text = true.ToString();
                btnAddTelefono.Visible = Habilitado;
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
        protected void lblCancelarEdicionTelefono_OnClick(object sender, EventArgs e)
        {
            try
            {
                LlenaTelefonosUsuarios();

                ddlTipoTelefono.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlTipoTelefono_OnSelectedIndexChanged(ddlTipoTelefono, null);
                txtTelefono.Text = string.Empty;
                txtExtension.Text = string.Empty;

                lblNuevoTelefonoEdicion.Text = true.ToString();
                btnAddTelefono.Visible = Habilitado;
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

        protected void lbtnEditarTelefono_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (LinkButton)sender;
                if (lbtn != null)
                {
                    RepeaterItem item = (RepeaterItem)lbtn.NamingContainer;
                    if (item != null)
                    {
                        DropDownList ddlTipoTelefonoRepeater = (DropDownList)item.FindControl("ddlTipoTelefonoRepeater");
                        TextBox txtNumeroRepeater = (TextBox)item.FindControl("txtNumero");
                        TextBox txtExtensionRepeater = (TextBox)item.FindControl("txtExtension");
                        Label lblTelefonoPrincipal = (Label)item.FindControl("lblPrincipal");
                        if (ddlTipoTelefono != null && txtNumeroRepeater != null && txtExtensionRepeater != null && lblTelefonoPrincipal != null)
                        {
                            ddlTipoTelefono.SelectedValue = ddlTipoTelefonoRepeater.SelectedValue;
                            txtTelefono.Text = txtNumeroRepeater.Text;
                            txtExtension.Text = txtExtensionRepeater.Text;
                            lblNuevoTelefonoEdicion.Text = false.ToString();
                            lblTipoTelefonoAnteriorEdicion.Text = ddlTipoTelefonoRepeater.SelectedValue;
                            lblTelefonoAnteriorEdicion.Text = txtNumeroRepeater.Text;
                            lblExtensionAnteriorEdicion.Text = txtExtensionRepeater.Text;
                            lblPrincipal.Text = lblTelefonoPrincipal.Text;
                        }

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

        protected void lbtnEliminarTelefono_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (LinkButton)sender;
                if (lbtn != null)
                {
                    TelefonosUsuario.RemoveAll(r => r.Numero == lbtn.CommandArgument && r.Extension == lbtn.CommandName);
                }
                LlenaTelefonosUsuarios();
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

        protected void rptTelefonos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Header || e.Item.ItemType != ListItemType.Footer)
                {
                    Label lblTelefonoPrincipal = (Label)e.Item.FindControl("lblPrincipal");
                    DropDownList ddlTipoTelefonoRepeater = (DropDownList)e.Item.FindControl("ddlTipoTelefonoRepeater");
                    LinkButton btnEditarTelefono = (LinkButton)e.Item.FindControl("lbtnEditarTelefono");
                    LinkButton lbtnEliminarTelefono = (LinkButton)e.Item.FindControl("lbtnEliminarTelefono");
                    if (lblTelefonoPrincipal != null && ddlTipoTelefonoRepeater != null && btnEditarTelefono != null && lbtnEliminarTelefono != null)
                    {

                        ddlTipoTelefonoRepeater.DataSource = _servicioTipoTelefono.ObtenerTiposTelefono(true);
                        ddlTipoTelefonoRepeater.DataTextField = "Descripcion";
                        ddlTipoTelefonoRepeater.DataValueField = "Id";
                        ddlTipoTelefonoRepeater.DataBind();
                        if (((TelefonoUsuario)e.Item.DataItem).IdTipoTelefono == 0)
                        {
                            e.Item.FindControl("divExtension").Visible = false;
                        }
                        else
                        {
                            ddlTipoTelefonoRepeater.SelectedValue = ((TelefonoUsuario)e.Item.DataItem).IdTipoTelefono.ToString();
                            e.Item.FindControl("divExtension").Visible = ((TelefonoUsuario)e.Item.DataItem).IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Oficina;
                        }
                        ddlTipoTelefonoRepeater.Enabled = false;
                        btnEditarTelefono.Visible = Habilitado;
                        lbtnEliminarTelefono.Visible = Habilitado;
                        if (bool.Parse(lblTelefonoPrincipal.Text))
                        {
                            lbtnEliminarTelefono.Visible = false;
                        }
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

        protected void ddlTipoTelefonoRepeater_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddl = (DropDownList)sender;
                if (sender != null)
                {
                    RepeaterItem item = (RepeaterItem)ddl.NamingContainer;
                    item.FindControl("divExtension").Visible = false;
                    if (int.Parse(ddl.SelectedValue) == (int)BusinessVariables.EnumTipoTelefono.Oficina)
                    {
                        item.FindControl("divExtension").Visible = true;
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

        #endregion Telefonos

        #region Correos
        protected void btnAddCorreo_OnClick(object sender, EventArgs e)
        {
            try
            {

                if (txtCorreoPrincipal.Text.Trim() == string.Empty)
                    throw new Exception("Debe ingresar un correoo");

                if (CorreosUsuario.Any(a => a.Correo == txtCorreoPrincipal.Text.Trim()))
                    throw new Exception("Este correo ya ha sido registrado");

                if (txtCorreoPrincipalConfirmacion.Text.ToLower().Trim() == string.Empty)
                    throw new Exception("Debe confirmar el correo electronico.");
                if (txtCorreoPrincipal.Text.ToLower().Trim() != txtCorreoPrincipalConfirmacion.Text.ToLower().Trim())
                    throw new Exception("Los correos no coinciden");

                if (!BusinessCorreo.IsValidEmail(txtCorreoPrincipal.Text.Trim()) || txtCorreoPrincipal.Text.Trim().Contains(" "))
                {
                    throw new Exception(string.Format("Correo {0} con formato invalido", txtCorreoPrincipal.Text.Trim()));
                }

                bool principal = !CorreosUsuario.Any(a => a.Obligatorio);

                if (bool.Parse(lblNuevoCorreoEdicion.Text))
                {
                    CorreosUsuario.Add(new CorreoUsuario { Correo = txtCorreoPrincipal.Text.ToLower().Trim(), Obligatorio = principal });
                }
                else
                {
                    if (CorreosUsuario.Any(s => s.Correo == lblCorreoAnteriorEdicion.Text))
                    {
                        CorreosUsuario.Single(s => s.Correo == lblCorreoAnteriorEdicion.Text).Correo = txtCorreoPrincipal.Text.ToLower().Trim();
                    }
                    else
                    {
                        CorreosUsuario.Add(new CorreoUsuario { Correo = txtCorreoPrincipal.Text.ToLower().Trim(), Obligatorio = principal });
                    }
                }

                LlenaCorreosUsuarios();
                txtCorreoPrincipal.Text = string.Empty;
                txtCorreoPrincipalConfirmacion.Text = string.Empty;
                lblNuevoCorreoEdicion.Text = true.ToString();
                btnAddCorreo.Visible = Habilitado;
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

        protected void lbtnCancelarEdicionCorreo_OnClick(object sender, EventArgs e)
        {
            try
            {
                LlenaCorreosUsuarios();
                txtCorreoPrincipal.Text = string.Empty;
                txtCorreoPrincipalConfirmacion.Text = string.Empty;
                lblNuevoCorreoEdicion.Text = true.ToString();
                btnAddCorreo.Visible = Habilitado;
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

        protected void lbtnEditarCorreo_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (LinkButton)sender;
                if (lbtn != null)
                {
                    RepeaterItem item = (RepeaterItem)lbtn.NamingContainer;
                    if (item != null)
                    {
                        TextBox txt = (TextBox)item.FindControl("txtCorreo");
                        Label lbl = (Label)item.FindControl("lblCorreoPrincipal");
                        if (txt != null && lbl != null)
                        {
                            txtCorreoPrincipal.Text = txt.Text;
                            lblNuevoCorreoEdicion.Text = false.ToString();
                            lblCorreoAnteriorEdicion.Text = txt.Text;
                            lblCorreoPrincipalEdicion.Text = lbl.Text;
                        }

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

        protected void lbtnEliminarCorreo_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (LinkButton)sender;
                if (lbtn != null)
                {
                    CorreosUsuario.RemoveAll(r => r.Correo == lbtn.CommandArgument && r.Obligatorio == bool.Parse(lbtn.CommandName));
                }
                LlenaCorreosUsuarios();
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

        protected void rptCorreos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Header || e.Item.ItemType != ListItemType.Footer)
                {
                    Label lblCorreoPrincipal = (Label)e.Item.FindControl("lblCorreoPrincipal");
                    TextBox txt = (TextBox)e.Item.FindControl("txtCorreo");
                    LinkButton lbtnEditarCorreo = (LinkButton)e.Item.FindControl("lbtnEditarCorreo");
                    LinkButton lbtnEliminarCorreo = (LinkButton)e.Item.FindControl("lbtnEliminarCorreo");
                    if (lblPrincipal != null && txt != null && lbtnEditarCorreo != null && lbtnEliminarCorreo != null)
                    {
                        if (bool.Parse(lblCorreoPrincipal.Text))
                        {
                            lbtnEditarCorreo.Visible = Habilitado;
                            lbtnEliminarCorreo.Visible = false;
                        }
                        else
                        {
                            lbtnEditarCorreo.Visible = Habilitado;
                            lbtnEliminarCorreo.Visible = Habilitado;
                        }
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

        #endregion Correos

        #region Organizacion

        protected void btnModalOrganizacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione el Tipo de usuario");
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                ucAltaOrganizaciones.EsSeleccion = true;
                ucAltaOrganizaciones.EsAlta = true;
                ucAltaOrganizaciones.Title = "Selecciona Organización";
                ucAltaOrganizaciones.IdTipoUsuario = idTipoUsuario;

                if (rptOrganizacion.Items.Count > 0)
                {
                    ucAltaOrganizaciones.IdOrganizacion = int.Parse(((Label)rptOrganizacion.Items[0].FindControl("lblIdOrganizacion")).Text);
                    ucAltaOrganizaciones.SetOrganizacionSeleccion();
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                    "MostrarPopup(\"#modalOrganizacion\");", true);
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

        private void MuestraOrganizacion(List<Organizacion> org)
        {
            try
            {
                org.Add(ucAltaOrganizaciones.OrganizacionSeleccionada);
                rptOrganizacion.DataSource = org;
                rptOrganizacion.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void ucOrganizacion_OnTerminarModal()
        {
            try
            {
                MuestraOrganizacion(new List<Organizacion>());
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                    "CierraPopup(\"#modalOrganizacion\");", true);
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

        private void ucOrganizacion_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                    "CierraPopup(\"#modalOrganizacion\");", true);
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

        #endregion Organizacion

        #region Ubicacion

        protected void btnModalUbicacion_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione el Tipo de usuario");
                int idTipoUsuario = Convert.ToInt32(ddlTipoUsuario.SelectedValue);
                ucAltaUbicaciones.EsSeleccion = true;
                ucAltaUbicaciones.EsAlta = true;
                ucAltaUbicaciones.IdTipoUsuario = idTipoUsuario;
                if (rptUbicacion.Items.Count > 0)
                {
                    ucAltaUbicaciones.IdUbicacion = int.Parse(((Label)rptUbicacion.Items[0].FindControl("lblIdUbicacion")).Text);
                    ucAltaUbicaciones.SetUbicacionSeleccion();
                }

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalUbicacion\");", true);
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

        private void MuestraUbicacion(List<Ubicacion> ub)
        {
            try
            {
                ub.Add(ucAltaUbicaciones.UbicacionSeleccionada);
                rptUbicacion.DataSource = ub;
                rptUbicacion.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        protected void txtCp_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlColonia, _servicioSistemaDomicilio.ObtenerColoniasCp(int.Parse(txtCp.Text.Trim()), true));
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

        protected void ddlColonia_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlColonia.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    txtMunicipio.Text = string.Empty;
                    txtEstado.Text = string.Empty;
                    return;
                }
                Colonia col = _servicioSistemaDomicilio.ObtenerDetalleColonia(int.Parse(ddlColonia.SelectedValue));
                if (col != null)
                {
                    txtMunicipio.Text = col.Municipio.Descripcion;
                    txtEstado.Text = col.Municipio.Estado.Descripcion;
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

        private void UcUbicacion_OnTerminarModal()
        {
            try
            {
                MuestraUbicacion(new List<Ubicacion>());
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                    "CierraPopup(\"#modalUbicacion\");", true);
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

        private void UcUbicacion_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script",
                    "CierraPopup(\"#modalUbicacion\");", true);
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

        #endregion Ubicacion

        #region RolesGrupos

        private void MostrarGruposSeleccionados()
        {
            try
            {
                rptRoles.DataSource = GruposUsuario;
                rptRoles.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UcRolGrupoOnOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalRoles\");", true);
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

        private void UcRolGrupoOnTerminarModal()
        {
            try
            {
                AgregarGrupo(ucRolGrupo.GruposSeleccionados);
                ucRolGrupo.Limpiar();
                MostrarGruposSeleccionados();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalRoles\");",
                    true);
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

        protected void btnModalRoles_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione el Tipo de usuario");
                ucRolGrupo.AsignacionAutomatica = false;
                ucRolGrupo.IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalRoles\");",
                    true);
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

        protected void rptRoles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                Repeater rpt = (Repeater)e.Item.FindControl("rptGrupos");
                rpt.DataSource = ((HelperAsignacionRol)e.Item.DataItem).Grupos;
                rpt.DataBind();
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

        protected void rptGrupos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                Repeater rpt = (Repeater)e.Item.FindControl("rptSubGrupos");
                rpt.DataSource = ((HelperAsignacionGrupoUsuarios)e.Item.DataItem).SubGrupos;
                rpt.DataBind();
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

        private void ValidarRolVacio(int idrol, int idGrupo, bool esGrupo)
        {
            try
            {
                HelperAsignacionRol rol = GruposUsuario.Single(s => s.IdRol == idrol);
                if (rol != null)
                {
                    List<HelperAsignacionGrupoUsuarios> gposRol = rol.Grupos;
                    if (esGrupo)
                    {
                        if (!gposRol.Any())
                            EliminarRol(idrol);
                    }
                    else
                    {
                        HelperAsignacionGrupoUsuarios gpo = gposRol.SingleOrDefault(w => w.IdGrupo == idGrupo);
                        if (gpo != null)
                        {

                            bool containsSubGrpo = gposRol.Single(w => w.IdGrupo == idGrupo).SubGrupos.Count > 0;
                            if (!containsSubGrpo)
                            {
                                EliminarGrupo(idrol, idGrupo, null, false);
                                ValidarRolVacio(idrol, idGrupo, true);
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void btnRemoveRol_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                if (btn != null)
                {
                    if ((int)BusinessVariables.EnumTiposGrupos.Administrador == int.Parse(btn.CommandName) && IdUsuario == ((Usuario)Session["UserData"]).Id)
                        throw new Exception("No puede eliminar este Rol");
                    RepeaterItem itemGrupo = (RepeaterItem)btn.NamingContainer;
                    if (itemGrupo != null)
                    {

                        Label lblIdGrupo = (Label)itemGrupo.FindControl("lblIdGrupo");
                        if (lblIdGrupo != null)
                        {
                            Repeater rptGrupo = (Repeater)itemGrupo.NamingContainer;
                            if (rptGrupo != null)
                            {
                                RepeaterItem itemRol = (RepeaterItem)rptGrupo.NamingContainer;
                                if (itemRol != null)
                                {
                                    Label lblIdRol = (Label)itemRol.FindControl("lblIdRol");
                                    if (lblIdRol != null)
                                    {
                                        int idRol = int.Parse(lblIdRol.Text);
                                        int idGrupo = int.Parse(lblIdGrupo.Text);
                                        EliminarGrupo(idRol, idGrupo, null, false);
                                        ValidarRolVacio(idRol, idGrupo, true);
                                    }
                                }
                            }
                        }
                    }
                }

                MostrarGruposSeleccionados();
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

        protected void btnRemoveRolSub_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                if (btn != null)
                {
                    RepeaterItem itemRepeater = (RepeaterItem)btn.NamingContainer;
                    if (itemRepeater != null)
                    {
                        Repeater rptSubGrupos = (Repeater)itemRepeater.NamingContainer;
                        if (rptSubGrupos != null)
                        {
                            RepeaterItem itemGrupo = (RepeaterItem)rptSubGrupos.NamingContainer;
                            if (itemGrupo != null)
                            {
                                Label lblIdGrupo = (Label)itemGrupo.FindControl("lblIdGrupo");
                                Repeater rptGrupo = (Repeater)itemGrupo.NamingContainer;
                                if (rptGrupo != null)
                                {
                                    RepeaterItem itemRol = (RepeaterItem)rptGrupo.NamingContainer;
                                    if (itemRol != null)
                                    {
                                        Label lblIdRol = (Label)itemRol.FindControl("lblIdRol");
                                        if (lblIdRol != null)
                                        {
                                            if (lblIdGrupo != null)
                                            {
                                                int idRol = int.Parse(lblIdRol.Text);
                                                int idGrupo = int.Parse(lblIdGrupo.Text);
                                                EliminarGrupo(idRol, idGrupo, int.Parse(btn.CommandArgument), true);
                                                ValidarRolVacio(idRol, idGrupo, false);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                MostrarGruposSeleccionados();
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

        #endregion RolesGrupos
    }
}