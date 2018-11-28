using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Services.Parametros.Interface
{
    [ServiceContract]
    public interface IServiceParametros
    {

        [OperationContract]
        List<TelefonoUsuario> ObtenerTelefonosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        List<CorreoUsuario> ObtenerCorreosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion);

        [OperationContract]
        ParametrosGenerales ObtenerParametrosGenerales();
        [OperationContract]
        ParametrosUsuario ObtenerParametrosUsuario(int idTipoUsuario);

        [OperationContract]
        List<AliasOrganizacion> ObtenerAliasOrganizacion(int idTipoUsuario);

        [OperationContract]
        List<AliasUbicacion> ObtenerAliasUbicacion(int idTipoUsuario);

        [OperationContract]
        ParametroDatosAdicionales ObtenerDatosAdicionales(int idTipoUsuario);

        [OperationContract]
        ParametroPassword ObtenerParemtrosPassword();

        [OperationContract]
        GraficosDefault ObtenerParametrosGraficoDefault();
        [OperationContract]
        List<FrecuenciaFecha> ObtenerFrecuenciasFecha();

        [OperationContract]
        List<ColoresTop> ObtenerColoresTop();

        [OperationContract]
        List<ColoresSla> ObtenerColoresSla();
        [OperationContract]
        List<ArchivosPermitidos> ObtenerArchivosPermitidos();
    }
}
