using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using KinniNet.Core.Operacion;
using KinniNet.Data.Help;

namespace KinniNet.Core.Sistema
{
    public class BusinessTipoArbolAcceso : IDisposable
    {
        private bool _proxy;
        public BusinessTipoArbolAcceso(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        { }

        public List<TipoArbolAcceso> ObtenerTiposArbolAcceso(bool insertarSeleccion)
        {
            List<TipoArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.TipoArbolAcceso.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new TipoArbolAcceso
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

        public List<TipoArbolAcceso> ObtenerTiposArbolAccesoByGruposTercero(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, bool insertarSeleccion)
        {
            List<TipoArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                List<int> lstGpos = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                lstGpos.AddRange(usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdGrupoUsuario).ToList());
                lstGpos.AddRange(usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket).Select(s => s.IdGrupoUsuario).ToList());
                lstGpos.AddRange(usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList());
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                var qry = from aa in db.ArbolAcceso
                    join taa in db.TipoArbolAcceso on aa.IdTipoArbolAcceso equals taa.Id
                    join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                    join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                    join ug in db.UsuarioGrupo on new {gpo = guia.IdGrupoUsuario, sgpo = guia.IdSubGrupoUsuario} equals
                        new {gpo = ug.IdGrupoUsuario, sgpo = ug.IdSubGrupoUsuario}
                    where lstGpos.Contains(guia.IdGrupoUsuario) && aa.IdArea == idArea
                    select new {taa, guia};
                if (lstsubGpos.Any())
                    qry = from q in qry
                        where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                        select q;
                result = qry.Where(w => w.taa.Habilitado).Select(s => s.taa).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new TipoArbolAcceso
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

        public List<TipoArbolAcceso> ObtenerTiposArbolAccesoByGrupos(List<int> grupos, bool insertarSeleccion)
        {
            List<TipoArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join taa in db.TipoArbolAcceso on t.IdTipoArbolAcceso equals taa.Id
                          where t.EncuestaRespondida
                          select new { t, e, taa, tgu, };
                if (grupos.Any())
                    qry = from q in qry
                          where grupos.Contains(q.tgu.IdGrupoUsuario)
                          select q;
                result = qry.Select(s => s.taa).Distinct().ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new TipoArbolAcceso
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
