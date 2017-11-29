
using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoTelefono : IServiceTipoTelefono
    {

        public List<TipoTelefono> ObtenerTiposTelefono(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoTelefono negocio = new BusinessTipoTelefono())
                {
                    return negocio.ObtenerTiposTelefono(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
