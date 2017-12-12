using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.Funciones;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniHelp.ServiceSistemaCatalogos;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaCatalogo : UserControl
    {
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
        private readonly ServiceCatalogosClient _servicioCatalogos = new ServiceCatalogosClient();

        private List<string> _lstError = new List<string>();

        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;

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
                List<Catalogos> lstCatalogosConsultas = _servicioCatalogos.ObtenerCatalogosMascaraCaptura(true).Where(w => !w.Sistema).ToList();
                Metodos.LlenaComboCatalogo(ddlCatalogos, lstCatalogosConsultas);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        private void LlenaCatalogoConsulta()
        {
            try
            {
                if (ddlCatalogos.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    List<CatalogoGenerico> lst = _servicioCatalogos.ObtenerRegistrosSistemaCatalogo(int.Parse(ddlCatalogos.SelectedValue), true, false).Where(w => w.Id != 0).ToList();
                    tblResults.DataSource = lst;
                    tblResults.DataBind();
                }
                //else
                //{
                //    tblResults.DataSource = null;
                //}

               
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
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
                //lblBranding.Text = WebConfigurationManager.AppSettings["Brand"];
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    LlenaCombos();
                    List<CatalogoGenerico> lst = _servicioCatalogos.ObtenerRegistrosSistemaCatalogo(1, true, false).Where(w => w.Id != 0).ToList();
                    tblResults.DataSource = lst;
                    tblResults.DataBind();
                }
                
                ucRegistroCatalogo.OnTerminarModal += AltaRegistroCatalogoOnTerminarModal;
                ucRegistroCatalogo.OnCancelarModal += AltaRegistroCatalogoOnCancelarModal;
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

        private void AltaRegistroCatalogoOnCancelarModal()
        {
            try
            {
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaRegistro\");", true);
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

        private void AltaRegistroCatalogoOnTerminarModal()
        {
            try
            {
                LlenaCatalogoConsulta();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaRegistro\");", true);
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

        protected void ddlCatalogos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LlenaCatalogoConsulta();
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
                if(ddlCatalogos.SelectedIndex == BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                    throw new Exception("Seleccione un catalogo");
                ucRegistroCatalogo.EsAlta = true;
                ucRegistroCatalogo.IdCatalogo = int.Parse(ddlCatalogos.SelectedValue);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaRegistro\");", true);
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
                ucRegistroCatalogo.EsAlta = false;
                ucRegistroCatalogo.IdCatalogo = int.Parse(ddlCatalogos.SelectedValue);
                ucRegistroCatalogo.IdRegistro = int.Parse(((ImageButton)sender).CommandArgument);
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaRegistro\");", true);
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
                _servicioCatalogos.HabilitarRegistroSistema(int.Parse(ddlCatalogos.SelectedValue), ((CheckBox)sender).Checked, int.Parse(((CheckBox)sender).Attributes["data-id"]));
                LlenaCatalogoConsulta();
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

        protected void btnDownload_OnClick(object sender, EventArgs e)
        {
            try
            {
                List<CatalogoGenerico> lstRegistros = null;
                if (ddlCatalogos.SelectedIndex > BusinessVariables.ComboBoxCatalogo.IndexSeleccione)
                {
                    lstRegistros = _servicioCatalogos.ObtenerRegistrosSistemaCatalogo(int.Parse(ddlCatalogos.SelectedValue), true, false).Where(w => w.Id != 0).ToList();
                    tblResults.DataSource = lstRegistros;
                }
                if (lstRegistros == null)
                    throw new Exception("Seleccione un catálogo.");
                Response.Clear();
                string ultimaEdicion = "Últ. edición";
                MemoryStream ms =
                    new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstRegistros.Select(
                                s => new
                                {
                                    Nombre = s.Descripcion
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Catalgos.xlsx");
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

        #region Paginador
        protected void gvPaginacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            tblResults.PageIndex = e.NewPageIndex;
            LlenaCatalogoConsulta();
        }

        #endregion 
    }
}
