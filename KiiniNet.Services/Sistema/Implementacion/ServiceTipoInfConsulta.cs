using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoInfConsulta : IServiceTipoInfConsulta
    {
        public List<TipoInfConsulta> ObtenerTipoInformacionConsulta(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoInfConsulta negocio = new BusinessTipoInfConsulta())
                {
                    return negocio.ObtenerTipoInformacionConsulta(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
