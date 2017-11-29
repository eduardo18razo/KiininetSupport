using System;
using System.Collections.Generic;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceAtencionTicket : IServiceAtencionTicket
    {
        public void AutoAsignarTicket(int idTicket, int idUsuario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.AutoAsignarTicket(idTicket, idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CambiarEstatus(int idTicket, int idEstatus, int idUsuario, string comentario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.CambiarEstatus(idTicket, idEstatus, idUsuario, comentario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CambiarAsignacionTicket(int idTicket, int idEstatusAsignacion, int idUsuarioAsignado, int idNivelAsignado, int idUsuarioAsigna, string comentario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.CambiarAsignacionTicket(idTicket, idEstatusAsignacion, idUsuarioAsignado, idNivelAsignado, idUsuarioAsigna, comentario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AgregarComentarioConversacionTicket(int idTicket, int idUsuario, string mensaje, bool sistema, List<string> archivos, bool privado)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.AgregarComentarioConversacionTicket(idTicket, idUsuario, mensaje, sistema, archivos, privado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void MarcarAsignacionLeida(int idAsignacion)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.MarcarAsignacionLeida(idAsignacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperticketEnAtencion ObtenerTicketEnAtencion(int idTicket, int idUsuario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    return negocio.ObtenerTicketEnAtencion(idTicket, idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int ObtenerNumeroTicketsEnAtencionNuevos(int idUsuario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    return negocio.ObtenerNumeroTicketsEnAtencionNuevos(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
