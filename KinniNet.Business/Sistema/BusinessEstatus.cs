using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Tickets;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;
using Telerik.Web.UI.Export;

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

        public List<EstatusTicket> ObtenerEstatusTicketUsuarioPublico(int idTicket, int idGrupo, int idEstatusActual, bool esPropietario, int? idSubRol, bool insertarSeleccion)
        {
            List<EstatusTicket> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new List<EstatusTicket>();
                var gpoTicket = db.TicketGrupoUsuario.SingleOrDefault(s => s.IdTicket == idTicket && s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario);
                if (gpoTicket != null)
                {
                    if (gpoTicket.IdGrupoUsuario != idGrupo)
                        throw new Exception("Datos incorrectos consulte a su administrador");
                    var qry = from etsrg in db.EstatusTicketSubRolGeneral
                              join et in db.EstatusTicket on etsrg.IdEstatusTicketAccion equals et.Id
                              where etsrg.IdEstatusTicketActual == idEstatusActual
                                    && etsrg.Propietario == esPropietario
                                    && etsrg.Habilitado
                              select new { etsrg, et };
                    if (idSubRol != null)
                        qry = from q in qry
                              where q.etsrg.IdSubRolPertenece == idSubRol
                              select q;
                    if (idGrupo != 0)
                        qry = from q in qry
                              where q.etsrg.IdGrupoUsuario == idGrupo
                              select q;

                    result.AddRange((from q in qry select q.et).Distinct().ToList());
                }
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

        public List<EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int estatusAsignacionActual, bool esPropietario, int subRolActual, bool insertarSeleccion)
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
                                easg.TieneSupervisor == ug.GrupoUsuario.TieneSupervisor && easg.IdSubRol == subRolActual &&
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
            bool result = false;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                GrupoUsuario gpo = db.GrupoUsuario.SingleOrDefault(s => s.Id == idGrupo);
                if (gpo != null)
                {
                    if (gpo.TieneSupervisor)
                        idSubRol = idSubRol <= 2 ? (int)BusinessVariables.EnumSubRoles.Supervisor : idSubRol;
                    else
                        idSubRol = idSubRol <= 2 ? (int)BusinessVariables.EnumSubRoles.PrimererNivel : idSubRol;
                    result = (from easg in db.EstatusAsignacionSubRolGeneral
                              join ea in db.EstatusAsignacion on easg.IdEstatusAsignacionActual equals ea.Id
                              join ea1 in db.EstatusAsignacion on easg.IdEstatusAsignacionAccion equals ea1.Id
                              join ug in db.UsuarioGrupo on easg.IdGrupoUsuario equals ug.IdGrupoUsuario
                              where ug.IdUsuario == idUsuario && easg.IdSubRol == idSubRol && easg.IdGrupoUsuario == idGrupo &&
                                  easg.IdEstatusAsignacionActual == estatusAsignacionActual &&
                                  easg.IdEstatusAsignacionAccion == estatusAsignar && easg.Propietario == esPropietario
                              select easg.ComentarioObligado).First();
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
            return result;
        }

        public bool HasCambioEstatusComentarioObligatorio(int? idUsuario, int idTicket, int idEstatusTickteAsignar, bool esPropietario, bool publico)
        {
            bool result = false;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket ticket = db.Ticket.SingleOrDefault(s => s.Id == idTicket);
                if (ticket != null)
                {
                    if (idUsuario == null)
                        idUsuario = ticket.IdUsuarioSolicito;
                    db.LoadProperty(ticket, "TicketGrupoUsuario");
                    foreach (TicketGrupoUsuario tgu in ticket.TicketGrupoUsuario)
                    {
                        db.LoadProperty(tgu, "GrupoUsuario");
                    }
                    GrupoUsuario gpo;
                    if (idUsuario == ticket.IdUsuarioSolicito)
                        gpo = ticket.TicketGrupoUsuario.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.GrupoUsuario).SingleOrDefault();
                    else
                        gpo = ticket.TicketGrupoUsuario.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.GrupoUsuario).FirstOrDefault();
                    if (gpo != null)
                    {
                        int? subRolPertenece = UtilsTicket.ObtenerSubRolAsignacionByIdNivel(ticket.IdNivelTicket);
                        var qry = from etsrg in db.EstatusTicketSubRolGeneral
                                  where etsrg.IdGrupoUsuario == gpo.Id
                                        && etsrg.TieneSupervisor == gpo.TieneSupervisor
                                        && etsrg.IdEstatusTicketActual == ticket.IdEstatusTicket
                                        && etsrg.IdEstatusTicketAccion == idEstatusTickteAsignar
                                  select etsrg;
                        if (!publico && idUsuario == ticket.IdUsuarioSolicito)
                            qry = from q in qry
                                  where q.IdRolPertenece == (int)BusinessVariables.EnumRoles.Usuario
                                  select q;
                        else
                            qry = from q in qry
                                  where q.IdSubRolPertenece == subRolPertenece
                                  select q;
                        var res = (from q in qry
                                   select q.ComentarioObligado).FirstOrDefault();
                        if (res == false)
                            result = false;
                        else
                            result = true;
                    }
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
            return result;
        }
    }
}
