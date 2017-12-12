using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceDiasHorario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaDiasFestivos : UserControl, IControllerModal
    {
        private readonly ServiceDiasHorarioClient _servicioDias = new ServiceDiasHorarioClient();
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

        public int IdGrupoEditar
        {
            get { return int.Parse(hdIdGrupoDias.Value); }
            set
            {
                hdIdGrupoDias.Value = value.ToString();
                DiasFeriados grupoEdicion = _servicioDias.ObtenerDiasFeriadosUserById(value);

                if (grupoEdicion != null)
                {
                    txtDescripcionDias.Text = grupoEdicion.Descripcion;
                    foreach (DiasFeriadosDetalle detalle in grupoEdicion.DiasFeriadosDetalle)
                    {
                        DiaFeriado diaAdd = new DiaFeriado();
                        diaAdd.Descripcion = detalle.Descripcion;
                        diaAdd.Fecha = detalle.Dia;
                        diaAdd.Id = _servicioDias.ObtenerDiaByFecha(detalle.Dia).Id;
                        DiasFeriadosDetalle.Add(diaAdd);
                    }
                    LlenaDias();
                }
                if (EsAlta)
                    hdIdGrupoDias.Value = "0";
            }
        }
        private List<DiaFeriado> DiasFeriadosDetalle
        {
            get { return (List<DiaFeriado>)Session["DiasSeleccionados"]; }
            set { Session["DiasSeleccionados"] = value; }
        }

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set
            {
                DiasFeriadosDetalle = new List<DiaFeriado>();
                hfEsAlta.Value = value.ToString();
                if (value)
                    IdGrupoEditar = 0;
            }
        }
        public int IdDiaFeriadoEdicion
        {
            get { return int.Parse(hfIdDiaFeriado.Value); }
            set
            {
                hfIdDiaFeriado.Value = value.ToString();
                DiaFeriado dia = _servicioDias.ObtenerDiaFeriado(value);
                if (dia != null)
                {
                    txtDescripcionDia.Text = dia.Descripcion;
                    txtDate.Text = dia.Fecha.ToString("yyyy-MM-dd");
                }
            }
        }
        private void LimpiarPantalla()
        {
            try
            {
                txtDescripcionDias.Text = string.Empty;
                LimpiaCapturaDias();
                DiasFeriadosDetalle = new List<DiaFeriado>();
                LlenaDias();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LimpiaCapturaDias()
        {
            try
            {
                hfEditando.Value = "0";
                IdDiaFeriadoEdicion = 0;
                txtDescripcionDia.Text = string.Empty;
                txtDate.Text = string.Empty;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LlenaCombos()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlDiasFeriados, _servicioDias.ObtenerDiasFeriados(true));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LlenaDias()
        {
            try
            {
                rptDias.DataSource = DiasFeriadosDetalle.OrderBy(o => o.Fecha).ToList();
                rptDias.DataBind();
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
                    LlenaCombos();
                    txtDate.Focus();
                    txtDescripcionDias.Focus();
                    txtDate.Focus();
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
                if (ddlDiasFeriados.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un día feriado");
                int idDiaFeriado = int.Parse(ddlDiasFeriados.SelectedValue);
                DiaFeriado selectedDay = _servicioDias.ObtenerDiaFeriado(idDiaFeriado);
                List<DiaFeriado> tmpSeleccionados = DiasFeriadosDetalle ?? new List<DiaFeriado>();
                if (hfEditando.Value != string.Empty && hfEditando.Value != "0")
                {
                    tmpSeleccionados.Single(a => a.Fecha == selectedDay.Fecha).Descripcion = txtDescripcionDia.Text.ToUpper();
                    tmpSeleccionados.Single(a => a.Fecha == selectedDay.Fecha).Fecha = Convert.ToDateTime(txtDate.Text);
                }
                else
                {
                    if (tmpSeleccionados.Any(a => a.Fecha == selectedDay.Fecha))
                        throw new Exception("Ya se ha ingresado esta fecha");
                    tmpSeleccionados.Add(selectedDay);
                }

                DiasFeriadosDetalle = tmpSeleccionados;
                LlenaDias();
                ddlDiasFeriados.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
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
        protected void btnAddDiaDescanso_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDate.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese una fecha");
                if (txtDescripcionDia.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese una descripción");
                if (DateTime.Parse(txtDate.Text) < DateTime.Parse(DateTime.Now.ToShortDateString()))
                    throw new Exception("La fecha no puede ser anterior al dia actual");

                DiaFeriado newDay = new DiaFeriado { Id = IdDiaFeriadoEdicion == 0 ? 0 : IdDiaFeriadoEdicion, Descripcion = txtDescripcionDia.Text, Fecha = Convert.ToDateTime(txtDate.Text), IdUsuarioAlta = ((Usuario)Session["UserData"]).Id };
                _servicioDias.AgregarDiaFeriado(newDay);
                if (newDay.Id != 0)
                {
                    DiasFeriadosDetalle.Single(s => s.Id == newDay.Id).Descripcion = newDay.Descripcion;
                    DiasFeriadosDetalle.Single(s => s.Id == newDay.Id).Fecha = newDay.Fecha;
                    LlenaDias();
                }
                LimpiaCapturaDias();
                LlenaCombos();
                UsuariosMaster master = (UsuariosMaster)Page.Master;
                if (master != null)
                    master.AlertaSucces();
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
        protected void lnkBtnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                if (btn != null)
                {
                    DateTime fecha = Convert.ToDateTime(btn.CommandArgument);
                    IdDiaFeriadoEdicion = _servicioDias.ObtenerDiaByFecha(fecha).Id;
                    hfEditando.Value = btn.CommandArgument;
                    //List<DiaFeriado> tmpSeleccionados = DiasFeriados ?? new List<DiaFeriado>();
                    //DiaFeriado diaSeleccion = tmpSeleccionados.Single(f => f.Fecha == fecha);
                    //txtDescripcionDia.Text = diaSeleccion.Descripcion;
                    //txtDate.Text = diaSeleccion.Fecha.ToString("yyyy-MM-dd");
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
        protected void lbkBtnBorrar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                if (btn != null)
                {
                    DateTime fecha = Convert.ToDateTime(btn.CommandArgument);
                    List<DiaFeriado> tmpSeleccionados = DiasFeriadosDetalle ?? new List<DiaFeriado>();
                    tmpSeleccionados.Remove(tmpSeleccionados.Single(f => f.Fecha == fecha));
                    DiasFeriadosDetalle = tmpSeleccionados;
                    LlenaDias();
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
        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDescripcionDias.Text.Trim() == string.Empty)
                    throw new Exception("Ingrese un nombre");
                if (DiasFeriadosDetalle == null || DiasFeriadosDetalle.Count <= 0)
                    throw new Exception("Ingrese la menos un día");
                if (EsAlta)
                {
                    DiasFeriados newDias = new DiasFeriados();
                    newDias.Descripcion = txtDescripcionDias.Text;
                    newDias.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;
                    newDias.DiasFeriadosDetalle = new List<DiasFeriadosDetalle>();
                    foreach (DiaFeriado feriado in DiasFeriadosDetalle)
                    {
                        DiaFeriado day = _servicioDias.ObtenerDiaFeriado(feriado.Id);
                        if (day != null)
                        {
                            newDias.DiasFeriadosDetalle.Add(new DiasFeriadosDetalle
                            {
                                Dia = day.Fecha,
                                Descripcion = day.Descripcion,
                                Habilitado = true
                            });
                        }
                    }
                    _servicioDias.CrearDiasFestivos(newDias);
                }
                else
                {
                    DiasFeriados newDias = new DiasFeriados();
                    newDias.Id = IdGrupoEditar;
                    newDias.Descripcion = txtDescripcionDias.Text;
                    newDias.DiasFeriadosDetalle = new List<DiasFeriadosDetalle>();
                    newDias.IdUsuarioModifico = ((Usuario)Session["UserData"]).Id;
                    foreach (DiaFeriado feriado in DiasFeriadosDetalle)
                    {
                        DiaFeriado day = _servicioDias.ObtenerDiaFeriado(feriado.Id);
                        if (day != null)
                        {
                            newDias.DiasFeriadosDetalle.Add(new DiasFeriadosDetalle
                            {
                                Dia = day.Fecha,
                                Descripcion = day.Descripcion,
                                Habilitado = true
                            });
                        }
                    }
                    _servicioDias.ActualizarDiasFestivos(newDias);
                }
                LlenaCombos();
                LimpiarPantalla();
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
                LimpiarPantalla();
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
                LimpiarPantalla();
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

        protected void rptDias_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //e.Item.FindControl("lnkBtnEditar").Visible = EsAlta;
                    e.Item.FindControl("lblSeparador").Visible = EsAlta;
                    e.Item.FindControl("lbkBtnBorrar").Visible = EsAlta;
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