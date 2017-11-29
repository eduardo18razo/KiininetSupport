using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoUsuario : IServiceTipoUsuario
    {
        public List<TipoUsuario> ObtenerTiposUsuarioResidentes(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoUsuario negocio = new BusinessTipoUsuario())
                {
                    return negocio.ObtenerTiposUsuarioResidentes(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TipoUsuario> ObtenerTiposUsuarioInvitados(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoUsuario negocio = new BusinessTipoUsuario())
                {
                    return negocio.ObtenerTiposUsuarioInvitados(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TipoUsuario> ObtenerTiposUsuario(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoUsuario negocio = new BusinessTipoUsuario())
                {
                    return negocio.ObtenerTiposUsuario(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TipoUsuario ObtenerTipoUsuarioById(int id)
        {
            try
            {
                using (BusinessTipoUsuario negocio = new BusinessTipoUsuario())
                {
                    return negocio.ObtenerTipoUsuarioById(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
