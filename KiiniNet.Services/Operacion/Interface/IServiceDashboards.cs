﻿using System.ServiceModel;
using KiiniNet.Entities.Operacion.Dashboard;

namespace KiiniNet.Services.Operacion.Interface
{
    [ServiceContract]
    public interface IServiceDashboards
    {
        [OperationContract]
        DashboardAdministrador GetDashboardAdministrador();

        [OperationContract]
        DashboardAgente GetDashboardAgente(int? idGrupo, int? idUsuario);
    }
}
