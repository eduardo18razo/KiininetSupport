using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Mascaras;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    public class BusinessTipoCampoMascara: IDisposable
    {
        private bool _proxy;
        public BusinessTipoCampoMascara(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        { }

        public List<TipoCampoMascara> ObtenerTipoCampoMascara(bool insertarSeleccion)
        {
            List<TipoCampoMascara> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.TipoCampoMascara.Where(w => w.Habilitado).OrderBy(o => o.Id).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new TipoCampoMascara
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

        public TipoCampoMascara TipoCampoMascaraId(int idTipoCampo)
        {
            TipoCampoMascara result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.TipoCampoMascara.SingleOrDefault(w => w.Id == idTipoCampo);
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

        public List<TipoMascara> ObtenerTipoMascara(bool insertarSeleccion)
        {
            List<TipoMascara> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.TipoMascara.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new TipoMascara
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
