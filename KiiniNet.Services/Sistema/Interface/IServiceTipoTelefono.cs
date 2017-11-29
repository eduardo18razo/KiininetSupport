﻿using System.Collections.Generic;
using System.ServiceModel;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Services.Sistema.Interface
{
    [ServiceContract]
    public interface IServiceTipoTelefono
    {
        [OperationContract]
        List<TipoTelefono> ObtenerTiposTelefono(bool insertarSeleccion);

    }
}
