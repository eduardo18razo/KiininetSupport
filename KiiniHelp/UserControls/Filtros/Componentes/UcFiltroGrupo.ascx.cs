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
    public partial class UcFiltroGrupo : UserControl, IControllerModal
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
                panelAlerta.Visible = value.Any();
                if (!panelAlerta.Visible) return;
                rptError.DataSource = value;
                rptError.DataBind();
            }
        }
        public List<int> GruposSeleccionados
        {
            get
            {
                if (rptGpoSeleccionado.Items.Count <= 0)
                    throw new Exception("Debe seleccionar un grupo");
                return (from RepeaterItem item in rptGpoSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
        }
        private void LlenaGrupos()
        {
            try
            {
                rptGpos.DataSource = _servicioGrupoUsuario.ObtenerGruposByIdUsuario(((Usuario)Session["UserData"]).Id, false).Where(w => w.IdTipoGrupo != (int)BusinessVariables.EnumTiposGrupos.Usuario);              
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
                        Label lblAbrevTipoUsuario = (Label)rptGpos.Items[index].FindControl("lblAbrevTipoUsuario");
                        HiddenField hfColor = (HiddenField)rptGpos.Items[index].FindControl("hfColor");
                        Label lblTipoGrupoDes = (Label)rptGpos.Items[index].FindControl("lblTipoGrupoDes");
                        if (!lst.Any(a => a.Id == int.Parse(lblIdGrupo.Text)))
                            lst.Add(new GrupoUsuario
                            {
                                Id = Convert.ToInt32(lblIdGrupo.Text),
                                IdTipoUsuario = Convert.ToInt32(lblIdTipoUsuario.Text),
                                TipoUsuario = new TipoUsuario { Descripcion = lblTipoUsuario.Text, Id = Convert.ToInt32(lblIdTipoUsuario.Text), Abreviacion = lblAbrevTipoUsuario.Text, Color = hfColor.Value.ToString() }, // 
                                Descripcion = lblDescripcion.Text,
                                TipoGrupo = new TipoGrupo {Descripcion = lblTipoGrupoDes.Text }
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

        protected void btnSeleccionarTodo_Click(object sender, EventArgs e)
        {
            try
            {
                List<GrupoUsuario> lst = Session["GruposSeleccionados"] == null ? new List<GrupoUsuario>() : (List<GrupoUsuario>)Session["GruposSeleccionados"];
                //Button button = (sender as Button);
                //if (button != null)
                //{
                //    RepeaterItem item = button.NamingContainer as RepeaterItem;
                    //if (item != null)
                for (int index = 0; index < rptGpos.Items.Count ; index++)
                {
                    //int index = item.ItemIndex;
                    Label lblIdGrupo = (Label)rptGpos.Items[index].FindControl("lblId");
                    Label lblIdTipoUsuario = (Label)rptGpos.Items[index].FindControl("lblIdTipoUsuario");
                    Label lblTipoUsuario = (Label)rptGpos.Items[index].FindControl("lblTipoUsuario");
                    Label lblDescripcion = (Label)rptGpos.Items[index].FindControl("lblDescripcion");
                    Label lblAbrevTipoUsuario = (Label)rptGpos.Items[index].FindControl("lblAbrevTipoUsuario");
                    HiddenField hfColor = (HiddenField)rptGpos.Items[index].FindControl("hfColor");
                    Label lblTipoGrupoDes = (Label)rptGpos.Items[index].FindControl("lblTipoGrupoDes");
                    if (!lst.Any(a => a.Id == int.Parse(lblIdGrupo.Text)))
                        lst.Add(new GrupoUsuario
                        {
                            Id = Convert.ToInt32(lblIdGrupo.Text),
                            IdTipoUsuario = Convert.ToInt32(lblIdTipoUsuario.Text),
                            TipoUsuario = new TipoUsuario { Descripcion = lblTipoUsuario.Text, Id = Convert.ToInt32(lblIdTipoUsuario.Text), Abreviacion = lblAbrevTipoUsuario.Text, Color = hfColor.Value.ToString() }, // 
                            Descripcion = lblDescripcion.Text,
                            TipoGrupo = new TipoGrupo { Descripcion = lblTipoGrupoDes.Text }
                        });                    
                }
                //}
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
    }
}