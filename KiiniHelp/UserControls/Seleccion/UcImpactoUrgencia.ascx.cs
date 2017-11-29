using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceImpactourgencia;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcImpactoUrgencia : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceImpactoUrgenciaClient _servicioImpactoUrgencia = new ServiceImpactoUrgenciaClient();

        private List<string> _lstError = new List<string>();

        private List<string> Alerta
        {
            set
            {
                panelAlertaImpacto.Visible = value.Any();
                if (!panelAlertaImpacto.Visible) return;
                rptErrorImpacto.DataSource = value;
                rptErrorImpacto.DataBind();
            }
        }

        private void LlenaCombo()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlPrioridad, _servicioImpactoUrgencia.ObtenerPrioridad(true));
                Metodos.LlenaComboCatalogo(ddlUrgencia, _servicioImpactoUrgencia.ObtenerUrgencia(true));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Limpiar()
        {
            try
            {
                Metodos.LimpiarCombo(ddlPrioridad);
                Metodos.LimpiarCombo(ddlUrgencia);
                LlenaCombo();
                ObtenerImpactoUrgencia();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Impacto ObtenerImpactoUrgencia()
        {
            Impacto result = null;
            try
            {
                if (ddlPrioridad.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione && ddlUrgencia.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    result = _servicioImpactoUrgencia.ObtenerImpactoByPrioridadUrgencia(Convert.ToInt32(ddlPrioridad.SelectedValue), Convert.ToInt32(ddlUrgencia.SelectedValue));
                divImpacto.Visible = result != null;
                if (result != null)
                {

                    if (result.Descripcion == "ALTO")
                        divImpacto.Attributes["class"] = "form-group bg-danger";
                    if (result.Descripcion == "MEDIO")
                        divImpacto.Attributes["class"] = "form-group bg-warning";
                    if (result.Descripcion == "BAJO")
                        divImpacto.Attributes["class"] = "form-group bg-success";
                    
                    lblImpacto.Text = result.Descripcion;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return result;
        }

        public void SetImpactoUrgencia(int? idImpacto)
        {
            try
            {
                if (idImpacto != null)
                {
                    Impacto impacto = _servicioImpactoUrgencia.ObtenerImpactoById((int) idImpacto);
                    ddlPrioridad.SelectedValue = impacto.IdPrioridad.ToString();
                    ddlUrgencia.SelectedValue = impacto.IdUrgencia.ToString();
                }
                ddlUrgencia_OnSelectedIndexChanged(ddlUrgencia, null);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    LlenaCombo();
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

        protected void btnAsignar_OnClick(object sender, EventArgs e)
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

        protected void ddlImpacto_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerImpactoUrgencia();
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

        protected void ddlUrgencia_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ObtenerImpactoUrgencia();
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