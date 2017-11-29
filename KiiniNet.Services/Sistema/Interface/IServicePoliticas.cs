using KiiniNet.Entities.Parametros;
using System.Collections.Generic;
using System.ServiceModel;

namespace KiiniNet.Services.Sistema.Interface
{
    //
    [ServiceContract]
    public interface IServicePoliticas
    {
        [OperationContract]
        List<EstatusAsignacionSubRolGeneralDefault> ObtenerEstatusAsignacionSubRolGeneralDefault();
        [OperationContract]
        List<EstatusTicketSubRolGeneralDefault> ObtenerEstatusTicketSubRolGeneralDefault();
        [OperationContract]
        List<EstatusAsignacionSubRolGeneral> ObtenerEstatusAsignacionSubRolGeneral();
        [OperationContract]
        List<EstatusTicketSubRolGeneral> ObtenerEstatusTicketSubRolGeneral();

        [OperationContract]
        void HabilitarEstatusAsignacionSubRolGeneralDefault(int idAsignacion, bool habilitado);

        [OperationContract]
        void HabilitarEstatusTicketSubRolGeneralDefault(int idAsignacion, bool habilitado);

        [OperationContract]
        void HabilitarEstatusAsignacionSubRolGeneral(int idAsignacion, bool habilitado);
        
        [OperationContract]
        void HabilitarEstatusTicketSubRolGeneral(int idAsignacion, bool habilitado);


        

        
    }

     
}
