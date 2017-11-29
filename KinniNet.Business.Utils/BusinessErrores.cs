using KiiniNet.Entities.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace KinniNet.Business.Utils
{
    public static class BusinessErrores
    {                 //nuevo
        private readonly static string fileNotificaciones = ConfigurationManager.AppSettings["ArchivoNotificaciones"];
        public static void GenerarArchivoErrores()
        {
            try
            {
                if (File.Exists(fileNotificaciones))
                    File.Delete(fileNotificaciones);
                List<HelperMensajes> lst = new List<HelperMensajes>();
                lst.Add(new HelperMensajes { Key = "Exito", Value = "Se guardó el registro exitosamente." });
                lst.Add(new HelperMensajes { Key = "Error", Value = "No se puede guardar el registro." });
                lst.Add(new HelperMensajes { Key = "FaltaTipoUsuario", Value = "Debe seleccionar un Tipo de Usuario" });
                lst.Add(new HelperMensajes { Key = "FaltaDescripcion", Value = "Debe especificar una descripción." });
                lst.Add(new HelperMensajes { Key = "Actualizacion", Value = "Se actualizó el registro exitosamente." });    

                XmlSerializer ser = new XmlSerializer(typeof(List<HelperMensajes>));
                XmlSerializer writer = new XmlSerializer(typeof(List<HelperMensajes>));
                using (FileStream file = File.OpenWrite(fileNotificaciones))
                {
                    writer.Serialize(file, lst);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static List<HelperMensajes> DeserializeFile()
        {
            List<HelperMensajes> result;
            try
            {
                XmlSerializer reader = new XmlSerializer(typeof(List<HelperMensajes>));
                using (FileStream input = File.OpenRead(fileNotificaciones))
                {
                    result = (List<HelperMensajes>)reader.Deserialize(input);
                }
            }
            catch (Exception ex)
            {
                GenerarArchivoErrores();
                XmlSerializer reader = new XmlSerializer(typeof(List<HelperMensajes>));
                using (FileStream input = File.OpenRead(fileNotificaciones))
                {
                    result = (List<HelperMensajes>)reader.Deserialize(input);
                }
            }
            return result;
        }

        public static string ObtenerMensajeByKey(Utils.BusinessVariables.EnumMensajes key)
        {
            string result = string.Empty;
            try
            {

                HelperMensajes mensaje = DeserializeFile().SingleOrDefault(s => s.Key == key.ToString());
                if (mensaje != null)
                    result = mensaje.Value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

    }
}
