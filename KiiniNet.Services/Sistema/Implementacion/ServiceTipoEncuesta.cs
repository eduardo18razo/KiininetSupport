using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoEncuesta : IServiceTipoEncuesta
    {
        public List<TipoEncuesta> ObtenerTiposEncuesta(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoEncuesta negocio = new BusinessTipoEncuesta())
                {
                    return negocio.ObtenerTiposEncuesta(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TipoEncuesta TipoEncuestaId(int idTipoEncuesta)
        {
            try
            {
                using (BusinessTipoEncuesta negocio = new BusinessTipoEncuesta())
                {
                    return negocio.TipoEncuestaId(idTipoEncuesta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
