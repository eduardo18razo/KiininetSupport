using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceDiasHorario;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniHelp.ServiceSistemaSubRol;
using KiiniHelp.ServiceSistemaTipoGrupo;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaGrupoUsuario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private readonly ServiceDiasHorarioClient _servicioDiaHorario = new ServiceDiasHorarioClient();
        private readonly ServiceGrupoUsuarioClient _servicioGrupoUsuario = new ServiceGrupoUsuarioClient();
        private readonly ServiceSubRolClient _servicioSistemaSubRol = new ServiceSubRolClient();
        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceTipoGrupoClient _servicioTipoGrupo = new ServiceTipoGrupoClient();
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

        public bool FromOpcion
        {
            get { return Convert.ToBoolean(hfFromOpcion.Value); }
            set { hfFromOpcion.Value = value.ToString(); }
        }

        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuario.SelectedValue); }
            set
            {
                ddlTipoUsuario.SelectedValue = value.ToString();
            }
        }

        public int IdTipoGrupo
        {
            get { return Convert.ToInt32(ddlTipoGrupo.SelectedValue); }
            set
            {
                ddlTipoGrupo.SelectedValue = value.ToString();

            }
        }

        public int IdRol
        {
            get { return Convert.ToInt32(btnCancelar.CommandArgument); }
            set { btnCancelar.CommandArgument = value.ToString(); }
        }

        public bool Alta
        {
            get { return Convert.ToBoolean(ViewState["Alta"].ToString()); }
            set
            {
                ViewState["Alta"] = value.ToString();
                ddlTipoUsuario.Enabled = value;
                ddlTipoGrupo.Enabled = value;
                lbltitulo.Text = value ? "Nuevo Grupo" : "Editar Grupo";
            }
        }

        public GrupoUsuario GrupoUsuario
        {
            get { return (GrupoUsuario)Session["GrupoUsuarioEditar"]; }
            set
            {
                try
                {
                    Session["GrupoUsuarioEditar"] = value;
                    IdTipoUsuario = value.IdTipoUsuario;
                    ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                    IdTipoGrupo = value.IdTipoGrupo;
                    ddlTipoGrupo_OnSelectedIndexChanged(ddlTipoGrupo, null);
                    txtDescripcionGrupoUsuario.Text = value.Descripcion;
                    if (value.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal)
                    {
                        rbtnLevanta.Checked = value.LevantaTicket;
                        rbtnRecado.Checked = value.RecadoTicket;
                    }
                    if (value.SubGrupoUsuario == null) return;
                    foreach (SubGrupoUsuario subGrupo in value.SubGrupoUsuario.OrderBy(o => o.IdSubRol))
                    {
                        foreach (RepeaterItem item in rptSubRoles.Items)
                        {
                            CheckBox chk = (CheckBox)item.FindControl("chkSubRol");
                            if (chk != null)
                            {
                                //Button btnHorarios = (Button)item.FindControl("btnHorarios");
                                //btnHorarios.CommandName = subGrupo.Id.ToString();

                                //Button btnDias = (Button)item.FindControl("btnDiasDescanso");
                                //btnDias.CommandName = subGrupo.Id.ToString();
                                if (subGrupo.IdSubRol == Convert.ToInt32(chk.Attributes["value"]))
                                {
                                    chk.Checked = subGrupo.IdSubRol == Convert.ToInt32(chk.Attributes["value"]);
                                    if (chk.Checked)
                                    {
                                        chk.Enabled = false;
                                        OnCheckedChanged(chk, null);
                                        if (subGrupo.HorarioSubGrupo != null && subGrupo.HorarioSubGrupo.Count > 0)
                                            ((DropDownList)item.FindControl("ddlHorario")).SelectedValue = subGrupo.HorarioSubGrupo.First(s => s.IdSubGrupoUsuario == subGrupo.Id).IdHorario.ToString();
                                        if (subGrupo.DiaFestivoSubGrupo != null && subGrupo.DiaFestivoSubGrupo.Count > 0)
                                            ((DropDownList)item.FindControl("ddlDiasFeriados")).SelectedValue = subGrupo.DiaFestivoSubGrupo.First(s => s.IdSubGrupoUsuario == subGrupo.Id).IdDiasFeriados.ToString();
                                        //CargaHorario();
                                        //ucAltaDiasFestivos.SetDiasFestivosSubRol(_servicioGrupoUsuario.ObtenerDiasByIdSubGrupo(subGrupo.Id), int.Parse(btnDias.CommandArgument));
                                        //CargaDias();
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private void ValidaCapturaGrupoUsuario()
        {
            List<string> sb = new List<string>();

            
            if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                sb.Add("Selecciones Tipo de Usuario.<br>");
            if (ddlTipoGrupo.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                sb.Add("Selecciones Tipo de Grupo.<br>");
            if (txtDescripcionGrupoUsuario.Text.Trim() == string.Empty)
                sb.Add("Descripcion es un campo obligatorio.<br>");

            if (ddlTipoGrupo.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione  && IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal)
            {
                if (!rbtnLevanta.Checked && !rbtnRecado.Checked)
                    sb.Add("Seleccione una opción para este grupo.<br>");
            }

            if (sb.Count > 0)
            {
                _lstError = sb;
                throw new Exception();
            }
        }

        private void LimpiarCampos()
        {
            try
            {
                IdTipoUsuario = BusinessVariables.ComboBoxCatalogo.ValueSeleccione;
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                txtDescripcionGrupoUsuario.Text = String.Empty;
                divParametros.Visible = false;
                divSubRoles.Visible = false;
                rptSubRoles.DataSource = null;
                rptSubRoles.DataBind();
                if (Session["GrupoUsuarioEditar"] != null)
                    foreach (SubGrupoUsuario subGrupo in ((GrupoUsuario)Session["GrupoUsuarioEditar"]).SubGrupoUsuario.OrderBy(o => o.IdSubRol))
                    {
                        Session.Remove("DiasFestivos" + subGrupo.IdSubRol);
                        Session.Remove(subGrupo.IdSubRol.ToString());
                    }
                rbtnLevanta.Checked = false;
                rbtnRecado.Checked = false;
                Session.Remove("GrupoUsuarioEditar");
                Session.Remove("HorariosSubRoles");
                Session.Remove("DiasSubRoles");
                Session.Remove("DiasFestivos");
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
                Alerta = new List<string>();
                //TODO: Se elimina para bloque de boton al click
                //btnGuardar.OnClientClick = "this.disabled = document.getElementById('form1').checkValidity(); if(document.getElementById('form1').checkValidity()){ " + Page.ClientScript.GetPostBackEventReference(btnGuardar, null) + ";}";  
                if (!IsPostBack)
                {
                    Metodos.LlenaComboCatalogo(ddlTipoUsuario, _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true));
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

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                const int idTipogrupo = (int)BusinessVariables.EnumTiposGrupos.Administrador;
                Metodos.LlenaComboCatalogo(ddlTipoGrupo, _servicioTipoGrupo.ObtenerTiposGruposByTipoUsuario(IdTipoUsuario, true).Where(w => w.Id != idTipogrupo));
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

        protected void ddlTipoGrupo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IdTipoGrupo = int.Parse(ddlTipoGrupo.SelectedValue);
                divParametros.Visible = false;
                lblTitle.Text = "Agregar Grupo ";
                switch (IdTipoGrupo)
                {
                    case (int)BusinessVariables.EnumTiposGrupos.Administrador:
                        lblTitle.Text += "Administrador";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.Usuario:
                        lblTitle.Text += "Usuario";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales:
                        lblTitle.Text += "Consultas Especiales";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.Agente:
                        lblTitle.Text += "Responsable de Atención";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                        lblTitle.Text += "Responsable de Contenido";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación:
                        lblTitle.Text += "Responsable de Operación";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                        lblTitle.Text += "Responsable de Desarrollo";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría:
                        lblTitle.Text += "Responsable de Servicio";
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal:
                        divParametros.Visible = true;
                        lblTitle.Text = "Contac Center";
                        break;
                    default:
                        return;
                }
                List<SubRol> lstRoles = _servicioSistemaSubRol.ObtenerSubRolesByTipoGrupo(IdTipoGrupo, false);
                divSubRoles.Visible = lstRoles.Count > 0;

                rptSubRoles.DataSource = lstRoles;
                rptSubRoles.DataBind();
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
                ValidaCapturaGrupoUsuario();
                GrupoUsuario grupoUsuario;
                if (Alta)
                {
                    grupoUsuario = new GrupoUsuario
                    {
                        IdTipoUsuario = IdTipoUsuario,
                        IdTipoGrupo = Convert.ToInt32(IdTipoGrupo),
                        Descripcion = txtDescripcionGrupoUsuario.Text,
                        Habilitado = chkHabilitado.Checked,
                        SubGrupoUsuario = new List<SubGrupoUsuario>()
                    };
                    if (IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal)
                    {
                        grupoUsuario.LevantaTicket = rbtnLevanta.Checked;
                        grupoUsuario.RecadoTicket = rbtnRecado.Checked;
                    }
                    else
                    {
                        grupoUsuario.LevantaTicket = false;
                        grupoUsuario.RecadoTicket = false;
                    }
                    Dictionary<int, int> horarios = new Dictionary<int, int>();
                    Dictionary<int, List<DiaFestivoSubGrupo>> diasDescanso = new Dictionary<int, List<DiaFestivoSubGrupo>>();
                    foreach (CheckBox chk in (from RepeaterItem item in rptSubRoles.Items select (CheckBox)item.FindControl("chkSubRol")).Where(chk => chk.Checked))
                    {
                        DropDownList ddlHorario = (DropDownList)chk.NamingContainer.FindControl("ddlHorario");
                        if (ddlHorario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Debe capturar horarios");

                        horarios.Add(int.Parse(chk.Attributes["value"]), int.Parse(ddlHorario.SelectedValue));

                        DropDownList ddlDiasFeriados = (DropDownList)chk.NamingContainer.FindControl("ddlDiasFeriados");
                        if (ddlDiasFeriados == null || ddlDiasFeriados.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            continue;
                        DiasFeriados descansoSeleccionado = _servicioDiaHorario.ObtenerDiasFeriadosUserById(int.Parse(ddlDiasFeriados.SelectedValue));
                        diasDescanso.Add(int.Parse(chk.Attributes["value"]), descansoSeleccionado.DiasFeriadosDetalle.Select(s => new DiaFestivoSubGrupo
                        {
                            IdSubGrupoUsuario = Convert.ToInt32(chk.Attributes["value"]),
                            Fecha = s.Dia,
                            Descripcion = s.Descripcion,
                            IdDiasFeriados = s.IdDiasFeriados
                        }).ToList());
                    }
                    _servicioGrupoUsuario.GuardarGrupoUsuario(grupoUsuario, horarios, diasDescanso);
                }
                else
                {
                    grupoUsuario = GrupoUsuario;
                    grupoUsuario.Descripcion = txtDescripcionGrupoUsuario.Text;
                    if (IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal)
                    {
                        grupoUsuario.LevantaTicket = rbtnLevanta.Checked;
                        grupoUsuario.RecadoTicket = rbtnRecado.Checked;
                    }
                    else
                    {
                        grupoUsuario.LevantaTicket = false;
                        grupoUsuario.RecadoTicket = false;
                    }
                    Dictionary<int, int> horarios = new Dictionary<int, int>();
                    Dictionary<int, List<DiaFestivoSubGrupo>> diasDescanso = new Dictionary<int, List<DiaFestivoSubGrupo>>();
                    foreach (CheckBox chk in (from RepeaterItem item in rptSubRoles.Items select (CheckBox)item.FindControl("chkSubRol")).Where(chk => chk.Checked))
                    {
                        DropDownList ddlHorario = (DropDownList)chk.NamingContainer.FindControl("ddlHorario");
                        if (ddlHorario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception("Debe capturar horarios");

                        horarios.Add(int.Parse(chk.Attributes["value"]), int.Parse(ddlHorario.SelectedValue));

                        DropDownList ddlDiasFeriados = (DropDownList)chk.NamingContainer.FindControl("ddlDiasFeriados");
                        if (ddlDiasFeriados == null || ddlDiasFeriados.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            continue;
                        DiasFeriados descansoSeleccionado = _servicioDiaHorario.ObtenerDiasFeriadosUserById(int.Parse(ddlDiasFeriados.SelectedValue));
                        diasDescanso.Add(int.Parse(chk.Attributes["value"]), descansoSeleccionado.DiasFeriadosDetalle.Select(s => new DiaFestivoSubGrupo
                        {
                            IdSubGrupoUsuario = Convert.ToInt32(chk.Attributes["value"]),
                            Fecha = s.Dia,
                            Descripcion = s.Descripcion,
                            IdDiasFeriados = s.IdDiasFeriados
                        }).ToList());
                    }
                    _servicioGrupoUsuario.ActualizarGrupo(grupoUsuario, horarios, diasDescanso);
                }
                LimpiarCampos();
                if (OnAceptarModal != null)
                    OnAceptarModal();
            }
            catch (Exception ex)
            {
                if (_lstError == null || _lstError.Count <= 0)
                {
                    _lstError = new List<string>();

                    _lstError.Add(ex.Message);
                }
                Alerta = _lstError;
            }
        }

        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarCampos();
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
                LimpiarCampos();
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

        protected void OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;
                if (chk != null)
                {

                    if (chk.DataItemContainer != null)
                    {
                        int valueSelected = Convert.ToInt32(chk.Attributes["value"]);

                        switch (valueSelected)
                        {
                            case (int)BusinessVariables.EnumSubRoles.TercerNivel:
                                if ((from RepeaterItem item in rptSubRoles.Items select (CheckBox)item.FindControl("chkSubRol")).Where(chkRepeater => Convert.ToInt32(chkRepeater.Attributes["value"]) == (int)BusinessVariables.EnumSubRoles.SegundoNivel).Any(chkRepeater => !chkRepeater.Checked))
                                {
                                    chk.Checked = false;
                                    throw new Exception("Require Segundo nivel.");
                                }
                                break;
                            case (int)BusinessVariables.EnumSubRoles.CuartoNivel:
                                if ((from RepeaterItem item in rptSubRoles.Items select (CheckBox)item.FindControl("chkSubRol")).Where(chkRepeater => Convert.ToInt32(chkRepeater.Attributes["value"]) == (int)BusinessVariables.EnumSubRoles.TercerNivel).Any(chkRepeater => !chkRepeater.Checked))
                                {
                                    chk.Checked = false;
                                    throw new Exception("Require Tercer nivel.");
                                }
                                break;
                        }
                        DropDownList ddlHorario = (DropDownList)chk.DataItemContainer.FindControl("ddlHorario");

                        ddlHorario.Enabled = chk.Checked;
                        ddlHorario.DataSource = _servicioDiaHorario.ObtenerHorarioDefault(true);
                        ddlHorario.DataTextField = "Descripcion";
                        ddlHorario.DataValueField = "Id";
                        ddlHorario.DataBind();
                        DropDownList ddlDiasFeriados = (DropDownList)chk.DataItemContainer.FindControl("ddlDiasFeriados");
                        ddlDiasFeriados.Enabled = chk.Checked;
                        ddlDiasFeriados.DataSource = _servicioDiaHorario.ObtenerDiasFeriadosUser(true);
                        ddlDiasFeriados.DataTextField = "Descripcion";
                        ddlDiasFeriados.DataValueField = "Id";
                        ddlDiasFeriados.DataBind();
                        Button btnHorarios = (Button)chk.DataItemContainer.FindControl("btnHorarios");
                        Button btnDiasDescanso = (Button)chk.DataItemContainer.FindControl("btnDiasDescanso");
                        if (btnHorarios != null && btnDiasDescanso != null)
                        {
                            if (chk.Checked)
                            {
                                chk.CssClass = "btn btn-primary active";
                                btnHorarios.CssClass = "col-sm-2 btn btn-sm btn-primary";
                                btnDiasDescanso.CssClass = "col-sm-2 btn btn-sm btn-primary";
                            }
                            else
                            {
                                Metodos.LimpiarCombo(ddlHorario);
                                Metodos.LimpiarCombo(ddlDiasFeriados);
                                chk.CssClass = "btn btn-primary";
                                btnHorarios.CssClass = "col-sm-2 btn btn-sm btn-primary disabled";
                                btnDiasDescanso.CssClass = "col-sm-2 btn btn-sm btn-primary disabled";
                            }
                        }
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

        protected void rptSubRoles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
                SubRol sbRol = (SubRol)e.Item.DataItem;
                switch (IdTipoGrupo)
                {
                    case (int)BusinessVariables.EnumTiposGrupos.Agente:
                        if (sbRol.Id == (int)BusinessVariables.EnumSubRoles.PrimererNivel)
                        {
                            ((CheckBox)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("chkSubRol")).Checked = true;
                            OnCheckedChanged(((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("chkSubRol"), null);
                            ((CheckBox)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("chkSubRol")).Enabled = false;
                            //((Button)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("btnHorarios")).CssClass = "col-sm-2 btn btn-sm btn-primary";
                            //((Button)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("btnDiasDescanso")).CssClass = "col-sm-2 btn btn-sm btn-primary";
                        }
                        break;
                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                        if (sbRol.Id == (int)BusinessVariables.EnumSubRoles.Autorizador)
                        {
                            ((CheckBox)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("chkSubRol")).Checked = true;
                            OnCheckedChanged(((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("chkSubRol"), null);
                            ((CheckBox)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("chkSubRol")).Enabled = false;
                            //((Button)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("btnHorarios")).CssClass = "col-sm-2 btn btn-sm btn-primary";
                            //((Button)((Repeater)sender).Controls[e.Item.ItemIndex].FindControl("btnDiasDescanso")).CssClass = "col-sm-2 btn-sm btn btn-primary";
                        }
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