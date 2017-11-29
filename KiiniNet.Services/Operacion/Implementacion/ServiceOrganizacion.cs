using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceOrganizacion : IServiceOrganizacion
    {
        public List<Holding> ObtenerHoldings(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerHoldings(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Compania> ObtenerCompañias(int idTipoUsuario, int idHolding, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerCompañias(idTipoUsuario, idHolding, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Direccion> ObtenerDirecciones(int idTipoUsuario, int idCompañia, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerDirecciones(idTipoUsuario, idCompañia, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SubDireccion> ObtenerSubDirecciones(int idTipoUsuario, int idDireccoin, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerSubDirecciones(idTipoUsuario, idDireccoin, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Gerencia> ObtenerGerencias(int idTipoUsuario, int idSubdireccion, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerGerencias(idTipoUsuario, idSubdireccion, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SubGerencia> ObtenerSubGerencias(int idTipoUsuario, int idGerencia, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerSubGerencias(idTipoUsuario, idGerencia, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Jefatura> ObtenerJefaturas(int idTipoUsuario, int idSubGerencia, bool insertarSeleccion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerJefaturas(idTipoUsuario, idSubGerencia, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Organizacion ObtenerOrganizacion(int idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerOrganizacion(idHolding, idCompania, idDireccion, idSubDireccion, idGerencia, idSubGerencia, idJefatura);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarOrganizacion(Organizacion organizacion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarOrganizacion(organizacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarHolding(Holding entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarHolding(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarCompania(Compania entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarCompania(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarDireccion(Direccion entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarDireccion(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarSubDireccion(SubDireccion entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarSubDireccion(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarGerencia(Gerencia entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarGerencia(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarSubGerencia(SubGerencia entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarSubGerencia(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarJefatura(Jefatura entidad)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.GuardarJefatura(entidad);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Organizacion ObtenerOrganizacionUsuario(int idOrganizacion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerOrganizacionUsuario(idOrganizacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Organizacion> ObtenerOrganizaciones(int? idTipoUsuario, int? idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerOrganizaciones(idTipoUsuario, idHolding, idCompania, idDireccion, idSubDireccion, idGerencia, idSubGerencia, idJefatura);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarOrganizacion(int idOrganizacion, bool habilitado)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.HabilitarOrganizacion(idOrganizacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Organizacion ObtenerOrganizacionById(int idOrganizacion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerOrganizacionById(idOrganizacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarOrganizacion(Organizacion org)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    negocio.ActualizarOrganizacion(org);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Organizacion> ObtenerOrganizacionesGrupos(List<int> grupos)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerOrganizacionesGrupos(grupos);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<int> ObtenerOrganizacionesByIdOrganizacion(int idOrganizacion)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.ObtenerOrganizacionesByIdOrganizacion(idOrganizacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Organizacion> BuscarPorPalabra(int? idTipoUsuario, int? idHolding, int? idCompania, int? idDireccion, int? idSubDireccion, int? idGerencia, int? idSubGerencia, int? idJefatura, string filtro)
        {
            try
            {
                using (BusinessOrganizacion negocio = new BusinessOrganizacion())
                {
                    return negocio.BuscarPorPalabra(idTipoUsuario, idHolding, idCompania, idDireccion, idSubDireccion, idGerencia, idSubGerencia, idJefatura, filtro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
