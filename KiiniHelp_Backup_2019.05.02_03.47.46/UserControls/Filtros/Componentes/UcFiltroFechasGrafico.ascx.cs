using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceParametrosSistema;
using KiiniNet.Entities.Parametros;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroFechasGrafico : UserControl, IControllerModal
    {
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

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

        public int IdReporteGrafico
        {
            get { return int.Parse(hfGrafico.Value); }
            set
            {
                hfGrafico.Value = value.ToString();
                LlenaFrecuencias();
                ObtenerFechasParametro();
            }
        }
        public int TipoPeriodo
        {
            get { return Convert.ToInt32(ddlTipoFiltro.SelectedValue); }
            set { ddlTipoFiltro.SelectedValue = value.ToString(); }
        }

        public string FechaInicio
        {
            get
            {
                string result;

                if (ValidaFechas())
                {
                    DateTime fecha;
                    DateTime.TryParse(txtFechaInicio.Text, out fecha);
                    result = fecha.ToString("dd/MM/yyyy");
                }
                else
                    result = string.Empty;

                return result;
            }
            set { txtFechaInicio.Text = value; }
        }

        public string FechaFin
        {
            get
            {
                string result;
                if (ValidaFechas())
                {
                    DateTime fecha;
                    DateTime.TryParse(txtFechaFin.Text, out fecha);
                    result = fecha.ToString("dd/MM/yyyy");
                }
                else
                    result = string.Empty;

                return result;
            }
            set { txtFechaFin.Text = value; }
        }

        public Dictionary<string, DateTime> RangoFechas
        {
            get
            {
                return Metodos.ManejoFechas.ObtenerFechas(int.Parse(ddlTipoFiltro.SelectedValue), txtFechaInicio.Text.Trim(), txtFechaFin.Text.Trim());
            }
        }

        private bool ValidaFechas()
        {
            try
            {
                if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                    return false;

                string fechaInicio = txtFechaInicio.Text, fechaFin = txtFechaFin.Text;
                if (fechaInicio.Trim() != string.Empty || fechaFin.Trim() != string.Empty)
                {
                    if (fechaInicio.Length < 10)
                    {
                        string[] fechaParserInicio = fechaInicio.Split('/');
                        if (fechaParserInicio.Length < 3)
                            throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                        fechaInicio = string.Empty;
                        for (int i = 0; i < fechaParserInicio.Length; i++)
                        {

                            if (i == 2)
                            {
                                fechaInicio += fechaParserInicio[i].PadLeft(4, '0');
                            }
                            else
                            {
                                fechaInicio += fechaParserInicio[i].PadLeft(2, '0') + '/';
                            }
                        }
                    }

                    if (fechaFin.Length < 10)
                    {
                        string[] fechaParserFin = fechaFin.Split('/');
                        if (fechaParserFin.Length < 3)
                            throw new Exception("Formato de fecha incorrecto dd/mm/yyyy");
                        fechaFin = string.Empty;
                        for (int i = 0; i < fechaParserFin.Length; i++)
                        {

                            if (i == 2)
                            {
                                fechaFin += fechaParserFin[i].PadLeft(4, '0');
                            }
                            else
                            {
                                fechaFin += fechaParserFin[i].PadLeft(2, '0') + '/';
                            }
                        }
                    }
                }

                switch (ddlTipoFiltro.SelectedValue)
                {
                    case "1":
                        if (fechaInicio == string.Empty || fechaFin == string.Empty)
                            return false;
                        if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) > DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                            return false;
                        break;
                    case "2":
                        if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                            return false;
                        if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) > DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                            return false;
                        break;
                    case "3":
                        if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                            return false;
                        if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) > DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                            return false;
                        break;
                    case "4":
                        if (fechaInicio.Trim() == string.Empty || fechaFin.Trim() == string.Empty)
                            return false;
                        if (DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", null) > DateTime.ParseExact(fechaFin, "dd/MM/yyyy", null))
                            return false;
                        break;
                }


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return true;
        }

        private void LlenaFrecuencias()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlTipoFiltro, _servicioParametros.ObtenerFrecuenciasFecha());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ObtenerFechasParametro()
        {
            try
            {
                GraficosDefault parametrosGrafico = _servicioParametros.ObtenerParametrosGraficoDefault(IdReporteGrafico);
                if (parametrosGrafico != null)
                {
                    ddlTipoFiltro.SelectedValue = parametrosGrafico.IdFrecuenciaFecha.ToString();
                    ddlTipoFiltro_OnSelectedIndexChanged(ddlTipoFiltro, null);
                    switch (parametrosGrafico.IdFrecuenciaFecha)
                    {
                        case 1:
                            txtFechaInicio.Text = DateTime.Now.AddDays(-parametrosGrafico.Periodo).ToString("dd/MM/yyyy");
                            txtFechaFin.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            break;
                        case 2:
                            txtFechaInicio.Text = string.Format("{0}", DateTime.Now.AddDays(-(parametrosGrafico.Periodo * 7)).ToString("dd/MM/yyyy"));
                            txtFechaFin.Text = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy"));
                            break;
                        case 3:
                            txtFechaInicio.Text = string.Format("{0}", DateTime.Now.AddMonths(-parametrosGrafico.Periodo).ToString("dd/MM/yyyy"));
                            txtFechaFin.Text = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy"));
                            break;
                        case 4:
                            txtFechaInicio.Text = string.Format("{0}", DateTime.Now.AddYears(-parametrosGrafico.Periodo).ToString("dd/MM/yyyy"));
                            txtFechaFin.Text = string.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy"));
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtener fechas parametro: " + ex.InnerException.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (FechaInicio == string.Empty && FechaFin == string.Empty)
                    {
                        LlenaFrecuencias();
                        ObtenerFechasParametro();
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

        protected void ddlTipoFiltro_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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

        protected void btnAplicar_OnClick(object sender, EventArgs e)
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
    }
}