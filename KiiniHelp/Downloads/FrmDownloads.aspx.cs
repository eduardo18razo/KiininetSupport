using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI;
using KinniNet.Business.Utils;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace KiiniHelp.Downloads
{
    public partial class FrmDownloads : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Params["file"] != null)
                {

                    string[] values = Request.Params["file"].Split('~');
                    FileInfo file = new FileInfo(values[0] + values[1]);
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name.Replace(" ", string.Empty));
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.Flush();
                        Response.TransmitFile(file.FullName);
                        Response.End();
                    }
                }

            }
        }
    }
}