﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniHelp.ServiceTicket;
using KiiniHelp.ServiceUsuario;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using RepeatDirection = System.Web.UI.WebControls.RepeatDirection;

namespace KiiniHelp.UserControls.Temporal
{
    public partial class UcTicketPortal : UserControl, IControllerModal
    {


        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceCatalogosClient _servicioCatalogos = new ServiceCatalogosClient();
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private readonly ServiceTicketClient _servicioTicket = new ServiceTicketClient();
        private readonly ServiceParametrosClient _serviciosParametros = new ServiceParametrosClient();
        private readonly ServiceArbolAccesoClient _servicioArbolAccesoClient = new ServiceArbolAccesoClient();
        private readonly ServiceUsuariosClient _servicioUsuariosClient = new ServiceUsuariosClient();

        private List<Control> _lstControles;
        private List<string> _lstError = new List<string>();
        private bool ValidCaptcha = false;
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

        protected void Page_PreInit(object sender, EventArgs e)
        {
            try
            {
                Control myControl = GetPostBackControl(Page);

                if ((myControl != null))
                {
                    if ((myControl.ClientID == "btnAddTextBox"))
                    {

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
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                _lstControles = new List<Control>();
                ucAltaUsuarioRapida.Visible = Session["UserData"] == null;
                ArbolAcceso arbol = _servicioArbolAccesoClient.ObtenerArbolAcceso(int.Parse(_serviciosParametros.ObtenerParametrosGenerales().FormularioPortal));
                if (arbol != null)
                {
                    int? idMascara = arbol.InventarioArbolAcceso.First().IdMascara;
                    if (idMascara != null) IdMascara = (int)idMascara;
                    Mascara mascara = _servicioMascaras.ObtenerMascaraCaptura(IdMascara);
                    if (mascara != null)
                    {
                        hfComandoInsertar.Value = mascara.ComandoInsertar;
                        hfComandoActualizar.Value = mascara.ComandoInsertar;
                        hfRandom.Value = mascara.Random.ToString();
                        ParametrosGenerales parametros = _serviciosParametros.ObtenerParametrosGenerales();
                        if (parametros != null)
                        {
                            foreach (ArchivosPermitidos alowedFile in _serviciosParametros.ObtenerArchivosPermitidos())
                            {
                                ArchivosPermitidos += string.Format("{0}|", alowedFile.Extensiones);
                            }
                            TamañoArchivo = double.Parse(parametros.TamanoDeArchivo);
                        }
                        PintaControles(mascara.CampoMascara);
                        Session["DataFormularioPortal"] = mascara;
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

        public static Control GetPostBackControl(Page thePage)
        {
            Control myControl = null;
            string ctrlName = thePage.Request.Params.Get("__EVENTTARGET");
            if (((ctrlName != null) & (ctrlName != string.Empty)))
            {
                myControl = thePage.FindControl(ctrlName);
            }
            else
            {
                foreach (string item in thePage.Request.Form)
                {
                    Control c = thePage.FindControl(item);
                    if (((c) is Button))
                    {
                        myControl = c;
                    }
                }

            }
            return myControl;
        }

        public int IdMascara
        {
            get { return Convert.ToInt32(hfIdMascara.Value); }
            set { hfIdMascara.Value = value.ToString(); }
        }

        public double TamañoArchivo
        {
            get
            {
                return double.Parse(hfMaxSizeAllow.Value);
            }
            set { hfMaxSizeAllow.Value = value.ToString(); }
        }

        public string ArchivosPermitidos
        {
            get
            {
                return hfFileTypes.Value;
            }
            set
            {
                hfFileTypes.Value = value;
            }
        }

        public string ComandoInsertar
        {
            get { return hfComandoInsertar.Value; }
        }

        public string ComandoActualizar
        {
            get { return hfComandoActualizar.Value; }
        }

        public bool CampoRandom
        {
            get { return Convert.ToBoolean(hfRandom.Value); }
        }

        public int TicketGenerado
        {
            get { return int.Parse(hfTicketGenerado.Value); }
        }
        public string RandomGenerado
        {
            get { return hfRandomGenerado.Value; }
        }
        public void ValidaMascaraCaptura()
        {
            try
            {
                Mascara mascara = (Mascara)Session["DataFormularioPortal"];
                mascara = mascara ?? _servicioMascaras.ObtenerMascaraCaptura(IdMascara);
                foreach (CampoMascara campo in mascara.CampoMascara)
                {
                    string nombreControl;
                    switch (campo.IdTipoCampoMascara)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtSimple = (TextBox)divControles.FindControl(nombreControl);
                            if (txtSimple != null)
                            {
                                if (campo.Requerido)
                                    if (txtSimple.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {
                                        if (txtSimple.Text.Trim().Length < campo.LongitudMinima)
                                            throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                        if (txtSimple.Text.Trim().Length > campo.LongitudMaxima)
                                            throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                    }
                                if (!campo.Requerido && txtSimple.Text.Trim() != string.Empty)
                                {
                                    if (txtSimple.Text.Trim().Length < campo.LongitudMinima)
                                        throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                    if (txtSimple.Text.Trim().Length > campo.LongitudMaxima)
                                        throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                }

                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtMultilinea = (TextBox)divControles.FindControl(nombreControl);
                            if (txtMultilinea != null)
                            {
                                if (campo.Requerido)
                                    if (txtMultilinea.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {
                                        if (txtMultilinea.Text.Trim().Length < campo.LongitudMinima)
                                            throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                        if (txtMultilinea.Text.Trim().Length > campo.LongitudMaxima)
                                            throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                    }
                                if (!campo.Requerido && txtMultilinea.Text.Trim() != string.Empty)
                                {
                                    if (txtMultilinea.Text.Trim().Length < campo.LongitudMinima)
                                        throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                    if (txtMultilinea.Text.Trim().Length > campo.LongitudMaxima)
                                        throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                }


                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            nombreControl = "lstRadio" + campo.NombreCampo;
                            RadioButtonList rbtnList = (RadioButtonList)divControles.FindControl(nombreControl);
                            if (rbtnList != null)
                            {
                                if (campo.Requerido)
                                    if (rbtnList.SelectedIndex < (BusinessVariables.ComboBoxCatalogo.IndexSeleccione))
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                            nombreControl = "ddl" + campo.NombreCampo;
                            DropDownList ddl = (DropDownList)divControles.FindControl(nombreControl);
                            if (ddl != null)
                            {
                                if (campo.Requerido)
                                    if (ddl.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            nombreControl = "chklist" + campo.NombreCampo;
                            CheckBoxList chklist = (CheckBoxList)divControles.FindControl(nombreControl);
                            if (chklist != null)
                            {
                                if (campo.Requerido)
                                    if (!chklist.Items.Cast<ListItem>().Any(item => item.Selected))
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtEntero = (TextBox)divControles.FindControl(nombreControl);
                            if (txtEntero != null)
                            {
                                if (campo.Requerido)
                                    if (txtEntero.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {
                                        if (int.Parse(txtEntero.Text.Trim()) < campo.ValorMinimo)
                                            throw new Exception(string.Format("Campo {0} debe se mayor o igual a {1}", campo.Descripcion, campo.ValorMinimo));
                                        if (int.Parse(txtEntero.Text.Trim()) > campo.ValorMaximo)
                                            throw new Exception(string.Format("Campo {0} debe se menor o igual a {1}", campo.Descripcion, campo.ValorMaximo));
                                    }
                                if (!campo.Requerido && txtEntero.Text.Trim() != string.Empty)
                                {
                                    if (int.Parse(txtEntero.Text.Trim()) < campo.ValorMinimo)
                                        throw new Exception(string.Format("Campo {0} debe se mayor o igual a {1}", campo.Descripcion, campo.ValorMinimo));
                                    if (int.Parse(txtEntero.Text.Trim()) > campo.ValorMaximo)
                                        throw new Exception(string.Format("Campo {0} debe se menor o igual a {1}", campo.Descripcion, campo.ValorMaximo));
                                }
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtDecimal = (TextBox)divControles.FindControl(nombreControl);
                            if (txtDecimal != null)
                            {
                                if (campo.Requerido)
                                    if (txtDecimal.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {
                                        if (decimal.Parse(txtDecimal.Text.Trim().Replace('_', '0')) < campo.ValorMinimo)
                                            throw new Exception(string.Format("Campo {0} debe se mayor o igual a {1}", campo.Descripcion, campo.ValorMinimo));
                                        if (decimal.Parse(txtDecimal.Text.Trim().Replace('_', '0')) > campo.ValorMaximo)
                                            throw new Exception(string.Format("Campo {0} debe se menor o igual a {1}", campo.Descripcion, campo.ValorMaximo));
                                    }
                                if (!campo.Requerido && txtDecimal.Text.Trim() != string.Empty)
                                {
                                    if (decimal.Parse(txtDecimal.Text.Trim().Replace('_', '0')) < campo.ValorMinimo)
                                        throw new Exception(string.Format("Campo {0} debe se mayor o igual a {1}", campo.Descripcion, campo.ValorMinimo));
                                    if (decimal.Parse(txtDecimal.Text.Trim().Replace('_', '0')) > campo.ValorMaximo)
                                        throw new Exception(string.Format("Campo {0} debe se menor o igual a {1}", campo.Descripcion, campo.ValorMaximo));
                                }
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:

                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:

                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtFecha = (TextBox)divControles.FindControl(nombreControl);
                            if (txtFecha != null)
                            {
                                try
                                {
                                    if (txtFecha.Text.Trim() != String.Empty)
                                    {
                                        var d = DateTime.ParseExact(txtFecha.Text.Trim(), "dd/MM/yyyy", null);
                                    }
                                }
                                catch
                                {
                                    throw new Exception(string.Format("Campo {0} contiene una fecha no valida", campo.Descripcion));
                                }
                                if (campo.Requerido)
                                    if (txtFecha.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtFechaInicio = (TextBox)divControles.FindControl(nombreControl + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);
                            TextBox txtFechaFin = (TextBox)divControles.FindControl(nombreControl + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                            if (txtFechaInicio != null && txtFechaFin != null)
                            {
                                if (campo.Requerido)
                                {
                                    if (txtFechaInicio.Text.Trim() == String.Empty || txtFechaFin.Text.Trim() == string.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    try
                                    {
                                        var dI = DateTime.ParseExact(txtFechaInicio.Text.Trim(), "dd/MM/yyyy", null);
                                        var dF = DateTime.ParseExact(txtFechaFin.Text.Trim(), "dd/MM/yyyy", null);
                                        if (dI > dF)
                                            throw new Exception(
                                                string.Format("Campo {0} no es un rango de fechas valido",
                                                    campo.Descripcion));
                                    }
                                    catch
                                    {
                                        throw new Exception(string.Format("Campo {0} contiene una fecha no valida",
                                            campo.Descripcion));
                                    }
                                }
                                else
                                {
                                    if (txtFechaInicio.Text.Trim() != String.Empty && txtFechaFin.Text.Trim() != string.Empty)
                                    {
                                        try
                                        {
                                            var dI = DateTime.ParseExact(txtFechaInicio.Text.Trim(), "dd/MM/yyyy", null);
                                            var dF = DateTime.ParseExact(txtFechaFin.Text.Trim(), "dd/MM/yyyy", null);
                                            if (dI > dF)
                                                throw new Exception(string.Format("Campo {0} no es un rango de fechas valido", campo.Descripcion));
                                        }
                                        catch
                                        {
                                            throw new Exception(string.Format("Campo {0} contiene una fecha no valida", campo.Descripcion));
                                        }
                                    }
                                    else if (txtFechaInicio.Text.Trim() == String.Empty && txtFechaFin.Text.Trim() != string.Empty)
                                        throw new Exception(string.Format("Campo {0} debe capturar un rango valido", campo.Descripcion));
                                    else if (txtFechaInicio.Text.Trim() != String.Empty && txtFechaFin.Text.Trim() == string.Empty)
                                        throw new Exception(string.Format("Campo {0} debe capturar un rango valido", campo.Descripcion));
                                }
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtMascara = (TextBox)divControles.FindControl(nombreControl);
                            if (txtMascara != null)
                            {
                                if (campo.Requerido)
                                    if (txtMascara.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {
                                        if (txtMascara.Text.Trim().Length < campo.LongitudMinima)
                                            throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                        if (txtMascara.Text.Trim().Length > campo.LongitudMaxima)
                                            throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                    }
                                if (!campo.Requerido && txtMascara.Text.Trim() != string.Empty)
                                {
                                    if (txtMascara.Text.Trim().Length < campo.LongitudMinima)
                                        throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                    if (txtMascara.Text.Trim().Length > campo.LongitudMaxima)
                                        throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                }
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            if (campo.Requerido)
                            {
                                if (Session["Files"] != null)
                                {
                                    if (!((List<string>)Session["Files"]).Any())
                                    {
                                        throw new Exception(string.Format("Campo {0} debe seleccionar un archivo", campo.Descripcion));
                                    }
                                }
                                else
                                    throw new Exception(string.Format("Campo {0} debe seleccionar un archivo", campo.Descripcion));
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtTelefono = (TextBox)divControles.FindControl(nombreControl);
                            if (txtTelefono != null)
                            {
                                if (campo.Requerido)
                                    if (txtTelefono.Text.Trim() == String.Empty || txtTelefono.Text.Trim() == campo.MascaraDetalle)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {

                                        if (txtTelefono.Text.Trim().Length < campo.LongitudMinima)
                                            throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                        if (txtTelefono.Text.Trim().Length > campo.LongitudMaxima)
                                            throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                    }
                                if (!campo.Requerido && txtTelefono.Text.Trim() != string.Empty)
                                {
                                    if (txtTelefono.Text.Trim().Length < campo.LongitudMinima)
                                        throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                    if (txtTelefono.Text.Trim().Length > campo.LongitudMaxima)
                                        throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                }
                            }
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtCorreo = (TextBox)divControles.FindControl(nombreControl);
                            if (txtCorreo != null)
                            {
                                if (campo.Requerido)
                                    if (txtCorreo.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                    else
                                    {
                                        if (txtCorreo.Text.Trim().Length < campo.LongitudMinima)
                                            throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                        if (txtCorreo.Text.Trim().Length > campo.LongitudMaxima)
                                            throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                    }
                                if (!campo.Requerido && txtCorreo.Text.Trim() != string.Empty)
                                {
                                    if (txtCorreo.Text.Trim().Length < campo.LongitudMinima)
                                        throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                    if (txtCorreo.Text.Trim().Length > campo.LongitudMaxima)
                                        throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));
                                }
                                if (!BusinessCorreo.IsValidEmail(txtCorreo.Text.Trim()) || txtCorreo.Text.Trim().Contains(" "))
                                {
                                    throw new Exception(string.Format("Correo {0} con formato invalido", campo.Descripcion));
                                }
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ConfirmaArchivos(int idTicket)
        {
            try
            {

                for (int i = 0; i < ((List<string>)Session["Files"]).Count; i++)
                {
                    File.Move(BusinessVariables.Directorios.RepositorioTemporalMascara + ((List<string>)Session["Files"])[i], BusinessVariables.Directorios.RepositorioTemporalMascara + ((List<string>)Session["Files"])[i].Replace("ticketid", idTicket.ToString()));
                    ((List<string>)Session["Files"])[i] = ((List<string>)Session["Files"])[i].Replace("ticketid", idTicket.ToString());
                }
                if (Session["Files"] != null)
                {
                    BusinessFile.MoverTemporales(BusinessVariables.Directorios.RepositorioTemporalMascara, BusinessVariables.Directorios.RepositorioMascara, (List<string>)Session["Files"]);
                    Session["Files"] = null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<HelperCampoMascaraCaptura> ObtenerCapturaMascara()
        {
            List<HelperCampoMascaraCaptura> lstCamposCapturados;
            try
            {
                ValidaMascaraCaptura();
                Mascara mascara = (Mascara)Session["DataFormularioPortal"];
                string nombreControl = null;
                lstCamposCapturados = new List<HelperCampoMascaraCaptura>();
                foreach (CampoMascara campo in mascara.CampoMascara)
                {
                    bool campoTexto = true;
                    bool rango = false;
                    switch (campo.IdTipoCampoMascara)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            nombreControl = "lstRadio" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                            nombreControl = "ddl" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            nombreControl = "chklist" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            nombreControl = "chk" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.SelecciónCascada:
                            nombreControl = null;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            nombreControl = "txt" + campo.NombreCampo;
                            rango = true;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            nombreControl = "fu" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                    }

                    if (campoTexto && nombreControl != null)
                    {
                        TextBox txt = (TextBox)divControles.FindControl(nombreControl);
                        if (txt != null || rango)
                        {
                            HelperCampoMascaraCaptura campoCapturado;
                            if (rango)
                            {
                                TextBox txtFechaInicio = (TextBox)divControles.FindControl(nombreControl + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio);
                                TextBox txtFechaFin = (TextBox)divControles.FindControl(nombreControl + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin);
                                campoCapturado = new HelperCampoMascaraCaptura
                                {
                                    NombreCampo = campo.NombreCampo,
                                    Valor = txtFechaFin.Text.Trim() == string.Empty ? "" : DateTime.ParseExact(txtFechaInicio.Text.Trim().ToUpper(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") + "|" + DateTime.ParseExact(txtFechaFin.Text.Trim().ToUpper(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                };
                                lstCamposCapturados.Add(campoCapturado);
                            }
                            else
                                switch (txt.Attributes["for"])
                                {
                                    case "FECHA":
                                        campoCapturado = new HelperCampoMascaraCaptura
                                        {
                                            NombreCampo = campo.NombreCampo,
                                            Valor = txt.Text.Trim() == string.Empty ? string.Empty : DateTime.ParseExact(txt.Text.Trim().ToUpper(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd"),
                                        };
                                        lstCamposCapturados.Add(campoCapturado);
                                        break;
                                    case "FECHAINICIO":

                                        break;
                                    case "DECIMAL":
                                        campoCapturado = new HelperCampoMascaraCaptura
                                        {
                                            NombreCampo = campo.NombreCampo,
                                            Valor = campo.Requerido ? txt.Text.Trim().Replace('_', '0') : txt.Text.Trim() == string.Empty ? "0" : txt.Text.Trim().Replace('_', '0')
                                        };
                                        lstCamposCapturados.Add(campoCapturado);
                                        break;

                                    default:
                                        campoCapturado = new HelperCampoMascaraCaptura
                                        {
                                            NombreCampo = campo.NombreCampo,
                                            Valor = campo.Requerido ? txt.Text.Trim() : txt.Text.Trim() == string.Empty ? "0" : txt.Text.Trim()
                                        };
                                        lstCamposCapturados.Add(campoCapturado);
                                        break;
                                }

                        }
                    }
                    else if (!campoTexto)
                    {
                        switch (campo.IdTipoCampoMascara)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                                RadioButtonList rbtnList = (RadioButtonList)divControles.FindControl(nombreControl);
                                if (rbtnList != null)
                                {
                                    HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = rbtnList.SelectedValue
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                                DropDownList ddl = (DropDownList)divControles.FindControl(nombreControl);
                                if (ddl != null)
                                {
                                    HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = ddl.SelectedValue
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                                CheckBoxList chkList = (CheckBoxList)divControles.FindControl(nombreControl);
                                if (chkList != null)
                                {
                                    string valores = chkList.Items.Cast<ListItem>().Where(item => item.Selected).Aggregate<ListItem, string>(null, (current, item) => current + (item.Value + "|"));
                                    if (valores != null)
                                    {
                                        valores = valores.TrimEnd('|');
                                        HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                        {
                                            NombreCampo = campo.NombreCampo,
                                            Valor = valores
                                        };
                                        lstCamposCapturados.Add(campoCapturado);
                                    }
                                    else
                                    {
                                        HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                        {
                                            NombreCampo = campo.NombreCampo,
                                            Valor = string.Empty
                                        };
                                        lstCamposCapturados.Add(campoCapturado);
                                    }
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                                CheckBox chk = (CheckBox)divControles.FindControl(nombreControl);
                                if (chk != null)
                                {
                                    HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = chk.Checked.ToString()
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                                AsyncFileUpload upload = (AsyncFileUpload)divControles.FindControl(nombreControl);
                                if (upload != null)
                                {
                                    HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = Session["Files"] == null ? string.Empty : ((List<string>)Session["Files"]).First(),
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                }

                                break;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lstCamposCapturados;
        }

        public void PintaControles(List<CampoMascara> lstControles)
        {
            try
            {
                foreach (CampoMascara campo in lstControles)
                {
                    HtmlGenericControl hr = new HtmlGenericControl("HR");
                    HtmlGenericControl createDiv = new HtmlGenericControl("DIV") { ID = "createDiv" + campo.NombreCampo };
                    createDiv.Attributes["class"] = "form-group clearfix";
                    Label lbl = new Label { Text = campo.Descripcion + (campo.Requerido ? "<span style='color: red'> *</span>" : string.Empty), CssClass = "col-sm-12 control-label proxima12" };
                    switch (campo.TipoCampoMascara.Id)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:

                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtSimple = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control",

                            };
                            txtSimple.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtSimple.Attributes["MaxLength"] = campo.TipoCampoMascara.LongitudMaxima.ToString();
                            _lstControles.Add(txtSimple);
                            createDiv.Controls.Add(txtSimple);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMultilinea = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control",
                                TextMode = TextBoxMode.MultiLine,
                                Rows = 10
                            };
                            txtMultilinea.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtMultilinea.Attributes["MaxLength"] = campo.LongitudMaxima.ToString();
                            _lstControles.Add(txtMultilinea);
                            createDiv.Controls.Add(txtMultilinea);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            lbl.Attributes["for"] = "lstRadio" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            RadioButtonList lstRadio = new RadioButtonList
                            {
                                SelectedValue = "0",
                                ID = "lstRadio" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                RepeatColumns = 5,
                                RepeatDirection = RepeatDirection.Horizontal,
                            };
                            lstRadio.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            if (campo.EsArchivo)
                            {
                                foreach (DataRow row in _servicioCatalogos.ObtenerRegistrosArchivosCatalogo(int.Parse(campo.IdCatalogo.ToString())).Rows)
                                {
                                    lstRadio.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                                }
                            }
                            else
                            {
                                if (campo.IdCatalogo != null)
                                    foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, false, true))
                                    {
                                        lstRadio.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                    }
                            }
                            createDiv.Controls.Add(lstRadio);
                            _lstControles.Add(lstRadio);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                            divControles.Controls.Add(new Literal { Text = "<hr/>" });

                            lbl.Attributes["for"] = "ddl" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            DropDownList ddlCatalogo = new DropDownList
                            {
                                SelectedValue = "0",
                                ID = "ddl" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "col-sm-10 form-control"
                            };
                            ddlCatalogo.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            if (campo.EsArchivo)
                            {
                                foreach (DataRow row in _servicioCatalogos.ObtenerRegistrosArchivosCatalogo(int.Parse(campo.IdCatalogo.ToString())).Rows)
                                {
                                    ddlCatalogo.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                                }
                            }
                            else
                            {
                                if (campo.IdCatalogo != null)
                                    foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, true, true))
                                    {
                                        ddlCatalogo.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                    }
                            }
                            createDiv.Controls.Add(ddlCatalogo);
                            _lstControles.Add(ddlCatalogo);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            lbl.Attributes["for"] = "chklist" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            CheckBoxList chklist = new CheckBoxList
                            {
                                SelectedValue = "0",
                                ID = "chklist" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                RepeatColumns = 5,
                                RepeatDirection = RepeatDirection.Horizontal
                            };
                            chklist.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            if (campo.EsArchivo)
                            {
                                foreach (DataRow row in _servicioCatalogos.ObtenerRegistrosArchivosCatalogo(int.Parse(campo.IdCatalogo.ToString())).Rows)
                                {
                                    chklist.Items.Add(new ListItem(row[1].ToString(), row[0].ToString()));
                                }
                            }
                            else
                            {
                                if (campo.IdCatalogo != null)
                                    foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, false, true))
                                    {
                                        chklist.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                    }
                            }
                            createDiv.Controls.Add(chklist);
                            _lstControles.Add(chklist);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtDecimal = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtDecimal.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtDecimal.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtDecimal.Attributes["type"] = "number";
                            txtDecimal.Attributes["step"] = "0.01";
                            txtDecimal.Attributes["for"] = "DECIMAL";
                            createDiv.Controls.Add(txtDecimal);
                            _lstControles.Add(txtDecimal);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtEntero = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = campo.NombreCampo,
                                CssClass = "form-control"
                            };
                            txtEntero.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtEntero.Attributes["type"] = "number";
                            txtEntero.Attributes["step"] = "1";
                            txtEntero.Attributes["min"] = "1";
                            txtEntero.Attributes["max"] = campo.ValorMaximo.ToString();
                            createDiv.Controls.Add(txtEntero);
                            _lstControles.Add(txtEntero);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            lbl.Attributes["for"] = "FECHA";
                            createDiv.Controls.Add(lbl);
                            TextBox txtFecha = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control"
                            };
                            CalendarExtender ceeFechaOpcion = new CalendarExtender
                           {
                               ID = "cee" + campo.Descripcion,
                               TargetControlID = txtFecha.ID,
                               Format = "dd/MM/yyyy"
                           };
                            txtFecha.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtFecha.Attributes["for"] = "FECHA";
                            txtFecha.Attributes["MaxLength"] = "10";
                            createDiv.Controls.Add(ceeFechaOpcion);
                            createDiv.Controls.Add(txtFecha);
                            _lstControles.Add(txtFecha);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            lbl.Attributes["for"] = "FECHAINICIO";
                            createDiv.Controls.Add(lbl);

                            HtmlGenericControl createDivGrupoFechas = new HtmlGenericControl("DIV");
                            createDivGrupoFechas.ID = "createDivGrupoFechas";
                            createDivGrupoFechas.Attributes["class"] = "form-group";

                            Label lblDe = new Label { Text = "De:", CssClass = "" };
                            lblDe.Attributes["for"] = "FECHAINICIO";
                            createDivGrupoFechas.Controls.Add(lblDe);

                            TextBox txtFechaInicio = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio,
                                CssClass = "form-control"
                            };
                            CalendarExtender ceeFechaInicio = new CalendarExtender
                           {
                               ID = "cee" + campo.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio,
                               TargetControlID = txtFechaInicio.ID,
                               Format = "dd/MM/yyyy"
                           };
                            txtFechaInicio.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtFechaInicio.Attributes["for"] = "FECHAINICIO";
                            txtFechaInicio.Attributes["MaxLength"] = "10";
                            createDivGrupoFechas.Controls.Add(ceeFechaInicio);
                            createDivGrupoFechas.Controls.Add(txtFechaInicio);


                            Label lblHasta = new Label { Text = "De:", CssClass = "" };
                            lblHasta.Attributes["for"] = "FECHAFIN";
                            createDivGrupoFechas.Controls.Add(lblHasta);
                            TextBox txtFechaFin = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin,
                                CssClass = "form-control"
                            };
                            CalendarExtender ceeFechaFin = new CalendarExtender
                            {
                                ID = "cee" + campo.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin,
                                TargetControlID = txtFechaFin.ID,
                                Format = "dd/MM/yyyy"
                            };
                            txtFechaFin.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtFechaFin.Attributes["for"] = "FECHAFIN";
                            txtFechaFin.Attributes["MaxLength"] = "10";
                            createDivGrupoFechas.Controls.Add(ceeFechaFin);
                            createDivGrupoFechas.Controls.Add(txtFechaFin);

                            HtmlGenericControl createDivFormFechas = new HtmlGenericControl("DIV");
                            createDivFormFechas.ID = "createDivFormFechas";
                            createDivFormFechas.Attributes["class"] = "form-inline";
                            createDivFormFechas.Controls.Add(createDivGrupoFechas);
                            createDiv.Controls.Add(createDivFormFechas);

                            _lstControles.Add(txtFechaInicio);
                            _lstControles.Add(txtFechaFin);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            CheckBox chk = new CheckBox { ID = "chk" + campo.NombreCampo, Text = campo.Descripcion, ViewStateMode = ViewStateMode.Inherit };
                            _lstControles.Add(chk);
                            createDiv.Controls.Add(chk);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMascara = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtMascara.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtMascara.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtMascara.Attributes["for"] = "txt" + campo.Descripcion.Replace(" ", string.Empty);
                            MaskedEditExtender meeMascara = new MaskedEditExtender
                            {
                                ID = "mee" + campo.NombreCampo,
                                TargetControlID = txtMascara.ID,
                                InputDirection = MaskedEditInputDirection.LeftToRight,
                                Mask = campo.MascaraDetalle,
                                MaskType = MaskedEditType.Date,
                                AcceptAMPM = false,
                                AcceptNegative = MaskedEditShowSymbol.None,
                            };
                            createDiv.Controls.Add(meeMascara);
                            createDiv.Controls.Add(txtMascara);
                            _lstControles.Add(txtMascara);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            divControles.Controls.Add(hr);
                            lbl.Text = string.Format("{0} (max {1}MB). {2}", campo.Descripcion, TamañoArchivo, campo.Requerido ? "<span style='color: red'> *</span>" : string.Empty);
                            lbl.Attributes["for"] = "fu" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            AsyncFileUpload asyncFileUpload = new AsyncFileUpload
                            {
                                ID = "fu" + campo.NombreCampo,
                                PersistFile = true,
                                UploaderStyle = AsyncFileUploaderStyle.Traditional,
                                OnClientUploadStarted = "ShowLanding",
                                OnClientUploadComplete = "HideLanding"
                            };
                            asyncFileUpload.OnClientUploadStarted = "UploadStart";
                            asyncFileUpload.OnClientUploadError = "uploadError";
                            asyncFileUpload.Attributes["style"] = "margin-top: 25px";
                            asyncFileUpload.UploadedComplete += asyncFileUpload_UploadedComplete;
                            createDiv.Controls.Add(asyncFileUpload);
                            _lstControles.Add(asyncFileUpload);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Telefono:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMascaraTelefono = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "form-control",
                                EnableViewState = true,

                            };
                            txtMascaraTelefono.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            MaskedEditExtender meeMascaraTelefono = new MaskedEditExtender
                            {
                                ID = "mee" + campo.NombreCampo,
                                Mask = campo.MascaraDetalle,
                                TargetControlID = txtMascaraTelefono.ID,
                                ClearMaskOnLostFocus = true,
                                ClearTextOnInvalid = true,
                            };

                            createDiv.Controls.Add(txtMascaraTelefono);
                            createDiv.Controls.Add(meeMascaraTelefono);
                            _lstControles.Add(txtMascaraTelefono);
                            _lstControles.Add(meeMascaraTelefono);
                            break;

                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CorreoElectronico:
                            lbl.Attributes["for"] = "txt" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            TextBox txtCorreo = new TextBox
                            {
                                ID = "txt" + campo.Descripcion,
                                CssClass = "form-control",
                            };
                            txtCorreo.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtCorreo.Attributes["MaxLength"] = campo.TipoCampoMascara.LongitudMaximaPermitida;
                            txtCorreo.Attributes["type"] = "email";
                            _lstControles.Add(txtCorreo);
                            createDiv.Controls.Add(txtCorreo);
                            break;
                    }

                    divControles.Controls.Add(createDiv);
                }
                upMascara.Update();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        void asyncFileUpload_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            try
            {
                if (TamañoArchivo > 0)
                {
                    if ((double.Parse(e.FileSize) / 1024) > (10 * 1024))
                    {
                        Response.Write("Size is limited to 2MB");
                        throw new Exception(string.Format("El tamaño maximo de carga es de {0}MB", "10"));
                    }
                }


                List<string> lstArchivo = Session["Files"] == null ? new List<string>() : (List<string>)Session["Files"];
                if (lstArchivo.Any(archivosCargados => archivosCargados.Split('_')[0] == e.FileName.Split('\\').Last()))
                    return;
                string extension = Path.GetExtension(e.FileName.Split('\\').Last());
                if (extension == null) return;
                string filename = string.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}{8}", e.FileName.Split('\\').Last().Replace(extension, string.Empty), "ticketid", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, extension);
                AsyncFileUpload uploadControl = (AsyncFileUpload)sender;
                if (!Directory.Exists(BusinessVariables.Directorios.RepositorioTemporalMascara))
                    Directory.CreateDirectory(BusinessVariables.Directorios.RepositorioTemporalMascara);
                uploadControl.SaveAs(BusinessVariables.Directorios.RepositorioTemporalMascara + filename);
                lstArchivo.Add(filename);
                Session["Files"] = lstArchivo;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
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

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (!ValidCaptcha)
                {
                    txtCaptcha.Text = string.Empty;
                    throw new Exception("Captcha incorrecto");
                }
                List<HelperCampoMascaraCaptura> capturaMascara = ObtenerCapturaMascara();
                if (ucAltaUsuarioRapida.Visible)
                    ucAltaUsuarioRapida.RegistraUsuario();
                int idUsuario = ucAltaUsuarioRapida.Visible ? ucAltaUsuarioRapida.IdUsuario : ((Usuario)Session["UserData"]).Id;
                Usuario user = _servicioUsuariosClient.ObtenerDetalleUsuario(idUsuario);
                if (user != null)
                {
                    KiiniNet.Entities.Operacion.Tickets.Ticket result = _servicioTicket.CrearTicket(user.Id, user.Id,
                        int.Parse(_serviciosParametros.ObtenerParametrosGenerales().FormularioPortal), capturaMascara,
                        (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Portal, CampoRandom, true, false);
                    hfTicketGenerado.Value = result.Id.ToString();
                    if (CampoRandom)
                        hfRandomGenerado.Value = result.ClaveRegistro;
                    if (Session["Files"] != null)
                        ConfirmaArchivos(result.Id);
                    if (OnAceptarModal != null)
                        OnAceptarModal();
                    Limpiar();
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
            finally
            {
                btnGuardar.Enabled = true;
            }
        }

        public void Limpiar()
        {
            try
            {
                Session["Files"] = null;
                _lstControles = new List<Control>();
                divControles.Controls.Clear();
                txtCaptcha.Text = string.Empty;
                OnInit(new EventArgs());

            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void OnServerValidate(object source, ServerValidateEventArgs e)
        {
            try
            {
                if (txtCaptcha.Text.Trim() == string.Empty) return;
                captchaTicket.ValidateCaptcha(txtCaptcha.Text.Trim());
                e.IsValid = captchaTicket.UserValidated;
                ValidCaptcha = e.IsValid;
                if (!e.IsValid)
                {
                    throw new Exception("Captcha incorrecto.");
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                    _lstError.Add(ex.Message);
                }

                Alerta = _lstError;
            }
        }
    }
}