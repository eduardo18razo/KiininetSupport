using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Helper;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceMascaras
    {
        [OperationContract]
        void CrearMascara(Mascara mascara);

        [OperationContract]
        void ActualizarMascara(Mascara mascara);

        [OperationContract]
        Mascara ObtenerMascaraCaptura(int idMascara);
        [OperationContract]
        Mascara ObtenerMascaraCapturaByIdTicket(int idTicket);

        [OperationContract]
        List<Mascara> ObtenerMascarasAcceso(int idTipoMascara, bool sistema, bool insertarSeleccion);
        [OperationContract]
        List<CatalogoGenerico> ObtenerCatalogoCampoMascara(int idCatalogo, bool insertarSeleccion, bool filtraHabilitados);

        [OperationContract]
        List<Mascara> Consulta(string descripcion);

        [OperationContract]
        void HabilitarMascara(int idMascara, bool habilitado);

        [OperationContract]
        List<HelperMascaraData> ObtenerDatosMascara(int idMascara, int idTicket);

        [OperationContract]
        DataTable ObtenerReporteMascara(int idMascara, Dictionary<string, DateTime> fechas);
    }
}
