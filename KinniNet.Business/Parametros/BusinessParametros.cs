using System;
using System.Collections.Generic;
using System.Linq;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KinniNet.Business.Utils;
using KinniNet.Data.Help;

namespace KinniNet.Core.Parametros
{
    public class BusinessParametros : IDisposable
    {
        private bool _proxy;
        public BusinessParametros(bool proxy = false)
        {
            _proxy = proxy;
        }

        public void Dispose()
        {

        }

        public List<ParametrosTelefonos> TelefonosObligatorios(int idTipoUsuario)
        {
            List<ParametrosTelefonos> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.ParametrosTelefonos.Where(w => w.IdTipoUsuario == idTipoUsuario).ToList();
                foreach (ParametrosTelefonos param in result)
                {
                    db.LoadProperty(param, "TipoTelefono");
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

        public List<TelefonoUsuario> ObtenerTelefonosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {
            List<TelefonoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                int obligatorios = 0;
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new List<TelefonoUsuario>();
                foreach (ParametrosTelefonos parametrosTelefonose in db.ParametrosTelefonos.Where(w => w.IdTipoUsuario == idTipoUsuario))
                {
                    db.LoadProperty(parametrosTelefonose, "TipoTelefono");
                    obligatorios = parametrosTelefonose.Obligatorios;
                    for (int i = 0; i < parametrosTelefonose.NumeroTelefonos; i++)
                    {
                        result.Add(new TelefonoUsuario { IdTipoTelefono = parametrosTelefonose.IdTipoTelefono, TipoTelefono = parametrosTelefonose.TipoTelefono, Obligatorio = i + 1 <= obligatorios });
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

        public List<CorreoUsuario> ObtenerCorreosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {
            List<CorreoUsuario> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                int obligatorios = 0;
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = new List<CorreoUsuario>();
                foreach (TipoUsuario tipoUsuario in db.TipoUsuario.Where(w => w.Id == idTipoUsuario))
                {
                    obligatorios = tipoUsuario.CorreosObligatorios;
                    for (int i = 0; i < tipoUsuario.NumeroCorreos; i++)
                    {
                        result.Add(new CorreoUsuario { Obligatorio = i + 1 <= obligatorios });
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

        public ParametrosSla ObtenerParametrosSla()
        {
            ParametrosSla result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.ParametrosSla.First();
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

        public ParametrosGenerales ObtenerParametrosGenerales()
        {
            ParametrosGenerales result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.ParametrosGenerales.First();
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

        public List<AliasOrganizacion> ObtenerAliasOrganizacion(int idTipoUsuario)
        {
            List<AliasOrganizacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.AliasOrganizacion.Where(w => w.IdTipoUsuario == idTipoUsuario).ToList();
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

        public AliasOrganizacion ObtenerAliasOrganizacionNivel(int idTipoUsuario, int nivel)
        {
            AliasOrganizacion result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.AliasOrganizacion.SingleOrDefault(w => w.IdTipoUsuario == idTipoUsuario && w.Nivel == nivel);
                if (result == null)
                {
                    string message = null;
                    switch (nivel)
                    {
                        case 1:
                            message = BusinessVariables.AliasOrganizaciones.Nivel1;
                            break;
                        case 2:
                            message = BusinessVariables.AliasOrganizaciones.Nivel2;
                            break;
                        case 3:
                            message = BusinessVariables.AliasOrganizaciones.Nivel3;
                            break;
                        case 4:
                            message = BusinessVariables.AliasOrganizaciones.Nivel4;
                            break;
                        case 5:
                            message = BusinessVariables.AliasOrganizaciones.Nivel5;
                            break;
                        case 6:
                            message = BusinessVariables.AliasOrganizaciones.Nivel6;
                            break;
                        case 7:
                            message = BusinessVariables.AliasOrganizaciones.Nivel7;
                            break;
                            
                    }
                    result = new AliasOrganizacion { Descripcion = string.Format("{0}", message) };
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

        public List<AliasUbicacion> ObtenerAliasUbicacion(int idTipoUsuario)
        {
            List<AliasUbicacion> result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.AliasUbicacion.Where(w => w.IdTipoUsuario == idTipoUsuario).ToList();
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

        public AliasUbicacion ObtenerAliasUbicacionNivel(int idTipoUsuario, int nivel)
        {
            AliasUbicacion result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.AliasUbicacion.SingleOrDefault(w => w.IdTipoUsuario == idTipoUsuario && w.Nivel == nivel);
                if (result == null)
                {
                    string message = null;
                    switch (nivel)
                    {
                        case 1:
                            message = BusinessVariables.AliasUbicaciones.Nivel1;
                            break;
                        case 2:
                            message = BusinessVariables.AliasUbicaciones.Nivel2;
                            break;
                        case 3:
                            message = BusinessVariables.AliasUbicaciones.Nivel3;
                            break;
                        case 4:
                            message = BusinessVariables.AliasUbicaciones.Nivel4;
                            break;
                        case 5:
                            message = BusinessVariables.AliasUbicaciones.Nivel5;
                            break;
                        case 6:
                            message = BusinessVariables.AliasUbicaciones.Nivel6;
                            break;
                        case 7:
                            message = BusinessVariables.AliasUbicaciones.Nivel7;
                            break;

                    }
                    result = new AliasUbicacion { Descripcion = string.Format("{0}", message) };
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

        public ParametroDatosAdicionales ObtenerDatosAdicionales(int idTipoUsuario)
        {
            ParametroDatosAdicionales result;
            DataBaseModelContext db = new DataBaseModelContext();
            try
            {
                db.ContextOptions.ProxyCreationEnabled = _proxy;
                result = db.ParametroDatosAdicionales.SingleOrDefault(w => w.IdTipoUsuario == idTipoUsuario);
                if (result != null)
                {
                    db.LoadProperty(result, "Mascara");
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
