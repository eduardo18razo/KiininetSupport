using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceDomicilioSistema : IServiceDomicilioSistema
    {
        public List<Colonia> ObtenerColoniasCp(int cp, bool insertarSeleccion)
        {
            try
            {
                using (BusinessDomicilioSistema negocio = new BusinessDomicilioSistema())
                {
                    return negocio.ObtenerColoniasCp(cp, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Colonia ObtenerDetalleColonia(int idColonia)
        {
            try
            {
                using (BusinessDomicilioSistema negocio = new BusinessDomicilioSistema())
                {
                    return negocio.ObtenerDetalleColonia(idColonia);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
