using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        private void NotificacionesVencimiento(bool antesVencimiento)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                //Listas Notificacion Correo
                List<Ticket> informeDueñoCorreo = new List<Ticket>();
                List<Ticket> informeMantenimientoCorreo = new List<Ticket>();
                List<Ticket> informeDesarrolloCorreo = new List<Ticket>();
                List<Ticket> informeConsultaCorreo = new List<Ticket>();
                //Listas Notificacion SMS
                List<Ticket> informeDueñoSms = new List<Ticket>();
                List<Ticket> informeMantenimientoSms = new List<Ticket>();
                List<Ticket> informeDesarrolloSms = new List<Ticket>();
                List<Ticket> informeConsultaSms = new List<Ticket>();

                ParametrosGenerales parametros = db.ParametrosGenerales.SingleOrDefault();
                if (parametros != null)
                {
                    List<int> tiposGposNotificar = new List<int>();
                    tiposGposNotificar.Add((int)BusinessVariables.EnumTiposGrupos.Agente);
                    tiposGposNotificar.Add((int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido);
                    tiposGposNotificar.Add((int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo);
                    tiposGposNotificar.Add((int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales);

                    DateTime fechaConsulta = DateTime.Now;
                    List<TiempoInformeArbol> lstTiempoInforme = (from tia in db.TiempoInformeArbol
                                                                 join t in db.Ticket on tia.IdArbol equals t.IdArbolAcceso
                                                                 where tia.AntesVencimiento == antesVencimiento
                                                                 && t.DentroSla == antesVencimiento
                                                                 select tia).Distinct().ToList();
                    foreach (TiempoInformeArbol tiempoInforme in lstTiempoInforme)
                    {
                        int tiemponotificacionHoras = 0;
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
                                      where System.Data.Objects.EntityFunctions.AddDays((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                            if (tiempoInforme.Horas > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Horas);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddHours((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                            if (tiempoInforme.Minutos > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Minutos);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddMinutes((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                            if (tiempoInforme.Segundos > 0)
                            {
                                tiemponotificacionHoras = decimal.ToInt32(tiempoInforme.Segundos);
                                qry = from q in qry
                                      where System.Data.Objects.EntityFunctions.AddSeconds((DateTime?)q.FechaHoraFinProceso, tiemponotificacionHoras) <= fechaConsulta
                                      select q;
                            }
                        }
                        List<Ticket> tickets = qry.Distinct().ToList();
                        foreach (Ticket t in tickets)
                        {
                            db.LoadProperty(t, "UsuarioLevanto");
                        }
                        switch (tiempoInforme.IdTipoNotificacion)
                        {
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo:
                                switch (tiempoInforme.IdTipoGrupo)
                                {
                                    case (int)BusinessVariables.EnumTiposGrupos.Agente:
                                        informeDueñoCorreo.AddRange(tickets.ToList().Distinct());
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                                        informeMantenimientoCorreo.AddRange(tickets.ToList().Distinct());
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                                        informeDesarrolloCorreo.AddRange(tickets.ToList().Distinct());
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales:
                                        informeConsultaCorreo.AddRange(tickets.ToList().Distinct());
                                        break;
                                }
                                break;
                            case (int)BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms:
                                switch (tiempoInforme.IdTipoGrupo)
                                {
                                    case (int)BusinessVariables.EnumTiposGrupos.Agente:
                                        informeDueñoSms.AddRange(tickets.ToList().Distinct());
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                                        informeMantenimientoSms.AddRange(tickets.ToList().Distinct());
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                                        informeDesarrolloSms.AddRange(tickets.ToList().Distinct());
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales:
                                        informeConsultaSms.AddRange(tickets.ToList().Distinct());
                                        break;
                                }
                                break;
                        }
                    }

                    // Envia notificacion Correo
                    if (informeDueñoCorreo.Any())
                        EnviaNotificacion(informeDueñoCorreo, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo, parametros);
                    if (informeMantenimientoCorreo.Any())
                        EnviaNotificacion(informeMantenimientoCorreo, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo, parametros);
                    if (informeDesarrolloCorreo.Any())
                        EnviaNotificacion(informeDesarrolloCorreo, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo, parametros);
                    if (informeConsultaCorreo.Any())
                        EnviaNotificacion(informeConsultaCorreo, (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Correo, parametros);

                    //Envia Notificacion SMS
                    if (informeDueñoSms.Any())
                        EnviaNotificacion(informeDueñoSms, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms, parametros);
                    if (informeMantenimientoSms.Any())
                        EnviaNotificacion(informeMantenimientoSms, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms, parametros);
                    if (informeDesarrolloSms.Any())
                        EnviaNotificacion(informeDesarrolloSms, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms, parametros);
                    if (informeConsultaSms.Any())
                        EnviaNotificacion(informeConsultaSms, (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales, antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion.Sms, parametros);
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
        }

        private void EnviaNotificacion(List<Ticket> lstInforme, int idTipoGrupo, bool antesVencimiento, BusinessVariables.EnumeradoresKiiniNet.EnumTipoNotificacion tipoNotificacion, ParametrosGenerales parametros)
        {
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
                        if (lastNotificacion.FechaNotificacion.AddMinutes(parametros.FrecuenciaNotificacionMinutos) >= DateTime.Now)
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

        public void EnvioNotificacion()
        {
            try
            {
                ActualizaSla();
                NotificacionesVencimiento(true);
                NotificacionesVencimiento(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void CierraTicketsResueltos()
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                ParametrosGenerales parametros = new BusinessParametros().ObtenerParametrosGenerales();
                if (parametros != null)
                {
                    if (parametros.DiasCierreTicket != null)
                    {
                        DateTime fecha = DateTime.Now.AddDays(-(double)parametros.DiasCierreTicket);
                        var tickets = db.Ticket.Where(w => w.IdEstatusTicket == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Resuelto && w.FechaTermino <= fecha);
                        foreach (Ticket ticket in tickets)
                        {
                            new BusinessAtencionTicket().CambiarEstatus(ticket.Id, (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado, ticket.IdUsuarioSolicito, "Cerrado por sistema");
                        }
                    }
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
        }
    }
}
