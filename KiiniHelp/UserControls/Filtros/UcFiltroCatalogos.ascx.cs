using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniHelp.Test;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Filtros
{
    public partial class UcFiltroCatalogos : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
        private readonly ServiceCatalogosClient _servicioCatalogo = new ServiceCatalogosClient();
        readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private List<Control> _lstControles;
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
            int? idCatalogo = ((FrmTest)Page).IdCatalogo;
            if (idCatalogo != null) IdCatalogo = (int)idCatalogo;
            Mascara mascara = _servicioMascaras.ObtenerMascaraCaptura(IdCatalogo);
            if (mascara != null)
            {
                hfComandoInsertar.Value = mascara.ComandoInsertar;
                hfComandoActualizar.Value = mascara.ComandoInsertar;
                hfRandom.Value = mascara.Random.ToString();
                lblDescCatalogo.Text = mascara.Descripcion;
                PintaControles(mascara.CampoMascara);
                Session["MascaraActiva"] = mascara;
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

        public int IdCatalogo
        {
            get { return Convert.ToInt32(hfIdCatalogo.Value); }
            set { hfIdCatalogo.Value = value.ToString(); }
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

        public List<HelperCampoMascaraCaptura> ObtenerCapturaMascara()
        {
            List<HelperCampoMascaraCaptura> lstCamposCapturados;
            try
            {
                ValidaMascaraCaptura();
                Mascara mascara = (Mascara)Session["MascaraActiva"];
                string nombreControl = null;
                lstCamposCapturados = new List<HelperCampoMascaraCaptura>();
                foreach (CampoMascara campo in mascara.CampoMascara)
                {
                    bool campoTexto = true;
                    switch (campo.TipoCampoMascara.Descripcion)
                    {
                        case "ALFANUMERICO":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "DECIMAL":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "ENTERO":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "FECHA":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "HORA":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "MONEDA":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "CATALOGO":
                            nombreControl = "ddl" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case "SI/NO":
                            nombreControl = "chk" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                        case "MASCARA":
                            nombreControl = "txt" + campo.NombreCampo;
                            break;
                        case "CARGA DE ARCHIVO":
                            nombreControl = "fu" + campo.NombreCampo;
                            campoTexto = false;
                            break;
                    }

                    if (campoTexto && nombreControl != null)
                    {
                        TextBox txt = (TextBox)divControles.FindControl(nombreControl);
                        if (txt != null)
                        {
                            HelperCampoMascaraCaptura campoCapturado;
                            switch (txt.Attributes["for"])
                            {
                                case "FECHA":
                                    campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = Convert.ToDateTime(txt.Text.Trim().ToUpper()).ToString("yyyy-MM-dd"),
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                    break;
                                case "DECIMAL":
                                    campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = txt.Text.Trim().Replace('_', '0')
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                    break;

                                default:
                                    campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = txt.Text.Trim().ToUpper()
                                    };
                                    lstCamposCapturados.Add(campoCapturado);
                                    break;
                            }

                        }
                    }
                    else if (!campoTexto)
                    {
                        switch (campo.TipoCampoMascara.Descripcion)
                        {
                            case "CATALOGO":
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
                            case "SI/NO":
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
                            case "CARGA DE ARCHIVO":
                                AsyncFileUpload upload = (AsyncFileUpload)divControles.FindControl(nombreControl);
                                if (upload != null)
                                {
                                    HelperCampoMascaraCaptura campoCapturado = new HelperCampoMascaraCaptura
                                    {
                                        NombreCampo = campo.NombreCampo,
                                        Valor = Session[nombreControl] == null ? string.Empty : Session[nombreControl].ToString(),
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
                    HtmlGenericControl createDiv = new HtmlGenericControl("DIV") { ID = "createDiv" + campo.NombreCampo };
                    createDiv.Attributes["class"] = "form-group";
                    //createDiv.InnerHtml = campo.Descripcion;
                    Label lbl = new Label { Text = campo.Descripcion, CssClass = "control-label" };
                    switch (campo.TipoCampoMascara.Descripcion)
                    {
                        case "ALFANUMERICO":

                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtAlfanumerico = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control",

                            };
                            txtAlfanumerico.Attributes["MaxLength"] = campo.LongitudMaxima.ToString();
                            txtAlfanumerico.Attributes["placeholder"] = campo.Descripcion;
                            _lstControles.Add(txtAlfanumerico);
                            createDiv.Controls.Add(txtAlfanumerico);
                            break;
                        case "CATALOGO":
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            DropDownList ddlCatalogo = new DropDownList
                            {
                                ID = "ddl" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "DropSelect"
                            };
                            if (campo.IdCatalogo != null)
                                foreach (CatalogoGenerico cat in _servicioMascaras.ObtenerCatalogoCampoMascara((int)campo.IdCatalogo, true, false))
                                {
                                    ddlCatalogo.Items.Add(new ListItem(cat.Descripcion, cat.Id.ToString()));
                                }
                            createDiv.Controls.Add(ddlCatalogo);
                            _lstControles.Add(ddlCatalogo);
                            break;
                        case "DECIMAL":
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtDecimal = new TextBox
                            {
                                ID = "txt" + campo.Descripcion.Replace(" ", string.Empty),
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtDecimal.Attributes["placeholder"] = campo.Descripcion;
                            txtDecimal.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtDecimal.Attributes["type"] = "number";
                            txtDecimal.Attributes["step"] = "0.01";
                            txtDecimal.Attributes["for"] = "DECIMAL";
                            createDiv.Controls.Add(txtDecimal);
                            _lstControles.Add(txtDecimal);
                            break;
                        case "ENTERO":
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtEntero = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtEntero.Attributes["placeholder"] = campo.Descripcion;
                            txtEntero.Attributes["type"] = "number";
                            txtEntero.Attributes["step"] = "1";
                            txtEntero.Attributes["min"] = "1";
                            txtEntero.Attributes["max"] = campo.ValorMaximo.ToString();
                            createDiv.Controls.Add(txtEntero);
                            _lstControles.Add(txtEntero);
                            break;
                        case "FECHA":
                            lbl.Attributes["for"] = "FECHA";
                            createDiv.Controls.Add(lbl);
                            TextBox txtFecha = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control"
                            };
                            txtFecha.Attributes["placeholder"] = campo.Descripcion;
                            txtFecha.Attributes["for"] = "FECHA";
                            txtFecha.Attributes["type"] = "date";
                            txtFecha.Attributes["step"] = "1";
                            createDiv.Controls.Add(txtFecha);
                            _lstControles.Add(txtFecha);
                            break;
                        case "HORA":
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtHora = new TextBox
                            {
                                ID = "txt" + campo.NombreCampo,
                                CssClass = "form-control"
                            };
                            txtHora.Attributes["placeholder"] = campo.Descripcion;
                            txtHora.Attributes["min"] = "00:00";
                            txtHora.Attributes["max"] = "23:59:59";
                            txtHora.Attributes["step"] = "30";
                            txtHora.Attributes["type"] = "time";
                            createDiv.Controls.Add(txtHora);
                            _lstControles.Add(txtHora);
                            break;
                        case "MONEDA":
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMoneda = new TextBox
                            {
                                ID = "txt" + campo.Descripcion.Replace(" ", string.Empty),
                                CssClass = "form-control"
                            };
                            txtMoneda.Attributes["placeholder"] = campo.Descripcion;
                            txtMoneda.Attributes["type"] = "number";
                            txtMoneda.Attributes["step"] = "0.01";
                            _lstControles.Add(txtMoneda);
                            createDiv.Controls.Add(txtMoneda);
                            break;
                        case "SI/NO":
                            CheckBox chk = new CheckBox { ID = "chk" + campo.NombreCampo, Text = campo.Descripcion, ViewStateMode = ViewStateMode.Inherit };
                            _lstControles.Add(chk);
                            createDiv.Controls.Add(chk);
                            break;
                        case "MASCARA":
                            lbl.Attributes["for"] = "txt" + campo.NombreCampo;
                            createDiv.Controls.Add(lbl);
                            TextBox txtMascara = new TextBox
                            {
                                ID = "txt" + campo.Descripcion.Replace(" ", string.Empty),
                                Text = campo.Descripcion,
                                CssClass = "form-control"
                            };
                            txtMascara.Attributes["placeholder"] = campo.Descripcion;
                            txtMascara.Attributes["max"] = campo.ValorMaximo.ToString();
                            txtMascara.Attributes["for"] = "MASCARA";
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
                        case "CARGA DE ARCHIVO":
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
                            //Session[asyncFileUpload.ID] = string.Empty;
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
                if (lstArchivo.Contains(e.FileName.Split('\\').Last())) return;
                AsyncFileUpload uploadControl = (AsyncFileUpload)sender;
                if (!Directory.Exists(BusinessVariables.Directorios.RepositorioTemporalMascara))
                    Directory.CreateDirectory(BusinessVariables.Directorios.RepositorioTemporalMascara);
                uploadControl.SaveAs(BusinessVariables.Directorios.RepositorioTemporalMascara + e.FileName.Split('\\').Last());
                //Session[uploadControl.ID] = e.FileName.Split('\\').Last();
                lstArchivo.Add(e.FileName.Split('\\').Last());
                Session["Files"] = lstArchivo;
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


            }
        }

        public void ValidaMascaraCaptura()
        {
            try
            {
                Mascara mascara = (Mascara)Session["MascaraActiva"];
                foreach (CampoMascara campo in mascara.CampoMascara)
                {
                    string nombreControl;
                    switch (campo.TipoCampoMascara.Descripcion)
                    {
                        case "ALFANUMERICO":
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtAlfanumerico = (TextBox)divControles.FindControl(nombreControl);
                            if (txtAlfanumerico != null)
                            {
                                if (campo.Requerido)
                                    if (txtAlfanumerico.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                if (txtAlfanumerico.Text.Trim().Length < campo.LongitudMinima)
                                    throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                if (txtAlfanumerico.Text.Trim().Length > campo.LongitudMaxima)
                                    throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));

                            }
                            break;
                        case "DECIMAL":
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtDecimal = (TextBox)divControles.FindControl(nombreControl);
                            if (txtDecimal != null)
                            {
                                if (campo.Requerido)
                                    if (txtDecimal.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                //TODO: AGREGAR VALOR MINIMO A ESQUEMA
                                //if (decimal.Parse(txtDecimal.Text.Trim()) < campo.LongitudMinima)
                                //    throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                if (decimal.Parse(txtDecimal.Text.Trim().Replace('_', '0')) > campo.ValorMaximo)
                                    throw new Exception(string.Format("Campo {0} debe se menor o igual a {1}", campo.Descripcion, campo.ValorMaximo));

                            }
                            break;
                        case "ENTERO":
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtEntero = (TextBox)divControles.FindControl(nombreControl);
                            if (txtEntero != null)
                            {
                                if (campo.Requerido)
                                    if (txtEntero.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                //TODO: AGREGAR VALOR MINIMO A ESQUEMA
                                //if (txtEntero.Text.Trim().Length < campo.LongitudMinima)
                                //    throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                if (int.Parse(txtEntero.Text.Trim()) > campo.ValorMaximo)
                                    throw new Exception(string.Format("Campo {0} debe se menor o igual a {1}", campo.Descripcion, campo.ValorMaximo));

                            }
                            break;
                        case "FECHA":
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtFecha = (TextBox)divControles.FindControl(nombreControl);
                            if (txtFecha != null)
                            {
                                try
                                {
                                    var d = DateTime.Parse(txtFecha.Text.Trim());
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
                        case "HORA":
                            nombreControl = "txt" + campo.NombreCampo;

                            DateTime outTime;
                            //return ;
                            TextBox txtHora = (TextBox)divControles.FindControl(nombreControl);
                            if (txtHora != null)
                            {
                                try
                                {
                                    DateTime.TryParseExact(txtHora.Text.Trim(), "HHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out outTime);
                                }
                                catch
                                {
                                    throw new Exception(string.Format("Campo {0} contiene una hora no valida", campo.Descripcion));
                                }
                                if (campo.Requerido)
                                    if (txtHora.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                            }
                            break;
                        case "MONEDA":
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtMoneda = (TextBox)divControles.FindControl(nombreControl);
                            if (txtMoneda != null)
                            {
                                if (campo.Requerido)
                                    if (txtMoneda.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                //TODO: AGREGAR VALOR MINIMO A ESQUEMA
                                //if (decimal.Parse(txtDecimal.Text.Trim()) < campo.LongitudMinima)
                                //    throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                //if (decimal.Parse(txtMoneda.Text.Trim()) > campo.LongitudMaxima)
                                //    throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));

                            }
                            break;
                        case "CATALOGO":
                            nombreControl = "ddl" + campo.NombreCampo;
                            DropDownList ddl = (DropDownList)divControles.FindControl(nombreControl);
                            if (ddl != null)
                            {
                                if (campo.Requerido)
                                    if (ddl.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                            }
                            break;
                        case "MASCARA":
                            nombreControl = "txt" + campo.NombreCampo;
                            TextBox txtMascara = (TextBox)divControles.FindControl(nombreControl);
                            if (txtMascara != null)
                            {
                                if (campo.Requerido)
                                    if (txtMascara.Text.Trim() == String.Empty)
                                        throw new Exception(string.Format("Campo {0} es obligatorio", campo.Descripcion));
                                if (txtMascara.Text.Trim().Length < campo.LongitudMinima)
                                    throw new Exception(string.Format("Campo {0} debe tener al menos {1} caracteres", campo.Descripcion, campo.LongitudMinima));
                                if (txtMascara.Text.Trim().Length > campo.LongitudMaxima)
                                    throw new Exception(string.Format("Campo {0} debe no puede tener mas de {1} caracteres", campo.Descripcion, campo.LongitudMaxima));

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

        public void ConfirmaArchivos()
        {
            try
            {
                if (Session["Files"] != null)
                    BusinessFile.MoverTemporales(BusinessVariables.Directorios.RepositorioTemporalMascara, BusinessVariables.Directorios.RepositorioMascara, (List<string>)Session["Files"]);
                Session["Files"] = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}