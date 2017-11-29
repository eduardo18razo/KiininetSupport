using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using KiiniHelp.Funciones;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Genericos
{
    public partial class UcMostrarArchivo : UserControl
    {
        private List<string> _lstError = new List<string>();

        public string NombreDocumento
        {
            get { return hfFile.Value; }
            set { hfFile.Value = value; }
        }

        public int TipoInformacion
        {
            get { return Convert.ToInt32(hfTipoInformacion.Value); }
            set { hfTipoInformacion.Value = value.ToString(); }
        }

        private List<string> AlertaGeneral
        {
            set
            {
                pnlAlertaGeneral.Visible = value.Any();
                if (!pnlAlertaGeneral.Visible) return;
                rptErrorGeneral.DataSource = value;
                rptErrorGeneral.DataBind();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string nombreDocto = Request.QueryString["NombreDocumento"];
                int tipoInformacion = Convert.ToInt32(Request.QueryString["TipoDocumento"]);
                string directorio = Server.MapPath("~/General");
                if (!IsPostBack)
                {
                    switch (tipoInformacion)
                    {
                        case (int)BusinessVariables.EnumTiposDocumento.Word:
                            Documentos.MostrarDocumento(nombreDocto, Page, directorio);
                            break;
                        case (int)BusinessVariables.EnumTiposDocumento.PowerPoint:
                            Documentos.MostrarDocumento(nombreDocto, Page, directorio);
                            break;
                        case (int)BusinessVariables.EnumTiposDocumento.Excel:
                            Documentos.MostrarDocumento(nombreDocto, Page, directorio);
                            break;
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
                AlertaGeneral = _lstError;
            }
        }
    }
}