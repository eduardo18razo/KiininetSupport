using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceSistemaSubRol;
using KiiniHelp.ServiceSistemaTipoGrupo;
using KiiniHelp.ServiceSubGrupoUsuario;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class AsociarGrupoUsuario : UserControl
    {
        readonly ServiceTipoGrupoClient _servicioSistemaTipoGrupo = new ServiceTipoGrupoClient();
        readonly ServiceSubRolClient _servicioSistemaSubRoles = new ServiceSubRolClient();
        readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        readonly ServiceSubGrupoUsuarioClient _servicioSubGrupoUsuario = new ServiceSubGrupoUsuarioClient();
        private List<string> _lstError = new List<string>();
        public string Modal { get; set; }

        public bool AsignacionAutomatica
        {
            get { return Convert.ToBoolean(hfAsignacionAutomatica.Value); }
            set
            { hfAsignacionAutomatica.Value = value.ToString(); }
        }


        public bool Administrador
        {
            get { return divGrupoAdministrador.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.Administrador, value); }
        }

        public bool Acceso
        {
            get { return divGrupoAcceso.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.Usuario, value); }
        }

        public bool Dueño
        {
            get { return divDuenoServicio.Visible; }
            set
            {
                HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeCategoría, value);
            }
        }

        public bool EspecialConsulta
        {
            get { return divGrupoEspConsulta.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.ConsultasEspeciales, value); }
        }

        public bool Atencion
        {
            get { return divGrupoRespAtencion.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.Agente, value); }
        }

        public bool Mtto
        {
            get { return divGrupoRespMtto.Visible; }
            set
            {
                HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeContenido, value);
            }
        }

        public bool Operacion
        {
            get { return divGrupoRespOperacion.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeOperación, value); }
        }

        public bool Desarrollo
        {
            get { return divGrupoRespDesarrollo.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo, value); }
        }

        public bool ContacCenter
        {
            get { return divdivContactCenter.Visible; }
            set
            { HabilitaGrupos((int)BusinessVariables.EnumRoles.AgenteUniversal, value); }
        }

        //public bool UbicacionEmpleado
        //{
        //    get { return divUbicacionEmpleado.Visible; }
        //    set
        //    { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUbicacion, value); }
        //}

        //public bool OrganizacionEmpleado
        //{
        //    get { return divOrganizacionEmpleado.Visible; }
        //    set
        //    { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoOrganizacion, value); }
        //}

        //public bool UsuarioEmpleado
        //{
        //    get { return divUsuarioEmpleado.Visible; }
        //    set
        //    { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUsuario, value); }
        //}

        //public bool UsuarioCliente
        //{
        //    get { return divUsuarioCliente.Visible; }
        //    set
        //    { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableMantenimientoCliente, value); }
        //}

        //public bool UsuarioProveedor
        //{
        //    get { return divUsuarioProveedor.Visible; }
        //    set
        //    { HabilitaGrupos((int)BusinessVariables.EnumRoles.ResponsableMantenimientoProveedor, value); }
        //}

        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(hfTipoUsuario.Value); }
            set { hfTipoUsuario.Value = value.ToString(); }
        }

        public RepeaterItemCollection GruposAsociados
        {
            get { return rptUsuarioGrupo.Items; }
        }

        private List<string> AlertaGrupos
        {
            set
            {
                panelAlertaGrupos.Visible = value.Any();
                if (!panelAlertaGrupos.Visible) return;
                rptErrorGrupos.DataSource = value;
                rptErrorGrupos.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "UpScroll(\"" + Modal + "\");", true);
            }
        }

        public bool ValidaCapturaGrupos()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (Administrador)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.Administrador) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Administrador.</li>");
                if (Acceso)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.Usuario) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Acceso.</li>");
                if (EspecialConsulta)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.ConsultasEspeciales) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Especial de consulta.</li>");
                if (Atencion)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.Agente) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Responsable de Atención.</li>");
                if (Mtto)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.ResponsableDeContenido) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Responsable de Mantenimiento.</li>");
                if (Dueño)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.ResponsableDeCategoría) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Dueño del Servicio.</li>");
                if (Operacion)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.ResponsableDeOperación) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Responsable de Operación.</li>");
                if (Desarrollo)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Responsable de Desarrollo.</li>");
                if (ContacCenter)
                    if (GruposAsociados.Cast<RepeaterItem>().Select(item => (Label)item.FindControl("lblIdTipoSubGrupo")).Count(lblIdRol => int.Parse(lblIdRol.Text) == (int)BusinessVariables.EnumRoles.AgenteUniversal) <= 0)
                        sb.AppendLine("<li>Debe asignar al menos un grupo de Tipo Contact Center.</li>");

                if (sb.ToString() != string.Empty)
                {
                    sb.Append("</ul>");
                    sb.Insert(0, "<ul>");
                    sb.Insert(0, "<h3>Asignación de Grupos</h3>");
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
                return false;
            }


            return true;
        }

        private void ObtenerGruposHerencia()
        {
            try
            {
                foreach (GrupoUsuario grupo in _servicioGrupoUsuario.ObtenerGruposUsuarioSistema(IdTipoUsuario))
                {
                    if (grupo.SubGrupoUsuario.Any())
                    {
                        foreach (SubGrupoUsuario subGrupo in grupo.SubGrupoUsuario)
                        {
                            AsignarGrupo(grupo, subGrupo.SubRol.IdRol, subGrupo.Id);
                        }
                    }
                    else
                    {
                        AsignarGrupo(grupo, grupo.TipoGrupo.RolTipoGrupo.First().IdRol, null);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Limpiar()
        {
            try
            {
                Session["UsuarioGrupo"] = null;
                List<UsuarioGrupo> lst = (List<UsuarioGrupo>)Session["UsuarioGrupo"];
                rptUsuarioGrupo.DataSource = lst;
                rptUsuarioGrupo.DataBind();
                if (Administrador)
                    ddlGrupoAdministrador.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (Acceso)
                    ddlGrupoAcceso.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (EspecialConsulta)
                    ddlGrupoEspecialConsulta.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (Atencion)
                    ddlGrupoResponsableAtencion.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (Mtto)
                    ddlGrupoResponsableMantenimiento.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (Operacion)
                    ddlGrupoResponsableOperacion.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (Desarrollo)
                    ddlGrupoResponsableDesarrollo.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (Dueño)
                    ddlDuenoServicio.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                if (ContacCenter)
                    ddlContactCenter.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }

        public void HabilitaGrupos(int idRol, bool visible)
        {
            try
            {
                switch (idRol)
                {
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        divGrupoAdministrador.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoAdministrador, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.Usuario:
                        divGrupoAcceso.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoAcceso, _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ConsultasEspeciales:
                        divGrupoEspConsulta.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoEspecialConsulta, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.Agente:
                        divGrupoRespAtencion.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoResponsableAtencion, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        divGrupoRespMtto.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoResponsableMantenimiento, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeOperación:
                        divGrupoRespOperacion.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoResponsableOperacion, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo:
                        divGrupoRespDesarrollo.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlGrupoResponsableDesarrollo, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeCategoría:
                        divDuenoServicio.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlDuenoServicio, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.AgenteUniversal:
                        divdivContactCenter.Visible = visible;
                        if (visible)
                            Metodos.LlenaComboCatalogo(ddlContactCenter, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUbicacion:
                    //    divUbicacionEmpleado.Visible = visible;
                    //    if (visible)
                    //        Metodos.LlenaComboCatalogo(ddlUbicacionEmpleado, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoOrganizacion:
                    //    divOrganizacionEmpleado.Visible = visible;
                    //    if (visible)
                    //        Metodos.LlenaComboCatalogo(ddlOrganizacionEmpleado, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoCliente:
                    //    divUsuarioCliente.Visible = visible;
                    //    if (visible)
                    //        Metodos.LlenaComboCatalogo(ddlUsuarioCliente, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUsuario:
                    //    divUsuarioEmpleado.Visible = visible;
                    //    if (visible)
                    //        Metodos.LlenaComboCatalogo(ddlUsuarioEmpleado, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoProveedor:
                    //    divUsuarioProveedor.Visible = visible;
                    //    if (visible)
                    //        Metodos.LlenaComboCatalogo(ddlUsuarioProveedor, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        #region Grupos
        protected void OnClickAsignarGrupo(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn == null) return;
                int operacion;
                int.TryParse(btn.CommandArgument, out operacion);
                GrupoUsuario grupoUsuario = null;
                List<HelperSubGurpoUsuario> lstSubRoles = new List<HelperSubGurpoUsuario>();
                int idGrupoUsuario = 0, idRol = 0;
                switch (operacion)
                {
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        idRol = (int)BusinessVariables.EnumRoles.Administrador;
                        if (ddlGrupoAdministrador.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoAdministrador.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;
                    case (int)BusinessVariables.EnumRoles.Usuario:
                        idRol = (int)BusinessVariables.EnumRoles.Usuario;
                        if (ddlGrupoAcceso.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoAcceso.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;
                    case (int)BusinessVariables.EnumRoles.ConsultasEspeciales:
                        idRol = (int)BusinessVariables.EnumRoles.ConsultasEspeciales;
                        if (ddlGrupoEspecialConsulta.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoEspecialConsulta.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;
                    case (int)BusinessVariables.EnumRoles.Agente:
                        idRol = (int)BusinessVariables.EnumRoles.Agente;
                        if (ddlGrupoResponsableAtencion.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableAtencion.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeContenido;
                        if (ddlGrupoResponsableMantenimiento.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableMantenimiento.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeOperación:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación;
                        if (ddlGrupoResponsableOperacion.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableOperacion.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo;
                        if (ddlGrupoResponsableDesarrollo.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableDesarrollo.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;

                    case (int)BusinessVariables.EnumRoles.ResponsableDeCategoría:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeCategoría;
                        if (ddlDuenoServicio.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlDuenoServicio.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;

                    case (int)BusinessVariables.EnumRoles.AgenteUniversal:
                        idRol = (int)BusinessVariables.EnumRoles.AgenteUniversal;
                        if (ddlContactCenter.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Seleccione un grupo valido");
                        idGrupoUsuario = Convert.ToInt32(ddlContactCenter.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                        break;


                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoOrganizacion:
                    //    idRol = (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoOrganizacion;
                    //    if (ddlOrganizacionEmpleado.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    //        throw new Exception("Seleccione un grupo valido");
                    //    idGrupoUsuario = Convert.ToInt32(ddlOrganizacionEmpleado.SelectedItem.Value);
                    //    grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                    //    lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                    //    break;
                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUbicacion:
                    //    idRol = (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUbicacion;
                    //    if (ddlUbicacionEmpleado.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    //        throw new Exception("Seleccione un grupo valido");
                    //    idGrupoUsuario = Convert.ToInt32(ddlUbicacionEmpleado.SelectedItem.Value);
                    //    grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                    //    lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                    //    break;
                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoCliente:
                    //    idRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación;
                    //    if (ddlUsuarioCliente.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    //        throw new Exception("Seleccione un grupo valido");
                    //    idGrupoUsuario = Convert.ToInt32(ddlUsuarioCliente.SelectedItem.Value);
                    //    grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                    //    lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                    //    break;
                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUsuario:
                    //    idRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación;
                    //    if (ddlUsuarioEmpleado.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    //        throw new Exception("Seleccione un grupo valido");
                    //    idGrupoUsuario = Convert.ToInt32(ddlUsuarioEmpleado.SelectedItem.Value);
                    //    grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                    //    lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                    //    break;
                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoProveedor:
                    //    idRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación;
                    //    if (ddlUsuarioProveedor.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    //        throw new Exception("Seleccione un grupo valido");
                    //    idGrupoUsuario = Convert.ToInt32(ddlUsuarioProveedor.SelectedItem.Value);
                    //    grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                    //    lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(idGrupoUsuario, false);
                    //    break;

                }
                if (grupoUsuario != null)
                {
                    if (lstSubRoles.Count > 0)
                    {
                        if (AsignacionAutomatica)
                        {
                            foreach (SubRol sbRol in _servicioSistemaSubRoles.ObtenerSubRolesByGrupoUsuarioRol(idGrupoUsuario, idRol, false))
                            {
                                AsignarGrupo(grupoUsuario, idRol, sbRol.Id);
                            }
                        }
                        else
                        {
                            hfOperacion.Value = operacion.ToString();
                            lblTitleSubRoles.Text = String.Format("Seleccione Sub Rol de Grupo {0}", grupoUsuario.Descripcion);
                            Metodos.LlenaListBoxCatalogo(chklbxSubRoles, _servicioSistemaSubRoles.ObtenerSubRolesByGrupoUsuarioRol(idGrupoUsuario, idRol, false));
                            ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalSeleccionRol\");", true);
                        }
                        return;
                    }
                    AsignarGrupo(grupoUsuario, idRol, null);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        public List<GrupoUsuarioInventarioArbol> GruposAsignados
        {
            set
            {
                foreach (GrupoUsuarioInventarioArbol gpo in value)
                {
                    if (gpo.SubGrupoUsuario != null)
                        AsignarGrupo(gpo.GrupoUsuario, gpo.IdRol, gpo.SubGrupoUsuario.Id);
                    else
                        AsignarGrupo(gpo.GrupoUsuario, gpo.IdRol, null);
                }
            }
        }

        private void AsignarGrupo(GrupoUsuario grupoUsuario, int idRol, int? idSubGrupoUsuario)
        {
            try
            {
                List<UsuarioGrupo> lst = (List<UsuarioGrupo>)Session["UsuarioGrupo"] ?? new List<UsuarioGrupo>();
                if (grupoUsuario != null)
                {
                    if (lst.Any(a => a.IdGrupoUsuario == grupoUsuario.Id && a.IdSubGrupoUsuario == idSubGrupoUsuario && a.IdRol == idRol))
                        throw new Exception("Este grupo ya ha sido asignado");
                    grupoUsuario.TipoGrupo = _servicioSistemaTipoGrupo.ObtenerTiposGrupo(false).SingleOrDefault(s => s.Id == grupoUsuario.IdTipoGrupo);
                    lst.Add(new UsuarioGrupo
                    {
                        IdGrupoUsuario = grupoUsuario.Id,
                        IdRol = idRol,
                        IdSubGrupoUsuario = idSubGrupoUsuario,
                        GrupoUsuario = grupoUsuario,
                        SubGrupoUsuario = idSubGrupoUsuario != null ? _servicioSubGrupoUsuario.ObtenerSubGrupoUsuario(grupoUsuario.Id, (int)idSubGrupoUsuario) : null
                    });
                }
                Session["UsuarioGrupo"] = lst;
                rptUsuarioGrupo.DataSource = lst;
                rptUsuarioGrupo.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void OnClickAltaGrupo(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (sender == null) return;
                if (btn.CommandArgument != "0")
                {
                    ucAltaGrupoUsuario.IdRol = Convert.ToInt32(btn.CommandArgument);
                    ucAltaGrupoUsuario.IdTipoUsuario = IdTipoUsuario;
                    ucAltaGrupoUsuario.IdTipoGrupo = int.Parse(btn.CommandArgument);
                    ucAltaGrupoUsuario.Alta = true;
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaGrupoUsuarios\");", true);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        #endregion Grupos

        #region SubRoles
        protected void btnAsignarSubRoles_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (btn == null) return;
                int operacion = Convert.ToInt32(hfOperacion.Value);
                GrupoUsuario grupoUsuario = null;
                int idGrupoUsuario, idRol = 0;

                switch (operacion)
                {
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        idRol = (int)BusinessVariables.EnumRoles.Administrador;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoAdministrador.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.Usuario:
                        idRol = (int)BusinessVariables.EnumRoles.Usuario;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoAcceso.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.ConsultasEspeciales:
                        idRol = (int)BusinessVariables.EnumRoles.ConsultasEspeciales;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoEspecialConsulta.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.Agente:
                        idRol = (int)BusinessVariables.EnumRoles.Agente;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableAtencion.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeContenido;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableMantenimiento.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeOperación:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableOperacion.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo;
                        idGrupoUsuario = Convert.ToInt32(ddlGrupoResponsableDesarrollo.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeCategoría:
                        idRol = (int)BusinessVariables.EnumRoles.ResponsableDeCategoría;
                        idGrupoUsuario = Convert.ToInt32(ddlDuenoServicio.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                    case (int)BusinessVariables.EnumRoles.AgenteUniversal:
                        idRol = (int)BusinessVariables.EnumRoles.AgenteUniversal;
                        idGrupoUsuario = Convert.ToInt32(ddlContactCenter.SelectedItem.Value);
                        grupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(idGrupoUsuario);
                        break;
                }
                if (grupoUsuario != null)
                {
                    foreach (ListItem item in chklbxSubRoles.Items.Cast<ListItem>().Where(item => item.Selected))
                    {
                        AsignarGrupo(grupoUsuario, idRol, grupoUsuario.SubGrupoUsuario.First(w => w.IdSubRol == int.Parse(item.Value)).Id);
                    }
                }

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalSeleccionRol\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        protected void btnCancelarSubRoles_OnClick(object sender, EventArgs e)
        {
            try
            {
                int operacion = Convert.ToInt32(hfOperacion.Value);
                switch (operacion)
                {
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        ddlGrupoAdministrador.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.Usuario:
                        ddlGrupoAcceso.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.ConsultasEspeciales:
                        ddlGrupoEspecialConsulta.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.Agente:
                        ddlGrupoResponsableAtencion.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        ddlGrupoResponsableMantenimiento.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeOperación:
                        ddlGrupoResponsableOperacion.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo:
                        ddlGrupoResponsableDesarrollo.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeCategoría:
                        ddlDuenoServicio.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                    case (int)BusinessVariables.EnumRoles.AgenteUniversal:
                        ddlContactCenter.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                        break;
                }

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalSeleccionRol\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }
        #endregion SubRoles

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGrupos = new List<string>();
                ucAltaGrupoUsuario.FromOpcion = AsignacionAutomatica;
                ucAltaGrupoUsuario.OnAceptarModal += ucAltaGrupoUsuario_OnAceptarModal;
                ucAltaGrupoUsuario.OnCancelarModal += ucAltaGrupoUsuario_OnCancelarModal;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        void ucAltaGrupoUsuario_OnAceptarModal()
        {
            try
            {
                int idRol = ucAltaGrupoUsuario.IdRol;
                switch (idRol)
                {
                    case (int)BusinessVariables.EnumRoles.Administrador:
                        divGrupoAdministrador.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoAdministrador, AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.Usuario:
                        divGrupoAcceso.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoAcceso,
                            _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ConsultasEspeciales:
                        divGrupoEspConsulta.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoEspecialConsulta,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.Agente:
                        divGrupoRespAtencion.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableAtencion,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        divGrupoRespMtto.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableMantenimiento,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeOperación:
                        divGrupoRespOperacion.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableOperacion,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo:
                        divGrupoRespDesarrollo.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlGrupoResponsableDesarrollo,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeCategoría:
                        divDuenoServicio.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlDuenoServicio,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;
                    case (int)BusinessVariables.EnumRoles.AgenteUniversal:
                        divdivContactCenter.Visible = true;
                        Metodos.LlenaComboCatalogo(ddlContactCenter,
                            AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                        break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUbicacion:
                    //    divUbicacionEmpleado.Visible = true;
                    //    Metodos.LlenaComboCatalogo(ddlUbicacionEmpleado,
                    //        AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoOrganizacion:
                    //    divOrganizacionEmpleado.Visible = true;
                    //    Metodos.LlenaComboCatalogo(ddlOrganizacionEmpleado,
                    //        AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoCliente:
                    //    divUsuarioCliente.Visible = true;
                    //    Metodos.LlenaComboCatalogo(ddlUsuarioCliente,
                    //        AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoEmpleadoUsuario:
                    //    divUsuarioEmpleado.Visible = true;
                    //    Metodos.LlenaComboCatalogo(ddlUsuarioEmpleado,
                    //        AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;

                    //case (int)BusinessVariables.EnumRoles.ResponsableMantenimientoProveedor:
                    //    divUsuarioProveedor.Visible = true;
                    //    Metodos.LlenaComboCatalogo(ddlUsuarioProveedor,
                    //        AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(idRol, true) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, IdTipoUsuario, true));
                    //    break;
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaGrupoUsuarios\");", true);

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        void ucAltaGrupoUsuario_OnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaGrupoUsuarios\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        protected void btnCerrarGrupos_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaGrupos();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalGrupos\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Limpiar();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        protected void chklbxSubRoles_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int value = 0;
                string result = Request.Form["__EVENTTARGET"];
                string[] checkedBox = result.Split('$');
                int index = int.Parse(checkedBox[checkedBox.Length - 1]);
                if (chklbxSubRoles.Items[index].Selected)
                {
                    value = Convert.ToInt32(chklbxSubRoles.Items[index].Value);
                }
                switch (int.Parse(hfOperacion.Value))
                {
                    case (int)BusinessVariables.EnumRoles.Agente:
                        bool permitir = chklbxSubRoles.Items.Cast<ListItem>().Any(item => int.Parse(item.Value) == (int)BusinessVariables.EnumSubRoles.Supervisor && item.Selected);
                        if (!permitir)
                            foreach (ListItem item in chklbxSubRoles.Items)
                            {
                                item.Selected = int.Parse(item.Value) == value;
                            }
                        break;
                    case (int)BusinessVariables.EnumRoles.ResponsableDeContenido:
                        foreach (ListItem item in chklbxSubRoles.Items)
                        {
                            item.Selected = int.Parse(item.Value) == value;
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        protected void chkAsignarGruposSistema_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Limpiar();
                if (!chkAsignarGruposSistema.Checked)
                {
                    return;
                }
                ObtenerGruposHerencia();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaGrupos = _lstError;
            }
        }

        protected void btnEliminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button button = (sender as Button);
                //Get the Repeater Item reference
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    //Get the repeater item index
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupoUsuario = (Label)rptUsuarioGrupo.Items[index].FindControl("lblIdGrupoUsuario");
                        Label lblIdSubGrupoUsuario = (Label)rptUsuarioGrupo.Items[index].FindControl("lblIdSubGrupo");
                        List<UsuarioGrupo> lst = (List<UsuarioGrupo>)Session["UsuarioGrupo"];

                        if (lblIdSubGrupoUsuario.Text != string.Empty)
                            lst.Remove(lst.Single(s => s.IdGrupoUsuario == int.Parse(lblIdGrupoUsuario.Text) && s.IdSubGrupoUsuario == int.Parse(lblIdSubGrupoUsuario.Text)));
                        else
                            lst.Remove(lst.Single(s => s.IdGrupoUsuario == int.Parse(lblIdGrupoUsuario.Text)));
                        Session["UsuarioGrupo"] = lst;
                        rptUsuarioGrupo.DataSource = lst;
                        rptUsuarioGrupo.DataBind();
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
                AlertaGrupos = _lstError;
            }
        }
    }
}