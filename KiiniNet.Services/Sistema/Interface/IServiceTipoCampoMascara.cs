using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Mascaras;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoCampoMascara
    {
        [OperationContract]
        List<TipoCampoMascara> ObtenerTipoCampoMascara(bool insertarSeleccion);
        [OperationContract]
        TipoCampoMascara TipoCampoMascaraId(int idTipoCampo);
    }
}
