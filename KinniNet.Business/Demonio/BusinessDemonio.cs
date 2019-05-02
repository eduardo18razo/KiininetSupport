using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;
using KinniNet.Core.Parametros;
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

        public string ActualizaSla()
        {
            StringBuilder result = new StringBuilder();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                List<int> enumEstatusExcluidos = new List<int>();
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado);
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado);
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera);
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReTipificado);
                var ticketDentroSla = db.Ticket.Where(w => w.DentroSla && w.FechaTermino == null && w.FechaHoraFinProceso != null && !enumEstatusExcluidos.Contains(w.IdEstatusTicket));
                result.AppendLine(string.Format("Procesando {0} tickets ", ticketDentroSla.Count()));
                foreach (Ticket ticket in ticketDentroSla)
                {
                    ticket.DentroSla = DateTime.Now <= ticket.FechaHoraFinProceso;
                    result.AppendLine(string.Format("Ticket {0} {1} ", ticket.Id, ticket.DentroSla ? "Dentro SLA" : "Fuera SLA"));
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                result.AppendLine("Error: " + e.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result.ToString().Trim();
        }

        private string NotificacionesVencimiento(bool antesVencimiento)
        {
            StringBuilder result = new StringBuilder();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                result.AppendLine(string.Format("Procesando notificaciones {0} de vencimiento", antesVencimiento ? "antes" : "despues"));
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                //Listas Notificacion Correo
                List<Ticket> informeNotificacionCorreo = new List<Ticket>();

                ////Listas Notificacion SMS
                List<Ticket> informeNotificacionSms = new List<Ticket>();
                List<int> enumEstatusExcluidos = new List<int>();
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado);
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cancelado);
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.EnEspera);
                enumEstatusExcluidos.Add((int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.ReTipificado);
                ParametrosGenerales parametros = db.ParametrosGenerales.SingleOrDefault();
                if (parametros != null)
                {
                    List<int> tiposGposNotificar = new List<int>();
                    tiposGposNotificar.Add((int)BusinessVariables.EnumTiposGrupos.Notificaciones);

                    //DateTime fechaConsulta = antesVencimiento ? DateTime.Now : DateTime.Now.AddMinutes(parametros.MensajesNotificacion * 10);
                    DateTime fechaConsulta = DateTime.Now;
                    DateTime fechaInicioConsulta = fechaConsulta.AddMinutes((parametros.MensajesNotificacion * 10) * -1);
                    DateTime fechaFinConsulta = fechaConsulta.AddMinutes(parametros.MensajesNotificacion * 10);
                    List<TiempoInformeArbol> lstTiempoInforme = (from tia in db.TiempoInformeArbol
                                                                 join t in db.Ticket on tia.IdArbol equals t.IdArbolAcceso
                                                                 where tia.AntesVencimiento == antesVencimiento
                                                                 && t.DentroSla == antesVencimiento && t.FechaHoraFinProceso != null && !enumEstatusExcluidos.Contains(t.IdEstatusTicket)
                                                                 select tia).Distinct().ToList();
                    foreach (TiempoInformeArbol tiempoInforme in lstTiempoInforme)
                    {
                        int tiemponotificacionHoras;
                        var qry = from t in db.Ticket
                                  where t.IdEstatusTicket < (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto
                                  && t.IdArbolAcceso == tiempoInforme.IdArbol
                                  select t;
                        if (antesVencimiento)
                        {
                            if (tiempoInforme.Dias > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Dias);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddDays((DateTime?)q.FechaHoraFinProceso, -tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                                //where System.Data.Objects.EntityFunctions.AddDays((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) >= fechaInicioConsulta
                                //&& System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaFinConsulta
                            }
                            if (tiempoInforme.Horas > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Horas);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddHours((DateTime?)q.FechaHoraFinProceso, -tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                            if (tiempoInforme.Minutos > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Minutos);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, -tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                            if (tiempoInforme.Segundos > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Segundos);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddSeconds((DateTime?)q.FechaHoraFinProceso, -tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                        }
                        else
                        {
                            if (tiempoInforme.Dias > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Dias);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddDays((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) >= fechaInicioConsulta
                                      && System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaFinConsulta
                                      select q;
                            }
                            if (tiempoInforme.Horas > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Horas);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddHours((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) >= fechaInicioConsulta
                                      && System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaFinConsulta
                                      select q;
                            }
                            if (tiempoInforme.Minutos > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Minutos);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) >= fechaInicioConsulta
                                      && System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaFinConsulta
                                      select q;
                            }
                            if (tiempoInforme.Segundos > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Segundos);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddSeconds((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) >= fechaInicioConsulta
                                      && System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaFinConsulta
                                      select q;
                            }
                        }
                        List<Ticket> tickets = qry.Distinct().ToList();
                        result.AppendLine(string.Format("Se encontraron {0} para procesar", tickets.Count));
                        foreach (Ticket t in tickets)
                        {
                            db.LoadProperty(t, "UsuarioLevanto");
                        }
                        switch (tiempoInforme.IdTipoNotificacion)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo:
                                switch (tiempoInforme.IdTipoGrupo)
                                {
                                    case (int)BusinessVariables.EnumTiposGrupos.Notificaciones:
                                        informeNotificacionCorreo.AddRange(tickets.ToList().Distinct());
                                        break;
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms:
                                switch (tiempoInforme.IdTipoGrupo)
                                {
                                    case (int)BusinessVariables.EnumTiposGrupos.Notificaciones:
                                        informeNotificacionSms.AddRange(tickets.ToList().Distinct());
                                        break;
                                }
                                break;
                        }
                    }

                    // Envia notificacion Correo
                    if (informeNotificacionCorreo.Any())
                        result.AppendLine(EnviaNotificacion(informeNotificacionCorreo, (int)BusinessVariables.EnumTiposGrupos.Notificaciones, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo, parametros));

                    //Envia Notificacion SMS
                    if (informeNotificacionSms.Any())
                        result.AppendLine(EnviaNotificacion(informeNotificacionSms, (int)BusinessVariables.EnumTiposGrupos.Notificaciones, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms, parametros));
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result.ToString().Trim();
        }

        private string EnviaNotificacion(List<Ticket> lstInforme, int idTipoGrupo, bool antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion tipoNotificacion, ParametrosGenerales parametros)
        {
            StringBuilder result = new StringBuilder();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                foreach (Ticket ticket in lstInforme.Distinct())
                {
                    //db.LoadProperty(ticket, "UsuarioLevanto");
                    foreach (TicketGrupoUsuario tgu in db.TicketGrupoUsuario.Where(w => w.IdTicket == ticket.Id && w.GrupoUsuario.IdTipoGrupo == idTipoGrupo).Distinct())
                    {
                        db.LoadProperty(tgu, "GrupoUsuario");
                        db.LoadProperty(tgu.GrupoUsuario, "UsuarioGrupo");
                        foreach (UsuarioGrupo ug in tgu.GrupoUsuario.UsuarioGrupo.Distinct())
                        {
                            string mensaje;
                            switch (tipoNotificacion)
                            {

                                case BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo:
                                    db.LoadProperty(ug, "Usuario");
                                    db.LoadProperty(ug.Usuario, "CorreoUsuario");
                                    foreach (CorreoUsuario correoUsuario in ug.Usuario.CorreoUsuario.Distinct())
                                    {
                                        mensaje = string.Format("Ticket No.: <b>{0}</b> Clave <b>{1}</b>" +
                                                                "<br>Grupo Notificado <b>{2}</b> " +
                                                                "<br>Usuario que pertenece al grupo <b>{3}</b> " +
                                                                "<br>Persona Levanto <b>{4}</b>" +
                                                                "<br>Fecha levanto <b>{5}</b> " +
                                                                "<br>Fecha termino estimada <b>{6}</b> ", ticket.Id,
                                            ticket.Random ? ticket.ClaveRegistro : "N/A",
                                            tgu.GrupoUsuario.Descripcion,
                                            correoUsuario.Usuario.NombreCompleto,
                                            ticket.UsuarioLevanto.NombreCompleto,
                                            ticket.FechaHoraAlta, ticket.FechaHoraFinProceso);
                                        GeneraCorreo(ticket.Id, ug.IdGrupoUsuario, correoUsuario.IdUsuario, correoUsuario.Correo, antesVencimiento, mensaje,
                                            parametros);
                                        result.AppendLine(string.Format("Se envio notificación de ticket {0} a correo: {1}", ticket.Id, correoUsuario.Correo));
                                    }
                                    break;
                                case BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms:
                                    db.LoadProperty(ug, "Usuario");
                                    db.LoadProperty(ug.Usuario, "TelefonoUsuario");
                                    foreach (TelefonoUsuario telefono in ug.Usuario.TelefonoUsuario.Where(w => w.IdTipoTelefono == (int)BusinessVariables.EnumTipoTelefono.Celular).Distinct())
                                    {
                                        mensaje = string.Format("Ticket No.: <b>{0}</b> Clave <b>{1}</b>" +
                                                                "<br>Grupo Notificado <b>{2}</b> " +
                                                                "<br>Usuario que pertenece al grupo <b>{3}</b> " +
                                                                "<br>Persona Levanto <b>{4}</b>" +
                                                                "<br>Fecha levanto <b>{5}</b> " +
                                                                "<br>Fecha termino estimada <b>{6}</b> ", ticket.Id,
                                            ticket.Random ? ticket.ClaveRegistro : "N/A",
                                            tgu.GrupoUsuario.Descripcion,
                                            telefono.Usuario.NombreCompleto,
                                            ticket.UsuarioLevanto.NombreCompleto,
                                            ticket.FechaHoraAlta, ticket.FechaHoraFinProceso);
                                        GeneraSms(ticket.Id, ug.IdGrupoUsuario, telefono.IdUsuario, telefono.Numero, mensaje, parametros);
                                        result.AppendLine(string.Format("Se envio notificación de ticket {0} a numero: {1}", ticket.Id, telefono.Numero));
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch { }
            finally
            {
                db.Dispose();
            }
            return result.ToString().Trim();
        }

        private void GeneraCorreo(int idTicket, int idGrupoUsuario, int idUsuario, string correoUsuario, bool antesVencimiento, string mensaje, ParametrosGenerales parametros)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (db.TicketNotificacion.Count(c => c.IdTicket == idTicket && c.IdGrupoUsuario == idGrupoUsuario && c.CorreoUsuario == correoUsuario) < parametros.MensajesNotificacion)
                {
                    TicketNotificacion lastNotificacion = db.TicketNotificacion.Where(w => w.IdTicket == idTicket && w.IdGrupoUsuario == idGrupoUsuario && w.CorreoUsuario == correoUsuario).ToList().LastOrDefault();
                    if (lastNotificacion != null)
                    {
                        if (DateTime.Now >= lastNotificacion.FechaNotificacion.AddMinutes(parametros.FrecuenciaNotificacionMinutos))
                        {

                            TicketNotificacion tn = new TicketNotificacion
                            {
                                IdTicket = idTicket,
                                IdGrupoUsuario = idGrupoUsuario,
                                IdUsuario = idUsuario,
                                CorreoUsuario = correoUsuario,
                                IdTipoNotificacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo,
                                FechaNotificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture)
                            };
                            db.TicketNotificacion.AddObject(tn);
                            db.SaveChanges();
                            BusinessCorreo.SendMail(correoUsuario,
                                string.Format("{0}", antesVencimiento ? "Ticket apunto de vencer" : "Ticket ha vencido"),
                                mensaje);
                        }
                    }
                    else
                    {
                        TicketNotificacion tn = new TicketNotificacion
                        {
                            IdTicket = idTicket,
                            IdGrupoUsuario = idGrupoUsuario,
                            IdUsuario = idUsuario,
                            CorreoUsuario = correoUsuario,
                            IdTipoNotificacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo,
                            FechaNotificacion =
                                DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),
                                    "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture)
                        };
                        db.TicketNotificacion.AddObject(tn);
                        db.SaveChanges();
                        BusinessCorreo.SendMail(correoUsuario,
                            string.Format("{0}", antesVencimiento ? "Ticket apunto de vencer" : "Ticket ha vencido"),
                            mensaje);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                db.Dispose();
            }
        }

        private void GeneraSms(int idTicket, int idGrupoUsuario, int idUsuario, string numero, string mensaje, ParametrosGenerales parametros)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (db.TicketNotificacion.Count(c => c.IdTicket == idTicket && c.IdGrupoUsuario == idGrupoUsuario && c.TelefonoUsuario == numero && c.SmsService.Numero == numero) < parametros.MensajesNotificacion)
                {
                    TicketNotificacion lastNotificacion = db.TicketNotificacion.Where(w => w.IdTicket == idTicket && w.IdGrupoUsuario == idGrupoUsuario && w.TelefonoUsuario == numero && w.SmsService.Numero == numero).ToList().LastOrDefault();
                    if (lastNotificacion != null)
                    {
                        if (lastNotificacion.FechaNotificacion.AddMinutes(parametros.FrecuenciaNotificacionMinutos) >= DateTime.Now)
                        {
                            TicketNotificacion tn = new TicketNotificacion
                            {
                                IdTicket = idTicket,
                                IdGrupoUsuario = idGrupoUsuario,
                                IdUsuario = idUsuario,
                                TelefonoUsuario = numero,
                                IdTipoNotificacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms,
                                FechaNotificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                                SmsService = new SmsService
                                {
                                    IdUsuario = idUsuario,
                                    IdTipoLink = (int)BusinessVariables.EnumTipoLink.Notificacion,
                                    Numero = numero,
                                    Mensaje = mensaje,
                                    Enviado = false,
                                    Habilitado = true
                                }
                            };
                            db.TicketNotificacion.AddObject(tn);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        TicketNotificacion tn = new TicketNotificacion
                        {
                            IdTicket = idTicket,
                            IdGrupoUsuario = idGrupoUsuario,
                            IdUsuario = idUsuario,
                            TelefonoUsuario = numero,
                            IdTipoNotificacion = (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms,
                            FechaNotificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            SmsService = new SmsService
                            {
                                IdUsuario = idUsuario,
                                IdTipoLink = (int)BusinessVariables.EnumTipoLink.Notificacion,
                                Numero = numero,
                                Mensaje = mensaje,
                                Enviado = false,
                                Habilitado = true
                            }
                        };
                        db.TicketNotificacion.AddObject(tn);
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                db.Dispose();
            }
        }

        public string EnvioNotificacion()
        {
            StringBuilder result = new StringBuilder();
            try
            {
                result.AppendLine(ActualizaSla());
                result.AppendLine(NotificacionesVencimiento(false));
                result.AppendLine(NotificacionesVencimiento(true));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return result.ToString().Trim();
        }

        public string CierraTicketsResueltos()
        {
            StringBuilder result = new StringBuilder();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                result.AppendLine("Obteniendo parametros generales");
                ParametrosGenerales parametros = new BusinessParametros().ObtenerParametrosGenerales();
                if (parametros != null)
                {
                    if (parametros.DiasCierreTicket != null)
                    {
                        DateTime fecha = DateTime.Now.AddDays(-(double)parametros.DiasCierreTicket);
                        result.AppendLine(string.Format("Fecha aplicada {0} - {1}", fecha.ToLongDateString(), fecha.ToLongTimeString()));
                        var tickets = db.Ticket.Where(w => w.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto && w.FechaTermino <= fecha);
                        result.AppendLine(string.Format("{0} Tickets encontrados", tickets.Count()));
                        foreach (Ticket ticket in tickets)
                        {
                            new BusinessAtencionTicket().CambiarEstatus(ticket.Id, (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado, ticket.IdUsuarioSolicito, "Cerrado por sistema");
                            result.AppendLine(string.Format("Cierre de ticket {0} correctamente", ticket.Id));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                result.AppendLine(e.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result.ToString().Trim();
        }
    }
}
