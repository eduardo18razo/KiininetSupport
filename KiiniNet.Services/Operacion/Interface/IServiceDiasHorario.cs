using System;
using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceDiasHorario
    {
        #region Horarios
        [OperationContract]
        List<Horario> ObtenerHorarioDefault(bool insertarSeleccion);
        [OperationContract]
        List<Horario> ObtenerHorarioConsulta(string filtro);
        [OperationContract]
        void CrearHorario(Horario horario);
        [OperationContract]
        void ActualizarHorario(Horario horario);
        [OperationContract]
        Horario ObtenerHorarioById(int idHorario);

        [OperationContract]
        void HabilitarHorario(int idHorario, bool habilitado);
        #endregion Horarios

        #region Dias Feriados

        [OperationContract]
        List<DiasFeriados> ObtenerDiasFeriadosConsulta(string filtro);
        [OperationContract]
        List<DiaFestivoDefault> ObtenerDiasDefault(bool insertarSeleccion);
        [OperationContract]
        void AgregarDiaFeriado(DiaFeriado dia);
        [OperationContract]
        DiaFeriado ObtenerDiaByFecha(DateTime fecha);
        [OperationContract]
        DiaFeriado ObtenerDiaFeriado(int id);
        [OperationContract]
        List<DiaFeriado> ObtenerDiasFeriados(bool insertarSeleccion);
        [OperationContract]
        void ActualizarDiasFestivos(DiasFeriados item);
        [OperationContract]
        void CrearDiasFestivos(DiasFeriados item);

        [OperationContract]
        List<DiasFeriados> ObtenerDiasFeriadosUser(bool insertarSeleccion);
        [OperationContract]
        DiasFeriados ObtenerDiasFeriadosUserById(int idCatalogo);

        [OperationContract]
        void HabilitarDiasFestivos(int idDiasFestivos, bool habilitado);

        #endregion Dias Feriados
    }
}
