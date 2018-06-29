using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroGrupoEncuesta : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
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
        public List<int> GruposSeleccionados
        {
            get
            {
                List<int> result = new List<int>();
                foreach (RepeaterItem item in rptGpos.Items.Cast<RepeaterItem>().Where(item => item.ItemType == ListItemType.Item))
                {
                    CheckBox chk = (CheckBox)item.FindControl("chkSeleccion");
                    if (chk != null)
                    {
                        if (chk.Checked)
                            result.Add(int.Parse(chk.Attributes["data-id"]));
                    }
                }
                return result;
                //if (rptGpoSeleccionado.Items.Count <= 0)
                //    throw new Exception("Debe seleccionar un grupo");
                //return (from RepeaterItem item in rptGpoSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }
        private void LlenaGrupos()
        {
            try
            {
                List<int> filtroGrupos = new List<int>
                {
                    (int) BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría,
                    (int) BusinessVariables.EnumTiposGrupos.ResponsableDeContenido,
                    (int) BusinessVariables.EnumTiposGrupos.Agente,
                    (int) BusinessVariables.EnumTiposGrupos.ResponsableDeOperación,
                    (int) BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo,
                    (int) BusinessVariables.EnumTiposGrupos.ConsultasEspeciales
                };

                rptGpos.DataSource = _servicioGrupoUsuario.ObtenerGruposByIdUsuario(((Usuario)Session["UserData"]).Id, false).Where(w => w.IdTipoGrupo != (int)BusinessVariables.EnumTiposGrupos.Usuario && filtroGrupos.Contains(w.IdTipoGrupo));
                rptGpos.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaGruposSeleccionados()
        {
            try
            {
                rptGpoSeleccionado.DataSource = Session["GruposSeleccionados"];
                rptGpoSeleccionado.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void Limpiar()
        {
            try
            {
                Session["GruposSeleccionados"] = null;
                LlenaGruposSeleccionados();
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
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Session["GruposSeleccionados"] = null;
                    LlenaGrupos();
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

        protected void btnSeleccionar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<GrupoUsuario> lst = Session["GruposSeleccionados"] == null ? new List<GrupoUsuario>() : (List<GrupoUsuario>)Session["GruposSeleccionados"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptGpos.Items[index].FindControl("lblId");
                        Label lblIdTipoUsuario = (Label)rptGpos.Items[index].FindControl("lblIdTipoUsuario");
                        Label lblTipoUsuario = (Label)rptGpos.Items[index].FindControl("lblTipoUsuario");
                        Label lblDescripcion = (Label)rptGpos.Items[index].FindControl("lblDescripcion");

                        if (!lst.Any(a => a.Id == int.Parse(lblIdGrupo.Text)))
                            lst.Add(new GrupoUsuario
                            {
                                Id = Convert.ToInt32(lblIdGrupo.Text),
                                IdTipoUsuario = Convert.ToInt32(lblIdTipoUsuario.Text),
                                TipoUsuario = new TipoUsuario { Descripcion = lblTipoUsuario.Text, Id = Convert.ToInt32(lblIdTipoUsuario.Text) },
                                Descripcion = lblDescripcion.Text
                            });
                    }
                }
                Session["GruposSeleccionados"] = lst;
                LlenaGruposSeleccionados();
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

        protected void btnQuitar_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<GrupoUsuario> lst = Session["GruposSeleccionados"] == null ? new List<GrupoUsuario>() : (List<GrupoUsuario>)Session["GruposSeleccionados"];
                Button button = (sender as Button);
                if (button != null)
                {
                    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    if (item != null)
                    {
                        int index = item.ItemIndex;
                        Label lblIdGrupo = (Label)rptGpoSeleccionado.Items[index].FindControl("lblId");

                        lst.Remove(lst.Single(s => s.Id == int.Parse(lblIdGrupo.Text)));
                    }
                }
                Session["GruposSeleccionados"] = lst;
                LlenaGruposSeleccionados();
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

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {

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

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Limpiar();
                if (OnLimpiarModal != null)
                    OnLimpiarModal();
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

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                var valida = GruposSeleccionados;
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
    }
}