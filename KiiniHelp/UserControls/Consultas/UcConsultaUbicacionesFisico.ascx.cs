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

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaUbicacionesFisico : UserControl, IControllerModal
    {
        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
        private readonly ServiceDomicilioSistemaClient _servicioSistemaDomicilio = new ServiceDomicilioSistemaClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();

        private List<string> _lstError = new List<string>();
        public delegate void DelegateSeleccionUbicacionModal();
        //public event UcConsultaUbicaciones.DelegateSeleccionUbicacionModal OnSeleccionUbicacionModal;

        public bool Modal
        {
            get { return Convert.ToBoolean(hfModal.Value); }
            set
            {
                hfModal.Value = value.ToString();
                lblTitleUbicacion.Text = value ? "Agregar Ubicación" : "Ubicaciones";
            }
        }

        public string ModalName
        {
            set { hfModalName.Value = value; }
        }

        public int IdTipoUsuario
        {
            get { return Convert.ToInt32(ddlTipoUsuario.SelectedValue); }
            set
            {
                ddlTipoUsuario.SelectedValue = value.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(null, null);
                ddlTipoUsuario.Enabled = false;
            }
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

        public int UbicacionSeleccionada
        {
            get
            {
                if (hfIdSeleccion.Value == null || hfIdSeleccion.Value.Trim() == string.Empty)
                    throw new Exception("Debe Seleccionar una organización");
                return Convert.ToInt32(hfIdSeleccion.Value);
            }
            set
            {
                LlenaUbicaciones();
                foreach (RepeaterItem item in rptResultados.Items)
                {
                    if ((((DataBoundLiteralControl)item.Controls[0])).Text.Split('\n')[1].Contains("id='" + value + "'"))
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "Scripts", "SeleccionaOrganizacion(\"" + value + "\");", true);
                }
                hfIdSeleccion.Value = value.ToString();
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
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true);
                if (lstTipoUsuario.Count >= 2)
                    lstTipoUsuario.Insert(BusinessVariables.ComboBoxCatalogo.IndexTodos, new TipoUsuario { Id = BusinessVariables.ComboBoxCatalogo.ValueTodos, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionTodos });
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

        private void LlenaUbicaciones()
        {
            try
            {
                int? idTipoUsuario = null;
                int? idPais = null;
                int? idCampus = null;
                int? idTorre = null;
                int? idPiso = null;
                int? idZona = null;
                int? idSubZona = null;
                int? idSiteRack = null;
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

                if (ddlpais.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idPais = int.Parse(ddlpais.SelectedValue);

                if (ddlCampus.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idCampus = int.Parse(ddlCampus.SelectedValue);

                if (ddlTorre.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTorre = int.Parse(ddlTorre.SelectedValue);

                if (ddlPiso.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idPiso = int.Parse(ddlPiso.SelectedValue);

                if (ddlZona.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idZona = int.Parse(ddlZona.SelectedValue);

                if (ddlSubZona.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idSubZona = int.Parse(ddlSubZona.SelectedValue);

                if (ddlSiteRack.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idSiteRack = int.Parse(ddlSiteRack.SelectedValue);
                List<Ubicacion> lstUbicaciones = _servicioUbicacion.ObtenerUbicaciones(idTipoUsuario, idPais, idCampus, idTorre, idPiso, idZona, idSubZona, idSiteRack);
                if (Modal)
                    lstUbicaciones = lstUbicaciones.Where(w => w.Habilitado == Modal).ToList();

                rptResultados.DataSource = lstUbicaciones;
                rptResultados.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LimpiarOrganizaciones()
        {
            try
            {
                rptResultados.DataSource = null;
                rptResultados.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void SetAlias()
        {
            try
            {
                lblNivel1.Text = "Nivel 1";
                lblNivel2.Text = "Nivel 2";
                lblNivel3.Text = "Nivel 3";
                lblNivel4.Text = "Nivel 4";
                lblNivel5.Text = "Nivel 5";
                lblNivel6.Text = "Nivel 6";
                lblNivel7.Text = "Nivel 7";
                if (ddlTipoUsuario.SelectedIndex <= BusinessVariables.ComboBoxCatalogo.IndexTodos) return;
                List<AliasUbicacion> alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue));
                if (alias.Count != 7)
                {
                    return;
                }
                lblNivel1.Text = alias.Single(s => s.Nivel == 1).Descripcion;
                lblNivel2.Text = alias.Single(s => s.Nivel == 2).Descripcion;
                lblNivel3.Text = alias.Single(s => s.Nivel == 3).Descripcion;
                lblNivel4.Text = alias.Single(s => s.Nivel == 4).Descripcion;
                lblNivel5.Text = alias.Single(s => s.Nivel == 5).Descripcion;
                lblNivel6.Text = alias.Single(s => s.Nivel == 6).Descripcion;
                lblNivel7.Text = alias.Single(s => s.Nivel == 7).Descripcion;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AlertaUbicacion = new List<string>();
                AlertaCatalogos = new List<string>();
                AlertaCampus = new List<string>();
                if (!IsPostBack)
                {
                    LlenaCombos();
                }
                if (Request["__EVENTTARGET"] == "SeleccionarUbicacion")
                    Seleccionar(Convert.ToInt32(Request["__EVENTARGUMENT"]));
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
                txtFiltroDecripcion.Text = string.Empty;
                SetAlias();
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    LimpiarOrganizaciones();
                    btnNew.Visible = false;
                    return;
                }
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexTodos)
                {
                    LlenaUbicaciones();
                    btnNew.Visible = false;
                }
                else if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                {
                    LlenaComboUbicacion(IdTipoUsuario);
                    LlenaUbicaciones();
                    btnNew.Visible = true;
                    //if (ddlHolding.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    //    ddlHolding_OnSelectedIndexChanged(ddlHolding, null);
                }
                else
                {
                    btnNew.Visible = false;
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
                btnNew.Visible = false;
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlCampus);
                Metodos.LimpiarCombo(ddlTorre);
                Metodos.LimpiarCombo(ddlPiso);
                Metodos.LimpiarCombo(ddlZona);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlCampus, _servicioUbicacion.ObtenerCampus(idTipoUsuario, id, true));
                LlenaUbicaciones();
                AliasUbicacion alias;
                string nivel;
                if (ddlpais.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 2);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 2";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "2";
                }
                else
                {
                    if (int.Parse(ddlTipoUsuario.SelectedValue) == (int)BusinessVariables.EnumTiposUsuario.Operador)
                    {
                        btnNew.Visible = false;
                    }
                    else
                    {
                        alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 1);
                        nivel = alias != null ? alias.Descripcion : "NIVEL 1";
                        btnNew.Visible = true;
                        btnNew.CommandName = nivel;
                        btnNew.Text = nivel;
                        btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "1";
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
                LlenaUbicaciones();
                AliasUbicacion alias;
                string nivel;
                if (ddlCampus.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 3);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 3";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "3";
                }
                else
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 2);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 2";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "2";
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
                LlenaUbicaciones();
                AliasUbicacion alias;
                string nivel;
                if (ddlTorre.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 4);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 4";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "4";
                }
                else
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 3);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 3";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "3";
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
                LlenaUbicaciones();
                AliasUbicacion alias;
                string nivel;
                if (ddlPiso.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 5);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 5";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "5";
                }
                else
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 4);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 4";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "4";
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

        protected void ddlZona_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlSubZona);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlSubZona, _servicioUbicacion.ObtenerSubZonas(idTipoUsuario, id, true));
                LlenaUbicaciones();
                AliasUbicacion alias;
                string nivel;
                if (ddlZona.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 6);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 6";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "6";
                }
                else
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 5);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 5";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "5";
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

        protected void ddlSubZona_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idTipoUsuario = IdTipoUsuario;
                int id = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                Metodos.LimpiarCombo(ddlSiteRack);
                FiltraCombo((DropDownList)sender, ddlSiteRack, _servicioUbicacion.ObtenerSiteRacks(idTipoUsuario, id, true));
                LlenaUbicaciones();
                AliasUbicacion alias;
                string nivel;
                if (ddlSubZona.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 7);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 7";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "7";
                }
                else
                {
                    alias = _servicioParametros.ObtenerAliasUbicacion(int.Parse(ddlTipoUsuario.SelectedValue)).SingleOrDefault(w => w.Nivel == 6);
                    nivel = alias != null ? alias.Descripcion : "NIVEL 6";
                    btnNew.Visible = true;
                    btnNew.CommandName = nivel;
                    btnNew.Text = nivel;
                    btnNew.CommandArgument = alias != null ? alias.Nivel.ToString() : "6";
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

        protected void ddlSiteRack_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (((DropDownList)sender).SelectedValue == "")
                    return;
                LlenaUbicaciones();
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

        protected void btnBaja_OnClick(object sender, EventArgs e)
        {
            try
            {
                _servicioUbicacion.HabilitarUbicacion(Convert.ToInt32(hfId.Value), false);
                LlenaUbicaciones();
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

        protected void btnAlta_OnClick(object sender, EventArgs e)
        {
            try
            {
                _servicioUbicacion.HabilitarUbicacion(Convert.ToInt32(hfId.Value), true);
                LlenaUbicaciones();
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

        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                int nivel = 0;
                string descripcion = null;
                Ubicacion ubicacion = _servicioUbicacion.ObtenerUbicacionById(Convert.ToInt32(hfId.Value));
                if (ubicacion == null) return;
                ddlTipoUsuario.SelectedValue = ubicacion.IdTipoUsuario.ToString();
                ddlTipoUsuario_OnSelectedIndexChanged(ddlTipoUsuario, null);
                if (ubicacion.Pais != null)
                {
                    ddlpais.SelectedValue = ubicacion.IdPais.ToString();
                    ddlpais_OnSelectedIndexChanged(ddlpais, null);
                }
                if (ubicacion.Campus != null)
                {
                    ddlCampus.SelectedValue = ubicacion.IdCampus.ToString();
                    ddlCampus_OnSelectedIndexChanged(ddlCampus, null);
                }
                if (ubicacion.Torre != null)
                {
                    ddlTorre.SelectedValue = ubicacion.IdTorre.ToString();
                    ddlTorre_OnSelectedIndexChanged(ddlTorre, null);
                }
                if (ubicacion.Piso != null)
                {
                    ddlPiso.SelectedValue = ubicacion.IdPiso.ToString();
                    ddlPiso_OnSelectedIndexChanged(ddlPiso, null);
                }
                if (ubicacion.Zona != null)
                {
                    ddlZona.SelectedValue = ubicacion.IdZona.ToString();
                    ddlZona_OnSelectedIndexChanged(ddlZona, null);
                }
                if (ubicacion.SubZona != null)
                {
                    ddlSubZona.SelectedValue = ubicacion.IdSubZona.ToString();
                    ddlSubZona_OnSelectedIndexChanged(ddlSubZona, null);
                }
                if (ubicacion.SiteRack != null)
                {
                    ddlSiteRack.SelectedValue = ubicacion.IdSiteRack.ToString();
                    ddlSiteRack_OnSelectedIndexChanged(ddlSiteRack, null);
                }

                Session["UbicacionSeleccionada"] = ubicacion;
                lblTitleCatalogo.Text = ObtenerRuta(ubicacion, ref nivel, ref descripcion);
                txtDescripcionCatalogo.Text = descripcion;
                hfCatalogo.Value = nivel.ToString();
                hfAlta.Value = false.ToString();
                ddlTipoUsuarioCatalogo.SelectedValue = ubicacion.IdTipoUsuario.ToString();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editCatalogoUbicacion\");", true);
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

        private string ObtenerRuta(Ubicacion ubicacion, ref int nivel, ref string descripcion)
        {
            string result = null;
            try
            {
                if (ubicacion.Pais != null)
                {
                    nivel = 1;
                    result = ubicacion.Pais.Descripcion;
                    descripcion = ubicacion.Pais.Descripcion;
                }
                if (ubicacion.Campus != null)
                {
                    nivel = 2;
                    result += ">" + ubicacion.Campus.Descripcion;
                    descripcion = ubicacion.Campus.Descripcion;
                }
                if (ubicacion.Torre != null)
                {
                    nivel = 3;
                    result += ">" + ubicacion.Torre.Descripcion;
                    descripcion = ubicacion.Torre.Descripcion;
                }
                if (ubicacion.Piso != null)
                {
                    nivel = 4;
                    result += ">" + ubicacion.Piso.Descripcion;
                    descripcion = ubicacion.Piso.Descripcion;
                }
                if (ubicacion.Zona != null)
                {
                    nivel = 5;
                    result += ">" + ubicacion.Zona.Descripcion;
                    descripcion = ubicacion.Zona.Descripcion;
                }
                if (ubicacion.SubZona != null)
                {
                    nivel = 6;
                    result += ">" + ubicacion.SubZona.Descripcion;
                    descripcion = ubicacion.SubZona.Descripcion;
                }
                if (ubicacion.SiteRack != null)
                {
                    nivel = 7;
                    result += ">" + ubicacion.SiteRack.Descripcion;
                    descripcion = ubicacion.SiteRack.Descripcion;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                if (sender == null) return;
                lblTitleCatalogo.Text = ObtenerRuta(btn.CommandArgument, btn.CommandName.ToUpper());
                hfCatalogo.Value = btn.CommandArgument;
                hfAlta.Value = true.ToString();
                ddlTipoUsuarioCatalogo.SelectedValue = IdTipoUsuario.ToString();
                ValidaSeleccion(btn.CommandArgument);
                if (btn.CommandArgument == "2")
                {
                    txtDescripcionCampus.Focus();
                    ddlTipoUsuarioCampus.SelectedValue = IdTipoUsuario.ToString();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editCampus\");", true);
                }
                else
                {
                    txtDescripcionCatalogo.Focus();
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

        public string ObtenerRuta(string command, string modulo)
        {
            string result = "<h3>AGREGAR " + modulo + "</h3><span style=\"font-size: x-small;\">";
            switch (command)
            {
                case "9":
                    result += ddlpais.SelectedItem.Text;
                    break;
                case "10":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text;
                    break;
                case "11":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text;
                    break;
                case "12":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text + ">" + ddlPiso.SelectedItem.Text;
                    break;
                case "13":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text + ">" + ddlPiso.SelectedItem.Text + ">" + ddlZona.SelectedItem.Text;
                    break;
                case "14":
                    result += ddlpais.SelectedItem.Text + ">" + ddlCampus.SelectedItem.Text + ">" + ddlTorre.SelectedItem.Text + ">" + ddlPiso.SelectedItem.Text + ">" + ddlZona.SelectedItem.Text + ">" + ddlSubZona.SelectedItem.Text;
                    break;
            }
            result += "</span>";
            return result;
        }

        protected void btnCrearCampus_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (Metodos.ValidaCapturaCatalogoCampus(Convert.ToInt32(ddlTipoUsuarioCampus.SelectedValue), txtDescripcionCampus.Text, ddlColonia.SelectedValue == "" ? 0 : Convert.ToInt32(ddlColonia.SelectedValue), txtCalle.Text.Trim(), txtNoExt.Text.Trim(), txtNoInt.Text.Trim()))
                {
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
                    ddlpais_OnSelectedIndexChanged(ddlpais, null);
                    ddlCampus.SelectedValue = ddlCampus.Items.FindByText(txtDescripcionCampus.Text.Trim().ToUpper()).Value;
                    ddlCampus_OnSelectedIndexChanged(ddlCampus, null);
                    LimpiaCampus();
                    LlenaUbicaciones();
                    ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCampus\");", true);
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

        protected void btnCerrar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //if (OnSeleccionUbicacionModal != null)
                //    OnSeleccionUbicacionModal();
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
                if (!Metodos.ValidaCapturaCatalogo(txtDescripcionCatalogo.Text)) return;
                Ubicacion ubicacion;
                if (Convert.ToBoolean(hfAlta.Value))
                {

                    int idTipoUsuario = IdTipoUsuario;
                    ubicacion = new Ubicacion
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
                            ddlTorre.SelectedValue = ddlTorre.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlTorre_OnSelectedIndexChanged(ddlTorre, null);
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
                            ddlPiso.SelectedValue = ddlPiso.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlPiso_OnSelectedIndexChanged(ddlPiso, null);
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
                            ddlZona.SelectedValue = ddlZona.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlZona_OnSelectedIndexChanged(ddlZona, null);
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
                            ddlSubZona.SelectedValue = ddlSubZona.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlSubZona_OnSelectedIndexChanged(ddlSubZona, null);
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
                            ddlSiteRack.SelectedValue = ddlSiteRack.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            break;
                    }

                }
                else
                {
                    ubicacion = (Ubicacion)Session["UbicacionSeleccionada"];
                    switch (int.Parse(hfCatalogo.Value))
                    {
                        case 1:
                            ubicacion.Pais.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            ddlpais_OnSelectedIndexChanged(ddlpais, null);
                            break;
                        case 2:
                            ubicacion.Campus.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            ddlpais_OnSelectedIndexChanged(ddlpais, null);
                            ddlCampus.SelectedValue = ddlCampus.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlCampus_OnSelectedIndexChanged(ddlCampus, null);
                            break;
                        case 3:
                            ubicacion.Torre.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            ddlCampus_OnSelectedIndexChanged(ddlCampus, null);
                            ddlTorre.SelectedValue = ddlTorre.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlTorre_OnSelectedIndexChanged(ddlTorre, null);
                            break;
                        case 4:
                            ubicacion.Piso.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            ddlTorre_OnSelectedIndexChanged(ddlTorre, null);
                            ddlPiso.SelectedValue = ddlPiso.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlPiso_OnSelectedIndexChanged(ddlPiso, null);
                            break;
                        case 5:
                            ubicacion.Zona.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            ddlPiso_OnSelectedIndexChanged(ddlPiso, null);
                            ddlZona.SelectedValue = ddlZona.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlZona_OnSelectedIndexChanged(ddlZona, null);
                            break;
                        case 6:
                            ubicacion.SubZona.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            ddlZona_OnSelectedIndexChanged(ddlZona, null);
                            ddlSubZona.SelectedValue = ddlSubZona.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            ddlSubZona_OnSelectedIndexChanged(ddlSubZona, null);
                            break;
                        case 7:
                            ubicacion.SiteRack.Descripcion = txtDescripcionCatalogo.Text.Trim();
                            _servicioUbicacion.ActualizarUbicacion(ubicacion);
                            ddlSubZona_OnSelectedIndexChanged(ddlSubZona, null);
                            ddlSiteRack.SelectedValue = ddlSiteRack.Items.FindByText(txtDescripcionCatalogo.Text.Trim().ToUpper()).Value;
                            break;
                    }
                }
                txtFiltroDecripcion.Text = string.Empty;
                LimpiaCatalogo();
                LlenaUbicaciones();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#editCatalogoUbicacion\");", true);
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

        private void Seleccionar(int id)
        {
            ViewState["UbicacionSeleccionada"] = id;
            //if (OnSeleccionUbicacionModal != null)
            //    OnSeleccionUbicacionModal();
        }

        #endregion Campus

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

        protected void rptResultados_OnItemCreated(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                    if (e.Item.ItemType == ListItemType.Header)
                    {
                        List<AliasUbicacion> alias = _servicioParametros.ObtenerAliasUbicacion(IdTipoUsuario);
                        if (alias.Count != 7) return;
                        ((Label)e.Item.FindControl("lblNivel1")).Text = alias.Single(s => s.Nivel == 1).Descripcion;
                        ((Label)e.Item.FindControl("lblNivel2")).Text = alias.Single(s => s.Nivel == 2).Descripcion;
                        ((Label)e.Item.FindControl("lblNivel3")).Text = alias.Single(s => s.Nivel == 3).Descripcion;
                        ((Label)e.Item.FindControl("lblNivel4")).Text = alias.Single(s => s.Nivel == 4).Descripcion;
                        ((Label)e.Item.FindControl("lblNivel5")).Text = alias.Single(s => s.Nivel == 5).Descripcion;
                        ((Label)e.Item.FindControl("lblNivel6")).Text = alias.Single(s => s.Nivel == 6).Descripcion;
                        ((Label)e.Item.FindControl("lblNivel7")).Text = alias.Single(s => s.Nivel == 7).Descripcion;
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

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                //LlenaUbicaciones();
                int? idTipoUsuario = null;
                int? idHolding = null;
                int? idCompania = null;
                int? idDireccion = null;
                int? idSubDireccion = null;
                int? idGerencia = null;
                int? idSubGerencia = null;
                int? idJefatura = null;
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

                if (ddlpais.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idHolding = int.Parse(ddlpais.SelectedValue);

                if (ddlCampus.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idCompania = int.Parse(ddlCampus.SelectedValue);

                if (ddlTorre.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idDireccion = int.Parse(ddlTorre.SelectedValue);

                if (ddlPiso.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idSubDireccion = int.Parse(ddlPiso.SelectedValue);

                if (ddlZona.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idGerencia = int.Parse(ddlZona.SelectedValue);

                if (ddlSubZona.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idSubGerencia = int.Parse(ddlSubZona.SelectedValue);

                if (ddlSiteRack.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idJefatura = int.Parse(ddlSiteRack.SelectedValue);

                rptResultados.DataSource = _servicioUbicacion.BuscarPorPalabra(idTipoUsuario, idHolding, idCompania, idDireccion, idSubDireccion, idGerencia, idSubGerencia, idJefatura, txtFiltroDecripcion.Text.Trim());
                rptResultados.DataBind();
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "HightSearch(\"tblHeader\", \"" + txtFiltroDecripcion.Text.Trim() + "\");", true);

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

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
    }
}