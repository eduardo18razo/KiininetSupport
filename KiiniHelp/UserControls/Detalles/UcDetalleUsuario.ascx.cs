using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceSistemaTipoTelefono;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleUsuario : UserControl, IControllerModal
    {

        private readonly ServiceTipoTelefonoClient _servicioTipoTelefono = new ServiceTipoTelefonoClient();

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private List<string> _lstError = new List<string>();

        public bool FromModal
        {
            get { return btnCerrarModal.Visible; }
            set
            {
                btnCerrarModal.Visible = value;

            }
        }
        public int IdUsuario
        {
            get { return Convert.ToInt32(ViewState["IdUsuario"].ToString()); }
            set
            {
                Usuario userDetail = new ServiceUsuariosClient().ObtenerDetalleUsuario(value);
                if(userDetail != null)
                {
                    ViewState["IdUsuario"] = value;
                    IdTipoUsuario = userDetail.IdTipoUsuario;
                    lblnombreCompleto.Text = userDetail.NombreCompleto;
                    lblFechaUltimoAcceso.Text = userDetail.FechaUltimoAccesoExito;
                    //txtAp.Text = userDetail.ApellidoPaterno;
                    //txtAm.Text = userDetail.ApellidoMaterno;
                    //txtNombre.Text = userDetail.Nombre;
                    //txtUserName.Text = userDetail.NombreUsuario;
                    imgPerfil.ImageUrl = userDetail.Foto != null ? "~/DisplayImages.ashx?id=" + userDetail.Id : "~/assets/images/profiles/profile-square-1.png";
                    ddlPuesto.SelectedValue = userDetail.IdPuesto.ToString();
                    chkVip.Checked = userDetail.Vip;
                    chkDirectoriActivo.Checked = userDetail.DirectorioActivo;
                    rptCorreos.DataSource = userDetail.CorreoUsuario;
                    rptCorreos.DataBind();
                    rptTelefonos.DataSource = userDetail.TelefonoUsuario;
                    rptTelefonos.DataBind();
                    UcDetalleOrganizacion.IdOrganizacion = userDetail.IdOrganizacion;
                    UcDetalleUbicacion.IdUbicacion = userDetail.IdUbicacion;
                    UcDetalleGrupoUsuario.IdUsuario = userDetail.Id;
                }
            }
        }

        private int IdTipoUsuario
        {
            get { return Convert.ToInt32(ViewState["IdTipoUsuario"].ToString()); }
            set { ViewState["IdTipoUsuario"] = value; }
        }
        private List<string> AlertaGeneral
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _lstError = new List<string>();
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
                AlertaGeneral = _lstError;
            }
        }
        protected void rptTelefonos_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                DropDownList ddlTipoTelefono = (DropDownList)e.Item.FindControl("ddlTipoTelefono");
                if (ddlTipoTelefono != null)
                {
                    ddlTipoTelefono.DataSource = _servicioTipoTelefono.ObtenerTiposTelefono(true);
                    ddlTipoTelefono.DataTextField = "Descripcion";
                    ddlTipoTelefono.DataValueField = "Id";
                    ddlTipoTelefono.DataBind();
                    if (((TelefonoUsuario)e.Item.DataItem).IdTipoTelefono == 0)
                    {
                        e.Item.FindControl("divExtension").Visible = false;
                        ddlTipoTelefono.Enabled = true;
                    }
                    else
                    {
                        ddlTipoTelefono.SelectedValue = ((TelefonoUsuario)e.Item.DataItem).IdTipoTelefono.ToString();
                        e.Item.FindControl("divExtension").Visible = ((TelefonoUsuario)e.Item.DataItem).IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Oficina;
                        ddlTipoTelefono.Enabled = false;
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
                AlertaGeneral = _lstError;
            }
        }
        protected void btnCerrarModal_OnClick(object sender, EventArgs e)
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
                AlertaGeneral = _lstError;
            }
        }
    }
}