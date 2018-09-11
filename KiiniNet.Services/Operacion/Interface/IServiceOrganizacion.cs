using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceOrganizacion
    {
        [OperationContract]
        List<Holding> ObtenerHoldings(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        List<Compania> ObtenerCompañias(int idTipoUsuario, int idHolding, bool insertarSeleccion);

        [OperationContract]
        List<Direccion> ObtenerDirecciones(int idTipoUsuario, int idCompañia, bool insertarSeleccion);

        [OperationContract]
        List<SubDireccion> ObtenerSubDirecciones(int idTipoUsuario, int idDireccoin, bool insertarSeleccion);

        [OperationContract]
        List<Gerencia> ObtenerGerencias(int idTipoUsuario, int idSubdireccion, bool insertarSeleccion);

        [OperationContract]
        List<SubGerencia> ObtenerSubGerencias(int idTipoUsuario, int idGerencia, bool insertarSeleccion);

        [OperationContract]
        List<Jefatura> ObtenerJefaturas(int idTipoUsuario, int idSubGerencia, bool insertarSeleccion);

        [OperationContract]
        Organizacion ObtenerOrganizacion(int idHolding, int? idCompania, int? idDireccion, int? idSubDireccion,int? idGerencia, int? idSubGerencia, int? idJefatura);

        [OperationContract]
        void GuardarOrganizacion(Organizacion organizacion);

        [OperationContract]
        void GuardarHolding(Holding entidad);

        [OperationContract]
        void GuardarCompania(Compania entidad);

        [OperationContract]
        void GuardarDireccion(Direccion entidad);

        [OperationContract]
        void GuardarSubDireccion(SubDireccion entidad);

        [OperationContract]
        void GuardarGerencia(Gerencia entidad);

        [OperationContract]
        void GuardarSubGerencia(SubGerencia entidad);

        [OperationContract]
        void GuardarJefatura(Jefatura entidad);

        [OperationContract]
        Organizacion ObtenerOrganizacionUsuario(int idOrganizacion);

        [OperationContract]
        List<Organizacion> ObtenerOrganizaciones(int? idTipoUsuario, int? idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura);

        [OperationContract]
        void HabilitarOrganizacion(int idOrganizacion, bool habilitado);

        [OperationContract]
        Organizacion ObtenerOrganizacionById(int idOrganizacion);

        [OperationContract]
        void ActualizarOrganizacion(Organizacion org);

        [OperationContract]
        List<Organizacion> ObtenerOrganizacionesGrupos(List<int> grupos);

        [OperationContract]
        List<Organizacion> ObtenerOrganizacionesTipoUsuario(List<int> tiposUsuario);

        [OperationContract]
        List<int> ObtenerOrganizacionesByIdOrganizacion(int idOrganizacion);

        [OperationContract]
        List<Organizacion> BuscarPorPalabra(int? idTipoUsuario, int? idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura, string filtro);
    }
}
