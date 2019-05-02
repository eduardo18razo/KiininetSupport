using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Helper.Reportes;
using KiiniNet.Entities.Operacion;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceEncuesta
    {
        [OperationContract]
        List<Encuesta> ObtenerEncuestas(bool insertarSeleccion);
        [OperationContract]
        Encuesta ObtenerEncuestaById(int idEncuesta);
        [OperationContract]
        Encuesta ObtenerEncuestaByIdTicket(int idTicket);
        [OperationContract]
        Encuesta ObtenerEncuestaByIdConsulta(int idConsulta);

        [OperationContract]
        void GuardarEncuesta(Encuesta encuesta);

        [OperationContract]
        void ActualizarEncuesta(Encuesta encuesta);

        [OperationContract]
        List<Encuesta> Consulta(string descripcion);

        [OperationContract]
        void HabilitarEncuesta(int idencuesta, bool habilitado);

        [OperationContract]
        List<HelperEncuesta> ObtenerEncuestasPendientesUsuario(int idUsuario);

        [OperationContract]

        void ContestaEncuesta(List<RespuestaEncuesta> encuestaRespondida, int idTicket);

        [OperationContract]
        List<Encuesta> ObtenerEncuestasContestadas(bool insertarSeleccion);

        [OperationContract]
        List<Encuesta> ObtenerEncuestaByGrupos(List<int> grupos, bool insertarSeleccion);

        [OperationContract]
        HelperReporteEncuesta ObtenerGraficoNps(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha);

        [OperationContract]
        DataTable ObtenerGraficoNpsDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas);

        [OperationContract]
        HelperReporteEncuesta ObtenerGraficoCalificacion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha);

        [OperationContract]
        DataTable ObtenerGraficoCalificacionDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas);

        [OperationContract]
        HelperReporteEncuesta ObtenerGraficoSatisfaccion(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha);
        [OperationContract]
        DataTable ObtenerGraficoSatisfaccionDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas);

        [OperationContract]
        HelperReporteEncuesta ObtenerGraficoLogica(int idArbolAcceso, Dictionary<string, DateTime> fechas, int tipoFecha);

        [OperationContract]
        DataTable ObtenerGraficoLogicaDescarga(int idArbolAcceso, Dictionary<string, DateTime> fechas);

    }
}
