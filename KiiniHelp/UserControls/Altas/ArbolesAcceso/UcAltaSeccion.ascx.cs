using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceOrganizacion;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.ArbolesAcceso
{
    public partial class UcAltaSeccion : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private UsuariosMaster _mp;
        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceArbolAccesoClient _servicioArbol = new ServiceArbolAccesoClient();
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
        public void SetNivelActualizar()
        {
            try
            {
                ArbolAcceso arbol = _servicioArbol.ObtenerArbolAcceso(IdArbol);
                if (arbol == null) return;
                int nivelArbol = 0;
                if (arbol.Nivel1 != null)
                    nivelArbol = 1;
                if (arbol.Nivel2 != null)
                    nivelArbol = 2;
                if (arbol.Nivel3 != null)
                    nivelArbol = 3;
                if (arbol.Nivel4 != null)
                    nivelArbol = 4;
                if (arbol.Nivel5 != null)
                    nivelArbol = 5;
                if (arbol.Nivel6 != null)
                    nivelArbol = 6;
                if (arbol.Nivel7 != null)
                    nivelArbol = 7;

                btnSeleccionarModal.CommandArgument = nivelArbol.ToString();
                Session["ArbolSeleccionado"] = arbol;
                hfCatalogo.Value = nivelArbol.ToString();
                ddlTipoUsuario.SelectedValue = arbol.IdTipoUsuario.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                ddlNivelSeleccionModal.Visible = EsAlta ;
                switch (nivelArbol)
                {
                    case 1:
                        ddlNivelSeleccionModal.Enabled = !EsAlta;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        LlenaCombosModal();
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel1.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel1.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-seleccione btn-square";
                        lblStepNivel1.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel1.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel1.Text;

                        break;
                    case 2:
                        ddlNivelSeleccionModal.Enabled = !EsAlta ;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel1.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel2.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel2.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        divStep2.Visible = true;

                        lblStepNivel2.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        break;
                    case 3:
                        ddlNivelSeleccionModal.Enabled = !EsAlta ;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal,_servicioArbol.ObtenerNivel3(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel2.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel3.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel3.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        divStep3.Visible = true;

                        lblStepNivel3.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        break;
                    case 4:
                        ddlNivelSeleccionModal.Enabled = !EsAlta ;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel4(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel3.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel4.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel4.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        divStep4.Visible = true;

                        lblStepNivel4.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        break;
                    case 5:
                        ddlNivelSeleccionModal.Enabled = !EsAlta ;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = arbol.IdNivel5.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel5(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel4.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel5.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel5.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        divStep5.Visible = true;

                        lblStepNivel5.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        break;
                    case 6:
                        ddlNivelSeleccionModal.Enabled = !EsAlta ;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = arbol.IdNivel5.ToString();
                        hfNivel6.Value = arbol.IdNivel6.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel6(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel5.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel6.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel6.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";
                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = arbol.Nivel5.Descripcion;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = ddlNivelSeleccionModal.SelectedItem.Text;
                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;
                        break;
                    case 7:
                        ddlNivelSeleccionModal.Enabled = !EsAlta ;
                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = arbol.IdNivel5.ToString();
                        hfNivel6.Value = arbol.IdNivel6.ToString();
                        hfNivel7.Value = arbol.IdNivel7.ToString();
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel7(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel6.Value), true));
                        ddlNivelSeleccionModal.SelectedValue = arbol.IdNivel7.ToString();
                        txtDescripcionCatalogo.Text = arbol.Nivel7.Descripcion;
                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        divStep2.Visible = true;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        divStep3.Visible = true;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        divStep4.Visible = true;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        divStep5.Visible = true;
                        lblStepNivel5.Text = arbol.Nivel5.Descripcion;
                        divStep6.Visible = true;
                        lblStepNivel6.Text = arbol.Nivel6.Descripcion;
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
        public void SetNivelSeleccion()
        {
            try
            {
                ArbolAcceso arbol = _servicioArbol.ObtenerArbolAcceso(IdArbol);
                if (arbol == null) return;
                int nivelArbol = 0;
                if (arbol.Nivel1 != null)
                    nivelArbol = 1;
                if (arbol.Nivel2 != null)
                    nivelArbol = 2;
                if (arbol.Nivel3 != null)
                    nivelArbol = 3;
                if (arbol.Nivel4 != null)
                    nivelArbol = 4;
                if (arbol.Nivel5 != null)
                    nivelArbol = 5;
                if (arbol.Nivel6 != null)
                    nivelArbol = 6;
                if (arbol.Nivel7 != null)
                    nivelArbol = 7;

                IdTipoUsuario = arbol.IdTipoUsuario;
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                btnSeleccionarModal.CommandArgument = nivelArbol.ToString();
                Session["ArbolSeleccionado"] = arbol;
                hfCatalogo.Value = nivelArbol.ToString();
                divStep1.Visible = false;
                divStep2.Visible = false;
                divStep3.Visible = false;
                divStep4.Visible = false;
                divStep5.Visible = false;
                divStep6.Visible = false;
                divStep7.Visible = false;
                pnlAlta.Visible = EsAlta;
                switch (nivelArbol)
                {
                    case 1:
                        divStep1.Visible = true;
                        divStep2.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel1.Value), true));

                        lblOperacion.Text = lblAliasNivel2.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel2.Text;
                        hfCatalogo.Value = "2";
                        break;
                    case 2:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        lblStepNivel3.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel3(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel2.Value), true));

                        lblOperacion.Text = lblAliasNivel3.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel3.Text;
                        hfCatalogo.Value = "3";
                        break;
                    case 3:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        lblStepNivel4.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel3.Value), true));

                        lblOperacion.Text = lblAliasNivel4.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel4.Text;
                        hfCatalogo.Value = "4";
                        break;
                    case 4:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        lblStepNivel5.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel4.Value), true));

                        lblOperacion.Text = lblAliasNivel5.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel5.Text;
                        hfCatalogo.Value = "5";
                        break;
                    case 5:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;
                        divStep6.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        lblStepNivel5.Text = arbol.Nivel5.Descripcion;
                        lblStepNivel6.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = arbol.IdNivel5.ToString();
                        hfNivel6.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel5.Value), true));

                        lblOperacion.Text = lblAliasNivel6.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel6.Text;

                        hfCatalogo.Value = "6";
                        break;
                    case 6:

                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;
                        divStep6.Visible = true;
                        divStep7.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        lblStepNivel5.Text = arbol.Nivel5.Descripcion;
                        lblStepNivel6.Text = arbol.Nivel6.Descripcion;
                        lblStepNivel7.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = arbol.IdNivel5.ToString();
                        hfNivel6.Value = arbol.IdNivel6.ToString();
                        hfNivel7.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";
                        btnSeleccionarModal.CommandArgument = (int.Parse(btnSeleccionarModal.CommandArgument) + 1).ToString();

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel6.Value), true));

                        lblOperacion.Text = lblAliasNivel7.Text;
                        lblOperacionDescripcion.Text = lblAliasNivel7.Text;
                        hfCatalogo.Value = "7";
                        break;
                    case 7:
                        divStep1.Visible = true;
                        divStep2.Visible = true;
                        divStep3.Visible = true;
                        divStep4.Visible = true;
                        divStep5.Visible = true;
                        divStep6.Visible = true;
                        divStep7.Visible = true;

                        lblStepNivel1.Text = arbol.Nivel1.Descripcion;
                        lblStepNivel2.Text = arbol.Nivel2.Descripcion;
                        lblStepNivel3.Text = arbol.Nivel3.Descripcion;
                        lblStepNivel4.Text = arbol.Nivel4.Descripcion;
                        lblStepNivel5.Text = arbol.Nivel5.Descripcion;
                        lblStepNivel6.Text = arbol.Nivel6.Descripcion;
                        lblStepNivel7.Text = "...";

                        hfNivel1.Value = arbol.IdNivel1.ToString();
                        hfNivel2.Value = arbol.IdNivel2.ToString();
                        hfNivel3.Value = arbol.IdNivel3.ToString();
                        hfNivel4.Value = arbol.IdNivel4.ToString();
                        hfNivel5.Value = arbol.IdNivel5.ToString();
                        hfNivel6.Value = arbol.IdNivel6.ToString();
                        hfNivel7.Value = string.Empty;

                        btnStatusNivel1.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel2.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel3.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel4.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel5.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel6.CssClass = "btn btn-primary btn-square";
                        btnStatusNivel7.CssClass = "btn btn-seleccione btn-square";

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea, arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel6.Value), true));

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
        public int IdArbol
        {
            get { return int.Parse(hfIdArbol.Value); }
            set
            {
                hfIdArbol.Value = value.ToString();
            }
        }
        public ArbolAcceso ArbolSeleccionado
        {
            get
            {
                return hfArbolSeleccionado.Value == string.Empty ? new ArbolAcceso() : _servicioArbol.ObtenerArbolAcceso(int.Parse(hfArbolSeleccionado.Value));
            }
            set
            {
                if (value.Id == 0)
                {
                    hfArbolSeleccionado.Value = string.Empty;
                }
                else
                    hfArbolSeleccionado.Value = value.Id.ToString();
                IdArbol = value.Id;
                EsAlta = false;
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

        private void LlenaCombosModal()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true, false);
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
                    Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel1(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, true));
                }
                else
                {
                    pnlAlta.Visible = false;
                    Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel1(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, true));
                }
                if (EsAlta)
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel1.Value), true));
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
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel3(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel2.Value), true));
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
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel4(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel3.Value), true));
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
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel5(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel4.Value), true));
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
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel6(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel5.Value), true));
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
                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel7(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel6.Value), true));
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
                        btnTerminar_OnClick(sender, e);
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
                ArbolAcceso arbol;
                //if (EsAlta)
                //{
                //    arbol = new ArbolAcceso();
                //    {
                //        IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue)
                //    };
                //    switch (int.Parse(hfCatalogo.Value))
                //    {
                //        case 1:
                //            arbol.Holding = new Holding
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            if (OnAceptarModal != null)
                //                OnAceptarModal();
                //            ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                //            break;
                //        case 2:
                //            arbol.IdHolding = int.Parse(hfNivel1.Value);
                //            arbol.Compania = new Compania
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerCompañias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel1.Value), true));
                //            break;
                //        case 3:
                //            arbol.IdHolding = int.Parse(hfNivel1.Value);
                //            arbol.IdCompania = int.Parse(hfNivel2.Value);
                //            arbol.Direccion = new Direccion
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel2.Value), true));
                //            break;
                //        case 4:
                //            arbol.IdHolding = int.Parse(hfNivel1.Value);
                //            arbol.IdCompania = int.Parse(hfNivel2.Value);
                //            arbol.IdDireccion = int.Parse(hfNivel3.Value);
                //            arbol.SubDireccion = new SubDireccion
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubDirecciones(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel3.Value), true));
                //            break;
                //        case 5:
                //            arbol.IdHolding = int.Parse(hfNivel1.Value);
                //            arbol.IdCompania = int.Parse(hfNivel2.Value);
                //            arbol.IdDireccion = int.Parse(hfNivel3.Value);
                //            arbol.IdSubDireccion = int.Parse(hfNivel4.Value);
                //            arbol.Gerencia = new Gerencia
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel4.Value), true));
                //            break;
                //        case 6:
                //            arbol.IdHolding = int.Parse(hfNivel1.Value);
                //            arbol.IdCompania = int.Parse(hfNivel2.Value);
                //            arbol.IdDireccion = int.Parse(hfNivel3.Value);
                //            arbol.IdSubDireccion = int.Parse(hfNivel4.Value);
                //            arbol.IdGerencia = int.Parse(hfNivel5.Value);
                //            arbol.SubGerencia = new SubGerencia
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerSubGerencias(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel5.Value), true));
                //            break;
                //        case 7:
                //            arbol.IdHolding = int.Parse(hfNivel1.Value);
                //            arbol.IdCompania = int.Parse(hfNivel2.Value);
                //            arbol.IdDireccion = int.Parse(hfNivel3.Value);
                //            arbol.IdSubDireccion = int.Parse(hfNivel4.Value);
                //            arbol.IdGerencia = int.Parse(hfNivel5.Value);
                //            arbol.IdSubGerencia = int.Parse(hfNivel6.Value);
                //            arbol.Jefatura = new Jefatura
                //            {
                //                IdTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue),
                //                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                //                Habilitado = chkHabilitado.Checked
                //            };
                //            _servicioOrganizacion.GuardarOrganizacion(arbol);
                //            if (OnAceptarModal != null)
                //                OnAceptarModal();
                //            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioOrganizacion.ObtenerJefaturas(int.Parse(ddlTipoUsuario.SelectedValue), int.Parse(hfNivel6.Value), true));

                //            break;
                //    }
                //    _mp.AlertaSucces();
                //    ddlNivelSeleccionModal.SelectedIndex = ddlNivelSeleccionModal.Items.IndexOf(ddlNivelSeleccionModal.Items.FindByText(txtDescripcionCatalogo.Text.Trim()));
                //}
                //else
                //{
                    arbol = (ArbolAcceso)Session["ArbolSeleccionado"];
                    switch (int.Parse(hfCatalogo.Value))
                    {
                        case 1:
                            arbol.Nivel1.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaCombosModal();
                            break;
                        case 2:
                            arbol.Nivel2.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(arbol.IdArea,arbol.IdTipoArbolAcceso, arbol.IdTipoUsuario, int.Parse(hfNivel1.Value), true));
                            break;
                        case 3:
                            arbol.Nivel3.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel3(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel2.Value), true));
                            break;
                        case 4:
                            arbol.Nivel4.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel4(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel3.Value), true));
                            break;
                        case 5:
                            arbol.Nivel5.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel5(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel4.Value), true));
                            break;
                        case 6:
                            arbol.Nivel6.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel6(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel5.Value), true));
                            break;
                        case 7:
                            arbol.Nivel7.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioArbol.ActualizarSeccion(arbol.Id, arbol, txtDescripcionCatalogo.Text.Trim());
                            LlenaComboDinamico(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel7(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel6.Value), true));
                            break;
                    }

                    _mp.AlertaSucces(BusinessErrores.ObtenerMensajeByKey(BusinessVariables.EnumMensajes.Actualizacion));
                //}

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
        protected void btnTerminar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!EsAlta )
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
        protected void btnStatusNivel1_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;

                if (!EsAlta )
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel2(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel1.Value), true));
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel3(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel2.Value), true));
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel4(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel3.Value), true));
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel5(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel4.Value), true));
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

                        Metodos.LlenaComboCatalogo(ddlNivelSeleccionModal, _servicioArbol.ObtenerNivel6(ArbolSeleccionado.IdArea, ArbolSeleccionado.IdTipoArbolAcceso, ArbolSeleccionado.IdTipoUsuario, int.Parse(hfNivel5.Value), true));
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