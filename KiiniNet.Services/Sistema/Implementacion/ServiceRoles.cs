using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceRoles : IServiceRoles
    {
        public List<Rol> ObtenerRoles(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessRoles negocio = new BusinessRoles())
                {
                    return negocio.ObtenerRoles(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<RolTipoArbolAcceso> ObtenerRolesArbolAcceso(int idTipoArbolAcceso)
        {
            try
            {
                using (BusinessRoles negocio = new BusinessRoles())
                {
                    return negocio.ObtenerRolesArbolAcceso(idTipoArbolAcceso);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
