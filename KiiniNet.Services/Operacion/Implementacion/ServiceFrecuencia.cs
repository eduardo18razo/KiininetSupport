using System;
using System.Collections.Generic;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceFrecuencia : IServiceFrecuencia
    {
        public List<HelperFrecuencia> ObtenerTopTenGeneral(int idTipoUsuario)
        {
            try
            {
                using (BusinessFrecuencia negocio = new BusinessFrecuencia())
                {
                    return negocio.ObtenerTopTenGeneral(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperFrecuencia> ObtenerTopTenConsulta(int idTipoUsuario)
        {
            try
            {
                using (BusinessFrecuencia negocio = new BusinessFrecuencia())
                {
                    return negocio.ObtenerTopTenConsulta(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperFrecuencia> ObtenerTopTenServicio(int idTipoUsuario)
        {
            try
            {
                using (BusinessFrecuencia negocio = new BusinessFrecuencia())
                {
                    return negocio.ObtenerTopTenServicio(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperFrecuencia> ObtenerTopTenIncidente(int idTipoUsuario)
        {
            try
            {
                using (BusinessFrecuencia negocio = new BusinessFrecuencia())
                {
                    return negocio.ObtenerTopTenIncidente(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
