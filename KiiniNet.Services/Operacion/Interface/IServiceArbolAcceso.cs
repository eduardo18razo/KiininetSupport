using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceArbolAcceso
    {
        [OperationContract]
        List<HelperArbolAcceso> ObtenerOpcionesPermitidas(int idUsuarioSolicita, int idUsuarioLevanta, int? idArea, bool insertarSeleccion);
        [OperationContract]
        bool LevantaTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);
        [OperationContract]
        bool RecadoTicketTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);

        [OperationContract]
        List<Nivel1> ObtenerNivel1ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, bool insertarSeleccion);
        [OperationContract]
        List<Nivel2> ObtenerNivel2ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel1, bool insertarSeleccion);
        [OperationContract]
        List<Nivel3> ObtenerNivel3ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel2, bool insertarSeleccion);
        [OperationContract]
        List<Nivel4> ObtenerNivel4ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel3, bool insertarSeleccion);
        [OperationContract]
        List<Nivel5> ObtenerNivel5ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel4, bool insertarSeleccion);
        [OperationContract]
        List<Nivel6> ObtenerNivel6ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel5, bool insertarSeleccion);
        [OperationContract]
        List<Nivel7> ObtenerNivel7ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel6, bool insertarSeleccion);
        [OperationContract]
        bool EsNodoTerminalByGrupos(int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);


        [OperationContract]
        List<Nivel1> ObtenerNivel1(int idArea, int idTipoArbol, int idTipoUsuario, bool insertarSeleccion);
        [OperationContract]
        List<Nivel2> ObtenerNivel2(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel1, bool insertarSeleccion);
        [OperationContract]
        List<Nivel3> ObtenerNivel3(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel2, bool insertarSeleccion);
        [OperationContract]
        List<Nivel4> ObtenerNivel4(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel3, bool insertarSeleccion);
        [OperationContract]
        List<Nivel5> ObtenerNivel5(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel4, bool insertarSeleccion);
        [OperationContract]
        List<Nivel6> ObtenerNivel6(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel5, bool insertarSeleccion);
        [OperationContract]
        List<Nivel7> ObtenerNivel7(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel6, bool insertarSeleccion);
        [OperationContract]
        bool EsNodoTerminal(int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);
        [OperationContract]
        void GuardarArbol(ArbolAcceso arbol);
        [OperationContract]
        List<ArbolAcceso> ObtenerArbolesAccesoByGruposUsuario(int idUsuario, int idTipoArbol, int idArea);
        [OperationContract]
        ArbolAcceso ObtenerArbolAcceso(int idArbol);

        [OperationContract]
        List<ArbolAcceso> ObtenerArbolesAccesoAll(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);

        [OperationContract]
        void HabilitarArbol(int idArbol, bool habilitado);
        [OperationContract]
        void ActualizardArbol(int idArbolAcceso, ArbolAcceso arbolAcceso, string descripcion);

        [OperationContract]
        List<ArbolAcceso> ObtenerArbolesAccesoTerminalAll(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);

        [OperationContract]
        List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalAllTipificacion(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7);

        [OperationContract]
        List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalByIdUsuario(int idUsuario, bool insertarSeleccion);

        [OperationContract]
        List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalByGrupoUsuario(int idGrupo);

        [OperationContract]
        List<HelperBusquedaArbolAcceso> BusquedaGeneral(int? idUsuario, string filter, List<int> tipoUsuario, int? idTipoArbol, int? idCategoria, int page, int pagesize);
    }
}
