using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using KiiniNet.Entities.Helper;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceConsultas
    {
        [OperationContract]
        List<HelperReportesTicket> ConsultarTickets(int idUsuario, List<int> grupos, List<int> canales, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, int pageIndex, int pageSize);

        [OperationContract]
        List<HelperReportesTicket> ConsultarEficienciaTickets(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int pageIndex, int pageSize);

        [OperationContract]
        List<HelperHits> ConsultarHits(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, int pageIndex, int pageSize);

        [OperationContract]
        List<HelperReportesTicket> ConsultarEncuestas(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int pageIndex, int pageSize);

        [OperationContract]
        List<HelperReportesTicket> ConsultaEncuestaPregunta(int idUsuario, int idEncuesta, Dictionary<string, DateTime> fechas, int tipoFecha, int tipoEncuesta, int idPregunta, int respuesta);

        [OperationContract]
        DataTable GraficarConsultaTicket(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha);

        [OperationContract]
        DataTable GraficarConsultaTicketEficiencia(int idUsuario, List<int> tipoUsuario, List<int> area, List<int> grupos, List<int> agentes, List<int> estatusAsignacion, List<int> canal, List<int> tipoArbol, List<int> opciones, List<int> estatus, List<int> prioridad, List<bool?> sla, List<bool?> vip, List<int> organizaciones, List<int> ubicaciones, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha);

        [OperationContract]
        string GraficarConsultaTicketGeografico(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha);

        [OperationContract]
        DataTable GraficarConsultaHits(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha);

        [OperationContract]
        string GraficarConsultaHitsGeografico(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha);

        [OperationContract]
        DataTable GraficarConsultaEncuesta(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha);

        [OperationContract]
        string GraficarConsultaEncuestaGeografica(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha);

        [OperationContract]
        List<DataTable> GraficarConsultaEncuestaPregunta(int idUsuario, int idEncuesta, Dictionary<string, DateTime> fechas, int tipoFecha, int tipoEncuesta);

        [OperationContract]
        string GraficarConsultaEncuestaPreguntaGeografica(int idUsuario, List<int?> encuestas, Dictionary<string, DateTime> fechas, int tipoFecha);

        [OperationContract]
        DataTable GraficarConsultaEficienciaTicket(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, string stack, int tipoFecha);

        [OperationContract]
        string GraficarConsultaEEficienciaTicketsGeografica(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha);
    }
}
