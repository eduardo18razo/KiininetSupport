using System;
using System.Collections.Generic;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceAtencionTicket : IServiceAtencionTicket
    {
        public void AutoAsignarTicket(int idTicket, int idUsuario, string comentario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.AutoAsignarTicket(idTicket, idUsuario, comentario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GenerarEvento(int idTicket, int idUsuarioGeneraEvento, int? idEstatusTicket, int? idEstatusAsignacion,
            int? idNivelAsignado, int? idUsuarioAsignado, string mensajeConversacion, bool conversacionPrivada, bool enviaCorreo,
            bool sistema, List<string> archivos, string comentarioAsignacion, bool esPropietario)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.GenerarEvento(idTicket, idUsuarioGeneraEvento, idEstatusTicket,  idEstatusAsignacion,
             idNivelAsignado,  idUsuarioAsignado, mensajeConversacion, conversacionPrivada, enviaCorreo,
            sistema, archivos, comentarioAsignacion, esPropietario);
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

        public void AgregarComentarioConversacionTicket(int idTicket, int idUsuario, string mensaje, bool sistema, List<string> archivos, bool privado, bool enviaCorreo)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    negocio.AgregarComentarioConversacionTicket(idTicket, idUsuario, mensaje, sistema, archivos, privado, enviaCorreo);
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

        public HelperTicketEnAtencion ObtenerTicketEnAtencion(int idTicket, int idUsuario, bool esDetalle)
        {
            try
            {
                using (BusinessAtencionTicket negocio = new BusinessAtencionTicket())
                {
                    return negocio.ObtenerTicketEnAtencion(idTicket, idUsuario, esDetalle);
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
