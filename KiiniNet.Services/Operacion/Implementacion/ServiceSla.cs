using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceSla : IServiceSla
    {
        public List<Sla> ObtenerSla(bool insertarSeleccion)
        {
            try
            {
                using (BusinessSla negocio = new BusinessSla())
                {
                    return negocio.ObtenerSla(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Sla ObtenerSlaById(int idSla)
        {
            try
            {
                using (BusinessSla negocio = new BusinessSla())
                {
                    return negocio.ObtenerSla(idSla);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Guardar(Sla sla)
        {
            try
            {
                using (BusinessSla negocio = new BusinessSla())
                {
                    negocio.Guardar(sla);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
