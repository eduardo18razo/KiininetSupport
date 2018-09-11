using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using OfficeOpenXml;
using Telerik.Web.UI;

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
                return (from RadComboBoxItem item in rcbFiltroGrupos.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaGrupos()
        {
            try
            {
                List<GrupoUsuario> lst = _servicioGrupoUsuario.ObtenerGruposUsuarioByIdRol((int)BusinessVariables.EnumRoles.Agente, false).OrderBy(o => o.Descripcion).ToList();
                rcbFiltroGrupos.DataSource = lst;
                rcbFiltroGrupos.DataTextField = "Descripcion";
                rcbFiltroGrupos.DataValueField = "Id";
                rcbFiltroGrupos.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                    LlenaGrupos();
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