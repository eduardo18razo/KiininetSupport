using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoNota : IServiceTipoNota
    {
        public List<TipoNota> ObtenerTipoNotas(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoNota negocio = new BusinessTipoNota())
                {
                    return negocio.ObtenerTipoNotas(insertarSeleccion);
                }
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }
    }
}
