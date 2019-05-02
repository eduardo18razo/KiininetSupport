using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Demonio;
using KinniNet.Core.Sistema;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessTicket : IDisposable
    {
        private readonly bool _proxy;

        public void Dispose()
        {

        }

        public BusinessTicket(bool proxy = false)
        {
            _proxy = proxy;
        }

        private DateTime TiempoGeneral(List<HorarioSubGrupo> horarioSubGrupo, List<DiaFestivoSubGrupo> diasFeriados, DateTime fechaLevanta, decimal? tiempoProceso)
        {
            DateTime result;
            try
            {
                List<DateTime> diasAsignados = new List<DateTime>();
                string horarioInicio = horarioSubGrupo.Min(s => s.HoraInicio);
                string horarioFin = horarioSubGrupo.Max(s => s.HoraFin);
                double tiempotrabajo = Double.Parse(horarioFin.Replace(':', '.').Substring(0, 5)) - Double.Parse(horarioInicio.Replace(':', '.').Substring(0, 5));

                if (tiempoProceso != null)
                {
                    double horasTotalSolucion = Double.Parse(tiempoProceso.ToString());
                    int contador = 0;
                    while (horasTotalSolucion > 0)
                    {
                        if (horarioSubGrupo.Any(a => a.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek))
                        {
                            horarioInicio = horarioSubGrupo.Where(w => w.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek).Min(m => m.HoraInicio);
                            horarioFin = horarioSubGrupo.Where(w => w.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek).Max(m => m.HoraInicio);
                            if (horarioFin == "23:00:00")
                                horarioFin = "23:59:59";
                            else
                                horarioFin = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + horarioFin).AddHours(1).ToString("HH:mm:ss");
                            if (!diasFeriados.Any(a => a.Fecha.ToShortDateString() == DateTime.Now.AddDays(contador).ToShortDateString()))
                                if (DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioFin) > fechaLevanta)
                                {
                                    if (contador == 0)
                                        tiempotrabajo = Double.Parse(Math.Round((DateTime.Parse(DateTime.Now.ToShortDateString() + " " + horarioFin) - fechaLevanta).TotalHours, 2, MidpointRounding.ToEven).ToString());
                                    else
                                        tiempotrabajo = Double.Parse(Math.Round((DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioInicio) - DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioFin)).TotalHours, 2, MidpointRounding.ToEven).ToString());
                                    tiempotrabajo = Math.Abs(tiempotrabajo);
                                    if (tiempotrabajo >= horasTotalSolucion)
                                    {
                                        if (diasAsignados.Count <= 0)
                                        {
                                            if (contador == 0)
                                                diasAsignados.Add(fechaLevanta.AddHours(horasTotalSolucion));
                                            else
                                                diasAsignados.Add(DateTime.Parse(fechaLevanta.AddDays(contador).ToShortDateString() + " " + horarioInicio).AddHours(horasTotalSolucion));
                                        }
                                        else
                                            diasAsignados.Add(DateTime.Parse(fechaLevanta.AddDays(contador).ToShortDateString() + " " + horarioInicio).AddHours(horasTotalSolucion));
                                        horasTotalSolucion -= horasTotalSolucion;
                                    }
                                    else
                                    {
                                        if (tiempotrabajo > 0)
                                        {
                                            if (diasAsignados.Count <= 0)
                                                if (contador == 0)
                                                    diasAsignados.Add(fechaLevanta.AddHours(tiempotrabajo));
                                                else
                                                    diasAsignados.Add(DateTime.Parse(fechaLevanta.AddDays(contador).ToShortDateString() + " " + horarioInicio).AddHours(tiempotrabajo));
                                            else
                                                diasAsignados.Add(DateTime.Parse(fechaLevanta.AddDays(contador).ToShortDateString() + " " + horarioInicio).AddHours(tiempotrabajo));
                                        }
                                        else
                                        {
                                            if (diasAsignados.Count <= 0)
                                            {
                                                diasAsignados.Add(fechaLevanta.AddDays(contador + 1).AddHours(horasTotalSolucion));
                                            }
                                            else
                                                diasAsignados.Add(diasAsignados.Last().AddHours(horasTotalSolucion));
                                        }
                                        horasTotalSolucion -= tiempotrabajo;
                                    }
                                }

                        }
                        contador++;
                    }
                }
                if (tiempoProceso == 0)
                    diasAsignados.Add(fechaLevanta);
                result = DateTime.ParseExact(diasAsignados.Max().ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result;
        }

        

        private int ObtenerSubRolAsignadoTicket(int? nivelAsignacion)
        {
            int result = 0;
            if (nivelAsignacion != null)
            {
                switch (nivelAsignacion)
                {
                    case 1:
                        result = (int)BusinessVariables.EnumSubRoles.Supervisor;
                        break;
                    case 2:
                        result = (int)BusinessVariables.EnumSubRoles.PrimererNivel;
                        break;
                    case 3:
                        result = (int)BusinessVariables.EnumSubRoles.SegundoNivel;
                        break;
                    case 4:
                        result = (int)BusinessVariables.EnumSubRoles.TercerNivel;
                        break;
                    case 5:

                        result = (int)BusinessVariables.EnumSubRoles.CuartoNivel;
                        break;
                }
            }
            return result;
        }


        private List<HelperTickets> GeneraTicketsBandeja(DataBaseModelContext db, List<Ticket> lstTickets, List<int?> lstEstatusPermitidos, int idUsuario, bool grupoConSupervisor, bool supervisor, int pageIndex, int pageSize)
        {
            List<HelperTickets> result = null;
            try
            {
                int totalRegistros = lstTickets.Count;

                if (totalRegistros > 0)
                {
                    List<EstatusTicket> lstEstatusEstatusTicket = new BusinessEstatus().ObtenerEstatusTicket(false);
                    List<EstatusAsignacion> lstEstatusAsignaciones = new BusinessEstatus().ObtenerEstatusAsignacion(false);
                    result = new List<HelperTickets>();
                    foreach (Ticket ticket in lstTickets.Skip(pageIndex * pageSize).Take(pageSize))
                    {
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket.UsuarioLevanto, "TipoUsuario");
                        db.LoadProperty(ticket, "UsuarioSolicito");
                        db.LoadProperty(ticket.UsuarioSolicito, "TipoUsuario");

                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Canal");

                        db.LoadProperty(ticket, "UsuarioUltimoMovimiento");
                        db.LoadProperty(ticket, "UltimoAgenteAsignado");

                        HelperTickets hticket = new HelperTickets
                        {
                            IdTicket = ticket.Id,
                            IdUsuario = ticket.IdUsuarioLevanto
                        };
                        var gpoAgenteAtencion = (db.TicketGrupoUsuario.Where(w => w.IdTicket == ticket.Id).Join(db.GrupoUsuario.Where(
                            w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente),
                            tgu => tgu.IdGrupoUsuario, gu => gu.Id, (tgu, gu) => gu)).Distinct().First();

                        hticket.FechaHora = ticket.FechaHoraAlta;
                        hticket.NumeroTicket = ticket.Id;
                        hticket.NombreUsuario = ticket.UsuarioLevanto.NombreCompleto;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);

                        hticket.FechaUltimoEvento = ticket.FechaUltimoMovimiento;
                        hticket.EstatusTicket = lstEstatusEstatusTicket.Single(s => s.Id == ticket.IdEstatusTicket);
                        hticket.EstatusAsignacion = lstEstatusAsignaciones.Single(s => s.Id == ticket.IdEstatusAsignacion);
                        hticket.FechaCambioEstatusAsignacion = ticket.FechaUltimoAgenteAsignado;
                        hticket.IdUsuarioAsignado = ticket.IdUsuarioUltimoAgenteAsignado != null ? ticket.UltimoAgenteAsignado.Id : 0;
                        hticket.UsuarioAsignado = ticket.IdUsuarioUltimoAgenteAsignado != null ? ticket.UltimoAgenteAsignado.NombreCompleto : "";
                        hticket.IdSubRolAsignado = ObtenerSubRolAsignadoTicket(ticket.IdNivelTicket);
                        hticket.IdNivelAsignado = ticket.IdNivelTicket == null ? 0 : (int)ticket.IdNivelTicket;
                        hticket.TieneEncuesta = ticket.IdEncuesta.HasValue;
                        hticket.NivelUsuarioAsignado = ticket.IdNivelTicket != null ? ((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)ticket.IdNivelTicket).ToString() : String.Empty;
                        hticket.NivelUsuarioAsignado = ticket.IdNivelTicket != null ? ((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)ticket.IdNivelTicket).ToString() : String.Empty;
                        hticket.EsPropietario = ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar ? false : idUsuario == ticket.IdUsuarioUltimoAgenteAsignado;
                        hticket.CambiaEstatus = hticket.IdUsuarioAsignado == idUsuario;
                        if (gpoAgenteAtencion != null)
                        {
                            hticket.IdGrupoAsignado = gpoAgenteAtencion.Id;
                            hticket.GrupoAsignado = gpoAgenteAtencion.Descripcion;

                            if (!hticket.EsPropietario)
                            {
                                List<EstatusAsignacionSubRolGeneral> estatusPermitidos;
                                if (gpoAgenteAtencion.TieneSupervisor)
                                    estatusPermitidos = db.EstatusAsignacionSubRolGeneral.Where(easrg => easrg.IdGrupoUsuario == gpoAgenteAtencion.Id
                                                     && easrg.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor
                                                     && easrg.IdEstatusAsignacionActual == ticket.IdEstatusAsignacion
                                                     && easrg.Propietario == hticket.EsPropietario
                                                     && easrg.Habilitado).Distinct().ToList();
                                else
                                    estatusPermitidos = db.EstatusAsignacionSubRolGeneral.Where(easrg => easrg.IdGrupoUsuario == gpoAgenteAtencion.Id
                                                     && easrg.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel
                                                     && easrg.IdEstatusAsignacionActual == ticket.IdEstatusAsignacion
                                                     && easrg.Propietario == hticket.EsPropietario
                                                     && easrg.Habilitado).Distinct().ToList();
                                hticket.Asigna = estatusPermitidos.Count > 0;
                            }
                            else
                            {
                                hticket.Asigna = gpoAgenteAtencion.TieneSupervisor
                                        ? (ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true
                                            : idUsuario == ticket.IdUsuarioUltimoAgenteAsignado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado)
                                        : lstEstatusPermitidos.Contains((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar);
                            }
                        }
                        hticket.ImagenPrioridad = ticket.Impacto.Descripcion == "ALTO"
                            ? "~/assets/images/icons/prioridadalta.png"
                            : ticket.Impacto.Descripcion == "MEDIO" ? "~/assets/images/icons/prioridadmedia.png" : "~/assets/images/icons/prioridadbaja.png";
                        hticket.ImagenSla = ticket.FechaHoraFinProceso == null ? "~/assets/images/icons/SLA_verde.png" : ticket.DentroSla ? "~/assets/images/icons/SLA_verde.png" : "~/assets/images/icons/SLA_rojo.png";
                        hticket.Total = totalRegistros;
                        hticket.IdImpacto = ticket.IdImpacto;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.TipoTicketDescripcion = ticket.TipoArbolAcceso.Descripcion;
                        hticket.TipoTicketAbreviacion = ticket.TipoArbolAcceso.Abreviacion;
                        hticket.UsuarioSolicito = ticket.UsuarioSolicito;
                        hticket.Vip = ticket.UsuarioSolicito.Vip;
                        hticket.Canal = ticket.Canal.Descripcion;
                        hticket.DentroSla = ticket.FechaHoraFinProceso == null ? true : ticket.DentroSla;
                        hticket.RecienCerrado = ticket.FechaTermino == null ? false : ((TimeSpan)(DateTime.Now - ticket.FechaTermino)).Hours <= 36 ? true : false;
                        result.Add(hticket);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }


        #region Ticket Alta

        public Ticket CrearTicket(int idUsuario, int idUsuarioSolicito, int idArbol, List<HelperCampoMascaraCaptura> lstCaptura, int idCanal, bool campoRandom, bool esTercero, bool esMail, bool forzarCorreo = false)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            Ticket result;
            try
            {
                string correo = String.Empty;
                Usuario usuarioLevanto = new BusinessUsuarios().ObtenerUsuario(idUsuario);
                Usuario usuarioSolicito = new BusinessUsuarios().ObtenerUsuario(idUsuarioSolicito);
                ArbolAcceso arbol = new BusinessArbolAcceso().ObtenerArbolAcceso(idArbol);
                Mascara mascara = new BusinessMascaras().ObtenerMascaraCaptura(arbol.InventarioArbolAcceso.First().IdMascara ?? 0);
                Encuesta encuesta = new BusinessEncuesta().ObtenerEncuestaById(arbol.InventarioArbolAcceso.First().IdEncuesta ?? 0);
                Sla sla = arbol.InventarioArbolAcceso.First().IdSla != null ? new BusinessSla().ObtenerSla(arbol.InventarioArbolAcceso.First().IdSla ?? 0)
                    : null;
                if (!new BusinessMascaras().ValidaEstructuraMascara(mascara))
                    throw new Exception("Datos Invalidos Consulte a su administrador");
                DateTime fechaMovimiento = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                Ticket ticket = new Ticket
                {
                    IdTipoUsuario = usuarioSolicito.IdTipoUsuario,
                    IdTipoArbolAcceso = arbol.IdTipoArbolAcceso,
                    IdArbolAcceso = arbol.Id,
                    IdImpacto = (int)arbol.IdImpacto,
                    IdUsuarioLevanto = usuarioLevanto.Id,
                    IdUsuarioSolicito = idUsuarioSolicito,
                    IdOrganizacion = usuarioSolicito.IdOrganizacion,
                    IdUbicacion = usuarioSolicito.IdUbicacion,
                    IdMascara = mascara.Id,
                    IdEncuesta = encuesta != null ? encuesta.Id : (int?)null,
                    IdCanal = idCanal,
                    IdEstatusTicket = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto,
                    FechaHoraAlta = fechaMovimiento,
                    IdEstatusAsignacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                    Random = campoRandom,
                    ClaveRegistro = UtilsTicket.GeneraCampoRandom(),
                    EsTercero = usuarioLevanto.Id != idUsuarioSolicito,
                    DentroSla = true,
                    TicketCorreo = new List<TicketCorreo>(),
                    TicketGrupoUsuario = new List<TicketGrupoUsuario>(),
                    IdUsuarioUltimoMovimiento = usuarioLevanto.Id,
                    FechaUltimoMovimiento = fechaMovimiento,
                    FechaUltimoAgenteAsignado = fechaMovimiento
                };
                correo = usuarioSolicito.CorreoUsuario.FirstOrDefault(f => f.Obligatorio) == null ? String.Empty : usuarioSolicito.CorreoUsuario.FirstOrDefault(f => f.Obligatorio).Correo;
                if (correo != String.Empty)
                    ticket.TicketCorreo.Add(new TicketCorreo
                    {
                        Correo = correo
                    });
                foreach (GrupoUsuarioInventarioArbol grupoArbol in arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                {
                    TicketGrupoUsuario grupo = new TicketGrupoUsuario { IdGrupoUsuario = grupoArbol.IdGrupoUsuario };
                    if (grupoArbol.IdSubGrupoUsuario != null)
                        grupo.IdSubGrupoUsuario = grupoArbol.IdSubGrupoUsuario;
                    ticket.TicketGrupoUsuario.Add(grupo);
                }

                //SLA
                if (sla != null)
                {
                    ticket.SlaEstimadoTicket = new SlaEstimadoTicket
                    {
                        FechaInicio = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        Dias = sla.Dias,
                        Horas = sla.Horas,
                        Minutos = sla.Minutos,
                        Segundos = sla.Segundos,
                        FechaInicioProceso = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                        TiempoHoraProceso = sla.TiempoHoraProceso,
                        Terminado = false,
                        SlaEstimadoTicketDetalle = new List<SlaEstimadoTicketDetalle>()
                    };

                    List<HorarioSubGrupo> lstHorarioGrupo = new List<HorarioSubGrupo>();
                    List<DiaFestivoSubGrupo> lstDiasFestivosGrupo = new List<DiaFestivoSubGrupo>();
                    foreach (SubGrupoUsuario sGpoUsuario in ticket.TicketGrupoUsuario.SelectMany(tgrupoUsuario => db.GrupoUsuario.Where(w => w.Id == tgrupoUsuario.IdGrupoUsuario && w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).SelectMany(gpoUsuario => gpoUsuario.SubGrupoUsuario)))
                    {
                        lstHorarioGrupo.AddRange(db.HorarioSubGrupo.Where(w => w.IdSubGrupoUsuario == sGpoUsuario.Id).ToList());
                        lstDiasFestivosGrupo.AddRange(db.DiaFestivoSubGrupo.Where(w => w.IdSubGrupoUsuario == sGpoUsuario.Id));
                    }
                    DateTime fechaTermino = TiempoGeneral(lstHorarioGrupo, lstDiasFestivosGrupo, ticket.FechaHoraAlta, ticket.SlaEstimadoTicket.TiempoHoraProceso);
                    ticket.FechaHoraFinProceso = fechaTermino;
                    ticket.SlaEstimadoTicket.FechaFinProceso = fechaTermino;
                    ticket.SlaEstimadoTicket.FechaFin = fechaTermino;

                    //SLA DETALLE
                    ticket.SlaEstimadoTicket.SlaEstimadoTicketDetalle.AddRange(sla.SlaDetalle.Select(detalle => new SlaEstimadoTicketDetalle { IdSubRol = detalle.IdSubRol, Dias = sla.Dias, Horas = sla.Horas, Minutos = sla.Minutos, Segundos = sla.Segundos, TiempoProceso = detalle.TiempoProceso }));
                }


                ticket.IdEstatusTicket = (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Abierto;
                ticket.TicketEvento = new List<TicketEvento>();
                ticket.TicketEvento.Add(new TicketEvento
                {
                    IdUsuarioRealizo = idUsuarioSolicito,
                    FechaHora = fechaMovimiento,
                    TicketEventoEstatus = new List<TicketEventoEstatus>()
                    {
                        
                        new TicketEventoEstatus
                        {
                            FechaHora = fechaMovimiento,
                            TicketEstatus = new TicketEstatus
                            {
                                IdEstatus = ticket.IdEstatusTicket,
                                IdUsuarioMovimiento = idUsuario,
                                FechaMovimiento = fechaMovimiento,

                            }
                        }
                    },
                    TicketEventoAsignacion = new List<TicketEventoAsignacion>()
                    {
                        new TicketEventoAsignacion
                        {
                            FechaHora =fechaMovimiento,
                            TicketAsignacion = new TicketAsignacion
                            {
                                IdEstatusAsignacion = (int) BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar,
                                FechaAsignacion =fechaMovimiento,
                            }
                        }
                    }

                });


                DateTime fechaTicket = fechaMovimiento;

                List<Frecuencia> frecuencias = db.Frecuencia.Where(s => s.IdTipoUsuario == ticket.IdTipoUsuario && s.IdTipoArbolAcceso == ticket.IdTipoArbolAcceso && s.IdArbolAcceso == ticket.IdArbolAcceso && s.Fecha == fechaTicket).ToList();
                Frecuencia frecuencia = (frecuencias.Count > 0) ? frecuencias.LastOrDefault() : null;
                if (frecuencia == null)
                {
                    frecuencia = new Frecuencia
                    {
                        IdTipoUsuario = ticket.IdTipoUsuario,
                        IdTipoArbolAcceso = ticket.IdTipoArbolAcceso,
                        IdArbolAcceso = ticket.IdArbolAcceso,
                        NumeroVisitas = 1,
                        Fecha = fechaTicket,
                        UltimaVisita = fechaMovimiento
                    };
                    db.Frecuencia.AddObject(frecuencia);
                }
                else
                {
                    frecuencia.NumeroVisitas++;
                    frecuencia.UltimaVisita = fechaMovimiento;
                }
                if (mascara.CampoMascara.Any(a => a.IdTipoCampoMascara == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación))
                {
                    ticket.MascaraSeleccionCatalogo = new List<MascaraSeleccionCatalogo>();
                    foreach (CampoMascara campoCasilla in mascara.CampoMascara.Where(w => w.IdTipoCampoMascara == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación))
                    {
                        Catalogos cat = new BusinessCatalogos().ObtenerCatalogo((int)campoCasilla.IdCatalogo);
                        if (!cat.Archivo)
                        {
                            HelperCampoMascaraCaptura campo = lstCaptura.SingleOrDefault(w => w.NombreCampo == campoCasilla.NombreCampo);
                            if (campo != null)
                            {
                                if (campo.Valor != String.Empty)
                                {
                                    string[] values = campo.Valor.Split('|');
                                    foreach (string value in values)
                                    {
                                        ticket.MascaraSeleccionCatalogo.Add(new MascaraSeleccionCatalogo
                                        {
                                            NombreCampoMascara = campo.NombreCampo,
                                            IdRegistroCatalogo = Int32.Parse(value),
                                            Seleccionado = true
                                        });
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }

                db.Ticket.AddObject(ticket);
                db.SaveChanges();
                new BusinessArbolAcceso().HitArbolAcceso(ticket.IdArbolAcceso);

                string store = String.Format("{0} '{1}',", mascara.ComandoInsertar, ticket.Id);
                bool contieneArchivo = false;
                foreach (HelperCampoMascaraCaptura helperCampoMascaraCaptura in lstCaptura)
                {
                    if (mascara.CampoMascara.Any(s => s.NombreCampo == helperCampoMascaraCaptura.NombreCampo && s.TipoCampoMascara.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo))
                    {

                        store += String.Format("'{0}',", helperCampoMascaraCaptura.Valor.Replace("ticketid", ticket.Id.ToString()));
                        contieneArchivo = true;
                    }
                    else if (mascara.CampoMascara.Any(s => s.NombreCampo == helperCampoMascaraCaptura.NombreCampo && s.TipoCampoMascara.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación))
                    {
                        store += String.Format("'{0}',", 1);
                    }
                    else if (mascara.CampoMascara.Any(s => s.NombreCampo == helperCampoMascaraCaptura.NombreCampo && s.TipoCampoMascara.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango))
                    {
                        if (helperCampoMascaraCaptura.Valor != String.Empty)
                        {
                            string[] values = helperCampoMascaraCaptura.Valor.Split('|');
                            store += String.Format("'{0}',", values[0]);
                            store += String.Format("'{0}',", values[1]);
                        }
                        else
                        {
                            store += String.Format("'{0}',", "");
                            store += String.Format("'{0}',", "");
                        }
                    }
                    else
                        store += String.Format("'{0}',", helperCampoMascaraCaptura.Valor.Replace("'", "''"));
                }
                if (!contieneArchivo && esMail)
                    store += String.Format("'{0}',", String.Empty);
                store = store.Trim().TrimEnd(',');
                if (ticket.Random)
                    store = store + ", '" + ticket.ClaveRegistro + "'";
                db.ExecuteStoreCommand(store);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new Ticket { Id = ticket.Id, Random = campoRandom, ClaveRegistro = ticket.ClaveRegistro };
                new BusinessDemonio().ActualizaSla();

                if ((correo != String.Empty && !esMail) || (correo != String.Empty && forzarCorreo))
                {
                    string cuerpo = String.Format("Hola {0},<br><br>" +
                                    "Hemos recibido tu solicitud, los datos de tu ticket son:<br> ~replaceTicket~ <br><br>" +
                                    "<p><a href='" + ConfigurationManager.AppSettings["siteUrl"] + "/" + (string.IsNullOrEmpty(ConfigurationManager.AppSettings["siteUrlfolder"]) ? string.Empty : ConfigurationManager.AppSettings["siteUrlfolder"] + "/") + "/Publico/Consultas/FrmConsultaTicket.aspx?idTicket=" + result.Id + "&cveRandom=" + result.ClaveRegistro + "'>Ver ticket</a></p>" +
                                    "Nuestro personal de atención lo está revisando, si requieres hacer una actualización a tu solicitud por favor contesta este correo electrónico o ingresa a tu <a href='" + ConfigurationManager.AppSettings["siteUrl"] + "/" +
                            (string.IsNullOrEmpty(ConfigurationManager.AppSettings["siteUrlfolder"]) ? string.Empty : ConfigurationManager.AppSettings["siteUrlfolder"] + "/") + "'>cuenta</a>.", usuarioSolicito.Nombre);
                    new BusinessTicketMailService().EnviaCorreoTicketGenerado(result.Id, result.ClaveRegistro, string.Format("[Ticket: {0}]", ticket.Id), cuerpo, correo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }
        public PreTicket GeneraPreticket(int idArbol, int idUsuarioSolicita, int idUsuarioLevanto, string observaciones)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            PreTicket result;
            try
            {
                result = new PreTicket
                {
                    IdArbol = idArbol,
                    IdUsuarioSolicito = idUsuarioSolicita,
                    IdUsuarioAtendio = idUsuarioLevanto,
                    FechaHora =
                        DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff",
                            CultureInfo.InvariantCulture),
                    ClaveRegistro = UtilsTicket.GeneraCampoRandom(),
                    Observaciones = observaciones.Trim()
                };
                db.PreTicket.AddObject(result);
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
            return result;
        }
        public List<int> CapturaCasillaTicket(int idTicket, string nombreCampo)
        {
            List<int> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.MascaraSeleccionCatalogo.Where(w => w.IdTicket == idTicket && w.NombreCampoMascara == nombreCampo).Select(s => s.IdRegistroCatalogo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        #endregion Ticket Alta

        #region Movimientos
        private List<EstatusTicket> CambiaEstatus(int idEstatusActualTicket, int idGrupoUsuarioTicket, int? subRolPertenece)
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
                if (subRolPertenece == null)
                    qry = from q in qry
                          where q.IdSubRolPertenece == null
                          select q;
                else
                    qry = from q in qry
                          where q.IdSubRolPertenece == subRolPertenece
                          select q;
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
        #endregion Movimientos

        #region Consultas
        public HelperTicketDetalle ObtenerTicket(int idTicket, int idUsuario)
        {
            HelperTicketDetalle result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                if (ticket != null)
                {
                    db.LoadProperty(ticket, "EstatusTicket");
                    db.LoadProperty(ticket, "EstatusAsignacion");
                    db.LoadProperty(ticket, "TicketEstatus");
                    db.LoadProperty(ticket, "TicketConversacion");
                    foreach (TicketEstatus tEstatus in ticket.TicketEstatus)
                    {
                        db.LoadProperty(tEstatus, "EstatusTicket");
                        db.LoadProperty(tEstatus, "Usuario");
                    }
                    db.LoadProperty(ticket, "UsuarioLevanto");
                    db.LoadProperty(ticket, "EstatusTicket");
                    db.LoadProperty(ticket, "EstatusAsignacion");
                    db.LoadProperty(ticket, "TicketEstatus");
                    db.LoadProperty(ticket, "TicketAsignacion");
                    db.LoadProperty(ticket, "Impacto");
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
                    db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");
                    db.LoadProperty(ticket.ArbolAcceso.InventarioArbolAcceso.First(), "GrupoUsuarioInventarioArbol");
                    foreach (GrupoUsuarioInventarioArbol grupoinv in ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                    {
                        db.LoadProperty(grupoinv, "GrupoUsuario");
                    }

                    foreach (TicketAsignacion tAsignacion in ticket.TicketAsignacion)
                    {
                        db.LoadProperty(tAsignacion, "EstatusAsignacion");
                        db.LoadProperty(tAsignacion, "UsuarioAsignado");
                        db.LoadProperty(tAsignacion, "UsuarioAsigno");
                    }

                    bool grupoConSupervisor = (db.GrupoUsuario.Join(db.UsuarioGrupo, gu => gu.Id, ug => ug.IdGrupoUsuario,
                   (gu, ug) => new { gu, ug }).Where(@t => @t.ug.IdUsuario == idUsuario).Select(@t => @t.gu)).Any(a => a.TieneSupervisor);
                    bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);

                    List<int?> lstEstatusPermitidos = new List<int?>();
                    List<int> lstGrupos = db.UsuarioGrupo.Where(ug => ug.IdUsuario == idUsuario && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdGrupoUsuario).Distinct().ToList();

                    if (lstGrupos.Count <= 0)
                    {
                        lstGrupos = db.UsuarioGrupo.Where(ug => ug.IdUsuario == idUsuario && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).Distinct().ToList();
                        foreach (int idGrupo in lstGrupos)
                        {
                            lstEstatusPermitidos.AddRange((from etsrg in db.EstatusTicketSubRolGeneral
                                                           join gu in db.GrupoUsuario on new { gpo = etsrg.IdGrupoUsuario, sup = (bool)etsrg.TieneSupervisor } equals new { gpo = gu.Id, sup = gu.TieneSupervisor }
                                                           join sgu in db.SubGrupoUsuario on new { Gpo = gu.Id, gpoIn = etsrg.IdGrupoUsuario, sbr = (int)etsrg.IdSubRolSolicita } equals new { Gpo = sgu.IdGrupoUsuario, gpoIn = sgu.IdGrupoUsuario, sbr = sgu.IdSubRol }
                                                           join ug in db.UsuarioGrupo on new { idGpo = gu.Id, rol = etsrg.IdRolSolicita, dbgpo = sgu.Id } equals new { idGpo = ug.IdGrupoUsuario, rol = ug.IdRol, dbgpo = (int)ug.IdSubGrupoUsuario }
                                                           where gu.Id == idGrupo && ug.IdUsuario == idUsuario && etsrg.Habilitado
                                                           select etsrg.IdEstatusTicketActual).Distinct().ToList());
                        }
                    }
                    else
                    {
                        foreach (int idGrupo in lstGrupos)
                        {
                            lstEstatusPermitidos.AddRange((from etsrg in db.EstatusTicketSubRolGeneral
                                                           join gu in db.GrupoUsuario on new { gpo = etsrg.IdGrupoUsuario, sup = (bool)etsrg.TieneSupervisor } equals new { gpo = gu.Id, sup = gu.TieneSupervisor }
                                                           join sgu in db.SubGrupoUsuario on new { Gpo = gu.Id, gpoIn = etsrg.IdGrupoUsuario, sbr = (int)etsrg.IdSubRolSolicita } equals new { Gpo = sgu.IdGrupoUsuario, gpoIn = sgu.IdGrupoUsuario, sbr = sgu.IdSubRol }
                                                           join ug in db.UsuarioGrupo on new { idGpo = gu.Id, rol = etsrg.IdRolSolicita, dbgpo = sgu.Id } equals new { idGpo = ug.IdGrupoUsuario, rol = ug.IdRol, dbgpo = (int)ug.IdSubGrupoUsuario }
                                                           where gu.Id == idGrupo && ug.IdUsuario == idUsuario && etsrg.Habilitado
                                                           select etsrg.IdEstatusTicketActual).Distinct().ToList());
                        }
                    }

                    string nivelAsignado = String.Empty;

                    result = new HelperTicketDetalle();
                    result.IdTicket = ticket.Id;
                    result.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                    result.IdUsuarioLevanto = ticket.IdUsuarioLevanto;
                    result.UsuarioLevanto = ticket.UsuarioLevanto.NombreCompleto;
                    result.FechaSolicitud = ticket.FechaHoraAlta;
                    result.IdImpacto = ticket.IdImpacto;
                    result.Impacto = ticket.Impacto.Descripcion;
                    result.DiferenciaSla = String.Empty;
                    result.EstatusTicket = ticket.EstatusTicket;
                    result.EstatusAsignacion = ticket.EstatusAsignacion;
                    result.UltimaActualizacion = ticket.TicketEstatus.LastOrDefault() != null ? ticket.TicketEstatus.Last().FechaMovimiento : ticket.FechaHoraAlta;
                    result.EsPropietario = ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true : idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado;

                    result.Asigna = grupoConSupervisor ? (ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true
                        : idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado && ticket.IdEstatusTicket < (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto)
                        : lstEstatusPermitidos.Contains((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar);
                    result.Reasigna = false;
                    result.Escala = false;
                    result.CambiaEstatus = result.IdUsuarioAsignado == idUsuario;

                    result.NivelUsuarioAsignado = ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado != null ? ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado.UsuarioGrupo.Where(w => w.SubGrupoUsuario != null).Aggregate(nivelAsignado, (current, usuarioAsignado) => current + usuarioAsignado.SubGrupoUsuario.SubRol.Descripcion) : "";
                    result.IdUsuarioAsignado = ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado != null ? ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado.Id : 0;
                    result.UsuarioAsignado = ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado != null ? ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado : null;

                    result.DetalleUsuarioLevanto = new BusinessUsuarios().ObtenerDetalleUsuario(ticket.IdUsuarioLevanto);
                    result.CorreoUsuarioPrincipal = result.DetalleUsuarioLevanto.CorreoUsuario.First().Correo;

                    result.GrupoAsignado = ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Distinct().First().GrupoUsuario;
                    result.ConversacionDetalle = new List<HelperConversacionDetalle>();
                    foreach (HelperConversacionDetalle comentario in ticket.TicketConversacion.Select(conversacion => new HelperConversacionDetalle { Id = conversacion.Id, Nombre = conversacion.Usuario.NombreCompleto, FechaHora = conversacion.FechaGeneracion, Comentario = conversacion.Mensaje }))
                    {
                        result.ConversacionDetalle.Add(comentario);
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
        public List<HelperTickets> ObtenerTicketsUsuario(int idUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperTickets> result = null;
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<Ticket> lstTickets = db.Ticket.Where(w => w.IdUsuarioSolicito == idUsuario).ToList();

                bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug })
                        .Any(@t => @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);


                int totalRegistros = lstTickets.Count;
                if (totalRegistros > 0)
                {
                    result = new List<HelperTickets>();
                    foreach (Ticket ticket in lstTickets)
                    {
                        db.LoadProperty(ticket, "UsuarioLevanto");
                        db.LoadProperty(ticket.UsuarioLevanto, "TipoUsuario");
                        db.LoadProperty(ticket, "UsuarioSolicito");
                        db.LoadProperty(ticket.UsuarioSolicito, "TipoUsuario");

                        db.LoadProperty(ticket, "EstatusTicket");
                        db.LoadProperty(ticket, "EstatusAsignacion");
                        db.LoadProperty(ticket, "TicketEstatus");
                        db.LoadProperty(ticket, "TicketAsignacion");
                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Canal");
                        db.LoadProperty(ticket, "TicketEvento");
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
                        db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");
                        db.LoadProperty(ticket.ArbolAcceso.InventarioArbolAcceso.First(), "GrupoUsuarioInventarioArbol");
                        foreach (GrupoUsuarioInventarioArbol grupoinv in ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol)
                        {
                            db.LoadProperty(grupoinv, "GrupoUsuario");
                        }

                        HelperTickets hticket = new HelperTickets();
                        hticket.IdTicket = ticket.Id;
                        hticket.IdUsuario = ticket.IdUsuarioLevanto;
                        hticket.IdGrupoAsignado = ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Distinct().First().GrupoUsuario.Id;
                        hticket.FechaHora = ticket.FechaHoraAlta;
                        hticket.NumeroTicket = ticket.Id;
                        hticket.NombreUsuario = ticket.UsuarioLevanto.NombreCompleto;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                        hticket.GrupoAsignado = ticket.ArbolAcceso.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Distinct().First().GrupoUsuario.Descripcion;
                        hticket.EstatusTicket = ticket.EstatusTicket;
                        hticket.FechaCambioEstatusTicket = ticket.TicketEstatus.Last().FechaMovimiento;
                        hticket.FechaUltimoEvento = ticket.TicketEvento.Last().FechaHora;
                        hticket.EstatusAsignacion = ticket.EstatusAsignacion;
                        hticket.FechaCambioEstatusAsignacion = ticket.TicketAsignacion.Last().FechaAsignacion;
                        hticket.IdUsuarioAsignado = ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado != null ? ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado.Id : 0;
                        hticket.UsuarioAsignado = ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado != null ? ticket.TicketAsignacion.OrderBy(o => o.Id).Last().UsuarioAsignado.NombreCompleto : "";
                        hticket.IdSubRolAsignado = ObtenerSubRolAsignadoTicket(ticket.IdNivelTicket);
                        hticket.IdNivelAsignado = ticket.IdNivelTicket == null ? 0 : (int)ticket.IdNivelTicket;
                        hticket.NivelUsuarioAsignado = ticket.IdNivelTicket != null ? ((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)ticket.IdNivelTicket).ToString() : String.Empty;
                        hticket.EsPropietario = ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true : idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado;
                        hticket.CambiaEstatus = hticket.IdUsuarioAsignado == idUsuario;
                        hticket.Asigna = ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true : idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado && ticket.IdEstatusTicket < (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto;
                        hticket.Total = totalRegistros;
                        hticket.IdImpacto = ticket.IdImpacto;
                        hticket.TieneEncuesta = ticket.IdEncuesta.HasValue;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.TipoTicketDescripcion = ticket.TipoArbolAcceso.Descripcion;
                        hticket.TipoTicketAbreviacion = ticket.TipoArbolAcceso.Abreviacion;
                        hticket.UsuarioSolicito = ticket.UsuarioSolicito;
                        hticket.Vip = ticket.UsuarioLevanto.Vip;
                        hticket.Canal = ticket.Canal.Descripcion;
                        hticket.FechaEstimadaTermino = ticket.FechaHoraFinProceso ?? null;
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
        public HelperDetalleTicket ObtenerDetalleTicket(int idTicket)
        {
            HelperDetalleTicket result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket);
                if (ticket != null)
                {
                    db.LoadProperty(ticket, "EstatusTicket");
                    db.LoadProperty(ticket, "EstatusAsignacion");
                    db.LoadProperty(ticket, "TicketEstatus");
                    db.LoadProperty(ticket, "TicketConversacion");
                    foreach (TicketEstatus tEstatus in ticket.TicketEstatus)
                    {
                        db.LoadProperty(tEstatus, "EstatusTicket");
                        db.LoadProperty(tEstatus, "Usuario");
                    }
                    db.LoadProperty(ticket, "TicketAsignacion");
                    foreach (TicketAsignacion tAsignacion in ticket.TicketAsignacion)
                    {
                        db.LoadProperty(tAsignacion, "EstatusAsignacion");
                        db.LoadProperty(tAsignacion, "UsuarioAsignado");
                        db.LoadProperty(tAsignacion, "UsuarioAsigno");
                    }
                    result = new HelperDetalleTicket
                    {
                        IdTicket = ticket.Id,
                        IdEstatusTicket = ticket.IdEstatusTicket,
                        CveRegistro = ticket.ClaveRegistro,
                        IdEstatusAsignacion = ticket.IdEstatusAsignacion,
                        IdUsuarioLevanto = ticket.IdUsuarioSolicito,
                        EstatusActual = ticket.EstatusTicket.Descripcion,
                        AsignacionActual = ticket.EstatusAsignacion.Descripcion,
                        FechaCreacion = ticket.FechaHoraAlta,
                        TieneEncuesta = ticket.IdEncuesta.HasValue,
                        EstatusDetalle = new List<HelperEstatusDetalle>(),
                        AsignacionesDetalle = new List<HelperAsignacionesDetalle>(),
                        ConversacionDetalle = new List<HelperConversacionDetalle>()

                    };
                    foreach (HelperEstatusDetalle detalle in ticket.TicketEstatus.Select(movEstatus => new HelperEstatusDetalle { Descripcion = movEstatus.EstatusTicket.Descripcion, UsuarioMovimiento = movEstatus.Usuario.NombreCompleto, FechaMovimiento = movEstatus.FechaMovimiento, Comentarios = movEstatus.Comentarios }))
                    {
                        result.EstatusDetalle.Add(detalle);
                    }
                    foreach (HelperAsignacionesDetalle detalle in ticket.TicketAsignacion.Select(movAsignacion => new HelperAsignacionesDetalle { Descripcion = movAsignacion.EstatusAsignacion.Descripcion, UsuarioAsignado = movAsignacion.UsuarioAsignado != null ? movAsignacion.UsuarioAsignado.NombreCompleto : "SIN ASGNACIÓN", UsuarioAsigno = movAsignacion.UsuarioAsigno != null ? movAsignacion.UsuarioAsigno.NombreCompleto : "NO APLICA", FechaMovimiento = movAsignacion.FechaAsignacion }))
                    {
                        result.AsignacionesDetalle.Add(detalle);
                    }
                    foreach (HelperConversacionDetalle comentario in ticket.TicketConversacion.Select(conversacion => new HelperConversacionDetalle { Id = conversacion.Id, Nombre = conversacion.Usuario.NombreCompleto, FechaHora = conversacion.FechaGeneracion, Comentario = conversacion.Mensaje }))
                    {
                        result.ConversacionDetalle.Add(comentario);
                    }
                    result.ConversacionDetalle = result.ConversacionDetalle.OrderByDescending(o => o.FechaHora).ToList();
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
        public List<HelperTickets> ObtenerTickets(int idUsuario, List<int> estatus, int pageIndex, int pageSize)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperTickets> result = null;
            try
            {
                new BusinessDemonio().ActualizaSla();
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<Ticket> lstTickets;
                List<int> lstPrueba = new List<int>();
                List<GrupoUsuario> gruposPertenece = db.UsuarioGrupo.Where(w => w.IdUsuario == idUsuario && w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.GrupoUsuario).Distinct().ToList();
                List<int?> lstEstatusPermitidos;
                result = new List<HelperTickets>();
                #region Usuario
                if (gruposPertenece.Count <= 0)
                {
                    gruposPertenece = db.UsuarioGrupo.Where(ug => ug.IdUsuario == idUsuario && ug.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.GrupoUsuario).Distinct().ToList();
                    foreach (GrupoUsuario grupo in gruposPertenece)
                    {
                        lstEstatusPermitidos = (from etsrg in db.EstatusTicketSubRolGeneral
                                                join gu in db.GrupoUsuario on new { gpo = etsrg.IdGrupoUsuario, sup = (bool)etsrg.TieneSupervisor } equals new { gpo = gu.Id, sup = gu.TieneSupervisor }
                                                join sgu in db.SubGrupoUsuario on new { Gpo = gu.Id, gpoIn = etsrg.IdGrupoUsuario, sbr = (int)etsrg.IdSubRolSolicita } equals new { Gpo = sgu.IdGrupoUsuario, gpoIn = sgu.IdGrupoUsuario, sbr = sgu.IdSubRol }
                                                join ug in db.UsuarioGrupo on new { idGpo = gu.Id, rol = etsrg.IdRolSolicita, dbgpo = sgu.Id } equals new { idGpo = ug.IdGrupoUsuario, rol = ug.IdRol, dbgpo = (int)ug.IdSubGrupoUsuario }
                                                where gu.Id == grupo.Id && ug.IdUsuario == idUsuario && etsrg.Habilitado
                                                select etsrg.IdEstatusTicketActual).Distinct().ToList();
                        bool grupoConSupervisor = grupo.TieneSupervisor;
                        bool supervisor = db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug }).Any(@t => @t.sgu.IdGrupoUsuario == grupo.Id && @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario);
                        foreach (int? estatusPermitido in lstEstatusPermitidos)
                        {
                            if (estatusPermitido == null)
                                lstTickets = db.Ticket.Join(db.TicketAsignacion.OrderByDescending(o => o.Id).Take(1), t => t.Id,
                                        ta => ta.IdTicket, (t, ta) => new { t, ta }).Join(db.TicketGrupoUsuario, @t1 => @t1.t.Id, tgu => tgu.IdTicket, (@t1, tgu) => new { @t1, tgu })
                                        .Where(@t1 => @t1.tgu.IdGrupoUsuario == grupo.Id && @t1.@t1.ta.IdUsuarioAsignado == null && @t1.t1.t.IdEstatusAsignacion == estatusPermitido)
                                        .Select(@t1 => @t1.@t1.t).Distinct().ToList();
                            else
                            {
                                lstTickets = db.Ticket.Join(db.TicketAsignacion.OrderByDescending(o => o.Id).Take(1), t => t.Id, ta => ta.IdTicket, (t, ta) => new { t, ta })
                                        .Join(db.TicketGrupoUsuario, @t1 => @t1.t.Id, tgu => tgu.IdTicket, (@t1, tgu) => new { @t1, tgu })
                                        .Where(@t1 => @t1.tgu.IdGrupoUsuario == grupo.Id && @t1.t1.t.IdEstatusTicket == estatusPermitido && @t1.t1.ta.IdUsuarioAsignado == idUsuario)
                                        .Select(@t1 => @t1.@t1.t).Distinct().ToList();
                            }
                            lstTickets = lstTickets.Distinct().ToList();
                            lstPrueba.AddRange(lstTickets.Select(s => s.Id));
                            if (lstTickets.Count > 0)
                                result.AddRange(GeneraTicketsBandeja(db, lstTickets, lstEstatusPermitidos, idUsuario, grupoConSupervisor, supervisor, pageIndex, pageSize));
                        }
                    }
                }
                #endregion Usuario

                else
                {
                    foreach (GrupoUsuario grupo in gruposPertenece)
                    {
                        bool grupoConSupervisor = grupo.TieneSupervisor;
                        bool supervisor = grupoConSupervisor ?
                                             db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug }).Any(@t => @t.sgu.IdGrupoUsuario == grupo.Id && @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.Supervisor && @t.ug.IdUsuario == idUsuario)
                                             : db.SubGrupoUsuario.Join(db.UsuarioGrupo, sgu => sgu.Id, ug => ug.IdSubGrupoUsuario, (sgu, ug) => new { sgu, ug }).Any(@t => @t.sgu.IdGrupoUsuario == grupo.Id && @t.sgu.IdSubRol == (int)BusinessVariables.EnumSubRoles.PrimererNivel && @t.ug.IdUsuario == idUsuario);

                        var qryAsignacionPermitida = from easrg in db.EstatusAsignacionSubRolGeneral
                                                     join ug in db.UsuarioGrupo on easrg.IdGrupoUsuario equals ug.IdGrupoUsuario
                                                     join gu in db.GrupoUsuario on new { joingpoeasrg = easrg.IdGrupoUsuario, joingpoug = ug.IdGrupoUsuario } equals new { joingpoeasrg = gu.Id, joingpoug = gu.Id }
                                                     join sgu in db.SubGrupoUsuario on new { joinsreasrg = (int)easrg.IdSubRol, joingpoeasrgsgu = easrg.IdGrupoUsuario, joingpoug = ug.IdGrupoUsuario, joinidsguug = (int)ug.IdSubGrupoUsuario } equals new { joinsreasrg = sgu.IdSubRol, joingpoeasrgsgu = sgu.IdGrupoUsuario, joingpoug = sgu.IdGrupoUsuario, joinidsguug = sgu.Id }
                                                     where easrg.Habilitado && easrg.TieneSupervisor == grupoConSupervisor && gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente && ug.IdUsuario == idUsuario
                                                     select easrg;


                        var qryEstatusticket = from etsrg in db.EstatusTicketSubRolGeneral
                                               join ug in db.UsuarioGrupo on etsrg.IdGrupoUsuario equals ug.IdGrupoUsuario
                                               join gu in db.GrupoUsuario on new { joingpoeasrg = etsrg.IdGrupoUsuario, joingpoug = ug.IdGrupoUsuario } equals new { joingpoeasrg = gu.Id, joingpoug = gu.Id }
                                               join sgu in db.SubGrupoUsuario on new { joinsreasrg = (int)etsrg.IdSubRolSolicita, joingpoeasrgsgu = etsrg.IdGrupoUsuario, joingpoug = ug.IdGrupoUsuario }
                                               equals new { joinsreasrg = sgu.IdSubRol, joingpoeasrgsgu = sgu.IdGrupoUsuario, joingpoug = sgu.IdGrupoUsuario }
                                               where etsrg.Habilitado && etsrg.TieneSupervisor == grupoConSupervisor && gu.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente && ug.IdUsuario == idUsuario
                                               select etsrg;
                        if (!supervisor)
                        {
                            qryEstatusticket = from qe in qryEstatusticket
                                               where (bool)qe.Propietario
                                               select qe;
                            qryAsignacionPermitida = from q in qryAsignacionPermitida
                                                     where q.Propietario
                                                     select q;
                        }
                        List<int> lstEstatusAsignacionPermitidos = qryAsignacionPermitida.Select(s => s.IdEstatusAsignacionActual).Distinct().ToList();
                        lstEstatusPermitidos = qryEstatusticket.Select(s => s.IdEstatusTicketActual).Distinct().ToList();
                        foreach (int? estatusAsignacionPermitido in lstEstatusAsignacionPermitidos)
                        {
                            List<int> idsTicketMostrar = new List<int>();
                            lstTickets = new List<Ticket>();
                            if (supervisor)
                                lstTickets = db.Ticket.Join(db.TicketGrupoUsuario, t => t.Id, tg => tg.IdTicket, (t, tg) => new { t, tg })
                                        .Where(@t1 => @t1.tg.IdGrupoUsuario == grupo.Id && @t1.t.IdEstatusAsignacion == estatusAsignacionPermitido)
                                        .Select(@t1 => @t1.t).Distinct().ToList();
                            else if (estatusAsignacionPermitido == null)
                                lstTickets = db.Ticket.Join(db.TicketAsignacion.OrderByDescending(o => o.Id).Take(1), t => t.Id, ta => ta.IdTicket, (t, ta) => new { t, ta })
                                        .Join(db.TicketGrupoUsuario, @t1 => @t1.t.Id, tgu => tgu.IdTicket, (@t1, tgu) => new { @t1, tgu })
                                        .Where(@t1 => @t1.tgu.IdGrupoUsuario == grupo.Id && @t1.@t1.ta.IdUsuarioAsignado == null && @t1.t1.t.IdEstatusAsignacion == estatusAsignacionPermitido)
                                        .Select(@t1 => @t1.@t1.t).Distinct().ToList();
                            else
                            {
                                foreach (Ticket ticket in db.Ticket.Join(db.TicketGrupoUsuario, t => t.Id, tgu => tgu.IdTicket, (t, tgu) => new { t, tgu }).Where(@t1 => @t1.tgu.IdGrupoUsuario == grupo.Id && @t1.t.IdEstatusAsignacion == estatusAsignacionPermitido).Select(@t1 => @t1.t))
                                {
                                    db.LoadProperty(ticket, "TicketAsignacion");
                                    if (ticket.TicketAsignacion.Last().IdUsuarioAsignado == idUsuario)
                                        idsTicketMostrar.Add(ticket.Id);
                                    else
                                    {
                                        if (ticket.TicketAsignacion.Any() && ticket.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto)
                                            if (ticket.TicketAsignacion[ticket.TicketAsignacion.Count - 2].IdUsuarioAsignado == idUsuario)
                                                idsTicketMostrar.Add(ticket.Id);

                                    }
                                }
                                lstTickets = db.Ticket.Where(w => idsTicketMostrar.Contains(w.Id)).Distinct().ToList();
                            }
                            if (estatus != null && estatus.Count > 0)
                                lstTickets = lstTickets.Where(w => estatus.Contains(w.IdEstatusTicket)).Distinct().ToList();
                            lstTickets = lstTickets.Distinct().ToList();
                            lstPrueba.AddRange(lstTickets.Select(s => s.Id));
                            if (lstTickets.Count > 0)
                                result.AddRange(GeneraTicketsBandeja(db, lstTickets, lstEstatusPermitidos, idUsuario, grupoConSupervisor, supervisor, pageIndex, pageSize));
                        }
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

        public HelperDetalleTicket ObtenerDetalleTicketNoRegistrado(int idTicket, string cveRegistro)
        {
            HelperDetalleTicket result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket ticket = db.Ticket.SingleOrDefault(t => t.Id == idTicket && t.Random && t.ClaveRegistro == cveRegistro);
                if (ticket != null)
                {
                    db.LoadProperty(ticket, "EstatusTicket");
                    db.LoadProperty(ticket, "EstatusAsignacion");
                    db.LoadProperty(ticket, "TicketEstatus");
                    db.LoadProperty(ticket, "TicketConversacion");
                    db.LoadProperty(ticket, "UsuarioLevanto");
                    db.LoadProperty(ticket, "TicketGrupoUsuario");
                    foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                    {
                        db.LoadProperty(tgu, "GrupoUsuario");
                    }
                    foreach (TicketConversacion conversacion in ticket.TicketConversacion)
                    {
                        db.LoadProperty(conversacion, "ConversacionArchivo");
                        db.LoadProperty(conversacion, "Usuario");
                    }
                    foreach (TicketEstatus tEstatus in ticket.TicketEstatus)
                    {
                        db.LoadProperty(tEstatus, "EstatusTicket");
                        db.LoadProperty(tEstatus, "Usuario");
                    }
                    db.LoadProperty(ticket, "TicketAsignacion");
                    foreach (TicketAsignacion tAsignacion in ticket.TicketAsignacion)
                    {
                        db.LoadProperty(tAsignacion, "EstatusAsignacion");
                        db.LoadProperty(tAsignacion, "UsuarioAsignado");
                        db.LoadProperty(tAsignacion, "UsuarioAsigno");
                    }
                    result = new HelperDetalleTicket
                    {
                        IdTicket = ticket.Id,
                        IdNivelAsignado = ticket.IdNivelTicket != null ? (int)ticket.IdNivelTicket : 0,
                        IdEstatusTicket = ticket.IdEstatusTicket,
                        IdTipoUsuarioLevanto = ticket.UsuarioLevanto.IdTipoUsuario,
                        IdEstatusAsignacion = ticket.IdEstatusAsignacion,
                        IdUsuarioLevanto = ticket.IdUsuarioSolicito,
                        EstatusActual = ticket.EstatusTicket.Descripcion,
                        AsignacionActual = ticket.EstatusAsignacion.Descripcion,
                        FechaCreacion = ticket.FechaHoraAlta,
                        EstatusDetalle = new List<HelperEstatusDetalle>(),
                        AsignacionesDetalle = new List<HelperAsignacionesDetalle>(),
                        ConversacionDetalle = new List<HelperConversacionDetalle>()
                    };

                    result.EstatusDisponibles = CambiaEstatus(ticket.IdEstatusTicket, ticket.TicketGrupoUsuario.Single(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).IdGrupoUsuario, UtilsTicket.ObtenerRolAsignacionByIdNivel(ticket.IdNivelTicket));
                    foreach (HelperEstatusDetalle detalle in ticket.TicketEstatus.Select(movEstatus => new HelperEstatusDetalle { Descripcion = movEstatus.EstatusTicket.Descripcion, UsuarioMovimiento = movEstatus.Usuario.NombreCompleto, FechaMovimiento = movEstatus.FechaMovimiento, Comentarios = movEstatus.Comentarios }))
                    {
                        result.EstatusDetalle.Add(detalle);
                    }
                    foreach (HelperAsignacionesDetalle detalle in ticket.TicketAsignacion.Select(movAsignacion => new HelperAsignacionesDetalle { Descripcion = movAsignacion.EstatusAsignacion.Descripcion, UsuarioAsignado = movAsignacion.UsuarioAsignado != null ? movAsignacion.UsuarioAsignado.NombreCompleto : "SIN ASGNACIÓN", UsuarioAsigno = movAsignacion.UsuarioAsigno != null ? movAsignacion.UsuarioAsigno.NombreCompleto : "NO APLICA", FechaMovimiento = movAsignacion.FechaAsignacion }))
                    {
                        result.AsignacionesDetalle.Add(detalle);
                    }
                    foreach (TicketConversacion comentario in ticket.TicketConversacion)
                    {
                        HelperConversacionDetalle conversacion = new HelperConversacionDetalle
                        {
                            Id = comentario.Id,
                            Nombre = comentario.Usuario.NombreCompleto,
                            FechaHora = comentario.FechaGeneracion,
                            Comentario = comentario.Mensaje,
                            Privado = comentario.Privado.HasValue ? (bool)comentario.Privado : false
                        };
                        conversacion.Archivo = comentario.ConversacionArchivo.Any()
                            ? new List<HelperConversacionArchivo>()
                            : null;
                        if (conversacion.Archivo != null)
                            foreach (ConversacionArchivo archivoActual in comentario.ConversacionArchivo)
                            {
                                conversacion.Archivo.Add(new HelperConversacionArchivo { IdConversacion = archivoActual.IdTicketConversacion, Archivo = archivoActual.Archivo });
                            }
                        result.ConversacionDetalle.Add(conversacion);
                    }
                    result.ConversacionDetalle = result.ConversacionDetalle.OrderByDescending(o => o.FechaHora).ToList();
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
        #endregion Consultas

        #region Ticket Correo
        private void ObtenerFormularioCorreo(out ArbolAcceso arbol, out Mascara mascara)
        {
            try
            {
                int arbolParametro = int.Parse(ConfigurationManager.AppSettings["ServicioCorreo"]);
                arbol = new BusinessArbolAcceso().ObtenerArbolAcceso(arbolParametro);
                int? idMascara = arbol.InventarioArbolAcceso.First().IdMascara;
                if (idMascara != null)
                    mascara = new BusinessMascaras().ObtenerMascaraCaptura((int)idMascara);
                else
                    throw new Exception("No se encontro la Mascara de captura");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public PreTicketCorreo GeneraPreticketCorreo(string nombre, string apellidoPaterno, string apellidoMaterno, string correo, string asunto, string comentario, string archivoAdjunto)
        {
            PreTicketCorreo result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                string guid = String.Empty;
                bool guidValido = false;
                while (!guidValido)
                {
                    guid = Guid.NewGuid().ToString();
                    guidValido = !db.PreTicketCorreo.Any(a => a.Guid == guid);
                }
                result = new PreTicketCorreo
                {
                    Guid = guid,
                    Nombre = nombre.Trim().Length > 32 ? nombre.Trim().Substring(0, 32) : nombre.Trim(),
                    ApellidoPaterno = apellidoPaterno.Trim().Length > 32 ? apellidoPaterno.Trim().Substring(0, 32) : apellidoPaterno.Trim(),
                    ApellidoMaterno = apellidoMaterno.Trim().Length > 32 ? apellidoMaterno.Trim().Substring(0, 32) : apellidoMaterno.Trim(),
                    Correo = correo,
                    Asunto = asunto,
                    Comentario = comentario.Trim().Length > 3900 ? comentario.Trim().Substring(0, 3900) : comentario.Trim(),
                    Confirmado = false,
                    FechaSolicito = DateTime.Now,
                    FechaConfirmo = null
                };
                if (archivoAdjunto.Trim() != string.Empty)
                    result.ArchivoAdjunto = archivoAdjunto.Replace("ticketid", guid);
                else
                    result.ArchivoAdjunto = string.Empty;

                db.PreTicketCorreo.AddObject(result);
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
            return result;
        }

        public PreTicketCorreo ObtenerPreticketCorreo(string guid)
        {
            PreTicketCorreo result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.PreTicketCorreo.SingleOrDefault(s => s.Guid == guid && !s.Confirmado);
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

        //TODO: CAMBIAR A CORREO
        public void ConfirmaPreTicket(string guid, int idUsuario)
        {
            PreTicketCorreo datosPreticket;
            DataBaseModelContext db = new DataBaseModelContext();
            MailMessage reply = new MailMessage();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                datosPreticket = db.PreTicketCorreo.SingleOrDefault(s => s.Guid == guid);
                if (datosPreticket != null)
                {
                    string attname;
                    ArbolAcceso arbol;
                    Mascara mascara;
                    ObtenerFormularioCorreo(out arbol, out mascara);
                    Usuario user = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuario);
                    List<HelperCampoMascaraCaptura> capturaMascara = GeneraCaptura(mascara, user.NombreCompleto, user.CorreoPrincipal, datosPreticket.Asunto, datosPreticket.Comentario, datosPreticket.ArchivoAdjunto, out attname);
                    Ticket ticket = new BusinessTicket().CrearTicket(user.Id, user.Id, arbol.Id, capturaMascara, (int)BusinessVariables.EnumeradoresKiiniNet.EnumCanal.Correo, mascara.Random, true, true, true);
                    if (ticket != null)
                    {
                        datosPreticket.Confirmado = true;
                        datosPreticket.FechaConfirmo = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        db.SaveChanges();
                        reply.From = new MailAddress(datosPreticket.Correo);
                        reply.To.Add(datosPreticket.Correo);
                        reply.ReplyToList.Add(datosPreticket.Correo);
                        reply.Headers.Add("In-Reply-To", ticket.Id.ToString());
                        string references = "~ticket&" + ticket.Id + "~";

                        reply.Headers.Add("References", references);
                        reply.Subject = "Re: " + datosPreticket.Asunto;

                        reply.Subject = reply.Subject.Replace("(Trial Version)", string.Empty).Trim();
                        reply.IsBodyHtml = true;
                        reply.Body = GeneraRespuestaTicket(datosPreticket.Correo, datosPreticket.Nombre + datosPreticket.ApellidoPaterno + datosPreticket.ApellidoMaterno, datosPreticket.Comentario, DateTime.Now.ToString("dd/MM/yyyy"), ticket.Id, ticket.ClaveRegistro);
                        //new BusinessTicketMailService().EnviaCorreoTicketGenerado(datosPreticket.Id, ticket.ClaveRegistro, reply.Body, datosPreticket.Correo);
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

        public static List<HelperCampoMascaraCaptura> GeneraCaptura(Mascara mascara, string nombre, string from, string subject, string contenido, string archivoAdjunto, out string attname)
        {
            List<HelperCampoMascaraCaptura> result = new List<HelperCampoMascaraCaptura>();
            try
            {
                Attachment att;
                attname = String.Empty;
                foreach (CampoMascara campo in mascara.CampoMascara.OrderBy(o => o.Id))
                {
                    switch (campo.NombreCampo.ToUpper())
                    {
                        case "NOMBRE":
                            result.Add(new HelperCampoMascaraCaptura
                            {
                                IdCampo = campo.Id,
                                NombreCampo = campo.NombreCampo,
                                Valor = nombre
                            });
                            break;
                        case "CORREO":
                            result.Add(new HelperCampoMascaraCaptura
                            {
                                IdCampo = campo.Id,
                                NombreCampo = campo.NombreCampo,
                                Valor = from
                            });
                            break;
                        case "ASUNTO":
                            result.Add(new HelperCampoMascaraCaptura
                            {
                                IdCampo = campo.Id,
                                NombreCampo = campo.NombreCampo,
                                Valor = subject
                            });
                            break;
                        case "COMENTARIO":
                            result.Add(new HelperCampoMascaraCaptura
                            {
                                IdCampo = campo.Id,
                                NombreCampo = campo.NombreCampo,
                                Valor = contenido
                            });
                            break;
                        case "ARCHIVOADJUNTO":
                            if (string.IsNullOrEmpty(archivoAdjunto))
                            {
                                result.Add(new HelperCampoMascaraCaptura
                                {
                                    IdCampo = campo.Id,
                                    NombreCampo = campo.NombreCampo,
                                    Valor = string.Empty
                                });
                            }
                            else if (archivoAdjunto.Trim() != string.Empty)
                            {
                                result.Add(new HelperCampoMascaraCaptura
                                {
                                    IdCampo = campo.Id,
                                    NombreCampo = campo.NombreCampo,
                                    Valor = archivoAdjunto
                                });
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        #endregion Ticket Correo

        public string GeneraRespuestaTicket(string fromAddress, string fromName, string sourceBody, string sentDate, int idTicket, string claveRegistro)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                result.Append("<hr/>");
                result.Append("<p>Gracias por tu correo!</p>");
                result.Append(string.Format("<p>Hemos generado su ticket <br>No.: {0} <br>Clave: {1}.</p>", idTicket, claveRegistro));
                result.Append("<p>Responderemos lo mas pronto posible, Si tienes algun comentario contesta este correo .</p>");
                result.Append("<p>Saludos,<br>");
                result.Append("KiiniHelp");
                result.Append("</p>");
                result.Append("<br>");

                result.Append("<div>");
                result.AppendFormat("Fecha {0}, ", sentDate);

                if (!string.IsNullOrEmpty(fromName))
                    result.Append(fromName + ' ');

                result.AppendFormat("<<a href=\"mailto:{0}\">{0}</a>> wrote:<br/>", fromAddress);

                if (!string.IsNullOrEmpty(sourceBody))
                {
                    result.Append("<blockqoute style=\"margin: 0 0 0 5px;border-left:2px blue solid;padding-left:5px\">");
                    result.Append(sourceBody);
                    result.Append("</blockquote>");
                }

                result.Append("</div>");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result.ToString();
        }
    }
}
