using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceGrupoUsuario;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Detalles
{
    public partial class UcDetalleGrupoOpciones : UserControl, IControllerModal
    {
        private readonly ServiceGrupoUsuarioClient _servicioGrupos = new ServiceGrupoUsuarioClient();
        private readonly ServiceArbolAccesoClient _servicioArbol = new ServiceArbolAccesoClient();
        private List<string> _lstError = new List<string>();
        
        public event DelegateAceptarModal OnAceptarModal;
        public event DelegateLimpiarModal OnLimpiarModal;
        public event DelegateCancelarModal OnCancelarModal;
        public event DelegateTerminarModal OnTerminarModal;
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
        public int IdGrupo
        {
            get { return int.Parse(hfIdGrupo.Value); }
            set
            {
                try
                {
                    hfIdGrupo.Value = value.ToString();
                    GrupoUsuario grupo = _servicioGrupos.ObtenerGrupoUsuarioById(value);
                    if (grupo != null)
                    {
                        lblTipoGrupo.Text = grupo.TipoGrupo.Descripcion;
                        lblNombreGrupo.Text = grupo.Descripcion;
                        LlenaOpciones();
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

        private void LlenaOpciones()
        {
            try
            {

                tblResults.DataSource = _servicioArbol.ObtenerArbolesAccesoTerminalByGrupoUsuario(IdGrupo);
                tblResults.DataBind();
                ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "ScriptTable", "hidden();", true);
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
            try
            {
                tblResults.PageIndex = e.NewPageIndex;
                LlenaOpciones();
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

        #endregion

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            try
            {
                if (OnCancelarModal != null)
                    OnCancelarModal();
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
                List<HelperArbolAcceso> lstcatalogos = _servicioArbol.ObtenerArbolesAccesoTerminalByGrupoUsuario(IdGrupo);

                Response.Clear();
                MemoryStream ms =
                    new MemoryStream(BusinessFile.ExcelManager.ListToExcel(lstcatalogos.Select(
                                s => new
                                {
                                    TU = s.TipoUsuario,
                                    s.Titulo,
                                    s.Categoria,
                                    s.DescripcionTipificacion,
                                    s.Tipo,
                                    s.Nivel,
                                    s.Activo
                                })
                                .ToList()).GetAsByteArray());
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=DettalleGrupoOpciones.xlsx");
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

        protected void lnkBtnDetalleOpcion_OnClick(object sender, EventArgs e)
        {
            try
            {

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