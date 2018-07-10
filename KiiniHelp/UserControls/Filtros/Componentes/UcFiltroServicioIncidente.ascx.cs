using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSistemaTipoArbolAcceso;
using KinniNet.Business.Utils;

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
                return (from ListItem item in lstFiltroServicioIncidente.Items where item.Selected select int.Parse(item.Value)).ToList();
            }
        }
        private void LlenaTipoArbolTicket()
        {
            try
            {
                Metodos.LlenaListBoxCatalogo(lstFiltroServicioIncidente, _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false).Where(w => w.Id != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion));
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
                Metodos.LlenaListBoxCatalogo(lstFiltroServicioIncidente, _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false).Where(w => w.Id == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion));
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
                Metodos.LlenaListBoxCatalogo(lstFiltroServicioIncidente, _servicioGrupoUsuario.ObtenerTiposArbolAcceso(false));
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    Session["TipoArbolSeleccionado"] = null;
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