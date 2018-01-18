using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceInformacionConsulta;
using KiiniHelp.ServiceSistemaTipoInformacionConsulta;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaInformacionConsulta : UserControl
    {
        private readonly ServiceTipoInfConsultaClient _servicioSistemaTipoInformacion = new ServiceTipoInfConsultaClient();
        private readonly ServiceInformacionConsultaClient _servicioInformacionConsulta = new ServiceInformacionConsultaClient();

        private List<string> _lstError = new List<string>();
        
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
        
        private void LlenaInformacionConsulta()
        {
            try
            {
                string filtro = txtFiltro.Text;
                tblResults.DataSource = _servicioInformacionConsulta.ObtenerConsulta(filtro);
                tblResults.DataBind();
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

                LlenaInformacionConsulta();
                Alerta = new List<string>();
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

                Response.Redirect("~/Users/Administracion/InformaciondeConsulta/FrmAltaInfConsulta.aspx?IdInformacionConsulta=" + ((LinkButton)sender).CommandArgument);               
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
                Response.Redirect("~/Users/Administracion/InformaciondeConsulta/FrmAltaInfConsulta.aspx");
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
                LlenaInformacionConsulta();
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
                _servicioInformacionConsulta.HabilitarInformacion(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                LlenaInformacionConsulta();
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
                string filtro = txtFiltro.Text.Trim().ToUpper();
                List<InformacionConsulta> lstInformacion = _servicioInformacionConsulta.ObtenerConsulta(filtro);

                Response.Clear();
                string ultimaEdicion = "Últ. edición";
                MemoryStream ms = new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstInformacion.Select(
                                s => new
                                {
                                    Nombre = s.Descripcion,
                                    Creación = s.FechaAlta.ToShortDateString().ToString(),
                                    ultimaEdicion = s.FechaModificacion == null ? "" : s.FechaModificacion.Value.ToShortDateString().ToString(),
                                    Habilitado = s.Habilitado ? "Si" : "No"
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Articulos.xlsx");
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
            LlenaInformacionConsulta();
        }

        #endregion 


    }
}