using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceNotificacion : IServiceNotificacion
    {
        public List<TipoNotificacion> ObtenerTipos(bool insertarSeleccion)
        {
            try
            {
                
                using (BusinessNotificacion negocio = new BusinessNotificacion())
                {
                    return negocio.ObtenerTipos(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
