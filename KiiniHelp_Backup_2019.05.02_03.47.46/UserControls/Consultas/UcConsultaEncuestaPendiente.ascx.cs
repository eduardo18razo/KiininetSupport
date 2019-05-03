using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.ServiceEncuesta;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniHelp.UserControls.Consultas
{
    public partial class UcConsultaEncuestaPendiente : System.Web.UI.UserControl
    {
        private readonly ServiceEncuestaClient _servicioEncuestas = new ServiceEncuestaClient();
        private List<string> _lstError = new List<string>();
        public List<string> Alerta
        {
            set
            {
                panelAlertaGeneral.Visible = value.Any();
                if (!panelAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }

        private void ObtenerEncuestasPendientes()
        {
            try
            {
                List<HelperEncuesta> lst = _servicioEncuestas.ObtenerEncuestasPendientesUsuario(((Usuario)Session["UserData"]).Id);
                if (lst != null)
                {
                    rptEncuestasPendientes.DataSource = lst;
                    rptEncuestasPendientes.DataBind();
                }
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
                Alerta = new List<string>();
                if (!IsPostBack)
                {
                    ObtenerEncuestasPendientes();
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