using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    public class BusinessTipoEncuesta: IDisposable
    {
        private bool _proxy;
        public BusinessTipoEncuesta(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        { }
        public List<TipoEncuesta> ObtenerTiposEncuesta(bool insertarSeleccion)
        {
            List<TipoEncuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.TipoEncuesta.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new TipoEncuesta
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

        public TipoEncuesta TipoEncuestaId(int idTipoEncuesta)
        {
            TipoEncuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.TipoEncuesta.SingleOrDefault(w => w.Id == idTipoEncuesta && w.Habilitado);
                db.LoadProperty(result, "RespuestaTipoEncuesta");
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
