using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using KinniNet.Core.Demonio;
using KinniNet.Core.Sistema;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessConsultas : IDisposable
    {
        private readonly bool _proxy;
        public void Dispose()
        {

        }
        public BusinessConsultas(bool proxy = false)
        {
            _proxy = proxy;
        }
        #region Consultas
        public List<HelperReportesTicket> ConsultarTickets(int idUsuario, List<int> grupos, List<int> canales, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, int pageIndex, int pageSize)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperReportesTicket> result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          select new { t, tgu };

                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;

                if (canales.Any())
                    qry = from q in qry
                          where canales.Contains(q.t.IdCanal)
                          select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;
                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;
                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.t.IdArbolAcceso)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (estatus.Any())
                    qry = from q in qry
                          where estatus.Contains(q.t.IdEstatusTicket)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                List<Ticket> lstTickets = qry.Select(s => s.t).Distinct().ToList();

                int totalRegistros = lstTickets.Count;
                if (totalRegistros > 0)
                {
                    result = new List<HelperReportesTicket>();
                    foreach (Ticket ticket in lstTickets.Skip(pageIndex * pageSize).Take(pageSize))
                    {
                        db.LoadProperty(ticket, "Canal");
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket, "TipoUsuario");
                        db.LoadProperty(ticket, "EstatusTicket");
                        db.LoadProperty(ticket, "EstatusAsignacion");
                        db.LoadProperty(ticket, "TicketEstatus");
                        db.LoadProperty(ticket, "TicketAsignacion");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "Organizacion");
                        db.LoadProperty(ticket, "Ubicacion");
                        foreach (TicketAsignacion asignacion in ticket.TicketAsignacion)
                        {

                            db.LoadProperty(asignacion, "UsuarioAsignado");
                            if (asignacion.UsuarioAsignado != null)
                            {
                                db.LoadProperty(asignacion.UsuarioAsignado, "UsuarioGrupo");
                                foreach (UsuarioGrupo grupo in asignacion.UsuarioAsignado.UsuarioGrupo)
                                {
                                    db.LoadProperty(grupo, "SubGrupoUsuario");
                                    if (grupo.SubGrupoUsuario != null)
                                        db.LoadProperty(grupo.SubGrupoUsuario, "SubRol");
                                }
                            }
                        }
                        db.LoadProperty(ticket, "ArbolAcceso");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket.Impacto, "Prioridad");
                        db.LoadProperty(ticket.Impacto, "Urgencia");
                        db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");
                        db.LoadProperty(ticket.ArbolAcceso.InventarioArbolAcceso.First(), "GrupoUsuarioInventarioArbol");
                        foreach (GrupoUsuarioInventarioArbol grupoinv in ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                        {
                            db.LoadProperty(grupoinv, "GrupoUsuario");
                        }
                        db.LoadProperty(ticket, "TicketGrupoUsuario");
                        foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                        {
                            db.LoadProperty(tgu, "GrupoUsuario");
                            db.LoadProperty(tgu.GrupoUsuario, "TipoUsuario");
                        }

                        HelperReportesTicket hticket = new HelperReportesTicket
                        {
                            IdTicket = ticket.Id,
                            TipoUsuario = ticket.TipoUsuario
                        };

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).IdGrupoUsuario))
                                hticket.GrupoEspecialConsulta = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).IdGrupoUsuario))
                                hticket.GrupoAtendedor = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).IdGrupoUsuario))
                                hticket.GrupoMantenimiento = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).IdGrupoUsuario))
                                hticket.GrupoOperacion = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).IdGrupoUsuario))
                                hticket.GrupoDesarrollo = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).GrupoUsuario.Descripcion;

                        hticket.IdCanal = ticket.IdCanal;
                        hticket.Canal = ticket.Canal.Descripcion;

                        hticket.IdOrganizacion = ticket.IdOrganizacion;
                        hticket.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(ticket.IdOrganizacion, false);
                        hticket.IdNivelOrganizacion = ticket.Organizacion.IdNivelOrganizacion;

                        hticket.IdUbicacion = ticket.IdUbicacion;
                        hticket.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(ticket.IdUbicacion, false);
                        hticket.IdNivelUbicacion = ticket.Ubicacion.IdNivelUbicacion;

                        hticket.IdTipificacion = ticket.IdArbolAcceso;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                        hticket.IdServicioIncidente = ticket.IdTipoArbolAcceso;
                        hticket.ServicioIncidente = ticket.TipoArbolAcceso.Descripcion;
                        hticket.Prioridad = ticket.Impacto.Prioridad.Descripcion;
                        hticket.Urgencia = ticket.Impacto.Urgencia.Descripcion;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.IdEstatus = ticket.IdEstatusTicket;
                        hticket.Estatus = ticket.EstatusTicket.Descripcion;
                        hticket.DentroSla = ticket.DentroSla;
                        hticket.Sla = ticket.DentroSla ? "DENTRO" : "FUERA";
                        hticket.FechaHora = ticket.FechaHoraAlta.ToString("dd/MM/yyyy");
                        result.Add(hticket);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        private string getorganizacion(int idorganizacion)
        {
            return new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(idorganizacion, false);
        }

        private string getUbicacion(int idUbicacion)
        {
            return new BusinessUbicacion().ObtenerDescripcionUbicacionById(idUbicacion, false);
        }
        private string gettipificacion(int idArbol)
        {
            return new BusinessArbolAcceso().ObtenerTipificacion(idArbol);
        }

        public List<HelperHits> ConsultarHits(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, int pageIndex, int pageSize)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperHits> result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                //db.ContextOptions.ProxyCreationEnabled = _proxy;
                db.ContextOptions.LazyLoadingEnabled = true;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }
                var qry = from h in db.HitConsulta
                          join tgu in db.HitGrupoUsuario on h.Id equals tgu.IdHit
                          //join gu in db.GrupoUsuario on tgu.IdGrupoUsuario equals gu.Id
                          //join r in db.Rol on tgu.IdRol equals r.Id
                          join or in db.Organizacion on h.IdOrganizacion equals or.Id
                          into orj
                          from b in orj.DefaultIfEmpty()
                          join ub in db.Ubicacion on h.IdUbicacion equals ub.Id
                          into ubj
                          from c in ubj.DefaultIfEmpty()
                          select new { h, tgu };
                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.h.Usuario.IdTipoUsuario)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains((int)q.h.IdOrganizacion)
                          select q;
                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains((int)q.h.IdUbicacion)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.h.IdArbolAcceso)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.h.Usuario.Vip)
                          select q;

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.h.FechaHoraAlta >= fechaInicio
                                    && q.h.FechaHoraAlta <= fechaFin
                              select q;

                List<HitConsulta> lstHits = qry.Select(s => s.h).Distinct().ToList();

                int totalRegistros = lstHits.Count;
                if (totalRegistros > 0)
                {
                    result = new List<HelperHits>();
                    foreach (HitConsulta hit in lstHits.Skip(pageIndex * pageSize).Take(pageSize).OrderBy(o => o.IdArbolAcceso))
                    {

                        if (result.Any(a => a.IdHit == hit.Id)) continue;
                        var dateFilter = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        var t = lstHits.Count(c => c.IdArbolAcceso == hit.IdArbolAcceso);
                        result.Add(new HelperHits
                        {
                            TipoUsuarioAbreviacion = hit.Usuario != null ? hit.Usuario.TipoUsuario.Abreviacion : "P",
                            TipoUsuarioColor = hit.Usuario != null ? hit.Usuario.TipoUsuario.Color : "#ffffff",
                            IdOrganizacion = hit.IdOrganizacion.HasValue ? int.Parse(hit.IdOrganizacion.ToString()) : 0,
                            Organizacion = hit.IdOrganizacion.HasValue ? new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(int.Parse(hit.IdOrganizacion.ToString()), false) : null,
                            IdUbicacion = hit.IdUbicacion.HasValue ? int.Parse(hit.IdUbicacion.ToString()) : 0,
                            Ubicacion = hit.IdUbicacion.HasValue ? new BusinessUbicacion().ObtenerDescripcionUbicacionById(int.Parse(hit.IdUbicacion.ToString()), false) : null,
                            Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(hit.IdArbolAcceso),
                            TipoServicio = hit.TipoArbolAcceso.Descripcion,
                            Vip = hit.Usuario != null && hit.Usuario.Vip,
                            FechaHora = hit.FechaHoraAlta.ToString("dd/MM/yyyy"),
                            Hora = hit.FechaHoraAlta.ToString("hh:mm:ss"),
                            Total = lstHits.Count(c => c.IdArbolAcceso == hit.IdArbolAcceso),
                            IdHit = hit.Id,
                            IdTipoArbolAcceso = hit.IdTipoArbolAcceso,
                            IdTipificacion = hit.IdArbolAcceso,
                            IdUsuario = hit.IdUsuario,
                            NombreUsuario = hit.Usuario != null ? hit.Usuario.NombreCompleto : string.Empty,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<HelperReportesTicket> ConsultarEncuestas(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int pageIndex, int pageSize)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperReportesTicket> result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          where t.EncuestaRespondida
                          select new { t, tgu, or, ub, ug };


                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;

                //grupos.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;
                if (responsables.Any())
                    qry = responsables.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (encuestas.Any())
                    qry = from q in qry
                          where encuestas.Contains(q.t.IdEncuesta)
                          select q;

                if (atendedores.Any())
                    qry = from q in qry
                          where atendedores.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //atendedores.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                List<Ticket> lstTickets = qry.Select(s => s.t).Distinct().ToList();

                int totalRegistros = lstTickets.Count;
                if (totalRegistros > 0)
                {
                    result = new List<HelperReportesTicket>();
                    foreach (Ticket ticket in lstTickets.Skip(pageIndex * pageSize).Take(pageSize))
                    {
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket, "TipoUsuario");
                        db.LoadProperty(ticket, "EstatusTicket");
                        db.LoadProperty(ticket, "EstatusAsignacion");
                        db.LoadProperty(ticket, "TicketEstatus");
                        db.LoadProperty(ticket, "TicketAsignacion");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "Organizacion");
                        db.LoadProperty(ticket, "Ubicacion");
                        foreach (TicketAsignacion asignacion in ticket.TicketAsignacion)
                        {

                            db.LoadProperty(asignacion, "UsuarioAsignado");
                            if (asignacion.UsuarioAsignado != null)
                            {
                                db.LoadProperty(asignacion.UsuarioAsignado, "UsuarioGrupo");
                                foreach (UsuarioGrupo grupo in asignacion.UsuarioAsignado.UsuarioGrupo)
                                {
                                    db.LoadProperty(grupo, "SubGrupoUsuario");
                                    if (grupo.SubGrupoUsuario != null)
                                        db.LoadProperty(grupo.SubGrupoUsuario, "SubRol");
                                }
                            }
                        }
                        db.LoadProperty(ticket, "ArbolAcceso");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket.Impacto, "Prioridad");
                        db.LoadProperty(ticket.Impacto, "Urgencia");
                        db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");
                        db.LoadProperty(ticket.ArbolAcceso.InventarioArbolAcceso.First(), "GrupoUsuarioInventarioArbol");
                        foreach (GrupoUsuarioInventarioArbol grupoinv in ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                        {
                            db.LoadProperty(grupoinv, "GrupoUsuario");
                        }
                        db.LoadProperty(ticket, "TicketGrupoUsuario");
                        foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                        {
                            db.LoadProperty(tgu, "GrupoUsuario");
                        }

                        HelperReportesTicket hticket = new HelperReportesTicket
                        {
                            IdTicket = ticket.Id,
                            TipoUsuario = ticket.TipoUsuario
                        };

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).IdGrupoUsuario))
                                hticket.GrupoEspecialConsulta = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).IdGrupoUsuario))
                                hticket.GrupoAtendedor = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).IdGrupoUsuario))
                                hticket.GrupoMantenimiento = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).IdGrupoUsuario))
                                hticket.GrupoOperacion = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).IdGrupoUsuario))
                                hticket.GrupoDesarrollo = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).GrupoUsuario.Descripcion;

                        hticket.IdOrganizacion = ticket.IdOrganizacion;
                        hticket.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(ticket.IdOrganizacion, false);
                        hticket.IdNivelOrganizacion = ticket.Organizacion.IdNivelOrganizacion;

                        hticket.IdUbicacion = ticket.IdUbicacion;
                        hticket.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(ticket.IdUbicacion, false);
                        hticket.IdNivelUbicacion = ticket.Ubicacion.IdNivelUbicacion;

                        hticket.IdTipificacion = ticket.IdArbolAcceso;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                        hticket.IdServicioIncidente = ticket.IdTipoArbolAcceso;
                        hticket.ServicioIncidente = ticket.TipoArbolAcceso.Descripcion;
                        hticket.Prioridad = ticket.Impacto.Prioridad.Descripcion;
                        hticket.Urgencia = ticket.Impacto.Urgencia.Descripcion;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.IdEstatus = ticket.IdEstatusTicket;
                        hticket.Estatus = ticket.EstatusTicket.Descripcion;
                        hticket.DentroSla = ticket.DentroSla;
                        hticket.Sla = ticket.DentroSla ? "DENTRO" : "FUERA";
                        hticket.FechaHora = ticket.FechaHoraAlta.ToString("dd/MM/yyyy");
                        result.Add(hticket);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<HelperReportesTicket> ConsultarEficienciaTickets(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int pageIndex, int pageSize)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperReportesTicket> result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          select t;

                if (grupos.Any())
                    qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (responsables.Any())
                    qry = responsables.Aggregate(qry, (current, grupo) => (from q in current where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.IdTipoArbolAcceso)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.IdArbolAcceso)
                          select q;

                //TODO: Filtrar nivel de atencion

                if (atendedores.Any())
                    qry = atendedores.Aggregate(qry, (current, grupo) => (from q in current where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.FechaHoraAlta >= fechaInicio
                                    && q.FechaHoraAlta <= fechaFin
                              select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.IdTipoUsuario)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.IdImpacto)
                          select q;

                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.IdUbicacion)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.IdOrganizacion)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.UsuarioLevanto.Vip)
                          select q;

                List<Ticket> lstTickets = qry.Distinct().ToList();

                int totalRegistros = lstTickets.Count;
                if (totalRegistros > 0)
                {
                    result = new List<HelperReportesTicket>();
                    foreach (Ticket ticket in lstTickets.Skip(pageIndex * pageSize).Take(pageSize))
                    {
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket, "TipoUsuario");
                        db.LoadProperty(ticket, "EstatusTicket");
                        db.LoadProperty(ticket, "EstatusAsignacion");
                        db.LoadProperty(ticket, "TicketEstatus");
                        db.LoadProperty(ticket, "TicketAsignacion");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "Organizacion");
                        db.LoadProperty(ticket, "Ubicacion");
                        foreach (TicketAsignacion asignacion in ticket.TicketAsignacion)
                        {

                            db.LoadProperty(asignacion, "UsuarioAsignado");
                            if (asignacion.UsuarioAsignado != null)
                            {
                                db.LoadProperty(asignacion.UsuarioAsignado, "UsuarioGrupo");
                                foreach (UsuarioGrupo grupo in asignacion.UsuarioAsignado.UsuarioGrupo)
                                {
                                    db.LoadProperty(grupo, "SubGrupoUsuario");
                                    if (grupo.SubGrupoUsuario != null)
                                        db.LoadProperty(grupo.SubGrupoUsuario, "SubRol");
                                }
                            }
                        }
                        db.LoadProperty(ticket, "ArbolAcceso");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket.Impacto, "Prioridad");
                        db.LoadProperty(ticket.Impacto, "Urgencia");
                        db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");
                        db.LoadProperty(ticket.ArbolAcceso.InventarioArbolAcceso.First(), "GrupoUsuarioInventarioArbol");
                        foreach (GrupoUsuarioInventarioArbol grupoinv in ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                        {
                            db.LoadProperty(grupoinv, "GrupoUsuario");
                        }
                        db.LoadProperty(ticket, "TicketGrupoUsuario");
                        foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                        {
                            db.LoadProperty(tgu, "GrupoUsuario");
                        }

                        HelperReportesTicket hticket = new HelperReportesTicket
                        {
                            IdTicket = ticket.Id,
                            TipoUsuario = ticket.TipoUsuario
                        };

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).IdGrupoUsuario))
                                hticket.GrupoEspecialConsulta = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).IdGrupoUsuario))
                                hticket.GrupoAtendedor = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).IdGrupoUsuario))
                                hticket.GrupoMantenimiento = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).IdGrupoUsuario))
                                hticket.GrupoOperacion = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo))
                            if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).IdGrupoUsuario))
                                hticket.GrupoDesarrollo = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).GrupoUsuario.Descripcion;

                        hticket.IdOrganizacion = ticket.IdOrganizacion;
                        hticket.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(ticket.IdOrganizacion, false);
                        hticket.IdNivelOrganizacion = ticket.Organizacion.IdNivelOrganizacion;

                        hticket.IdUbicacion = ticket.IdUbicacion;
                        hticket.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(ticket.IdUbicacion, false);
                        hticket.IdNivelUbicacion = ticket.Ubicacion.IdNivelUbicacion;

                        hticket.IdTipificacion = ticket.IdArbolAcceso;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                        hticket.IdServicioIncidente = ticket.IdTipoArbolAcceso;
                        hticket.ServicioIncidente = ticket.TipoArbolAcceso.Descripcion;
                        hticket.Prioridad = ticket.Impacto.Prioridad.Descripcion;
                        hticket.Urgencia = ticket.Impacto.Urgencia.Descripcion;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.IdEstatus = ticket.IdEstatusTicket;
                        hticket.Estatus = ticket.EstatusTicket.Descripcion;
                        hticket.DentroSla = ticket.DentroSla;
                        hticket.Sla = ticket.DentroSla ? "DENTRO" : "FUERA";
                        hticket.FechaHora = ticket.FechaHoraAlta.ToString("dd/MM/yyyy");
                        result.Add(hticket);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<HelperReportesTicket> ConsultaEncuestaPregunta(int idUsuario, int idEncuesta, Dictionary<string, DateTime> fechas, int tipoFecha, int tipoEncuesta, int idPregunta, int respuesta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperReportesTicket> result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }
                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join ep in db.EncuestaPregunta on new { idenctick = (int)t.IdEncuesta, ids = e.Id } equals new { idenctick = ep.IdEncuesta, ids = ep.IdEncuesta }
                          join re in db.RespuestaEncuesta on
                          new { idTickTick = (int?)t.Id, idEncTick = (int)t.IdEncuesta, idEncPadre = e.Id, idPreg = ep.Id } equals
                          new { idTickTick = re.IdTicket, idEncTick = re.IdEncuesta, idEncPadre = re.IdEncuesta, idPreg = re.IdPregunta }
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          where t.EncuestaRespondida && t.IdEncuesta == idEncuesta
                          && re.IdPregunta == idPregunta && re.ValorRespuesta == respuesta
                          select new { t, tgu, ug, e, re, ep };



                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                var lstTickets = qry.Select(s => s.t).Distinct().ToList();
                int totalRegistros = lstTickets.Count;
                if (totalRegistros > 0)
                {
                    result = new List<HelperReportesTicket>();
                    foreach (Ticket ticket in lstTickets)
                    {
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket, "TipoUsuario");
                        db.LoadProperty(ticket, "EstatusTicket");
                        db.LoadProperty(ticket, "EstatusAsignacion");
                        db.LoadProperty(ticket, "TicketEstatus");
                        db.LoadProperty(ticket, "TicketAsignacion");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "Organizacion");
                        db.LoadProperty(ticket, "Ubicacion");
                        db.LoadProperty(ticket, "RespuestaEncuesta");
                        foreach (TicketAsignacion asignacion in ticket.TicketAsignacion)
                        {

                            db.LoadProperty(asignacion, "UsuarioAsignado");
                            if (asignacion.UsuarioAsignado != null)
                            {
                                db.LoadProperty(asignacion.UsuarioAsignado, "UsuarioGrupo");
                                foreach (UsuarioGrupo grupo in asignacion.UsuarioAsignado.UsuarioGrupo)
                                {
                                    db.LoadProperty(grupo, "SubGrupoUsuario");
                                    if (grupo.SubGrupoUsuario != null)
                                        db.LoadProperty(grupo.SubGrupoUsuario, "SubRol");
                                }
                            }
                        }
                        db.LoadProperty(ticket, "ArbolAcceso");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket.Impacto, "Prioridad");
                        db.LoadProperty(ticket.Impacto, "Urgencia");
                        db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");
                        db.LoadProperty(ticket.ArbolAcceso.InventarioArbolAcceso.First(), "GrupoUsuarioInventarioArbol");
                        foreach (GrupoUsuarioInventarioArbol grupoinv in ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                        {
                            db.LoadProperty(grupoinv, "GrupoUsuario");
                        }
                        db.LoadProperty(ticket, "TicketGrupoUsuario");
                        foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                        {
                            db.LoadProperty(tgu, "GrupoUsuario");
                        }

                        HelperReportesTicket hticket = new HelperReportesTicket
                        {
                            IdTicket = ticket.Id,
                            TipoUsuario = ticket.TipoUsuario
                        };

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos))
                            //    if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.EspecialDeConsulta).IdGrupoUsuario))
                            hticket.GrupoEspecialConsulta = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente))
                            //    if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeAtención).IdGrupoUsuario))
                            hticket.GrupoAtendedor = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido))
                            //    if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeInformaciónPublicada).IdGrupoUsuario))
                            hticket.GrupoMantenimiento = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación))
                            //    if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).IdGrupoUsuario))
                            hticket.GrupoOperacion = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación).GrupoUsuario.Descripcion;

                        if (ticket.TicketGrupoUsuario.Any(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo))
                            //    if (grupos.Contains(ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).IdGrupoUsuario))
                            hticket.GrupoDesarrollo = ticket.TicketGrupoUsuario.First(f => f.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo).GrupoUsuario.Descripcion;

                        hticket.IdOrganizacion = ticket.IdOrganizacion;
                        hticket.Organizacion = new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(ticket.IdOrganizacion, false);
                        hticket.IdNivelOrganizacion = ticket.Organizacion.IdNivelOrganizacion;

                        hticket.IdUbicacion = ticket.IdUbicacion;
                        hticket.Ubicacion = new BusinessUbicacion().ObtenerDescripcionUbicacionById(ticket.IdUbicacion, false);
                        hticket.IdNivelUbicacion = ticket.Ubicacion.IdNivelUbicacion;

                        hticket.IdTipificacion = ticket.IdArbolAcceso;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                        hticket.IdServicioIncidente = ticket.IdTipoArbolAcceso;
                        hticket.ServicioIncidente = ticket.TipoArbolAcceso.Descripcion;
                        hticket.Prioridad = ticket.Impacto.Prioridad.Descripcion;
                        hticket.Urgencia = ticket.Impacto.Urgencia.Descripcion;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.IdEstatus = ticket.IdEstatusTicket;
                        hticket.Estatus = ticket.EstatusTicket.Descripcion;
                        hticket.DentroSla = ticket.DentroSla;
                        hticket.Sla = ticket.DentroSla ? "DENTRO" : "FUERA";
                        //hticket.Respuesta = ticket.RespuestaEncuesta
                        hticket.FechaHora = ticket.FechaHoraAlta.ToString("dd/MM/yyyy");
                        result.Add(hticket);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        #endregion Consultas
        #region Graficas
        public DataTable GraficarConsultaTicket(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            DateTime fechaInicio = new DateTime();
            int conteo = 1;
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          select new { t, tgu };

                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;
                if (ubicaciones.Any())
                {
                    List<int> lstUbicaciones = new BusinessUbicacion().ObtenerUbicacionesByEstado(ubicaciones);
                    qry = from q in qry
                          where lstUbicaciones.Contains(q.t.IdUbicacion)
                          select q;
                }
                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.t.IdArbolAcceso)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (estatus.Any())
                    qry = from q in qry
                          where estatus.Contains(q.t.IdEstatusTicket)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (fechas != null)
                {
                    if (fechas.Count == 2)
                    {
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;
                    }
                }

                List<Ticket> lstTickets = qry.Select(s => s.t).Distinct().ToList();

                if (lstTickets.Any())
                {
                    result = new DataTable("dt");
                    result.Columns.Add(new DataColumn("Id"));
                    result.Columns.Add(new DataColumn("Descripcion"));

                    List<string> lstFechas = lstTickets.OrderBy(o => o.FechaHoraAlta).Distinct().Select(s => s.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                result.Columns.Add(fecha);
                            }
                            break;
                        case 2:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                conteo++;
                            }
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                            }
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                            }
                            break;
                    }

                    int row = 0;
                    switch (stack)
                    {
                        case "Ubicaciones":

                            foreach (int idUbicacion in lstTickets.Select(s => s.IdUbicacion).Distinct())
                            {
                                result.Rows.Add(idUbicacion, new BusinessUbicacion().ObtenerDescripcionUbicacionById(idUbicacion, false));
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdUbicacion == idUbicacion);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdUbicacion == idUbicacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdUbicacion == idUbicacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdUbicacion == idUbicacion);
                                            break;
                                    }

                                }
                                row++;
                            }
                            break;
                        case "Organizaciones":
                            foreach (int idOrganizacion in lstTickets.Select(s => s.IdOrganizacion).Distinct())
                            {
                                result.Rows.Add(idOrganizacion, new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(idOrganizacion, false));
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdOrganizacion == idOrganizacion);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Tipo Ticket":
                            foreach (int idTipoArbolAcceso in lstTickets.Select(s => s.IdTipoArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idTipoArbolAcceso, new BusinessTipoArbolAcceso().ObtenerTiposArbolAcceso(false).Single(s => s.Id == idTipoArbolAcceso).Descripcion);
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Tipificaciones":
                            foreach (int idArbol in lstTickets.Select(s => s.IdArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idArbol, new BusinessArbolAcceso().ObtenerTipificacion(idArbol));
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdArbolAcceso == idArbol);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Estatus Ticket":
                            foreach (int idEstatusticket in lstTickets.Select(s => s.IdEstatusTicket).Distinct())
                            {
                                result.Rows.Add(idEstatusticket, new BusinessEstatus().ObtenerEstatusTicket(false).Single(s => s.Id == idEstatusticket).Descripcion);
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "SLA":
                            foreach (bool dentroSla in lstTickets.Select(s => s.DentroSla).Distinct())
                            {
                                result.Rows.Add(dentroSla ? 1 : 0, dentroSla ? "Dentro" : "Fuera");
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.DentroSla == dentroSla);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.DentroSla == dentroSla);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.DentroSla == dentroSla);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.DentroSla == dentroSla);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Canal":
                            foreach (int idCanal in lstTickets.Select(s => s.IdCanal).Distinct())
                            {
                                result.Rows.Add(idCanal, new BusinessCanal().ObtenerCanalById(idCanal).Descripcion);
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdCanal == idCanal);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdCanal == idCanal);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdCanal == idCanal);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdCanal == idCanal);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public DataTable GraficarConsultaTicketEficiencia(int idUsuario, List<int> tipoUsuario, List<int> area, List<int> grupos, List<int> agentes, List<int> estatusAsignacion, List<int> canal, List<int> tipoArbol, List<int> opciones, List<int> estatus, List<int> prioridad, List<bool?> sla, List<bool?> vip, List<int> organizaciones, List<int> ubicaciones, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            DateTime fechaInicio = new DateTime();
            int conteo = 1;
            try
            {
                bool restaMes = false;
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          select new { t, tgu };

                if (tipoUsuario.Any())
                    qry = from q in qry
                          where tipoUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (area.Any())
                    qry = from q in qry
                          where area.Contains(q.t.ArbolAcceso.IdArea)
                          select q;


                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;

                if (agentes.Any())
                    qry = from q in qry
                          where agentes.Contains((int)q.t.IdUsuarioUltimoAgenteAsignado)
                          select q;

                if (estatusAsignacion.Any())
                    qry = from q in qry
                          where estatusAsignacion.Contains(q.t.IdEstatusAsignacion)
                          select q;

                if (canal.Any())
                    qry = from q in qry
                          where canal.Contains(q.t.IdCanal)
                          select q;

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                if (opciones.Any())
                    qry = from q in qry
                          where opciones.Contains(q.t.IdArbolAcceso)
                          select q;

                if (estatus.Any())
                    qry = from q in qry
                          where estatus.Contains(q.t.IdEstatusTicket)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;



                if (organizaciones.Any())
                {
                    List<int> lstOrganizaciones = new BusinessOrganizacion().ObtenerOrganizacionesByHolding(organizaciones);
                    qry = from q in qry
                          where lstOrganizaciones.Contains(q.t.IdOrganizacion)
                          select q;
                }
                if (ubicaciones.Any())
                {
                    List<int> lstUbicaciones = new BusinessUbicacion().ObtenerUbicacionesByEstado(ubicaciones);
                    qry = from q in qry
                          where lstUbicaciones.Contains(q.t.IdUbicacion)
                          select q;
                }

                if (fechas != null)
                {
                    if (fechas.Count == 2)
                    {
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;
                    }
                }

                List<Ticket> lstTickets = qry.Select(s => s.t).Distinct().ToList();


                string rango = string.Empty;
                if (lstTickets.Any())
                {
                    result = new DataTable("dt");
                    result.Columns.Add(new DataColumn("Id", typeof(int)));
                    result.Columns.Add(new DataColumn("Descripcion", typeof(string)));
                    result.Columns.Add(new DataColumn("Color", typeof(string)));

                    if (fechas == null)
                    {
                        List<string> lstFechas = lstTickets.OrderBy(o => o.FechaUltimoMovimiento).Distinct().Select(s => s.FechaUltimoMovimiento.ToString("dd/MM/yyyy")).Distinct().ToList();
                        switch (tipoFecha)
                        {
                            case 1:
                                foreach (string fecha in lstFechas)
                                {
                                    result.Columns.Add(fecha);
                                }
                                break;
                            case 2:
                                foreach (string fecha in lstFechas)
                                {
                                    if (!result.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.Parse(fecha).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToShortDateString()))
                                        result.Columns.Add(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(DateTime.Parse(fecha).Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToShortDateString());
                                    conteo++;
                                }
                                break;
                            case 3:
                                foreach (string fecha in lstFechas)
                                {
                                    var firstDayOfMonth = new DateTime(DateTime.Parse(fecha).Year, DateTime.Parse(fecha).Month, 1);

                                    if (!result.Columns.Contains(firstDayOfMonth.ToShortDateString()))
                                        result.Columns.Add(firstDayOfMonth.ToShortDateString());
                                }
                                break;
                            case 4:
                                foreach (string fecha in lstFechas)
                                {
                                    DateTime firstDay = new DateTime(DateTime.Parse(fecha).Year, 1, 1);
                                    if (!result.Columns.Contains(firstDay.ToShortDateString()))
                                        result.Columns.Add(firstDay.ToShortDateString());
                                }
                                break;
                        }
                    }
                    else
                    {
                        restaMes = true;
                        fechaInicio = DateTime.Parse(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"));
                        fechaFin = DateTime.Parse(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"));
                        if (fechaInicio > fechaFin)
                            throw new Exception("Fechas incorrectas");

                        DateTime tmpFecha = (DateTime)fechaInicio;
                        bool continua = true;
                        while (continua)
                        {
                            switch (tipoFecha)
                            {
                                case 1:
                                    if (tmpFecha < fechaFin)
                                    {
                                        result.Columns.Add(new DataColumn(tmpFecha.ToShortDateString(), typeof(int)));
                                        tmpFecha = tmpFecha.AddDays(1);
                                    }
                                    else
                                        continua = false;
                                    rango = "Diario";
                                    break;
                                case 2:
                                    if (tmpFecha < fechaFin)
                                    {
                                        if (!result.Columns.Contains(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToShortDateString()))
                                            result.Columns.Add(new DataColumn(BusinessCadenas.Fechas.ObtenerFechaInicioSemana(tmpFecha.Year, CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(tmpFecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)).ToShortDateString(), typeof(int)));
                                        tmpFecha = tmpFecha.AddDays(7);
                                    }
                                    else
                                        continua = false;
                                    rango = "Semanal";
                                    break;
                                case 3:
                                    if (tmpFecha < fechaFin)
                                    {
                                        var firstDayOfMonth = new DateTime(tmpFecha.Year, tmpFecha.Month, 1);
                                        if (!result.Columns.Contains(firstDayOfMonth.ToShortDateString()))
                                            result.Columns.Add(firstDayOfMonth.ToShortDateString());
                                        tmpFecha = tmpFecha.AddMonths(1);
                                    }
                                    else
                                        continua = false;
                                    rango = "Mensual";
                                    break;
                                case 4:
                                    if (tmpFecha < fechaFin)
                                    {
                                        if (!result.Columns.Contains(tmpFecha.ToShortDateString()))
                                            result.Columns.Add(tmpFecha.ToShortDateString());
                                        tmpFecha = tmpFecha.AddYears(1);
                                    }
                                    else
                                        continua = false;
                                    rango = "Anual";
                                    break;
                            }
                        }


                    }


                    int row = 0;
                    switch (stack)
                    {
                        #region EstatusTicket
                        case "Estatus Ticket":
                            foreach (int idEstatusticket in lstTickets.Select(s => s.IdEstatusTicket).Distinct())
                            {
                                result.Rows.Add(idEstatusticket, new BusinessEstatus().ObtenerEstatusTicket(false).Single(s => s.Id == idEstatusticket).Descripcion, new BusinessEstatus().ObtenerEstatusTicket(false).Single(s => s.Id == idEstatusticket).ColorGrafico);
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM")
                                                && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy")
                                                && c.IdEstatusTicket == idEstatusticket);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion EstatusTicket

                        #region TipoTicket
                        case "Tipo Ticket":
                            foreach (int idTipoArbolAcceso in lstTickets.Select(s => s.IdTipoArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idTipoArbolAcceso, new BusinessTipoArbolAcceso().ObtenerTiposArbolAcceso(false).Single(s => s.Id == idTipoArbolAcceso).Descripcion, new BusinessTipoArbolAcceso().ObtenerTiposArbolAcceso(false).Single(s => s.Id == idTipoArbolAcceso).ColorGrafico);
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion TipoTicket

                        #region Sla
                        case "SLA":
                            foreach (bool dentroSla in lstTickets.Select(s => s.DentroSla).Distinct())
                            {
                                result.Rows.Add(dentroSla ? 1 : 0, dentroSla ? "Dentro" : "Fuera", dentroSla ? "#B5E6A1" : "#E36F5B");
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.DentroSla == dentroSla);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.DentroSla == dentroSla);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && c.DentroSla == dentroSla);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && c.DentroSla == dentroSla);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion Sla

                        #region Tipificacion
                        case "Tipificaciones":
                            foreach (int idArbol in lstTickets.Select(s => s.IdArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idArbol, new BusinessArbolAcceso().ObtenerTipificacion(idArbol));
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && c.IdArbolAcceso == idArbol);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion Tipificacion

                        #region Estatus Asignacion
                        case "Estatus Asignacion":
                            foreach (int idEstatusAsignacion in lstTickets.Select(s => s.IdEstatusAsignacion).Distinct())
                            {
                                result.Rows.Add(idEstatusAsignacion, new BusinessEstatus().ObtenerEstatusAsignacion(false).Single(s => s.Id == idEstatusAsignacion).Descripcion, new BusinessEstatus().ObtenerEstatusAsignacion(false).Single(s => s.Id == idEstatusAsignacion).ColorGrafico);
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdEstatusAsignacion == idEstatusAsignacion);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.IdEstatusAsignacion == idEstatusAsignacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && c.IdEstatusAsignacion == idEstatusAsignacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && c.IdEstatusAsignacion == idEstatusAsignacion);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion Estatus Asignacion

                        #region Canal
                        case "Canal":
                            foreach (int idCanal in lstTickets.Select(s => s.IdCanal).Distinct())
                            {
                                result.Rows.Add(idCanal, new BusinessCanal().ObtenerCanalById(idCanal).Descripcion, new BusinessCanal().ObtenerCanalById(idCanal).ColorGrafico);
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdCanal == idCanal);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.IdCanal == idCanal);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && c.IdCanal == idCanal);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && c.IdCanal == idCanal);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion Canal

                        #region Organizacion
                        case "Organizaciones":
                            foreach (int idOrganizacion in lstTickets.Select(s => s.IdOrganizacion).Distinct())
                            {
                                result.Rows.Add(idOrganizacion, new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(idOrganizacion, false));
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && c.IdOrganizacion == idOrganizacion);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        #endregion Organizacion

                        #region Ubicacion
                        case "Ubicaciones":
                            List<KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Estado> lstEstado = new BusinessDomicilioSistema().ObtenerEstados(false);
                            foreach (KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Estado estado in lstEstado)
                            {
                                List<int> lstUbicaciones = new BusinessUbicacion().ObtenerUbicacionesByEstado(new List<int> { estado.Id });

                                if (lstTickets.Any(c => lstUbicaciones.Contains(c.IdUbicacion)))
                                {
                                    result.Rows.Add(estado.Id, estado.Descripcion);
                                    for (int i = 3; i < result.Columns.Count; i++)
                                    {
                                        switch (tipoFecha)
                                        {
                                            case 1:
                                                result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && lstUbicaciones.Contains(c.IdUbicacion));
                                                break;
                                            case 2:

                                                result.Rows[row][i] = lstTickets.Count(c =>
                                                    DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[i].ColumnName)
                                                && DateTime.Parse(c.FechaUltimoMovimiento.ToString("dd/MM/yyyy")) < DateTime.Parse(result.Columns[i].ColumnName).AddDays(7)
                                                    && lstUbicaciones.Contains(c.IdUbicacion));
                                                break;
                                            case 3:
                                                result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("MM") == DateTime.Parse(result.Columns[i].ColumnName).ToString("MM") && lstUbicaciones.Contains(c.IdUbicacion));
                                                break;
                                            case 4:
                                                result.Rows[row][i] = lstTickets.Count(c => c.FechaUltimoMovimiento.ToString("yyyy") == DateTime.Parse(result.Columns[i].ColumnName).ToString("yyyy") && lstUbicaciones.Contains(c.IdUbicacion));
                                                break;
                                        }
                                    }
                                    row++;
                                }
                            }
                            break;
                        #endregion Ubicacion
                    }

                    switch (tipoFecha)
                    {
                        case 1:
                            for (int i = 3; i < result.Columns.Count; i++)
                            {
                                result.Columns[i].ColumnName = DateTime.Parse(result.Columns[i].ColumnName).ToString("dd MMM yy").Replace(".", string.Empty);
                            }
                            break;
                        case 2:
                            for (int i = 3; i < result.Columns.Count; i++)
                            {
                                result.Columns[i].ColumnName = DateTime.Parse(result.Columns[i].ColumnName).ToString("dd MMM yy").Replace(".", string.Empty);
                            }
                            break;
                        case 3:
                            for (int i = 3; i < result.Columns.Count; i++)
                            {
                                result.Columns[i].ColumnName = DateTime.Parse(result.Columns[i].ColumnName).ToString("dd MMM yy").Replace(".", string.Empty);
                            }
                            break;
                        case 4:
                            for (int i = 3; i < result.Columns.Count; i++)
                            {
                                result.Columns[i].ColumnName = DateTime.Parse(result.Columns[i].ColumnName).ToString("dd MMM yy").Replace(".", string.Empty);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }


        public string GraficarConsultaTicketGeografico(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipoArbol, List<int> tipificacion, List<int> prioridad, List<int> estatus, List<bool?> sla, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            string result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          join d in db.Domicilio on ub.IdCampus equals d.IdCampus
                          join col in db.Colonia on d.IdColonia equals col.Id
                          join m in db.Municipio on col.IdMunicipio equals m.Id
                          join e in db.Estado on m.IdEstado equals e.Id
                          select new { t, e, tgu };

                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;
                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;
                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.t.IdArbolAcceso)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (estatus.Any())
                    qry = from q in qry
                          where estatus.Contains(q.t.IdEstatusTicket)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (fechas != null)
                {
                    if (fechas.Count == 2)
                    {
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;
                    }
                }

                var lstTickets = (from q in qry.Distinct()
                                  group new { q.e.RegionCode } by new { q.e.RegionCode }
                                      into g
                                      select new { g.Key.RegionCode, Hits = g.Count() }).ToList();
                if (lstTickets.Any())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Region");
                    dt.Columns.Add("Tickets");
                    foreach (var data in lstTickets)
                    {
                        dt.Rows.Add(data.RegionCode, data.Hits);
                    }
                    result = dt.Columns.Cast<DataColumn>().Aggregate("[\n[", (current, column) => current + string.Format("'{0}', ", column.ColumnName));
                    result = result.Trim().TrimEnd(',') + "], \n";
                    result = dt.Rows.Cast<DataRow>().Aggregate(result, (current, row) => current + string.Format("['{0}', {1}]\n", row[0], row[1]));
                    result += "]";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public DataTable GraficarConsultaHits(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            DateTime fechaInicio = new DateTime();
            int conteo = 1;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from h in db.HitConsulta
                          join tgu in db.HitGrupoUsuario on h.Id equals tgu.IdHit
                          join or in db.Organizacion on h.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on h.IdUbicacion equals ub.Id
                          select new { h, tgu };

                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.HitGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.h.Usuario.IdTipoUsuario)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains((int)q.h.IdOrganizacion)
                          select q;
                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains((int)q.h.IdUbicacion)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.h.IdArbolAcceso)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.h.Usuario.Vip)
                          select q;

                if (fechas != null)
                {
                    if (fechas.Count == 2)
                    {
                        qry = from q in qry
                              where q.h.FechaHoraAlta >= fechaInicio
                                    && q.h.FechaHoraAlta <= fechaFin
                              select q;
                    }
                }

                List<HitConsulta> lstTickets = qry.Select(s => s.h).Distinct().ToList();

                if (lstTickets.Any())
                {
                    result = new DataTable("dt");
                    result.Columns.Add(new DataColumn("Id"));
                    result.Columns.Add(new DataColumn("Descripcion"));

                    List<string> lstFechas = lstTickets.OrderBy(o => o.FechaHoraAlta).Distinct().Select(s => s.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                result.Columns.Add(fecha);
                            }
                            break;
                        case 2:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                conteo++;
                            }
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                            }
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                            }
                            break;
                    }

                    int row = 0;
                    switch (stack)
                    {
                        case "Ubicaciones":

                            foreach (int idUbicacion in lstTickets.Select(s => s.IdUbicacion).Distinct())
                            {
                                result.Rows.Add(idUbicacion, new BusinessUbicacion().ObtenerDescripcionUbicacionById(idUbicacion, false));
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdUbicacion == idUbicacion);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdUbicacion == idUbicacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdUbicacion == idUbicacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdUbicacion == idUbicacion);
                                            break;
                                    }

                                }
                                row++;
                            }
                            break;
                        case "Organizaciones":
                            foreach (int idOrganizacion in lstTickets.Select(s => s.IdOrganizacion).Distinct())
                            {
                                result.Rows.Add(idOrganizacion, new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(idOrganizacion, false));
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdOrganizacion == idOrganizacion);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Tipo Ticket":
                            foreach (int idTipoArbolAcceso in lstTickets.Select(s => s.IdTipoArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idTipoArbolAcceso, new BusinessTipoArbolAcceso().ObtenerTiposArbolAcceso(false).Single(s => s.Id == idTipoArbolAcceso).Descripcion);
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Tipificaciones":
                            foreach (int idArbol in lstTickets.Select(s => s.IdArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idArbol, new BusinessArbolAcceso().ObtenerTipificacion(idArbol));
                                for (int i = 2; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c =>
                                                DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                                && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdArbolAcceso == idArbol);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdArbolAcceso == idArbol);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public string GraficarConsultaHitsGeografico(int idUsuario, List<int> grupos, List<int> tiposUsuario, List<int> organizaciones, List<int> ubicaciones, List<int> tipificacion, List<bool?> vip, Dictionary<string, DateTime> fechas, List<int> filtroStackColumn, string stack, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            string result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from h in db.HitConsulta
                          join tgu in db.HitGrupoUsuario on h.Id equals tgu.IdHit
                          join or in db.Organizacion on h.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on h.IdUbicacion equals ub.Id
                          join d in db.Domicilio on ub.IdCampus equals d.IdCampus
                          join col in db.Colonia on d.IdColonia equals col.Id
                          join m in db.Municipio on col.IdMunicipio equals m.Id
                          join e in db.Estado on m.IdEstado equals e.Id
                          select new { h, e, tgu };

                if (grupos != null && grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.h.HitGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tiposUsuario != null && tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.h.Usuario.IdTipoUsuario)
                          select q;

                if (organizaciones != null && organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains((int)q.h.IdOrganizacion)
                          select q;
                if (ubicaciones != null && ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains((int)q.h.IdUbicacion)
                          select q;

                if (tipificacion != null && tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.h.IdArbolAcceso)
                          select q;

                if (vip != null && vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.h.Usuario.Vip)
                          select q;

                if (fechas != null)
                {
                    if (fechas.Count == 2)
                    {
                        qry = from q in qry
                              where q.h.FechaHoraAlta >= fechaInicio
                                    && q.h.FechaHoraAlta <= fechaFin
                              select q;
                    }
                }

                var lstTickets = (from q in qry.Distinct()
                                  group new { q.e.RegionCode } by new { q.e.RegionCode }
                                      into g
                                      select new { g.Key.RegionCode, Hits = g.Count() }).ToList();

                if (lstTickets.Any())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Region");
                    dt.Columns.Add("Hits");
                    foreach (var data in lstTickets)
                    {
                        dt.Rows.Add(data.RegionCode, data.Hits);
                    }
                    result = dt.Columns.Cast<DataColumn>().Aggregate("[\n[", (current, column) => current + string.Format("'{0}', ", column.ColumnName));
                    result = result.Trim().TrimEnd(',') + "], \n";
                    result = dt.Rows.Cast<DataRow>().Aggregate(result, (current, row) => current + string.Format("['{0}', {1}]\n", row[0], row[1]));
                    result += "]";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public DataTable GraficarConsultaEncuesta(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            DateTime fechaInicio = new DateTime();
            int conteo = 1;
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join er in db.RespuestaEncuesta on new { idtick = (int?)t.Id, padre = e.Id } equals new { idtick = er.IdTicket, padre = er.IdEncuesta }
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          where t.EncuestaRespondida
                          select new { t, tgu, or, ub, ug, e, er };


                if (grupos.Any())
                    qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;
                if (responsables.Any())
                    qry = responsables.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (encuestas.Any())
                    qry = from q in qry
                          where encuestas.Contains(q.t.IdEncuesta)
                          select q;

                if (atendedores.Any())
                    qry = atendedores.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                var lstTickets = qry.Distinct().ToList();

                if (lstTickets.Any())
                {
                    result = new DataTable("dt");
                    result.Columns.Add(new DataColumn("Id"));
                    result.Columns.Add(new DataColumn("Descripcion"));
                    result.Columns.Add(new DataColumn("Total"));

                    List<string> lstFechas = lstTickets.OrderBy(o => o.t.FechaHoraAlta).Distinct().Select(s => s.t.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                result.Columns.Add(fecha);
                            }
                            break;
                        case 2:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                conteo++;
                            }
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                            }
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                            }
                            break;
                    }

                    int row = 0;
                    foreach (int? idEncuesta in lstTickets.Select(s => s.t.IdEncuesta).Distinct())
                    {
                        if (idEncuesta == null) continue;
                        result.Rows.Add(idEncuesta, new BusinessEncuesta().ObtenerEncuestaById((int)idEncuesta).Descripcion);
                        var enumeracion = lstTickets.Where(w => w.t.IdEncuesta == idEncuesta).SelectMany(s => s.e.RespuestaEncuesta).Distinct().ToList().GroupBy(ge => new { ge.IdTicket, ge.IdEncuesta, ge.Ponderacion })
                                .Distinct().Select(ssum =>
                                        new
                                        {
                                            ssum.Key.IdTicket,
                                            ssum.Key.IdEncuesta,
                                            SumaEncuesta = ssum.Sum(sumas => sumas.Ponderacion)
                                        })
                                .Distinct().ToList();
                        var sumEncuesta = enumeracion.Select(s => new { s.IdTicket, TotalEncuesta = enumeracion.Where(w => w.IdTicket == s.IdTicket).Sum(sm => sm.SumaEncuesta) }).Distinct();
                        var total = sumEncuesta.Average(av => av.TotalEncuesta);

                        result.Rows[row][2] = total;
                        for (int i = 3; i < result.Columns.Count; i++)
                        {
                            switch (tipoFecha)
                            {
                                case 1:
                                    result.Rows[row][i] =
                                        lstTickets.Select(s => s.t).Distinct().Count(c => c.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.IdEncuesta == idEncuesta);
                                    break;
                                case 2:

                                    result.Rows[row][i] = lstTickets.Select(s => s.t).Distinct().Count(c =>
                                        DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                        && DateTime.Parse(c.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1]))
                                        && c.IdEncuesta == idEncuesta);
                                    break;
                                case 3:
                                    result.Rows[row][i] = lstTickets.Select(s => s.t).Distinct().Count(c => c.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.IdEncuesta == idEncuesta);
                                    break;
                                case 4:
                                    result.Rows[row][i] = lstTickets.Select(s => s.t).Distinct().Count(c => c.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.IdEncuesta == idEncuesta);
                                    break;
                            }

                        }
                        row++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public string GraficarConsultaEncuestaGeografica(int idUsuario, List<int> grupos, List<int> tipoArbol, List<int> responsables, List<int?> encuestas, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<bool?> sla, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            string result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join re in db.RespuestaEncuesta on new { idTickTick = (int?)t.Id, idEncTick = (int)t.IdEncuesta, idEncPadre = e.Id } equals new { idTickTick = re.IdTicket, idEncTick = re.IdEncuesta, idEncPadre = re.IdEncuesta }
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          join d in db.Domicilio on ub.IdCampus equals d.IdCampus
                          join col in db.Colonia on d.IdColonia equals col.Id
                          join m in db.Municipio on col.IdMunicipio equals m.Id
                          join es in db.Estado on m.IdEstado equals es.Id
                          where t.EncuestaRespondida
                          select new { t, es, ug };


                if (grupos.Any())
                    qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;
                if (responsables.Any())
                    qry = responsables.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (encuestas.Any())
                    qry = from q in qry
                          where encuestas.Contains(q.t.IdEncuesta)
                          select q;

                if (atendedores.Any())
                    qry = atendedores.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (sla.Any())
                    qry = from q in qry
                          where sla.Contains(q.t.DentroSla)
                          select q;

                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                var lstTickets = (from q in qry.Distinct()
                                  group new { q.es.RegionCode } by new { q.es.RegionCode }
                                      into g
                                      select new { g.Key.RegionCode, Hits = g.Count() }).ToList();

                if (lstTickets.Any())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Region");
                    dt.Columns.Add("Hits");
                    foreach (var data in lstTickets)
                    {
                        dt.Rows.Add(data.RegionCode, data.Hits);
                    }
                    result = dt.Columns.Cast<DataColumn>().Aggregate("[\n[", (current, column) => current + string.Format("'{0}', ", column.ColumnName));
                    result = result.Trim().TrimEnd(',') + "], \n";
                    result = dt.Rows.Cast<DataRow>().Aggregate(result, (current, row) => current + string.Format("['{0}', {1}]\n", row[0], row[1]));
                    result += "]";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<DataTable> GraficarConsultaEncuestaPregunta(int idUsuario, int idEncuesta, Dictionary<string, DateTime> fechas, int tipoFecha, int tipoEncuesta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<DataTable> result;
            DateTime fechaInicio = new DateTime();
            int conteo = 1;
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }
                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join ep in db.EncuestaPregunta on new { idenctick = (int)t.IdEncuesta, ids = e.Id } equals new { idenctick = ep.IdEncuesta, ids = ep.IdEncuesta }
                          join re in db.RespuestaEncuesta on
                          new { idTickTick = (int?)t.Id, idEncTick = (int)t.IdEncuesta, idEncPadre = e.Id, idPreg = ep.Id } equals
                          new { idTickTick = re.IdTicket, idEncTick = re.IdEncuesta, idEncPadre = re.IdEncuesta, idPreg = re.IdPregunta }
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          where t.EncuestaRespondida && t.IdEncuesta == idEncuesta
                          select new { t, tgu, ug, e, re, ep };

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                var lstTickets = qry.Distinct().ToList();
                result = new List<DataTable>();
                int row = 0;
                foreach (EncuestaPregunta pregunta in lstTickets.Select(s => s.ep).Distinct())
                {
                    DataTable dt = new DataTable(pregunta.Pregunta);
                    switch (tipoEncuesta)
                    {
                        #region Logico
                        case (int)BusinessVariables.EnumTipoEncuesta.SiNo:
                            row = 0;
                            dt.Columns.Add(new DataColumn("Id"));
                            dt.Columns.Add(new DataColumn("Descripcion"));
                            dt.Columns.Add(new DataColumn("Total"));
                            List<string> lstFechasLogica = lstTickets.OrderBy(o => o.t.FechaHoraAlta).Distinct().Select(s => s.t.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                            switch (tipoFecha)
                            {
                                case 1:
                                    foreach (string fecha in lstFechasLogica)
                                    {
                                        dt.Columns.Add(fecha);
                                    }
                                    break;
                                case 2:
                                    foreach (string fecha in lstFechasLogica)
                                    {
                                        if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                            dt.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                        conteo++;
                                    }
                                    break;
                                case 3:
                                    foreach (string fecha in lstFechasLogica)
                                    {
                                        if (!dt.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                            dt.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                                    }
                                    break;
                                case 4:
                                    foreach (string fecha in lstFechasLogica)
                                    {
                                        if (!dt.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                            dt.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                                    }
                                    break;
                            }
                            dt.Rows.Add(pregunta.Id, "SI");

                            var totalEncuestas = lstTickets.Select(s => new { s.re.IdTicket, s.re.IdEncuesta }).Where(w => w.IdEncuesta == idEncuesta).Distinct().Count();
                            var ponderacionHundredPorcent = pregunta.Ponderacion * decimal.Parse(totalEncuestas.ToString());
                            var enumeracionLogica = lstTickets.Where(w => w.re.IdPregunta == pregunta.Id && w.re.IdEncuesta == pregunta.IdEncuesta)
                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.ValorRespuesta, s.re.Ponderacion }).Distinct().ToList().Where(w => w.ValorRespuesta == 1).GroupBy(ge => new { ge.IdTicket, ge.IdPregunta, ge.IdEncuesta, ge.Ponderacion })
                            .Distinct().Select(ssum =>
                                    new
                                    {
                                        ssum.Key.IdTicket,
                                        ssum.Key.IdPregunta,
                                        ssum.Key.IdEncuesta,
                                        SumaEncuesta = ssum.Sum(sumas => sumas.Ponderacion)
                                    })
                            .Distinct().ToList();
                            var totalLogica = enumeracionLogica.Count * pregunta.Ponderacion;
                            dt.Rows[row][2] = (totalLogica * 100) / ponderacionHundredPorcent;

                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                switch (tipoFecha)
                                {
                                    case 1:
                                        var preguntaEncontradaDiario = lstTickets
                                           .Select(s => new { s.t, s.re })
                                           .Where(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == dt.Columns[i].ColumnName && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 1)
                                           .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                           .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta)
                                           .Distinct().Count();
                                        dt.Rows[row][i] = (preguntaEncontradaDiario * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                    case 2:
                                        var preguntaEncontradaSemanal = lstTickets.Select(s => new { s.t, s.re }).Where(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1]))
                                            && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 1)
                                            .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion }).Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                        dt.Rows[row][i] = (preguntaEncontradaSemanal * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                    case 3:
                                        var preguntaEncontradaMensual = lstTickets.Select(s => new { s.t, s.re }).Where(c => c.t.FechaHoraAlta.ToString("MM") == dt.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 1)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion }).Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                        dt.Rows[row][i] = (preguntaEncontradaMensual * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                    case 4:
                                        var preguntaEncontradaAnual = lstTickets.Select(s => new { s.t, s.re }).Where(c => c.t.FechaHoraAlta.ToString("yyyy") == dt.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 1)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion }).Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                        dt.Rows[row][i] = (preguntaEncontradaAnual * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                }
                            }
                            dt.Rows.Add(pregunta.Id, "NO");
                            enumeracionLogica = lstTickets.Where(w => w.re.IdPregunta == pregunta.Id && w.re.IdEncuesta == pregunta.IdEncuesta)
                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.ValorRespuesta, s.re.Ponderacion }).Distinct().ToList().Where(w => w.ValorRespuesta == 0).GroupBy(ge => new { ge.IdTicket, ge.IdPregunta, ge.IdEncuesta, ge.Ponderacion })
                            .Distinct().Select(ssum =>
                                    new
                                    {
                                        ssum.Key.IdTicket,
                                        ssum.Key.IdPregunta,
                                        ssum.Key.IdEncuesta,
                                        SumaEncuesta = ssum.Sum(sumas => sumas.Ponderacion)
                                    })
                            .Distinct().ToList();
                            totalLogica = enumeracionLogica.Count * pregunta.Ponderacion;
                            dt.Rows[row + 1][2] = (totalLogica * 100) / ponderacionHundredPorcent;

                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                switch (tipoFecha)
                                {
                                    case 1:
                                        var preguntaEncontradaDiario = lstTickets
                                           .Select(s => new { s.t, s.re })
                                           .Where(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == dt.Columns[i].ColumnName && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 0)
                                           .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                           .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta)
                                           .Distinct().Count();
                                        dt.Rows[row + 1][i] = (preguntaEncontradaDiario * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                    case 2:
                                        var preguntaEncontradaSemanal = lstTickets.Select(s => new { s.t, s.re }).Where(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1]))
                                            && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1]))
                                            && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 0).Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion }).Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                        dt.Rows[row + 1][i] = (preguntaEncontradaSemanal * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                    case 3:
                                        var preguntaEncontradaMensual = lstTickets.Select(s => new { s.t, s.re }).Where(c => c.t.FechaHoraAlta.ToString("MM") == dt.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 0)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion }).Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                        dt.Rows[row + 1][i] = (preguntaEncontradaMensual * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                    case 4:
                                        var preguntaEncontradaAnual = lstTickets.Select(s => new { s.t, s.re }).Where(c => c.t.FechaHoraAlta.ToString("yyyy") == dt.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == 0)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion }).Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                        dt.Rows[row + 1][i] = (preguntaEncontradaAnual * pregunta.Ponderacion * 100) / ponderacionHundredPorcent;
                                        break;
                                }
                            }

                            result.Add(dt);
                            break;
                        #endregion Logico
                        #region Calificacion
                        case (int)BusinessVariables.EnumTipoEncuesta.Calificacion:
                            row = 0;
                            dt.Columns.Add(new DataColumn("Id"));
                            dt.Columns.Add(new DataColumn("Descripcion"));
                            dt.Columns.Add(new DataColumn("Total"));
                            List<string> lstFechasCalificacion = lstTickets.OrderBy(o => o.t.FechaHoraAlta).Distinct().Select(s => s.t.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                            switch (tipoFecha)
                            {
                                case 1:
                                    foreach (string fecha in lstFechasCalificacion)
                                    {
                                        dt.Columns.Add(fecha);
                                    }
                                    break;
                                case 2:
                                    foreach (string fecha in lstFechasCalificacion)
                                    {
                                        if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                            dt.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                        conteo++;
                                    }
                                    break;
                                case 3:
                                    foreach (string fecha in lstFechasCalificacion)
                                    {
                                        if (!dt.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                            dt.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                                    }
                                    break;
                                case 4:
                                    foreach (string fecha in lstFechasCalificacion)
                                    {
                                        if (!dt.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                            dt.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                                    }
                                    break;
                            }
                            var totalEncuestasCalificacion = lstTickets.Select(s => new { s.re.IdTicket, s.re.IdEncuesta }).Where(w => w.IdEncuesta == idEncuesta).Distinct().Count();
                            var ponderacionHundredPorcentCalificacion = pregunta.Ponderacion * decimal.Parse(totalEncuestasCalificacion.ToString());

                            for (int valRespuesta = 0; valRespuesta < 10; valRespuesta++)
                            {
                                dt.Rows.Add(pregunta.Id, valRespuesta);


                                var enumeracionCalificacion = lstTickets
                                    .Where(w => w.re.IdPregunta == pregunta.Id && w.re.IdEncuesta == pregunta.IdEncuesta && w.re.ValorRespuesta == valRespuesta)
                                        .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.ValorRespuesta, s.re.Ponderacion }).Distinct().ToList()
                                        .Where(w => w.ValorRespuesta == valRespuesta)
                                        .GroupBy(ge => new { ge.IdTicket, ge.IdPregunta, ge.IdEncuesta, ge.Ponderacion })
                                        .Distinct().Select(ssum => new { ssum.Key.IdTicket, ssum.Key.IdPregunta, ssum.Key.IdEncuesta, SumaEncuesta = ssum.Sum(sumas => sumas.Ponderacion) }).Distinct().ToList();

                                var totalCalificacion = enumeracionCalificacion.Count() * pregunta.Ponderacion;
                                dt.Rows[row + valRespuesta][2] = (totalCalificacion * 100) / ponderacionHundredPorcentCalificacion;
                                for (int i = 3; i < dt.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            var preguntaEncontradaDiario = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == dt.Columns[i].ColumnName && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();

                                            dt.Rows[row + valRespuesta][i] = (preguntaEncontradaDiario * pregunta.Ponderacion * 100) / ponderacionHundredPorcentCalificacion;

                                            break;
                                        case 2:
                                            var preguntaEncontradaSemanal = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                            dt.Rows[row + valRespuesta][i] = (preguntaEncontradaSemanal * pregunta.Ponderacion * 100) / ponderacionHundredPorcentCalificacion;
                                            break;
                                        case 3:
                                            var preguntaEncontradaMensual = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => c.t.FechaHoraAlta.ToString("MM") == dt.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                            dt.Rows[row + valRespuesta][i] = (preguntaEncontradaMensual * pregunta.Ponderacion * 100) / ponderacionHundredPorcentCalificacion;
                                            break;
                                        case 4:
                                            var preguntaEncontradaAnual = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => c.t.FechaHoraAlta.ToString("yyyy") == dt.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                            dt.Rows[row + valRespuesta][i] = (preguntaEncontradaAnual * pregunta.Ponderacion * 100) / ponderacionHundredPorcentCalificacion;
                                            break;
                                    }
                                }
                            }
                            result.Add(dt);
                            break;
                        #endregion Calificacion
                        #region Opcional
                        case (int)BusinessVariables.EnumTipoEncuesta.CalificacionPesimoMaloRegularBuenoExcelente:
                            row = 0;
                            dt.Columns.Add(new DataColumn("Id"));
                            dt.Columns.Add(new DataColumn("Descripcion"));
                            dt.Columns.Add(new DataColumn("Total"));
                            List<string> lstFechasOpcional = lstTickets.OrderBy(o => o.t.FechaHoraAlta).Distinct().Select(s => s.t.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                            switch (tipoFecha)
                            {
                                case 1:
                                    foreach (string fecha in lstFechasOpcional)
                                    {
                                        dt.Columns.Add(fecha);
                                    }
                                    break;
                                case 2:
                                    foreach (string fecha in lstFechasOpcional)
                                    {
                                        if (!dt.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                            dt.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                        conteo++;
                                    }
                                    break;
                                case 3:
                                    foreach (string fecha in lstFechasOpcional)
                                    {
                                        if (!dt.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                            dt.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                                    }
                                    break;
                                case 4:
                                    foreach (string fecha in lstFechasOpcional)
                                    {
                                        if (!dt.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                            dt.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                                    }
                                    break;
                            }
                            var totalEncuestasOpcional = lstTickets.Select(s => new { s.re.IdTicket, s.re.IdEncuesta }).Where(w => w.IdEncuesta == idEncuesta).Distinct().Count();
                            var ponderacionHundredPorcentOpcional = pregunta.Ponderacion * decimal.Parse(totalEncuestasOpcional.ToString());

                            for (int valRespuesta = 1; valRespuesta < 6; valRespuesta++)
                            {
                                switch (valRespuesta)
                                {
                                    case 1:
                                        dt.Rows.Add(pregunta.Id, "PESIMO");
                                        break;
                                    case 2:
                                        dt.Rows.Add(pregunta.Id, "MALO");
                                        break;
                                    case 3:
                                        dt.Rows.Add(pregunta.Id, "REGULAR");
                                        break;
                                    case 4:
                                        dt.Rows.Add(pregunta.Id, "BUENO");
                                        break;
                                    case 5:
                                        dt.Rows.Add(pregunta.Id, "EXCELENTE");
                                        break;
                                }
                                var enumeracionCalificacion = lstTickets
                                    .Where(w => w.re.IdPregunta == pregunta.Id && w.re.IdEncuesta == pregunta.IdEncuesta && w.re.ValorRespuesta == valRespuesta)
                                        .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.ValorRespuesta, s.re.Ponderacion }).Distinct().ToList()
                                        .Where(w => w.ValorRespuesta == valRespuesta)
                                        .GroupBy(ge => new { ge.IdTicket, ge.IdPregunta, ge.IdEncuesta, ge.Ponderacion })
                                        .Distinct().Select(ssum => new { ssum.Key.IdTicket, ssum.Key.IdPregunta, ssum.Key.IdEncuesta, SumaEncuesta = ssum.Sum(sumas => sumas.Ponderacion) }).Distinct().ToList();

                                var totalOpcional = enumeracionCalificacion.Count() * pregunta.Ponderacion;
                                dt.Rows[row + valRespuesta - 1][2] = (totalOpcional * 100) / ponderacionHundredPorcentOpcional;
                                for (int i = 3; i < dt.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            var preguntaEncontradaDiario = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == dt.Columns[i].ColumnName && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();

                                            dt.Rows[row + valRespuesta - 1][i] = (preguntaEncontradaDiario * pregunta.Ponderacion * 100) / ponderacionHundredPorcentOpcional;

                                            break;
                                        case 2:
                                            var preguntaEncontradaSemanal = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(dt.Columns[i].ColumnName.Split(' ')[3]), int.Parse(dt.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                            dt.Rows[row + valRespuesta - 1][i] = (preguntaEncontradaSemanal * pregunta.Ponderacion * 100) / ponderacionHundredPorcentOpcional;
                                            break;
                                        case 3:
                                            var preguntaEncontradaMensual = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => c.t.FechaHoraAlta.ToString("MM") == dt.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                            dt.Rows[row + valRespuesta - 1][i] = (preguntaEncontradaMensual * pregunta.Ponderacion * 100) / ponderacionHundredPorcentOpcional;
                                            break;
                                        case 4:
                                            var preguntaEncontradaAnual = lstTickets
                                                .Select(s => new { s.t, s.re })
                                                .Where(c => c.t.FechaHoraAlta.ToString("yyyy") == dt.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdEncuesta == pregunta.IdEncuesta && c.re.ValorRespuesta == valRespuesta)
                                                .Select(s => new { s.re.IdTicket, s.re.IdEncuesta, s.re.IdPregunta, s.re.Ponderacion })
                                                .Where(w => w.IdPregunta == pregunta.Id && w.IdEncuesta == pregunta.IdEncuesta).Distinct().Count();
                                            dt.Rows[row + valRespuesta - 1][i] = (preguntaEncontradaAnual * pregunta.Ponderacion * 100) / ponderacionHundredPorcentOpcional;
                                            break;
                                    }
                                }
                            }
                            result.Add(dt);
                            break;
                        #endregion Logica
                    }
                    row++;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public string GraficarConsultaEncuestaPreguntaGeografica(int idUsuario, List<int?> encuestas, Dictionary<string, DateTime> fechas, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            string result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join ep in db.EncuestaPregunta on new { idenctick = (int)t.IdEncuesta, ids = e.Id } equals new { idenctick = ep.IdEncuesta, ids = ep.IdEncuesta }
                          join re in db.RespuestaEncuesta on new { idTickTick = (int?)t.Id, idEncTick = (int)t.IdEncuesta, idEncPadre = e.Id, idPreg = ep.Id } equals new { idTickTick = re.IdTicket, idEncTick = re.IdEncuesta, idEncPadre = re.IdEncuesta, idPreg = re.IdPregunta }
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          join d in db.Domicilio on ub.IdCampus equals d.IdCampus
                          join col in db.Colonia on d.IdColonia equals col.Id
                          join m in db.Municipio on col.IdMunicipio equals m.Id
                          join es in db.Estado on m.IdEstado equals es.Id
                          where t.EncuestaRespondida
                          select new { t, tgu, ug, es };

                if (encuestas.Any())
                    qry = from q in qry
                          where encuestas.Contains(q.t.IdEncuesta)
                          select q;

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                var lstTickets = (from q in qry.Distinct()
                                  group new { q.es.RegionCode } by new { q.es.RegionCode }
                                      into g
                                      select new { g.Key.RegionCode, Hits = g.Count() }).ToList();

                if (lstTickets.Any())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Region");
                    dt.Columns.Add("Hits");
                    foreach (var data in lstTickets)
                    {
                        dt.Rows.Add(data.RegionCode, data.Hits);
                    }
                    result = dt.Columns.Cast<DataColumn>().Aggregate("[\n[", (current, column) => current + string.Format("'{0}', ", column.ColumnName));
                    result = result.Trim().TrimEnd(',') + "], \n";
                    result = dt.Rows.Cast<DataRow>().Aggregate(result, (current, row) => current + string.Format("['{0}', {1}]\n", row[0], row[1]));
                    result += "]";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public DataTable GraficarConsultaEficienciaTicket(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, string stack, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            DataTable result = null;
            DateTime fechaInicio = new DateTime();
            int conteo = 1;
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario,
                    (sgu, ug) => new { sgu, ug })
                    .Any(
                        @t =>
                            @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor &&
                            @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          select new { t, tgu };

                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = grupos.Aggregate(qry,
                //    (current, grupo) =>
                //        (from q in current
                //         where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo)
                //         select q));

                if (responsables.Any())
                    qry = from q in qry
                          where responsables.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = responsables.Aggregate(qry,
                //    (current, grupo) =>
                //        (from q in current
                //         where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo)
                //         select q));

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.t.IdArbolAcceso)
                          select q;

                //TODO: Filtrar nivel de atencion

                if (atendedores.Any())
                    qry = from q in qry
                          where atendedores.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = atendedores.Aggregate(qry,
                //    (current, grupo) =>
                //        (from q in current
                //         where q.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo)
                //         select q));

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                var lstTickets = qry.Distinct().ToList();

                if (lstTickets.Any())
                {
                    result = new DataTable("dt");
                    result.Columns.Add(new DataColumn("Id"));
                    result.Columns.Add(new DataColumn("Descripcion"));
                    result.Columns.Add(new DataColumn("Total"));

                    List<string> lstFechas = lstTickets.OrderBy(o => o.t.FechaHoraAlta).Distinct().Select(s => s.t.FechaHoraAlta.ToString("dd/MM/yyyy")).Distinct().ToList();
                    switch (tipoFecha)
                    {
                        case 1:
                            foreach (string fecha in lstFechas)
                            {
                                result.Columns.Add(fecha);
                            }
                            break;
                        case 2:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add("SEMANA " + CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Parse(fecha), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday) + " AÑO " + DateTime.Parse(fecha).Year.ToString());
                                conteo++;
                            }
                            break;
                        case 3:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Month.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Month.ToString());
                            }
                            break;
                        case 4:
                            foreach (string fecha in lstFechas)
                            {
                                if (!result.Columns.Contains(DateTime.Parse(fecha).Year.ToString()))
                                    result.Columns.Add(DateTime.Parse(fecha).Year.ToString());
                            }
                            break;
                    }
                    int row = 0;
                    switch (stack)
                    {
                        case "Ubicaciones":

                            foreach (int idUbicacion in lstTickets.Select(s => s.t.IdUbicacion).Distinct())
                            {
                                result.Rows.Add(idUbicacion, new BusinessUbicacion().ObtenerDescripcionUbicacionById(idUbicacion, false));
                                var total = 0;
                                switch (tipoFecha)
                                {
                                    case 1:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[3].ColumnName) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName) && c.t.IdUbicacion == idUbicacion);
                                        break;
                                    case 2:
                                        total = lstTickets.Count(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[3].ColumnName.Split(' ')[3]), int.Parse(result.Columns[3].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[3]), int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[1])) && c.t.IdUbicacion == idUbicacion);
                                        break;
                                    case 3:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) >= DateTime.Parse(result.Columns[3].ColumnName.PadLeft(2, '0')) && DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(2, '0')) && c.t.IdUbicacion == idUbicacion);
                                        break;
                                    case 4:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) >= new DateTime(int.Parse(result.Columns[3].ColumnName.PadLeft(4, '0'))) && DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) <= new DateTime(int.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(4, '0'))) && c.t.IdUbicacion == idUbicacion);

                                        break;
                                }
                                result.Rows[row][2] = total;
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.t.IdUbicacion == idUbicacion);
                                            break;
                                        case 2:
                                            result.Rows[row][i] = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdUbicacion == idUbicacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdUbicacion == idUbicacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdUbicacion == idUbicacion);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Organizaciones":
                            foreach (int idOrganizacion in lstTickets.Select(s => s.t.IdOrganizacion).Distinct())
                            {
                                result.Rows.Add(idOrganizacion, new BusinessOrganizacion().ObtenerDescripcionOrganizacionById(idOrganizacion, false));
                                var total = 0;
                                switch (tipoFecha)
                                {
                                    case 1:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[3].ColumnName) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName) && c.t.IdOrganizacion == idOrganizacion);
                                        break;
                                    case 2:
                                        total = lstTickets.Count(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[3].ColumnName.Split(' ')[3]), int.Parse(result.Columns[3].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[3]), int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[1])) && c.t.IdOrganizacion == idOrganizacion);
                                        break;
                                    case 3:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) >= DateTime.Parse(result.Columns[3].ColumnName.PadLeft(2, '0')) && DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(2, '0')) && c.t.IdOrganizacion == idOrganizacion);
                                        break;
                                    case 4:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) >= new DateTime(int.Parse(result.Columns[3].ColumnName.PadLeft(4, '0'))) && DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) <= new DateTime(int.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(4, '0'))) && c.t.IdOrganizacion == idOrganizacion);

                                        break;
                                }
                                result.Rows[row][2] = total;
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.t.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 2:
                                            result.Rows[row][i] = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdOrganizacion == idOrganizacion);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdOrganizacion == idOrganizacion);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Tipo Ticket":
                            foreach (int idTipoArbolAcceso in lstTickets.Select(s => s.t.IdTipoArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idTipoArbolAcceso, new BusinessTipoArbolAcceso().ObtenerTiposArbolAcceso(false).Single(s => s.Id == idTipoArbolAcceso).Descripcion);
                                var total = 0;
                                switch (tipoFecha)
                                {
                                    case 1:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[3].ColumnName) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName) && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                        break;
                                    case 2:
                                        total = lstTickets.Count(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[3].ColumnName.Split(' ')[3]), int.Parse(result.Columns[3].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[3]), int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[1])) && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                        break;
                                    case 3:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) >= DateTime.Parse(result.Columns[3].ColumnName.PadLeft(2, '0')) && DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(2, '0')) && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                        break;
                                    case 4:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) >= new DateTime(int.Parse(result.Columns[3].ColumnName.PadLeft(4, '0'))) && DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) <= new DateTime(int.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(4, '0'))) && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);

                                        break;
                                }
                                result.Rows[row][2] = total;
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdTipoArbolAcceso == idTipoArbolAcceso);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Tipificaciones":
                            foreach (int idArbol in lstTickets.Select(s => s.t.IdArbolAcceso).Distinct())
                            {
                                result.Rows.Add(idArbol, new BusinessArbolAcceso().ObtenerTipificacion(idArbol));
                                var total = 0;
                                switch (tipoFecha)
                                {
                                    case 1:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[3].ColumnName) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName) && c.t.IdArbolAcceso == idArbol);
                                        break;
                                    case 2:
                                        total = lstTickets.Count(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[3].ColumnName.Split(' ')[3]), int.Parse(result.Columns[3].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[3]), int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[1])) && c.t.IdArbolAcceso == idArbol);
                                        break;
                                    case 3:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) >= DateTime.Parse(result.Columns[3].ColumnName.PadLeft(2, '0')) && DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(2, '0')) && c.t.IdArbolAcceso == idArbol);
                                        break;
                                    case 4:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) >= new DateTime(int.Parse(result.Columns[3].ColumnName.PadLeft(4, '0'))) && DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) <= new DateTime(int.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(4, '0'))) && c.t.IdArbolAcceso == idArbol);

                                        break;
                                }
                                result.Rows[row][2] = total;
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.t.IdArbolAcceso == idArbol);
                                            break;
                                        case 2:
                                            result.Rows[row][i] = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdArbolAcceso == idArbol);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdArbolAcceso == idArbol);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdArbolAcceso == idArbol);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "Estatus Ticket":
                            foreach (int idEstatusticket in lstTickets.Select(s => s.t.IdEstatusTicket).Distinct())
                            {
                                result.Rows.Add(idEstatusticket, new BusinessEstatus().ObtenerEstatusTicket(false).Single(s => s.Id == idEstatusticket).Descripcion);
                                var total = 0;
                                switch (tipoFecha)
                                {
                                    case 1:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[3].ColumnName) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName) && c.t.IdEstatusTicket == idEstatusticket);
                                        break;
                                    case 2:
                                        total = lstTickets.Count(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[3].ColumnName.Split(' ')[3]), int.Parse(result.Columns[3].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[3]), int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[1])) && c.t.IdEstatusTicket == idEstatusticket);
                                        break;
                                    case 3:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) >= DateTime.Parse(result.Columns[3].ColumnName.PadLeft(2, '0')) && DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(2, '0')) && c.t.IdEstatusTicket == idEstatusticket);
                                        break;
                                    case 4:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) >= new DateTime(int.Parse(result.Columns[3].ColumnName.PadLeft(4, '0'))) && DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) <= new DateTime(int.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(4, '0'))) && c.t.IdEstatusTicket == idEstatusticket);

                                        break;
                                }
                                result.Rows[row][2] = total;
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.t.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && c.t.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.t.IdEstatusTicket == idEstatusticket);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.t.IdEstatusTicket == idEstatusticket);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
                        case "SLA":
                            foreach (bool dentroSla in lstTickets.Select(s => s.t.DentroSla).Distinct())
                            {
                                result.Rows.Add(dentroSla ? 1 : 0, dentroSla ? "Dentro" : "Fuera");
                                var total = 0;
                                switch (tipoFecha)
                                {
                                    case 1:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= DateTime.Parse(result.Columns[3].ColumnName) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName) && c.t.DentroSla == dentroSla);
                                        break;
                                    case 2:
                                        total = lstTickets.Count(c =>
                                            DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[3].ColumnName.Split(' ')[3]), int.Parse(result.Columns[3].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[3]), int.Parse(result.Columns[result.Columns.Count].ColumnName.Split(' ')[1])) && c.t.DentroSla == dentroSla);
                                        break;
                                    case 3:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) >= DateTime.Parse(result.Columns[3].ColumnName.PadLeft(2, '0')) && DateTime.Parse(c.t.FechaHoraAlta.ToString("MM")) <= DateTime.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(2, '0')) && c.t.DentroSla == dentroSla);
                                        break;
                                    case 4:
                                        total = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) >= new DateTime(int.Parse(result.Columns[3].ColumnName.PadLeft(4, '0'))) && DateTime.Parse(c.t.FechaHoraAlta.ToString("yyyy")) <= new DateTime(int.Parse(result.Columns[result.Columns.Count - 1].ColumnName.PadLeft(4, '0'))) && c.t.DentroSla == dentroSla);

                                        break;
                                }
                                result.Rows[row][2] = total;
                                for (int i = 3; i < result.Columns.Count; i++)
                                {
                                    switch (tipoFecha)
                                    {
                                        case 1:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("dd/MM/yyyy") == result.Columns[i].ColumnName && c.t.DentroSla == dentroSla);
                                            break;
                                        case 2:

                                            result.Rows[row][i] = lstTickets.Count(c => DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) >= BusinessCadenas.Fechas.ObtenerFechaInicioSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && DateTime.Parse(c.t.FechaHoraAlta.ToString("dd/MM/yyyy")) <= BusinessCadenas.Fechas.ObtenerFechaFinSemana(int.Parse(result.Columns[i].ColumnName.Split(' ')[3]), int.Parse(result.Columns[i].ColumnName.Split(' ')[1])) && c.t.DentroSla == dentroSla);
                                            break;
                                        case 3:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("MM") == result.Columns[i].ColumnName.PadLeft(2, '0') && c.t.DentroSla == dentroSla);
                                            break;
                                        case 4:
                                            result.Rows[row][i] = lstTickets.Count(c => c.t.FechaHoraAlta.ToString("yyyy") == result.Columns[i].ColumnName.PadLeft(4, '0') && c.t.DentroSla == dentroSla);
                                            break;
                                    }
                                }
                                row++;
                            }
                            break;
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

        public string GraficarConsultaEEficienciaTicketsGeografica(int idUsuario, List<int> grupos, List<int> responsables, List<int> tipoArbol, List<int> tipificacion, List<int> nivelAtencion, List<int> atendedores, Dictionary<string, DateTime> fechas, List<int> tiposUsuario, List<int> prioridad, List<int> ubicaciones, List<int> organizaciones, List<bool?> vip, int tipoFecha)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            string result = null;
            DateTime fechaInicio = new DateTime();
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                DateTime fechaFin = new DateTime();
                if (fechas != null)
                {
                    fechaInicio = fechas.Single(s => s.Key == "inicio").Value;
                    fechaFin = fechas.Single(s => s.Key == "fin").Value.AddDays(1);
                }

                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join or in db.Organizacion on t.IdOrganizacion equals or.Id
                          join ub in db.Ubicacion on t.IdUbicacion equals ub.Id
                          join ug in db.UsuarioGrupo on new { tgu.IdGrupoUsuario, tgu.IdSubGrupoUsuario } equals new { ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                          join d in db.Domicilio on ub.IdCampus equals d.IdCampus
                          join col in db.Colonia on d.IdColonia equals col.Id
                          join m in db.Municipio on col.IdMunicipio equals m.Id
                          join es in db.Estado on m.IdEstado equals es.Id
                          select new { t, es, ug, tgu };


                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = grupos.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (responsables.Any())
                    qry = from q in qry
                          where responsables.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = responsables.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (tipoArbol.Any())
                    qry = from q in qry
                          where tipoArbol.Contains(q.t.IdTipoArbolAcceso)
                          select q;

                if (tipificacion.Any())
                    qry = from q in qry
                          where tipificacion.Contains(q.t.IdArbolAcceso)
                          select q;

                //TODO: Filtrar nivel de atencion

                if (atendedores.Any())
                    qry = from q in qry
                          where atendedores.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                //qry = atendedores.Aggregate(qry, (current, grupo) => (from q in current where q.t.TicketGrupoUsuario.Select(s => s.IdGrupoUsuario).Contains(grupo) select q));

                if (fechas != null)
                    if (fechas.Count == 2)
                        qry = from q in qry
                              where q.t.FechaHoraAlta >= fechaInicio
                                    && q.t.FechaHoraAlta <= fechaFin
                              select q;

                if (tiposUsuario.Any())
                    qry = from q in qry
                          where tiposUsuario.Contains(q.t.IdTipoUsuario)
                          select q;

                if (prioridad.Any())
                    qry = from q in qry
                          where prioridad.Contains(q.t.IdImpacto)
                          select q;

                if (ubicaciones.Any())
                    qry = from q in qry
                          where ubicaciones.Contains(q.t.IdUbicacion)
                          select q;

                if (organizaciones.Any())
                    qry = from q in qry
                          where organizaciones.Contains(q.t.IdOrganizacion)
                          select q;

                if (vip.Any())
                    qry = from q in qry
                          where vip.Contains(q.t.UsuarioLevanto.Vip)
                          select q;

                if (!supervisor)
                    qry = from q in qry
                          where q.ug.IdUsuario == idUsuario
                          select q;

                var lstTickets = (from q in qry.Distinct()
                                  group new { q.es.RegionCode } by new { q.es.RegionCode }
                                      into g
                                      select new { g.Key.RegionCode, Hits = g.Count() }).ToList();

                if (lstTickets.Any())
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Region");
                    dt.Columns.Add("Hits");
                    foreach (var data in lstTickets)
                    {
                        dt.Rows.Add(data.RegionCode, data.Hits);
                    }
                    result = dt.Columns.Cast<DataColumn>().Aggregate("[\n[", (current, column) => current + string.Format("'{0}', ", column.ColumnName));
                    result = result.Trim().TrimEnd(',') + "], \n";
                    result = dt.Rows.Cast<DataRow>().Aggregate(result, (current, row) => current + string.Format("['{0}', {1}]\n", row[0], row[1]));
                    result += "]";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        #endregion Graficas
    }
}
