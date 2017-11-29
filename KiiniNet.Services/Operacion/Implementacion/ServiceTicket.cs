using System;
using System.Collections.Generic;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceTicket : IServiceTicket
    {
        public Ticket CrearTicketAndroid(int idUsuario, int idUsuarioSolicito, int idArbol, string latitudinicio, string longitudinicio,
            string fechaalta, string latitudfin, string longitudfin, string costo, int idCanal, bool campoRandom, bool esTercero, bool esMail)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    List<HelperCampoMascaraCaptura> campos = new List<HelperCampoMascaraCaptura>();
                    campos.Add(new HelperCampoMascaraCaptura { IdCampo = 17, NombreCampo = "LATITUDINICIO", Valor = latitudinicio });
                    campos.Add(new HelperCampoMascaraCaptura { IdCampo = 18, NombreCampo = "LONGITUDINICIO", Valor = longitudinicio });
                    campos.Add(new HelperCampoMascaraCaptura { IdCampo = 19, NombreCampo = "FECHAALTA", Valor = DateTime.Now.ToString("yyyy-MM-dd") });
                    campos.Add(new HelperCampoMascaraCaptura { IdCampo = 20, NombreCampo = "LATITUDFIN", Valor = latitudfin });
                    campos.Add(new HelperCampoMascaraCaptura { IdCampo = 21, NombreCampo = "LONGITUDFIN", Valor = latitudfin });
                    campos.Add(new HelperCampoMascaraCaptura { IdCampo = 22, NombreCampo = "COSTO", Valor = costo });

                    return negocio.CrearTicket(idUsuario, idUsuarioSolicito, idArbol, campos, idCanal, campoRandom, esTercero, esMail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Ticket CrearTicket(int idUsuario, int idUsuarioSolicito, int idArbol, List<HelperCampoMascaraCaptura> lstCaptura, int idCanal, bool campoRandom, bool esTercero, bool esMail)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {

                    return negocio.CrearTicket(idUsuario, idUsuarioSolicito, idArbol, lstCaptura, idCanal, campoRandom, esTercero, esMail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperTickets> ObtenerTicketsUsuario(int idUsuario, int pageIndex, int pageSize)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.ObtenerTicketsUsuario(idUsuario, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperTickets> ObtenerTickets(int idUsuario, List<int> estatus, int pageIndex, int pageSize)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.ObtenerTickets(idUsuario, estatus, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperDetalleTicket ObtenerDetalleTicket(int idTicket)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.ObtenerDetalleTicket(idTicket);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperTicketDetalle ObtenerTicket(int idTicket, int idUsuario)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.ObtenerTicket(idTicket, idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public HelperDetalleTicket ObtenerDetalleTicketNoRegistrado(int idTicket, string cveRegistro)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.ObtenerDetalleTicketNoRegistrado(idTicket, cveRegistro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PreTicket GeneraPreticket(int idArbol, int idUsuarioSolicita, int idUsuarioLevanto, string observaciones)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.GeneraPreticket(idArbol, idUsuarioSolicita, idUsuarioLevanto, observaciones);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<int> CapturaCasillaTicket(int idTicket, string nombreCampo)
        {
            try
            {
                using (BusinessTicket negocio = new BusinessTicket())
                {
                    return negocio.CapturaCasillaTicket(idTicket, nombreCampo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
