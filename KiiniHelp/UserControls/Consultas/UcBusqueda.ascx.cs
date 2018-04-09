using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using KiiniHelp.ServiceArbolAcceso;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcBusqueda : UserControl
    {
        private const int PageSize = 3;
        private readonly ServiceArbolAccesoClient _servicioArbol = new ServiceArbolAccesoClient();

        private List<string> _lstError = new List<string>();
        private void LlenaResultados(string filter, List<int> tiposUsuario, int page)
        {
            try
            {
                List<HelperBusquedaArbolAcceso> result = _servicioArbol.BusquedaGeneral(Session["UserData"] == null ? null : (int?)((Usuario)Session["UserData"]).Id, filter, tiposUsuario, page, PageSize);
                rptResults.DataSource = _servicioArbol.BusquedaGeneral(Session["UserData"] == null ? null : (int?)((Usuario)Session["UserData"]).Id, filter, tiposUsuario, page, PageSize);
                rptResults.DataBind();
                List<int> pages = new List<int>();
                for (int i = 1; i <= result.Average(a => a.TotalPage); i++)
                {
                    pages.Add(i);
                }
                rptPager.DataSource = pages;
                rptPager.DataBind();
                lblNumeroResultados.Text = string.Format("{0} resultados para campo búsqueda", result.Count);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Alerta = new List<string>();
                if (Request.Params["w"] != null && !string.IsNullOrEmpty(Request.Params["w"]))
                {
                    List<int> tiposUsuario = null;
                    if (Request.Params["tu"] != null)
                    {
                        tiposUsuario = new List<int>();
                        tiposUsuario.Add(int.Parse(Request.Params["tu"]));
                    }
                    LlenaResultados(Request.Params["w"], tiposUsuario, 1);
                }
                else
                {
                    string urlName = Request.UrlReferrer.ToString();
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

        protected void lnkPage_OnClick(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                if (btn != null)
                {
                    List<int> tiposUsuario = null;
                    if (Request.Params["tu"] != null)
                    {
                        tiposUsuario = new List<int>();
                        tiposUsuario.Add(int.Parse(Request.Params["tu"]));
                    }
                    LlenaResultados(Request.Params["w"], tiposUsuario, int.Parse(btn.CommandArgument));
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