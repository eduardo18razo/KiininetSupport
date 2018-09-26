﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArea;
using KiiniHelp.ServiceFrecuencia;
using KiiniHelp.ServiceSeguridad;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using Menu = KiiniNet.Entities.Cat.Sistema.Menu;

namespace KiiniHelp.UserControls.Seleccion
{
    public partial class UcReportSelect : UserControl, IControllerModal
    {

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _servicioTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceFrecuenciaClient _servicioFrecuencia = new ServiceFrecuenciaClient();
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private readonly ServiceSecurityClient _servicioSeguridad = new ServiceSecurityClient();

        private void GetData(int idTipoUsuario)
        {
            try
            {
                //List<KiiniNet.Entities.Cat.Sistema.Menu> menuActivo = _servicioSeguridad.ObtenerMenuUsuario(((Usuario)Session["UserData"]).Id,int.Parse(Session["RolSeleccionado"].ToString()), true)
                //        .Where(w => w.Id == (int)BusinessVariables.EnumMenu.View).ToList();
                //List<List<Menu>> menuAnaliticos = menuActivo.Where(w=>w.Id == (int)BusinessVariables.EnumMenu.View).Select(s=>s.Menu1);
                //rpt10Frecuentes.DataSource = menuAnaliticos;
                //rpt10Frecuentes.DataBind();

                //int idArea = Request.Params["idArea"] != null ? int.Parse(Request.Params["idArea"]) : 0;
                //divCategoriaTitle.Visible = idArea > 0;
                //if (divCategoriaTitle.Visible)
                //{
                //    divCategoriaReference.Visible = false;
                //    Area area = _servicioArea.ObtenerAreaById(idArea);
                //    lblCategoria.Text = area.Descripcion;

                //    rpt10Frecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenGeneral(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null).Where(w => w.IdArea == area.Id);
                //    rpt10Frecuentes.DataBind();

                //    rptConsultasFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenConsulta(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null).Where(w => w.IdArea == area.Id);
                //    rptConsultasFrecuentes.DataBind();

                //    rptServiciosFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenServicio(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null).Where(w => w.IdArea == area.Id);
                //    rptServiciosFrecuentes.DataBind();

                //    rptProblemasFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenIncidente(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null).Where(w => w.IdArea == area.Id);
                //    rptProblemasFrecuentes.DataBind();
                //}
                //else
                //{
                //    divCategoriaReference.Visible = true;

                //    rptConsultasFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenConsulta(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null);
                //    rptConsultasFrecuentes.DataBind();

                //    rptServiciosFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenServicio(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null);
                //    rptServiciosFrecuentes.DataBind();

                //    rptProblemasFrecuentes.DataSource = _servicioFrecuencia.ObtenerTopTenIncidente(idTipoUsuario, Session["UserData"] != null ? (int?)((Usuario)Session["UserData"]).Id : null);
                //    rptProblemasFrecuentes.DataBind();
                //}
                //lbtnCategoria.Enabled = rpt10Frecuentes.Items.Count > 0
                //        || rptConsultasFrecuentes.Items.Count > 0
                //        || rptServiciosFrecuentes.Items.Count > 0
                //        || rptProblemasFrecuentes.Items.Count > 0;

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
                    int idtipoUsuario;
                    if (Request.Params["userType"] != null)
                    {
                        idtipoUsuario = int.Parse(Request.Params["userType"]);
                    }
                    else
                    {
                        idtipoUsuario = ((Usuario)Session["UserData"]).IdTipoUsuario;
                    }
                    TipoUsuario tipoUsuario = _servicioTipoUsuario.ObtenerTipoUsuarioById(idtipoUsuario);
                    if (tipoUsuario != null)
                    {
                        GetData(tipoUsuario.Id);
                        lbltipoUsuario.Text = tipoUsuario.Descripcion;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void lbtnCategoria_OnClick(object sender, EventArgs e)
        {
            if (Session["UserData"] == null)
                Response.Redirect("~/Publico/FrmCategoria.aspx?userType=" + int.Parse(Request.Params["userType"]));
            else
                Response.Redirect("~/Users/FrmCategorias.aspx?userType=" + ((Usuario)Session["UserData"]).IdTipoUsuario);
        }

        protected void verOpcion_Click(object sender, EventArgs e)
        {
            int tipoArbol = Convert.ToInt32(((LinkButton)sender).CommandName);
            if (Session["UserData"] == null)
                switch (tipoArbol)
                {
                    case (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion:
                        Response.Redirect("~/Publico/FrmConsulta.aspx?IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.SolicitarServicio:
                        Response.Redirect("~/Publico/FrmTicketPublico.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.ReportarProblemas:
                        Response.Redirect("~/Publico/FrmTicketPublico.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;
                }
            else
                switch (tipoArbol)
                {
                    case (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion:
                        Response.Redirect("~/Users/General/FrmNodoConsultas.aspx?IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.SolicitarServicio:
                        Response.Redirect("~/Users/Ticket/FrmTicket.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;

                    case (int)BusinessVariables.EnumTipoArbol.ReportarProblemas:
                        Response.Redirect("~/Users/Ticket/FrmTicket.aspx?Canal=" + (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal + "&IdArbol=" + Convert.ToInt32(((LinkButton)sender).CommandArgument));
                        break;
                }

        }

        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserData"] == null)
                    Response.Redirect("~/Publico/FrmUserSelect.aspx?userType=" + Request.Params["userType"]);
                else
                    Response.Redirect("~/Users/FrmDashboardUser.aspx");
            }
            catch
            {
            }
        }
    }
}