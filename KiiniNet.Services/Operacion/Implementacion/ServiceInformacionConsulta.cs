using System;
using System.Collections.Generic;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Reportes;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceInformacionConsulta : IServiceInformacionConsulta
    {
        public List<InformacionConsulta> ObtenerInformacionConsulta(BusinessVariables.EnumTiposInformacionConsulta tipoinfoConsulta, bool insertarSeleccion)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.ObtenerInformacionConsulta(tipoinfoConsulta, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<InformacionConsulta> ObtenerInformacionConsultaArbol(int idArbol)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.ObtenerInformacionConsultaArbol(idArbol);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InformacionConsulta ObtenerInformacionConsultaById(int idInformacion)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.ObtenerInformacionConsultaById(idInformacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InformacionConsulta GuardarInformacionConsulta(InformacionConsulta informacion, List<string> documentosDescarga)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.GuardarInformacionConsulta(informacion, documentosDescarga);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public InformacionConsulta ActualizarInformacionConsulta(int idInformacionConsulta, InformacionConsulta informacion, List<string> documentosDescarga)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.ActualizarInformacionConsulta(idInformacionConsulta, informacion, documentosDescarga);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarHit(int idArbol, int idTipoUsuario, int? idUsuario)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    negocio.GuardarHit(idArbol, idTipoUsuario, idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperInformacionConsulta> ObtenerConsulta(string descripcion, Dictionary<string, DateTime> fechas)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.ObtenerInformacionConsulta(descripcion, fechas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RateConsulta(int idArbol, int idConsulta, int idUsuario, bool meGusta)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    negocio.RateConsulta(idArbol, idConsulta, idUsuario, meGusta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarInformacion(int idInformacion, bool habilitado)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    negocio.HabilitarInformacion(idInformacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ReporteInformacionConsulta ObtenerReporteInformacionConsulta(int idInformacionConsulta, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            try
            {
                using (BusinessInformacionConsulta negocio = new BusinessInformacionConsulta())
                {
                    return negocio.ObtenerReporteInformacionConsulta(idInformacionConsulta, fechas, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
