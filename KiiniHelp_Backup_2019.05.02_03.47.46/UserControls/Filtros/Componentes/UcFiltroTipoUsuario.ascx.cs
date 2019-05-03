using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceSistemaTipoUsuario;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroTipoUsuario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _serviciotipoUsuario = new ServiceTipoUsuarioClient();
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

        private void LlenaTipoUsuario()
        {
            try
            {
                rcbFiltroTipoUsuario.DataSource = _serviciotipoUsuario.ObtenerTiposUsuario(false).OrderBy(s => s.Descripcion);
                rcbFiltroTipoUsuario.DataTextField = "Descripcion";
                rcbFiltroTipoUsuario.DataValueField = "Id";
                rcbFiltroTipoUsuario.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<int> TipoUsuarioSeleccionados
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroTipoUsuario.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }

        private void CerroListBox()
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaTipoUsuario();
                }
                if (IsPostBack)
                {
                    if (Page.Request.Params["__EVENTTARGET"] == "Seleccion")
                    {
                        CerroListBox();
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

    }
}