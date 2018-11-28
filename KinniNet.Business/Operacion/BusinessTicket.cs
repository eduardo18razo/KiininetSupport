using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
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

        public void Dispose()
        {

        }

        public BusinessTicket(bool proxy = false)
        {
            _proxy = proxy;
        }

        private DateTime TiempoGeneral(List<HorarioSubGrupo> horarioSubGrupo, DateTime fechaLevanta, decimal? tiempoProceso)
        {
            DateTime result;
            try
            {
                List<DateTime> diasAsignados = new List<DateTime>();
                string horarioInicio = horarioSubGrupo.Min(s => s.HoraInicio);
                string horarioFin = horarioSubGrupo.Max(s => s.HoraFin);
                double tiempotrabajo = double.Parse(horarioFin.Replace(':', '.').Substring(0, 5)) - double.Parse(horarioInicio.Replace(':', '.').Substring(0, 5));
                TimeSpan horasLevanta = fechaLevanta.TimeOfDay;


                if (tiempoProceso != null)
                {
                    double horasTotalSolucion = double.Parse(tiempoProceso.ToString());
                    int contador = 0;
                    while (horasTotalSolucion > 0)
                    {

                        if (horarioSubGrupo.Any(a => a.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek))
                        {
                            horarioInicio = horarioSubGrupo.Where(w => w.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek).Min(m => m.HoraInicio);
                            horarioFin = horarioSubGrupo.Where(w => w.Dia == (int)DateTime.Now.AddDays(contador).DayOfWeek).Max(m => m.HoraFin);
                            if (contador == 0)
                                tiempotrabajo = double.Parse(Math.Round((DateTime.Parse(DateTime.Now.ToShortDateString() + " " + horarioFin) - fechaLevanta).TotalHours, 2, MidpointRounding.ToEven).ToString());
                            else
                                tiempotrabajo = double.Parse(Math.Round((DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioInicio) - DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioFin)).TotalHours, 2, MidpointRounding.ToEven).ToString());
                            tiempotrabajo = Math.Abs(tiempotrabajo);
                            if (tiempotrabajo >= horasTotalSolucion)
                            {
                                if (diasAsignados.Count <= 0)
                                {
                                    diasAsignados.Add(fechaLevanta.AddHours(horasTotalSolucion));
                                }
                                else
                                    diasAsignados.Add(diasAsignados.Last().AddHours(horasTotalSolucion));
                                horasTotalSolucion -= horasTotalSolucion;
                            }
                            else
                            {
                                diasAsignados.Add(fechaLevanta.AddHours(double.Parse(tiempotrabajo.ToString())));
                                horasTotalSolucion -= tiempotrabajo;
                            }



                            //if (diasAsignados.Count <= 0)
                            //{
                            //    decimal hrasRestantes = decimal.Parse(Math.Round((DateTime.Parse(DateTime.Now.ToShortDateString() + " " + horarioFin) - DateTime.Now).TotalHours, 2, MidpointRounding.ToEven).ToString());
                            //    if()
                            //        diasAsignados.Add(horasTotalSolucion < 0 ? DateTime.Now.AddHours(double.Parse(Math.Abs(decimal.Parse(horasTotalSolucion.ToString())).ToString())) : DateTime.Now.AddDays(contador + 1));
                            //    horasTotalSolucion -= decimal.Parse(Math.Round((DateTime.Parse(DateTime.Now.ToShortDateString() + " " + horarioFin) - DateTime.Now).TotalHours, 2, MidpointRounding.ToEven).ToString());

                            //}
                            //else
                            //{
                            //    if (horasTotalSolucion >= decimal.Parse(tiempotrabajo.ToString()))
                            //    {
                            //        horasTotalSolucion -= decimal.Parse(tiempotrabajo.ToString());
                            //        diasAsignados.Add(DateTime.Now.AddDays(contador));
                            //    }
                            //    else
                            //    {
                            //        DateTime fecha = DateTime.Parse(DateTime.Now.AddDays(contador).ToShortDateString() + " " + horarioInicio).AddHours(double.Parse(horasTotalSolucion.ToString()));
                            //        horasTotalSolucion -= horasTotalSolucion;
                            //        diasAsignados.Add(fecha);
                            //    }
                            //}
                        }
                        contador++;
                    }
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

        public Ticket CrearTicket(int idUsuario, int idUsuarioSolicito, int idArbol, List<HelperCampoMascaraCaptura> lstCaptura, int idCanal, bool campoRandom, bool esTercero, bool esMail)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            Ticket result;
            try
            {
                string correo = string.Empty;
                Usuario usuarioLevanto = new BusinessUsuarios().ObtenerUsuario(idUsuario);
                Usuario usuarioSolicito = new BusinessUsuarios().ObtenerUsuario(idUsuarioSolicito);
                ArbolAcceso arbol = new BusinessArbolAcceso().ObtenerArbolAcceso(idArbol);
                Mascara mascara = new BusinessMascaras().ObtenerMascaraCaptura(arbol.InventarioArbolAcceso.First().IdMascara ?? 0);
                Encuesta encuesta = new BusinessEncuesta().ObtenerEncuestaById(arbol.InventarioArbolAcceso.First().IdEncuesta ?? 0);
                Sla sla = new BusinessSla().ObtenerSla(arbol.InventarioArbolAcceso.First().IdSla ?? 0);
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
                correo = usuarioSolicito.CorreoUsuario.FirstOrDefault(f => f.Obligatorio) == null ? string.Empty : usuarioSolicito.CorreoUsuario.FirstOrDefault(f => f.Obligatorio).Correo;
                if (correo != string.Empty)
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
                DateTime fechaTermino = TiempoGeneral(lstHorarioGrupo, ticket.FechaHoraAlta, ticket.SlaEstimadoTicket.TiempoHoraProceso);
                ticket.FechaHoraFinProceso = fechaTermino;
                ticket.SlaEstimadoTicket.FechaFinProceso = fechaTermino;
                ticket.SlaEstimadoTicket.FechaFin = fechaTermino;

                //SLA DETALLE
                ticket.SlaEstimadoTicket.SlaEstimadoTicketDetalle.AddRange(
                    sla.SlaDetalle.Select(
                        detalle =>
                            new SlaEstimadoTicketDetalle
                            {
                                IdSubRol = detalle.IdSubRol,
                                Dias = sla.Dias,
                                Horas = sla.Horas,
                                Minutos = sla.Minutos,
                                Segundos = sla.Segundos,
                                TiempoProceso = detalle.TiempoProceso
                            }));

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
                                if (campo.Valor != string.Empty)
                                {
                                    string[] values = campo.Valor.Split('|');
                                    foreach (string value in values)
                                    {
                                        ticket.MascaraSeleccionCatalogo.Add(new MascaraSeleccionCatalogo
                                        {
                                            NombreCampoMascara = campo.NombreCampo,
                                            IdRegistroCatalogo = int.Parse(value),
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

                string store = string.Format("{0} '{1}',", mascara.ComandoInsertar, ticket.Id);
                bool contieneArchivo = false;
                foreach (HelperCampoMascaraCaptura helperCampoMascaraCaptura in lstCaptura)
                {
                    if (mascara.CampoMascara.Any(s => s.NombreCampo == helperCampoMascaraCaptura.NombreCampo && s.TipoCampoMascara.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.AdjuntarArchivo))
                    {

                        store += string.Format("'{0}',", helperCampoMascaraCaptura.Valor.Replace("ticketid", ticket.Id.ToString()));
                        contieneArchivo = true;
                    }
                    else if (mascara.CampoMascara.Any(s => s.NombreCampo == helperCampoMascaraCaptura.NombreCampo && s.TipoCampoMascara.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.CasillaDeVerificación))
                    {
                        store += string.Format("'{0}',", 1);
                    }
                    else if (mascara.CampoMascara.Any(s => s.NombreCampo == helperCampoMascaraCaptura.NombreCampo && s.TipoCampoMascara.Id == (int)BusinessVariables.EnumeradoresKiiniNet.EnumTiposCampo.FechaRango))
                    {
                        if (helperCampoMascaraCaptura.Valor != string.Empty)
                        {
                            string[] values = helperCampoMascaraCaptura.Valor.Split('|');
                            store += string.Format("'{0}',", values[0]);
                            store += string.Format("'{0}',", values[1]);
                        }
                        else
                        {
                            store += string.Format("'{0}',", "");
                            store += string.Format("'{0}',", "");
                        }
                    }
                    else
                        store += string.Format("'{0}',", helperCampoMascaraCaptura.Valor.Replace("'", "''"));
                }
                if (!contieneArchivo && esMail)
                    store += string.Format("'{0}',", string.Empty);
                store = store.Trim().TrimEnd(',');
                if (ticket.Random)
                    store = store + ", '" + ticket.ClaveRegistro + "'";
                db.ExecuteStoreCommand(store);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new Ticket { Id = ticket.Id, Random = campoRandom, ClaveRegistro = ticket.ClaveRegistro };
                new BusinessDemonio().ActualizaSla();

                if (correo != string.Empty && !esMail)
                {
                    string cuerpo = string.Format("Hola {0},<br><br>" +
                                    "Hemos recibido tu solicitud, los datos de tu ticket son:<br> ~replaceTicket~ <br><br>" +
                                    "Nuestro personal de atención lo está revisando, si requieres hacer una actualización a tu solicitud por favor contesta este correo electrónico o ingresa a tu <a href='\"" + ConfigurationManager.AppSettings["siteUrl"] + ConfigurationManager.AppSettings["siteUrlfolder"] + "/Publico/Consultas/FrmConsultaTicket.aspx?idTicket=" + result.Id + "&cveRandom=" + result.ClaveRegistro + "'\">cuenta</a>.", usuarioSolicito.Nombre);
                    new BusinessTicketMailService().EnviaCorreoTicketGenerado(result.Id, result.ClaveRegistro, cuerpo, correo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally { db.Dispose(); }
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
                        hticket.NivelUsuarioAsignado = ticket.IdNivelTicket != null ? ((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)ticket.IdNivelTicket).ToString() : string.Empty;
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

                        //db.LoadProperty(ticket, "EstatusTicket");
                        //db.LoadProperty(ticket, "EstatusAsignacion");

                        db.LoadProperty(ticket, "Impacto");
                        db.LoadProperty(ticket, "TipoArbolAcceso");
                        db.LoadProperty(ticket, "Canal");


                        db.LoadProperty(ticket, "UsuarioUltimoMovimiento");
                        db.LoadProperty(ticket, "UltimoAgenteAsignado");

                        //foreach (TicketGrupoUsuario ticketGrupoUsuario in ticket.TicketGrupoUsuario)
                        //{
                        //    db.LoadProperty(ticketGrupoUsuario, "GrupoUsuario");
                        //}
                        //db.LoadProperty(ticket, "ArbolAcceso");
                        //db.LoadProperty(ticket.ArbolAcceso, "InventarioArbolAcceso");

                        HelperTickets hticket = new HelperTickets();
                        hticket.IdTicket = ticket.Id;
                        hticket.IdUsuario = ticket.IdUsuarioLevanto;
                        var gpoAgenteAtencion = (db.TicketGrupoUsuario.Where(w => w.IdTicket == ticket.Id).Join(db.GrupoUsuario.Where(
                            w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente),
                            tgu => tgu.IdGrupoUsuario, gu => gu.Id, (tgu, gu) => gu)).Distinct().First();

                        hticket.FechaHora = ticket.FechaHoraAlta;
                        hticket.NumeroTicket = ticket.Id;
                        hticket.NombreUsuario = ticket.UsuarioLevanto.NombreCompleto;
                        hticket.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);

                        hticket.FechaUltimoEvento = ticket.FechaUltimoMovimiento;
                        hticket.EstatusTicket = lstEstatusEstatusTicket.Single(s=>s.Id == ticket.IdEstatusTicket);
                        hticket.EstatusAsignacion = lstEstatusAsignaciones.Single(s=>s.Id == ticket.IdEstatusAsignacion);
                        hticket.FechaCambioEstatusAsignacion = ticket.FechaUltimoAgenteAsignado;
                        hticket.IdUsuarioAsignado = ticket.IdUsuarioUltimoAgenteAsignado != null ? ticket.UltimoAgenteAsignado.Id : 0;
                        hticket.UsuarioAsignado = ticket.IdUsuarioUltimoAgenteAsignado != null ? ticket.UltimoAgenteAsignado.NombreCompleto : "";
                        hticket.IdSubRolAsignado = ObtenerSubRolAsignadoTicket(ticket.IdNivelTicket);
                        hticket.IdNivelAsignado = ticket.IdNivelTicket == null ? 0 : (int)ticket.IdNivelTicket;
                        hticket.TieneEncuesta = ticket.IdEncuesta.HasValue;
                        hticket.NivelUsuarioAsignado = ticket.IdNivelTicket != null ? ((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)ticket.IdNivelTicket).ToString() : string.Empty;
                        hticket.NivelUsuarioAsignado = ticket.IdNivelTicket != null ? ((BusinessVariables.EnumeradoresKiiniNet.EnumeradorNivelAsignacion)ticket.IdNivelTicket).ToString() : string.Empty;
                        //hticket.EsPropietario = ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar ? false : idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado;
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
                                //hticket.Asigna = gpoAgenteAtencion.TieneSupervisor
                                //        ? (ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true
                                //            : idUsuario == ticket.TicketAsignacion.Last().IdUsuarioAsignado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado)
                                //        : lstEstatusPermitidos.Contains((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar);
                                hticket.Asigna = gpoAgenteAtencion.TieneSupervisor
                                        ? (ticket.IdEstatusAsignacion == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar && supervisor ? true
                                            : idUsuario == ticket.IdUsuarioUltimoAgenteAsignado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado && ticket.IdEstatusTicket != (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado)
                                        : lstEstatusPermitidos.Contains((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusAsignacion.PorAsignar);
                            }
                        }
                        hticket.ImagenPrioridad = ticket.Impacto.Descripcion == "ALTO"
                            ? "~/assets/images/icons/prioridadalta.png"
                            : ticket.Impacto.Descripcion == "MEDIO" ? "~/assets/images/icons/prioridadmedia.png" : "~/assets/images/icons/prioridadbaja.png";
                        hticket.ImagenSla = ticket.DentroSla ? "~/assets/images/icons/SLA_verde.png" : "~/assets/images/icons/SLA_rojo.png";
                        hticket.Total = totalRegistros;
                        hticket.IdImpacto = ticket.IdImpacto;
                        hticket.Impacto = ticket.Impacto.Descripcion;
                        hticket.TipoTicketDescripcion = ticket.TipoArbolAcceso.Descripcion;
                        hticket.TipoTicketAbreviacion = ticket.TipoArbolAcceso.Abreviacion;
                        hticket.UsuarioSolicito = ticket.UsuarioSolicito;
                        hticket.Vip = ticket.UsuarioSolicito.Vip;
                        hticket.Canal = ticket.Canal.Descripcion;
                        hticket.DentroSla = ticket.DentroSla;
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

        public List<HelperTickets> ObtenerTickets(int idUsuario, List<int> estatus, int pageIndex, int pageSize)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            List<HelperTickets> result = null;
            try
            {
                ActualizaSla();
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
                                where (bool) qe.Propietario
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

                    string nivelAsignado = string.Empty;

                    result = new HelperTicketDetalle();
                    result.IdTicket = ticket.Id;
                    result.Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso);
                    result.IdUsuarioLevanto = ticket.IdUsuarioLevanto;
                    result.UsuarioLevanto = ticket.UsuarioLevanto.NombreCompleto;
                    result.FechaSolicitud = ticket.FechaHoraAlta;
                    result.IdImpacto = ticket.IdImpacto;
                    result.Impacto = ticket.Impacto.Descripcion;
                    result.DiferenciaSla = string.Empty;
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
    }
}
