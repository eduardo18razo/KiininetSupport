using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Operacion
{
    public class BusinessEncuesta : IDisposable
    {
        private bool _proxy;
        public void Dispose()
        {

        }
        public BusinessEncuesta(bool proxy = false)
        {
            _proxy = proxy;
        }

        public List<Encuesta> ObtenerEncuestas(bool insertarSeleccion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Encuesta.Where(w => w.Habilitado).OrderBy(o => o.Descripcion).ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Encuesta
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

        public Encuesta ObtenerEncuestaById(int idEncuesta)
        {
            Encuesta result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.Encuesta.SingleOrDefault(w => w.Id == idEncuesta);
                if (result != null)
                {
                    db.LoadProperty(result, "TipoEncuesta");
                    db.LoadProperty(result, "EncuestaPregunta");
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

        public List<Encuesta> ObtenerEncuestaByGrupos(List<int> grupos, bool insertarSeleccion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from t in db.Ticket
                          join tgu in db.TicketGrupoUsuario on t.Id equals tgu.IdTicket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          where t.EncuestaRespondida
                          && grupos.Contains(tgu.IdGrupoUsuario)
                          select new { t, e };
                result = qry.Select(s => s.e).Distinct().ToList();
                foreach (Encuesta encuesta in result)
                {
                    db.LoadProperty(encuesta, "TipoEncuesta");
                    db.LoadProperty(encuesta, "EncuestaPregunta");
                }
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Encuesta
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

        public Encuesta ObtenerEncuestaByIdTicket(int idTicket)
        {
            Encuesta result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                Ticket tk = db.Ticket.Single(s => s.Id == idTicket);
                if (tk != null)
                {
                    if(tk.EncuestaRespondida)
                        throw new Exception("Esta encuesta ya ha sido contestada anteriormente.");
                    db.LoadProperty(tk, "Encuesta");
                    result = tk.Encuesta;
                    if (result != null)
                    {
                        db.LoadProperty(result, "TipoEncuesta");
                        db.LoadProperty(result, "EncuestaPregunta");
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
        public Encuesta ObtenerEncuestaByIdConsulta(int idConsulta)
        {
            Encuesta result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                ArbolAcceso ac = db.ArbolAcceso.Single(s => s.Id == idConsulta);
                if (ac != null)
                {
                    db.LoadProperty(ac, "InventarioArbolAcceso");
                    foreach (InventarioArbolAcceso inventarioArbolAcceso in ac.InventarioArbolAcceso)
                    {
                        db.LoadProperty(inventarioArbolAcceso, "Encuesta");
                    }
                    result = ac.InventarioArbolAcceso.First().Encuesta;
                    if (result != null)
                    {
                        db.LoadProperty(result, "TipoEncuesta");
                        db.LoadProperty(result, "EncuestaPregunta");
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

        public void GuardarEncuesta(Encuesta encuesta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;

                encuesta.Titulo = encuesta.Titulo.Trim();
                encuesta.TituloCliente = encuesta.TituloCliente.Trim();
                encuesta.Descripcion = encuesta.Descripcion.Trim();
                encuesta.FechaAlta = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                //TODO: Cambiar habilitado por el embebido
                encuesta.Sistema = false;
                encuesta.Habilitado = true;
                if (db.Encuesta.Any(a => a.Titulo == encuesta.Titulo && a.IdTipoEncuesta == encuesta.IdTipoEncuesta))
                    throw new Exception("Esta encuesta ya Existe.");
                if (encuesta.Id == 0)
                    db.Encuesta.AddObject(encuesta);
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

        public void ActualizarEncuesta(Encuesta encuesta)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.LazyLoadingEnabled = true;
                Encuesta encuestaActualizar = db.Encuesta.SingleOrDefault(s => s.Id == encuesta.Id);
                if (encuestaActualizar != null)
                {
                    encuestaActualizar.Titulo = encuesta.Titulo.Trim();
                    encuestaActualizar.TituloCliente = encuesta.TituloCliente.Trim();
                    encuestaActualizar.Descripcion = encuesta.Descripcion.Trim();
                    encuestaActualizar.IdUsuarioModifico = encuesta.IdUsuarioModifico;
                    encuestaActualizar.FechaModificacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), "yyyy-MM-dd HH:mm:ss:fff", CultureInfo.InvariantCulture);
                    if (db.Encuesta.Any(a => a.Titulo == encuesta.Titulo && a.IdTipoEncuesta == encuesta.IdTipoEncuesta && a.Id != encuesta.Id))
                        throw new Exception("Esta encuesta ya Existe.");
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

        public List<Encuesta> Consulta(string descripcion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                IQueryable<Encuesta> qry = db.Encuesta;
                if (descripcion.Trim() != string.Empty)
                    qry = qry.Where(w => w.Titulo.ToLower().Contains(descripcion));
                result = qry.ToList();
                foreach (Encuesta encuesta in result)
                {
                    db.LoadProperty(encuesta, "TipoEncuesta");
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

        public List<HelperEncuesta> ObtenerEncuestasPendientesUsuario(int idUsuario)
        {
            List<HelperEncuesta> result = null;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                List<Ticket> lstEncuestas = db.Ticket.Where(w => w.EncuestaRespondida == false && w.IdEncuesta != null && w.IdUsuarioLevanto == idUsuario).ToList();
                if (lstEncuestas.Count > 0)
                {
                    result = new List<HelperEncuesta>();
                    foreach (Ticket ticket in lstEncuestas)
                    {

                        db.LoadProperty(ticket, "Encuesta");
                        if (ticket.IdEncuesta != null)
                        {
                            HelperEncuesta hEncuesta = new HelperEncuesta
                            {
                                NumeroTicket = ticket.Id,
                                IdEncuesta = (int)ticket.IdEncuesta,
                                Tipificacion = new BusinessArbolAcceso().ObtenerTipificacion(ticket.IdArbolAcceso),
                                Descripcion = ticket.Encuesta.Descripcion,
                                Respondida = ticket.EncuestaRespondida,
                            };
                            result.Add(hEncuesta);
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

        public void HabilitarEncuesta(int idencuesta, bool habilitado)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Encuesta encuesta = db.Encuesta.SingleOrDefault(w => w.Id == idencuesta);
                if (encuesta != null) encuesta.Habilitado = habilitado;
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

        public void ContestaEncuesta(List<RespuestaEncuesta> encuestaRespondida, int idTicket)
        {
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                Ticket t = db.Ticket.SingleOrDefault(s => s.Id == idTicket);
                if (t != null && !t.EncuestaRespondida)
                {
                    foreach (RespuestaEncuesta respuesta in encuestaRespondida)
                    {
                        respuesta.IdArbol = t.IdArbolAcceso;
                        respuesta.Ponderacion = (respuesta.Ponderacion * db.EncuestaPregunta.Single(s => s.Id == respuesta.IdPregunta).Ponderacion) / 100;
                        db.RespuestaEncuesta.AddObject(respuesta);
                    }
                    t.EncuestaRespondida = true;
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

        public List<Encuesta> ObtenerEncuestasContestadas(bool insertarSeleccion)
        {
            List<Encuesta> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                var qry = from t in db.Ticket
                          join e in db.Encuesta on t.IdEncuesta equals e.Id
                          where t.EncuestaRespondida
                          select new { t, e };
                result = qry.Select(s => s.e).Distinct().ToList();
                if (insertarSeleccion)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione,
                        new Encuesta
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

