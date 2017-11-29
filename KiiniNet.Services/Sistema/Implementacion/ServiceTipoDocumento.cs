using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoDocumento : IServiceTipoDocumento
    {
        public List<TipoDocumento> ObtenerTipoDocumentos(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoDocumento negocio = new BusinessTipoDocumento())
                {
                    return negocio.ObtenerTipoDocumentos(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TipoDocumento ObtenerTiposDocumentoId(int idTipoDocumento)
        {
            try
            {
                using (BusinessTipoDocumento negocio = new BusinessTipoDocumento())
                {
                    return negocio.ObtenerTiposDocumentoId(idTipoDocumento);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
