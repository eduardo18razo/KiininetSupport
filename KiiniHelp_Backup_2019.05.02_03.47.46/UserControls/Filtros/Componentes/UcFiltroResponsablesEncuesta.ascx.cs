﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroResponsablesEncuesta : UserControl, IControllerModal
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
                return (from RepeaterItem item in rptGpoSeleccionado.Items select int.Parse(((Label)item.FindControl("lblId")).Text)).ToList();
            }
            set { }
        }
        public void LlenaGrupos(List<int> grupos, List<int> tipoServicio)
        {
            try
            {
                rptGpos.DataSource = _servicioGrupoUsuario.ObtenerGruposUsuarioResponsablesByGruposTipoServicio(((Usuario)Session["UserData"]).Id, grupos, tipoServicio);
                rptGpos.DataBind();
                upResponsable.Update();
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
                    LlenaGrupos(new List<int>(),new List<int>() );
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