using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceCatalogos
    {
        [OperationContract]
        void CrearCatalogo(Catalogos catalogo, bool esMascara, List<CatalogoGenerico> registros);
        [OperationContract]
        void ActualizarCatalogo(Catalogos catalogo, bool esMascara, List<CatalogoGenerico> registros);

        [OperationContract]
        Catalogos ObtenerCatalogo(int idCatalogo);
        [OperationContract]
        List<Catalogos> ObtenerCatalogos(bool insertarSeleccion);
        [OperationContract]
        List<Catalogos> ObtenerCatalogoConsulta(int? idCatalogo);
        [OperationContract]
        List<Catalogos> ObtenerCatalogosMascaraCaptura(bool insertarSeleccion);

        [OperationContract]
        void Habilitar(int idCatalogo, bool habilitado);

        [OperationContract]
        void AgregarRegistroSistema(int idCatalogo, string descripcion);

        [OperationContract]
        void ActualizarRegistroSistema(int idCatalogo, string descripcion, int idRegistro);

        [OperationContract]
        void HabilitarRegistroSistema(int idCatalogo, bool habilitado, int idRegistro);

        [OperationContract]
        List<CatalogoGenerico> ObtenerRegistrosSistemaCatalogo(int idCatalogo, bool insertarSeleccion, bool filtroHabilitado);
        [OperationContract]
        DataTable ObtenerRegistrosArchivosCatalogo(int idCatalogo);

        [OperationContract]
        void CrearCatalogoExcel(Catalogos catalogo, bool esMascara, string archivo, string hoja);

        [OperationContract]
        void ActualizarCatalogoExcel(Catalogos cat, bool esMascara, string archivo, string hoja);
    }
}
