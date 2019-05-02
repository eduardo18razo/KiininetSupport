using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Usuario;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessDiasHorario : IDisposable
    {
        private readonly bool _proxy;
        public BusinessDiasHorario(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        {

        }
        #region Horarios
        public List<Horario> ObtenerHorarioSistema(bool insertarSeleccion)
        {
            List<Horario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Horario.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Horario
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

        public List<Horario> ObtenerHorarioConsulta(string filtro)
        {
            List<Horario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                filtro = filtro.ToLower().Trim();
                result = db.Horario.Where(w => w.Descripcion.ToLower().Contains(filtro)).OrderBy(o => o.Descripcion).ToList();
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

        public void CrearHorario(Horario horario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                horario.Descripcion = horario.Descripcion.Trim();
                horario.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                horario.Habilitado = true;
                if (db.Horario.Any(a => a.Descripcion == horario.Descripcion))
                    throw new Exception("Ya existe un horario con esta descripción");
                db.Horario.AddObject(horario);
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

        public void ActualizarHorario(Horario horario)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                db.ContextOptions.ProxyCreationEnabled = true;
                Horario dbHorario = db.Horario.SingleOrDefault(s => s.Id == horario.Id);
                if (dbHorario != null)
                {
                    dbHorario.Descripcion = horario.Descripcion.Trim();
                    dbHorario.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    if (db.Horario.Any(a => a.Descripcion == horario.Descripcion && a.Id != horario.Id))
                        throw new Exception("Ya existe un horario con esta descripción");
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

        public Horario ObtenerHorarioById(int idHorario)
        {
            Horario result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Horario.SingleOrDefault(s => s.Id == idHorario);
                if (result != null)
                    db.LoadProperty(result, "HorarioDetalle");
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

        public void HabilitarHorario(int idHorario, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Horario inf = db.Horario.SingleOrDefault(w => w.Id == idHorario);
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
        #endregion Horarios

        #region Dias Feriados

        public List<DiasFeriados> ObtenerDiasFeriadosConsulta(string filtro)
        {
            List<DiasFeriados> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                filtro = filtro.ToLower().Trim();
                result = db.DiasFeriados.Where(w => w.Descripcion.ToLower().Contains(filtro)).OrderBy(o => o.Descripcion).ToList();
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
        public List<DiaFeriado> ObtenerDiasFeriados(bool insertarSeleccion)
        {
            List<DiaFeriado> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiaFeriado.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new DiaFeriado
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

        public List<DiaFestivoDefault> ObtenerDiasDefault(bool insertarSeleccion)
        {
            List<DiaFestivoDefault> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiaFestivoDefault.Where(w => w.Habilitado).OrderBy(o => o.Mes).ThenBy(t => t.Dia).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new DiaFestivoDefault
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

        public void AgregarDiaFeriado(DiaFeriado dia)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {

                db.ContextOptions.ProxyCreationEnabled = _proxy;
                dia.Descripcion = dia.Descripcion.Trim();
                dia.Habilitado = true;
                if (dia.Id == 0)
                {
                    if (db.DiaFeriado.Any(a => a.Fecha == dia.Fecha))
                        throw new Exception("Esta fecha ya fue registrada anteriormente.");
                    if (db.DiaFeriado.Any(a => a.Descripcion == dia.Descripcion.Trim()))
                        throw new Exception("Esta Descripción ya fue registrada anteriormente.");
                    dia.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    db.DiaFeriado.AddObject(dia);
                }
                else
                {
                    if (db.DiaFeriado.Any(a => a.Fecha == dia.Fecha && a.Id != dia.Id))
                        throw new Exception("Esta fecha ya fue registrada anteriormente.");
                    if (db.DiaFeriado.Any(a => a.Descripcion == dia.Descripcion.Trim() && a.Id != dia.Id))
                        throw new Exception("Esta Descripción ya fue registrada anteriormente.");
                    DiaFeriado diaF = db.DiaFeriado.SingleOrDefault(s => s.Id == dia.Id);
                    if (diaF != null)
                    {
                        diaF.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                        diaF.IdUsuarioModifico = dia.IdUsuarioAlta;
                        foreach (DiasFeriadosDetalle detalle in db.DiasFeriadosDetalle.Where(w => w.Dia == diaF.Fecha))
                        {
                            detalle.Descripcion = dia.Descripcion;
                            detalle.Dia = dia.Fecha;
                        }
                        diaF.Descripcion = dia.Descripcion;
                        diaF.Fecha = dia.Fecha;

                    }
                }
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

        public DiaFeriado ObtenerDiaByFecha(DateTime fecha)
        {
            DiaFeriado result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiaFeriado.SingleOrDefault(w => w.Fecha == fecha);
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

        public DiaFeriado ObtenerDiaFeriado(int id)
        {
            DiaFeriado result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiaFeriado.SingleOrDefault(w => w.Id == id && w.Habilitado);
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

        public void ActualizarDiasFestivos(DiasFeriados item)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                item.Descripcion = item.Descripcion;
                if (db.DiasFeriados.Any(a => a.Descripcion == item.Descripcion && a.Id != item.Id))
                    throw new Exception("Ya Existe un grupo de Dias Con este Nombre.");
                DiasFeriados diadb = db.DiasFeriados.SingleOrDefault(s => s.Id == item.Id);
                if (diadb != null)
                {
                    diadb.Descripcion = item.Descripcion;
                    diadb.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    
                    List<DiasFeriadosDetalle> diasEliminar = db.DiasFeriadosDetalle.Where(w => w.IdDiasFeriados == diadb.Id).ToList();
                    foreach (DiasFeriadosDetalle detalle in diasEliminar)
                    {
                        db.DiasFeriadosDetalle.DeleteObject(detalle);
                    }
                    diadb.DiasFeriadosDetalle = item.DiasFeriadosDetalle;

                    List<int> subgpos = db.DiaFestivoSubGrupo.Where(w => w.IdDiasFeriados == diadb.Id).Select(s => s.IdSubGrupoUsuario).ToList();
                    List<int> gpos = db.SubGrupoUsuario.Where(w => subgpos.Contains(w.Id)).Select(s => s.IdGrupoUsuario).Distinct().ToList();

                    List<DiaFestivoSubGrupo> diasSgpoEliminar = db.DiaFestivoSubGrupo.Where(w => w.IdDiasFeriados == diadb.Id).ToList();
                    foreach (DiaFestivoSubGrupo diaEliminar in diasSgpoEliminar)
                    {
                        db.DiaFestivoSubGrupo.DeleteObject(diaEliminar);
                    }

                    foreach (DiasFeriadosDetalle detalleFeriado in item.DiasFeriadosDetalle)
                    {
                        foreach (SubGrupoUsuario subGrupoUsuario in db.SubGrupoUsuario.Where(w => gpos.Contains(w.IdGrupoUsuario)))
                        {
                            DiaFestivoSubGrupo diasubgpo = new DiaFestivoSubGrupo
                            {
                                IdSubGrupoUsuario = subGrupoUsuario.Id,
                                IdDiasFeriados = diadb.Id,
                                Fecha = detalleFeriado.Dia,
                                Descripcion = detalleFeriado.Descripcion
                            };
                            db.DiaFestivoSubGrupo.AddObject(diasubgpo);
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
        public void CrearDiasFestivos(DiasFeriados item)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                item.Descripcion = item.Descripcion;
                if (db.DiasFeriados.Any(a => a.Descripcion == item.Descripcion))
                    throw new Exception("Ya Existe un grupo de Dias Con este Nombre.");
                item.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                item.Descripcion = item.Descripcion.Trim();
                item.Habilitado = true;
                foreach (DiasFeriadosDetalle detalle in item.DiasFeriadosDetalle)
                {
                    detalle.Descripcion = detalle.Descripcion.Trim();
                    detalle.Habilitado = true;
                }
                db.DiasFeriados.AddObject(item);
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

        public List<DiasFeriados> ObtenerDiasFeriadosUser(bool insertarSeleccion)
        {
            List<DiasFeriados> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiasFeriados.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new DiasFeriados
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
        public DiasFeriados ObtenerDiasFeriadosUserById(int id)
        {
            DiasFeriados result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.DiasFeriados.SingleOrDefault(w => w.Id == id);
                if (result != null)
                    db.LoadProperty(result, "DiasFeriadosDetalle");
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

        public void HabilitarDiasFestivos(int idDiasFestivos, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                DiasFeriados inf = db.DiasFeriados.SingleOrDefault(w => w.Id == idDiasFestivos);
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
        #endregion Dias Feriados
    }
}
