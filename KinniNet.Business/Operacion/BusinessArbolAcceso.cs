using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                    //arbol.InventarioArbolAcceso.First().Sla.TiempoHoraProceso = (arbol.InventarioArbolAcceso.First().Sla.Dias * 24) + arbol.InventarioArbolAcceso.First().Sla.Horas + (arbol.InventarioArbolAcceso.First().Sla.Minutos / 60) + ((arbol.InventarioArbolAcceso.First().Sla.Minutos / 60) / 60); ;
                }
                #region Arbol Terminal

                if (arbol.EsTerminal)
                {
                    string descripcionFinal = arbol.InventarioArbolAcceso.First().Descripcion.Trim();
                    bool existe = false;
                    int nivelArbol = ObtenerNivelArbol(arbol);
                    switch (nivelArbol)
                    {
                        case 1:
                            existe = db.Nivel1.Join(db.ArbolAcceso, n1 => n1.Id, aa => aa.IdNivel1, (n1, aa) => new { n1, aa }).Any(@t => @t.n1.Descripcion == descripcionFinal && @t.n1.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea);
                            break;
                        case 2:
                            existe = db.Nivel2.Join(db.ArbolAcceso, n2 => n2.Id, aa => aa.IdNivel2, (n2, aa) => new { n2, aa }).Any(@t => @t.n2.Descripcion == descripcionFinal && @t.n2.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                && @t.aa.IdNivel1 == arbol.IdNivel1);
                            break;
                        case 3:
                            existe = db.Nivel3.Join(db.ArbolAcceso, n3 => n3.Id, aa => aa.IdNivel3, (n3, aa) => new { n3, aa }).Any(@t => @t.n3.Descripcion == descripcionFinal && @t.n3.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea);
                            break;
                        case 4:
                            existe = db.Nivel4.Join(db.ArbolAcceso, n4 => n4.Id, aa => aa.IdNivel4, (n4, aa) => new { n4, aa }).Any(@t => @t.n4.Descripcion == descripcionFinal && @t.n4.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2);
                            break;
                        case 5:
                            existe = db.Nivel5.Join(db.ArbolAcceso, n5 => n5.Id, aa => aa.IdNivel5, (n5, aa) => new { n5, aa }).Any(@t => @t.n5.Descripcion == descripcionFinal && @t.n5.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4);
                            break;
                        case 6:
                            existe = db.Nivel6.Join(db.ArbolAcceso, n6 => n6.Id, aa => aa.IdNivel6, (n6, aa) => new { n6, aa }).Any(@t => @t.n6.Descripcion == descripcionFinal && @t.n6.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4
                                && @t.aa.IdNivel5 == arbol.IdNivel5
                                );
                            break;
                        case 7:
                            existe = db.Nivel7.Join(db.ArbolAcceso, n7 => n7.Id, aa => aa.IdNivel7, (n7, aa) => new { n7, aa }).Any(@t =>
                                @t.n7.Descripcion == descripcionFinal && @t.n7.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4
                                && @t.aa.IdNivel5 == arbol.IdNivel5
                                && @t.aa.IdNivel6 == arbol.IdNivel6
                                );
                            break;
                    }

                    if (existe)
                        throw new Exception("Esta opcion ya se encuentra registrada");

                    // TODO: ESTE FRAGMENTO AGREGA LOS GRUPOS ESPECIALES DE CONSULTA ESPECIFICOS AL TIPO DE USUARIO

                    List<GrupoUsuario> gpoEspCons;
                    if (arbol.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        if (
                            !arbol.InventarioArbolAcceso.First()
                                .GrupoUsuarioInventarioArbol.Any(
                                    a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeContenido))
                        {
                            gpoEspCons = new BusinessGrupoUsuario().ObtenerGrupoDefaultRolOpcion(
                                arbol.IdTipoArbolAcceso, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeContenido,
                                arbol.IdTipoUsuario);
                            foreach (GrupoUsuario grupoUsuario in gpoEspCons)
                            {
                                if (
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.All(a => a.IdGrupoUsuario != grupoUsuario.Id))
                                {
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                        {
                                            IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeContenido,
                                            IdGrupoUsuario = grupoUsuario.Id,
                                            IdSubGrupoUsuario = null
                                        });
                                }
                            }
                        }

                    if (arbol.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        if (
                            !arbol.InventarioArbolAcceso.First()
                                .GrupoUsuarioInventarioArbol.Any(
                                    a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeOperación))
                        {
                            gpoEspCons = new BusinessGrupoUsuario().ObtenerGrupoDefaultRolOpcion(
                                arbol.IdTipoArbolAcceso, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación,
                                arbol.IdTipoUsuario);
                            foreach (GrupoUsuario grupoUsuario in gpoEspCons)
                            {
                                if (
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.All(a => a.IdGrupoUsuario != grupoUsuario.Id))
                                {
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                        {
                                            IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeOperación,
                                            IdGrupoUsuario = grupoUsuario.Id,
                                            IdSubGrupoUsuario = null
                                        });
                                }
                            }
                        }

                    if (arbol.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        if (
                            !arbol.InventarioArbolAcceso.First()
                                .GrupoUsuarioInventarioArbol.Any(
                                    a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo))
                        {
                            gpoEspCons = new BusinessGrupoUsuario().ObtenerGrupoDefaultRolOpcion(
                                arbol.IdTipoArbolAcceso, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeDesarrollo,
                                arbol.IdTipoUsuario);
                            foreach (GrupoUsuario grupoUsuario in gpoEspCons)
                            {
                                if (
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.All(a => a.IdGrupoUsuario != grupoUsuario.Id))
                                {
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                        {
                                            IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo,
                                            IdGrupoUsuario = grupoUsuario.Id,
                                            IdSubGrupoUsuario = null
                                        });
                                }
                            }
                        }

                    gpoEspCons = new BusinessGrupoUsuario().ObtenerGrupoDefaultRolOpcion(arbol.IdTipoArbolAcceso,
                        (int)BusinessVariables.EnumTiposGrupos.AccesoAnalíticos, arbol.IdTipoUsuario);
                    foreach (GrupoUsuario grupoUsuario in gpoEspCons)
                    {
                        if (
                            arbol.InventarioArbolAcceso.First()
                                .GrupoUsuarioInventarioArbol.All(a => a.IdGrupoUsuario != grupoUsuario.Id))
                        {
                            arbol.InventarioArbolAcceso.First()
                                .GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                {
                                    IdRol = (int)BusinessVariables.EnumRoles.AccesoAnalíticos,
                                    IdGrupoUsuario = grupoUsuario.Id,
                                    IdSubGrupoUsuario = null
                                });
                        }
                    }

                    if (arbol.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        if (
                            !arbol.InventarioArbolAcceso.First()
                                .GrupoUsuarioInventarioArbol.Any(
                                    a => a.IdRol == (int)BusinessVariables.EnumRoles.ResponsableDeDesarrollo))
                        {
                            gpoEspCons = new BusinessGrupoUsuario().ObtenerGrupoDefaultRolOpcion(
                                arbol.IdTipoArbolAcceso, (int)BusinessVariables.EnumTiposGrupos.ResponsableDeCategoría,
                                arbol.IdTipoUsuario);
                            foreach (GrupoUsuario grupoUsuario in gpoEspCons)
                            {
                                if (
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.All(a => a.IdGrupoUsuario != grupoUsuario.Id))
                                {
                                    arbol.InventarioArbolAcceso.First()
                                        .GrupoUsuarioInventarioArbol.Add(new GrupoUsuarioInventarioArbol
                                        {
                                            IdRol = (int)BusinessVariables.EnumRoles.ResponsableDeCategoría,
                                            IdGrupoUsuario = grupoUsuario.Id,
                                            IdSubGrupoUsuario = null
                                        });
                                }
                            }
                        }
                }
                #endregion Arbol Terminal

                else
                {
                    if (arbol.Nivel1 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel1.Descripcion == arbol.Nivel1.Descripcion.Trim()))
                            throw new Exception("Esta sección ya se encuentra registrada");
                    }
                    if (arbol.Nivel2 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel2.Descripcion == arbol.Nivel2.Descripcion.Trim() && a.IdNivel1 == arbol.IdNivel1))
                            throw new Exception("Esta sección ya se encuentra registrada");
                    }
                    if (arbol.Nivel3 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel3.Descripcion == arbol.Nivel3.Descripcion.Trim()
                            && a.IdNivel1 == arbol.IdNivel1 && a.IdNivel2 == arbol.IdNivel2))
                            throw new Exception("Esta sección ya se encuentra registrada");
                    }
                    if (arbol.Nivel4 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel4.Descripcion == arbol.Nivel4.Descripcion.Trim()
                                && a.IdNivel1 == arbol.IdNivel1 && a.IdNivel2 == arbol.IdNivel2 && a.IdNivel3 == arbol.IdNivel3))
                            throw new Exception("Esta sección ya se encuentra registrada");
                    }
                    if (arbol.Nivel5 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel5.Descripcion == arbol.Nivel5.Descripcion.Trim()
                            && a.IdNivel1 == arbol.IdNivel1 && a.IdNivel2 == arbol.IdNivel2 && a.IdNivel3 == arbol.IdNivel3 && a.IdNivel4 == arbol.IdNivel4))
                            throw new Exception("Esta sección ya se encuentra registrada");
                    }
                    if (arbol.Nivel6 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel6.Descripcion == arbol.Nivel6.Descripcion.Trim()
                            && a.IdNivel1 == arbol.IdNivel1 && a.IdNivel2 == arbol.IdNivel2 && a.IdNivel3 == arbol.IdNivel3 && a.IdNivel4 == arbol.IdNivel4 && a.IdNivel5 == arbol.IdNivel5))
                            throw new Exception("Esta sección ya se encuentra registrada");
                    }
                    if (arbol.Nivel7 != null)
                    {
                        if (db.ArbolAcceso.Any(a => a.IdArea == arbol.IdArea && a.Nivel1.IdTipoUsuario == arbol.IdTipoUsuario && a.Nivel7.Descripcion == arbol.Nivel7.Descripcion.Trim()
                            && a.IdNivel1 == arbol.IdNivel1 && a.IdNivel2 == arbol.IdNivel2 && a.IdNivel3 == arbol.IdNivel3 && a.IdNivel4 == arbol.IdNivel4 && a.IdNivel5 == arbol.IdNivel5 && a.IdNivel6 == arbol.IdNivel6))
                            throw new Exception("Esta sección ya se encuentra registrada");
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
                                              && (guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte)
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
                                              where ug.IdUsuario == idUsuario && guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte
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
                                              where lstGrupos.Contains(guia.IdGrupoUsuario) && guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte
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
                                              && guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte
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

        public List<ArbolAcceso> ObtenerArbolesAccesoSeccion(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            List<ArbolAcceso> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<ArbolAcceso> qry = db.ArbolAcceso.Where(w => !w.Sistema && !w.EsTerminal);
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

        public List<ArbolAcceso> ObtenerArbolesAccesoAllReporte(int? idArea, int? idTipoUsuario, int? idTipoArbol, int idTipoEncuesta, Dictionary<string, DateTime> fechas)
        {
            List<ArbolAcceso> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from re in db.RespuestaEncuesta
                          join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                          join t in db.Ticket on re.IdTicket equals t.Id
                          join aa in db.ArbolAcceso on t.IdArbolAcceso equals aa.Id
                          where te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                && !aa.Sistema && re.Encuesta.IdTipoEncuesta == idTipoEncuesta
                          select new { re, te, aa };

                if (idArea.HasValue)
                    qry = qry.Where(w => w.aa.IdArea == idArea);
                if (idTipoUsuario.HasValue)
                    qry = qry.Where(w => w.aa.IdTipoUsuario == idTipoUsuario);
                if (idTipoArbol.HasValue)
                    qry = qry.Where(w => w.aa.IdTipoArbolAcceso == idTipoArbol);

                switch (idTipoEncuesta)
                {
                    case (int)BusinessVariables.EnumTipoEncuesta.PromotorScore:
                        var qryPromotores = from re in db.RespuestaEncuesta
                                            join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                            join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                            where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                                  && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                                  && re.ValorRespuesta >= 9
                                            select new { re, te };

                        List<int> neutros = new List<int> { 7, 8 };
                        var qryNeutros = from re in db.RespuestaEncuesta
                                         join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                         join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                         where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                               && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                               && neutros.Contains(re.ValorRespuesta)
                                         select new { re, te };

                        var qryDetractores = from re in db.RespuestaEncuesta
                                             join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                             join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                             where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.PromotorScore
                                                   && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                                   && re.ValorRespuesta <= 6
                                             select new { re, te };
                        if (fechas != null)
                        {
                            DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            qry = qry.Where(w => w.te.FechaMovimiento >= fechaInicio && w.te.FechaMovimiento < fechaFin);

                            qryPromotores = from q in qryPromotores
                                            where q.te.FechaMovimiento >= fechaInicio
                                                   && q.te.FechaMovimiento < fechaFin
                                            select q;

                            qryNeutros = from q in qryNeutros
                                         where q.te.FechaMovimiento >= fechaInicio
                                                   && q.te.FechaMovimiento < fechaFin
                                         select q;

                            qryDetractores = from q in qryDetractores
                                             where q.te.FechaMovimiento >= fechaInicio
                                                   && q.te.FechaMovimiento < fechaFin
                                             select q;
                        }
                        result = qry.Select(s => s.aa).Distinct().ToList();

                        var ratePromotores = qryPromotores.Distinct().ToList();
                        var rateNeutros = qryNeutros.Distinct().ToList();
                        var rateDetractores = qryDetractores.Distinct().ToList();

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
                            arbol.Promotores = ratePromotores.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Neutros = rateNeutros.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Detractores = rateDetractores.Count(w => w.re.IdArbol == arbol.Id);
                        }
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.Calificacion:
                        var qryFiltroCalificacion = from re in db.RespuestaEncuesta
                                                    join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                                    join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                                    where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.Calificacion
                                                          && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                                    select new { re, te };

                        var qryCeroCinco = from q in qryFiltroCalificacion
                                           where q.re.ValorRespuesta <= 5
                                           select q;

                        var qrySeisSiete = from q in qryFiltroCalificacion
                                           where q.re.ValorRespuesta >= 6 && q.re.ValorRespuesta <= 7
                                           select q;

                        var qryOchoNueve = from q in qryFiltroCalificacion
                                           where q.re.ValorRespuesta >= 8 && q.re.ValorRespuesta <= 9
                                           select q;

                        var qryDiez = from q in qryFiltroCalificacion
                                      where q.re.ValorRespuesta == 10
                                      select q;

                        if (fechas != null)
                        {
                            DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            qry = qry.Where(w => w.te.FechaMovimiento >= fechaInicio && w.te.FechaMovimiento < fechaFin);

                            qryCeroCinco = from q in qryCeroCinco
                                           where q.te.FechaMovimiento >= fechaInicio
                                                  && q.te.FechaMovimiento < fechaFin
                                           select q;

                            qrySeisSiete = from q in qrySeisSiete
                                           where q.te.FechaMovimiento >= fechaInicio
                                                     && q.te.FechaMovimiento < fechaFin
                                           select q;

                            qryOchoNueve = from q in qryOchoNueve
                                           where q.te.FechaMovimiento >= fechaInicio
                                                 && q.te.FechaMovimiento < fechaFin
                                           select q;
                            qryDiez = from q in qryDiez
                                      where q.te.FechaMovimiento >= fechaInicio
                                            && q.te.FechaMovimiento < fechaFin
                                      select q;
                        }

                        result = qry.Select(s => s.aa).Distinct().ToList();

                        var rateCeroCinco = qryCeroCinco.Distinct().ToList();
                        var rateSeisSiete = qrySeisSiete.Distinct().ToList();
                        var rateOchoNueve = qryOchoNueve.Distinct().ToList();
                        var rateDiez = qryDiez.Distinct().ToList();

                        foreach (ArbolAcceso arbol in result)
                        {
                            db.LoadProperty(arbol, "Area");
                            db.LoadProperty(arbol, "TipoUsuario");
                            db.LoadProperty(arbol, "TipoArbolAcceso");
                            db.LoadProperty(arbol, "InventarioArbolAcceso");
                            db.LoadProperty(arbol, "Nivel1");
                            db.LoadProperty(arbol, "Nivel2");
                            db.LoadProperty(arbol, "Nivel3");
                            db.LoadProperty(arbol, "Nivel4");
                            db.LoadProperty(arbol, "Nivel5");
                            db.LoadProperty(arbol, "Nivel6");
                            db.LoadProperty(arbol, "Nivel7");
                            arbol.Tipificacion = ObtenerTipificacion(arbol.Id);
                            arbol.Nivel = ObtenerNivel(arbol.Id);
                            arbol.NumeroEncuestas = qryFiltroCalificacion.Where(w => w.re.IdArbol == arbol.Id).Select(s => s.re.IdTicket).Distinct().Count();
                            int idEncuesta = (int)arbol.InventarioArbolAcceso.First().IdEncuesta;
                            arbol.NumeroPreguntasEncuesta = db.EncuestaPregunta.Count(c => c.IdEncuesta == idEncuesta);
                            arbol.CeroCinco = rateCeroCinco.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.SeisSiete = rateSeisSiete.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.OchoNueve = rateOchoNueve.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Diez = rateDiez.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.PromedioPonderado = qryFiltroCalificacion.Where(w => w.re.IdArbol == arbol.Id).Average(a => a.re.ValorRespuesta);
                        }
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.CalificacionPesimoMaloRegularBuenoExcelente:
                        var qryFiltroSatisfaccion = from re in db.RespuestaEncuesta
                                                    join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                                    join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                                    where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.CalificacionPesimoMaloRegularBuenoExcelente
                                                          && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                                    select new { re, te };

                        var qryPesimo = from q in qryFiltroSatisfaccion
                                        where q.re.ValorRespuesta == 1
                                        select q;

                        var qryMalo = from q in qryFiltroSatisfaccion
                                      where q.re.ValorRespuesta == 2
                                      select q;

                        var qryRegular = from q in qryFiltroSatisfaccion
                                         where q.re.ValorRespuesta == 3
                                         select q;

                        var qryBueno = from q in qryFiltroSatisfaccion
                                       where q.re.ValorRespuesta == 4
                                       select q;
                        var qryExcelente = from q in qryFiltroSatisfaccion
                                           where q.re.ValorRespuesta == 5
                                           select q;

                        if (fechas != null)
                        {
                            DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            qry = qry.Where(w => w.te.FechaMovimiento >= fechaInicio && w.te.FechaMovimiento < fechaFin);

                            qryPesimo = from q in qryPesimo
                                        where q.te.FechaMovimiento >= fechaInicio
                                               && q.te.FechaMovimiento < fechaFin
                                        select q;

                            qryMalo = from q in qryMalo
                                      where q.te.FechaMovimiento >= fechaInicio
                                                && q.te.FechaMovimiento < fechaFin
                                      select q;

                            qryRegular = from q in qryRegular
                                         where q.te.FechaMovimiento >= fechaInicio
                                               && q.te.FechaMovimiento < fechaFin
                                         select q;
                            qryBueno = from q in qryBueno
                                       where q.te.FechaMovimiento >= fechaInicio
                                             && q.te.FechaMovimiento < fechaFin
                                       select q;
                            qryExcelente = from q in qryExcelente
                                           where q.te.FechaMovimiento >= fechaInicio
                                                 && q.te.FechaMovimiento < fechaFin
                                           select q;
                        }
                        result = qry.Select(s => s.aa).Distinct().ToList();

                        var ratePesimo = qryPesimo.Distinct().ToList();
                        var rateMalo = qryMalo.Distinct().ToList();
                        var rateRegular = qryRegular.Distinct().ToList();
                        var rateBueno = qryBueno.Distinct().ToList();
                        var rateExcelente = qryExcelente.Distinct().ToList();

                        foreach (ArbolAcceso arbol in result)
                        {
                            db.LoadProperty(arbol, "Area");
                            db.LoadProperty(arbol, "TipoUsuario");
                            db.LoadProperty(arbol, "TipoArbolAcceso");
                            db.LoadProperty(arbol, "InventarioArbolAcceso");
                            db.LoadProperty(arbol, "Nivel1");
                            db.LoadProperty(arbol, "Nivel2");
                            db.LoadProperty(arbol, "Nivel3");
                            db.LoadProperty(arbol, "Nivel4");
                            db.LoadProperty(arbol, "Nivel5");
                            db.LoadProperty(arbol, "Nivel6");
                            db.LoadProperty(arbol, "Nivel7");
                            arbol.Tipificacion = ObtenerTipificacion(arbol.Id);
                            arbol.Nivel = ObtenerNivel(arbol.Id);
                            arbol.NumeroEncuestas = qryFiltroSatisfaccion.Where(w => w.re.IdArbol == arbol.Id).Select(s => s.re.IdTicket).Distinct().Count();
                            int idEncuesta = (int)arbol.InventarioArbolAcceso.First().IdEncuesta;
                            arbol.NumeroPreguntasEncuesta = db.EncuestaPregunta.Count(c => c.IdEncuesta == idEncuesta);
                            arbol.Pesimo = ratePesimo.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Malo = rateMalo.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Regular = rateRegular.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Bueno = rateBueno.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.Diez = rateExcelente.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.PromedioPonderado = qryFiltroSatisfaccion.Where(w => w.re.IdArbol == arbol.Id).Average(a => a.re.ValorRespuesta);
                        }
                        break;
                    case (int)BusinessVariables.EnumTipoEncuesta.SiNo:
                        var qryFiltroLogica = from re in db.RespuestaEncuesta
                                              join aa in db.ArbolAcceso on re.IdArbol equals aa.Id
                                              join te in db.TicketEstatus on re.IdTicket equals te.IdTicket
                                              where re.Encuesta.IdTipoEncuesta == (int)BusinessVariables.EnumTipoEncuesta.SiNo
                                                    && te.IdEstatus == (int)BusinessVariables.EnumeradoresKiiniNet.EnumEstatusTicket.Cerrado
                                              select new { re, te };

                        var qrySi = from q in qryFiltroLogica
                                    where q.re.ValorRespuesta == 1
                                    select q;

                        var qryNo = from q in qryFiltroLogica
                                    where q.re.ValorRespuesta == 0
                                    select q;

                        if (fechas != null)
                        {
                            DateTime fechaInicio = DateTime.ParseExact(fechas.Single(s => s.Key == "inicio").Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            DateTime fechaFin = DateTime.ParseExact(fechas.Single(s => s.Key == "fin").Value.AddDays(1).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
                            qry = qry.Where(w => w.te.FechaMovimiento >= fechaInicio && w.te.FechaMovimiento < fechaFin);

                            qrySi = from q in qrySi
                                    where q.te.FechaMovimiento >= fechaInicio
                                           && q.te.FechaMovimiento < fechaFin
                                    select q;

                            qryNo = from q in qryNo
                                    where q.te.FechaMovimiento >= fechaInicio
                                              && q.te.FechaMovimiento < fechaFin
                                    select q;
                        }
                        result = qry.Select(s => s.aa).Distinct().ToList();


                        var rateSi = qrySi.Distinct().ToList();
                        var rateNo = qryNo.Distinct().ToList();

                        foreach (ArbolAcceso arbol in result)
                        {
                            db.LoadProperty(arbol, "Area");
                            db.LoadProperty(arbol, "TipoUsuario");
                            db.LoadProperty(arbol, "TipoArbolAcceso");
                            db.LoadProperty(arbol, "InventarioArbolAcceso");
                            db.LoadProperty(arbol, "Nivel1");
                            db.LoadProperty(arbol, "Nivel2");
                            db.LoadProperty(arbol, "Nivel3");
                            db.LoadProperty(arbol, "Nivel4");
                            db.LoadProperty(arbol, "Nivel5");
                            db.LoadProperty(arbol, "Nivel6");
                            db.LoadProperty(arbol, "Nivel7");
                            arbol.Tipificacion = ObtenerTipificacion(arbol.Id);
                            arbol.Nivel = ObtenerNivel(arbol.Id);
                            arbol.NumeroEncuestas = qryFiltroLogica.Where(w => w.re.IdArbol == arbol.Id).Select(s => s.re.IdTicket).Distinct().Count();
                            int idEncuesta = (int)arbol.InventarioArbolAcceso.First().IdEncuesta;
                            arbol.NumeroPreguntasEncuesta = db.EncuestaPregunta.Count(c => c.IdEncuesta == idEncuesta);
                            arbol.Si = rateSi.Count(w => w.re.IdArbol == arbol.Id);
                            arbol.No = rateNo.Count(w => w.re.IdArbol == arbol.Id);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Obtener Arboles " + ex.Message);
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
                    arbol.Tipificacion = ObtenerTipificacion(arbol.Id);
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
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(w => w.Id == idArbol);
                if (arbol == null) return null;
                db.LoadProperty(arbol, "Nivel1");
                db.LoadProperty(arbol, "Nivel2");
                db.LoadProperty(arbol, "Nivel3");
                db.LoadProperty(arbol, "Nivel4");
                db.LoadProperty(arbol, "Nivel5");
                db.LoadProperty(arbol, "Nivel6");
                db.LoadProperty(arbol, "Nivel7");
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
                if (db.InventarioArbolAcceso.Any(w => w.IdArbolAcceso == idArbol))
                    result = db.InventarioArbolAcceso.First(w => w.IdArbolAcceso == idArbol).Descripcion;
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

        private int ObtenerNivelArbol(ArbolAcceso arbol)
        {
            int result = 0;
            try
            {
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

        public void ActualizarSeccion(int idArbolAcceso, ArbolAcceso arbolAccesoActualizar, string descripcion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(s => s.Id == idArbolAcceso);
                if (arbol != null)
                {

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

        public void ActualizardArbol(int idArbolAcceso, ArbolAcceso arbolAccesoActualizar, string descripcion)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                ArbolAcceso arbol = db.ArbolAcceso.SingleOrDefault(s => s.Id == idArbolAcceso);
                if (arbol != null)
                {
                    arbol.Descripcion = arbolAccesoActualizar.Descripcion;
                    arbol.Publico = arbolAccesoActualizar.Publico;
                    arbol.InventarioArbolAcceso.First().Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion;
                    arbol.IdArea = arbolAccesoActualizar.IdArea;

                    //Grupo acceso
                    #region Grupos Acceso

                    int idGpoAccesoActualizar = arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.First(s => s.IdRol == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).IdGrupoUsuario;
                    int idGpoAccesoExistente = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Any(s => s.IdRol == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte) ? arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.First(s => s.IdRol == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).IdGrupoUsuario : 0;
                    if (idGpoAccesoExistente != idGpoAccesoActualizar)
                    {
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.IdGrupoUsuario == idGpoAccesoExistente).ToList().ForEach(d => db.GrupoUsuarioInventarioArbol.DeleteObject(d));
                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.AddRange(arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.IdGrupoUsuario == idGpoAccesoActualizar));
                    }

                    #endregion GruposAgente
                    if (arbol.IdTipoArbolAcceso != (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                    {
                        arbol.IdTipoArbolAcceso = arbolAccesoActualizar.IdTipoArbolAcceso;

                        //Grupo Agente
                        #region Grupo Agente

                        int idGpoAgenteActualizar = arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.First(s => s.IdRol == (int)BusinessVariables.EnumTiposGrupos.Agente).IdGrupoUsuario;
                        int idGpoAgenteExistente = arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Any(s => s.IdRol == (int)BusinessVariables.EnumTiposGrupos.Agente) ? arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.First(s => s.IdRol == (int)BusinessVariables.EnumTiposGrupos.Agente).IdGrupoUsuario : 0;
                        if (idGpoAgenteExistente == idGpoAgenteActualizar)
                        {
                            if (db.SubGrupoUsuario.Count(c => c.IdGrupoUsuario == idGpoAgenteActualizar) != arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Count(c => c.IdGrupoUsuario == idGpoAgenteActualizar))
                            {
                                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.IdGrupoUsuario == idGpoAgenteExistente).ToList().ForEach(d => db.GrupoUsuarioInventarioArbol.DeleteObject(d));
                                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.AddRange(arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.IdGrupoUsuario == idGpoAgenteActualizar));
                            }
                        }
                        else
                        {
                            arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(s => s.IdGrupoUsuario == idGpoAgenteExistente).ToList().ForEach(d => db.GrupoUsuarioInventarioArbol.DeleteObject(d));
                            arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.AddRange(arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.IdGrupoUsuario == idGpoAgenteActualizar));
                        }

                        #endregion Grupo Agente



                        #region Sla
                        if (arbolAccesoActualizar.InventarioArbolAcceso.First().Sla == null) //Borra sla
                        {
                            if (arbol.InventarioArbolAcceso.First().IdSla != null)
                            {
                                db.Sla.DeleteObject(arbol.InventarioArbolAcceso.First().Sla);
                                arbol.InventarioArbolAcceso.First().IdSla = null;
                                arbol.TiempoInformeArbol.ForEach(d => db.TiempoInformeArbol.DeleteObject(d));
                                arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.IdRol == (int)BusinessVariables.EnumRoles.Notificaciones).ToList().ForEach(d => db.GrupoUsuarioInventarioArbol.DeleteObject(d));
                            }
                        }
                        else //Actualizacion sla y notificaciones
                        {
                            if ((arbol.InventarioArbolAcceso.First().IdSla == null))
                            {
                                arbol.InventarioArbolAcceso.First().Sla = arbolAccesoActualizar.InventarioArbolAcceso.First().Sla;
                            }
                            else  //Actualiza SLA si tiene
                            {
                                arbol.InventarioArbolAcceso.First().Sla.Dias = arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Dias;
                                arbol.InventarioArbolAcceso.First().Sla.Horas = arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Horas;
                                arbol.InventarioArbolAcceso.First().Sla.Minutos = arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Minutos;
                                arbol.InventarioArbolAcceso.First().Sla.Segundos = arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.Segundos;
                                arbol.InventarioArbolAcceso.First().Sla.TiempoHoraProceso = arbolAccesoActualizar.InventarioArbolAcceso.First().Sla.TiempoHoraProceso;

                                if (arbolAccesoActualizar.TiempoInformeArbol == null)
                                {
                                    arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.IdRol == (int)BusinessVariables.EnumRoles.Notificaciones).ToList().ForEach(d => db.GrupoUsuarioInventarioArbol.DeleteObject(d));
                                }
                                else
                                {
                                    if (arbolAccesoActualizar.TiempoInformeArbol.Count <= 0)
                                    {
                                        arbol.TiempoInformeArbol.ForEach(d => db.TiempoInformeArbol.DeleteObject(d));
                                        arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(w => w.IdRol == (int)BusinessVariables.EnumRoles.Notificaciones).ToList().ForEach(d => db.GrupoUsuarioInventarioArbol.DeleteObject(d));
                                    }
                                    else
                                    {
                                        if (arbol.TiempoInformeArbol == null)
                                        {
                                            if (arbol.TiempoInformeArbol == null)
                                                arbol.TiempoInformeArbol = new List<TiempoInformeArbol>();
                                        }
                                        if (arbol.TiempoInformeArbol != null && arbol.TiempoInformeArbol.Count <= 0)
                                        {
                                            arbol.TiempoInformeArbol.Add(new TiempoInformeArbol());
                                        }

                                        arbol.TiempoInformeArbol.First().IdTipoGrupo = arbolAccesoActualizar.TiempoInformeArbol.First().IdTipoGrupo;
                                        arbol.TiempoInformeArbol.First().IdGrupoUsuario = arbolAccesoActualizar.TiempoInformeArbol.First().IdGrupoUsuario;
                                        arbol.TiempoInformeArbol.First().Dias = arbolAccesoActualizar.TiempoInformeArbol.First().Dias;
                                        arbol.TiempoInformeArbol.First().Horas = arbolAccesoActualizar.TiempoInformeArbol.First().Horas;
                                        arbol.TiempoInformeArbol.First().Minutos = arbolAccesoActualizar.TiempoInformeArbol.First().Minutos;
                                        arbol.TiempoInformeArbol.First().Segundos = arbolAccesoActualizar.TiempoInformeArbol.First().Segundos;
                                        arbol.TiempoInformeArbol.First().TiempoNotificacion = arbolAccesoActualizar.TiempoInformeArbol.First().TiempoNotificacion;
                                        arbol.TiempoInformeArbol.First().IdTipoNotificacion = arbolAccesoActualizar.TiempoInformeArbol.First().IdTipoNotificacion;
                                        arbol.TiempoInformeArbol.First().AntesVencimiento = arbolAccesoActualizar.TiempoInformeArbol.First().AntesVencimiento;
                                        if (arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Any(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Notificaciones))
                                            arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.First(s => s.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Notificaciones).IdGrupoUsuario = arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.First(f => f.IdRol == (int)BusinessVariables.EnumRoles.Notificaciones).IdGrupoUsuario;
                                        else
                                            arbol.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.AddRange(arbolAccesoActualizar.InventarioArbolAcceso.First().GrupoUsuarioInventarioArbol.Where(f => f.IdRol == (int)BusinessVariables.EnumRoles.Notificaciones));
                                    }
                                }
                            }

                        }
                        #endregion Sla
                    }
                    else if (arbol.IdTipoArbolAcceso == (int)BusinessVariables.EnumTipoArbol.ConsultarInformacion)
                        arbol.Evaluacion = arbolAccesoActualizar.Evaluacion;

                    int nivelArbolBase = ObtenerNivelArbol(arbol);
                    int nivelArbolActualizar = ObtenerNivelArbol(arbolAccesoActualizar);
                    if (nivelArbolBase != nivelArbolActualizar)
                    {
                        switch (nivelArbolBase)
                        {
                            case 1:
                                db.Nivel1.DeleteObject(arbol.Nivel1);
                                break;
                            case 2:
                                db.Nivel2.DeleteObject(arbol.Nivel2);
                                break;
                            case 3:
                                db.Nivel3.DeleteObject(arbol.Nivel3);
                                break;
                            case 4:
                                db.Nivel4.DeleteObject(arbol.Nivel4);
                                break;
                            case 5:
                                db.Nivel5.DeleteObject(arbol.Nivel5);
                                break;
                            case 6:
                                db.Nivel6.DeleteObject(arbol.Nivel6);
                                break;
                            case 7:
                                db.Nivel7.DeleteObject(arbol.Nivel7);
                                break;
                        }
                        string descripcionFinal;
                        bool existe;
                        switch (nivelArbolActualizar)
                        {
                            case 1:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                existe = db.Nivel1.Join(db.ArbolAcceso, n1 => n1.Id, aa => aa.IdNivel1, (n1, aa) => new { n1, aa }).Any(@t => @t.n1.Descripcion == descripcionFinal && @t.n1.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel1 = new Nivel1();
                                arbol.Nivel1.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel1.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel1.Habilitado = true;
                                arbol.IdNivel2 = null;
                                arbol.IdNivel3 = null;
                                arbol.IdNivel4 = null;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 2:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                existe = db.Nivel2.Join(db.ArbolAcceso, n2 => n2.Id, aa => aa.IdNivel2, (n2, aa) => new { n2, aa }).Any(@t => @t.n2.Descripcion == descripcionFinal && @t.n2.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel2 = new Nivel2();
                                arbol.Nivel2.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel2.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel2.Habilitado = true;
                                arbol.IdNivel3 = null;
                                arbol.IdNivel4 = null;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 3:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                existe = db.Nivel3.Join(db.ArbolAcceso, n3 => n3.Id, aa => aa.IdNivel3, (n3, aa) => new { n3, aa }).Any(@t => @t.n3.Descripcion == descripcionFinal && @t.n3.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel3 = new Nivel3();
                                arbol.Nivel3.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel3.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel3.Habilitado = true;
                                arbol.IdNivel4 = null;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 4:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                existe = db.Nivel4.Join(db.ArbolAcceso, n4 => n4.Id, aa => aa.IdNivel4, (n4, aa) => new { n4, aa }).Any(@t => @t.n4.Descripcion == descripcionFinal && @t.n4.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel4 = new Nivel4();
                                arbol.Nivel4.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel4.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel4.Habilitado = true;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 5:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel5 = new Nivel5();
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                arbol.IdNivel4 = arbolAccesoActualizar.IdNivel4;
                                existe = db.Nivel5.Join(db.ArbolAcceso, n5 => n5.Id, aa => aa.IdNivel5, (n5, aa) => new { n5, aa }).Any(@t => @t.n5.Descripcion == descripcionFinal && @t.n5.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel5.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel5.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel5.Habilitado = true;

                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 6:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                arbol.IdNivel4 = arbolAccesoActualizar.IdNivel4;
                                arbol.IdNivel5 = arbolAccesoActualizar.IdNivel5;
                                existe = db.Nivel6.Join(db.ArbolAcceso, n6 => n6.Id, aa => aa.IdNivel6, (n6, aa) => new { n6, aa }).Any(@t => @t.n6.Descripcion == descripcionFinal && @t.n6.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4
                                && @t.aa.IdNivel5 == arbol.IdNivel5);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel6 = new Nivel6();

                                arbol.Nivel6.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel6.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel6.Habilitado = true;

                                arbol.IdNivel7 = null;
                                break;
                            case 7:
                                descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                arbol.IdNivel4 = arbolAccesoActualizar.IdNivel4;
                                arbol.IdNivel5 = arbolAccesoActualizar.IdNivel5;
                                arbol.IdNivel6 = arbolAccesoActualizar.IdNivel6;
                                existe = db.Nivel7.Join(db.ArbolAcceso, n7 => n7.Id, aa => aa.IdNivel7, (n7, aa) => new { n7, aa }).Any(@t => @t.n7.Descripcion == descripcionFinal && @t.n7.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4
                                && @t.aa.IdNivel5 == arbol.IdNivel5
                                && @t.aa.IdNivel6 == arbol.IdNivel6);

                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel7 = new Nivel7();
                                arbol.Nivel7.Descripcion = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                                arbol.Nivel7.IdTipoUsuario = arbol.IdTipoUsuario;
                                arbol.Nivel7.Habilitado = true;
                                break;
                        }
                    }
                    else if (nivelArbolBase == nivelArbolActualizar)
                    {
                        string descripcionFinal = arbolAccesoActualizar.InventarioArbolAcceso.First().Descripcion.Trim();
                        bool existe;
                        switch (nivelArbolBase)
                        {
                            case 1:
                                existe = db.Nivel1.Join(db.ArbolAcceso, n1 => n1.Id, aa => aa.IdNivel1, (n1, aa) => new { n1, aa }).Any(@t => @t.n1.Descripcion == descripcionFinal && @t.n1.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel1.Descripcion = descripcionFinal;
                                arbol.IdNivel2 = null;
                                arbol.IdNivel3 = null;
                                arbol.IdNivel4 = null;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 2:
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                existe = db.Nivel2.Join(db.ArbolAcceso, n2 => n2.Id, aa => aa.IdNivel2, (n2, aa) => new { n2, aa }).Any(@t => @t.n2.Descripcion == descripcionFinal && @t.n2.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel2.Descripcion = descripcionFinal;
                                arbol.IdNivel3 = null;
                                arbol.IdNivel4 = null;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 3:
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                existe = db.Nivel3.Join(db.ArbolAcceso, n3 => n3.Id, aa => aa.IdNivel3, (n3, aa) => new { n3, aa }).Any(@t => @t.n3.Descripcion == descripcionFinal && @t.n3.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel3.Descripcion = descripcionFinal;
                                arbol.IdNivel4 = null;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 4:
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                existe = db.Nivel4.Join(db.ArbolAcceso, n4 => n4.Id, aa => aa.IdNivel4, (n4, aa) => new { n4, aa }).Any(@t => @t.n4.Descripcion == descripcionFinal && @t.n4.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel4.Descripcion = descripcionFinal;
                                arbol.IdNivel5 = null;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 5:

                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                arbol.IdNivel4 = arbolAccesoActualizar.IdNivel4;
                                existe = db.Nivel5.Join(db.ArbolAcceso, n5 => n5.Id, aa => aa.IdNivel5, (n5, aa) => new { n5, aa }).Any(@t => @t.n5.Descripcion == descripcionFinal && @t.n5.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel5.Descripcion = descripcionFinal;
                                arbol.IdNivel6 = null;
                                arbol.IdNivel7 = null;
                                break;
                            case 6:
                                existe = db.Nivel6.Join(db.ArbolAcceso, n6 => n6.Id, aa => aa.IdNivel6, (n6, aa) => new { n6, aa }).Any(@t => @t.n6.Descripcion == descripcionFinal && @t.n6.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4
                                && @t.aa.IdNivel5 == arbol.IdNivel5);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                arbol.IdNivel4 = arbolAccesoActualizar.IdNivel4;
                                arbol.IdNivel5 = arbolAccesoActualizar.IdNivel5;
                                arbol.Nivel6.Descripcion = descripcionFinal;
                                arbol.IdNivel7 = null;
                                break;
                            case 7:

                                arbol.IdNivel1 = arbolAccesoActualizar.IdNivel1;
                                arbol.IdNivel2 = arbolAccesoActualizar.IdNivel2;
                                arbol.IdNivel3 = arbolAccesoActualizar.IdNivel3;
                                arbol.IdNivel4 = arbolAccesoActualizar.IdNivel4;
                                arbol.IdNivel5 = arbolAccesoActualizar.IdNivel5;
                                arbol.IdNivel6 = arbolAccesoActualizar.IdNivel6;
                                existe = db.Nivel7.Join(db.ArbolAcceso, n7 => n7.Id, aa => aa.IdNivel7, (n7, aa) => new { n7, aa }).Any(@t => @t.n7.Descripcion == descripcionFinal && @t.n7.IdTipoUsuario == arbol.IdTipoUsuario && @t.aa.EsTerminal && @t.aa.Id != arbol.Id && @t.aa.IdArea == arbol.IdArea
                                    && @t.aa.IdNivel1 == arbol.IdNivel1
                                && @t.aa.IdNivel2 == arbol.IdNivel2
                                && @t.aa.IdNivel3 == arbol.IdNivel3
                                && @t.aa.IdNivel4 == arbol.IdNivel4
                                && @t.aa.IdNivel5 == arbol.IdNivel5
                                && @t.aa.IdNivel6 == arbol.IdNivel6);
                                if (existe)
                                    throw new Exception("Esta opcion ya se encuentra registrada");
                                arbol.Nivel7.Descripcion = descripcionFinal;
                                break;
                        }
                    }

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
                                              where ug.IdUsuario == idUsuario && guia.IdRol == (int)BusinessVariables.EnumRoles.AccesoCentroSoporte
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
                    List<int> lstGposSolicita = usuarioSolicita.UsuarioGrupo.Where(w => w.GrupoUsuario.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.AccesoCentroSoporte).Select(s => s.IdGrupoUsuario).ToList();
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
                    foreach (ArbolAcceso arbol in db.ArbolAcceso.Where(w => arbolesServicioProblema.Contains(w.Id) && w.EsTerminal).OrderBy(o => o.Id).Skip(skip).Take(pagesize).Distinct())
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
