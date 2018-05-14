using System;
using System.Net;
using System.Web;
using System.Web.UI;
using KinniNet.Business.Utils;

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
                    //if (BusinessFile.ExisteArchivo(Server.MapPath(values[1])))
                    //{
                        WebClient req = new WebClient();
                        HttpResponse response = HttpContext.Current.Response;
                        response.Clear();
                        response.ClearContent();
                        response.ClearHeaders();
                        response.Buffer = true;
                        response.AddHeader("Content-Type", "binary/octet-stream");
                        response.AddHeader("Content-Disposition", "attachment;filename=\"" + values[1] + "\"");
                        byte[] data = req.DownloadData(values[0] + values[1]);
                        response.BinaryWrite(data);
                        response.End();
                    //}
                }

            }
        }
    }
}