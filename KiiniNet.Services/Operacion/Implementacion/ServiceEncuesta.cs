using System;
using System.Collections.Generic;
using System.Data;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Helper.Reportes;
using KiiniNet.Entities.Operacion;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceEncuesta : IServiceEncuesta
    {
        public List<Encuesta> ObtenerEncuestas(bool insertarSeleccion)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestas(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Encuesta ObtenerEncuestaById(int idEncuesta)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestaById(idEncuesta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Encuesta ObtenerEncuestaByIdTicket(int idTicket)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestaByIdTicket(idTicket);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Encuesta ObtenerEncuestaByIdConsulta(int idConsulta)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestaByIdConsulta(idConsulta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarEncuesta(Encuesta encuesta)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    negocio.GuardarEncuesta(encuesta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarEncuesta(Encuesta encuesta)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    negocio.ActualizarEncuesta(encuesta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Encuesta> Consulta(string descripcion)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                   return negocio.Consulta(descripcion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarEncuesta(int idencuesta, bool habilitado)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    negocio.HabilitarEncuesta(idencuesta, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperEncuesta> ObtenerEncuestasPendientesUsuario(int idUsuario)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestasPendientesUsuario(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ContestaEncuesta(List<RespuestaEncuesta> encuestaRespondida, int idTicket)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    negocio.ContestaEncuesta(encuestaRespondida, idTicket);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Encuesta> ObtenerEncuestasContestadas(bool insertarSeleccion)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestasContestadas(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Encuesta> ObtenerEncuestaByGrupos(List<int> grupos, bool insertarSeleccion)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerEncuestaByGrupos(grupos, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperReporteEncuesta ObtenerGraficoNps(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoNps(idArbolAcceso, fechas, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperReporteEncuesta ObtenerGraficoCalificacion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoCalificacion(idArbolAcceso, fechas, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ObtenerGraficoCalificacionDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoCalificacionDescarga(idArbolAcceso, fechas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperReporteEncuesta ObtenerGraficoSatisfaccion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoSatisfaccion(idArbolAcceso, fechas, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ObtenerGraficoSatisfaccionDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoSatisfaccionDescarga(idArbolAcceso, fechas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperReporteEncuesta ObtenerGraficoLogica(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoLogica(idArbolAcceso, fechas, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ObtenerGraficoLogicaDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas)
        {
            try
            {
                using (BusinessEncuesta negocio = new BusinessEncuesta())
                {
                    return negocio.ObtenerGraficoLogicaDescarga(idArbolAcceso, fechas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
