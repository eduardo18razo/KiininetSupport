using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Configuration;
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
                List<TipoUsuario> lstTipoUsuario = _servicioSistemaTipoUsuario.ObtenerTiposUsuarioResidentes(true);
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

                List<Ubicacion> lstUbicaciones = _servicioUbicacion.ObtenerUbicaciones(idTipoUsuario, null, null, null, null, null, null, null);
                tblResults.DataSource = lstUbicaciones;
                tblResults.DataBind();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LimpiarUbicaciones()
        {
            try
            {
                tblResults.DataSource = null;
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
                txtFiltroDecripcion.Text = string.Empty;
                //if (ddlTipoUsuario.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                //{
                //    LimpiarUbicaciones();
                //    return;
                //}
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

        protected void rptResultados_OnItemCreated(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
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
                Alerta = _lstError;
            }
        }

        protected void btnBuscar_OnClick(object sender, EventArgs e)
        {
            try
            {
                int? idTipoUsuario = null;
                if (ddlTipoUsuario.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexTodos)
                    idTipoUsuario = int.Parse(ddlTipoUsuario.SelectedValue);

                tblResults.DataSource = _servicioUbicacion.BuscarPorPalabra(idTipoUsuario, null, null, null, null, null, null, null, txtFiltroDecripcion.Text.Trim());
                tblResults.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "HightSearch(\"tblHeader\", \"" + txtFiltroDecripcion.Text.Trim() + "\");", true);
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


        //#region Eventos paginador

        //protected void lbFirst_Click(object sender, EventArgs e)
        //{
        //    CurrentPage = 0;
        //    LlenaUbicaciones();
        //    //BindDataIntoRepeater();
        //}
        //protected void lbLast_Click(object sender, EventArgs e)
        //{
        //    CurrentPage = (Convert.ToInt32(ViewState["TotalPages"]) - 1);
        //    LlenaUbicaciones();
        //    //BindDataIntoRepeater();

        //}
        //protected void lbPrevious_Click(object sender, EventArgs e)
        //{
        //    CurrentPage -= 1;
        //    LlenaUbicaciones();
        //    //BindDataIntoRepeater();
        //}
        //protected void lbNext_Click(object sender, EventArgs e)
        //{
        //    CurrentPage += 1;
        //    LlenaUbicaciones();
        //    //BindDataIntoRepeater();
        //}

        //protected void rptPaging_ItemCommand(object source, DataListCommandEventArgs e)
        //{
        //    if (!e.CommandName.Equals("newPage")) return;
        //    CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
        //    LlenaUbicaciones();
        //    //BindDataIntoRepeater();
        //}

        //protected void rptPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        //{
        //    var lnkPage = (LinkButton)e.Item.FindControl("lbPaging");
        //    if (lnkPage.CommandArgument != CurrentPage.ToString()) return;
        //    lnkPage.Enabled = false;
        //    //lnkPage.BackColor = Color.FromName("#00FF00");
        //}


        //static DataTable GetDataFromDb()
        //{
        //    var con = new SqlConnection(ConfigurationManager.ConnectionStrings["stringConnection"].ToString());
        //    con.Open();
        //    var da = new SqlDataAdapter("Select Id, Name, Address, CreatedDate From tblPerson Order By Id desc", con);
        //    var dt = new DataTable();
        //    da.Fill(dt);
        //    con.Close();
        //    return dt;
        //}


        //private void BindDataIntoRepeater()
        //{
        //    var dt = GetDataFromDb();
        //    _pgsource.DataSource = dt.DefaultView;
        //    _pgsource.AllowPaging = true;
        //    // Number of items to be displayed in the Repeater
        //    _pgsource.PageSize = _pageSize;
        //    _pgsource.CurrentPageIndex = CurrentPage;
        //    // Keep the Total pages in View State
        //    ViewState["TotalPages"] = _pgsource.PageCount;
        //    // Example: "Page 1 of 10"
        //    lblpage.Text = "Page " + (CurrentPage + 1) + " of " + _pgsource.PageCount;
        //    // Enable First, Last, Previous, Next buttons
        //    lbPrevious.Enabled = !_pgsource.IsFirstPage;
        //    lbNext.Enabled = !_pgsource.IsLastPage;
        //    lbFirst.Enabled = !_pgsource.IsFirstPage;
        //    lbLast.Enabled = !_pgsource.IsLastPage;

        //    // Bind data into repeater
        //    LlenaUbicaciones();

        //    //rptResult.DataSource = _pgsource;
        //    //rptResult.DataBind();

        //    // Call the function to do paging
        //    HandlePaging();
        //}


        //private void HandlePaging()
        //{
        //    var dt = new DataTable();
        //    dt.Columns.Add("PageIndex"); //Start from 0
        //    dt.Columns.Add("PageText"); //Start from 1

        //    _firstIndex = CurrentPage - 5;
        //    if (CurrentPage > 5)
        //        _lastIndex = CurrentPage + 5;
        //    else
        //        _lastIndex = 10;

        //    // Check last page is greater than total page then reduced it to total no. of page is last index
        //    if (_lastIndex > Convert.ToInt32(ViewState["TotalPages"]))
        //    {
        //        _lastIndex = Convert.ToInt32(ViewState["TotalPages"]);
        //        _firstIndex = _lastIndex - 10;
        //    }

        //    if (_firstIndex < 0)
        //        _firstIndex = 0;

        //    // Now creating page number based on above first and last page index
        //    for (var i = _firstIndex; i < _lastIndex; i++)
        //    {
        //        var dr = dt.NewRow();
        //        dr[0] = i;
        //        dr[1] = i + 1;
        //        dt.Rows.Add(dr);
        //    }

        //    rptPaging.DataSource = dt;
        //    rptPaging.DataBind();
        //}





        //#endregion




    }
}