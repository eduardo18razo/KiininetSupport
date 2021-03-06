﻿using System;
using KiiniNet.Entities.Operacion.Dashboard;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceDashboards : IServiceDashboards
    {
        public DashboardAdministrador GetDashboardAdministrador()
        {
            try
            {
                using (BusinessDashboards negocio = new BusinessDashboards())
                {
                    return negocio.GetDashboardAdministrador();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DashboardAgente GetDashboardAgente(int? idGrupo, int? idUsuario)
        {
            try
            {
                using (BusinessDashboards negocio = new BusinessDashboards())
                {
                    return negocio.GetDashboardAgente(idGrupo, idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
