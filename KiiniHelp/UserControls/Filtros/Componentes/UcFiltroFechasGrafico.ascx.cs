using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Filtros.Componentes
{
    public partial class UcFiltroFechasGrafico : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

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

        public int TipoPeriodo
        {
            get { return Convert.ToInt32(ddlTipoFiltro.SelectedValue); }
            set { ddlTipoFiltro.SelectedValue = value.ToString(); }
        }
        public Dictionary<string, DateTime> RangoFechas
        {
            get
            {
                Dictionary<string, DateTime> result = null;

                switch (ddlTipoFiltro.SelectedValue)
                {
                    case "1":
                        if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                            throw new Exception("Debe Seleccionar un rango de fechas");
                        if (DateTime.Parse(txtFechaInicio.Text) > DateTime.Parse(txtFechaFin.Text))
                            throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                        result = new Dictionary<string, DateTime>
                        {
                            {"inicio", Convert.ToDateTime(txtFechaInicio.Text)},
                            {"fin", Convert.ToDateTime(txtFechaFin.Text)}
                        };
                        break;
                    case "2":
                        int anioInicialSemana = Convert.ToInt32(txtFechaInicio.Text.Split('-')[0]);
                        int semanaInicialSemana = Convert.ToInt32(txtFechaInicio.Text.Split('-')[1].Substring(1));
                        int anioFinSemana = Convert.ToInt32(txtFechaFin.Text.Split('-')[0]);
                        int semanaFinSemana = Convert.ToInt32(txtFechaFin.Text.Split('-')[1].Substring(1));

                        result = new Dictionary<string, DateTime>
                        {
                            {"inicio", BusinessCadenas.Fechas.ObtenerFechaInicioSemana(anioInicialSemana, semanaInicialSemana)},
                            {"fin", BusinessCadenas.Fechas.ObtenerFechaFinSemana(anioFinSemana, semanaFinSemana)}
                        };

                        break;
                    case "3":

                        int anioInicialMes = Convert.ToInt32(txtFechaInicio.Text.Split('-')[0]);
                        int mesInicialMes = Convert.ToInt32(txtFechaInicio.Text.Split('-')[1]);
                        int anioFinMes = Convert.ToInt32(txtFechaFin.Text.Split('-')[0]);
                        int mesFinMes = Convert.ToInt32(txtFechaFin.Text.Split('-')[1]);
                        if ((anioInicialMes > anioFinMes) || (mesInicialMes > mesFinMes))
                            throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");

                        result = new Dictionary<string, DateTime>
                        {
                            {"inicio", new DateTime(anioInicialMes, mesInicialMes, 1)},
                            {"fin", new DateTime(anioFinMes, mesFinMes, DateTime.DaysInMonth(anioFinMes, mesFinMes))}
                        };

                        break;
                    case "4":
                        if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                            throw new Exception("Debe Seleccionar un rango de fechas");
                        if (int.Parse(txtFechaInicio.Text) > int.Parse(txtFechaFin.Text))
                            throw new Exception("Año Inicio no puede se mayor a año Fin");
                        result = new Dictionary<string, DateTime>
                        {
                            {"inicio", new DateTime(int.Parse(txtFechaInicio.Text), 01, 1)},
                            {"fin", new DateTime(int.Parse(txtFechaFin.Text), 12, DateTime.DaysInMonth(int.Parse(txtFechaFin.Text), 12))}
                        };
                        break;
                }
                return result;
            }
        }

        private void ValidaFechas()
        {
            try
            {
                if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                    throw new Exception("Debe Seleccionar un rango de fechas");
                switch (ddlTipoFiltro.SelectedValue)
                {
                    case "1":
                        if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                            throw new Exception("Debe Seleccionar un rango de fechas");
                        if (DateTime.Parse(txtFechaInicio.Text) > DateTime.Parse(txtFechaFin.Text))
                            throw new Exception("Fecha Inicio no puede se mayor a Fecha Fin");
                        break;
                    case "2":
                        int anioInicialSemana = Convert.ToInt32(txtFechaInicio.Text.Split('-')[0]);
                        int semanaInicialSemana = Convert.ToInt32(txtFechaInicio.Text.Split('-')[1].Substring(1));
                        int anioFinSemana = Convert.ToInt32(txtFechaFin.Text.Split('-')[0]);
                        int semanaFinSemana = Convert.ToInt32(txtFechaFin.Text.Split('-')[1].Substring(1));
                        if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                            throw new Exception("Debe Seleccionar un rango de fechas");
                        if ((anioInicialSemana > anioFinSemana) || (semanaInicialSemana > semanaFinSemana))
                            throw new Exception("Semana Inicio no puede se mayor a Semana Fin");

                        break;
                    case "3":

                        int anioInicialMes = Convert.ToInt32(txtFechaInicio.Text.Split('-')[0]);
                        int semanaInicialMes = Convert.ToInt32(txtFechaInicio.Text.Split('-')[1]);
                        int anioFinMes = Convert.ToInt32(txtFechaFin.Text.Split('-')[0]);
                        int semanaFinMes = Convert.ToInt32(txtFechaFin.Text.Split('-')[1]);
                        if ((anioInicialMes > anioFinMes) || (semanaInicialMes > semanaFinMes))
                            throw new Exception("Mes Inicio no puede se mayor a Mes Fin");
                        break;
                    case "4":
                        if (txtFechaInicio.Text.Trim() == string.Empty || txtFechaFin.Text.Trim() == string.Empty)
                            throw new Exception("Debe Seleccionar un rango de fechas");
                        if (int.Parse(txtFechaInicio.Text) > int.Parse(txtFechaFin.Text))
                            throw new Exception("Año Inicio no puede se mayor a año Fin");
                        break;
                }


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
                ValidaFechas();
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
                txtFechaInicio.Text = string.Empty;
                txtFechaFin.Text = string.Empty;
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
                var valida = RangoFechas;
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


        protected void ddlTipoFiltro_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFechaInicio.Attributes["type"] != null)
                    txtFechaInicio.Attributes.Remove("type");
                if (txtFechaFin.Attributes["type"] != null)
                    txtFechaFin.Attributes.Remove("type");

                if (txtFechaInicio.Attributes["min"] != null)
                    txtFechaInicio.Attributes.Remove("type");
                if (txtFechaFin.Attributes["max"] != null)
                    txtFechaFin.Attributes.Remove("max");

                switch (ddlTipoFiltro.SelectedValue)
                {
                    case "1":
                        txtFechaInicio.Attributes["type"] = "date";
                        txtFechaFin.Attributes["type"] = "date";
                        txtFechaFin.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
                        break;
                    case "2":
                        txtFechaInicio.Attributes["type"] = "week";
                        txtFechaFin.Attributes["type"] = "week";
                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        DateTime date1 = new DateTime(DateTime.Now.Year, 12, 31);
                        Calendar cal = dfi.Calendar;
                        txtFechaInicio.Attributes["min"] = string.Format("{0}-W{1}", DateTime.Now.Year, "01");
                        txtFechaInicio.Attributes["max"] = string.Format("{0}-W{1}", DateTime.Now.Year, cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek));

                        break;
                    case "3":
                        txtFechaInicio.Attributes["type"] = "month";
                        txtFechaFin.Attributes["type"] = "month";
                        txtFechaInicio.Attributes["min"] = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM");
                        txtFechaInicio.Attributes["max"] = new DateTime(DateTime.Now.Year, 12, 31).ToString("yyyy-MM");

                        break;
                    case "4":
                        txtFechaInicio.Attributes["type"] = "number";
                        txtFechaFin.Attributes["type"] = "number";
                        txtFechaInicio.Attributes["min"] = "2000";
                        txtFechaInicio.Attributes["max"] = DateTime.Now.Year.ToString();
                        txtFechaInicio.Text = DateTime.Now.AddYears(-1).Year.ToString();
                        txtFechaFin.Text = DateTime.Now.Year.ToString();
                        break;
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