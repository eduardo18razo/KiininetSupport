using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessArbolAcceso : IDisposable
    {
        private readonly bool _proxy;
        public void Dispose()
        {

        }
        public BusinessArbolAcceso(bool proxy = false)
        {
            _proxy = proxy;
        }
        #region ticket tercero

        public List<HelperArbolAcceso> ObtenerOpcionesPermitidas(int idUsuarioSolicita, int idUsuarioLevanta, int? idArea, bool insertarSeleccion)
        {
            List<HelperArbolAcceso> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                if (usuarioLevanta == null || usuarioSolicita == null)
                    return null;
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          select new { aa, guia };
                if (idArea != null && idArea > 0)
                    qry = from q in qry
                          where q.aa.IdArea == idArea
                          select q;
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (!lstGposLevanta.Any() && lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();

                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                if (arbolesPermitidos.Count > 0)
                {
                    result = new List<HelperArbolAcceso>();
                    foreach (ArbolAcceso arbol in db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id)).Distinct().ToList())
                    {
                        db.LoadProperty(arbol, "Area");
                        HelperArbolAcceso addArbol = new HelperArbolAcceso();
                        addArbol.Id = arbol.Id;
                        addArbol.DescripcionTipificacion = ObtenerTipificacion(arbol.Id);
                        addArbol.Ruta = ObtenerRutaCompleta(arbol.Id);
                        addArbol.Categoria = arbol.Area.Descripcion;
                        result.Add(addArbol);
                    }
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
        public bool LevantaTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                //List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ContactCenter && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                var qry = db.ArbolAcceso.Where(w => w.IdTipoUsuario == idTipoUsuario && w.IdTipoArbolAcceso == idTipoArbol && w.IdNivel1 == nivel1);
                qry = nivel2.HasValue ? qry.Where(w => w.IdNivel2 == nivel2) : qry.Where(w => w.IdNivel2 == null);
                qry = nivel3.HasValue ? qry.Where(w => w.IdNivel3 == nivel3) : qry.Where(w => w.IdNivel3 == null);
                qry = nivel4.HasValue ? qry.Where(w => w.IdNivel4 == nivel4) : qry.Where(w => w.IdNivel4 == null);
                qry = nivel5.HasValue ? qry.Where(w => w.IdNivel5 == nivel5) : qry.Where(w => w.IdNivel5 == null);
                qry = nivel6.HasValue ? qry.Where(w => w.IdNivel6 == nivel6) : qry.Where(w => w.IdNivel6 == null);
                qry = nivel7.HasValue ? qry.Where(w => w.IdNivel7 == nivel7) : qry.Where(w => w.IdNivel7 == null);
                qry = from q in qry
                      join iaa in db.InventarioArbolAcceso on q.Id equals iaa.IdArbolAcceso
                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                      where lstGposLevanta.Contains(guia.IdGrupoUsuario) && guia.GrupoUsuario.LevantaTicket
                      select q;
                result = qry.Any();

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
        public bool RecadoTicketTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                var qry = db.ArbolAcceso.Where(w => w.IdTipoUsuario == idTipoUsuario && w.IdTipoArbolAcceso == idTipoArbol && w.IdNivel1 == nivel1);
                qry = nivel2.HasValue ? qry.Where(w => w.IdNivel2 == nivel2) : qry.Where(w => w.IdNivel2 == null);
                qry = nivel3.HasValue ? qry.Where(w => w.IdNivel3 == nivel3) : qry.Where(w => w.IdNivel3 == null);
                qry = nivel4.HasValue ? qry.Where(w => w.IdNivel4 == nivel4) : qry.Where(w => w.IdNivel4 == null);
                qry = nivel5.HasValue ? qry.Where(w => w.IdNivel5 == nivel5) : qry.Where(w => w.IdNivel5 == null);
                qry = nivel6.HasValue ? qry.Where(w => w.IdNivel6 == nivel6) : qry.Where(w => w.IdNivel6 == null);
                qry = nivel7.HasValue ? qry.Where(w => w.IdNivel7 == nivel7) : qry.Where(w => w.IdNivel7 == null);
                qry = from q in qry
                      join iaa in db.InventarioArbolAcceso on q.Id equals iaa.IdArbolAcceso
                      join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                      where lstGposRecado.Contains(guia.IdGrupoUsuario) && guia.GrupoUsuario.RecadoTicket
                      select q;
                result = qry.Any();
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
        public List<Nivel1> ObtenerNivel1ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, bool insertarSeleccion)
        {
            List<Nivel1> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (!lstGposLevanta.Any() && lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();

                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id)).SelectMany(nivel => db.Nivel1.Where(w => w.Id == nivel.IdNivel1)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel1 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel2> ObtenerNivel2ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel1, bool insertarSeleccion)
        {
            List<Nivel2> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n2 in db.Nivel2 on aa.IdNivel2 equals n2.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();
                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id) && w.IdNivel1 == idNivel1).SelectMany(nivel => db.Nivel2.Where(w => w.Id == nivel.IdNivel2)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel2 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel3> ObtenerNivel3ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel2, bool insertarSeleccion)
        {
            List<Nivel3> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n3 in db.Nivel3 on aa.IdNivel3 equals n3.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();
                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id) && w.IdNivel2 == idNivel2).SelectMany(nivel => db.Nivel3.Where(w => w.Id == nivel.IdNivel3)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel3 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel4> ObtenerNivel4ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel3, bool insertarSeleccion)
        {
            List<Nivel4> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n4 in db.Nivel4 on aa.IdNivel4 equals n4.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();
                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id) && w.IdNivel3 == idNivel3).SelectMany(nivel => db.Nivel4.Where(w => w.Id == nivel.IdNivel4)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel4 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel5> ObtenerNivel5ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel4, bool insertarSeleccion)
        {
            List<Nivel5> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n5 in db.Nivel5 on aa.IdNivel5 equals n5.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();
                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id) && w.IdNivel4 == idNivel4).SelectMany(nivel => db.Nivel5.Where(w => w.Id == nivel.IdNivel5)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel5 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel6> ObtenerNivel6ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel5, bool insertarSeleccion)
        {
            List<Nivel6> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n6 in db.Nivel6 on aa.IdNivel6 equals n6.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();
                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id) && w.IdNivel5 == idNivel5).SelectMany(nivel => db.Nivel6.Where(w => w.Id == nivel.IdNivel6)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel6 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel7> ObtenerNivel7ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel6, bool insertarSeleccion)
        {
            List<Nivel7> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Usuario usuarioLevanta = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioLevanta);
                Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario(idUsuarioSolicita);
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposLevanta = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || (w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.LevantaTicket)).Select(s => s.IdGrupoUsuario).ToList();
                List<int> lstGposRecado = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AgenteUniversal && w.GrupoUsuario.RecadoTicket).Select(s => s.IdGrupoUsuario).ToList();
                List<int?> lstsubGpos = usuarioLevanta.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente).Select(s => s.IdSubGrupoUsuario).ToList();
                lstsubGpos.RemoveAll(r => !r.HasValue);
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<int> qrySolicita = (from aa in db.ArbolAcceso
                                         join n7 in db.Nivel7 on aa.IdNivel7 equals n7.Id
                                         join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                                         where lstGposSolicita.Contains(guia.IdGrupoUsuario)
                                               && aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                                         select new { aa.Id }).Select(s => s.Id).Distinct().ToList();
                var qry = from aa in db.ArbolAcceso
                          join n1 in db.Nivel1 on aa.IdNivel1 equals n1.Id
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.IdArea == idArea && aa.IdTipoArbolAcceso == idTipoArbolAcceso
                          select new { aa, guia };
                if (lstGposLevanta.Any())
                    qry = from q in qry
                          where lstGposLevanta.Contains(q.guia.IdGrupoUsuario)
                          select q;
                if (lstsubGpos.Any())
                    qry = from q in qry
                          where lstsubGpos.Contains(q.guia.IdSubGrupoUsuario)
                          select q;
                if (lstGposRecado.Any())
                    qry = from q in qry
                          where lstGposRecado.Contains(q.guia.IdGrupoUsuario)
                          select q;
                List<int> qryLevanta = (from q in qry
                                        select new { q.aa.Id }).Distinct().Select(s => s.Id).ToList();
                List<int> arbolesPermitidos = qrySolicita.Where(qryLevanta.Contains).ToList();
                result = db.ArbolAcceso.Where(w => arbolesPermitidos.Contains(w.Id) && w.IdNivel6 == idNivel6).SelectMany(nivel => db.Nivel7.Where(w => w.Id == nivel.IdNivel7)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel7 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public bool EsNodoTerminalByGrupos(int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var qry = db.ArbolAcceso.Where(w => w.IdTipoUsuario == idTipoUsuario && w.IdTipoArbolAcceso == idTipoArbol && w.IdNivel1 == nivel1);
                qry = nivel2.HasValue ? qry.Where(w => w.IdNivel2 == nivel2) : qry.Where(w => w.IdNivel2 == null);
                qry = nivel3.HasValue ? qry.Where(w => w.IdNivel3 == nivel3) : qry.Where(w => w.IdNivel3 == null);
                qry = nivel4.HasValue ? qry.Where(w => w.IdNivel4 == nivel4) : qry.Where(w => w.IdNivel4 == null);
                qry = nivel5.HasValue ? qry.Where(w => w.IdNivel5 == nivel5) : qry.Where(w => w.IdNivel5 == null);
                qry = nivel6.HasValue ? qry.Where(w => w.IdNivel6 == nivel6) : qry.Where(w => w.IdNivel6 == null);
                qry = nivel7.HasValue ? qry.Where(w => w.IdNivel7 == nivel7) : qry.Where(w => w.IdNivel7 == null);
                result = qry.Any(a => a.EsTerminal);
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Datos");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
        #endregion ticket tercero

        #region Flujo normal
        public List<Nivel1> ObtenerNivel1(int idArea, int idTipoArbol, int idTipoUsuario, bool insertarSeleccion)
        {
            List<Nivel1> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.Habilitado).SelectMany(nivel => db.Nivel1.Where(w => w.Id == nivel.IdNivel1)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel1.Where(w => w.Id == nivel.IdNivel1)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel1 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel2> ObtenerNivel2(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel1, bool insertarSeleccion)
        {
            List<Nivel2> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel1 == idNivel1 && w.Habilitado).SelectMany(nivel => db.Nivel2.Where(w => w.Id == nivel.IdNivel2)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel1 == idNivel1 && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel2.Where(w => w.Id == nivel.IdNivel2)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel2 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel3> ObtenerNivel3(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel2, bool insertarSeleccion)
        {
            List<Nivel3> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel2 == idNivel2 && w.Habilitado).SelectMany(nivel => db.Nivel3.Where(w => w.Id == nivel.IdNivel3)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel2 == idNivel2 && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel3.Where(w => w.Id == nivel.IdNivel3)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel3 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel4> ObtenerNivel4(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel3, bool insertarSeleccion)
        {
            List<Nivel4> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel3 == idNivel3 && w.Habilitado).SelectMany(nivel => db.Nivel4.Where(w => w.Id == nivel.IdNivel4)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel3 == idNivel3 && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel4.Where(w => w.Id == nivel.IdNivel4)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel4 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel5> ObtenerNivel5(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel4, bool insertarSeleccion)
        {
            List<Nivel5> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel4 == idNivel4 && w.Habilitado).SelectMany(nivel => db.Nivel5.Where(w => w.Id == nivel.IdNivel5)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel4 == idNivel4 && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel5.Where(w => w.Id == nivel.IdNivel5)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel5 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel6> ObtenerNivel6(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel5, bool insertarSeleccion)
        {
            List<Nivel6> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel5 == idNivel5 && w.Habilitado).SelectMany(nivel => db.Nivel6.Where(w => w.Id == nivel.IdNivel6)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel5 == idNivel5 && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel6.Where(w => w.Id == nivel.IdNivel6)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel6 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public List<Nivel7> ObtenerNivel7(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel6, bool insertarSeleccion)
        {
            List<Nivel7> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //result = db.ArbolAcceso.Where(w => w.IdTipoArbolAcceso == idTipoArbol && w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel6 == idNivel6 && w.Habilitado).SelectMany(nivel => db.Nivel7.Where(w => w.Id == nivel.IdNivel7)).Distinct().OrderBy(o => o.Descripcion).ToList();
                result = db.ArbolAcceso.Where(w => w.IdArea == idArea && w.IdTipoUsuario == idTipoUsuario && w.IdNivel6 == idNivel6 && !w.EsTerminal && w.Habilitado).SelectMany(nivel => db.Nivel7.Where(w => w.Id == nivel.IdNivel7)).Distinct().OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Nivel7 { Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione, Descripcion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione, Habilitado = BusinessVariables.ComboBoxCatalogo.Habilitado });
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
        public bool EsNodoTerminal(int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            bool result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var qry = db.ArbolAcceso.Where(w => w.IdTipoUsuario == idTipoUsuario && w.IdTipoArbolAcceso == idTipoArbol && w.IdNivel1 == nivel1);
                qry = nivel2.HasValue ? qry.Where(w => w.IdNivel2 == nivel2) : qry.Where(w => w.IdNivel2 == null);
                qry = nivel3.HasValue ? qry.Where(w => w.IdNivel3 == nivel3) : qry.Where(w => w.IdNivel3 == null);
                qry = nivel4.HasValue ? qry.Where(w => w.IdNivel4 == nivel4) : qry.Where(w => w.IdNivel4 == null);
                qry = nivel5.HasValue ? qry.Where(w => w.IdNivel5 == nivel5) : qry.Where(w => w.IdNivel5 == null);
                qry = nivel6.HasValue ? qry.Where(w => w.IdNivel6 == nivel6) : qry.Where(w => w.IdNivel6 == null);
                qry = nivel7.HasValue ? qry.Where(w => w.IdNivel7 == nivel7) : qry.Where(w => w.IdNivel7 == null);
                result = qry.Any(a => a.EsTerminal);
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Datos");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }
        public void GuardarArbol(ArbolAcceso arbol)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                //TODO: Cambiar habilitado por el embebido
                arbol.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                arbol.FechaVisita = arbol.FechaAlta;
                arbol.Habilitado = true;
                List<GrupoUsuario> lstGrupoUsuario = null;
                if (arbol.Nivel1 != null)
                {
                    arbol.Nivel1.Descripcion = arbol.Nivel1.Descripcion.Trim();
                    arbol.Nivel1.Habilitado = arbol.Nivel1.Habilitado;
                }
                if (arbol.Nivel2 != null)
                {
                    arbol.Nivel2.Descripcion = arbol.Nivel2.Descripcion.Trim();
                    arbol.Nivel2.Habilitado = arbol.Nivel2.Habilitado;
                }
                if (arbol.Nivel3 != null)
                {
                    arbol.Nivel3.Descripcion = arbol.Nivel3.Descripcion.Trim();
                    arbol.Nivel3.Habilitado = arbol.Nivel3.Habilitado;
                }
                if (arbol.Nivel4 != null)
                {
                    arbol.Nivel4.Descripcion = arbol.Nivel4.Descripcion.Trim();
                    arbol.Nivel4.Habilitado = arbol.Nivel4.Habilitado;
                }
                if (arbol.Nivel5 != null)
                {
                    arbol.Nivel5.Descripcion = arbol.Nivel5.Descripcion.Trim();
                    arbol.Nivel5.Habilitado = arbol.Nivel5.Habilitado;
                }
                if (arbol.Nivel6 != null)
                {
                    arbol.Nivel6.Descripcion = arbol.Nivel6.Descripcion.Trim();
                    arbol.Nivel6.Habilitado = arbol.Nivel6.Habilitado;
                }
                if (arbol.Nivel7 != null)
                {
                    arbol.Nivel7.Descripcion = arbol.Nivel7.Descripcion.Trim();
                    arbol.Nivel7.Habilitado = arbol.Nivel7.Habilitado;
                }
                if (arbol.EsTerminal && arbol.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                {
                    arbol.InventarioArbolAcceso.First().Sla.TiempoHoraProceso = (arbol.InventarioArbolAcceso.First().Sla.Dias * 24) + (arbol.InventarioArbolAcceso.First().Sla.Horas / 24) + ((arbol.InventarioArbolAcceso.First().Sla.Minutos / 24) / 24) + (((arbol.InventarioArbolAcceso.First().Sla.Segundos / 60) / 24) / 24);
                }
                if (arbol.EsTerminal)
                {
                    // TODO: ESTE FRAGMENTO AGREGA LOS GRUPOS ESPECIALES DE CONSULTA ESPECIFICOS AL TIPO DE USUARIO
                    List<GrupoUsuario> gposEspCons = new BusinessGrupoUsuario().ObtenerGruposUsuarioByIdRol((int)BusinessVariables.EnumRoles.ConsultasEspeciales, false).Where(w => w.IdTipoUsuario == arbol.IdTipoUsuario).ToList();
                    foreach (GrupoUsuario gpoEspCons in gposEspCons)
                    {
                        if (arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.All(a => a.IdGrupoUsuario != gpoEspCons.Id))
                        {
                            arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                            {
                                IdRol = (int)BusinessVariables.EnumRoles.ConsultasEspeciales,
                                IdGrupoUsuario = gpoEspCons.Id,
                                IdSubGrupoUsuario = null
                            });
                        }
                    }
                }
                if (arbol.Id == 0)
                    db.ArbolAcceso.AddObject(arbol);
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
        public List<ArbolAcceso> ObtenerArbolesAccesoByUsuarioTipoArbol(int idUsuario, int idTipoArbol, int idArea)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = from ac in db.ArbolAcceso
                                              join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                              join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                              join ug in db.UsuarioGrupo on new { guia.IdRol, guia.IdGrupoUsuario, guia.IdSubGrupoUsuario, tu = ac.IdTipoUsuario } equals new { ug.IdRol, ug.IdGrupoUsuario, ug.IdSubGrupoUsuario, tu = ug.Usuario.IdTipoUsuario }
                                              where ug.IdUsuario == idUsuario && ac.IdTipoArbolAcceso == idTipoArbol && ac.IdArea == idArea && ac.Habilitado
                                              && (guia.IdRol == (int)BusinessVariables.EnumRoles.AgenteUniversal
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.ConsultasEspeciales
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.Agente
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeContenido
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeOperación
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeCategoría
                                              || guia.IdRol == (int)BusinessVariables.EnumRoles.Usuario)
                                              select ac;
                result = qry.ToList();
                foreach (ArbolAcceso arbol in result)
                {
                    db.LoadProperty(arbol, "Area");
                    db.LoadProperty(arbol, "Nivel1");
                    db.LoadProperty(arbol, "Nivel2");
                    db.LoadProperty(arbol, "Nivel3");
                    db.LoadProperty(arbol, "Nivel4");
                    db.LoadProperty(arbol, "Nivel5");
                    db.LoadProperty(arbol, "Nivel6");
                    db.LoadProperty(arbol, "Nivel7");
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

        public List<ArbolAcceso> ObtenerArbolesAccesoByIdUsuario(int idUsuario)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = from ac in db.ArbolAcceso
                                              join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                              join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                              join ug in db.UsuarioGrupo on new { guia.IdRol, guia.IdGrupoUsuario, guia.IdSubGrupoUsuario } equals new { ug.IdRol, ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                                              where ug.IdUsuario == idUsuario && guia.IdRol == (int)BusinessVariables.EnumRoles.Usuario
                                              select ac;
                result = qry.ToList();
                foreach (ArbolAcceso arbol in result)
                {
                    db.LoadProperty(arbol, "Area");
                    db.LoadProperty(arbol, "Nivel1");
                    db.LoadProperty(arbol, "Nivel2");
                    db.LoadProperty(arbol, "Nivel3");
                    db.LoadProperty(arbol, "Nivel4");
                    db.LoadProperty(arbol, "Nivel5");
                    db.LoadProperty(arbol, "Nivel6");
                    db.LoadProperty(arbol, "Nivel7");
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

        public List<ArbolAcceso> ObtenerArbolesAccesoByGrupos(List<int> lstGrupos)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = from ac in db.ArbolAcceso
                                              join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                              join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                              join ug in db.UsuarioGrupo on new { guia.IdRol, guia.IdGrupoUsuario, guia.IdSubGrupoUsuario } equals new { ug.IdRol, ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                                              where lstGrupos.Contains(guia.IdGrupoUsuario) && guia.IdRol == (int)BusinessVariables.EnumRoles.Usuario
                                              select ac;
                result = qry.ToList();
                foreach (ArbolAcceso arbol in result)
                {
                    db.LoadProperty(arbol, "Area");
                    db.LoadProperty(arbol, "Nivel1");
                    db.LoadProperty(arbol, "Nivel2");
                    db.LoadProperty(arbol, "Nivel3");
                    db.LoadProperty(arbol, "Nivel4");
                    db.LoadProperty(arbol, "Nivel5");
                    db.LoadProperty(arbol, "Nivel6");
                    db.LoadProperty(arbol, "Nivel7");
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

        public List<ArbolAcceso> ObtenerArbolesAccesoByTipoUsuarioTipoArbol(int idTipoUsuario, int idTipoArbol, int idArea)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = from ac in db.ArbolAcceso
                                              join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                              join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                              where ac.IdTipoUsuario == idTipoUsuario && ac.IdTipoArbolAcceso == idTipoArbol && ac.Habilitado && ac.IdArea == idArea
                                              && guia.IdRol == (int)BusinessVariables.EnumRoles.Usuario
                                              select ac;
                result = qry.ToList();
                foreach (ArbolAcceso arbol in result)
                {
                    db.LoadProperty(arbol, "Area");
                    db.LoadProperty(arbol, "Nivel1");
                    db.LoadProperty(arbol, "Nivel2");
                    db.LoadProperty(arbol, "Nivel3");
                    db.LoadProperty(arbol, "Nivel4");
                    db.LoadProperty(arbol, "Nivel5");
                    db.LoadProperty(arbol, "Nivel6");
                    db.LoadProperty(arbol, "Nivel7");
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

        public List<ArbolAcceso> ObtenerArbolesAccesoAll(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = db.ArbolAcceso.Where(w => !w.Sistema);
                if (idArea.HasValue)
                    qry = qry.Where(w => w.IdArea == idArea);
                if (idTipoUsuario.HasValue)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);
                if (idTipoArbol.HasValue)
                    qry = qry.Where(w => w.IdTipoArbolAcceso == idTipoArbol);

                if (nivel1.HasValue)
                    qry = qry.Where(w => w.IdNivel1 == nivel1);

                if (nivel2.HasValue)
                    qry = qry.Where(w => w.IdNivel2 == nivel2);

                if (nivel3.HasValue)
                    qry = qry.Where(w => w.IdNivel3 == nivel3);

                if (nivel4.HasValue)
                    qry = qry.Where(w => w.IdNivel4 == nivel4);

                if (nivel5.HasValue)
                    qry = qry.Where(w => w.IdNivel5 == nivel5);

                if (nivel6.HasValue)
                    qry = qry.Where(w => w.IdNivel6 == nivel6);

                if (nivel7.HasValue)
                    qry = qry.Where(w => w.IdNivel7 == nivel7);

                result = qry.ToList();

                foreach (ArbolAcceso arbol in result)
                {
                    db.LoadProperty(arbol, "Area");
                    db.LoadProperty(arbol, "TipoUsuario");
                    db.LoadProperty(arbol, "TipoArbolAcceso");
                    db.LoadProperty(arbol, "Nivel1");
                    db.LoadProperty(arbol, "Nivel2");
                    db.LoadProperty(arbol, "Nivel3");
                    db.LoadProperty(arbol, "Nivel4");
                    db.LoadProperty(arbol, "Nivel5");
                    db.LoadProperty(arbol, "Nivel6");
                    db.LoadProperty(arbol, "Nivel7");
                    arbol.Tipificacion = ObtenerTipificacion(arbol.Id);
                    arbol.Nivel = ObtenerNivel(arbol.Id);
                }
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Arboles");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<ArbolAcceso> ObtenerArbolesAccesoTerminalAll(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = db.ArbolAcceso;
                if (idArea.HasValue)
                    qry = qry.Where(w => w.IdArea == idArea);
                if (idTipoUsuario.HasValue)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);
                if (idTipoArbol.HasValue)
                    qry = qry.Where(w => w.IdTipoArbolAcceso == idTipoArbol);

                if (nivel1.HasValue)
                    qry = qry.Where(w => w.IdNivel1 == nivel1);

                if (nivel2.HasValue)
                    qry = qry.Where(w => w.IdNivel2 == nivel2);

                if (nivel3.HasValue)
                    qry = qry.Where(w => w.IdNivel3 == nivel3);

                if (nivel4.HasValue)
                    qry = qry.Where(w => w.IdNivel4 == nivel4);

                if (nivel5.HasValue)
                    qry = qry.Where(w => w.IdNivel5 == nivel5);

                if (nivel6.HasValue)
                    qry = qry.Where(w => w.IdNivel6 == nivel6);

                if (nivel7.HasValue)
                    qry = qry.Where(w => w.IdNivel7 == nivel7);

                qry = qry.Where(w => w.EsTerminal);
                result = qry.ToList();

                foreach (ArbolAcceso arbol in result)
                {
                    db.LoadProperty(arbol, "Area");
                    db.LoadProperty(arbol, "TipoUsuario");
                    db.LoadProperty(arbol, "TipoArbolAcceso");
                    db.LoadProperty(arbol, "Nivel1");
                    db.LoadProperty(arbol, "Nivel2");
                    db.LoadProperty(arbol, "Nivel3");
                    db.LoadProperty(arbol, "Nivel4");
                    db.LoadProperty(arbol, "Nivel5");
                    db.LoadProperty(arbol, "Nivel6");
                    db.LoadProperty(arbol, "Nivel7");
                }
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Arboles");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public ArbolAcceso ObtenerArbolAcceso(int idArbol)
        {
            ArbolAcceso result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.ArbolAcceso.SingleOrDefault(w => w.Habilitado && w.Id == idArbol);
                if (result == null) return null;
                db.LoadProperty(result, "Area");
                db.LoadProperty(result, "TipoUsuario");
                db.LoadProperty(result, "TipoArbolAcceso");
                db.LoadProperty(result, "Nivel1");
                db.LoadProperty(result, "Nivel2");
                db.LoadProperty(result, "Nivel3");
                db.LoadProperty(result, "Nivel4");
                db.LoadProperty(result, "Nivel5");
                db.LoadProperty(result, "Nivel6");
                db.LoadProperty(result, "Nivel7");
                db.LoadProperty(result, "InventarioArbolAcceso");
                db.LoadProperty(result, "TiempoInformeArbol");
                foreach (TiempoInformeArbol informeArbol in result.TiempoInformeArbol)
                {
                    db.LoadProperty(informeArbol, "GrupoUsuario");
                    db.LoadProperty(informeArbol, "TipoNotificacion");
                }
                db.LoadProperty(result, "Impacto");
                if (result.Impacto != null)
                {
                    db.LoadProperty(result.Impacto, "Prioridad");
                    db.LoadProperty(result.Impacto, "Urgencia");
                }

                foreach (InventarioArbolAcceso inventarioArbol in result.InventarioArbolAcceso)
                {
                    db.LoadProperty(inventarioArbol, "GrupoUsuarioInventarioArbol");
                    foreach (GrupoUsuarioInventarioArbol gpo in inventarioArbol.GrupoUsuarioInventarioArbol)
                    {
                        db.LoadProperty(gpo, "GrupoUsuario");
                        db.LoadProperty(gpo, "SubGrupoUsuario");
                    }
                    db.LoadProperty(inventarioArbol, "InventarioInfConsulta");
                    db.LoadProperty(inventarioArbol, "Mascara");
                    db.LoadProperty(inventarioArbol, "Sla");
                    db.LoadProperty(inventarioArbol, "Encuesta");
                    foreach (InventarioInfConsulta inventarioInformacion in inventarioArbol.InventarioInfConsulta)
                    {
                        db.LoadProperty(inventarioInformacion, "InformacionConsulta");
                        db.LoadProperty(inventarioInformacion.InformacionConsulta, "TipoInfConsulta");
                    }
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

        public void HitArbolAcceso(int idArbol)
        {
            ArbolAcceso result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = true;
                result = db.ArbolAcceso.SingleOrDefault(w => w.Habilitado && w.Id == idArbol);
                if (result != null)
                {
                    result.Visitas++;
                    result.FechaVisita = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    db.SaveChanges();
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
        }

        public List<GrupoUsuarioInventarioArbol> ObtenerGruposUsuarioArbol(int idArbol)
        {
            List<GrupoUsuarioInventarioArbol> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<GrupoUsuarioInventarioArbol> qry = from ac in db.ArbolAcceso
                                                              join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                                              join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                                              where ac.Id == idArbol
                                                              select guia;
                result = qry.Distinct().ToList();
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

        private string ObtenerRutaCompleta(int idArbol)
        {
            string result = string.Empty;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.LazyLoadingEnabled = true;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(w => w.Habilitado && w.Id == idArbol);
                if (arbol == null) return null;
                string nombre = arbol.InventarioArbolAcceso.First().Descripcion;
                bool esUltimoNivel = false;
                if (arbol.Nivel1 != null)
                    result += arbol.Nivel1.Descripcion;
                if (arbol.Nivel2 != null)
                    result += " " + arbol.Nivel2.Descripcion;
                if (arbol.Nivel3 != null)
                    result += " " + arbol.Nivel3.Descripcion;
                if (arbol.Nivel4 != null)
                    result += " " + arbol.Nivel4.Descripcion;
                if (arbol.Nivel5 != null)
                    result += " " + arbol.Nivel5.Descripcion;
                if (arbol.Nivel6 != null)
                    result += " " + arbol.Nivel6.Descripcion;
                if (arbol.Nivel7 != null)
                {
                    esUltimoNivel = true;
                    result += " " + arbol.Nivel7.Descripcion;
                }
                if (!esUltimoNivel)
                    result += " " + nombre;

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
        public string ObtenerTipificacion(int idArbol)
        {
            string result = string.Empty;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(w => w.Id == idArbol);
                if (arbol == null) return null;
                if (arbol.Nivel1 != null)
                    result = arbol.Nivel1.Descripcion;
                if (arbol.Nivel2 != null)
                    result = arbol.Nivel2.Descripcion;
                if (arbol.Nivel3 != null)
                    result = arbol.Nivel3.Descripcion;
                if (arbol.Nivel4 != null)
                    result = arbol.Nivel4.Descripcion;
                if (arbol.Nivel5 != null)
                    result = arbol.Nivel5.Descripcion;
                if (arbol.Nivel6 != null)
                    result = arbol.Nivel6.Descripcion;
                if (arbol.Nivel7 != null)
                    result = arbol.Nivel7.Descripcion;
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

        public int ObtenerNivel(int idArbol)
        {
            int result = 0;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(w => w.Id == idArbol);
                if (arbol != null)
                {
                    if (arbol.Nivel1 != null)
                        result = 1;
                    if (arbol.Nivel2 != null)
                        result = 2;
                    if (arbol.Nivel3 != null)
                        result = 3;
                    if (arbol.Nivel4 != null)
                        result = 4;
                    if (arbol.Nivel5 != null)
                        result = 5;
                    if (arbol.Nivel6 != null)
                        result = 6;
                    if (arbol.Nivel7 != null)
                        result = 7;
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

        public void HabilitarArbol(int idArbol, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(w => w.Id == idArbol);
                if (arbol != null) arbol.Habilitado = habilitado;
                db.SaveChanges();

                //db.ContextOptions.LazyLoadingEnabled = true;
                //db.ContextOptions.ProxyCreationEnabled = true;
                //ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(w => w.Id == idArbol);
                //if (arbol != null)
                //{
                //    int nivel = ObtenerNivel(idArbol);
                //    arbol.Habilitado = habilitado;
                //    var qry = db.ArbolAcceso.Where(w => w.IdTipoUsuario == arbol.IdTipoUsuario);
                //    if (!habilitado)
                //    {
                //        switch (nivel)
                //        {
                //            case 1:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1);
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //            case 2:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && w.IdNivel2 == arbol.IdNivel2);
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //            case 3:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && w.IdNivel2 == arbol.IdNivel2 && w.IdNivel3 == arbol.IdNivel3);
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //            case 4:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && w.IdNivel2 == arbol.IdNivel2 && w.IdNivel3 == arbol.IdNivel3 && w.IdNivel4 == arbol.IdNivel4);
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //            case 5:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && w.IdNivel2 == arbol.IdNivel2 && w.IdNivel3 == arbol.IdNivel3 && w.IdNivel4 == arbol.IdNivel4 && w.IdNivel5 == arbol.IdNivel5);
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //            case 6:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && w.IdNivel2 == arbol.IdNivel2 && w.IdNivel3 == arbol.IdNivel3 && w.IdNivel4 == arbol.IdNivel4 && w.IdNivel5 == arbol.IdNivel5 && w.IdNivel6 == arbol.IdNivel6);
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //            case 7:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1
                //                    && w.IdNivel2 == arbol.IdNivel2
                //                    && w.IdNivel3 == arbol.IdNivel3
                //                    && w.IdNivel4 == arbol.IdNivel4
                //                    && w.IdNivel5 == arbol.IdNivel5
                //                    && w.IdNivel6 == arbol.IdNivel6
                //                    && w.IdNivel7 == arbol.IdNivel7
                //                    );
                //                arbol.Habilitado = false;
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = false;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = false;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = false;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = false;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = false;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = false;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = false;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = false;
                //                }
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        switch (nivel)
                //        {
                //            case 1:
                //                arbol.Habilitado = true;
                //                if (arbol.IdNivel1.HasValue)
                //                    arbol.Nivel1.Habilitado = true;
                //                break;
                //            case 2:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && w.Id <= arbol.Id);
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {
                //                    arboles.Habilitado = true;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = true;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = true;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = true;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = true;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = true;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = true;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = true;
                //                }
                //                break;
                //            case 3:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && (w.IdNivel2 == arbol.IdNivel2 || w.IdNivel2 == null) && w.Id <= arbol.Id);
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {

                //                    arboles.Habilitado = true;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = true;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = true;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = true;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = true;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = true;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = true;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = true;
                //                }
                //                break;
                //            case 4:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && (w.IdNivel2 == arbol.IdNivel2 || w.IdNivel2 == null) && (w.IdNivel3 == arbol.IdNivel3 || w.IdNivel3 == null) && w.Id <= arbol.Id);
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {

                //                    arboles.Habilitado = true;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = true;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = true;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = true;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = true;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = true;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = true;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = true;
                //                }
                //                break;
                //            case 5:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && (w.IdNivel2 == arbol.IdNivel2 || w.IdNivel2 == null) && (w.IdNivel3 == arbol.IdNivel3 || w.IdNivel3 == null) && (w.IdNivel4 == arbol.IdNivel4 || w.IdNivel4 == null) && w.Id <= arbol.Id);
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {

                //                    arboles.Habilitado = true;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = true;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = true;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = true;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = true;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = true;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = true;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = true;
                //                }
                //                break;
                //            case 6:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && (w.IdNivel2 == arbol.IdNivel2 || w.IdNivel2 == null) && (w.IdNivel3 == arbol.IdNivel3 || w.IdNivel3 == null) && (w.IdNivel4 == arbol.IdNivel4 || w.IdNivel4 == null) && (w.IdNivel5 == arbol.IdNivel5 || w.IdNivel5 == null) && w.Id <= arbol.Id);
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {

                //                    arboles.Habilitado = true;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = true;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = true;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = true;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = true;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = true;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = true;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = true;
                //                }
                //                break;
                //            case 7:
                //                qry = qry.Where(w => w.IdNivel1 == arbol.IdNivel1 && (w.IdNivel2 == arbol.IdNivel2 || w.IdNivel2 == null) && (w.IdNivel3 == arbol.IdNivel3 || w.IdNivel3 == null) && (w.IdNivel4 == arbol.IdNivel4 || w.IdNivel4 == null) && (w.IdNivel5 == arbol.IdNivel5 || w.IdNivel5 == null) && (w.IdNivel6 == arbol.IdNivel6 || w.IdNivel6 == null) && w.Id <= arbol.Id);
                //                foreach (ArbolAcceso arboles in qry.OrderBy(o => o.Id))
                //                {

                //                    arboles.Habilitado = true;
                //                    if (arboles.IdNivel1.HasValue)
                //                        arboles.Nivel1.Habilitado = true;
                //                    if (arboles.IdNivel2.HasValue)
                //                        arboles.Nivel2.Habilitado = true;
                //                    if (arboles.IdNivel3.HasValue)
                //                        arboles.Nivel3.Habilitado = true;
                //                    if (arboles.IdNivel4.HasValue)
                //                        arboles.Nivel4.Habilitado = true;
                //                    if (arboles.IdNivel5.HasValue)
                //                        arboles.Nivel5.Habilitado = true;
                //                    if (arboles.IdNivel6.HasValue)
                //                        arboles.Nivel6.Habilitado = true;
                //                    if (arboles.IdNivel7.HasValue)
                //                        arboles.Nivel7.Habilitado = true;
                //                }
                //                break;
                //        }
                //    }
                //}
                //db.SaveChanges();
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

        public void ActualizardArbol(int idArbolAcceso, ArbolAcceso arbolAccesoActualizar, string descripcion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(s => s.Id == idArbolAcceso);
                if (arbol != null)
                {
                    List<GrupoUsuarioInventarioArbol> gpoToRemove = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(gpo => !arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Any(a => a.IdGrupoUsuario == gpo.IdGrupoUsuario && a.IdRol == gpo.IdRol && a.IdSubGrupoUsuario == gpo.IdSubGrupoUsuario)).ToList();

                    foreach (GrupoUsuarioInventarioArbol gpo in gpoToRemove)
                    {
                        db.GrupoUsuarioInventarioArbol.DeleteObject(gpo);
                    }

                    foreach (GrupoUsuarioInventarioArbol gpo in arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(gpo => !arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Any(a => a.IdGrupoUsuario == gpo.IdGrupoUsuario && a.IdRol == gpo.IdRol && a.IdSubGrupoUsuario == gpo.IdSubGrupoUsuario)))
                    {
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Add(gpo);
                    }

                    foreach (InventarioInfConsulta infConsulta in arbolAccesoActualizar.InventarioArbolAcceso.First().InventarioInfConsulta)
                    {
                        InformacionConsulta info = db.InformacionConsulta.Single(s => s.Id == infConsulta.IdInfConsulta);
                        arbol.InventarioArbolAcceso.First().InventarioInfConsulta.Single(w => w.InformacionConsulta.IdTipoInfConsulta == info.IdTipoInfConsulta).IdInfConsulta = infConsulta.IdInfConsulta;
                    }

                    if (arbol.IdTipoUsuario != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                    {
                        arbol.InventarioArbolAcceso.First().Sla.Dias =
                            arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Dias;
                        arbol.InventarioArbolAcceso.First().Sla.Horas =
                            arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Horas;
                        arbol.InventarioArbolAcceso.First().Sla.Minutos =
                            arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Minutos;
                        arbol.InventarioArbolAcceso.First().Sla.Segundos =
                            arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Segundos;
                        arbol.InventarioArbolAcceso.First().Sla.TiempoHoraProceso =
                            arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.TiempoHoraProceso;
                        arbol.InventarioArbolAcceso.First().Sla.Detallado =
                            arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Detallado;
                        List<SlaDetalle> slaDetalleRemove = new List<SlaDetalle>();
                        foreach (SlaDetalle detRemove in arbol.InventarioArbolAcceso.First().Sla.SlaDetalle)
                        {

                        }
                        if (arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Detallado)
                        {
                            if (arbol.InventarioArbolAcceso.First().Sla.SlaDetalle == null)
                                arbol.InventarioArbolAcceso.First().Sla.SlaDetalle = new List<SlaDetalle>();
                        }

                        if (arbol.TiempoInformeArbol.Count > 0)
                            foreach (TiempoInformeArbol informeArbol in arbolAccesoActualizar.TiempoInformeArbol.Distinct())
                            {
                                switch (informeArbol.IdTipoGrupo)
                                {
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido:
                                        TiempoInformeArbol tInformeMto = db.TiempoInformeArbol.SingleOrDefault(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo);
                                        tInformeMto = arbol.TiempoInformeArbol.SingleOrDefault(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo);
                                        if (tInformeMto != null)
                                        {
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Dias = informeArbol.Dias;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Horas = informeArbol.Horas;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Minutos = informeArbol.Minutos;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Segundos = informeArbol.Segundos;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).IdTipoNotificacion = informeArbol.IdTipoNotificacion;
                                        }
                                        break;
                                    case (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo:
                                        TiempoInformeArbol tInformeDev = db.TiempoInformeArbol.SingleOrDefault(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo);
                                        tInformeDev = arbol.TiempoInformeArbol.SingleOrDefault(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo);
                                        if (tInformeDev != null)
                                        {
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Dias = informeArbol.Dias;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Horas = informeArbol.Horas;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Minutos = informeArbol.Minutos;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Segundos = informeArbol.Segundos;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).IdTipoNotificacion = informeArbol.IdTipoNotificacion;
                                        }
                                        break;

                                    case (int)BusinessVariables.EnumTiposGrupos.ConsultasEspeciales:
                                        TiempoInformeArbol tInformeCons = db.TiempoInformeArbol.SingleOrDefault(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo);
                                        tInformeCons = arbol.TiempoInformeArbol.SingleOrDefault(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo);
                                        if (tInformeCons != null)
                                        {
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Dias = informeArbol.Dias;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Horas = informeArbol.Horas;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Minutos = informeArbol.Minutos;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).Segundos = informeArbol.Segundos;
                                            arbol.TiempoInformeArbol.Single(s => s.IdArbol == arbol.Id && s.IdGrupoUsuario == informeArbol.IdGrupoUsuario && s.IdTipoGrupo == informeArbol.IdTipoGrupo).IdTipoNotificacion = informeArbol.IdTipoNotificacion;
                                        }
                                        break;
                                }
                            }
                    }

                    arbol.IdImpacto = arbolAccesoActualizar.IdImpacto;
                    arbol.InventarioArbolAcceso.First().IdMascara = arbolAccesoActualizar.InventarioArbolAcceso.First().IdMascara;
                    arbol.InventarioArbolAcceso.First().IdEncuesta = arbolAccesoActualizar.InventarioArbolAcceso.First().IdEncuesta;

                    if (arbol.Nivel7 != null)
                        arbol.Nivel7.Descripcion = descripcion.Trim();
                    else if (arbol.Nivel6 != null)
                        arbol.Nivel6.Descripcion = descripcion.Trim();
                    else if (arbol.Nivel5 != null)
                        arbol.Nivel5.Descripcion = descripcion.Trim();
                    else if (arbol.Nivel4 != null)
                        arbol.Nivel4.Descripcion = descripcion.Trim();
                    else if (arbol.Nivel3 != null)
                        arbol.Nivel3.Descripcion = descripcion.Trim();
                    else if (arbol.Nivel2 != null)
                        arbol.Nivel2.Descripcion = descripcion.Trim();
                    else if (arbol.Nivel1 != null)
                        arbol.Nivel1.Descripcion = descripcion.Trim();

                    db.SaveChanges();
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
        }

        public List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalByIdUsuario(int idUsuario, bool insertarSeleccion)
        {
            List<HelperArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = from ac in db.ArbolAcceso
                                              join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                              join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                              join ug in db.UsuarioGrupo on new { guia.IdRol, guia.IdGrupoUsuario, guia.IdSubGrupoUsuario } equals new { ug.IdRol, ug.IdGrupoUsuario, ug.IdSubGrupoUsuario }
                                              where ug.IdUsuario == idUsuario && guia.IdRol == (int)BusinessVariables.EnumRoles.Usuario
                                              select ac;

                result = qry.ToList().Select(arbol => new HelperArbolAcceso { Id = arbol.Id, DescripcionTipificacion = ObtenerTipificacion(arbol.Id) }).ToList();
                if (insertarSeleccion)
                    result.Insert(0, new HelperArbolAcceso { Id = BusinessVariables.ComboBoxCatalogo.IndexSeleccione, DescripcionTipificacion = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione });
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
        public List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalAllTipificacion(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            List<HelperArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = db.ArbolAcceso;
                if (idArea.HasValue)
                    qry = qry.Where(w => w.IdArea == idArea);
                if (idTipoUsuario.HasValue)
                    qry = qry.Where(w => w.IdTipoUsuario == idTipoUsuario);
                if (idTipoArbol.HasValue)
                    qry = qry.Where(w => w.IdTipoArbolAcceso == idTipoArbol);

                if (nivel1.HasValue)
                    qry = qry.Where(w => w.IdNivel1 == nivel1);

                if (nivel2.HasValue)
                    qry = qry.Where(w => w.IdNivel2 == nivel2);

                if (nivel3.HasValue)
                    qry = qry.Where(w => w.IdNivel3 == nivel3);

                if (nivel4.HasValue)
                    qry = qry.Where(w => w.IdNivel4 == nivel4);

                if (nivel5.HasValue)
                    qry = qry.Where(w => w.IdNivel5 == nivel5);

                if (nivel6.HasValue)
                    qry = qry.Where(w => w.IdNivel6 == nivel6);

                if (nivel7.HasValue)
                    qry = qry.Where(w => w.IdNivel7 == nivel7);

                qry = qry.Where(w => w.EsTerminal);
                result = qry.ToList().Select(arbol => new HelperArbolAcceso { Id = arbol.Id, DescripcionTipificacion = ObtenerTipificacion(arbol.Id) }).ToList();
            }
            catch (Exception)
            {
                throw new Exception("Error al Obtener Arboles");
            }
            finally
            {
                db.Dispose();
            }
            return result;
        }

        public List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalByGrupoUsuario(int idGrupo)
        {
            List<HelperArbolAcceso> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<ArbolAcceso> qry = (from ac in db.ArbolAcceso
                                         join iac in db.InventarioArbolAcceso on ac.Id equals iac.IdArbolAcceso
                                         join guia in db.GrupoUsuarioInventarioArbol on iac.Id equals guia.IdInventarioArbolAcceso
                                         where guia.IdGrupoUsuario == idGrupo && !ac.Sistema && ac.EsTerminal
                                         select ac).ToList();
                if (qry.Any())
                {
                    result = new List<HelperArbolAcceso>();
                    foreach (ArbolAcceso arbolAcceso in qry.Distinct().ToList())
                    {
                        db.LoadProperty(arbolAcceso, "Area");
                        db.LoadProperty(arbolAcceso, "TipoArbolAcceso");
                        db.LoadProperty(arbolAcceso, "TipoUsuario");
                        db.LoadProperty(arbolAcceso, "InventarioArbolAcceso");
                        HelperArbolAcceso add = new HelperArbolAcceso
                        {

                            Id = arbolAcceso.Id,
                            Titulo = arbolAcceso.InventarioArbolAcceso.First().Descripcion,
                            TipoUsuario = arbolAcceso.TipoUsuario.Descripcion,
                            Categoria = arbolAcceso.Area.Descripcion,
                            DescripcionTipificacion = ObtenerTipificacion(arbolAcceso.Id),
                            Nivel = ObtenerNivel(arbolAcceso.Id),
                            Tipo = arbolAcceso.TipoArbolAcceso.Descripcion,
                            Activo = arbolAcceso.Habilitado ? "Si" : "No"
                        };
                        result.Add(add);
                    }
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

        #endregion Flujo normal

        public List<HelperBusquedaArbolAcceso> BusquedaGeneral(int? idUsuario, string filter, List<int> tipoUsuario, int? idTipoArbol, int? idCategoria, int page, int pagesize)
        {
            List<HelperBusquedaArbolAcceso> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                var qry = from aa in db.ArbolAcceso
                          join iaa in db.InventarioArbolAcceso on aa.Id equals iaa.IdArbolAcceso
                          join guia in db.GrupoUsuarioInventarioArbol on iaa.Id equals guia.IdInventarioArbolAcceso
                          where aa.EsTerminal && iaa.Descripcion.Contains(filter)
                          select new { aa, iaa, guia };
                if (tipoUsuario != null && tipoUsuario.Any())
                    qry = from q in qry
                          where tipoUsuario.Contains(q.aa.IdTipoUsuario)
                          select q;
                if (idTipoArbol != null)
                    qry = from q in qry
                          where q.aa.IdTipoArbolAcceso == idTipoArbol
                          select q;
                if (idCategoria != null)
                    qry = from q in qry
                          where q.aa.IdArea == idCategoria
                          select q;
                if (idUsuario != null)
                {
                    Usuario usuarioSolicita = new BusinessUsuarios().ObtenerDetalleUsuario((int)idUsuario);
                    List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Usuario).Select(s => s.IdGrupoUsuario).ToList();
                    qry = from q in qry
                          where lstGposSolicita.Contains(q.guia.IdGrupoUsuario)
                          select q;
                }
                else
                {
                    qry = from q in qry
                          where q.aa.Publico
                          select q;
                }
                var qryIds = from q in qry
                    select q.aa.Id;
                List<int> arbolesServicioProblema = (from q in qryIds select q).OrderBy(o => o).Distinct().ToList();

                int skip = (page - 1) * pagesize;
                int totalResults = int.Parse(Math.Ceiling(arbolesServicioProblema.Count() / decimal.Parse(pagesize.ToString())).ToString());
                
                if (arbolesServicioProblema.Any())
                {
                    result = new List<HelperBusquedaArbolAcceso>();
                    foreach (ArbolAcceso arbol in db.ArbolAcceso.Where(w => arbolesServicioProblema.Contains(w.Id) && w.EsTerminal).OrderBy(o=>o.Id).Skip(skip).Take(pagesize).Distinct())
                    {
                        if (arbol.EsTerminal)
                        {
                            db.LoadProperty(arbol, "Area");
                            db.LoadProperty(arbol, "InventarioArbolAcceso");
                            HelperBusquedaArbolAcceso addArbol = new HelperBusquedaArbolAcceso
                            {
                                Id = arbol.Id,
                                Titulo = arbol.InventarioArbolAcceso.First().Descripcion,
                                Descripcion = arbol.Descripcion,
                                IdCategoria = arbol.IdArea,
                                Categoria = arbol.Area.Descripcion,
                                TotalLikes = arbol.MeGusta,
                                TotalPage = totalResults
                            };
                            result.Add(addArbol);
                        }
                    }
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
