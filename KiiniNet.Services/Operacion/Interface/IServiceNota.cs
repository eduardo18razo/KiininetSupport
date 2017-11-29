using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceNota
    {
        [OperationContract]
        void CrearNotaGeneralUsuario(NotaGeneral notaGeneral);
        [OperationContract]
        void CrearNotaOpcionUsuario(NotaOpcionUsuario notaOpcionUsuario);
        [OperationContract]
        void CrearNotaOpcionGrupo(NotaOpcionGrupo notaOpcionGrupo);

        [OperationContract]
        List<HelperNotasUsuario> ObtenerNotasUsuario(int idUsuario, bool insertarSeleccion);

        [OperationContract]
        List<HelperNotasOpcion> ObtenerNotasOpcion(int idUsuario, bool insertarSeleccion);

        [OperationContract]
        List<HelperNotasUsuario> ObtenerNotasGrupo(int idUsuario);
    }
}
