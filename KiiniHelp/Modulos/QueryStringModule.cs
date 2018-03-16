#region Using

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

#endregion

namespace KiiniHelp.Modulos
{
    /// <summary>
    /// Summary description for QueryStringModule
    /// </summary>
    public class QueryStringModule : IHttpModule
    {
        ///
        /// variables const
        ///
        private const string nombreParametro = "q=";
        //private const string nombreParametro;
        private const string Key = "knnet";

        ///
        /// Salt para "reforzar" encriptado
        ///
        private readonly static byte[] salt = Encoding.ASCII.GetBytes(Key.Length.ToString());

        ///
        ///
        ///
        public QueryStringModule()
        {
            //Algún día necesitaré esto lol
        }

        ///
        /// Implementamos Dispose de interfaz
        ///
        public void Dispose()
        {
            //Nothing
        }

        ///
        /// Implementamos Init de Interfaz
        /// Con esto construimos un nuevo evento para manejar la petición
        ///
        ///
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        ///
        /// Evento que maneja la petición
        ///
        ///
        ///
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current; //Contexto http actual
            if (context.Request.Url.OriginalString.Contains("AsyncFileUploadID"))
            {
                return;
                //context.Request.Url.OriginalString.IndexOf("AsyncFileUploadID");
            }
            if (context.Request.Url.OriginalString.Contains("aspx") && context.Request.RawUrl.Contains("?")) //url contiene ".aspx" && "?" ?
            {
                string query = ExtraerCadena(context.Request.RawUrl);
                
                string ruta = ObtenerRutaVirtual();

                if (query.StartsWith(nombreParametro, StringComparison.OrdinalIgnoreCase))
                {
                    // Desencripta queryString y se vuelve a establecer la ruta
                    string rawQuery = query.Replace(nombreParametro, string.Empty);
                    string decryptedQuery = Decrypt(rawQuery);
                    context.RewritePath(ruta, string.Empty, decryptedQuery);
                }
                else if (context.Request.HttpMethod == "GET")
                {
                    // Encripta queryString y reedirecciona a la url encriptada
                    //Encripta todas las queryString automáticamente. ****Eliminar esta parte si no se desea encriptar automáticamente y realizar otras acciones
                    string encryptedQuery = Encrypt(query);
                    context.Response.Redirect(ruta + encryptedQuery);
                }
            }
        }

        ///
        /// Analiza la url actual y extrae la ruta virtual sin usar la queryString
        ///
        /// Ruta virtual de la url actual
        private static string ObtenerRutaVirtual()
        {
            string ruta = HttpContext.Current.Request.RawUrl;
            ruta = ruta.Substring(0, ruta.IndexOf("?"));
            ruta = ruta.Substring(ruta.LastIndexOf("/") + 1);
            return ruta;
        }

        ///
        /// Analiza la url y regresa la queryString
        ///
        ///url a analizar
        /// QueryString sin signo "?"
        private static string ExtraerCadena(string url)
        {
            int indice = url.IndexOf("?") + 1;
            return url.Substring(indice);
        }

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return clearText;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return "?" + nombreParametro + clearText;
        }

        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(Key, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                if (encryptor == null) return cipherText;
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
