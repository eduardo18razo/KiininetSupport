using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;
using RepeatDirection = System.Web.UI.WebControls.RepeatDirection;

namespace KiiniHelp.UserControls.Altas.Formularios
{
    public partial class UcPreviewFormulario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceCatalogosClient _servicioCatalogos = new ServiceCatalogosClient();
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private List<Control> _lstControles;
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

        protected void Page_PreInit(object sender, EventArgs e)
        {
            Control myControl = GetPostBackControl(Page);

            if ((myControl != null))
            {
                if ((myControl.ClientID == "btnAddTextBox"))
                {

                }
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _lstControles = new List<Control>();
            Mascara mascara = (Mascara)Session["PreviewDataFormulario"];
            if (mascara != null)
            {
                hfComandoInsertar.Value = mascara.ComandoInsertar;
                hfComandoActualizar.Value = mascara.ComandoInsertar;
                hfRandom.Value = mascara.Random.ToString();
                lblDescripcionMascara.Text = mascara.Descripcion;
                PintaControles(mascara.CampoMascara);
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

        public bool Preview
        {
            get { return Convert.ToBoolean(hfPreview.Value); }
            set
            {
                hfPreview.Value = value.ToString();
            }
        }

        public void PintaControles(List<CampoMascara> lstControles)
        {
            try
            {

                foreach (CampoMascara campo in lstControles)
                {
                    HtmlGenericControl createDiv = new HtmlGenericControl("DIV") ;
                    createDiv.Attributes["class"] = "form-group clearfix";
                    //createDiv.InnerHtml = campo.Descripcion;
                    Label lbl = new Label { Text = campo.Descripcion, CssClass = "col-sm-12 control-label" };
                    switch (campo.TipoCampoMascara.Id)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto:
                            lbl.Attributes["for"] = "txt" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            TextBox txtSimple = new TextBox
                            {
                                ID = "txt" + campo.Descripcion,
                                CssClass = "form-control",

                            };
                            txtSimple.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtSimple.Attributes["MaxLength"] = campo.TipoCampoMascara.LongitudMaxima.ToString();
                            //txtSimple.Attributes["placeholder"] = campo.Descripcion; ecl
                            _lstControles.Add(txtSimple);
                            createDiv.Controls.Add(txtSimple);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.TextoMultiLinea:
                            lbl.Attributes["for"] = "txt" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMultilinea = new TextBox
                            {
                                ID = "txt" + campo.Descripcion,
                                CssClass = "form-control",
                                TextMode = TextBoxMode.MultiLine,
                                Rows = 10
                            };
                            txtMultilinea.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtMultilinea.Attributes["MaxLength"] = campo.LongitudMaxima.ToString();
                            //txtMultilinea.Attributes["placeholder"] = campo.Descripcion; ecl
                            _lstControles.Add(txtMultilinea);
                            createDiv.Controls.Add(txtMultilinea);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton:
                            lbl.Attributes["for"] = "lstRadio" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            RadioButtonList lstRadio = new RadioButtonList
                            {
                                SelectedValue = "0",
                                ID = "lstRadio" + campo.Descripcion,
                                Text = campo.Descripcion,
                                RepeatColumns = 5,
                                RepeatDirection = RepeatDirection.Horizontal
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
                            lbl.Attributes["for"] = "ddl" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            DropDownList ddlCatalogo = new DropDownList
                            {
                                SelectedValue = "0",
                                ID = "ddl" + campo.Descripcion,
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
                            lbl.Attributes["for"] = "chklist" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            CheckBoxList chklist = new CheckBoxList
                            {
                                SelectedValue = "0",
                                ID = "chklist" + campo.Descripcion,
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
                            lbl.Attributes["for"] = "txt" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            TextBox txtDecimal = new TextBox
                            {
                                ID = "txt" + campo.Descripcion,
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtDecimal.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            //txtDecimal.Attributes["placeholder"] = campo.Descripcion;  ecl
                            txtDecimal.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtDecimal.Attributes["type"] = "number";
                            txtDecimal.Attributes["step"] = "0.01";
                            txtDecimal.Attributes["for"] = "DECIMAL";
                            createDiv.Controls.Add(txtDecimal);
                            _lstControles.Add(txtDecimal);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.NúmeroEntero:
                            lbl.Attributes["for"] = "txt" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            TextBox txtEntero = new TextBox
                            {
                                ID = "txt" + campo.Descripcion,
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtEntero.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            //txtEntero.Attributes["placeholder"] = campo.Descripcion; ecl
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
                                ID = "txt" + campo.Descripcion,
                                CssClass = "form-control"
                            };
                            CalendarExtender ceeFechaOpcion = new CalendarExtender
                            {
                                ID = "cee" + campo.Descripcion,
                                TargetControlID = txtFecha.ID,
                                Format = "dd/MM/yyyy"
                            };
                            //txtFecha.Attributes["placeholder"] = campo.Descripcion; ecl
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
                            createDivGrupoFechas.Attributes["class"] = "form-group";

                            Label lblDe = new Label { Text = "De: ", CssClass = "" };
                            lblDe.Attributes["for"] = "FECHAINICIO";
                            createDivGrupoFechas.Controls.Add(lblDe);

                            TextBox txtFechaInicio = new TextBox
                            {
                                ID = "txt" + campo.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio,
                                CssClass = "form-control"
                            };
                            CalendarExtender ceeFechaInicio = new CalendarExtender
                            {
                                ID = "cee" + campo.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaInicio,
                                TargetControlID = txtFechaInicio.ID,
                                Format = "dd/MM/yyyy"
                            };
                            //txtFechaInicio.Attributes["placeholder"] = campo.Descripcion; ecl
                            txtFechaInicio.Attributes["for"] = "FECHAINICIO";
                            txtFechaInicio.Attributes["MaxLength"] = "10";
                            createDivGrupoFechas.Controls.Add(ceeFechaInicio);
                            createDivGrupoFechas.Controls.Add(txtFechaInicio);


                            Label lblHasta = new Label { Text = "  A: ", CssClass = "" };
                            lblHasta.Attributes["for"] = "FECHAFIN";
                            createDivGrupoFechas.Controls.Add(lblHasta);
                            TextBox txtFechaFin = new TextBox
                            {
                                ID = "txt" + campo.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin,
                                CssClass = "form-control"
                            };
                            CalendarExtender ceeFechaFin = new CalendarExtender
                            {
                                ID = "cee" + campo.Descripcion + BusinessVariables.ParametrosMascaraCaptura.PrefijoFechaFin,
                                TargetControlID = txtFechaFin.ID,
                                Format = "dd/MM/yyyy"
                            };
                            //txtFechaFin.Attributes["placeholder"] = campo.Descripcion; ecl
                            txtFechaFin.Attributes["for"] = "FECHAFIN";
                            txtFechaFin.Attributes["MaxLength"] = "10";
                            createDivGrupoFechas.Controls.Add(ceeFechaFin);
                            createDivGrupoFechas.Controls.Add(txtFechaFin);

                            HtmlGenericControl createDivFormFechas = new HtmlGenericControl("DIV");
                            createDivFormFechas.Attributes["class"] = "form-inline";
                            createDivFormFechas.Controls.Add(createDivGrupoFechas);
                            createDiv.Controls.Add(createDivFormFechas);

                            _lstControles.Add(txtFechaInicio);
                            _lstControles.Add(txtFechaFin);
                            break;

                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico:
                            CheckBox chk = new CheckBox { ID = "chk" + campo.Descripcion, Text = campo.Descripcion, ViewStateMode = ViewStateMode.Inherit };
                            _lstControles.Add(chk);
                            createDiv.Controls.Add(chk);
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ExpresiónRegular:
                            lbl.Attributes["for"] = "txt" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMascara = new TextBox
                            {
                                ID = "txt" + campo.Descripcion,
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            //txtMascara.Attributes["placeholder"] = campo.Descripcion;
                            txtMascara.Attributes.Add("onkeydown", "return (event.keyCode!=13 && event.keyCode!=27);");
                            txtMascara.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtMascara.Attributes["for"] = "txt" + campo.Descripcion.Replace(" ", string.Empty);
                            MaskedEditExtender meeMascara = new MaskedEditExtender
                            {
                                ID = "mee" + campo.Descripcion,
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
                            lbl.Attributes["for"] = "fu" + campo.Descripcion;
                            createDiv.Controls.Add(lbl);
                            AsyncFileUpload asyncFileUpload = new AsyncFileUpload
                            {
                                ID = "fu" + campo.Descripcion,
                                PersistFile = true,
                                UploaderStyle = AsyncFileUploaderStyle.Traditional,
                                OnClientUploadStarted = "ShowLanding",
                                OnClientUploadComplete = "HideLanding"

                            };
                            //Session[asyncFileUpload.ID] = string.Empty;
                            asyncFileUpload.Attributes["style"] = "margin-top: 25px";
                            asyncFileUpload.UploadedComplete += asyncFileUpload_UploadedComplete;
                            createDiv.Controls.Add(asyncFileUpload);
                            _lstControles.Add(asyncFileUpload);
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
                //Session[uploadControl.ID] = filename;
                lstArchivo.Add(filename);
                //Session["Files"] = lstArchivo;
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