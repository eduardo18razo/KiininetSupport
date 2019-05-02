using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessSla : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessSla(bool proxy = false)
        {
            _proxy = proxy;
        }
        public List<Sla> ObtenerSla(bool insertarSeleccion)
        {
            List<Sla> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Sla.Where(w => w.Habilitado).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Sla
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                        });
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

        public Sla ObtenerSla(int idSla)
        {
            Sla result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Sla.SingleOrDefault(w => w.Id == idSla);
                if (result != null)
                {
                    db.LoadProperty(result, "SlaDetalle");
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

        public void Guardar(Sla sla)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                sla.Habilitado = true;
                if (sla.Id == 0)
                    db.Sla.AddObject(sla);
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
