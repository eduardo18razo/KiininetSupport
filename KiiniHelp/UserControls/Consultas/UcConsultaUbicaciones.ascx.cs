using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceParametrosSistema;
using KiiniHelp.ServiceSistemaTipoUsuario;
using KiiniHelp.ServiceUbicacion;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using System.IO;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaUbicaciones : UserControl, IControllerModal
    {
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;

        private readonly ServiceTipoUsuarioClient _servicioSistemaTipoUsuario = new ServiceTipoUsuarioClient();
        private readonly ServiceUbicacionClient _servicioUbicacion = new ServiceUbicacionClient();
        private readonly ServiceParametrosClient _servicioParametros = new ServiceParametrosClient();


        private List<string> _lstError = new List<string>();

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

        public List<string> Alerta
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

        private void LlenaCombos()
        {
            try
            {
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true, true);
                Metodos.LlenaComboCatalogo(ddlTipoUsuario, lstTipoUsuario);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LlenaUbicaciones()
        {
            try
            {
                int? idTipoUsuario = null;
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

                List<Ubicacion> lstUbicaciones = _servicioUbicacion.BuscarPorPalabra(idTipoUsuario, null, null, null, null, null, null, null, txtFiltroDecripcion.Text.Trim(), true);
                tblResults.DataSource = lstUbicaciones;
                tblResults.DataBind();
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
                Alerta = new List<string>();
                ucAltaUbicaciones.OnAceptarModal += UcAltaUbicacionesOnOnAceptarModal;
                ucAltaUbicaciones.OnCancelarModal += UcAltaUbicacionesOnOnCancelarModal;
                ucAltaUbicaciones.OnTerminarModal += ucAltaUbicaciones_OnTerminarModal;
                if (!IsPostBack)
                {
                    LlenaCombos();
                    LlenaUbicaciones();
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

        private void UcAltaUbicacionesOnOnAceptarModal()
        {
            try
            {
                LlenaUbicaciones();
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

        private void UcAltaUbicacionesOnOnCancelarModal()
        {
            try
            {
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
                Alerta = _lstError;
            }
        }

        private void ucAltaUbicaciones_OnTerminarModal()
        {
            try
            {
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
                Alerta = _lstError;
            }
        }

        protected void ddlTipoUsuario_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    LlenaUbicaciones();
                }
                else if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    if (IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Operador || IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Cliente || IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Proveedor || IdTipoUsuario == (int)BusinessVariables.EnumTiposUsuario.Empleado)
                        LlenaUbicaciones();
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

        protected void btnEditar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucAltaUbicaciones.IdUbicacion = int.Parse(((LinkButton)sender).CommandArgument);
                ucAltaUbicaciones.EsSeleccion = false;
                ucAltaUbicaciones.EsAlta = false;
                ucAltaUbicaciones.Title = "Editar Ubicación";
                ucAltaUbicaciones.SetUbicacionActualizar();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editCatalogoUbicacion\");", true);
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

        protected void btnNew_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucAltaUbicaciones.EsSeleccion = false;
                ucAltaUbicaciones.EsAlta = true;
                ucAltaUbicaciones.Title = "Nueva Ubicación";
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#editCatalogoUbicacion\");", true);
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

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                LlenaUbicaciones();
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

        protected void OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _servicioUbicacion.HabilitarUbicacion(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
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
            finally { LlenaUbicaciones(); }
        }

        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            try
            {
                int? idTipoUsuario = null;
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

                List<Ubicacion> lstUbicaciones = _servicioUbicacion.ObtenerUbicaciones(idTipoUsuario, null, null, null, null, null, null, null);

                Response.Clear();
                MemoryStream ms = new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstUbicaciones.Select(
                                s => new
                                {
                                    TipoUsuario = s.TipoUsuario.Descripcion,
                                    Pais = s.Pais.Descripcion,
                                    Campus = s.Campus != null ? s.Campus.Descripcion : "",
                                    Torre = s.Torre != null ? s.Torre.Descripcion : "",
                                    Piso = s.Piso != null ? s.Piso.Descripcion : "",
                                    Zona = s.Zona != null ? s.Zona.Descripcion : "",
                                    SubZona = s.SubZona != null ? s.SubZona.Descripcion : "",
                                    SiteRack = s.SiteRack != null ? s.SiteRack.Descripcion : "",
                                    Sistema = s.Sistema ? "Si" : "No",
                                    Habilitado = s.Habilitado ? "Si" : "No"
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Ubicaciones.xlsx");
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
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

        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblResults.PageIndex = e.NewPageIndex;
            LlenaUbicaciones();
        }


        protected void tblResults_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    if (e.Row.RowType == DataControlRowType.Header)
                    {
                        List<AliasUbicacion> alias = _servicioParametros.ObtenerAliasUbicacion(IdTipoUsuario);
                        if (alias.Count != 7) return;
                        e.Row.Cells[1].Text = alias.Single(s => s.Nivel == 1).Descripcion;
                        e.Row.Cells[2].Text = string.Format("Domicilio <br>{0}", alias.Single(s => s.Nivel == 2).Descripcion);
                        e.Row.Cells[3].Text = alias.Single(s => s.Nivel == 3).Descripcion;
                        e.Row.Cells[4].Text = alias.Single(s => s.Nivel == 4).Descripcion;
                        e.Row.Cells[5].Text = alias.Single(s => s.Nivel == 5).Descripcion;
                        e.Row.Cells[6].Text = alias.Single(s => s.Nivel == 6).Descripcion;
                        e.Row.Cells[7].Text = alias.Single(s => s.Nivel == 7).Descripcion;
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
    }
}