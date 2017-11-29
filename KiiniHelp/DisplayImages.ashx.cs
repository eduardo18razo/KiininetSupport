using System;
using System.IO;
using System.Web;
using KiiniHelp.ServiceUsuario;

namespace KiiniHelp
{
    /// <summary>
    /// Descripción breve de DisplayImages
    /// </summary>
    public class DisplayImages : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Int32 idUsuario;
            if (context.Request.QueryString["id"] != null)
                idUsuario = Convert.ToInt32(context.Request.QueryString["id"]);
            else
                throw new ArgumentException("No parameter specified");

            context.Response.ContentType = "image/jpeg";

            Stream strm = new MemoryStream(new ServiceUsuariosClient().ObtenerFoto(idUsuario));
            byte[] buffer = new byte[4096];
            int byteSeq = strm.Read(buffer, 0, 4096);

            while (byteSeq > 0)
            {
                context.Response.OutputStream.Write(buffer, 0, byteSeq);
                byteSeq = strm.Read(buffer, 0, 4096);
            }
            //context.Response.BinaryWrite(buffer);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}