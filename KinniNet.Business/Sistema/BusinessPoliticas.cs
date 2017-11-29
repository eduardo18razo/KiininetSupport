using KiiniNet.Entities.Parametros;
using System;
using System.Collections.Generic;
using System.Linq;
using KinniNet.Data.Help;


namespace KinniNet.Core.Sistema
{
    public class BusinessPoliticas : IDisposable
    {
        private readonly bool _proxy;
        public BusinessPoliticas(bool proxy = false)
        {
            _proxy = proxy;
        }
        public void Dispose()
        {

        }
        public List<EstatusAsignacionSubRolGeneralDefault> ObtenerEstatusAsignacionSubRolGeneralDefault()
        {
            List<EstatusAsignacionSubRolGeneralDefault> result = new List<EstatusAsignacionSubRolGeneralDefault>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusAsignacionSubRolGeneralDefault.ToList();
                if (result != null)
                {
                    foreach (EstatusAsignacionSubRolGeneralDefault data in result)
                    {
                        db.LoadProperty(data, "Rol");
                        db.LoadProperty(data, "SubRol");
                        db.LoadProperty(data, "EstatusAsignacionActual");
                        db.LoadProperty(data, "EstatusAsignacionAccion");

                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<EstatusTicketSubRolGeneralDefault> ObtenerEstatusTicketSubRolGeneralDefault()
        {
            List<EstatusTicketSubRolGeneralDefault> result = new List<EstatusTicketSubRolGeneralDefault>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusTicketSubRolGeneralDefault.ToList();
                if (result != null)
                {
                    foreach (EstatusTicketSubRolGeneralDefault data in result)
                    {
                        db.LoadProperty(data, "EstatusTicketActual");
                        db.LoadProperty(data, "EstatusTicketAccion");
                        db.LoadProperty(data, "RolSolicita");
                        db.LoadProperty(data, "SubSolicita");
                        db.LoadProperty(data, "RolPertenece");
                        db.LoadProperty(data, "SubRolPertenece");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<EstatusAsignacionSubRolGeneral> ObtenerEstatusAsignacionSubRolGeneral()
        {
            List<EstatusAsignacionSubRolGeneral> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusAsignacionSubRolGeneral.ToList();
                foreach (EstatusAsignacionSubRolGeneral data in result)
                {
                    db.LoadProperty(data, "GrupoUsuario");
                    db.LoadProperty(data, "Rol");
                    db.LoadProperty(data, "SubRol");
                    db.LoadProperty(data, "EstatusAsignacionActual");
                    db.LoadProperty(data, "EstatusAsignacionAccion");

                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public List<EstatusTicketSubRolGeneral> ObtenerEstatusTicketSubRolGeneral()
        {
            List<EstatusTicketSubRolGeneral> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusTicketSubRolGeneral.ToList();
                    foreach (EstatusTicketSubRolGeneral data in result)
                    {
                        db.LoadProperty(data, "GrupoUsuario");
                        db.LoadProperty(data, "EstatusTicketActual");
                        db.LoadProperty(data, "EstatusTicketAccion");
                        db.LoadProperty(data, "RolSolicita");
                        db.LoadProperty(data, "SubSolicita");
                        db.LoadProperty(data, "RolPertenece");
                        db.LoadProperty(data, "SubRolPertenece");
                    }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }
        public void HabilitarEstatusAsignacionSubRolGeneralDefault(int idAsignacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                EstatusAsignacionSubRolGeneralDefault inf = db.EstatusAsignacionSubRolGeneralDefault.SingleOrDefault(w => w.Id == idAsignacion);
                if (inf != null) inf.Habilitado = habilitado;
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

        public void HabilitarEstatusTicketSubRolGeneralDefault(int idAsignacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                EstatusTicketSubRolGeneralDefault inf = db.EstatusTicketSubRolGeneralDefault.SingleOrDefault(w => w.Id == idAsignacion);
                if (inf != null) inf.Habilitado = habilitado;
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

        public void HabilitarEstatusAsignacionSubRolGeneral(int idAsignacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                EstatusAsignacionSubRolGeneral inf = db.EstatusAsignacionSubRolGeneral.SingleOrDefault(w => w.Id == idAsignacion);
                if (inf != null) inf.Habilitado = habilitado;
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

        public void HabilitarEstatusTicketSubRolGeneral(int idAsignacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                EstatusTicketSubRolGeneral inf = db.EstatusTicketSubRolGeneral.SingleOrDefault(w => w.Id == idAsignacion);
                if (inf != null) inf.Habilitado = habilitado;
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
    }


}
