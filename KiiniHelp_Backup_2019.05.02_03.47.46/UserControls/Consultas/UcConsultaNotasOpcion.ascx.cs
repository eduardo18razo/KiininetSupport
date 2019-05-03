using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceNota;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaNotasOpcion : UserControl
    {
        private readonly ServiceNotaClient _servicioNotas = new ServiceNotaClient();

        private List<string> _lstError = new List<string>();

        public List<string> Alerta
        {
            set
            {
                panelAlertaGeneral.Visible = value.Any();
                if (!panelAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }
        private void LlenaNotasConsulta(string filtro = "")
        {
            try
            {
                List<HelperNotasOpcion> notas = _servicioNotas.ObtenerNotasOpcion(((Usuario)Session["UserData"]).Id, false);
                if (filtro != string.Empty)
                    notas = notas.Where(w => w.Nombre.Contains(filtro)).ToList();
                rptResultados.DataSource = notas;
                rptResultados.DataBind();
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
                ucAltaNotaOpcion.OnAceptarModal += AltaNotaOnAceptarModal;
                ucAltaNotaOpcion.OnCancelarModal += AltaNotaOnCancelarModal;
                LlenaNotasConsulta();
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
        private void AltaNotaOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaNotaGeneral\");", true);
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
        private void AltaNotaOnAceptarModal()
        {
            try
            {
                LlenaNotasConsulta();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaNotaGeneral\");", true);
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
        protected void btnBaja_OnClick(object sender, EventArgs e)
        {
            try
            {
                //_servicioAreas.Habilitar(Convert.ToInt32(hfId.Value), false);
                LlenaNotasConsulta();
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
        protected void btnAlta_OnClick(object sender, EventArgs e)
        {
            try
            {
                //_servicioAreas.Habilitar(Convert.ToInt32(hfId.Value), true);
                LlenaNotasConsulta();
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
        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //ucAltaArea.EsAlta = false;
                //Area puesto = _servicioAreas.ObtenerAreaById(Convert.ToInt32(hfId.Value));
                //if (puesto == null) return;
                //ucAltaArea.IdArea = puesto.Id;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaArea\");", true);
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
        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucAltaNotaOpcion.EsAlta = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaNotaGeneral\");", true);
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
        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LlenaNotasConsulta(txtFiltro.Text.Trim().ToUpper());
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