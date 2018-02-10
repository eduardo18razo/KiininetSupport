using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniHelp.ServiceSistemaTipoCampoMascara;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas.Formularios
{
    public partial class UcAltaFormulario : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        readonly ServiceTipoCampoMascaraClient _servicioSistemaTipoCampoMascara = new ServiceTipoCampoMascaraClient();
        readonly ServiceCatalogosClient _servicioSistemaCatalogos = new ServiceCatalogosClient();
        readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private List<string> _lstError = new List<string>();

        public int Ejemplo { get; set; }
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

        private bool EsAlta { get { return bool.Parse(hfEsAlta.Value); } set { hfEsAlta.Value = value.ToString(); } }
        private void LlenaCombos()
        {
            try
            {
                rptTiposControles.DataSource = _servicioSistemaTipoCampoMascara.ObtenerTipoCampoMascara(false);
                rptTiposControles.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LimpiarModalCampo()
        {
            try
            {
                txtDescripcionCampo.Text = string.Empty;
                chkRequerido.Checked = false;
                txtLongitudMinima.Text = string.Empty;
                txtLongitudMaxima.Text = string.Empty;
                txtValorMinimo.Text = string.Empty;
                txtValorMaximo.Text = string.Empty;
                txtMascara.Text = string.Empty;
                txtSimboloMoneda.Text = string.Empty;
                ddlCatalogosCampo.SelectedIndex = ddlCatalogosCampo.SelectedIndex >= 1 ? BusinessVariables.ComboBoxCatalogo.IndexSeleccione : -1;
                hfAltaCampo.Value = true.ToString();
                ucAltaCatalogo.LimpiarCampos();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void LimpiarMascara()
        {
            try
            {
                txtDescripcionCampo.Text = string.Empty;
                chkRequerido.Checked = false;
                txtLongitudMinima.Text = string.Empty;
                txtLongitudMaxima.Text = string.Empty;
                txtValorMaximo.Text = string.Empty;
                txtSimboloMoneda.Text = string.Empty;
                if (ddlCatalogosCampo.SelectedIndex > 0)
                    ddlCatalogosCampo.SelectedIndex = BusinessVariables.ComboBoxCatalogo.IndexSeleccione;
                Session["MascaraAlta"] = new Mascara();
                rptControles.DataSource = ((Mascara)Session["MascaraAlta"]).CampoMascara;
                rptControles.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void GeneraControl(int idTipoCampo)
        {
            try
            {
                hfTipoCampo.Value = idTipoCampo.ToString();
                TipoCampoMascara tipoCampo = _servicioSistemaTipoCampoMascara.TipoCampoMascaraId(idTipoCampo);
                if (tipoCampo == null) return;
                imgTitleImage.ImageUrl = "~/assets/images/controls/" + tipoCampo.Image;
                lblTitleAgregarCampo.Text = "" + tipoCampo.Descripcion.Trim();
                divValorMaximo.Visible = false;
                lblDescripcion.Text = tipoCampo.DescripcionTexto;

                txtLongitudMinima.Visible = tipoCampo.LongitudMinima;
                divLongitudMinima.Visible = tipoCampo.LongitudMinima;

                txtLongitudMaxima.Visible = tipoCampo.LongitudMaxima;
                divLongitudMaxima.Visible = tipoCampo.LongitudMaxima;

                divMoneda.Visible = tipoCampo.SimboloMoneda;
                if (tipoCampo.ValorMaximo)
                {
                    divValorMaximo.Visible = tipoCampo.ValorMaximo;
                    if (tipoCampo.Decimal)
                    {
                        txtValorMinimo.Attributes["min"] = "0.01";
                        txtValorMinimo.Attributes["step"] = "0.01";
                        txtValorMaximo.Attributes["min"] = "0.01";
                        txtValorMaximo.Attributes["step"] = "0.01";
                    }
                    else
                    {
                        txtValorMinimo.Attributes["min"] = "0";
                        txtValorMinimo.Attributes["step"] = "1";

                        txtValorMaximo.Attributes["min"] = "1";
                        txtValorMaximo.Attributes["step"] = "1";
                    }
                }
                divCatalgo.Visible = tipoCampo.Catalogo;
                divMascara.Visible = tipoCampo.Mask;
                btnAgregarCampo.Visible = !tipoCampo.Catalogo;
                chkRequerido.Visible = !(tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Logico);
                if (tipoCampo.Catalogo)
                {

                    Metodos.LlenaComboCatalogo(ddlCatalogosCampo, _servicioSistemaCatalogos.ObtenerCatalogosMascaraCaptura(true));
                    divCatalgo.Visible = tipoCampo.Catalogo;
                }
                if (tipoCampo.UploadFile)
                {
                    txtLongitudMaxima.Text = "255";
                    txtLongitudMaxima.Visible = false;
                }

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAgregarCampoMascara\");", true);
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
        private void LlenaDatosMascara(int idMascara)
        {
            try
            {
                Mascara formulario = _servicioMascaras.ObtenerMascaraCaptura(idMascara);
                if (formulario != null)
                {
                    txtNombre.Text = formulario.Descripcion;
                    Session["MascaraAlta"] = formulario;
                    if (EsAlta)
                        foreach (CampoMascara campo in formulario.CampoMascara)
                        {
                            campo.Id = 0;
                        }
                    rptControles.DataSource = formulario.CampoMascara;
                    rptControles.DataBind();
                }
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
                ucAltaCatalogo.OnAceptarModal += UcAltaCatalogoOnOnAceptarModal;
                ucAltaCatalogo.EsAlta = true;
                if (IsPostBack) return;
                Session["MascaraAlta"] = new Mascara();
                rptControles.DataSource = null;
                rptControles.DataBind();
                LlenaCombos();
                if (!IsPostBack)
                {
                    if (Request.QueryString["idFormulario"] != null)
                    {
                        if (Request.QueryString["Alta"] != null)
                            EsAlta = bool.Parse(Request.QueryString["Alta"]);
                        LlenaDatosMascara(int.Parse(Request.QueryString["idFormulario"]));
                    }
                    else
                    {
                        EsAlta = true;
                    }
                }
                divAgregarCampos.Visible = EsAlta;
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
        private void UcAltaCatalogoOnOnAceptarModal()
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlCatalogosCampo, _servicioSistemaCatalogos.ObtenerCatalogosMascaraCaptura(true));
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
        protected void chkObligatorio_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txtMin = (TextBox)rptControles.Items[((RepeaterItem)((CheckBox)sender).NamingContainer).ItemIndex].FindControl("txtLongitudMinima");
                TextBox txtMax = (TextBox)rptControles.Items[((RepeaterItem)((CheckBox)sender).NamingContainer).ItemIndex].FindControl("txtLongitudMaxima");
                if (txtMin == null || txtMax == null) return;
                txtMin.Enabled = ((CheckBox)sender).Checked;
                txtMax.Enabled = ((CheckBox)sender).Checked;
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
        protected void btnLimpiar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarMascara();
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
        protected void btnEditarCampo_OnClick(object sender, EventArgs e)
        {
            try
            {
                Mascara tmpMascara = ((Mascara)Session["MascaraAlta"]);
                LinkButton btn = (LinkButton)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                Label lblIdTipoCampo = (Label)item.FindControl("lblIdTipoCampoMascara");
                Label lblDescripcion = (Label)item.FindControl("lblDescripcion");
                bool requerido = bool.Parse(((Label)item.FindControl("lblRequerido")).Text);

                CampoMascara campoEditar = tmpMascara.CampoMascara.SingleOrDefault(s => s.IdTipoCampoMascara == int.Parse(lblIdTipoCampo.Text) && s.Descripcion == lblDescripcion.Text && s.Requerido == requerido);
                if (campoEditar == null) return;
                hfCampoEditado.Value = tmpMascara.CampoMascara.IndexOf(campoEditar).ToString();
                hfTipoCampo.Value = campoEditar.IdTipoCampoMascara.ToString();
                GeneraControl(int.Parse(hfTipoCampo.Value));
                chkRequerido.Checked = campoEditar.Requerido;
                txtDescripcionCampo.Text = campoEditar.Descripcion;

                chkRequerido.Enabled = EsAlta;

                if (divLongitudMinima.Visible)
                {
                    //txtLongitudMinima.Enabled = EsAlta;
                    txtLongitudMinima.Text = campoEditar.LongitudMinima.ToString();
                }

                if (divLongitudMaxima.Visible)
                {
                    //txtLongitudMaxima.Enabled = EsAlta;
                    txtLongitudMaxima.Text = campoEditar.LongitudMaxima.ToString();
                }

                if (divCatalgo.Visible)
                {
                    ddlCatalogosCampo.Enabled = EsAlta;
                    ucAltaCatalogo.Visible = EsAlta;
                    ddlCatalogosCampo.SelectedValue = campoEditar.IdCatalogo.ToString();
                }

                if (divMoneda.Visible)
                {
                    //txtSimboloMoneda.Enabled = EsAlta;
                    txtSimboloMoneda.Text = campoEditar.SimboloMoneda;
                }

                if (divValorMaximo.Visible)
                {
                    //txtValorMinimo.Enabled = EsAlta;
                    //txtValorMaximo.Enabled = true;
                    txtValorMinimo.Text = campoEditar.ValorMinimo.ToString();
                    txtValorMaximo.Text = campoEditar.ValorMaximo.ToString();
                }
                if (divMascara.Visible)
                {
                    //txtMascara.Enabled = EsAlta;
                    txtMascara.Text = campoEditar.MascaraDetalle;
                }
                GeneraControl(int.Parse(hfTipoCampo.Value));
                hfAltaCampo.Value = false.ToString();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAgregarCampoMascara\");", true);
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
        protected void btnEliminarCampo_OnClick(object sender, EventArgs e)
        {
            try
            {
                Mascara tmpMascara = ((Mascara)Session["MascaraAlta"]);
                LinkButton btn = (LinkButton)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                Label lblIdTipoCampo = (Label)item.FindControl("lblIdTipoCampoMascara");
                Label lblDescripcion = (Label)item.FindControl("lblDescripcion");
                Label lblRequerido = (Label)item.FindControl("lblRequerido");

                CampoMascara campoEditar = tmpMascara.CampoMascara.SingleOrDefault(s => s.IdTipoCampoMascara == int.Parse(lblIdTipoCampo.Text) && s.Descripcion == lblDescripcion.Text && s.Requerido == bool.Parse(lblRequerido.Text));
                if (campoEditar == null) return;
                tmpMascara.CampoMascara.Remove(campoEditar);
                rptControles.DataSource = tmpMascara.CampoMascara;
                rptControles.DataBind();
                Session["MascaraAlta"] = tmpMascara;
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
        protected void btnSubir_OnClick(object sender, EventArgs e)
        {
            try
            {
                Mascara tmpMascara = ((Mascara)Session["MascaraAlta"]);
                LinkButton btn = (LinkButton)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                Label lblIdTipoCampo = (Label)item.FindControl("lblIdTipoCampoMascara");
                Label lblDescripcion = (Label)item.FindControl("lblDescripcion");
                Label lblRequerido = (Label)item.FindControl("lblRequerido");

                CampoMascara campoEditar = tmpMascara.CampoMascara.SingleOrDefault(s => s.IdTipoCampoMascara == int.Parse(lblIdTipoCampo.Text) && s.Descripcion == lblDescripcion.Text && s.Requerido == bool.Parse(lblRequerido.Text));
                if (campoEditar == null) return;
                int indexActual = tmpMascara.CampoMascara.IndexOf(campoEditar);
                if (indexActual == 0) return;
                tmpMascara.CampoMascara.Remove(campoEditar);
                tmpMascara.CampoMascara.Insert(indexActual - 1, campoEditar);
                rptControles.DataSource = tmpMascara.CampoMascara;
                rptControles.DataBind();
                Session["MascaraAlta"] = tmpMascara;
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
        protected void btnBajar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Mascara tmpMascara = ((Mascara)Session["MascaraAlta"]);
                LinkButton btn = (LinkButton)sender;
                RepeaterItem item = (RepeaterItem)btn.NamingContainer;
                Label lblIdTipoCampo = (Label)item.FindControl("lblIdTipoCampoMascara");
                Label lblDescripcion = (Label)item.FindControl("lblDescripcion");
                Label lblRequerido = (Label)item.FindControl("lblRequerido");

                CampoMascara campoEditar = tmpMascara.CampoMascara.SingleOrDefault(s => s.IdTipoCampoMascara == int.Parse(lblIdTipoCampo.Text) && s.Descripcion == lblDescripcion.Text && s.Requerido == bool.Parse(lblRequerido.Text));
                if (campoEditar == null) return;
                int indexActual = tmpMascara.CampoMascara.IndexOf(campoEditar);
                tmpMascara.CampoMascara.Remove(campoEditar);
                tmpMascara.CampoMascara.Insert(indexActual + 1, campoEditar);
                rptControles.DataSource = tmpMascara.CampoMascara;
                rptControles.DataBind();
                Session["MascaraAlta"] = tmpMascara;
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
        protected void rptTiposControles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item)
                {
                    ((LinkButton)e.Item.FindControl("btnAgregarControl")).Enabled = EsAlta;
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

        protected void rptControles_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (rptControles.Items.Count < 1)
                {
                    if (e.Item.ItemType == ListItemType.Header)
                    {
                        HtmlGenericControl noRecordsDiv = (e.Item.FindControl("NoRecords") as HtmlGenericControl);
                        if (noRecordsDiv != null)
                        {
                            noRecordsDiv.Visible = true;
                        }

                    }
                }
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.FindControl("btnEliminarCampo").Visible = EsAlta;
                    e.Item.FindControl("btnSubir").Visible = EsAlta;
                    e.Item.FindControl("btnBajar").Visible = EsAlta;
                    e.Item.FindControl("lblSeparador").Visible = EsAlta;
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
        protected void btnAgregarControl_OnClick(object sender, EventArgs e)
        {
            try
            {

                GeneraControl(Convert.ToInt32(((LinkButton)sender).CommandArgument));
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAgregarCampoMascara\");", true);
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
        protected void btnAgregarCampo_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtDescripcionCampo.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar una descripcion");
                Regex rgx = new Regex("[a-zA-Z]");

                if (!rgx.IsMatch(txtDescripcionCampo.Text.Trim()))
                    throw new Exception("La descripcion debe contener almenos un caracter Alfanumerico.");

                if (divLongitudMinima.Visible)
                {
                    if (txtLongitudMinima.Text.Trim() == string.Empty)
                        throw new Exception("Debe especificar una longitud minima");
                }
                if (divLongitudMinima.Visible && !divLongitudMaxima.Visible)
                    if (int.Parse(txtLongitudMinima.Text) >= 100)
                        throw new Exception("Debe especificar una longitud minima menor a 100");

                if (divLongitudMaxima.Visible)
                {
                    if (txtLongitudMaxima.Text.Trim() == string.Empty)
                        throw new Exception("Debe especificar una longitud maxima");
                }

                if (divLongitudMinima.Visible && divLongitudMaxima.Visible)
                    if (int.Parse(txtLongitudMinima.Text.Trim()) > int.Parse(txtLongitudMaxima.Text.Trim()))
                    {
                        throw new Exception("Longitud minima no puede ser mayor que longitud maxima");
                    }

                if (divCatalgo.Visible)
                    if (ddlCatalogosCampo.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                        throw new Exception("Debe especificar un catalogo");

                if (divMoneda.Visible)
                    if (txtSimboloMoneda.Text.Trim() == string.Empty)
                        throw new Exception("Debe especificar una descripcion de moneda");

                if (divValorMaximo.Visible)
                {
                    if (txtValorMinimo.Text.Trim() == string.Empty)
                        throw new Exception("Debe especificar un valor minimo");
                    if (txtValorMaximo.Text.Trim() == string.Empty)
                        throw new Exception("Debe especificar un valor maximo");
                    if (double.Parse(txtValorMinimo.Text) > double.Parse(txtValorMaximo.Text))
                        throw new Exception("El valor Minimo no debe ser mayor a valor Maximo");
                }
                if (divMascara.Visible)
                    if (txtMascara.Text.Trim() == string.Empty)
                        throw new Exception("Debe especificar un Formulario de Cliente");


                Mascara tmpMascara = ((Mascara)Session["MascaraAlta"]);

                if (bool.Parse(hfAltaCampo.Value))
                {
                    if (tmpMascara.CampoMascara == null)
                        tmpMascara.CampoMascara = new List<CampoMascara>();

                    if (tmpMascara.CampoMascara.Any(a => a.Descripcion == txtDescripcionCampo.Text.Trim()))
                    {
                        throw new Exception("Este campo ya existe.");
                    }

                    TipoCampoMascara tipoCampo = _servicioSistemaTipoCampoMascara.TipoCampoMascaraId(Convert.ToInt32(hfTipoCampo.Value));

                    Catalogos catalogo = null;
                    if (tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton
                        || tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable
                        || tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación)
                        catalogo = new ServiceCatalogosClient().ObtenerCatalogo(int.Parse(ddlCatalogosCampo.SelectedValue));

                    tmpMascara.CampoMascara.Add(new CampoMascara
                    {
                        IdCatalogo = tipoCampo.Catalogo ? Convert.ToInt32(ddlCatalogosCampo.SelectedValue) : (int?)null,
                        IdTipoCampoMascara = tipoCampo.Id,
                        Multiple = tipoCampo.Multiple,
                        CheckBox = tipoCampo.Checkbox,
                        RadioButton = tipoCampo.RadioButton,
                        EsArchivo = tipoCampo.UploadFile,
                        Descripcion = txtDescripcionCampo.Text.Trim(),
                        Requerido = chkRequerido.Checked,
                        LongitudMinima = tipoCampo.LongitudMinima ? Convert.ToInt32(txtLongitudMinima.Text.Trim()) : tipoCampo.Mask ? 1 : (int?)null,
                        LongitudMaxima = tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto ? int.Parse(tipoCampo.LongitudMaximaPermitida) :
                                         tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo ? 3900 :
                                         tipoCampo.LongitudMaxima ? Convert.ToInt32(txtLongitudMaxima.Text.Trim()) : tipoCampo.Mask ? txtMascara.Text.Trim().Length : (int?)null,
                        SimboloMoneda = tipoCampo.SimboloMoneda ? txtSimboloMoneda.Text.Trim() : null,
                        ValorMinimo = tipoCampo.ValorMinimo ? decimal.Parse(txtValorMinimo.Text.Trim()) : (decimal?)null,
                        ValorMaximo = tipoCampo.ValorMaximo ? decimal.Parse(txtValorMaximo.Text.Trim()) : (decimal?)null,
                        MascaraDetalle = tipoCampo.Mask ? txtMascara.Text.Trim() : null,
                        TipoCampoMascara = tipoCampo
                    });
                }
                else
                {
                    TipoCampoMascara tipoCampo = _servicioSistemaTipoCampoMascara.TipoCampoMascaraId(Convert.ToInt32(hfTipoCampo.Value));
                    Catalogos catalogo = null;
                    if (tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.RadioBoton
                        || tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.ListaDespledable
                        ||
                        tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación)
                        catalogo = new ServiceCatalogosClient().ObtenerCatalogo(int.Parse(ddlCatalogosCampo.SelectedValue));

                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].IdCatalogo = tipoCampo.Catalogo ? Convert.ToInt32(ddlCatalogosCampo.SelectedValue) : (int?)null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].Catalogos = catalogo;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].IdTipoCampoMascara = tipoCampo.Id;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].Descripcion = txtDescripcionCampo.Text.Trim();
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].Requerido = chkRequerido.Checked;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].LongitudMinima = tipoCampo.LongitudMinima ? Convert.ToInt32(txtLongitudMinima.Text.Trim()) : tipoCampo.Mask ? 1 : (int?)null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].LongitudMaxima = tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.Texto ? int.Parse(tipoCampo.LongitudMaximaPermitida) :
                                         tipoCampo.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo ? 3900 :
                                         tipoCampo.LongitudMaxima ? Convert.ToInt32(txtLongitudMaxima.Text.Trim()) : tipoCampo.Mask ? txtMascara.Text.Trim().Length : (int?)null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].SimboloMoneda = tipoCampo.SimboloMoneda ? txtSimboloMoneda.Text.Trim() : null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].ValorMinimo = tipoCampo.ValorMinimo ? decimal.Parse(txtValorMinimo.Text.Trim()) : (decimal?)null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].ValorMaximo = tipoCampo.ValorMaximo ? decimal.Parse(txtValorMaximo.Text.Trim()) : (decimal?)null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].MascaraDetalle = tipoCampo.Mask ? txtMascara.Text.Trim() : null;
                    tmpMascara.CampoMascara[int.Parse(hfCampoEditado.Value)].TipoCampoMascara = tipoCampo;
                }

                rptControles.DataSource = tmpMascara.CampoMascara;
                rptControles.DataBind();
                Session["MascaraAlta"] = tmpMascara;
                LimpiarModalCampo();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAgregarCampoMascara\");", true);
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

        protected void btnCancelarModal_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarModalCampo();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAgregarCampoMascara\");", true);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/Administracion/Formularios/FrmConsultaFormularios.aspx");
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
                if (txtNombre.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar un nombre.");
                Mascara nuevaMascara = ((Mascara)Session["MascaraAlta"]);
                if (((Mascara)Session["MascaraAlta"]).CampoMascara != null && ((Mascara)Session["MascaraAlta"]).CampoMascara.Count <= 0)
                    throw new Exception("Debe al menos un campo.");
                if (!((Mascara)Session["MascaraAlta"]).CampoMascara.Any(a=>a.Requerido))
                    throw new Exception("Debe al menos un campo obligatorio.");
                nuevaMascara.Descripcion = txtNombre.Text.Trim();
                nuevaMascara.Random = chkClaveRegistro.Checked;
                nuevaMascara.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;
                if (EsAlta)
                {
                    nuevaMascara.Id = 0;
                    _servicioMascaras.CrearMascara(nuevaMascara);
                }
                else
                    _servicioMascaras.ActualizarMascara(nuevaMascara);

                LimpiarMascara();
                if (OnAceptarModal != null)
                    OnAceptarModal();
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
        protected void btnPreview_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.Text.Trim() == string.Empty)
                    throw new Exception("Debe especificar un nombre.");
                Mascara nuevaMascara = ((Mascara)Session["MascaraAlta"]);
                if (((Mascara)Session["MascaraAlta"]).CampoMascara.Count <= 0)
                    throw new Exception("Debe al menos un campo.");
                nuevaMascara.Descripcion = txtNombre.Text.Trim();
                nuevaMascara.Random = chkClaveRegistro.Checked;
                nuevaMascara.IdUsuarioAlta = ((Usuario)Session["UserData"]).Id;

                Session["PreviewDataFormulario"] = nuevaMascara; ;
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";

                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptErrorAlert", "window.open('" + url + "Users/Administracion/Formularios/FrmPreviewFormulario.aspx','_blank');", true);
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
        protected void btnLimpiarCampo_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiarModalCampo();
                hfTipoCampo.Value = string.Empty;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAgregarCampoMascara\");", true);
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