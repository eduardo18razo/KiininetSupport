using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceSistemaRol;
using KiiniHelp.ServiceSubGrupoUsuario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcRolGrupo : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceRolesClient _serviciosRoles = new ServiceRolesClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        private readonly ServiceSubGrupoUsuarioClient _servicioSubGrupoUsuario = new ServiceSubGrupoUsuarioClient();

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

        public int IdRol
        {
            get { return int.Parse(ddlRol.SelectedValue); }
            set { ddlRol.SelectedValue = value.ToString(); }
        }
        public int IdTipoUsuario
        {
            get { return int.Parse(hfIdUsuario.Value); }
            set
            {
                hfIdUsuario.Value = value.ToString();
                CargaRoles();
            }
        }
        public bool AsignacionAutomatica
        {
            get { return Convert.ToBoolean(hfAsignacionAutomatica.Value); }
            set { hfAsignacionAutomatica.Value = value.ToString(); }
        }

        public List<HelperAsignacionRol> GruposSeleccionados
        {
            get { return ViewState["GruposSeleccionados"] == null ? new List<HelperAsignacionRol>() : (List<HelperAsignacionRol>)ViewState["GruposSeleccionados"]; }
            set { ViewState["GruposSeleccionados"] = value; }
        }

        private void CargaRoles()
        {
            try
            {
                ddlRol.DataSource = _serviciosRoles.ObtenerRoles(IdTipoUsuario, true);
                ddlRol.DataTextField = "Descripcion";
                ddlRol.DataValueField = "Id";
                ddlRol.DataBind();
                LimpiarSeleccion();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LimpiarSeleccion()
        {
            try
            {
                ddlRol.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                rptGruposSub.DataSource = null;
                rptGruposSub.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool ValidaCapturaGrupos()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                if (GruposSeleccionados.Count <= 0)
                    sb.AppendLine("<li>Debe asignar al menos un grupo.</li>");


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
                AlertaGeneral = _lstError;
                return false;
            }


            return true;
        }

        public void Limpiar()
        {
            try
            {
                ddlRol.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ViewState["GruposSeleccionados"] = null;
                GruposSeleccionados = null;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void EliminarSeleccion(int idGrupo, bool esSubGrupo)
        {
            try
            {
                List<HelperAsignacionRol> roles = GruposSeleccionados;
                int idRolPadre = 0;
                bool eliminaRol = false;
                foreach (HelperAsignacionRol rol in roles)
                {
                    if (rol.Grupos.Any(a => a.IdGrupo == idGrupo))
                    {
                        idRolPadre = rol.IdRol;
                        rol.Grupos.Remove(rol.Grupos.Single(s => s.IdGrupo == idGrupo));
                        if (!rol.Grupos.Any())
                        {
                            eliminaRol = true;
                        }
                    }
                }
                if (eliminaRol)
                    roles.Remove(roles.Single(s => s.IdRol == idRolPadre));
                ViewState["GruposSeleccionados"] = roles;
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaGeneral = new List<string>();
                //lblBrandingModal.Text = WebConfigurationManager.AppSettings["Brand"];
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
        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarSeleccion();
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
        protected void ddlRol_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlRol.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    LimpiarSeleccion();
                    return;
                }
                List<GrupoUsuario> grupos = AsignacionAutomatica ? _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol(int.Parse(ddlRol.SelectedValue), false) : _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRolTipoUsuario(int.Parse(ddlRol.SelectedValue), IdTipoUsuario, false);
                rptGruposSub.DataSource = grupos;
                rptGruposSub.DataBind();
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
        protected void rptGruposSub_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                List<HelperSubGurpoUsuario> lstSubRoles = _servicioSubGrupoUsuario.ObtenerSubGruposUsuario(((GrupoUsuario)e.Item.DataItem).Id, false);
                CheckBox chkGrupo = (CheckBox)e.Item.FindControl("chkGrupo");
                if (lstSubRoles.Count > 0)
                {
                    chkGrupo.CssClass = "col-lg-12 col-md-12";
                    e.Item.FindControl("divSubGrupos").Visible = true;
                    Repeater rpt = ((Repeater)e.Item.FindControl("rptSubGrupos"));
                    rpt.DataSource = lstSubRoles;
                    rpt.DataBind();
                }
                else
                {
                    e.Item.FindControl("divSubGrupos").Visible = false;
                    chkGrupo.CssClass = "col-lg-4 col-md-4 no-padding-left";
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
        protected void chkGrupo_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk.NamingContainer.FindControl("divSubGrupos").Visible)
                {
                    Repeater rpt = (Repeater)chk.NamingContainer.FindControl("rptSubGrupos");
                    foreach (RepeaterItem item in rpt.Items)
                    {
                        ((CheckBox)item.FindControl("chkSubGrupo")).Enabled = chk.Checked;
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
                AlertaGeneral = _lstError;
            }
        }
        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<HelperAsignacionRol> lstFinal = ViewState["GruposSeleccionados"] == null ? new List<HelperAsignacionRol>() : (List<HelperAsignacionRol>)ViewState["GruposSeleccionados"];
                List<HelperAsignacionRol> tmplst = new List<HelperAsignacionRol>();
                HelperAsignacionRol rol = tmplst.SingleOrDefault(s => s.IdRol == int.Parse(ddlRol.SelectedValue)) ?? new HelperAsignacionRol
                {
                    IdRol = int.Parse(ddlRol.SelectedValue),
                    DescripcionRol = ddlRol.SelectedItem.Text,
                    Grupos = new List<HelperAsignacionGrupoUsuarios>()
                };
                foreach (RepeaterItem item in rptGruposSub.Items)
                {
                    CheckBox chkGrupo = (CheckBox)item.FindControl("chkGrupo");
                    if (!chkGrupo.Checked) continue;
                    Label lblIdGrupo = (Label)item.FindControl("lblIdGrupo");
                    Label lblTipoGrupo = (Label)item.FindControl("lblTipoGrupo");

                    if (rol.Grupos != null && rol.Grupos.Any(a => a.IdGrupo == int.Parse(lblIdGrupo.Text)))
                    {
                        rol.Grupos.Remove(rol.Grupos.Single(s => s.IdGrupo == int.Parse(lblIdGrupo.Text)));
                    }
                    HelperAsignacionGrupoUsuarios grupo = new HelperAsignacionGrupoUsuarios
                    {
                        IdGrupo = int.Parse(lblIdGrupo.Text),
                        IdTipoGrupo = int.Parse(lblTipoGrupo.Text),
                        DescripcionGrupo = chkGrupo.Text,
                    };
                    if (item.FindControl("divSubGrupos").Visible)
                    {
                        grupo.SubGrupos = new List<HelperSubGurpoUsuario>();
                        Repeater rpt = (Repeater)item.FindControl("rptSubGrupos");
                        foreach (RepeaterItem repeaterItem in rpt.Items)
                        {
                            CheckBox chkSubGrupo = (CheckBox)repeaterItem.FindControl("chkSubGrupo");
                            if (chkSubGrupo.Checked)
                            {
                                Label lblIdSubGrupo = (Label)repeaterItem.FindControl("lblIdSubGpo");
                                grupo.SubGrupos.Add(new HelperSubGurpoUsuario { Id = int.Parse(lblIdSubGrupo.Text), Descripcion = chkSubGrupo.Text });
                            }
                        }
                        if (!grupo.SubGrupos.Any())
                            throw new Exception("Seleccione un subGrupo");
                    }
                    if (rol.Grupos != null) 
                        rol.Grupos.Add(grupo);
                }
                if (rol.Grupos == null || rol.Grupos.Count <= 0)
                    throw new Exception("Seleccione un Grupo");
                tmplst.Add(rol);
                foreach (HelperAsignacionRol asignacionRol in tmplst)
                {
                    if (lstFinal.Any(a => a.IdRol == asignacionRol.IdRol))
                    {
                        lstFinal.Remove(lstFinal.Single(a => a.IdRol == asignacionRol.IdRol));
                    }
                }
                lstFinal.AddRange(tmplst);
                ViewState["GruposSeleccionados"] = lstFinal;
                LimpiarSeleccion();
                if (OnTerminarModal != null)
                    OnTerminarModal();
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
    }
}