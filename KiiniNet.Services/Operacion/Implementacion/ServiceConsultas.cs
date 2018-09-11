using System;
using System.Collections.Generic;
using System.Data;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceConsultas : IServiceConsultas
    {
        public List<HelperReportesTicket> ConsultarTickets(int idUsuario, List<int> grupos, List<int> canales, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, int pageIndex, int pageSize)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.ConsultarTickets(idUsuario, grupos, canales, tiposUsuario, organizaciones, ubicaciones, tipoArbol, tipificacion, prioridad, estatus, sla, vip, fechas, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperReportesTicket> ConsultarEficienciaTickets(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int pageIndex, int pageSize)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.ConsultarEficienciaTickets(idUsuario, grupos, responsables, tipoArbol, tipificacion, nivelAtencion, atendedores, fechas, tiposUsuario, prioridad, ubicaciones, organizaciones, vip, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperHits> ConsultarHits(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, int pageIndex, int pageSize)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.ConsultarHits(idUsuario, grupos, tiposUsuario, organizaciones, ubicaciones, tipificacion, vip, fechas, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperReportesTicket> ConsultarEncuestas(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int pageIndex, int pageSize)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.ConsultarEncuestas(idUsuario, grupos, tipoArbol, responsables, encuestas, atendedores, fechas, tiposUsuario, prioridad, sla, ubicaciones, organizaciones, vip, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperReportesTicket> ConsultaEncuestaPregunta(int idUsuario, int idEncuesta, Dictionary<string, DateTime> fechas, int tipoFecha, int tipoEncuesta, int idPregunta, int respuesta)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.ConsultaEncuestaPregunta(idUsuario, idEncuesta, fechas, tipoFecha, tipoEncuesta, idPregunta, respuesta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GraficarConsultaTicket(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaTicket(idUsuario, grupos, tiposUsuario, organizaciones, ubicaciones, tipoArbol, tipificacion, prioridad, estatus, sla, vip, fechas, filtroStackColumn, stack, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GraficarConsultaTicketEficiencia(int idUsuario, List<int> tipoUsuario, List<int> area, List<int> grupos, List<int> agentes, List<int> estatusAsignacion, List<int> canal, List<int> tipoArbol, List<int> opciones, List<int> estatus, List<int> prioridad, List<bool?> sla, List<bool?> vip, List<int> organizaciones, List<int> ubicaciones, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaTicketEficiencia(idUsuario, tipoUsuario, area, grupos, agentes, estatusAsignacion, canal, tipoArbol, opciones, estatus, prioridad,sla, vip, organizaciones, ubicaciones, fechas, filtroStackColumn, stack, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GraficarConsultaTicketGeografico(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaTicketGeografico(idUsuario, grupos, tiposUsuario, organizaciones, ubicaciones, tipoArbol, tipificacion, prioridad, estatus, sla, vip, fechas, filtroStackColumn, stack, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GraficarConsultaHits(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaHits(idUsuario, grupos, tiposUsuario, organizaciones, ubicaciones, tipificacion, vip, fechas, filtroStackColumn, stack, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GraficarConsultaHitsGeografico(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaHitsGeografico(idUsuario, grupos, tiposUsuario, organizaciones, ubicaciones, tipificacion, vip, fechas, filtroStackColumn, stack, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GraficarConsultaEncuesta(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaEncuesta(idUsuario, grupos, tipoArbol, responsables, encuestas,
                        atendedores, fechas, tiposUsuario, prioridad, sla, ubicaciones,
                        organizaciones, vip, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GraficarConsultaEncuestaGeografica(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaEncuestaGeografica(idUsuario, grupos, tipoArbol, responsables, encuestas,
                        atendedores, fechas, tiposUsuario, prioridad, sla, ubicaciones,
                        organizaciones, vip, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DataTable> GraficarConsultaEncuestaPregunta(int idUsuario, int idEncuesta, Dictionary<string, DateTime> fechas, int tipoFecha, int tipoEncuesta)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaEncuestaPregunta(idUsuario, idEncuesta, fechas, tipoFecha, tipoEncuesta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GraficarConsultaEncuestaPreguntaGeografica(int idUsuario, List<int?> encuestas, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaEncuestaPreguntaGeografica(idUsuario, encuestas, fechas, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable GraficarConsultaEficienciaTicket(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, string stack, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaEficienciaTicket(idUsuario, grupos,  responsables,  tipoArbol,  tipificacion,  nivelAtencion,  atendedores,  fechas,  tiposUsuario,  prioridad,  ubicaciones,  organizaciones,  vip, stack, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GraficarConsultaEEficienciaTicketsGeografica(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha)
        {
            try
            {
                using (BusinessConsultas negocio = new BusinessConsultas())
                {
                    return negocio.GraficarConsultaEEficienciaTicketsGeografica(idUsuario, grupos,  responsables,  tipoArbol,  tipificacion,  nivelAtencion,  atendedores,  fechas,  tiposUsuario,  prioridad,  ubicaciones,  organizaciones,  vip, tipoFecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
