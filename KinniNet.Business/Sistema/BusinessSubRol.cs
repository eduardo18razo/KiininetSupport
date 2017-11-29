using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    public class BusinessSubRol : IDisposable
    {
        private bool _proxy;
        public BusinessSubRol(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        { }
        public List<SubRol> ObtenerSubRolesByTipoGrupo(int idTipoGrupo, bool insertarSeleccion)
        {
            List<SubRol> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.RolTipoGrupo.Where(w => w.IdTipoGrupo == idTipoGrupo && w.Rol.Habilitado)
                        .OrderBy(o => o.Rol.Descripcion)
                        .SelectMany(s => s.Rol.SubRol).Distinct()
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new SubRol
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

        public SubRol ObtenerSubRolById(int idSubRol)
        {
            SubRol result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.SubRol.SingleOrDefault(w => w.Id == idSubRol && w.Habilitado);
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

        public List<SubRol> ObtenerSubRolesByGrupoUsuarioRol(int idGrupoUsuario, int idRol, bool insertarSeleccion)
        {
            List<SubRol> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.SubGrupoUsuario.Where(w => w.SubRol.Rol.Id == idRol && w.IdGrupoUsuario == idGrupoUsuario).Select(s => s.SubRol).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new SubRol
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

        public List<SubRol> ObtenerTipoSubRol(int idTipoGrupo, bool insertarSeleccion)
        {
            List<SubRol> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result =
                    db.SubRol.Where(w => w.IdRol == idTipoGrupo && w.Habilitado)
                        .OrderBy(o => o.Descripcion)
                        .ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new SubRol
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

        public List<SubRolEscalacionPermitida> ObtenerEscalacion(int idSubRol, int idEstatusAsignacion, int? nivelActual)
        {
            List<SubRolEscalacionPermitida> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                if (nivelActual != null)
                {
                    switch (nivelActual +1 )
                    {
                        case 1:
                            idSubRol = (int)BusinessVariables.EnumSubRoles.Supervisor;
                            break;
                        case 2:
                            idSubRol = (int)BusinessVariables.EnumSubRoles.PrimererNivel;
                            break;
                        case 3:
                            idSubRol = (int)BusinessVariables.EnumSubRoles.SegundoNivel;
                            break;
                        case 4:
                            idSubRol = (int)BusinessVariables.EnumSubRoles.TercerNivel;
                            break;
                        case 5:

                            idSubRol = (int)BusinessVariables.EnumSubRoles.CuartoNivel;
                            break;
                    }
                }
                result = db.SubRolEscalacionPermitida.Where(w => w.IdSubRol == idSubRol && w.IdEstatusAsignacion == idEstatusAsignacion && w.Habilitado).ToList();
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

        public List<SubRolEscalacionPermitida> ObtenerSubRolEscalacionPermitida()
        {
            List<SubRolEscalacionPermitida> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.SubRolEscalacionPermitida.ToList();
                foreach (SubRolEscalacionPermitida data in result)
                {
                    db.LoadProperty(data, "SubRol");
                    db.LoadProperty(data, "SubRolPermitido");
                    db.LoadProperty(data, "EstatusAsignacion");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally { db.Dispose(); }
            return result;
        }

        public void HabilitarPoliticaEscalacion(int idEscalacion, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                SubRolEscalacionPermitida inf = db.SubRolEscalacionPermitida.SingleOrDefault(w => w.Id == idEscalacion);
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
