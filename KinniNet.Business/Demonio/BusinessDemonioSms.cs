using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Data.Help;

namespace KinniNet.Core.Demonio
{
    public class BusinessDemonioSms : IDisposable
    {
        private readonly bool _proxy;

        public void Dispose()
        {

        }

        public BusinessDemonioSms(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void InsertarMensaje(int idUsuario, int idTipoLink, int idTelefono, string mensaje)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                if (!db.Usuario.Any(a => a.Id == idUsuario) && !db.TelefonoUsuario.Any(a => a.Id == idTelefono && a.IdUsuario == idUsuario)) return;
                List<SmsService> smsHabilitados = db.SmsService.Where(a => a.IdUsuario == idUsuario && a.IdTipoLink == idTipoLink && a.Habilitado).ToList();
                foreach (SmsService mensajeHabillitados in smsHabilitados)
                {
                    mensajeHabillitados.Habilitado = false;
                    db.SaveChanges();
                }
                SmsService sms = new SmsService
                {
                    IdUsuario = idUsuario,
                    IdTipoLink = idTipoLink,
                    Numero = db.TelefonoUsuario.Single(s => s.Id == idTelefono).Numero,
                    Mensaje = mensaje,
                    Enviado = false,
                    Habilitado = true
                };
                db.SmsService.AddObject(sms);
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

        public void ConfirmaMensaje(int idUsuario, int idTipoLink, int idTelefono)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                TelefonoUsuario telefono = db.TelefonoUsuario.Single(t => t.Id == idTelefono);
                SmsService sms = db.SmsService.SingleOrDefault(s => s.IdUsuario == idUsuario && s.IdTipoLink == idTipoLink && s.Numero == telefono.Numero && s.Habilitado);
                if (sms != null)
                {
                    sms.Enviado = true;
                    sms.Habilitado = false;
                    db.SaveChanges();
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
