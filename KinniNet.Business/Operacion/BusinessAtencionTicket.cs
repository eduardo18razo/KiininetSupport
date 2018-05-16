using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Demonio;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessAtencionTicket : IDisposable
    {
        private readonly bool _proxy;

        public void Dispose()
        {

        }

        private DateTime TiempoGeneral(List<HorarioSubGrupo> horarioSubGrupo, decimal? tiempoProceso)
        {
            DateTime result;
            try
            {
                List<DateTime> diasAsignados = new List<DateTime>();
                string horarioInicio = horarioSubGrupo.Min(s => s.HoraInicio);
                string horarioFin = horarioSubGrupo.Max(s => s.HoraFin);
                double tiempotrabajo = double.Parse(horarioFin.Replace(':', '.').Substring(0, 5)) - double.Parse(horarioInicio.Replace(':', '.').Substring(0, 5));

                decimal? horasTotalSolucion = tiempoProceso;
                int contador = 0;
                while (horasTotalSolucion > 0)
                {
                    if (horarioSubGrupo.Any(a => a.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek))
                    {
                        horarioInicio = horarioSubGrupo.Where(w => w.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek).Min(m => m.HoraInicio);
                        horarioFin = horarioSubGrupo.Where(w => w.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek).Max(m => m.HoraFin);
                        if (diasAsignados.Count <= 0)
                        {
                            horasTotalSolucion -= decimal.Parse(Math.Round((DateTime.Parse(DateTime.Now.ToShortDateString() + " " + horarioFin) - DateTime.Now).TotalHours, 2, MidpointRounding.ToEven).ToString());
                            diasAsignados.Add(DateTime.Now.AddDays(contador));
                        }
                        else
                        {
                            if (horasTotalSolucion >= decimal.Parse(tiempotrabajo.ToString()))
                            {
                                horasTotalSolucion -= decimal.Parse(tiempotrabajo.ToString());
                                diasAsignados.Add(DateTime.Now.AddDays(contador));
                            }
                            else
                            {
                                DateTime fecha = DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioInicio).AddHours(double.Parse(horasTotalSolucion.ToString()));
                                horasTotalSolucion -= horasTotalSolucion;
                                diasAsignados.Add(fecha);
                            }
                        }
                    }
                    contador++;
                }
                if (tiempoProceso == 0)
                    diasAsignados.Add(DateTime.Now);
                result = DateTime.ParseExact(diasAsignados.Max().ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }

        private bool PuedeAbrirTicket(int idTicket, int idUsuario)
        {
            bool result = false;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = _proxy;
                TicketGrupoUsuario gpoUsuarioTicket = db.TicketGrupoUsuario.FirstOrDefault(w => w.IdTicket == idTicket && w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente);
                if (gpoUsuarioTicket != null)
                {
                    GrupoUsuario gpoAgenteTicket = db.GrupoUsuario.SingleOrDefault(s => s.Id == gpoUsuarioTicket.IdGrupoUsuario);
                    if (gpoAgenteTicket != null)
                    {
                        var qry = from ug in db.UsuarioGrupo
                                  where ug.IdUsuario == idUsuario && ug.IdGrupoUsuario == gpoAgenteTicket.Id
                                  select ug;
                        if (gpoAgenteTicket.TieneSupervisor)
                        {
                            result = (from q in qry
                                      where q.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor
                                      select q).Any();
                        }
                        else
                        {
                            result = (from q in qry
                                      where q.SubGrupoUsuario.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel
                                      select q).Any();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        private int ObtenerNivelAutoAsignacion(int idUsuario, int idGrupo, bool espropietario)
        {
            int result = (int)BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion.Supervisor;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                SubRol subrol = (from ug in db.UsuarioGrupo
                                 join gu in db.GrupoUsuario on ug.IdGrupoUsuario equals gu.Id
                                 join sgu in db.SubGrupoUsuario on new { gpoid = ug.IdGrupoUsuario, subgpoid = (int)ug.IdSubGrupoUsuario } equals new { gpoid = sgu.IdGrupoUsuario, subgpoid = sgu.Id }
                                 join sr in db.SubRol on new { rolid = ug.IdRol, subrolid = sgu.IdSubRol } equals new { rolid = sr.IdRol, subrolid = sr.Id }
                                 join easrg in db.EstatusAsignacionSubRolGeneral on
                                 new { usuarioidgrupo = ug.IdGrupoUsuario, grupousuarioid = gu.Id, usuariogrupoidrol = ug.IdRol, subrolidrol = sr.IdRol, subrolids = sr.Id, grupotienesupervisor = gu.TieneSupervisor, grupoid = gu.Id, subrolidprincipal = sr.Id } equals
                                 new { usuarioidgrupo = easrg.IdGrupoUsuario, grupousuarioid = easrg.IdGrupoUsuario, usuariogrupoidrol = easrg.IdRol, subrolidrol = easrg.IdRol, subrolids = (int)easrg.IdSubRol, grupotienesupervisor = easrg.TieneSupervisor, grupoid = easrg.IdGrupoUsuario, subrolidprincipal = (int)easrg.IdSubRol }
                                 where ug.IdUsuario == idUsuario && ug.IdRol == (int)BusinessVariables.EnumRoles.Agente && easrg.Propietario == espropietario
                                 && easrg.IdEstatusAsignacionActual == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar
                                 && easrg.IdEstatusAsignacionAccion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado
                                 && ug.IdGrupoUsuario == idGrupo
                                 && easrg.Habilitado
                                 orderby sr.OrdenAsignacion
                                 select sr).FirstOrDefault();
                if (subrol != null)
                {
                    result = (int)((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)subrol.Id);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }
        private List<EstatusTicket> CambiaEstatus( int idEstatusActualTicket, int idGrupoUsuarioTicket, int? subRolPertenece)
        {
            List<EstatusTicket> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                var qry = from etsrg in db.EstatusTicketSubRolGeneral
                          where etsrg.IdGrupoUsuario == idGrupoUsuarioTicket &&
                              etsrg.IdEstatusTicketActual == idEstatusActualTicket
                              && etsrg.IdRolSolicita == 2
                          select etsrg;
                //if (subRolPertenece == null)
                //    qry = from q in qry
                //          where q.IdSubRolPertenece == null
                //          select q;
                //else
                //    qry = from q in qry
                //          where q.IdSubRolPertenece == subRolPertenece
                //          select q;
                result = qry.Select(s => s.EstatusTicketAccion).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public BusinessAtencionTicket(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void AutoAsignarTicket(int idTicket, int idUsuario, string comentario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                if (ticket != null)
                {
                    TicketGrupoUsuario ticketGrupoUsuario = db.TicketGrupoUsuario.FirstOrDefault(f => f.IdTicket == idTicket && f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente);
                    if (ticketGrupoUsuario != null)
                    {
                        int idGpoAtencion = ticketGrupoUsuario.IdGrupoUsuario;

                        ticket.IdNivelTicket = ObtenerNivelAutoAsignacion(idUsuario, idGpoAtencion, false) - 2;
                        ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado; DateTime fechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        TicketEvento evento = new TicketEvento
                        {
                            IdTicket = idTicket,
                            IdUsuarioRealizo = idUsuario,
                            FechaHora = fechaMovimiento,
                            TicketEventoAsignacion = new List<TicketEventoAsignacion>()
                        };
                        evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                        {
                            FechaHora = fechaMovimiento,
                            TicketAsignacion = new TicketAsignacion
                            {
                                FechaAsignacion = fechaMovimiento,
                                IdEstatusAsignacion =
                                    (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado,
                                IdUsuarioAsignado = idUsuario,
                                IdUsuarioAsigno = idUsuario,
                                IdTicket = idTicket,
                                Comentarios = comentario.Trim(),
                                Visto = false
                            }
                        });

                        db.TicketEvento.AddObject(evento);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void CambiarAsignacionTicket(int idTicket, int idEstatusAsignacion, int idUsuarioAsignado, int idNivelAsignado, int idUsuarioAsigna, string comentario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                if (ticket != null)
                {
                    DateTime fechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    TicketEvento evento = new TicketEvento
                    {
                        IdTicket = idTicket,
                        IdUsuarioRealizo = idUsuarioAsigna,
                        FechaHora = fechaMovimiento,
                        TicketEventoAsignacion = new List<TicketEventoAsignacion>()
                    };
                    evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                    {
                        FechaHora = fechaMovimiento,
                        TicketAsignacion = new TicketAsignacion
                        {
                            FechaAsignacion = fechaMovimiento,
                            IdEstatusAsignacion = idEstatusAsignacion,
                            IdUsuarioAsignado = idUsuarioAsignado,
                            IdUsuarioAsigno = idUsuarioAsigna,
                            IdTicket = idTicket,
                            Comentarios = comentario.Trim(),
                            Visto = false
                        }
                    });

                    db.TicketEvento.AddObject(evento);
                    ticket.IdNivelTicket = idNivelAsignado;
                    ticket.IdEstatusAsignacion = idEstatusAsignacion;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void CambiarEstatus(int idTicket, int idEstatus, int idUsuario, string comentario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                int? idEstatusAsignacion = null;
                //int? idNivelAsignado = null;
                if (ticket != null)
                {
                    db.LoadProperty(ticket, "TicketGrupoUsuario");
                    DateTime fechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    TicketEvento evento = new TicketEvento
                    {
                        IdTicket = idTicket,
                        IdUsuarioRealizo = idUsuario,
                        FechaHora = fechaMovimiento,
                        TicketEventoAsignacion = new List<TicketEventoAsignacion>(),
                        TicketEventoConversacion = new List<TicketEventoConversacion>(),
                        TicketEventoEstatus = new List<TicketEventoEstatus>(),

                    };
                    TicketEstatus cambioEstatus = new TicketEstatus
                    {
                        FechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        IdTicket = idTicket,
                        IdEstatus = idEstatus,
                        IdUsuarioMovimiento = idUsuario,
                        Comentarios = comentario.Trim()
                    };

                    #region Estatus
                    if (ticket.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                    {
                        if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                            throw new Exception("Ticket ya se encuentra en espera");

                        db.LoadProperty(ticket, "SlaEstimadoTicket");
                        List<HorarioSubGrupo> lstHorarioGrupo = new List<HorarioSubGrupo>();
                        List<DiaFestivoSubGrupo> lstDiasFestivosGrupo = new List<DiaFestivoSubGrupo>();
                        foreach (SubGrupoUsuario sGpoUsuario in ticket.TicketGrupoUsuario.SelectMany(tgrupoUsuario => db.GrupoUsuario.Where(w => w.Id == tgrupoUsuario.IdGrupoUsuario && w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).SelectMany(gpoUsuario => gpoUsuario.SubGrupoUsuario)))
                        {
                            lstHorarioGrupo.AddRange(db.HorarioSubGrupo.Where(w => w.IdSubGrupoUsuario == sGpoUsuario.Id).ToList());
                            lstDiasFestivosGrupo.AddRange(db.DiaFestivoSubGrupo.Where(w => w.IdSubGrupoUsuario == sGpoUsuario.Id));
                        }

                        ticket.FechaFinEspera = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        if (ticket.FechaInicioEspera != null)
                        {
                            DateTime oldDate = (DateTime)ticket.FechaInicioEspera;
                            DateTime newDate = (DateTime)ticket.FechaFinEspera;
                            TimeSpan ts = newDate - oldDate;
                            ticket.TiempoEspera = ticket.TiempoEspera == null ? (0 + ts.TotalHours).ToString() : (double.Parse(ticket.TiempoEspera) + ts.TotalHours).ToString();
                            ticket.FechaHoraFinProceso = TiempoGeneral(lstHorarioGrupo, ticket.SlaEstimadoTicket.TiempoHoraProceso).AddHours(ts.TotalHours);
                        }
                    }

                    switch (idEstatus)
                    {
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto:
                            ticket.IdUsuarioResolvio = idUsuario;
                            evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                            {
                                FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                TicketAsignacion = new TicketAsignacion
                                {
                                    IdTicket = idTicket,
                                    IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado,
                                    IdUsuarioAsigno = idUsuario,
                                    IdUsuarioAsignado = ticket.IdUsuarioLevanto,
                                    FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                    Comentarios = comentario.Trim()
                                }
                            });
                            idEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto:
                            ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                            ticket.IdNivelTicket = null;
                            evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                            {
                                FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                TicketAsignacion = new TicketAsignacion
                                {
                                    IdTicket = idTicket,
                                    IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                    IdUsuarioAsigno = idUsuario,
                                    IdUsuarioAsignado = null,
                                    FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                    Comentarios = comentario.Trim()
                                }
                            });
                            idEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado:
                            ticket.FechaTermino = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                            evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                            {
                                FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                TicketAsignacion = new TicketAsignacion
                                {
                                    IdTicket = idTicket,
                                    IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                    IdUsuarioAsigno = idUsuario,
                                    IdUsuarioAsignado = null,
                                    FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                    Comentarios = comentario.Trim()
                                }
                            });
                            idEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                            break;
                        case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera:
                            ticket.Espera = true;
                            ticket.FechaInicioEspera = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            ticket.FechaFinEspera = null;
                            break;
                    }
                    #endregion Estatus

                    evento.TicketEventoEstatus.Add(new TicketEventoEstatus { TicketEstatus = cambioEstatus, FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture) });
                    ticket.IdEstatusTicket = idEstatus;
                    if (idEstatusAsignacion != null)
                    {
                        ticket.IdEstatusAsignacion = (int)idEstatusAsignacion;
                    }

                    db.TicketEvento.AddObject(evento);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void AgregarComentarioConversacionTicket(int idTicket, int idUsuario, string mensaje, bool sistema, List<string> archivos, bool privado, bool enviaCorreo)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(s => s.Id == idTicket);
                if (ticket != null)
                {
                    TicketConversacion comment = new TicketConversacion
                    {
                        IdTicket = idTicket,
                        IdUsuario = idUsuario,
                        Mensaje = mensaje.Replace("\n", "<br/>"),
                        FechaGeneracion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        Sistema = sistema,
                        Leido = false,
                        Privado = privado,
                        FechaLectura = null
                    };
                    if (archivos != null)
                    {
                        comment.ConversacionArchivo = new List<ConversacionArchivo>();
                        foreach (ConversacionArchivo archivoComment in archivos.Select(archivo => new ConversacionArchivo { Archivo = archivo.Replace("ticketid", idTicket.ToString()) }))
                        {
                            comment.ConversacionArchivo.Add(archivoComment);
                        }
                    }
                    db.TicketConversacion.AddObject(comment);
                    db.SaveChanges();
                    Usuario usuario = new BusinessUsuarios().ObtenerUsuario(ticket.IdUsuarioSolicito);
                    string correo = usuario.CorreoUsuario.FirstOrDefault(f => f.Obligatorio) == null ? string.Empty : usuario.CorreoUsuario.FirstOrDefault(f => f.Obligatorio).Correo;
                    if (correo != string.Empty && enviaCorreo)
                    {
                        string cuerpo = string.Format("Hola {0},<br>" +
                                        "¡Se ha registrado un nuevo comentario en tu soicitud!" +
                                        "Si requieres hacer una actualización de tu solicitud, por favor contesta este correo o ingresa a https://soporte.kiininet.com/requests/3127595 Gracias", usuario.Nombre);
                        new BusinessTicketMailService().EnviaCorreoTicketGenerado(ticket.Id, ticket.ClaveRegistro, cuerpo, correo);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public void MarcarAsignacionLeida(int idAsignacion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                TicketAsignacion asignacion = db.TicketAsignacion.SingleOrDefault(s => s.Id == idAsignacion);
                if (asignacion != null)
                {
                    asignacion.Visto = true;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }

        public string FormatearFecha(DateTime fechaParse)
        {
            string fecha = "Hoy";
            try
            {
                CultureInfo ci = new CultureInfo("Es-Es");
                var days = (DateTime.Now - fechaParse).TotalDays;
                switch (int.Parse(Math.Abs(Math.Round(days)).ToString()))
                {
                    case 0:
                        fecha = "Hoy";
                        break;
                    case 1:
                        fecha = "Ayer";
                        break;
                    case 2:
                        fecha =
                            ci.DateTimeFormat.GetDayName(fechaParse.DayOfWeek).ToString();
                        break;
                    case 3:
                        fecha =
                            ci.DateTimeFormat.GetDayName(fechaParse.DayOfWeek).ToString();
                        break;
                    case 4:
                        fecha =
                            ci.DateTimeFormat.GetDayName(fechaParse.DayOfWeek).ToString();
                        break;
                    case 5:
                        fecha =
                            ci.DateTimeFormat.GetDayName(fechaParse.DayOfWeek).ToString();
                        break;
                    case 6:
                        fecha =
                            ci.DateTimeFormat.GetDayName(fechaParse.DayOfWeek).ToString();
                        break;
                    default:
                        fecha = fechaParse.ToString("dd-MM-yy");
                        break;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return string.Format("{0} {1} hrs.", fecha, fechaParse.ToString("HH:mm"));
        }

        public HelperTicketEnAtencion ObtenerTicketEnAtencion(int idTicket, int idUsuario, bool esDetalle)
        {
            HelperTicketEnAtencion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Ticket ticket = db.Ticket.SingleOrDefault(s => s.Id == idTicket);
                if (ticket != null)
                {
                    GrupoUsuario gpoAgenteTicket;
                    if (!esDetalle)
                        gpoAgenteTicket = db.Ticket.Join(db.TicketGrupoUsuario, t => t.Id, tgu => tgu.IdTicket, (t, tgu) => new { t, tgu }).Join(db.GrupoUsuario, @t1 => @t1.tgu.IdGrupoUsuario, gu => gu.Id, (@t1, gu) => new { @t1, gu })
                                .Join(db.UsuarioGrupo, @t1 => @t1.gu.Id, ug => ug.IdGrupoUsuario, (@t1, ug) => new { @t1, ug })
                                .Where(@t1 => @t1.@t1.@t1.t.Id == idTicket && @t1.@t1.gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente && @t1.ug.IdUsuario == idUsuario)
                                .Select(@t1 => @t1.@t1.gu).ToList().FirstOrDefault();
                    else
                        gpoAgenteTicket = (from t in db.Ticket
                                           join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                                           where tgu.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente
                                           select tgu.GrupoUsuario).FirstOrDefault();

                    if (gpoAgenteTicket != null)
                    {
                        result = new HelperTicketEnAtencion
                        {
                            IdTicket = ticket.Id,
                            GrupoConSupervisor = gpoAgenteTicket.TieneSupervisor,
                            Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso),
                            CorreoTicket = ticket.UsuarioLevanto.CorreoUsuario != null && ticket.UsuarioLevanto.CorreoUsuario.Count > 0 ? ticket.UsuarioLevanto.CorreoUsuario.First().Correo : "",
                            FechaLevanto = ticket.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm")
                        };
                        switch (ticket.Impacto.Descripcion.Trim())
                        {
                            case BusinessVariables.EnumImpacto.Alto:
                                result.Impacto = "prioridadalta.png";
                                break;
                            case BusinessVariables.EnumImpacto.Medio:
                                result.Impacto = "prioridadmedia.png";
                                break;
                            case BusinessVariables.EnumImpacto.Bajo:
                                result.Impacto = "prioridadbaja.png";
                                break;
                        }

                        List<SubGrupoUsuario> sbGruposUsuario = db.UsuarioGrupo.Join(db.SubGrupoUsuario, ug => ug.IdSubGrupoUsuario, sgu => sgu.Id, (ug, sgu) => new { ug, sgu }).Where(@t => @t.ug.IdGrupoUsuario == gpoAgenteTicket.Id && @t.ug.IdUsuario == idUsuario).Select(@t => @t.sgu).Distinct().ToList();
                        bool usuarioSupervisorGrupo = gpoAgenteTicket.TieneSupervisor ? sbGruposUsuario.Any(a => a.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor) : sbGruposUsuario.Any(a => a.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel);
                        bool propietarioTicket = idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado;
                        if (usuarioSupervisorGrupo && !propietarioTicket)
                        {
                            List<EstatusAsignacionSubRolGeneral> estatusPermitidos;
                            if (gpoAgenteTicket.TieneSupervisor)
                                estatusPermitidos = db.EstatusAsignacionSubRolGeneral.Where(easrg => easrg.IdGrupoUsuario == gpoAgenteTicket.Id
                                                 && easrg.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor
                                                 && easrg.IdEstatusAsignacionActual == ticket.IdEstatusAsignacion
                                                 && easrg.Propietario == propietarioTicket
                                                 && easrg.Habilitado).Distinct().ToList();
                            else
                                estatusPermitidos = db.EstatusAsignacionSubRolGeneral.Where(easrg => easrg.IdGrupoUsuario == gpoAgenteTicket.Id
                                                 && easrg.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel
                                                 && easrg.IdEstatusAsignacionActual == ticket.IdEstatusAsignacion
                                                 && easrg.Propietario == propietarioTicket
                                                 && easrg.Habilitado).Distinct().ToList();
                            propietarioTicket = estatusPermitidos.Count > 0;
                        }
                        result.IdTipoTicket = ticket.IdTipoArbolAcceso;
                        result.DentroSla = ticket.DentroSla;
                        result.IdEstatusTicket = ticket.IdEstatusTicket;
                        result.DescripcionEstatusTicket = ticket.EstatusTicket.Descripcion;
                        result.ColorEstatus = ticket.EstatusTicket.Color;
                        result.IdNivelAsignacion = ticket.IdNivelTicket.HasValue ? ticket.IdNivelTicket : null;
                        result.IdEstatusAsignacion = ticket.IdEstatusAsignacion;
                        result.DescripcionEstatusAsignacion = ticket.EstatusAsignacion.Descripcion;
                        result.PuedeAsignar = propietarioTicket && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto;
                        result.EsPropietario = idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado;
                        result.IdGrupoAsignado = ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Distinct().First().IdGrupoUsuario;
                        result.IdGrupoUsuario = ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Distinct().First().IdGrupoUsuario;
                        result.UsuarioAsignado = ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado != null ? ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado.NombreCompleto : "";
                        result.EstatusDisponibles = CambiaEstatus( ticket.IdEstatusTicket, ticket.TicketGrupoUsuario.Single(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).IdGrupoUsuario, UtilsTicket.ObtenerRolAsignacionByIdNivel(ticket.IdNivelTicket));
                        result.TieneEncuesta = ticket.ArbolAcceso.InventarioArbolAcceso.First().IdEncuesta != null;
                        #region Usuario Levanto
                        if (ticket.UsuarioLevanto != null)
                        {
                            result.UsuarioLevanto = new HelperUsuario();

                            result.UsuarioLevanto.IdUsuario = ticket.IdUsuarioLevanto;
                            result.UsuarioLevanto.NombreCompleto = string.Format("{0} {1} {2}", ticket.UsuarioLevanto.Nombre, ticket.UsuarioLevanto.ApellidoPaterno, ticket.UsuarioLevanto.ApellidoMaterno);
                            result.UsuarioLevanto.TipoUsuarioDescripcion = ticket.UsuarioLevanto.TipoUsuario.Descripcion;
                            result.UsuarioLevanto.TipoUsuarioColor = ticket.UsuarioLevanto.TipoUsuario.Color;
                            result.UsuarioLevanto.Vip = ticket.UsuarioLevanto.Vip;
                            result.UsuarioLevanto.FechaUltimoLogin = ticket.UsuarioLevanto.BitacoraAcceso != null && ticket.UsuarioLevanto.BitacoraAcceso.Count > 0 ? ticket.UsuarioLevanto.BitacoraAcceso.Last().Fecha.ToString("dd/MM/yyyy HH:mm") : "";
                            result.UsuarioLevanto.NumeroTicketsAbiertos = ticket.UsuarioLevanto.TicketsLevantados != null ? ticket.UsuarioLevanto.TicketsLevantados.Count : 0;
                            result.UsuarioLevanto.TicketsAbiertos = ticket.UsuarioLevanto.TicketsLevantados != null && ticket.UsuarioLevanto.TicketsLevantados.Count > 0 ? new List<HelperTicketsUsuario>() : null;
                            if (ticket.UsuarioLevanto.TicketsLevantados != null)
                                if (ticket.UsuarioLevanto.TicketsLevantados != null)
                                {
                                    result.UsuarioLevanto.NumeroTicketsAbiertos = ticket.UsuarioLevanto.TicketsLevantados != null ? ticket.UsuarioLevanto.TicketsLevantados.Count : 0;
                                    result.UsuarioLevanto.TicketsAbiertos = ticket.UsuarioLevanto.TicketsLevantados.Count > 0 ? new List<HelperTicketsUsuario>() : null;
                                    result.UsuarioLevanto.NumeroTicketsAbiertos = ticket.UsuarioLevanto.TicketsLevantados != null ? ticket.UsuarioLevanto.TicketsLevantados.Count : 0;
                                    result.UsuarioLevanto.TicketsAbiertos = new List<HelperTicketsUsuario>();
                                    if (ticket.UsuarioLevanto.TicketsLevantados != null)
                                        if (result.UsuarioLevanto.TicketsAbiertos != null)
                                            foreach (Ticket t in ticket.UsuarioLevanto.TicketsLevantados)
                                            {
                                                result.UsuarioLevanto.TicketsAbiertos.Add(new HelperTicketsUsuario
                                                {
                                                    IdTicket = t.Id,
                                                    Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(t.IdArbolAcceso),
                                                    IdEstatusTicket = t.IdEstatusTicket,
                                                    DescripcionEstatusTicket = t.EstatusTicket.Descripcion,
                                                    FechaCreacion = t.FechaHoraAlta,
                                                    FechaCreacionFormato = FormatearFecha(t.FechaHoraAlta),
                                                    PuedeVer = PuedeAbrirTicket(t.Id, idUsuario)
                                                });
                                            }
                                }
                            result.UsuarioLevanto.TicketsAbiertos.Insert(0, new HelperTicketsUsuario { IdTicket = BusinessVariables.ComboBoxCatalogo.IndexSeleccione, Tipificacion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione });

                            result.UsuarioLevanto.Puesto = ticket.UsuarioLevanto.Puesto != null ? ticket.UsuarioLevanto.Puesto.Descripcion : string.Empty;
                            result.UsuarioLevanto.Correos = ticket.UsuarioLevanto.CorreoUsuario != null ? ticket.UsuarioLevanto.CorreoUsuario.Select(s => s.Correo).ToList() : null;
                            result.UsuarioLevanto.Telefonos = ticket.UsuarioLevanto.TelefonoUsuario.Select(s => s.Numero).ToList(); result.UsuarioLevanto.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(ticket.UsuarioLevanto.IdOrganizacion, true);
                            result.UsuarioLevanto.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(ticket.UsuarioLevanto.IdUbicacion, true);
                            TimeSpan ts = DateTime.Now - DateTime.Now.AddDays(-50);
                            result.UsuarioLevanto.Creado = ts.Days.ToString();
                            result.UsuarioLevanto.UltimaActualizacion = DateTime.Now.AddDays(-21).ToString("dd/MM/yyyy HH:mm");
                        }
                        #endregion Usuario Levanto

                        #region Eventos
                        List<HelperEvento> eventos = new List<HelperEvento>();
                        foreach (TicketEvento eventoTicket in ticket.TicketEvento.OrderByDescending(o => o.Id))
                        {
                            HelperEvento evento = new HelperEvento();
                            evento.IdEvento = eventoTicket.Id;
                            evento.Foto = eventoTicket.Usuario.Foto;
                            evento.IdUsuarioGenero = eventoTicket.IdUsuarioRealizo;
                            evento.NombreUsuario = eventoTicket.Usuario.NombreCompleto;
                            evento.FechaHoraEvento = eventoTicket.FechaHora;
                            evento.FechaHoraEventoFormato = eventoTicket.FechaHora.ToString("dd/MM/yyyy hh:mm:ss");
                            foreach (TicketEventoConversacion eventoConversacion in eventoTicket.TicketEventoConversacion)
                            {
                                evento.Movimientos = evento.Movimientos ?? new List<HelperMovimientoEvento>();
                                HelperMovimientoEvento movimientoEstatus = new HelperMovimientoEvento();
                                movimientoEstatus.IdMovimiento = eventoConversacion.Id;
                                movimientoEstatus.EsMovimientoConversacion = true;
                                movimientoEstatus.Conversacion = eventoConversacion.TicketConversacion.Mensaje;
                                movimientoEstatus.Foto = eventoConversacion.TicketConversacion.Usuario.Foto;
                                movimientoEstatus.ComentarioPublico = (bool)eventoConversacion.TicketConversacion.Privado;
                                evento.Movimientos.Add(movimientoEstatus);
                            }

                            foreach (TicketEventoEstatus eventoEstatus in eventoTicket.TicketEventoEstatus)
                            {
                                evento.Movimientos = evento.Movimientos ?? new List<HelperMovimientoEvento>();
                                HelperMovimientoEvento movimientoEstatus = new HelperMovimientoEvento();
                                movimientoEstatus.IdMovimiento = eventoEstatus.Id;
                                movimientoEstatus.EsMovimientoEstatusTicket = true;
                                movimientoEstatus.IdEstatus = eventoEstatus.TicketEstatus.IdEstatus;
                                movimientoEstatus.DescripcionEstatus = eventoEstatus.TicketEstatus.EstatusTicket.Descripcion;
                                movimientoEstatus.NombreCambioEstatus = eventoEstatus.TicketEstatus.Usuario.NombreCompleto;
                                TicketEstatus asignacionAnterior = ticket.TicketEstatus.Where(s => s.Id != eventoEstatus.TicketEstatus.Id).OrderBy(o => o.Id).LastOrDefault();
                                if (asignacionAnterior != null)
                                {
                                    movimientoEstatus.IdEstatusAnterior = asignacionAnterior.IdEstatus;
                                    movimientoEstatus.DescripcionEstatusAnterior = asignacionAnterior.EstatusTicket.Descripcion;
                                }
                                movimientoEstatus.Foto = eventoEstatus.TicketEstatus.Usuario.Foto;
                                movimientoEstatus.Comentarios = eventoEstatus.TicketEstatus.Comentarios;
                                evento.Movimientos.Add(movimientoEstatus);
                            }
                            foreach (TicketEventoAsignacion eventoAsignacion in eventoTicket.TicketEventoAsignacion)
                            {

                                evento.Movimientos = evento.Movimientos ?? new List<HelperMovimientoEvento>();
                                HelperMovimientoEvento movimientoEstatus = new HelperMovimientoEvento();
                                movimientoEstatus.IdMovimiento = eventoAsignacion.Id;
                                movimientoEstatus.EsMovimientoAsignacion = true;
                                movimientoEstatus.IdEstatus = eventoAsignacion.TicketAsignacion.IdEstatusAsignacion;
                                movimientoEstatus.DescripcionEstatus = eventoAsignacion.TicketAsignacion.EstatusAsignacion.Descripcion;

                                TicketAsignacion asignacionAnterior = ticket.TicketAsignacion.Where(s => s.Id != eventoAsignacion.TicketAsignacion.Id).OrderBy(o => o.Id).LastOrDefault();
                                if (asignacionAnterior != null)
                                {
                                    movimientoEstatus.IdEstatusAnterior = asignacionAnterior.IdEstatusAsignacion;
                                    movimientoEstatus.DescripcionEstatusAnterior = asignacionAnterior.EstatusAsignacion.Descripcion;
                                }

                                movimientoEstatus.Foto = eventoAsignacion.TicketAsignacion.UsuarioAsignado != null ? eventoAsignacion.TicketAsignacion.UsuarioAsignado.Foto : null;
                                movimientoEstatus.IdUsuarioAsigno = eventoAsignacion.TicketAsignacion.IdUsuarioAsigno;
                                movimientoEstatus.NombreCambioEstatus = eventoAsignacion.TicketAsignacion.UsuarioAsigno == null ? string.Empty : eventoAsignacion.TicketAsignacion.UsuarioAsigno.NombreCompleto;
                                movimientoEstatus.IdUsuarioAsignado = eventoAsignacion.TicketAsignacion.IdUsuarioAsignado;
                                movimientoEstatus.NombreUsuarioAsignado = eventoAsignacion.TicketAsignacion.UsuarioAsignado == null ? string.Empty : eventoAsignacion.TicketAsignacion.UsuarioAsignado.NombreCompleto;
                                movimientoEstatus.Comentarios = eventoAsignacion.TicketAsignacion.Comentarios;
                                evento.Movimientos.Add(movimientoEstatus);
                            }

                            eventos.Add(evento);
                        }

                        result.Eventos = eventos;
                        #endregion Eventos

                        #region Conversaciones
                        result.Conversaciones = ticket.TicketConversacion != null ? new List<HelperConversacionDetalle>() : null;
                        if (result.Conversaciones != null && ticket.TicketConversacion != null)
                            foreach (TicketConversacion conversacion in ticket.TicketConversacion.OrderByDescending(o => o.FechaGeneracion))
                            {

                                result.Conversaciones.Add(new HelperConversacionDetalle
                                {
                                    Id = conversacion.Id,
                                    IdUsuario = conversacion.IdUsuario,
                                    Foto = conversacion.Usuario.Foto,
                                    Nombre = conversacion.Usuario.NombreCompleto,
                                    FechaHora = conversacion.FechaGeneracion,
                                    Comentario = conversacion.Mensaje,
                                    Privado = conversacion.Privado != null ? (bool)conversacion.Privado : false,
                                });
                            }
                        #endregion Conversaciones
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public int ObtenerNumeroTicketsEnAtencionNuevos(int idUsuario)
        {
            int result = 0;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                List<Ticket> tickets = db.Ticket.Where(s => s.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado).ToList();
                foreach (Ticket ticket in tickets)
                {
                    result = ticket.TicketAsignacion.Last().IdUsuarioAsignado == idUsuario && !ticket.TicketAsignacion.Last().Visto ? result + 1 : result;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public void GenerarEvento(int idTicket, int idUsuarioGeneraEvento, int? idEstatusTicket, int? idEstatusAsignacion, int? idNivelAsignado, int? idUsuarioAsignado, string mensajeConversacion, bool conversacionPrivada, bool enviaCorreo, bool sistema, List<string> archivos, string comentarioAsignacion, bool esPropietario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                int idTicketGrupoUsuario = db.TicketGrupoUsuario.First(s => s.IdTicket == idTicket && s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).IdGrupoUsuario;
                GrupoUsuario ticketGrupoUsuario = db.GrupoUsuario.SingleOrDefault(s => s.Id == idTicketGrupoUsuario);
                if (ticket != null && ticketGrupoUsuario != null)
                {
                    int idGpoAtencion = ticketGrupoUsuario.Id;
                    DateTime fechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    TicketEvento evento = new TicketEvento
                    {
                        IdTicket = idTicket,
                        IdUsuarioRealizo = idUsuarioGeneraEvento,
                        FechaHora = fechaMovimiento,
                        TicketEventoAsignacion = new List<TicketEventoAsignacion>(),
                        TicketEventoConversacion = new List<TicketEventoConversacion>(),
                        TicketEventoEstatus = new List<TicketEventoEstatus>(),

                    };
                    TicketEstatus cambioEstatus = null;
                    TicketConversacion conversacion = null;
                    #region Asignacion

                    if (idEstatusAsignacion != null && idEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado)
                        idUsuarioAsignado = idUsuarioGeneraEvento;
                    if (idEstatusAsignacion != null && idUsuarioAsignado != null)
                    {

                        if (idEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado)
                            idNivelAsignado = ObtenerNivelAutoAsignacion(idUsuarioGeneraEvento, idGpoAtencion, esPropietario);
                        else if (idNivelAsignado == null)
                            throw new Exception("Error al actualizar nivel de asignacion.");
                        evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                        {
                            FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            TicketAsignacion = new TicketAsignacion
                            {
                                FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                IdEstatusAsignacion = (int)idEstatusAsignacion,
                                IdUsuarioAsignado = idUsuarioAsignado,
                                IdUsuarioAsigno = idUsuarioGeneraEvento,
                                IdTicket = idTicket,
                                Comentarios = comentarioAsignacion.Trim(),
                                Visto = false
                            }
                        });

                    }

                    #endregion Asignacion
                    #region Estatus
                    if (idEstatusTicket != null)
                    {
                        int idEstatus = (int)idEstatusTicket;
                        if (ticket.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                        {
                            if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                                throw new Exception("Ticket ya se encuentra en espera");

                            db.LoadProperty(ticket, "SlaEstimadoTicket");
                            db.LoadProperty(ticket, "TicketGrupoUsuario");
                            List<HorarioSubGrupo> lstHorarioGrupo = new List<HorarioSubGrupo>();
                            List<DiaFestivoSubGrupo> lstDiasFestivosGrupo = new List<DiaFestivoSubGrupo>();
                            foreach (SubGrupoUsuario sGpoUsuario in ticket.TicketGrupoUsuario.SelectMany(tgrupoUsuario => db.GrupoUsuario.Where(w => w.Id == tgrupoUsuario.IdGrupoUsuario && w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).SelectMany(gpoUsuario => gpoUsuario.SubGrupoUsuario)))
                            {
                                lstHorarioGrupo.AddRange(db.HorarioSubGrupo.Where(w => w.IdSubGrupoUsuario == sGpoUsuario.Id).ToList());
                                lstDiasFestivosGrupo.AddRange(db.DiaFestivoSubGrupo.Where(w => w.IdSubGrupoUsuario == sGpoUsuario.Id));
                            }

                            ticket.FechaFinEspera = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                            if (ticket.FechaInicioEspera != null)
                            {
                                DateTime oldDate = (DateTime)ticket.FechaInicioEspera;
                                DateTime newDate = (DateTime)ticket.FechaFinEspera;
                                TimeSpan ts = newDate - oldDate;
                                ticket.TiempoEspera = ticket.TiempoEspera == null ? (0 + ts.TotalHours).ToString() : (double.Parse(ticket.TiempoEspera) + ts.TotalHours).ToString();
                                ticket.FechaHoraFinProceso = TiempoGeneral(lstHorarioGrupo, ticket.SlaEstimadoTicket.TiempoHoraProceso).AddHours(ts.TotalHours);
                            }
                        }
                        cambioEstatus = new TicketEstatus
                        {
                            FechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            IdTicket = idTicket,
                            IdEstatus = idEstatus,
                            IdUsuarioMovimiento = idUsuarioGeneraEvento,
                            Comentarios = comentarioAsignacion.Trim()
                        };
                        switch (idEstatus)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto:
                                ticket.IdUsuarioResolvio = idUsuarioGeneraEvento;
                                evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                                {
                                    FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                    TicketAsignacion = new TicketAsignacion
                                    {
                                        IdTicket = idTicket,
                                        IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado,
                                        IdUsuarioAsigno = idUsuarioGeneraEvento,
                                        IdUsuarioAsignado = ticket.IdUsuarioLevanto,
                                        FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                        Comentarios = comentarioAsignacion.Trim()
                                    }
                                });
                                idEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado;
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto:
                                ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                                ticket.IdNivelTicket = null;
                                evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                                {
                                    FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                    TicketAsignacion = new TicketAsignacion
                                    {
                                        IdTicket = idTicket,
                                        IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                        IdUsuarioAsigno = idUsuarioGeneraEvento,
                                        IdUsuarioAsignado = null,
                                        FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                        Comentarios = comentarioAsignacion.Trim()
                                    }
                                });
                                idEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado:
                                ticket.FechaTermino = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                                ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                                evento.TicketEventoAsignacion.Add(new TicketEventoAsignacion
                                {
                                    FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                    TicketAsignacion = new TicketAsignacion
                                    {
                                        IdTicket = idTicket,
                                        IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                        IdUsuarioAsigno = idUsuarioGeneraEvento,
                                        IdUsuarioAsignado = null,
                                        FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                        Comentarios = comentarioAsignacion.Trim()
                                    }
                                });
                                idEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera:
                                ticket.Espera = true;
                                ticket.FechaInicioEspera = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                                ticket.FechaFinEspera = null;
                                break;
                        }
                    }
                    #endregion Estatus



                    #region Conversacion

                    if (!string.IsNullOrEmpty(mensajeConversacion.Trim()))
                    {
                        conversacion = new TicketConversacion
                        {
                            IdTicket = idTicket,
                            IdUsuario = idUsuarioGeneraEvento,
                            Mensaje = mensajeConversacion.Trim(),
                            FechaGeneracion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            Sistema = sistema,
                            Leido = false,
                            Privado = conversacionPrivada,
                            FechaLectura = null
                        };
                        if (archivos != null)
                        {
                            conversacion.ConversacionArchivo = new List<ConversacionArchivo>();
                            foreach (ConversacionArchivo archivoComment in archivos.Select(archivo => new ConversacionArchivo { Archivo = archivo.Replace("ticketid", idTicket.ToString()) }))
                            {
                                conversacion.ConversacionArchivo.Add(archivoComment);
                            }
                        }
                    }

                    #endregion Conversacion


                    if (cambioEstatus != null)
                    {
                        evento.TicketEventoEstatus.Add(new TicketEventoEstatus { TicketEstatus = cambioEstatus, FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture) });
                        ticket.IdEstatusTicket = (int)idEstatusTicket;
                    }
                    if (idEstatusAsignacion != null)
                    {
                        ticket.IdEstatusAsignacion = (int)idEstatusAsignacion;
                        ticket.IdNivelTicket = idNivelAsignado - 2;
                    }
                    if (conversacion != null)
                        evento.TicketEventoConversacion.Add(new TicketEventoConversacion { TicketConversacion = conversacion, FechaHora = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture) });

                    db.TicketEvento.AddObject(evento);
                    db.SaveChanges();
                    if (conversacion != null && enviaCorreo && !conversacionPrivada)
                    {
                        Usuario usuario = new BusinessUsuarios().ObtenerUsuario(ticket.IdUsuarioSolicito);
                        string correo = usuario.CorreoUsuario.FirstOrDefault(f => f.Obligatorio) == null ? string.Empty : usuario.CorreoUsuario.First(f => f.Obligatorio).Correo;
                        if (correo != string.Empty)
                        {
                            string cuerpo = string.Format("Hola {0},<br>" +
                                            "¡Se ha registrado un nuevo comentario en tu soicitud!" +
                                            "Si requieres hacer una actualización de tu solicitud, por favor contesta este correo o ingresa a https://soporte.kiininet.com/requests/3127595 Gracias", usuario.Nombre);
                            new BusinessTicketMailService().EnviaCorreoTicketGenerado(ticket.Id, ticket.ClaveRegistro, cuerpo, correo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
