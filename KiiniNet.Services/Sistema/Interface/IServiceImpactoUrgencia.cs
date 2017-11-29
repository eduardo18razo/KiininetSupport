using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceImpactoUrgencia
    {
        [OperationContract]
        List<Prioridad> ObtenerPrioridad(bool insertarSeleccion);
        [OperationContract]
        List<Urgencia> ObtenerUrgencia(bool insertarSeleccion);
        [OperationContract]
        Impacto ObtenerImpactoById(int idImpacto);

        [OperationContract]
        Impacto ObtenerImpactoByPrioridadUrgencia(int idPrioridad, int idUrgencia);

        [OperationContract]
        List<Impacto> ObtenerAll(bool insertarSeleccion);
    }
}
