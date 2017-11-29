using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceSubRol
    {
        [OperationContract]
        List<SubRol> ObtenerSubRolesByTipoGrupo(int idTipoGrupo, bool insertarSeleccion);

        [OperationContract]
        SubRol ObtenerSubRolById(int idSubRol);

        [OperationContract]
        List<SubRol> ObtenerSubRolesByGrupoUsuarioRol(int idGrupoUsuario, int idRol, bool insertarSeleccion);

        [OperationContract]
        List<SubRol> ObtenerTipoSubRol(int idTipoGrupo, bool insertarSeleccion);

        [OperationContract]
        List<SubRolEscalacionPermitida> ObtenerEscalacion(int idSubRol, int idEstatusAsignacion, int? nivelActual);

        [OperationContract]
        List<SubRolEscalacionPermitida> ObtenerSubRolEscalacionPermitida();
        [OperationContract]
        void HabilitarPoliticaEscalacion(int idEscalacion, bool habilitado);
    }
}
