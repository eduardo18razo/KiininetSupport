using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion.Tickets;
using KinniNet.Business.Utils;
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
        private int ObtenerNivelAutoAsignacion(int idUsuario, bool espropietario)
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
                                 && easrg.Habilitado
                                 orderby sr.OrdenAsignacion
                                 select sr).FirstOrDefault();
                if (subrol != null)
                {
                    result = (int)((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)(int)subrol.Id);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public BusinessAtencionTicket(bool proxy = false)
        {
            _proxy = proxy;
        }
        public void AutoAsignarTicket(int idTicket, int idUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                if (ticket != null)
                {
                    ticket.IdNivelTicket = ObtenerNivelAutoAsignacion(idUsuario, false) - 2;
                    ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado;
                    ticket.TicketAsignacion = new List<TicketAsignacion>{new TicketAsignacion
                    {
                        FechaAsignacion =  DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff",CultureInfo.InvariantCulture),
                        IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Autoasignado,
                        IdUsuarioAsignado = idUsuario,
                        IdUsuarioAsigno = idUsuario,
                        IdTicket = idTicket, 
                        Visto = false
                    }};
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

        public void CambiarEstatus(int idTicket, int idEstatus, int idUsuario, string comentario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);

                if (ticket != null)
                {
                    db.LoadProperty(ticket, "TicketGrupoUsuario");
                    if (ticket.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                    {
                        if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                            throw new Exception("Ticket ya se encuentra en espera");

                        db.LoadProperty(ticket, "SlaEstimadoTicket");
                        List<HorarioSubGrupo> lstHorarioGrupo = new List<HorarioSubGrupo>();
                        List<DiaFestivoSubGrupo> lstDiasFestivosGrupo = new List<DiaFestivoSubGrupo>();
                        foreach (SubGrupoUsuario sGpoUsuario in ticket.TicketGrupoUsuario.SelectMany(
                            tgrupoUsuario => db.GrupoUsuario.Where(w => w.Id == tgrupoUsuario.IdGrupoUsuario && w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente)
                                .SelectMany(gpoUsuario => gpoUsuario.SubGrupoUsuario)))
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
                            ticket.TiempoEspera = "0.765564";
                            //ticket.TiempoEspera = ticket.TiempoEspera == null ? (0 + ts.TotalHours).ToString() : (double.Parse(ticket.TiempoEspera) + ts.TotalHours).ToString();
                            ticket.FechaHoraFinProceso = TiempoGeneral(lstHorarioGrupo, ticket.SlaEstimadoTicket.TiempoHoraProceso).AddHours(ts.TotalHours);
                        }
                    }

                    ticket.TicketEstatus = new List<TicketEstatus>{new TicketEstatus
                    {
                        FechaMovimiento =  DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff",
                            CultureInfo.InvariantCulture),
                            IdEstatus = idEstatus,
                            IdUsuarioMovimiento = idUsuario,
                            Comentarios = comentario.Trim()
                    }};
                    if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto)
                    {
                        ticket.IdUsuarioResolvio = idUsuario;
                        ticket.TicketAsignacion = new List<TicketAsignacion>
                        {
                            new TicketAsignacion
                            {
                                IdEstatusAsignacion =
                                    (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.Asignado,
                                IdUsuarioAsigno = idUsuario,
                                IdUsuarioAsignado = ticket.IdUsuarioLevanto,
                                FechaAsignacion =
                                    DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                        "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                Comentarios = comentario.Trim()
                            }
                        };
                    }
                    if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReAbierto)
                    {
                        ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                        ticket.IdNivelTicket = null;
                        ticket.TicketAsignacion = new List<TicketAsignacion>
                        {
                            new TicketAsignacion
                            {
                                IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                IdUsuarioAsigno = idUsuario,
                                IdUsuarioAsignado = null,
                                FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                Comentarios = comentario.Trim()
                            }
                        };
                    }
                    if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado)
                    {
                        ticket.FechaTermino = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        ticket.IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar;
                        ticket.TicketAsignacion = new List<TicketAsignacion>
                        {
                            new TicketAsignacion
                            {
                                IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                IdUsuarioAsigno = idUsuario,
                                IdUsuarioAsignado = null,
                                FechaAsignacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                Comentarios = comentario.Trim()
                            }
                        };
                    }
                    if (idEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera)
                    {
                        ticket.Espera = true;
                        ticket.FechaInicioEspera = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        ticket.FechaFinEspera = null;
                    }
                    ticket.IdEstatusTicket = idEstatus;
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

        public void CambiarAsignacionTicket(int idTicket, int idEstatusAsignacion, int idUsuarioAsignado, int idNivelAsignado, int idUsuarioAsigna, string comentario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                if (ticket != null)
                {
                    ticket.IdNivelTicket = idNivelAsignado;
                    ticket.IdEstatusAsignacion = idEstatusAsignacion;
                    ticket.TicketAsignacion = new List<TicketAsignacion>{new TicketAsignacion
                    {
                        FechaAsignacion =  DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff",CultureInfo.InvariantCulture),
                        IdEstatusAsignacion = idEstatusAsignacion,
                        IdUsuarioAsignado = idUsuarioAsignado,
                        IdUsuarioAsigno = idUsuarioAsigna,
                        IdTicket = idTicket, 
                        Comentarios = comentario.Trim(),
                        Visto = false
                    }};
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

        public void AgregarComentarioConversacionTicket(int idTicket, int idUsuario, string mensaje, bool sistema, List<string> archivos, bool privado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var comment = new TicketConversacion
                {
                    IdTicket = idTicket,
                    IdUsuario = idUsuario,
                    Mensaje = mensaje,
                    FechaGeneracion =
                        DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff",
                            CultureInfo.InvariantCulture),
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

        public HelperticketEnAtencion ObtenerTicketEnAtencion(int idTicket, int idUsuario)
        {
            HelperticketEnAtencion result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Ticket ticket = db.Ticket.SingleOrDefault(s => s.Id == idTicket);
                if (ticket != null)
                {
                    result = new HelperticketEnAtencion();
                    result.IdTicket = ticket.Id;
                    result.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                    result.CorreoTicket = ticket.UsuarioLevanto.CorreoUsuario != null && ticket.UsuarioLevanto.CorreoUsuario.Count > 0 ? ticket.UsuarioLevanto.CorreoUsuario.First().Correo : "";
                    result.FechaLevanto = ticket.FechaHoraAlta.ToString("dd/MM/yyyy HH:mm");
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

                    result.DentroSla = ticket.DentroSla;
                    result.IdEstatusTicket = ticket.IdEstatusTicket;
                    result.DescripcionEstatusTicket = ticket.EstatusTicket.Descripcion;
                    result.ColorEstatus = ticket.EstatusTicket.Color;
                    result.IdNivelAsignacion = ticket.IdNivelTicket.HasValue ? ticket.IdNivelTicket : null;
                    result.IdEstatusAsignacion = ticket.IdEstatusAsignacion;
                    result.DescripcionEstatusAsignacion = ticket.EstatusAsignacion.Descripcion;
                    result.EsPropietario = idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado;
                    result.IdGrupoAsignado = ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Distinct().First().IdGrupoUsuario;
                    if (ticket.UsuarioLevanto != null)
                    {
                        result.UsuarioLevanto = new HelperUsuario();

                        result.UsuarioLevanto.IdUsuario = ticket.IdUsuarioLevanto;
                        result.UsuarioLevanto.NombreCompleto = string.Format("{0} {1} {2}", ticket.UsuarioLevanto.Nombre, ticket.UsuarioLevanto.ApellidoPaterno, ticket.UsuarioLevanto.ApellidoMaterno);
                        result.UsuarioLevanto.TipoUsuarioDescripcion = ticket.UsuarioLevanto.TipoUsuario.Descripcion;
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
                                result.UsuarioLevanto.TicketsAbiertos = ticket.UsuarioLevanto.TicketsLevantados != null && ticket.UsuarioLevanto.TicketsLevantados.Count > 0 ? new List<HelperTicketsUsuario>() : null;
                                if (ticket.UsuarioLevanto.TicketsLevantados != null)
                                    if (result.UsuarioLevanto.TicketsAbiertos != null)
                                        foreach (Ticket t in ticket.UsuarioLevanto.TicketsLevantados)
                                        {
                                            result.UsuarioLevanto.TicketsAbiertos.Add(new HelperTicketsUsuario
                                            {
                                                IdTicket = t.Id,
                                                Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(t.IdArbolAcceso)
                                            });
                                        }


                            }


                        result.UsuarioLevanto.Puesto = ticket.UsuarioLevanto.Puesto != null ? ticket.UsuarioLevanto.Puesto.Descripcion : string.Empty;
                        result.UsuarioLevanto.Correos = ticket.UsuarioLevanto.CorreoUsuario != null ? ticket.UsuarioLevanto.CorreoUsuario.Select(s => s.Correo).ToList() : null;
                        result.UsuarioLevanto.Telefonos = ticket.UsuarioLevanto.TelefonoUsuario.Select(s => s.Numero).ToList();
                        result.UsuarioLevanto.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(ticket.UsuarioLevanto.IdOrganizacion, true);
                        result.UsuarioLevanto.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(ticket.UsuarioLevanto.IdUbicacion, true);
                        TimeSpan ts = DateTime.Now - DateTime.Now.AddDays(-50);
                        result.UsuarioLevanto.Creado = ts.Days.ToString();
                        result.UsuarioLevanto.UltimaActualizacion = DateTime.Now.AddDays(-21).ToString("dd/MM/yyyy HH:mm");
                    }
                    result.Conversaciones = ticket.TicketConversacion != null ? new List<HelperConversacionDetalle>() : null;
                    if (result.Conversaciones != null && ticket.TicketConversacion != null)
                        foreach (TicketConversacion conversacion in ticket.TicketConversacion.OrderByDescending(o => o.FechaGeneracion))
                        {

                            result.Conversaciones.Add(new HelperConversacionDetalle
                            {
                                Id = conversacion.Id,
                                IdUsuario = conversacion.IdUsuario,
                                Nombre = conversacion.Usuario.NombreCompleto,
                                FechaHora = conversacion.FechaGeneracion,
                                Comentario = conversacion.Mensaje,
                                Privado = (bool)conversacion.Privado,
                            });
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
    }
}
