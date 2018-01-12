using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceDiasHorario;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaHorario : UserControl
    {
        private readonly ServiceDiasHorarioClient _servicioHorarios = new ServiceDiasHorarioClient();

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


        private void LlenaHorariosConsulta()
        {
            try
            {
                tblResults.DataSource = _servicioHorarios.ObtenerHorarioConsulta(txtFiltro.Text.Trim());
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
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    
                }

                LlenaHorariosConsulta();

                ucAltaHorario.OnAceptarModal += AltaHorarioOnAceptarModal;
                ucAltaHorario.OnCancelarModal += AltaHorarioOnCancelarModal;
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

        private void AltaHorarioOnCancelarModal()
        {
            try
            {
                LlenaHorariosConsulta();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaHorario\");", true);
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

        private void AltaHorarioOnAceptarModal()
        {
            try
            {
                LlenaHorariosConsulta();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "CierraPopup(\"#modalAltaHorario\");", true);
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
                ucAltaHorario.EsAlta = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaHorario\");", true);
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
                LlenaHorariosConsulta();
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
                List<Horario> lstcatalogos = _servicioHorarios.ObtenerHorarioConsulta(filtro);
                if (filtro != string.Empty)
                    lstcatalogos = lstcatalogos.Where(w => w.Descripcion.Contains(filtro)).ToList();

                Response.Clear();
                string ultimaEdicion = "Últ. Edición";
                MemoryStream ms =
                    new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstcatalogos.Select(
                                s => new
                                {
                                    Nombre = s.Descripcion,
                                    Creación = s.FechaAlta.ToShortDateString().ToString(),
                                    ultimaEdicion = s.FechaModificacion == null ? "" : s.FechaModificacion.Value.ToShortDateString().ToString(),
                                    Habilitado = s.Habilitado ? "Si" : "No"
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Horarios.xlsx");
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

        protected void OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                _servicioHorarios.HabilitarHorario(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                LlenaHorariosConsulta();
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
                ucAltaHorario.IdHorario = int.Parse(((LinkButton)sender).CommandArgument);
                ucAltaHorario.EsAlta = false;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaHorario\");", true);
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

        protected void btnClonar_OnClick(object sender, EventArgs e)
        {
            try
            {
                ucAltaHorario.IdHorario = int.Parse(((LinkButton)sender).CommandArgument);
                ucAltaHorario.EsAlta = true;
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaHorario\");", true);
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
            LlenaHorariosConsulta();
        }

        #endregion 

    }
}