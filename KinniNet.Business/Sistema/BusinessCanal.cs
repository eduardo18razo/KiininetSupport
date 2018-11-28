using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    /// <summary>
    /// Capa de negocio Canal de apertura ticket
    /// </summary>
    public class BusinessCanal: IDisposable
    {
        private readonly bool _proxy;

        /// <summary>
        /// Inicializa Clase
        /// </summary>
        /// <param name="proxy">Modelo de base conectado</param>
        public BusinessCanal(bool proxy = false)
        {
            _proxy = proxy;
        }
        /// <summary>
        /// Libera La instancia de la clase
        /// </summary>
        public void Dispose()
        {

        }
        /// <summary>
        /// Obtiene todos los canales Habilitados
        /// </summary>
        /// <param name="insertarSeleccion">Insica si Se inserta un indice adicional.</param>
        /// <returns>Lista de Canales</returns>
        public List<Canal> ObtenerCanales(bool insertarSeleccion)
        {
            List<Canal> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Canal.Where(w => w.Habilitado).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Canal
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
        /// <summary>
        /// Obtiene todos los canales Habilitados y Deshabilitados
        /// </summary>
        /// <param name="insertarSeleccion">Insica si Se inserta un indice adicional.</param>
        /// <returns>Lista de Canales</returns>
        public List<Canal> ObtenerCanalesAll(bool insertarSeleccion)
        {
            List<Canal> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Canal.ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Canal
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
        /// <summary>
        /// Obtiene Canal por Id 
        /// </summary>
        /// <param name="idCanal"></param>
        /// <returns></returns>
        public Canal ObtenerCanalById(int idCanal)
        {
            Canal result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Canal.SingleOrDefault(w => w.Id == idCanal);
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
