using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessSubGrupoUsuario : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessSubGrupoUsuario(bool proxy = false)
        {
            _proxy = proxy;
        }

        public List<SubGrupoUsuario> ObtenerSubGruposUsuarioByIdGrupo(int idGrupoUsuario)
        {
            List<SubGrupoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.SubGrupoUsuario.Where(w => w.IdGrupoUsuario == idGrupoUsuario).ToList();
                foreach (SubGrupoUsuario subGrupo in result)
                {
                    db.LoadProperty(subGrupo, "SubRol");
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

        public List<HelperSubGurpoUsuario> ObtenerSubGruposUsuario(int idGrupoUsuario, bool insertarSeleccion)
        {
            //TODO: REVISAR  METODO
            List<HelperSubGurpoUsuario> result = new List<HelperSubGurpoUsuario>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var subgpos = from sgu in db.SubGrupoUsuario
                    join sr in db.SubRol on sgu.IdSubRol equals sr.Id
                    where sgu.IdGrupoUsuario == idGrupoUsuario
                    select new {sgu, sr};
                result = subgpos.Select(s => new HelperSubGurpoUsuario {Id = s.sgu.Id, Descripcion = s.sr.Descripcion}).ToList();
                //result = db.SubGrupoUsuario.Where(w => w.IdGrupoUsuario == idGrupoUsuario).Select(s => new HelperSubGurpoUsuario { Id = s.Id, Descripcion = "CAMBIAR ESTA DESCRIPCION" }).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new HelperSubGurpoUsuario { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione });

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

        public SubGrupoUsuario ObtenerSubGrupoUsuario(int idGrupoUsuario, int idSubGrupo)
        {
            SubGrupoUsuario result = new SubGrupoUsuario();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.SubGrupoUsuario.SingleOrDefault(s => s.IdGrupoUsuario == idGrupoUsuario && s.Id == idSubGrupo);
                if (result != null)
                {
                    db.LoadProperty(result, "SubRol");
                    db.LoadProperty(result, "GrupoUsuario");
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
    }
}
