using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceSistemaDomicilio;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Altas
{
    public partial class UcAltaUbicacion : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
        readonly ServiceDomicilioSistemaClient _servicioSistemaDomicilio = new ServiceDomicilioSistemaClient();

        private List<string> _lstError = new List<string>();

        public int IdTipoUsuario
        {
            get
            {
                return FromModal ? Convert.ToInt32(hfTipoUsuario.Value) : Convert.ToInt32(ddlTipoUsuario.SelectedValue);
            }
            set
            {
                hfTipoUsuario.Value = value.ToString();
                if (FromModal)
                {
                    LlenaComboUbicacion(IdTipoUsuario);
                    LlenaCombos();
                }

            }
        }
        public bool FromModal
        {
            get
            {
                bool s = (bool)ViewState["FromModal"];
                return s;
            }

            set
            {
                ViewState["FromModal"] = value;
            }
        }

        public Ubicacion ObtenerUbicacion()
        {
            Ubicacion ubicacion;
            try
            {
                ubicacion = new Ubicacion
                {
                    IdTipoUsuario = IdTipoUsuario,
                    IdPais = Convert.ToInt32(ddlpais.SelectedValue)
                };

                if (ddlCampus.SelectedValue != string.Empty & ddlCampus.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    ubicacion.IdCampus = Convert.ToInt32(ddlCampus.SelectedValue);

                if (ddlTorre.SelectedValue != string.Empty & ddlTorre.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    ubicacion.IdTorre = Convert.ToInt32(ddlTorre.SelectedValue);

                if (ddlPiso.SelectedValue != string.Empty & ddlPiso.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    ubicacion.IdPiso = Convert.ToInt32(ddlPiso.SelectedValue);

                if (ddlZona.SelectedValue != string.Empty & ddlZona.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    ubicacion.IdZona = Convert.ToInt32(ddlZona.SelectedValue);

                if (ddlSubZona.SelectedValue != string.Empty & ddlSubZona.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    ubicacion.IdSubZona = Convert.ToInt32(ddlSubZona.SelectedValue);

                if (ddlSiteRack.SelectedValue != string.Empty & ddlSiteRack.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    ubicacion.IdSiteRack = Convert.ToInt32(ddlSiteRack.SelectedValue);

            }
            catch
            {
                throw new Exception("Error al obtener Organizacion intente nuevamente.");
            }
            return ubicacion;
        }

        public List<string> AlertaUbicacion
        {
            set
            {
                panelAlertaUbicacion.Visible = value.Any();
                if (!panelAlertaUbicacion.Visible) return;
                rptErrorUbicacion.DataSource = value;
                rptErrorUbicacion.DataBind();
            }
        }

        private List<string> AlertaCampus
        {
            set
            {
                panelAlertaCampus.Visible = value.Any();
                if (!panelAlertaCampus.Visible) return;
                rptErrorCampus.DataSource = value;
                rptErrorCampus.DataBind();
            }
        }

        private List<string> AlertaCatalogos
        {
            set
            {
                panelAlertaCatalogo.Visible = value.Any();
                if (!panelAlertaCatalogo.Visible) return;
                rptErrorCatalogo.DataSource = value;
                rptErrorCatalogo.DataBind();
            }
        }

        private void LlenaCombos()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true, true);
                if (!FromModal)
                    Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
                Metodos.LlenaComboCatalogo(ddlTipoUsuarioCatalogo, lstTipoUsuario);
                Metodos.LlenaComboCatalogo(ddlTipoUsuarioCampus, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaComboUbicacion(int idTipoUsuario)
        {
            try
            {
                Metodos.LlenaComboCatalogo(ddlpais, _servicioUbicacion.ObtenerPais(idTipoUsuario, true));
                if (ddlpais.Items.Count != 2) return;
                ddlpais.SelectedIndex = 1;
                ddlpais_OnSelectedIndexChanged(ddlpais, null);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private void FiltraCombo(DropDownList ddlFiltro, DropDownList ddlLlenar, object source)
        {
            try
            {
                ddlLlenar.Items.Clear();
                if (ddlFiltro.SelectedValue != BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                {
                    ddlLlenar.Enabled = true;
                    Metodos.LlenaComboCatalogo(ddlLlenar, source);
                }
                else
                {
                    ddlLlenar.DataSource = null;
                    ddlLlenar.DataBind();
                }

                ddlLlenar.Enabled = ddlLlenar.DataSource != null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void ValidaCapturaUbicacion()
        {
            StringBuilder sb = new StringBuilder();
            if (ddlpais.SelectedValue == BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                sb.AppendLine("<li>Debe especificar al menos un Pais.</li>");
            if (ddlCampus.SelectedValue == BusinessVariables.ComboBoxCatalogo.ValueSeleccione.ToString())
                sb.AppendLine("<li>Debe especificar al menos un Campus.</li>");

            if (sb.ToString() != string.Empty)
            {
                sb.Append("</ul>");
                sb.Insert(0, "<ul>");
                sb.Insert(0, "<h3>Ubicacion</h3>");
                throw new Exception(sb.ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaUbicacion = new List<string>();
                AlertaCatalogos = new List<string>();
                AlertaCampus = new List<string>();
                if (!IsPostBack)
                {
                    divTipoUsuario.Visible = !FromModal;
                    if (FromModal)
                    {
                        LlenaComboUbicacion(IdTipoUsuario);
                    }
                }
                //LlenaCombos();
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Metodos.LimpiarCombo(ddlpais);
                Metodos.LimpiarCombo(ddlCampus);
                Metodos.LimpiarCombo(ddlTorre);
                Metodos.LimpiarCombo(ddlPiso);
                Metodos.LimpiarCombo(ddlZona);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                if (ddlTipoUsuario.SelectedIndex != BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    LlenaComboUbicacion(IdTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlpais_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlCampus);
                Metodos.LimpiarCombo(ddlTorre);
                Metodos.LimpiarCombo(ddlPiso);
                Metodos.LimpiarCombo(ddlZona);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlCampus, _servicioUbicacion.ObtenerCampus(idTipoUsuario, id, true));

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlCampus_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlTorre);
                Metodos.LimpiarCombo(ddlPiso);
                Metodos.LimpiarCombo(ddlZona);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlTorre, _servicioUbicacion.ObtenerTorres(IdTipoUsuario, id, true));

            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlTorre_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlPiso);
                Metodos.LimpiarCombo(ddlZona);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlPiso, _servicioUbicacion.ObtenerPisos(idTipoUsuario, id, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlPiso_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlZona);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlZona, _servicioUbicacion.ObtenerZonas(idTipoUsuario, id, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlZona_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlSubZona, _servicioUbicacion.ObtenerSubZonas(idTipoUsuario, id, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlSubZona_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlSiteRack, _servicioUbicacion.ObtenerSiteRacks(idTipoUsuario, id, true));
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void btnCrearCampus_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<string> sb = Metodos.ValidaCapturaCatalogoCampus(Convert.ToInt32(ddlTipoUsuarioCampus.SelectedValue), txtDescripcionCampus.Text, ddlColonia.SelectedValue == "" ? 0 : Convert.ToInt32(ddlColonia.SelectedValue), txtCalle.Text.Trim(), txtNoExt.Text.Trim(), txtNoInt.Text.Trim());
                if (sb.Count > 0)
                {
                    sb.Insert(0, "<h3>Datos Generales</h3>");
                    _lstError = sb;
                    throw new Exception("");
                }
                Ubicacion ubicacion = new Ubicacion
                {
                    IdTipoUsuario = IdTipoUsuario,
                    IdPais = Convert.ToInt32(ddlpais.SelectedValue),
                    Campus = new Campus
                    {
                        IdTipoUsuario = Convert.ToInt32(ddlTipoUsuarioCampus.SelectedValue),
                        Descripcion = txtDescripcionCampus.Text.Trim(),
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
                LimpiaCampus();
                ddlpais_OnSelectedIndexChanged(ddlpais, null);
                upUbicacion.Update();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCampus\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaCampus = _lstError;
            }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                Button lbtn = (Button)sender;
                if (sender == null) return;
                if (lbtn.CommandArgument == "0")
                    ddlTipoUsuarioCampus.SelectedValue = IdTipoUsuario.ToString();
                else
                {
                    ValidaSeleccion(lbtn.CommandArgument);
                    lblTitleCatalogo.Text = ObtenerRuta(lbtn.CommandArgument, lbtn.CommandName.ToUpper());
                    hfCatalogo.Value = lbtn.CommandArgument;
                    ddlTipoUsuarioCatalogo.SelectedValue = IdTipoUsuario.ToString();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editCatalogoUbicacion\");", true);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        public string ObtenerRuta(string command, string modulo)
        {
            string result = "<h3>AGREGAR " + modulo + "</h3><span style=\"font-size: x-small;\">";
            switch (command)
            {
                case "2":
                    result += ddlpais.SelectedItem.Text;
                    break;
                case "3":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text;
                    break;
                case "4":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text;
                    break;
                case "5":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text + ">" + ddlPiso.SelectedItem.Text;
                    break;
                case "6":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text + ">" + ddlPiso.SelectedItem.Text + ">" + ddlZona.SelectedItem.Text;
                    break;
                case "7":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text + ">" + ddlPiso.SelectedItem.Text + ">" + ddlZona.SelectedItem.Text + ">" + ddlSubZona.SelectedItem.Text;
                    break;
            }
            result += "</span>";
            return result;
        }

        private void ValidaSeleccion(string command)
        {
            try
            {
                switch (command)
                {
                    case "0":
                        if (ddlpais.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        break;
                    case "3":
                        if (ddlpais.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlCampus.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        break;
                    case "4":
                        if (ddlpais.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlCampus.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlTorre.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        break;
                    case "5":
                        if (ddlpais.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlCampus.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlTorre.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlPiso.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        break;
                    case "6":
                        if (ddlpais.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlCampus.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlTorre.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlPiso.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlZona.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        break;
                    case "7":
                        if (ddlpais.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlCampus.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlTorre.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlPiso.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlZona.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        if (ddlSubZona.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                            throw new Exception();
                        break;
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCatalogoUbicacion\");", true);
                throw new Exception("Debe de Seleccionarse un Padre para esta Operacion");
            }
        }

        #region CatalogoAlta
        private void LimpiaCatalogo()
        {
            try
            {
                txtDescripcionCatalogo.Text = string.Empty;
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void btnGuardarCatalogo_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Metodos.ValidaCapturaCatalogo(txtDescripcionCatalogo.Text))
                {
                    int idTipoUsuario = IdTipoUsuario;
                    Ubicacion ubicacion = new Ubicacion
                    {
                        IdTipoUsuario = idTipoUsuario,
                        IdPais = Convert.ToInt32(ddlpais.SelectedValue)
                    };
                    idTipoUsuario = IdTipoUsuario;
                    switch (int.Parse(hfCatalogo.Value))
                    {
                        case 3:
                            ubicacion.IdCampus = Convert.ToInt32(ddlCampus.SelectedValue);
                            ubicacion.Torre = new Torre
                            {
                                IdTipoUsuario = idTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            ddlCampus_OnSelectedIndexChanged(ddlCampus, null);
                            upUbicacion.Update();
                            break;
                        case 4:
                            ubicacion.IdCampus = Convert.ToInt32(ddlCampus.SelectedValue);
                            ubicacion.IdTorre = Convert.ToInt32(ddlTorre.SelectedValue);
                            ubicacion.Piso = new Piso
                            {
                                IdTipoUsuario = idTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            ddlTorre_OnSelectedIndexChanged(ddlTorre, null);
                            upUbicacion.Update();
                            break;
                        case 5:
                            ubicacion.IdCampus = Convert.ToInt32(ddlCampus.SelectedValue);
                            ubicacion.IdTorre = Convert.ToInt32(ddlTorre.SelectedValue);
                            ubicacion.IdPiso = Convert.ToInt32(ddlPiso.SelectedValue);
                            ubicacion.Zona = new Zona
                            {
                                IdTipoUsuario = idTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };

                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            ddlPiso_OnSelectedIndexChanged(ddlPiso, null);
                            upUbicacion.Update();
                            break;
                        case 6:
                            ubicacion.IdCampus = Convert.ToInt32(ddlCampus.SelectedValue);
                            ubicacion.IdTorre = Convert.ToInt32(ddlTorre.SelectedValue);
                            ubicacion.IdPiso = Convert.ToInt32(ddlPiso.SelectedValue);
                            ubicacion.IdZona = Convert.ToInt32(ddlZona.SelectedValue);
                            ubicacion.SubZona = new SubZona
                            {
                                IdTipoUsuario = idTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };

                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            ddlZona_OnSelectedIndexChanged(ddlZona, null);
                            upUbicacion.Update();
                            break;
                        case 7:
                            ubicacion.IdCampus = Convert.ToInt32(ddlCampus.SelectedValue);
                            ubicacion.IdTorre = Convert.ToInt32(ddlTorre.SelectedValue);
                            ubicacion.IdPiso = Convert.ToInt32(ddlPiso.SelectedValue);
                            ubicacion.IdZona = Convert.ToInt32(ddlZona.SelectedValue);
                            ubicacion.IdSubZona = Convert.ToInt32(ddlSubZona.SelectedValue);
                            ubicacion.SiteRack = new SiteRack
                            {
                                IdTipoUsuario = idTipoUsuario,
                                Descripcion = txtDescripcionCatalogo.Text.Trim(),
                                Habilitado = chkHabilitado.Checked
                            };
                            _servicioUbicacion.GuardarUbicacion(ubicacion);
                            ddlSubZona_OnSelectedIndexChanged(ddlSubZona, null);
                            upUbicacion.Update();
                            break;
                    }
                    LimpiaCatalogo();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCatalogoUbicacion\");", true);
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaCatalogos = _lstError;
            }
        }

        protected void btnCancelarCatalogo_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiaCatalogo();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCatalogoUbicacion\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        #endregion

        #region Campus
        private void LimpiaCampus()
        {
            try
            {
                txtDescripcionCampus.Text = string.Empty;
                txtCp.Text = string.Empty;
                txtCalle.Text = string.Empty;
                txtNoExt.Text = string.Empty;
                txtNoInt.Text = string.Empty;
                lblMunicipio.Text = string.Empty;
                lblEstado.Text = string.Empty;
                Metodos.LimpiarCombo(ddlColonia);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaCampus = _lstError;
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
                AlertaCampus = _lstError;
            }
        }

        protected void btnCancelarCampus_OnClick(object sender, EventArgs e)
        {
            try
            {
                LimpiaCampus();
                ddlTipoUsuarioCatalogo.SelectedIndex = 0;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCampus\");", true);
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaCampus = _lstError;
            }
        }

        #endregion Campus

        protected void btnAceptar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (OnAceptarModal != null)
                {
                    OnAceptarModal();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (OnCancelarModal != null)
                {
                    OnCancelarModal();
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaUbicacion = _lstError;
            }
        }

        protected void ddlColonia_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlColonia.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    lblMunicipio.Text = string.Empty;
                    lblEstado.Text = string.Empty;
                    return;
                }
                Colonia col = _servicioSistemaDomicilio.ObtenerDetalleColonia(int.Parse(ddlColonia.SelectedValue));
                if (col != null)
                {
                    lblMunicipio.Text = col.Municipio.Descripcion;
                    lblEstado.Text = col.Municipio.Estado.Descripcion;
                }
            }
            catch (Exception ex)
            {
                if (_lstError == null)
                {
                    _lstError = new List<string>();
                }
                _lstError.Add(ex.Message);
                AlertaCampus = _lstError;
            }
        }
    }
}