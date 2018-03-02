using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    public class BusinessEstatus : IDisposable
    {
        private readonly bool _proxy;
        public BusinessEstatus(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        {

        }

        public List<EstatusTicket> ObtenerEstatusTicket(bool insertarSeleccion)
        {
            List<EstatusTicket> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusTicket.Where(w => w.Habilitado).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new EstatusTicket
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

        public List<EstatusTicket> ObtenerEstatusTicketUsuario(int idUsuario, int idGrupo, int idEstatusActual, bool esPropietario, int? idSubRol, bool insertarSeleccion)
        {
            List<EstatusTicket> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new List<EstatusTicket>();
                GrupoUsuario gpo = db.GrupoUsuario.SingleOrDefault(s => s.Id == idGrupo);
                var qry = from etsrg in db.EstatusTicketSubRolGeneral
                          join et in db.EstatusTicket on etsrg.IdEstatusTicketAccion equals et.Id
                          join ug in db.UsuarioGrupo on etsrg.IdGrupoUsuario equals ug.IdGrupoUsuario
                          where ug.IdUsuario == idUsuario && etsrg.IdEstatusTicketActual == idEstatusActual
                                && etsrg.Propietario == esPropietario
                                && etsrg.Habilitado
                          select new { etsrg, et, ug };
                if (idSubRol != null)
                    qry = from q in qry
                          where q.etsrg.IdSubRolPertenece == idSubRol
                          select q;
                if (idGrupo != 0)
                    qry = from q in qry
                          where q.etsrg.IdGrupoUsuario == idGrupo
                          select q;
                else
                    qry = from q in qry
                          where q.ug.GrupoUsuario.IdTipoGrupo != (int)BusinessVariables.EnumTiposGrupos.Agente
                          select q;

                result.AddRange((from q in qry select q.et).Distinct().ToList());
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new EstatusTicket
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

        public List<EstatusAsignacion> ObtenerEstatusAsignacion(bool insertarSeleccion)
        {
            List<EstatusAsignacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.EstatusAsignacion.Where(w => w.Habilitado).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new EstatusAsignacion
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

        public List<EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int estatusAsignacionActual, bool esPropietario, bool insertarSeleccion)
        {
            List<EstatusAsignacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = (from easg in db.EstatusAsignacionSubRolGeneral
                          join ea in db.EstatusAsignacion on easg.IdEstatusAsignacionActual equals ea.Id
                          join ea1 in db.EstatusAsignacion on easg.IdEstatusAsignacionAccion equals ea1.Id
                          join ug in db.UsuarioGrupo on easg.IdGrupoUsuario equals ug.IdGrupoUsuario
                          where ug.IdUsuario == idUsuario && easg.IdSubRol == ug.SubGrupoUsuario.IdSubRol &&
                                easg.IdGrupoUsuario == idGrupo &&
                                easg.TieneSupervisor == ug.GrupoUsuario.TieneSupervisor &&
                                easg.IdEstatusAsignacionActual == estatusAsignacionActual && easg.Habilitado && easg.Propietario == esPropietario
                          select ea1).Distinct().ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new EstatusAsignacion
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

        public bool HasComentarioObligatorio(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, int estatusAsignar, bool esPropietario)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                idSubRol = idSubRol <= 2 ? 3 : idSubRol;
                result = (from easg in db.EstatusAsignacionSubRolGeneral
                          join ea in db.EstatusAsignacion on easg.IdEstatusAsignacionActual equals ea.Id
                          join ea1 in db.EstatusAsignacion on easg.IdEstatusAsignacionAccion equals ea1.Id
                          join ug in db.UsuarioGrupo on easg.IdGrupoUsuario equals ug.IdGrupoUsuario
                          where ug.IdUsuario == idUsuario && easg.IdSubRol == idSubRol && easg.IdGrupoUsuario == idGrupo &&
                                easg.IdEstatusAsignacionActual == estatusAsignacionActual && easg.IdEstatusAsignacionAccion == estatusAsignar && easg.Propietario == esPropietario
                          select easg.ComentarioObligado).First();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
    }
}
