using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    public class BusinessImpactoUrgencia : IDisposable
    {
        private bool _proxy;
        public BusinessImpactoUrgencia(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        { }

        public List<Prioridad> ObtenerPrioridad(bool insertarSeleccion)
        {
            List<Prioridad> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Prioridad.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Prioridad
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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

        public List<Urgencia> ObtenerUrgencia(bool insertarSeleccion)
        {
            List<Urgencia> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Urgencia.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Urgencia
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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

        public Impacto ObtenerImpactoById(int idImpacto)
        {
            Impacto result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Impacto.SingleOrDefault(w => w.Id == idImpacto);
                if (result != null)
                {
                    db.LoadProperty(result, "Prioridad");
                    db.LoadProperty(result, "Urgencia");
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

        public Impacto ObtenerPrioridadByImpactoUrgencia(int idPrioridad, int idUrgencia)
        {
            Impacto result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Impacto.SingleOrDefault(w => w.IdPrioridad == idPrioridad && w.IdUrgencia == idUrgencia);
                if (result != null)
                {
                    db.LoadProperty(result, "Prioridad");
                    db.LoadProperty(result, "Urgencia");
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

        public List<Impacto> ObtenerAll(bool insertarSeleccion)
        {
            List<Impacto> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Impacto.OrderBy(o => o.Descripcion).ToList();
                foreach (Impacto impacto in result)
                {
                    db.LoadProperty(impacto, "Prioridad");
                    db.LoadProperty(impacto, "Urgencia");
                }
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Impacto
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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

    }
}
