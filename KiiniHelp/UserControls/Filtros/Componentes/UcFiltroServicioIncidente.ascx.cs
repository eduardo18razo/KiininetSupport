using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KinniNet.Business.Utils;
using Telerik.Web.UI;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroServicioIncidente : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoArbolAccesoClient _servicioGrupoUsuario = new ServiceTipoArbolAccesoClient();
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
        public List<int> TipoArbolSeleccionados
        {
            get
            {
                return (from RadComboBoxItem item in rcbFiltroServicioIncidente.CheckedItems select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaTipoArbolTicket()
        {
            try
            {
                rcbFiltroServicioIncidente.DataSource = _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false).Where(w => w.Id != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion);
                rcbFiltroServicioIncidente.DataTextField = "Descripcion";
                rcbFiltroServicioIncidente.DataValueField = "Id";
                rcbFiltroServicioIncidente.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaTipoArbolConsulta()
        {
            try
            {
                rcbFiltroServicioIncidente.DataSource = _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false).Where(w => w.Id == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion);
                rcbFiltroServicioIncidente.DataTextField = "Descripcion";
                rcbFiltroServicioIncidente.DataValueField = "Id";
                rcbFiltroServicioIncidente.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaTipoArbolEncuesta()
        {
            try
            {
                rcbFiltroServicioIncidente.DataSource = _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false);
                rcbFiltroServicioIncidente.DataTextField = "Descripcion";
                rcbFiltroServicioIncidente.DataValueField = "Id";
                rcbFiltroServicioIncidente.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool EsTicket
        {
            get { return Convert.ToBoolean(hfticket.Value); }
            set
            {
                if (value)
                    LlenaTipoArbolTicket();
                hfticket.Value = value.ToString();
            }
        }

        public bool EsConsulta
        {
            get { return Convert.ToBoolean(hfticket.Value); }
            set
            {
                if (value)
                    LlenaTipoArbolConsulta();
                hfticket.Value = value.ToString();
            }
        }

        public bool EsEncuesta
        {
            get { return Convert.ToBoolean(hfticket.Value); }
            set
            {
                if (value)
                    LlenaTipoArbolEncuesta();
                hfticket.Value = value.ToString();
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