using KiiniNet.Services.Sistema.Interface;
using System;
using System.Collections.Generic;
using KinniNet.Core.Sistema;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServicePoliticas : IServicePoliticas
    {
        public List<EstatusAsignacionSubRolGeneralDefault> ObtenerEstatusAsignacionSubRolGeneralDefault()
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    return negocio.ObtenerEstatusAsignacionSubRolGeneralDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstatusTicketSubRolGeneralDefault> ObtenerEstatusTicketSubRolGeneralDefault()
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    return negocio.ObtenerEstatusTicketSubRolGeneralDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstatusAsignacionSubRolGeneral> ObtenerEstatusAsignacionSubRolGeneral()
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    return negocio.ObtenerEstatusAsignacionSubRolGeneral();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstatusTicketSubRolGeneral> ObtenerEstatusTicketSubRolGeneral()
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    return negocio.ObtenerEstatusTicketSubRolGeneral();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarEstatusAsignacionSubRolGeneralDefault(int idAsignacion, bool habilitado)
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    negocio.HabilitarEstatusAsignacionSubRolGeneralDefault(idAsignacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarEstatusTicketSubRolGeneralDefault(int idAsignacion, bool habilitado)
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    negocio.HabilitarEstatusTicketSubRolGeneralDefault(idAsignacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarEstatusAsignacionSubRolGeneral(int idAsignacion, bool habilitado)
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    negocio.HabilitarEstatusAsignacionSubRolGeneral(idAsignacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarEstatusTicketSubRolGeneral(int idAsignacion, bool habilitado)
        {
            try
            {
                using (BusinessPoliticas negocio = new BusinessPoliticas())
                {
                    negocio.HabilitarEstatusTicketSubRolGeneral(idAsignacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
