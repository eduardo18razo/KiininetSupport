using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceOrganizacion;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;


namespace KiiniHelp.UserControls.Altas.Organizaciones
{
    public partial class UcAltaOrganizaciones : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private UsuariosMaster _mp;

        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceOrganizacionClient _servicioOrganizacion = new ServiceOrganizacionClient();
        readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();
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

        public string Title
        {
            set { lblTitleCatalogo.Text = value; }
        }
        public bool EsAlta
        {
            get { return bool.Parse(hfAlta.Value); }
            set
            {
                hfAlta.Value = value.ToString();
                if (value)
                {
                    //Nuevo nivel
                    ddlTipoUsuario.Enabled = true;
                    btnSeleccionarModal.Visible = true;
                    pnlAlta.Visible = true;
                    seleccionar.Visible = true;
                    btnGuardar.Visible = false;
                }
                else
                {
                    //Editar nombre nivel
                    ddlTipoUsuario.Enabled = false;
                    btnSeleccionarModal.Visible = false;                 
                    pnlAlta.Visible = false;
                    lblAccion.Text = "Editar";                  
                    btnGuardar.Visible = true;
                }
            }
        }
        public bool EsSeleccion
        {
            get { return bool.Parse(hfEsSeleccion.Value); }
            set
            {
                //Seleccionar nivel
                hfEsSeleccion.Value = value.ToString();
                btnSeleccionarModal.Visible = value;
                pnlAlta.Visible = value;                                 
                ddlTipoUsuario.Enabled = !value;
                lblAccion.Text = "Crear Nuevo";    
                //Modificación para editar
                seleccionar.Visible = false;
                btnGuardar.Visible = false;
            }
        }
        public void SetOrganizacionActualizar()
        {
            try
            {
                Organizacion org = _servicioOrganizacion.ObtenerOrganizacionById(IdOrganizacion);
                if (org == null) return;

                btnSeleccionarModal.CommandArgument = org.IdNivelOrganizacion.ToString();
                Session["OrganizacionSeleccionada"] = org;
                hfCatalogo.Value = org.IdNivelOrganizacion.ToString();
                ddlTipoUsuario.SelectedValue = org.IdTipoUsuario.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                ddlNivelSeleccionModal.Visible = EsAlta || EsSeleccion;
                switch (org.IdNivelOrganizacion)
                {
                    case 1:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        LlenaCombosModal();
                        ddlNivelSeleccionModal.SelectedValue = org.IdHolding.ToString();
                        txtDescripcionCatalogo.Text = org.Holding.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-seleccione btn-square";
                        lblStepNivel1.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel1.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel1.Text;

                        break;
                    case 2:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue),
                                int.Parse(hfNivel1.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdCompania.ToString();
                        txtDescripcionCatalogo.Text = org.Compania.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        divStep2.Visible = true;

                        lblStepNivel2.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        break;
                    case 3:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioOrganizacion.ObtenerDirecciones(
                                int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdDireccion.ToString();
                        txtDescripcionCatalogo.Text = org.Direccion.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        divStep3.Visible = true;

                        lblStepNivel3.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        break;
                    case 4:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioOrganizacion.ObtenerSubDirecciones(
                                int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdSubDireccion.ToString();
                        txtDescripcionCatalogo.Text = org.SubDireccion.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        divStep4.Visible = true;

                        lblStepNivel4.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        break;
                    case 5:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = org.IdGerencia.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue),
                                int.Parse(hfNivel4.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdGerencia.ToString();
                        txtDescripcionCatalogo.Text = org.Gerencia.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        divStep5.Visible = true;

                        lblStepNivel5.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        break;
                    case 6:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = org.IdGerencia.ToString();
                        hfNivel6.Value = org.IdSubGerencia.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioOrganizacion.ObtenerSubGerencias(
                                int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdSubGerencia.ToString();
                        txtDescripcionCatalogo.Text = org.SubGerencia.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";
                        lblStepNivel1.Text = org.Holding.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = org.Gerencia.Descripcion;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        break;
                    case 7:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = org.IdGerencia.ToString();
                        hfNivel6.Value = org.IdSubGerencia.ToString();
                        hfNivel7.Value = org.IdJefatura.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue),
                                int.Parse(hfNivel6.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdJefatura.ToString();
                        txtDescripcionCatalogo.Text = org.Jefatura.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
                        lblStepNivel1.Text = org.Holding.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = org.Gerencia.Descripcion;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = org.SubGerencia.Descripcion;
                        divStep7.Visible = true;
                        lblStepNivel7.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        break;
                }
                pnlAlta.Visible = true;
                btnGuardarCatalogo.Visible = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public void SetOrganizacionSeleccion()
        {
            try
            {
                Organizacion org = _servicioOrganizacion.ObtenerOrganizacionById(IdOrganizacion);
                if (org == null) return;
                IdTipoUsuario = org.IdTipoUsuario;
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                btnSeleccionarModal.CommandArgument = org.IdNivelOrganizacion.ToString();
                Session["OrganizacionSeleccionada"] = org;
                hfCatalogo.Value = org.IdNivelOrganizacion.ToString();
                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = false;
                divStep5.Visible = false;
                divStep6.Visible = false;
                divStep7.Visible = false;
                pnlAlta.Visible = EsAlta && EsSeleccion;
                switch (org.IdNivelOrganizacion)
                {
                    case 1:
                        divStep1.Visible = true;
                        divStep2.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));

                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;

                        break;
                    case 2:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        lblStepNivel3.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));

                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        break;
                    case 3:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        lblStepNivel4.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();


                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));

                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        break;
                    case 4:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        lblStepNivel5.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));

                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        break;
                    case 5:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;
                        divStep6.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        lblStepNivel5.Text = org.Gerencia.Descripcion;
                        lblStepNivel6.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = org.IdGerencia.ToString();
                        hfNivel6.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));

                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        break;
                    case 6:

                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;
                        divStep6.Visible = true;
                        divStep7.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        lblStepNivel5.Text = org.Gerencia.Descripcion;
                        lblStepNivel7.Text = org.SubGerencia.Descripcion;
                        lblStepNivel6.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = org.IdGerencia.ToString();
                        hfNivel6.Value = org.IdSubGerencia.ToString();
                        hfNivel7.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));

                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        break;
                    case 7:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;
                        divStep6.Visible = true;
                        divStep7.Visible = true;

                        lblStepNivel1.Text = org.Holding.Descripcion;
                        lblStepNivel2.Text = org.Compania.Descripcion;
                        lblStepNivel3.Text = org.Direccion.Descripcion;
                        lblStepNivel4.Text = org.SubDireccion.Descripcion;
                        lblStepNivel5.Text = org.Gerencia.Descripcion;
                        lblStepNivel6.Text = org.SubGerencia.Descripcion;
                        lblStepNivel7.Text = "...";

                        hfNivel1.Value = org.IdHolding.ToString();
                        hfNivel2.Value = org.IdCompania.ToString();
                        hfNivel3.Value = org.IdDireccion.ToString();
                        hfNivel4.Value = org.IdSubDireccion.ToString();
                        hfNivel5.Value = org.IdGerencia.ToString();
                        hfNivel6.Value = org.IdSubGerencia.ToString();
                        hfNivel7.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public int IdOrganizacion
        {
            get { return int.Parse(hfIdOrganizacion.Value); }
            set
            {
                hfIdOrganizacion.Value = value.ToString();
            }
        }
        public Organizacion OrganizacionSeleccionada
        {
            get
            {
                return hfOrganizacionSeleccionada.Value == string.Empty ? new Organizacion() : _servicioOrganizacion.ObtenerOrganizacionById(int.Parse(hfOrganizacionSeleccionada.Value));
            }
            set
            {
                if (value.Id == 0)
                {
                    hfOrganizacionSeleccionada.Value = string.Empty;
                }
                else
                    hfOrganizacionSeleccionada.Value = value.Id.ToString();
                IdOrganizacion = value.Id;
                EsAlta = false;
                EsSeleccion = true;
            }
        }
        public int IdTipoUsuario
        {
            get { return int.Parse(ddlTipoUsuario.SelectedValue); }
            set
            {
                if (ddlTipoUsuario.Items.Count <= 0)
                    LlenaCombosModal();
                ddlTipoUsuario.SelectedValue = value.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                ddlTipoUsuario.Enabled = false;
            }
        }

        private void SetAliasModal()
        {
            try
            {
                lblAliasNivel1.Text = BusinessVariables.AliasOrganizaciones.Nivel1;
                lblAliasNivel2.Text = BusinessVariables.AliasOrganizaciones.Nivel2;
                lblAliasNivel3.Text = BusinessVariables.AliasOrganizaciones.Nivel3;
                lblAliasNivel4.Text = BusinessVariables.AliasOrganizaciones.Nivel4;
                lblAliasNivel5.Text = BusinessVariables.AliasOrganizaciones.Nivel5;
                lblAliasNivel6.Text = BusinessVariables.AliasOrganizaciones.Nivel6;
                lblAliasNivel7.Text = BusinessVariables.AliasOrganizaciones.Nivel7;
                if (ddlTipoUsuario.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione) return;
                List<AliasOrganizacion> alias = _servicioParametros.ObtenerAliasOrganizacion(int.Parse(ddlTipoUsuario.SelectedValue));
                if (alias.Count != 7)
                {
                    return;
                }
                lblAliasNivel1.Text = alias.Single(s => s.Nivel == 1).Descripcion;
                lblAliasNivel2.Text = alias.Single(s => s.Nivel == 2).Descripcion;
                lblAliasNivel3.Text = alias.Single(s => s.Nivel == 3).Descripcion;
                lblAliasNivel4.Text = alias.Single(s => s.Nivel == 4).Descripcion;
                lblAliasNivel5.Text = alias.Single(s => s.Nivel == 5).Descripcion;
                lblAliasNivel6.Text = alias.Single(s => s.Nivel == 6).Descripcion;
                lblAliasNivel7.Text = alias.Single(s => s.Nivel == 7).Descripcion;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LlenaCombosModal()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true);
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LimpiaCatalogo()
        {
            try
            {
                ddlTipoUsuario.Enabled = true;
                ddlTipoUsuario.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                ddlNivelSeleccionModal.Visible = true;
                ddlNivelSeleccionModal.Enabled = true;
                hfNivel1.Value = "0";
                hfNivel2.Value = "0";
                hfNivel3.Value = "0";
                hfNivel4.Value = "0";
                hfNivel5.Value = "0";
                hfNivel6.Value = "0";
                hfNivel7.Value = "0";
                divData.Visible = false;
                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = false;
                divStep5.Visible = false;
                divStep6.Visible = false;
                divStep7.Visible = false;
                lblStepNivel1.Text = "...";
                lblStepNivel2.Text = "...";
                lblStepNivel3.Text = "...";
                lblStepNivel4.Text = "...";
                lblStepNivel5.Text = "...";
                lblStepNivel6.Text = "...";
                lblStepNivel7.Text = "...";
                btnStatusNivel1.CssClass = "btn btn-seleccione btn-square";
                btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";
                btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";
                btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";
                btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";
                btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";
                btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
                btnSeleccionarModal.Visible = true;
                pnlAlta.Visible = true;
                btnGuardarCatalogo.Visible = true;
                txtDescripcionCatalogo.Text = string.Empty;
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
        private void LlenaComboDinamico(DropDownList ddl, object source)
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddl, source);
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
                _mp = (UsuariosMaster)Page.Master;
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaCombosModal();
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

                Metodos.LimpiarCombo(ddlNivelSeleccionModal);
                SetAliasModal();
                btnStatusNivel1.CssClass = "btn btn-seleccione btn-square";
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = false;
                divStep5.Visible = false;
                divStep6.Visible = false;
                divStep7.Visible = false;
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    divData.Visible = false;
                    return;
                }
                divData.Visible = true;
                divStep1.Visible = true;
                int level = hfCatalogo.Value == string.Empty ? 0 : int.Parse(hfCatalogo.Value);
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos && (int.Parse(ddlTipoUsuario.SelectedValue) != (int)BusinessVariables.EnumTiposUsuario.Operador || level > 1))
                {
                    pnlAlta.Visible = true;
                    LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerHoldings(int.Parse(ddlTipoUsuario.SelectedValue), true));
                }
                else
                {
                    pnlAlta.Visible = false;
                    LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerHoldings(int.Parse(ddlTipoUsuario.SelectedValue), true));
                }
                if (EsAlta || EsSeleccion)
                {
                    lblOperacion.Text = lblAliasNivel1.Text;
                    lblOperacionDescripcion.Text = lblAliasNivel1.Text;
                    btnSeleccionarModal.CommandArgument = "1";
                    lblStepNivel1.Text = "...";
                    hfCatalogo.Value = "1";
                }
                else
                {
                    pnlAlta.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void btnSeleccionarModal_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (ddlNivelSeleccionModal.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Debe seleccionar Nivel");

                switch (int.Parse(btnSeleccionarModal.CommandArgument))
                {
                    case 1:
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        lblStepNivel1.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel1.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        succNivel1.Visible = true;
                        btnSeleccionarModal.CommandArgument = "2";
                        hfCatalogo.Value = "2";
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = "...";
                        btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";
                        pnlAlta.Visible = true;
                        break;
                    case 2:
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        lblStepNivel2.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel2.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                        succNivel2.Visible = true;
                        btnSeleccionarModal.CommandArgument = "3";
                        hfCatalogo.Value = "3";
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = "...";
                        btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 3:
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        lblStepNivel3.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel3.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                        succNivel3.Visible = true;
                        btnSeleccionarModal.CommandArgument = "4";
                        hfCatalogo.Value = "4";
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = "...";
                        btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 4:
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        lblStepNivel4.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel4.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                        succNivel4.Visible = true;
                        btnSeleccionarModal.CommandArgument = "5";
                        hfCatalogo.Value = "5";
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = "...";
                        btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 5:
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        lblStepNivel5.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel5.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                        succNivel5.Visible = true;
                        btnSeleccionarModal.CommandArgument = "6";
                        hfCatalogo.Value = "6";
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = "...";
                        btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 6:
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        lblStepNivel6.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel6.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
                        succNivel6.Visible = true;
                        btnSeleccionarModal.CommandArgument = "7";
                        hfCatalogo.Value = "7";
                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        divStep7.Visible = true;
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 7:
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";
                        lblStepNivel7.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel7.Value = ddlNivelSeleccionModal.SelectedValue;
                        succNivel7.Visible = true;
                        break;

                }
            }
            catch (Exception ex)
            {
                switch (int.Parse(btnSeleccionarModal.CommandArgument))
                {
                    case 1:
                        lblStepNivel1.Text = "...";
                        hfNivel1.Value = ddlNivelSeleccionModal.SelectedValue;
                        succNivel1.Visible = false;
                        break;
                    case 2:
                        btnSeleccionarModal.CommandArgument = "2";
                        break;
                    case 3:
                        btnSeleccionarModal.CommandArgument = "3";
                        break;
                    case 4:
                        btnSeleccionarModal.CommandArgument = "4";
                        break;
                    case 5:
                        btnSeleccionarModal.CommandArgument = "5";
                        break;
                    case 6:
                        btnSeleccionarModal.CommandArgument = "6";
                        break;
                    case 7:
                        btnSeleccionarModal.CommandArgument = "7";
                        break;

                }
                if (_lstError == null || _lstError.Count <= 0)
                {
                    _lstError = new List<string>();
                    _lstError.Add(ex.Message);
                }
                Alerta = _lstError;
            }
        }
        protected void ddlNivelSeleccionModal_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch (int.Parse(btnSeleccionarModal.CommandArgument))
                {
                    case 1:
                        btnSeleccionarModal.CommandArgument = "1";
                        break;
                    case 2:
                        btnSeleccionarModal.CommandArgument = "2";
                        break;
                    case 3:
                        btnSeleccionarModal.CommandArgument = "3";
                        break;
                    case 4:
                        btnSeleccionarModal.CommandArgument = "4";
                        break;
                    case 5:
                        btnSeleccionarModal.CommandArgument = "5";
                        break;
                    case 6:
                        btnSeleccionarModal.CommandArgument = "6";
                        break;
                    case 7:
                        btnSeleccionarModal.CommandArgument = "7";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void Guardar()
        {
            try
            {
                if (!Metodos.ValidaCapturaCatalogo(txtDescripcionCatalogo.Text)) return;
                Organizacion organizacion;
                if (EsAlta)
                {
                    organizacion = new Organizacion
                    {
                        IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue)
                    };
                    switch (int.Parse(hfCatalogo.Value))
                    {
                        case 1:
                            organizacion.Holding = new Holding
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            if (OnAceptarModal != null)
                                OnAceptarModal();
                            ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                            break;
                        case 2:
                            organizacion.IdHolding = int.Parse(hfNivel1.Value);
                            organizacion.Compania = new Compania
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                            break;
                        case 3:
                            organizacion.IdHolding = int.Parse(hfNivel1.Value);
                            organizacion.IdCompania = int.Parse(hfNivel2.Value);
                            organizacion.Direccion = new Direccion
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                            break;
                        case 4:
                            organizacion.IdHolding = int.Parse(hfNivel1.Value);
                            organizacion.IdCompania = int.Parse(hfNivel2.Value);
                            organizacion.IdDireccion = int.Parse(hfNivel3.Value);
                            organizacion.SubDireccion = new SubDireccion
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                            break;
                        case 5:
                            organizacion.IdHolding = int.Parse(hfNivel1.Value);
                            organizacion.IdCompania = int.Parse(hfNivel2.Value);
                            organizacion.IdDireccion = int.Parse(hfNivel3.Value);
                            organizacion.IdSubDireccion = int.Parse(hfNivel4.Value);
                            organizacion.Gerencia = new Gerencia
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                            break;
                        case 6:
                            organizacion.IdHolding = int.Parse(hfNivel1.Value);
                            organizacion.IdCompania = int.Parse(hfNivel2.Value);
                            organizacion.IdDireccion = int.Parse(hfNivel3.Value);
                            organizacion.IdSubDireccion = int.Parse(hfNivel4.Value);
                            organizacion.IdGerencia = int.Parse(hfNivel5.Value);
                            organizacion.SubGerencia = new SubGerencia
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                            break;
                        case 7:
                            organizacion.IdHolding = int.Parse(hfNivel1.Value);
                            organizacion.IdCompania = int.Parse(hfNivel2.Value);
                            organizacion.IdDireccion = int.Parse(hfNivel3.Value);
                            organizacion.IdSubDireccion = int.Parse(hfNivel4.Value);
                            organizacion.IdGerencia = int.Parse(hfNivel5.Value);
                            organizacion.IdSubGerencia = int.Parse(hfNivel6.Value);
                            organizacion.Jefatura = new Jefatura
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioOrganizacion.GuardarOrganizacion(organizacion);
                            if (OnAceptarModal != null)
                                OnAceptarModal();
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));

                            break;
                    }
                    _mp.AlertaSucces();
                    ddlNivelSeleccionModal.SelectedIndex = ddlNivelSeleccionModal.Items.IndexOf(ddlNivelSeleccionModal.Items.FindByText(txtDescripcionCatalogo.Text.Trim()));
                }
                else
                {
                    organizacion = (Organizacion)Session["OrganizacionSeleccionada"];
                    switch (int.Parse(hfCatalogo.Value))
                    {
                        case 1:
                            organizacion.Holding.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaCombosModal();
                            break;
                        case 2:
                            organizacion.Compania.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                            break;
                        case 3:
                            organizacion.Direccion.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                            break;
                        case 4:
                            organizacion.SubDireccion.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                            break;
                        case 5:
                            organizacion.Gerencia.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                            break;
                        case 6:
                            organizacion.SubGerencia.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                            break;
                        case 7:
                            organizacion.Jefatura.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioOrganizacion.ActualizarOrganizacion(organizacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
                            break;
                    }

                    _mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Actualizacion));
                }

                txtDescripcionCatalogo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void btnGuardarCatalogo_OnClick(object sender, EventArgs e)
        {
            try
            {
                Guardar();
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
        protected void btnCancelarCatalogo_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiaCatalogo();
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
        /*Modificado Estilos pasos*/
        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (EsSeleccion)
                {
                    switch (int.Parse(btnSeleccionarModal.CommandArgument))
                    {
                        case 1:
                            throw new Exception("Seleccione una ubicacion de nivel 2");
                            if (btnStatusNivel1.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), null, null, null, null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 1);
                            }
                            break;
                        case 2:
                            if (btnStatusNivel2.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), null, null, null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 2);
                            }
                            else
                            {
                                throw new Exception("Selecciones una ubicacion de nivel 2");
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), null, null, null, null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 1);
                            }
                            break;
                        case 3:
                            if (btnStatusNivel3.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), null, null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 3);
                            }
                            else
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), null, null, null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 2);
                            }
                            break;
                        case 4:
                            if (btnStatusNivel4.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 4);
                            }
                            else
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), null, null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 3);
                            }
                            break;
                        case 5:
                            if (btnStatusNivel5.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 5);
                            }
                            else
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), null, null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 4);
                            }
                            break;
                        case 6:
                            if (btnStatusNivel6.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), int.Parse(hfNivel6.Value), null).SingleOrDefault(s => s.IdNivelOrganizacion == 6);
                            }
                            else
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), null, null).SingleOrDefault(s => s.IdNivelOrganizacion == 5);
                            }
                            break;
                        case 7:
                            if (btnStatusNivel7.CssClass == "btn btn-primary btn-square")
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), int.Parse(hfNivel6.Value), int.Parse(hfNivel7.Value)).SingleOrDefault(s => s.IdNivelOrganizacion == 7);
                            }
                            else
                            {
                                OrganizacionSeleccionada = _servicioOrganizacion.ObtenerOrganizaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), int.Parse(hfNivel6.Value), null).SingleOrDefault(s => s.IdNivelOrganizacion == 6);
                            }
                            break;
                    }
                }
                else if (!EsAlta && !EsSeleccion)
                {
                    Guardar();
                }
                LimpiaCatalogo();
                if (OnTerminarModal != null)
                    OnTerminarModal();
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

        /*Modificado Estilos pasos*/
        protected void btnStatusNivel1_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;

                if (!EsAlta && !EsSeleccion)
                {
                    throw new Exception("No puede cambiar de nivel");
                }

                switch (int.Parse(lnkButton.CommandArgument))
                {
                    case 1:
                        ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                        divStep2.Visible = false;
                        divStep3.Visible = false;
                        divStep4.Visible = false;
                        divStep5.Visible = false;
                        divStep6.Visible = false;
                        divStep7.Visible = false;
                        break;
                    case 2:
                        divStep3.Visible = false;
                        divStep4.Visible = false;
                        divStep5.Visible = false;
                        divStep6.Visible = false;
                        divStep7.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        succNivel1.Visible = true;
                        btnSeleccionarModal.CommandArgument = "2";
                        hfCatalogo.Value = "2";
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = "...";
                        btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 3:
                        divStep4.Visible = false;
                        divStep5.Visible = false;
                        divStep6.Visible = false;
                        divStep7.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                        succNivel2.Visible = true;
                        btnSeleccionarModal.CommandArgument = "3";
                        hfCatalogo.Value = "3";
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = "...";
                        btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";

                        break;
                    case 4:
                        divStep5.Visible = false;
                        divStep6.Visible = false;
                        divStep7.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                        succNivel3.Visible = true;
                        btnSeleccionarModal.CommandArgument = "4";
                        hfCatalogo.Value = "4";
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = "...";

                        btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 5:
                        divStep6.Visible = false;
                        divStep7.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                        succNivel4.Visible = true;
                        btnSeleccionarModal.CommandArgument = "5";
                        hfCatalogo.Value = "5";
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = "...";
                        btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";
                        break;
                    case 6:
                        divStep7.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                        succNivel5.Visible = true;
                        btnSeleccionarModal.CommandArgument = "6";
                        hfCatalogo.Value = "6";
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = "...";
                        btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";

                        break;
                    case 7:
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";
                        succNivel7.Visible = true;
                        lblStepNivel7.Text = "...";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
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

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Guardar();
                LimpiaCatalogo();
                if (OnTerminarModal != null)
                    OnTerminarModal();
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