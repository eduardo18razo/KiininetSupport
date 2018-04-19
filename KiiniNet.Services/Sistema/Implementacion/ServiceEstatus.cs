using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceEstatus : IServiceEstatus
    {
        public List<EstatusTicket> ObtenerEstatusTicket(bool insertarSeleccion)
        {
            try
            {
                using (BusinessEstatus negocio = new BusinessEstatus())
                {
                    return negocio.ObtenerEstatusTicket(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstatusAsignacion> ObtenerEstatusAsignacion(bool insertarSeleccion)
        {
            try
            {
                using (BusinessEstatus negocio = new BusinessEstatus())
                {
                    return negocio.ObtenerEstatusAsignacion(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstatusTicket> ObtenerEstatusTicketUsuario(int idUsuario, int idGrupo, int idEstatusActual, bool esPropietario, int? idSubRol, bool insertarSeleccion)
        {
            try
            {
                using (BusinessEstatus negocio = new BusinessEstatus())
                {
                    return negocio.ObtenerEstatusTicketUsuario(idUsuario, idGrupo, idEstatusActual, esPropietario, idSubRol, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int estatusAsignacionActual, bool esPropietario, int subRolActual, bool insertarSeleccion)
        {
            try
            {
                using (BusinessEstatus negocio = new BusinessEstatus())
                {
                    return negocio.ObtenerEstatusAsignacionUsuario(idUsuario, idGrupo,  estatusAsignacionActual,  esPropietario, subRolActual,  insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool HasComentarioObligatorio(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, int estatusAsignar, bool esPropietario)
        {
            try
            {
                using (BusinessEstatus negocio = new BusinessEstatus())
                {
                    return negocio.HasComentarioObligatorio(idUsuario, idGrupo, idSubRol, estatusAsignacionActual, estatusAsignar, esPropietario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
