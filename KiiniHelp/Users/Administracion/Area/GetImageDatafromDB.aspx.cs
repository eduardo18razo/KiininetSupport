using System;
using KiiniHelp.ServiceArea;

namespace KiiniHelp.Users.Administracion.Area
{
    public partial class GetImageDatafromDB : System.Web.UI.Page
    {
        private readonly ServiceArea.ServiceAreaClient _servicioArea = new ServiceAreaClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sProductID = Request.QueryString["id"];
                KiiniNet.Entities.Operacion.Area area = _servicioArea.ObtenerAreaById(int.Parse(sProductID));
                Byte[] bytes = area.Imagen;
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                Image1.ImageUrl = "data:image/jpg;base64," + base64String;
                Image1.Visible = true;
                Response.ContentType = "image/jpg";
                Response.OutputStream.Write(area.Imagen, 0, area.Imagen.Length);
                //byte[] bytes = area.Imagen;
                //string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                //Image1.ImageUrl = "data:image/jpg;base64," + base64String;
                ////string sProductID = Request.QueryString["id"];
                ////KiiniNet.Entities.Operacion.Area area = _servicioArea.ObtenerAreaById(int.Parse(sProductID));
                //Response.BinaryWrite(area.Imagen);

                //Response.End();
            }
        }
    }
}