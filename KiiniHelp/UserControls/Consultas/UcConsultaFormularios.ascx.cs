using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceMascaraAcceso;
using KiiniNet.Entities.Cat.Mascaras;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaFormularios : UserControl
    {
        private readonly ServiceMascarasClient _servicioMascaras = new ServiceMascarasClient();
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

        private void LlenaMascaras()
        {
            try
            {
                string descripcion = txtFiltro.Text.ToLower().Trim();

                tblResults.DataSource = _servicioMascaras.Consulta(descripcion);
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
                LlenaMascaras();                
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
                Response.Redirect("~/Users/Administracion/Formularios/FrmEdicionFormulario.aspx");
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
                LlenaMascaras();
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
                string descripcion = txtFiltro.Text.Trim();
                List<Mascara> lstcatalogos = _servicioMascaras.Consulta(descripcion);

                Response.Clear();
                string ultimaEdicion = "Últ. edición";
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
                Response.AddHeader("content-disposition", "attachment;  filename=Formularios.xlsx");
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
                _servicioMascaras.HabilitarMascara(int.Parse(((CheckBox)sender).Attributes["data-id"]), ((CheckBox)sender).Checked);
                LlenaMascaras();
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
                Response.Redirect("~/Users/Administracion/Formularios/FrmEdicionFormulario.aspx?Alta=false&idFormulario=" + ((ImageButton)sender).CommandArgument);
                //AltaInformacionConsulta.GrupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(Convert.ToInt32(hfId.Value));
                //ucAltaGrupoUsuario.Alta = false;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaGrupoUsuarios\");", true);
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
        protected void OnClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Users/Administracion/Formularios/FrmEdicionFormulario.aspx?idFormulario=" + ((LinkButton)sender).CommandArgument + "&Alta=true");
                //AltaInformacionConsulta.GrupoUsuario = _servicioGrupoUsuario.ObtenerGrupoUsuarioById(Convert.ToInt32(hfId.Value));
                //ucAltaGrupoUsuario.Alta = false;
                //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "Script", "MostrarPopup(\"#modalAltaGrupoUsuarios\");", true);
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
            LlenaMascaras();
        }

        #endregion 


    }
}