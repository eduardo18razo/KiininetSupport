using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSistemaDomicilio;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Ubicaciones
{
    public partial class UcAltaUbicaciones : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private UsuariosMaster _mp;

        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
        private readonly ServiceDomicilioSistemaClient _servicioSistemaDomicilio = new ServiceDomicilioSistemaClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();

        private List<string> _lstError = new List<string>();

        private string AlertaSucces
        {
            set
            {
                if (value.Trim() != string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "SuccsessAlert('Éxito: ','" + value + "');", true);
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
                    ddlTipoUsuario.Enabled = true;
                    btnSeleccionarModal.Visible = true;
                    btnGuardarCatalogo.Visible = true;

                    seleccionar.Visible = true;
                    btnGuardar.Visible = false;
                }
                else
                {
                    ddlTipoUsuario.Enabled = false;
                    btnSeleccionarModal.Visible = false;
                    btnGuardarCatalogo.Visible = false;

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
                hfEsSeleccion.Value = value.ToString();
                btnSeleccionarModal.Visible = value;
                btnGuardarCatalogo.Visible = value;
                ddlTipoUsuario.Enabled = !value;

                divNombre.Visible = true;
                seleccionar.Visible = false;
                btnGuardar.Visible = false;

            }
        }

        public void SetUbicacionActualizar()
        {
            try
            {
                Ubicacion org = _servicioUbicacion.ObtenerUbicacionById(IdUbicacion);
                if (org == null) return;

                btnSeleccionarModal.CommandArgument = org.IdNivelUbicacion.ToString();
                Session["UbicacionSeleccionada"] = org;
                hfCatalogo.Value = org.IdNivelUbicacion.ToString();
                ddlTipoUsuario.SelectedValue = org.IdTipoUsuario.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                ddlNivelSeleccionModal.Visible = EsAlta || EsSeleccion;
                switch (org.IdNivelUbicacion)
                {
                    case 1:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        LlenaCombosModal();
                        ddlNivelSeleccionModal.SelectedValue = org.IdPais.ToString();
                        txtDescripcionCatalogo.Text = org.Pais.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        lblStepNivel1.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel1.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel1.Text;

                        break;
                    case 2:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerCampus(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdCampus.ToString();
                        txtDescripcionCatalogo.Text = org.Campus.Descripcion;
                        if (org.Campus.Domicilio != null && org.Campus.Domicilio.Count > 0)
                        {
                            txtCp.Text = org.Campus.Domicilio.First().Colonia.CP.ToString();
                            txtCp_OnTextChanged(txtCp, null);
                            ddlColonia.SelectedValue = org.Campus.Domicilio.First().IdColonia.ToString();
                            ddlColonia_OnSelectedIndexChanged(ddlColonia, null);
                            txtCalle.Text = org.Campus.Domicilio.First().Calle;
                            txtNoExt.Text = org.Campus.Domicilio.First().NoExt;
                            txtNoInt.Text = org.Campus.Domicilio.First().NoInt;
                        }
                        SetAliasModal();
                        dataCampus.Visible = true;
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        divStep2.Visible = true;

                        lblStepNivel2.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        break;
                    case 3:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioUbicacion.ObtenerTorres(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdTorre.ToString();
                        txtDescripcionCatalogo.Text = org.Torre.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        divStep3.Visible = true;

                        lblStepNivel3.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        break;
                    case 4:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioUbicacion.ObtenerPisos(
                                int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdPiso.ToString();
                        txtDescripcionCatalogo.Text = org.Piso.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        divStep4.Visible = true;

                        lblStepNivel4.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        break;
                    case 5:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = org.IdZona.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,
                            _servicioUbicacion.ObtenerZonas(int.Parse(ddlTipoUsuario.SelectedValue),
                                int.Parse(hfNivel4.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdZona.ToString();
                        txtDescripcionCatalogo.Text = org.Zona.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        divStep5.Visible = true;

                        lblStepNivel5.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        break;
                    case 6:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = org.IdZona.ToString();
                        hfNivel6.Value = org.IdSubZona.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSubZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdSubZona.ToString();
                        txtDescripcionCatalogo.Text = org.SubZona.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-success btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        lblStepNivel1.Text = org.Pais.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = org.Zona.Descripcion;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        break;
                    case 7:
                        ddlNivelSeleccionModal.Enabled = !EsAlta || !EsSeleccion;
                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = org.IdZona.ToString();
                        hfNivel6.Value = org.IdSubZona.ToString();
                        hfNivel7.Value = org.IdSiteRack.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSiteRacks(int.Parse(ddlTipoUsuario.SelectedValue),
                                int.Parse(hfNivel6.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = org.IdSiteRack.ToString();
                        txtDescripcionCatalogo.Text = org.SiteRack.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-success btn-square";
                        btnStatusNivel6.CssClass = "btn btn-success btn-square";
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";
                        lblStepNivel1.Text = org.Pais.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = org.Zona.Descripcion;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = org.SubZona.Descripcion;
                        divStep7.Visible = true;
                        lblStepNivel7.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        break;
                }
                divCapturaDescripcion.Visible = true;
                divNombre.Visible = true;
                btnGuardarCatalogo.Visible = false;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void SetUbicacionSeleccion()
        {
            try
            {
                Ubicacion org = _servicioUbicacion.ObtenerUbicacionById(IdUbicacion);
                if (org == null) return;
                IdTipoUsuario = org.IdTipoUsuario;
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                btnSeleccionarModal.CommandArgument = org.IdNivelUbicacion.ToString();
                Session["UbicacionSeleccionada"] = org;
                hfCatalogo.Value = org.IdNivelUbicacion.ToString();
                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = false;
                divStep5.Visible = false;
                divStep6.Visible = false;
                divStep7.Visible = false;
                switch (org.IdNivelUbicacion)
                {
                    case 1:
                        divStep1.Visible = true;
                        divStep2.Visible = true;

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerCampus(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));

                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;

                        break;
                    case 2:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        lblStepNivel3.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerTorres(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));

                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        break;
                    case 3:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        lblStepNivel4.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();


                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPisos(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));

                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        break;
                    case 4:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        lblStepNivel5.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));

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

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        lblStepNivel5.Text = org.Zona.Descripcion;
                        lblStepNivel6.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = org.IdZona.ToString();
                        hfNivel6.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-success btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSubZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));

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

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        lblStepNivel5.Text = org.Zona.Descripcion;
                        lblStepNivel7.Text = org.SubZona.Descripcion;
                        lblStepNivel6.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = org.IdZona.ToString();
                        hfNivel6.Value = org.IdSubZona.ToString();
                        hfNivel7.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-success btn-square";
                        btnStatusNivel6.CssClass = "btn btn-success btn-square";
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSiteRacks(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));

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

                        lblStepNivel1.Text = org.Pais.Descripcion;
                        lblStepNivel2.Text = org.Campus.Descripcion;
                        lblStepNivel3.Text = org.Torre.Descripcion;
                        lblStepNivel4.Text = org.Piso.Descripcion;
                        lblStepNivel5.Text = org.Zona.Descripcion;
                        lblStepNivel6.Text = org.SubZona.Descripcion;
                        lblStepNivel7.Text = "...";

                        hfNivel1.Value = org.IdPais.ToString();
                        hfNivel2.Value = org.IdCampus.ToString();
                        hfNivel3.Value = org.IdTorre.ToString();
                        hfNivel4.Value = org.IdPiso.ToString();
                        hfNivel5.Value = org.IdZona.ToString();
                        hfNivel6.Value = org.IdSubZona.ToString();
                        hfNivel7.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        btnStatusNivel5.CssClass = "btn btn-success btn-square";
                        btnStatusNivel6.CssClass = "btn btn-success btn-square";
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSiteRacks(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
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

        public int IdUbicacion
        {
            get { return int.Parse(hfIdUbicacion.Value); }
            set
            {
                hfIdUbicacion.Value = value.ToString();
            }
        }
        public Ubicacion UbicacionSeleccionada
        {
            get { return hfUbicacionSeleccionada.Value.Trim() == string.Empty ? new Ubicacion() : _servicioUbicacion.ObtenerUbicacionById(int.Parse(hfUbicacionSeleccionada.Value)); }
            set
            {
                if (value.Id == 0)
                {
                    hfUbicacionSeleccionada.Value = string.Empty;
                }
                else
                    hfUbicacionSeleccionada.Value = value.Id.ToString();
                IdUbicacion = value.Id;
                EsAlta = true;
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
                lblAliasNivel1.Text = "Nivel 1";
                lblAliasNivel2.Text = "Nivel 2";
                lblAliasNivel3.Text = "Nivel 3";
                lblAliasNivel4.Text = "Nivel 4";
                lblAliasNivel5.Text = "Nivel 5";
                lblAliasNivel6.Text = "Nivel 6";
                lblAliasNivel7.Text = "Nivel 7";
                if (ddlTipoUsuario.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexTodos) return;
                List<AliasUbicacion> alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue));
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


        private List<string> Alerta
        {
            set
            {
                if (value.Any())
                {
                    string error = value.Aggregate("<ul>", (current, s) => current + ("<li>" + s.Replace("\r\n", string.Empty) + "</li>"));
                    error += "</ul>";
                    ScriptManager.RegisterStartupScript(Page, typeof(Page), "ScriptErrorAlert", "ErrorAlert('Error','" + error + "');", true);
                }
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


        private void LimpiaCatalogoAltaCampus()
        {
            try
            {
                txtDescripcionCatalogo.Text = string.Empty;
                txtCp.Text = string.Empty;
                Metodos.LimpiarCombo(ddlColonia);
                txtMunicipio.Text = string.Empty;
                txtEstado.Text = string.Empty;
                txtCalle.Text = string.Empty;
                txtNoExt.Text = string.Empty;
                txtNoInt.Text = string.Empty;
                btnGuardarCatalogo.Visible = true;
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

        private void LimpiaCatalogoNivel()
        {
            try
            {
                txtDescripcionCatalogo.Text = string.Empty;
                btnGuardarCatalogo.Visible = true;
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
                btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                btnStatusNivel7.CssClass = "btn btn-primary btn-square";
                btnSeleccionarModal.Visible = true;
                divCapturaDescripcion.Visible = true;
                divNombre.Visible = true;

                //dataCampus.Visible = false;
                txtDescripcionCatalogo.Text = string.Empty;
                txtCp.Text = string.Empty;
                Metodos.LimpiarCombo(ddlColonia);
                txtMunicipio.Text = string.Empty;
                txtEstado.Text = string.Empty;
                txtCalle.Text = string.Empty;
                txtNoExt.Text = string.Empty;
                txtNoInt.Text = string.Empty;
                btnGuardarCatalogo.Visible = true;
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
                Alerta = new List<string>();
                _mp = (UsuariosMaster)Page.Master;
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
                btnStatusNivel1.CssClass = "btn btn-primary btn-square";
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
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos && int.Parse(ddlTipoUsuario.SelectedValue) != (int)BusinessVariables.EnumTiposUsuario.Operador)
                {
                    LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPais(int.Parse(ddlTipoUsuario.SelectedValue), true));
                }
                else
                {
                    LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPais(int.Parse(ddlTipoUsuario.SelectedValue), true));
                }
                if (Convert.ToBoolean(hfAlta.Value))
                {
                    lblOperacion.Text = lblAliasNivel1.Text;
                    lblOperacionDescripcion.Text = lblAliasNivel1.Text;
                    btnSeleccionarModal.CommandArgument = "1";
                    lblStepNivel1.Text = "...";
                    hfCatalogo.Value = "1";
                }
                if (EsAlta)
                {
                    divCapturaDescripcion.Visible = false;
                    divNombre.Visible = false;
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


        protected void btnSeleccionarModal_OnClick(object sender, EventArgs e)
        {
            try
            {

                if (ddlNivelSeleccionModal.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Debe seleccionar Nivel");
                dataCampus.Visible = false;
                switch (int.Parse(btnSeleccionarModal.CommandArgument))
                {
                    case 1:
                        btnStatusNivel1.CssClass = "btn btn-success btn-square";
                        lblStepNivel1.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel1.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerCampus(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        succNivel1.Visible = true;
                        btnSeleccionarModal.CommandArgument = "2";
                        hfCatalogo.Value = "2";
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        divStep2.Visible = true;
                        dataCampus.Visible = true;
                        lblStepNivel2.Text = "...";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        break;
                    case 2:
                        btnStatusNivel2.CssClass = "btn btn-success btn-square";
                        lblStepNivel2.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel2.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerTorres(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                        succNivel2.Visible = true;
                        btnSeleccionarModal.CommandArgument = "3";
                        hfCatalogo.Value = "3";
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = "...";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        break;
                    case 3:
                        btnStatusNivel3.CssClass = "btn btn-success btn-square";
                        lblStepNivel3.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel3.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPisos(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                        succNivel3.Visible = true;
                        btnSeleccionarModal.CommandArgument = "4";
                        hfCatalogo.Value = "4";
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = "...";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        break;
                    case 4:
                        btnStatusNivel4.CssClass = "btn btn-success btn-square";
                        lblStepNivel4.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel4.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                        succNivel4.Visible = true;
                        btnSeleccionarModal.CommandArgument = "5";
                        hfCatalogo.Value = "5";
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = "...";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        break;
                    case 5:
                        btnStatusNivel5.CssClass = "btn btn-success btn-square";
                        lblStepNivel5.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel5.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSubZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                        succNivel5.Visible = true;
                        btnSeleccionarModal.CommandArgument = "6";
                        hfCatalogo.Value = "6";
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = "...";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        break;
                    case 6:
                        btnStatusNivel6.CssClass = "btn btn-success btn-square";
                        lblStepNivel6.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel6.Value = ddlNivelSeleccionModal.SelectedValue;
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSiteRacks(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
                        succNivel6.Visible = true;
                        btnSeleccionarModal.CommandArgument = "7";
                        hfCatalogo.Value = "7";
                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        divStep7.Visible = true;
                        lblStepNivel7.Text = "...";
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";
                        break;
                    case 7:
                        btnStatusNivel7.CssClass = "btn btn-success btn-square";
                        lblStepNivel7.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        hfNivel7.Value = ddlNivelSeleccionModal.SelectedValue;
                        succNivel7.Visible = true;
                        break;

                }
                if (EsAlta)
                {
                    divCapturaDescripcion.Visible = int.Parse(btnSeleccionarModal.CommandArgument) > 1;
                    divNombre.Visible = int.Parse(btnSeleccionarModal.CommandArgument) > 1;

                }
            }
            catch (Exception ex)
            {
                divCapturaDescripcion.Visible = false;
                divNombre.Visible = false;
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
        protected void txtCp_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlColonia, _servicioSistemaDomicilio.ObtenerColoniasCp(int.Parse(txtCp.Text.Trim()), true));
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
        protected void ddlColonia_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlColonia.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    txtMunicipio.Text = string.Empty;
                    txtEstado.Text = string.Empty;
                    return;
                }
                Colonia col = _servicioSistemaDomicilio.ObtenerDetalleColonia(int.Parse(ddlColonia.SelectedValue));
                if (col != null)
                {
                    txtMunicipio.Text = col.Municipio.Descripcion;
                    txtEstado.Text = col.Municipio.Estado.Descripcion;
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

        public bool ValidaCapturaCatalogoCampus(int idTipoUsuario, string descripcion, int idColonia, string calle, string noExt, string noInt)
        {
            List<string> sb = new List<string>();
            if (idTipoUsuario == BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                sb.Add("Tipo de usuario es un campo obligatorio.<br>");
            if (descripcion == string.Empty)
                sb.Add("Descripción es un campo obligatorio.<br>");
            if (txtCp.Text.Trim() == string.Empty)
                sb.Add("Ingrese un Codigo Postal.<br>");
            if (idColonia == BusinessVariables.ComboBoxCatalogo.ValueSeleccione)
                sb.Add("Colonia es un campo obligatorio.<br>");
            if (calle == string.Empty)
                sb.Add("Calle es un campo obligatorio.<br>");
            if (noExt == string.Empty)
                sb.Add("Número Exterior es un campo obligatorio.");
            //if (noInt == string.Empty)
            //    sb.AppendLine("Número Interior es un campo obligatorio.<br>");
            if (sb.Count > 0)
            {
                sb.Insert(0, "<h3>Datos Generales</h3>");
                _lstError = sb;
                throw new Exception("");
            }
            return true;
        }

        private void Guardar()
        {
            try
            {
                Ubicacion ubicacion;
                #region Es Nivel 2
                if (hfCatalogo.Value == "2")
                {
                    if (Convert.ToBoolean(hfAlta.Value))
                    {
                        if (ValidaCapturaCatalogoCampus(Convert.ToInt32(ddlTipoUsuario.SelectedValue), txtDescripcionCatalogo.Text, ddlColonia.SelectedValue == "" ? 0 : Convert.ToInt32(ddlColonia.SelectedValue), txtCalle.Text.Trim(), txtNoExt.Text.Trim(), txtNoInt.Text.Trim()))
                        {
                            ubicacion = new Ubicacion
                            {
                                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                IdPais = Convert.ToInt32(hfNivel1.Value),
                                Campus = new Campus
                                {
                                    IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                                    Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                    Domicilio = new List<Domicilio>
                                    {
                                        new Domicilio
                                        {
                                            IdColonia = Convert.ToInt32(ddlColonia.SelectedValue),
                                            Calle = txtCalle.Text.Trim(),
                                            NoExt = txtNoExt.Text.Trim(),
                                            NoInt = txtNoInt.Text.Trim()
                                        }
                                    },
                                    Habilitado = chkHabilitado.Checked
                                }
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            if (OnAceptarModal != null)
                                OnAceptarModal();
                            // AlertaSucces = txtDescripcionCatalogo.Text + " se guardó correctamente.";  
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerCampus(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        }
                        _mp.AlertaSucces();
                        ddlNivelSeleccionModal.SelectedIndex = ddlNivelSeleccionModal.Items.IndexOf(ddlNivelSeleccionModal.Items.FindByText(txtDescripcionCatalogo.Text.Trim()));

                    }
                    else
                    {
                        if (Metodos.ValidaCapturaCatalogoCampus(Convert.ToInt32(ddlTipoUsuario.SelectedValue), txtDescripcionCatalogo.Text, ddlColonia.SelectedValue == "" ? 0 : Convert.ToInt32(ddlColonia.SelectedValue), txtCalle.Text.Trim(), txtNoExt.Text.Trim(), txtNoInt.Text.Trim()))
                        {
                            ubicacion = (Ubicacion)Session["UbicacionSeleccionada"];
                            ubicacion.Campus.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            ubicacion.Campus.Domicilio = ubicacion.Campus.Domicilio ?? new List<Domicilio>();
                            if (ubicacion.Campus.Domicilio.Count == 0)
                                ubicacion.Campus.Domicilio.Add(new Domicilio());
                            ubicacion.Campus.Domicilio[0].IdColonia = Convert.ToInt32(ddlColonia.SelectedValue);
                            ubicacion.Campus.Domicilio[0].Calle = txtCalle.Text.Trim();
                            ubicacion.Campus.Domicilio[0].NoExt = txtNoExt.Text.Trim();
                            ubicacion.Campus.Domicilio[0].NoInt = txtNoInt.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerCampus(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        }
                    }
                    LimpiaCatalogoAltaCampus();
                }

                #endregion

                #region No es Nivel 2
                else
                {
                    if (!Metodos.ValidaCapturaCatalogo(txtDescripcionCatalogo.Text)) return; // Valida nombre de nivel

                    if (Convert.ToBoolean(hfAlta.Value))
                    {
                        int idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);
                        ubicacion = new Ubicacion
                        {
                            IdTipoUsuario = idTipoUsuario,
                            IdPais = Convert.ToInt32(hfNivel1.Value)
                        };
                        switch (int.Parse(hfCatalogo.Value))
                        {
                            case 3:
                                ubicacion.IdCampus = Convert.ToInt32(hfNivel2.Value);
                                ubicacion.Torre = new Torre
                                {
                                    IdTipoUsuario = idTipoUsuario,
                                    Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                    Habilitado = chkHabilitado.Checked
                                };
                                _servicioUbicacion.GuardarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerTorres(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                                break;
                            case 4:
                                ubicacion.IdCampus = Convert.ToInt32(hfNivel2.Value);
                                ubicacion.IdTorre = Convert.ToInt32(hfNivel3.Value);
                                ubicacion.Piso = new Piso
                                {
                                    IdTipoUsuario = idTipoUsuario,
                                    Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                    Habilitado = chkHabilitado.Checked
                                };
                                _servicioUbicacion.GuardarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPisos(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                                break;
                            case 5:
                                ubicacion.IdCampus = Convert.ToInt32(hfNivel2.Value);
                                ubicacion.IdTorre = Convert.ToInt32(hfNivel3.Value);
                                ubicacion.IdPiso = Convert.ToInt32(hfNivel4.Value);
                                ubicacion.Zona = new Zona
                                {
                                    IdTipoUsuario = idTipoUsuario,
                                    Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                    Habilitado = chkHabilitado.Checked
                                };
                                _servicioUbicacion.GuardarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                                break;
                            case 6:
                                ubicacion.IdCampus = Convert.ToInt32(hfNivel2.Value);
                                ubicacion.IdTorre = Convert.ToInt32(hfNivel3.Value);
                                ubicacion.IdPiso = Convert.ToInt32(hfNivel4.Value);
                                ubicacion.IdZona = Convert.ToInt32(hfNivel5.Value);
                                ubicacion.SubZona = new SubZona
                                {
                                    IdTipoUsuario = idTipoUsuario,
                                    Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                    Habilitado = chkHabilitado.Checked
                                };
                                _servicioUbicacion.GuardarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSubZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                                break;
                            case 7:
                                ubicacion.IdCampus = Convert.ToInt32(hfNivel2.Value);
                                ubicacion.IdTorre = Convert.ToInt32(hfNivel3.Value);
                                ubicacion.IdPiso = Convert.ToInt32(hfNivel4.Value);
                                ubicacion.IdZona = Convert.ToInt32(hfNivel5.Value);
                                ubicacion.IdSubZona = Convert.ToInt32(hfNivel6.Value);
                                ubicacion.SiteRack = new SiteRack
                                {
                                    IdTipoUsuario = idTipoUsuario,
                                    Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                    Habilitado = chkHabilitado.Checked
                                };
                                _servicioUbicacion.GuardarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSiteRacks(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
                                break;
                        }

                        _mp.AlertaSucces();
                        ddlNivelSeleccionModal.SelectedIndex = ddlNivelSeleccionModal.Items.IndexOf(ddlNivelSeleccionModal.Items.FindByText(txtDescripcionCatalogo.Text.Trim()));
                    }



                    else
                    {
                        ubicacion = (Ubicacion)Session["UbicacionSeleccionada"];
                        switch (int.Parse(hfCatalogo.Value))
                        {
                            case 1:
                                ubicacion.Pais.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                break;
                            case 2:
                                ubicacion.Campus.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                _servicioUbicacion.ActualizarUbicacion(ubicacion);
                                break;
                            case 3:
                                ubicacion.Torre.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                _servicioUbicacion.ActualizarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerTorres(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                                break;
                            case 4:
                                ubicacion.Piso.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                _servicioUbicacion.ActualizarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPisos(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                                break;
                            case 5:
                                ubicacion.Zona.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                _servicioUbicacion.ActualizarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                                break;
                            case 6:
                                ubicacion.SubZona.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                _servicioUbicacion.ActualizarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSubZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                                break;
                            case 7:
                                ubicacion.SiteRack.Descripcion = txtDescripcionCatalogo.Text.Trim();
                                _servicioUbicacion.ActualizarUbicacion(ubicacion);
                                LlenaComboDinamico(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSiteRacks(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));
                                break;
                        }
                        _mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Actualizacion));
                    }

                    dataCampus.Visible = false;
                }
                #endregion

                LimpiaCatalogoNivel();
                if (OnAceptarModal != null)
                    OnAceptarModal();
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
                    _lstError = new List<string>();
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
        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (EsSeleccion)
                {
                    switch (int.Parse(btnSeleccionarModal.CommandArgument))
                    {
                        case 1:
                            throw new Exception("Selecciones una ubicacion de nivel 2");
                            if (btnStatusNivel1.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), null, null, null, null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 1);
                            }
                            break;
                        case 2:
                            if (btnStatusNivel2.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), null, null, null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 2);
                            }
                            else
                            {
                                throw new Exception("Selecciones una ubicacion de nivel 2");
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), null, null, null, null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 1);
                            }
                            break;
                        case 3:
                            if (btnStatusNivel3.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), null, null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 3);
                            }
                            else
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), null, null, null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 2);
                            }
                            break;
                        case 4:
                            if (btnStatusNivel4.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 4);
                            }
                            else
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), null, null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 3);
                            }
                            break;
                        case 5:
                            if (btnStatusNivel5.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), null, null).SingleOrDefault(s => s.IdNivelUbicacion == 5);
                            }
                            else
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), null, null, null).SingleOrDefault(s => s.IdNivelUbicacion == 4);
                            }
                            break;
                        case 6:
                            if (btnStatusNivel6.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), int.Parse(hfNivel6.Value), null).SingleOrDefault(s => s.IdNivelUbicacion == 6);
                            }
                            else
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), null, null).SingleOrDefault(s => s.IdNivelUbicacion == 5);
                            }
                            break;
                        case 7:
                            if (btnStatusNivel7.CssClass == "btn btn-success btn-square")
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), int.Parse(hfNivel6.Value), int.Parse(hfNivel7.Value)).SingleOrDefault(s => s.IdNivelUbicacion == 7);
                            }
                            else
                            {
                                UbicacionSeleccionada = _servicioUbicacion.ObtenerUbicaciones(IdTipoUsuario, int.Parse(hfNivel1.Value), int.Parse(hfNivel2.Value), int.Parse(hfNivel3.Value), int.Parse(hfNivel4.Value), int.Parse(hfNivel5.Value), int.Parse(hfNivel6.Value), null).SingleOrDefault(s => s.IdNivelUbicacion == 6);
                            }
                            break;
                    }
                }
                else if (!EsAlta && !EsSeleccion)
                {
                    Guardar();
                    //btnGuardarCatalogo_OnClick(btnGuardarCatalogo, null);
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
                        dataCampus.Visible = false;
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerCampus(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                        succNivel1.Visible = true;
                        btnSeleccionarModal.CommandArgument = "2";
                        hfCatalogo.Value = "2";
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        divStep2.Visible = true;
                        dataCampus.Visible = true;
                        lblStepNivel2.Text = "...";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        break;
                    case 3:
                        divStep4.Visible = false;
                        divStep5.Visible = false;
                        divStep6.Visible = false;
                        divStep7.Visible = false;
                        dataCampus.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerTorres(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                        succNivel2.Visible = true;
                        btnSeleccionarModal.CommandArgument = "3";
                        hfCatalogo.Value = "3";
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = "...";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";

                        break;
                    case 4:
                        divStep5.Visible = false;
                        divStep6.Visible = false;
                        divStep7.Visible = false;
                        dataCampus.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerPisos(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                        succNivel3.Visible = true;
                        btnSeleccionarModal.CommandArgument = "4";
                        hfCatalogo.Value = "4";
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = "...";

                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        break;
                    case 5:
                        divStep6.Visible = false;
                        divStep7.Visible = false;
                        dataCampus.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                        succNivel4.Visible = true;
                        btnSeleccionarModal.CommandArgument = "5";
                        hfCatalogo.Value = "5";
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = "...";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        break;
                    case 6:
                        divStep7.Visible = false;
                        dataCampus.Visible = false;

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioUbicacion.ObtenerSubZonas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                        succNivel5.Visible = true;
                        btnSeleccionarModal.CommandArgument = "6";
                        hfCatalogo.Value = "6";
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = "...";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";

                        break;
                    case 7:
                        btnStatusNivel7.CssClass = "btn btn-success btn-square";
                        succNivel7.Visible = true;
                        lblStepNivel7.Text = "...";
                        btnStatusNivel7.CssClass = "btn btn-primary btn-square";
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
