using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessNota : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessNota(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void CrearNotaGeneral(NotaGeneral notaGeneral)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                notaGeneral.Nombre = notaGeneral.Nombre.Trim();
                notaGeneral.Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                notaGeneral.Habilitado = true;
                if (notaGeneral.Compartida)
                {
                    notaGeneral.Aprobada = true;
                    notaGeneral.NotaGeneralGrupo = new List<NotaGeneralGrupo>();
                    foreach (GrupoUsuario grupoUsuario in new BusinessGrupoUsuario().ObtenerGruposByIdUsuario(notaGeneral.IdUsuario, false).Where(w => w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.Agente || w.IdTipoGrupo == (int)BusinessVariables.EnumTiposGrupos.ResponsableDeOperación))
                    {
                        notaGeneral.NotaGeneralGrupo.Add(new NotaGeneralGrupo
                        {
                            IdGrupoUsuario = grupoUsuario.Id,
                            Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture),
                            Habilitado = true
                        });
                    }

                }
                if (notaGeneral.Id == 0)
                    db.NotaGeneral.AddObject(notaGeneral);
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

        public void CrearNotaOpcionUsuario(NotaOpcionUsuario notaOpcionUsuario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                notaOpcionUsuario.Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                notaOpcionUsuario.Habilitado = true;
                if (notaOpcionUsuario.Id == 0)
                    db.NotaOpcionUsuario.AddObject(notaOpcionUsuario);
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

        public void CrearNotaOpcionGrupo(NotaOpcionGrupo notaOpcionGrupo)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                notaOpcionGrupo.Fecha = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                notaOpcionGrupo.Habilitado = true;
                if (notaOpcionGrupo.Id == 0)
                    db.NotaOpcionGrupo.AddObject(notaOpcionGrupo);
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

        public List<HelperNotasUsuario> ObtenerNotasUsuario(int idUsuario, bool insertarSeleccion)
        {
            List<HelperNotasUsuario> result = new List<HelperNotasUsuario>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                List<NotaGeneral> lstGeneral = db.NotaGeneral.Where(w => w.IdUsuario == idUsuario).ToList();
                foreach (NotaGeneral nota in lstGeneral)
                {
                    db.LoadProperty(nota, "TipoNota");
                    result.Add(new HelperNotasUsuario
                    {
                        Id = nota.Id,
                        IdTipoNota = nota.IdTipoNota,
                        TipoNota = nota.TipoNota.Descripcion,
                        Nombre = nota.Nombre,
                        Contenido = nota.Contenido
                    });
                }

                List<int> grupos = new BusinessGrupoUsuario().ObtenerGruposByIdUsuario(idUsuario, false).Select(s => s.Id).Distinct().ToList();

                List<NotaGeneral> lstOpcion = db.NotaGeneralGrupo.Where(w => grupos.Contains(w.IdGrupoUsuario)).Select(s => s.NotaGeneral).Distinct().ToList();
                foreach (NotaGeneral nota in lstOpcion)
                {
                    db.LoadProperty(nota, "TipoNota");
                    result.Add(new HelperNotasUsuario
                    {
                        Id = nota.Id,
                        IdTipoNota = nota.IdTipoNota,
                        TipoNota = nota.TipoNota.Descripcion,
                        Nombre = nota.Nombre,
                        Contenido = nota.Contenido
                    });
                }

                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new HelperNotasUsuario
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Nombre = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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

        public List<HelperNotasOpcion> ObtenerNotasOpcion(int idUsuario, bool insertarSeleccion)
        {
            List<HelperNotasOpcion> result = new List<HelperNotasOpcion>();
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                List<int> grupos = new BusinessGrupoUsuario().ObtenerGruposByIdUsuario(idUsuario, false).Select(s => s.Id).Distinct().ToList();
                List<int> lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByGrupos(grupos).Select(s => s.Id).Distinct().ToList();
                List<NotaOpcionGrupo> lstGeneral = db.NotaOpcionGrupo.Where(w => w.IdUsuario == idUsuario && lstArboles.Contains(w.IdArbolAcceso)).ToList();
                foreach (NotaOpcionGrupo nota in lstGeneral)
                {
                    db.LoadProperty(nota, "TipoNota");
                    result.Add(new HelperNotasOpcion
                    {
                        Id = nota.Id,
                        IdTipoNota = nota.IdTipoNota,
                        TipoNota = nota.TipoNota.Descripcion,
                        IdArbol = nota.IdArbolAcceso,
                        Nombre = nota.Nombre,
                        Contenido = nota.Contenido
                    });
                }
                lstArboles = new BusinessArbolAcceso().ObtenerArbolesAccesoByIdUsuario(idUsuario).Select(s => s.Id).Distinct().ToList();
                List<NotaOpcionUsuario> lstOpcion = db.NotaOpcionUsuario.Where(w => w.IdUsuario == idUsuario && lstArboles.Contains(w.IdArbolAcceso)).ToList();
                foreach (NotaOpcionUsuario nota in lstOpcion)
                {
                    db.LoadProperty(nota, "TipoNota");
                    result.Add(new HelperNotasOpcion
                    {
                        Id = nota.Id,
                        IdTipoNota = nota.IdTipoNota,
                        TipoNota = nota.TipoNota.Descripcion,
                        IdArbol = nota.IdArbolAcceso,
                        Nombre = nota.Nombre,
                        Contenido = nota.Contenido
                    });
                }

                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new HelperNotasOpcion
                        {
                            Id = BusinessVariables.ComboBoxCatalogo.ValueSeleccione,
                            Nombre = BusinessVariables.ComboBoxCatalogo.DescripcionSeleccione
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

        public List<HelperNotasUsuario> ObtenerNotasGrupo(int idUsuario)
        {
            return new List<HelperNotasUsuario>();
        }
    }
}
