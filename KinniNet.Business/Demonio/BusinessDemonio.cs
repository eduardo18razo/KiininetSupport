using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Demonio
{
    public class BusinessDemonio : IDisposable
    {
        private readonly bool _proxy;

        public void Dispose()
        {

        }

        public BusinessDemonio(bool proxy = false)
        {
            _proxy = proxy;
        }

        private void NotificacionesAntesVencimiento(bool antesVencimiento)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<Ticket> informeDueño = new List<Ticket>();
                List<Ticket> informeMantenimiento = new List<Ticket>();
                List<Ticket> informeDesarrollo = new List<Ticket>();
                List<Ticket> informeConsulta = new List<Ticket>();
                Dictionary<int, List<Ticket>> dictionaryInformeDueño = new Dictionary<int, List<Ticket>>();

                List<TiempoInformeArbol> lstTiempoInforme = (from tia in db.TiempoInformeArbol
                                                             join t in db.Ticket on tia.IdArbol equals t.IdArbolAcceso
                                                             where tia.AntesVencimiento == antesVencimiento
                                                             select tia).Distinct().ToList();
                DateTime fechaFin = DateTime.Now;
                foreach (TiempoInformeArbol tiempoInforme in lstTiempoInforme)
                {
                    DateTime fechaInicio = antesVencimiento ? fechaFin.AddDays(-double.Parse(tiempoInforme.TiempoNotificacion.ToString())) : fechaFin.AddDays(double.Parse(tiempoInforme.TiempoNotificacion.ToString()));
                    List<Ticket> tickets = (from t in db.Ticket
                                            where t.IdEstatusTicket < (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto && t.FechaHoraFinProceso >= fechaInicio && t.FechaHoraFinProceso <= fechaFin
                                            && t.IdArbolAcceso == tiempoInforme.IdArbol
                                            select t).Distinct().ToList();
                    foreach (Ticket ticket in tickets)
                    {
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket, "TicketGrupoUsuario");

                        foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                        {
                            db.LoadProperty(tgu, "GrupoUsuario");
                            db.LoadProperty(tgu.GrupoUsuario, "UsuarioGrupo");
                            foreach (UsuarioGrupo ug in tgu.GrupoUsuario.UsuarioGrupo)
                            {
                                db.LoadProperty(ug, "Usuario");
                                db.LoadProperty(ug.Usuario, "CorreoUsuario");
                            }
                        }
                    }
                    switch (tiempoInforme.IdTipoGrupo)
                    {
                        case (int)BusinessVariables.EnumTiposGrupos.Agente:
                            informeDueño.AddRange(tickets.ToList().Distinct());
                            dictionaryInformeDueño.Add(tiempoInforme.IdTipoNotificacion, tickets.Distinct().ToList());
                            break;
                        case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                            informeMantenimiento.AddRange(tickets.ToList().Distinct());
                            break;
                        case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                            informeDesarrollo.AddRange(tickets.ToList().Distinct());
                            break;
                        case (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales:
                            informeConsulta.AddRange(tickets.ToList().Distinct());
                            break;
                    }
                }
                EnviaNotificacion(informeDueño, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría, antesVencimiento);
                EnviaNotificacion(informeMantenimiento, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido, antesVencimiento);
                EnviaNotificacion(informeDesarrollo, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo, antesVencimiento);
                EnviaNotificacion(informeConsulta, (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales, antesVencimiento);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void EnvioNotificacion()
        {
            try
            {
                ActualizaSla();
                NotificacionesAntesVencimiento(true);
                NotificacionesAntesVencimiento(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void Enviarnotificacion(Dictionary<int, List<Ticket>> informeConsulta, int idTipoGrupo)
        {
            try
            {
                foreach (KeyValuePair<int, List<Ticket>> valuePair in informeConsulta)
                {
                    foreach (Ticket ticket in valuePair.Value)
                    {
                        foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario.Where(w => w.GrupoUsuario.IdTipoGrupo == idTipoGrupo).Distinct())
                        {
                            foreach (UsuarioGrupo ug in tgu.GrupoUsuario.UsuarioGrupo)
                            {

                                switch (valuePair.Key)
                                {
                                    case 1:
                                        foreach (CorreoUsuario correoUsuario in ug.Usuario.CorreoUsuario)
                                        {
                                            EnviaCorreo(correoUsuario.Correo, correoUsuario.Usuario.NombreCompleto, ticket, tgu.GrupoUsuario.Descripcion);
                                        }

                                        break;
                                    case 2:
                                        foreach (TelefonoUsuario telefono in ug.Usuario.TelefonoUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular))
                                        {
                                            EnviaCorreo(telefono.Numero, telefono.Usuario.NombreCompleto, ticket, tgu.GrupoUsuario.Descripcion);
                                        }
                                        break;
                                    case 3:
                                        break;
                                    case 4:
                                        break;
                                    case 5:
                                        break;
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void EnviaNotificacion(List<Ticket> informeConsulta, int idTipoGrupo, bool antesVencimiento)
        {
            foreach (Ticket ticket in informeConsulta.Distinct())
            {
                foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario.Where(w => w.GrupoUsuario.IdTipoGrupo == idTipoGrupo).Distinct())
                {
                    foreach (UsuarioGrupo ug in tgu.GrupoUsuario.UsuarioGrupo)
                    {
                        foreach (CorreoUsuario correoUsuario in ug.Usuario.CorreoUsuario)
                        {
                            BusinessCorreo.SendMail(correoUsuario.Correo,
                                string.Format("{0}", antesVencimiento ? "Ticket apunto de vencer" : "Ticket ha vencido"),
                                string.Format("Ticket No.: <b>{0}</b> Clave <b>{1}</b>" +
                                              "<br>Grupo Notificado <b>{2}</b> " +
                                              "<br>Usuario que pertenece al grupo <b>{3}</b> " +
                                              "<br>Persona Levanto <b>{4}</b>" +
                                              "<br>Fecha levanto <b>{5}</b> " +
                                              "<br>Fecha termino estimada <b>{6}</b> ", ticket.Id,
                                    ticket.Random ? ticket.ClaveRegistro : "N/A", tgu.GrupoUsuario.Descripcion,
                                    correoUsuario.Usuario.NombreCompleto, ticket.UsuarioLevanto.NombreCompleto,
                                    ticket.FechaHoraAlta, ticket.FechaHoraFinProceso));
                        }
                    }
                }
            }
        }

        private void EnviaCorreo(string correo, string nombreCompleto, Ticket ticket, string grupo)
        {
            try
            {
                BusinessCorreo.SendMail(correo, string.Format("Ticket {0} Clave Registro {1} {2}", ticket.Id, ticket.Random ? ticket.ClaveRegistro : "N/A", grupo),
                                        string.Format("Grupo {0} " + "<br>Persona {1} " + "<br>Persona Levanto {2}" + "<br>Ticket Tiempo que levanto {3} " + "<br>tiempo envio {4}",
                                                      grupo, nombreCompleto, ticket.UsuarioLevanto.NombreCompleto, ticket.FechaHoraAlta, ticket.FechaHoraFinProceso));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private void EnviaSms(string numero, string nombreCompleto, Ticket ticket, string grupo)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ActualizaSla()
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var y = db.Ticket.Where(w => w.DentroSla && w.FechaTermino == null);
                foreach (Ticket ticket in y)
                {
                    ticket.DentroSla = DateTime.Now <= ticket.FechaHoraFinProceso;
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
