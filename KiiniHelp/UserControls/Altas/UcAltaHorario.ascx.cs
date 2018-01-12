using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using KiiniHelp.ServiceDiasHorario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaHorario : UserControl, IControllerModal
    {
        private readonly ServiceDiasHorarioClient _servicioHorario = new ServiceDiasHorarioClient();
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

        public bool EsAlta
        {
            get { return Convert.ToBoolean(hfEsAlta.Value); }
            set
            {
                hfEsAlta.Value = value.ToString();
                lbltitulo.Text = value ? "Nuevo Horario" : "Editar Horario";            
            }
        }


        //public bool Alta
        //{
        //    get { return Convert.ToBoolean(ViewState["Alta"].ToString()); }
        //    set
        //    {
        //        ViewState["Alta"] = value.ToString();
        //        //ddlTipoUsuario.Enabled = value;
        //        //ddlTipoGrupo.Enabled = value;
                
        //}

        public int IdHorario
        {
            get { return int.Parse(hdIdHorario.Value); }
            set
            {
                hdIdHorario.Value = value.ToString();
                Horario horario = _servicioHorario.ObtenerHorarioById(value);
                if (horario != null)
                {
                    txtDescripcion.Text = horario.Descripcion;
                    hfLunes.Value = string.Empty;
                    hfMartes.Value = string.Empty;
                    hfMiercoles.Value = string.Empty;
                    hfJueves.Value = string.Empty;
                    hfViernes.Value = string.Empty;
                    hfSabado.Value = string.Empty;
                    hfDomingo.Value = string.Empty;

                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 1))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour; horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23 ? DateTime.Parse(detalle.HoraInicio).Hour : DateTime.Parse(detalle.HoraFin).Hour); horainicio++)
                        {
                            hfLunes.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfLunes.Value += "23,";
                    }
                    hfLunes.Value = hfLunes.Value.TrimEnd(',');
                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 2))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour;horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23? DateTime.Parse(detalle.HoraInicio).Hour: DateTime.Parse(detalle.HoraFin).Hour);horainicio++)
                        {
                            hfMartes.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfMartes.Value += "23,";
                    }
                    hfMartes.Value = hfMartes.Value.TrimEnd(',');

                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 3))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour; horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23 ? DateTime.Parse(detalle.HoraInicio).Hour : DateTime.Parse(detalle.HoraFin).Hour); horainicio++)
                        {
                            hfMiercoles.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfMiercoles.Value += "23,";
                    }
                    hfMiercoles.Value = hfMiercoles.Value.TrimEnd(',');

                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 4))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour; horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23 ? DateTime.Parse(detalle.HoraInicio).Hour : DateTime.Parse(detalle.HoraFin).Hour); horainicio++)
                        {
                            hfJueves.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfJueves.Value += "23,";
                    }
                    hfJueves.Value = hfJueves.Value.TrimEnd(',');

                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 5))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour; horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23 ? DateTime.Parse(detalle.HoraInicio).Hour : DateTime.Parse(detalle.HoraFin).Hour); horainicio++)
                        {
                            hfViernes.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfViernes.Value += "23,";
                    }
                    hfViernes.Value = hfViernes.Value.TrimEnd(',');

                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 6))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour; horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23 ? DateTime.Parse(detalle.HoraInicio).Hour : DateTime.Parse(detalle.HoraFin).Hour); horainicio++)
                        {
                            hfSabado.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfSabado.Value += "23,";
                    }
                    hfSabado.Value = hfSabado.Value.TrimEnd(',');

                    foreach (HorarioDetalle detalle in horario.HorarioDetalle.Where(w => w.Dia == 0))
                    {
                        for (int horainicio = DateTime.Parse(detalle.HoraInicio).Hour; horainicio < (DateTime.Parse(detalle.HoraInicio).Hour == 23 ? DateTime.Parse(detalle.HoraInicio).Hour : DateTime.Parse(detalle.HoraFin).Hour); horainicio++)
                        {
                            hfDomingo.Value += horainicio + ",";
                        }
                        if (DateTime.Parse(detalle.HoraInicio).Hour == 23)
                            hfDomingo.Value += "23,";
                    }
                    hfDomingo.Value = hfDomingo.Value.TrimEnd(',');
                }
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptHorario", "SetTable();", true);

            }
        }

        private void LimpiarCampos()
        {
            try
            {
                txtDescripcion.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptHorario", "SetTable();", true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LimpiarPantalla()
        {
            try
            {
                LimpiarCampos();
                hfLunes.Value = string.Empty;
                hfMartes.Value = string.Empty;
                hfMiercoles.Value = string.Empty;
                hfJueves.Value = string.Empty;
                hfViernes.Value = string.Empty;
                hfSabado.Value = string.Empty;
                hfDomingo.Value = string.Empty;
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
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptHorario", "SetTable();", true);
                if (!IsPostBack)
                {
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

        private void ValidaCapturaHorario()
        {
            StringBuilder sb = new StringBuilder();
            if (txtDescripcion.Text == string.Empty)
                sb.Append("Debe especificar un nombre.");
            if (hfLunes.Value == string.Empty && hfMartes.Value == string.Empty && hfMiercoles.Value == string.Empty && hfJueves.Value == string.Empty && hfViernes.Value == string.Empty && hfSabado.Value == string.Empty && hfDomingo.Value == string.Empty)
            {
                sb.Append("Seleccione un rango de horas.");
            }
            if (sb.ToString() != string.Empty)
                throw new Exception(sb.ToString());
        }

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ValidaCapturaHorario();
                int[] diasLun = hfLunes.Value != string.Empty ? hfLunes.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                int[] diasMar = hfMartes.Value != string.Empty ? hfMartes.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                int[] diasMie = hfMiercoles.Value != string.Empty ? hfMiercoles.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                int[] diasJue = hfJueves.Value != string.Empty ? hfJueves.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                int[] diasVie = hfViernes.Value != string.Empty ? hfViernes.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                int[] diasSab = hfSabado.Value != string.Empty ? hfSabado.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                int[] diasDom = hfDomingo.Value != string.Empty ? hfDomingo.Value.Split(',').Select(int.Parse).ToArray() : new int[0];
                Horario nuevoHorario = new Horario
                {
                    Descripcion = txtDescripcion.Text,
                    HorarioDetalle = new List<HorarioDetalle>(),
                    Sistema = false,
                    IdUsuarioAlta = ((Usuario)Session["UserData"]).Id,
                };
                if (diasLun.Any())
                {
                    foreach (int i in diasLun)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 1,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (diasMar.Any())
                {
                    foreach (int i in diasMar)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 2,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (diasMie.Any())
                {
                    foreach (int i in diasMie)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 3,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (diasJue.Any())
                {
                    foreach (int i in diasJue)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 4,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (diasVie.Any())
                {
                    foreach (int i in diasVie)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 5,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (diasSab.Any())
                {
                    foreach (int i in diasSab)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 6,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (diasDom.Any())
                {
                    foreach (int i in diasDom)
                    {
                        HorarioDetalle detalle = new HorarioDetalle
                        {
                            Dia = 0,
                            HoraInicio = TimeSpan.FromHours(i).ToString(),
                            HoraFin = i == 23 ? "00:00:00" : TimeSpan.FromHours(i + 1).ToString()
                        };
                        nuevoHorario.HorarioDetalle.Add(detalle);
                    }
                }
                if (EsAlta)
                    _servicioHorario.CrearHorario(nuevoHorario);
                else
                {
                    nuevoHorario.Id = IdHorario;
                    nuevoHorario.IdUsuarioModifico = ((Usuario)Session["UserData"]).Id;
                    _servicioHorario.ActualizarHorario(nuevoHorario);
                }
                LimpiarPantalla();
                if (OnAceptarModal != null)
                    OnAceptarModal();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptHorario", "SetTable();", true);
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
    }
}