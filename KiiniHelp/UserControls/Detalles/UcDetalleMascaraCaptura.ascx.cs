using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniHelp.ServiceTicket;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;
using RepeatDirection = System.Web.UI.WebControls.RepeatDirection;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleMascaraCaptura : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private readonly ServiceCatalogosClient _servicioCatalogos = new ServiceCatalogosClient();
        private readonly ServiceTicketClient _servicioTicket = new ServiceTicketClient();
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    Control myControl = GetPostBackControl(Page);

        //    if ((myControl != null))
        //    {
        //        if ((myControl.ClientID == "btnAddTextBox"))
        //        {

        //        }
        //    }
        //}
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    _lstControles = new List<Control>();
        //    int? idMascara = ((FrmTest)Page).IdMascara;
        //    if (idMascara != null) IdMascara = (int)idMascara;
        //    int? idticket = ((FrmTest)Page).IdTicket;
        //    if (idticket != null) IdTicket = (int)idticket;
        //    Mascara mascara = _servicioMascaras.ObtenerMascaraCaptura(IdMascara);
        //    if (mascara != null)
        //    {
        //        lblDescripcionMascara.Text = mascara.Descripcion;
        //        PintaControles(mascara.CampoMascara, _servicioMascaras.ObtenerDatosMascara(IdMascara, IdTicket));
        //    }
        //}
        public void CargarDatos()
        {
            try
            {
                Mascara mascara = _servicioMascaras.ObtenerMascaraCapturaByIdTicket(IdTicket);
                if (mascara != null)
                {
                    lblDescripcionMascara.Text = mascara.Descripcion;
                    PintaControles(mascara.CampoMascara, _servicioMascaras.ObtenerDatosMascara(mascara.Id, IdTicket));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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


        public int IdTicket
        {
            get { return Convert.ToInt32(hfIdTicket.Value); }
            set
            {
                hfIdTicket.Value = value.ToString();
                CargarDatos();
            }
        }

        public void PintaControles(List<CampoMascara> lstControles, List<HelperMascaraData> datosMascara)
        {
            try
            {
                if (datosMascara == null)
                    throw new Exception("Ticket con informacion incorrecta.");
                divControles.Controls.Clear();
                foreach (CampoMascara campo in lstControles)
                {
                    HtmlGenericControl hr = new HtmlGenericControl("HR");
                    HtmlGenericControl createDiv = new HtmlGenericControl("DIV") { ID = "createDiv" + campo.NombreCampo };
                    createDiv.Attributes["class"] = "form-group col-lg-12 col-md-12 col-sm-12 col-xs-12  tableHeadTicket clearfix";
                    //createDiv.InnerHtml = campo.Descripcion;
                    Label lbl = new Label { Text = campo.Descripcion + (campo.Requerido ? "<span style='color: red'> *</span>" : string.Empty), CssClass = "col-lg-12 col-md-12 col-sm-12 col-xs-12 proxima12" };
                    switch (campo.TipoCampoMascara.Id)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtAlfanumerico = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "col-sm-6 form-control",
                                Text = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value,
                                ReadOnly = true
                            };
                            HtmlGenericControl createDivTexto = new HtmlGenericControl("DIV");
                            createDivTexto.ID = "createDivTexto" + campo.NombreCampo;
                            createDivTexto.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12 col-xs-12";
                            createDivTexto.Controls.Add(txtAlfanumerico);
                            createDiv.Controls.Add(createDivTexto);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMultilinea = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control",
                                TextMode = TextBoxMode.MultiLine,
                                Text = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value,
                                Rows = 10,
                                ReadOnly = true
                            };
                            txtMultilinea.Attributes["MaxLength"] = campo.LongitudMaxima.ToString();
                            txtMultilinea.Attributes["placeholder"] = campo.Descripcion;
                            HtmlGenericControl createDivMultilinea = new HtmlGenericControl("DIV");
                            createDivMultilinea.ID = "createDivMultilinea" + campo.NombreCampo;
                            createDivMultilinea.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12 col-xs-12";
                            createDivMultilinea.Controls.Add(txtMultilinea);
                            createDiv.Controls.Add(createDivMultilinea);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            lbl.Attributes["for"] = "lstRadio" + campo.NombreCampo;
                            createDiv.Attributes.Add("class", "tableHeadMascara");
                            createDiv.Controls.Add(lbl);
                            RadioButtonList lstRadio = new RadioButtonList
                            {
                                ID = "lstRadio" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                RepeatColumns = 5,
                                RepeatDirection = RepeatDirection.Horizontal,
                                Enabled = false
                            };
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
                                    foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, false, false))
                                    {
                                        lstRadio.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                    }
                            }
                            lstRadio.SelectedValue = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value;
                            HtmlGenericControl createDivRadio = new HtmlGenericControl("DIV");
                            createDivRadio.ID = "createDivRadio" + campo.NombreCampo;
                            createDivRadio.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12 col-xs-12";
                            createDivRadio.Controls.Add(lstRadio);

                            createDiv.Controls.Add(createDivRadio);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable:
                            lbl.Attributes["for"] = "ddl" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            DropDownList ddlCatalogo = new DropDownList
                            {
                                SelectedValue = "0",
                                ID = "ddl" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "col-sm-10 form-control",
                                Enabled = false
                            };
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
                                    foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, true, false))
                                    {
                                        ddlCatalogo.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                    }
                            }
                            ddlCatalogo.SelectedValue = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value;
                            HtmlGenericControl createDivDdl = new HtmlGenericControl("DIV");
                            createDivDdl.ID = "createDivDdl" + campo.NombreCampo;
                            createDivDdl.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                            createDivDdl.Controls.Add(ddlCatalogo);
                            createDiv.Controls.Add(createDivDdl);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación:
                            lbl.Attributes["for"] = "chklist" + campo.NombreCampo;
                            createDiv.Attributes.Add("class", "tableHeadMascara");
                            createDiv.Controls.Add(lbl);
                            CheckBoxList chklist = new CheckBoxList
                            {
                                SelectedValue = "0",
                                ID = "chklist" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                RepeatColumns = 5,
                                RepeatDirection = RepeatDirection.Horizontal,
                                Enabled = false,
                            };
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
                                    foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, false, false))
                                    {
                                        chklist.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                    }
                            }
                            List<int> values = _servicioTicket.CapturaCasillaTicket(IdTicket, campo.NombreCampo);
                            foreach (ListItem item in chklist.Items)
                            {
                                foreach (int value in values)
                                {
                                    if (item.Value == value.ToString())
                                    {
                                        item.Selected = true;
                                        break;
                                    }
                                }
                            }
                            HtmlGenericControl createDivchk = new HtmlGenericControl("DIV");
                            createDivchk.ID = "createDivchk" + campo.NombreCampo;
                            createDivchk.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                            createDivchk.Controls.Add(chklist);
                            createDiv.Controls.Add(createDivchk);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroDecimal:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtDecimal = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value,
                                CssClass = "form-control",
                                ReadOnly = true
                            };
                            txtDecimal.Attributes["placeholder"] = campo.Descripcion;
                            txtDecimal.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtDecimal.Attributes["type"] = "number";
                            txtDecimal.Attributes["step"] = "0.01";
                            txtDecimal.Attributes["for"] = "DECIMAL";
                            createDiv.Controls.Add(txtDecimal);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtEntero = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value,
                                CssClass = "form-control",
                                ReadOnly = true
                            };
                            txtEntero.Attributes["placeholder"] = campo.NombreCampo;
                            txtEntero.Attributes["type"] = "number";
                            txtEntero.Attributes["step"] = "1";
                            txtEntero.Attributes["min"] = "1";
                            txtEntero.Attributes["max"] = campo.ValorMaximo.ToString();
                            HtmlGenericControl createDivEntero = new HtmlGenericControl("DIV");
                            createDivEntero.ID = "createDivEntero" + campo.NombreCampo;
                            createDivEntero.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                            createDivEntero.Controls.Add(txtEntero);

                            createDiv.Controls.Add(createDivEntero);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Fecha:
                            lbl.Attributes["for"] = "FECHA";
                            createDiv.Controls.Add(lbl);
                            TextBox txtFecha = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control",
                                Text = DateTime.Parse(datosMascara.Single(s => s.Campo == campo.NombreCampo).Value).ToString("yyyy-MM-dd"),
                                ReadOnly = true
                            };
                            txtFecha.Attributes["placeholder"] = campo.Descripcion;
                            txtFecha.Attributes["for"] = "FECHA";
                            txtFecha.Attributes["type"] = "date";
                            txtFecha.Attributes["step"] = "1";
                            HtmlGenericControl createDivFecha = new HtmlGenericControl("DIV");
                            createDivFecha.ID = "createDivEntero" + campo.NombreCampo;
                            createDivFecha.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                            createDivFecha.Controls.Add(txtFecha);
                            createDiv.Controls.Add(createDivFecha);
                            break;

                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango:
                            lbl.Attributes["for"] = "FECHAINICIO";
                            createDiv.Controls.Add(lbl);

                            HtmlGenericControl createDivGrupoFechas = new HtmlGenericControl("DIV");
                            createDivGrupoFechas.ID = "createDivGrupoFechas" + campo.NombreCampo;
                            createDivGrupoFechas.Attributes["class"] = "form-group";

                            Label lblDe = new Label { Text = "De:", CssClass = "" };
                            lblDe.Attributes["for"] = "FECHAINICIO";
                            createDivGrupoFechas.Controls.Add(lblDe);
                            string nombreCampo = campo.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio;
                            TextBox txtFechaInicio = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio,
                                Text = DateTime.Parse(datosMascara.Single(s => s.Campo == nombreCampo).Value).ToString("yyyy-MM-dd"),
                                CssClass = "form-control",
                                ReadOnly = true
                            };
                            txtFechaInicio.Attributes["placeholder"] = campo.Descripcion;
                            txtFechaInicio.Attributes["for"] = "FECHAINICIO";
                            txtFechaInicio.Attributes["type"] = "date";
                            txtFechaInicio.Attributes["step"] = "1";
                            createDivGrupoFechas.Controls.Add(txtFechaInicio);


                            Label lblHasta = new Label { Text = "De:", CssClass = "" };
                            lblHasta.Attributes["for"] = "FECHAFIN";
                            createDivGrupoFechas.Controls.Add(lblHasta);
                            nombreCampo = campo.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin;
                            TextBox txtFechaFin = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin,
                                Text = DateTime.Parse(datosMascara.Single(s => s.Campo == nombreCampo).Value).ToString("yyyy-MM-dd"),
                                CssClass = "form-control",
                                ReadOnly = true
                            };
                            txtFechaFin.Attributes["placeholder"] = campo.Descripcion;
                            txtFechaFin.Attributes["for"] = "FECHAFIN";
                            txtFechaFin.Attributes["type"] = "date";
                            txtFechaFin.Attributes["step"] = "1";
                            createDivGrupoFechas.Controls.Add(txtFechaFin);

                            HtmlGenericControl createDivFormFechas = new HtmlGenericControl("DIV");
                            createDivFormFechas.Attributes["class"] = "form-inline";
                            createDivFormFechas.Controls.Add(createDivGrupoFechas);
                            createDiv.Controls.Add(createDivFormFechas);
                            break;

                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            CheckBox chk = new CheckBox { ID = "chk" + campo.NombreCampo, Text = campo.Descripcion, ViewStateMode = ViewStateMode.Inherit, Enabled = false };
                            HtmlGenericControl createDivCheck = new HtmlGenericControl("DIV");
                            createDivCheck.ID = "createDivCheck" + campo.NombreCampo;
                            createDivCheck.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                            createDivCheck.Controls.Add(chk);
                            createDiv.Controls.Add(createDivCheck);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMascara = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "form-control",
                                ReadOnly = true
                            };
                            //txtMascara.Attributes["placeholder"] = campo.Descripcion;
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
                            HtmlGenericControl createDivMask = new HtmlGenericControl("DIV");
                            createDivMask.ID = "createDivMask" + campo.NombreCampo;
                            createDivMask.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                            createDivMask.Controls.Add(meeMascara);
                            createDivMask.Controls.Add(txtMascara);

                            createDiv.Controls.Add(createDivMask);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo:
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            string archivo = datosMascara.Single(s => s.Campo == campo.NombreCampo).Value;
                            HyperLink lk = new HyperLink();
                            if (archivo != string.Empty)
                            {
                                lk.Text = archivo;
                                lk.NavigateUrl = ResolveUrl(string.Format("~/Downloads/FrmDownloads.aspx?file={0}", BusinessVariables.Directorios.RepositorioMascara + "~" + archivo));
                                lk.Style.Add("margin-auto", "10px");
                                HtmlGenericControl createDivFile = new HtmlGenericControl("DIV");
                                createDivFile.ID = "createDivFile" + campo.NombreCampo;
                                createDivFile.Attributes["class"] = "col-lg-12 col-md-12 col-sm-12";
                                createDivFile.Controls.Add(lk);
                                createDiv.Controls.Add(createDivFile);
                            }
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Response.Clear();
                //Response.ContentType = "text/csv";
                //Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", ""));
                //Response.WriteFile("ruta archivo");
                //Response.End();
            }
        }
    }
}