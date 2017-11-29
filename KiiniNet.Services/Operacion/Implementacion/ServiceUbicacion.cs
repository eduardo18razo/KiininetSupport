using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceUbicacion : IServiceUbicacion
    {
        public List<Pais> ObtenerPais(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerPais(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Campus> ObtenerCampus(int idTipoUsuario, int idPais, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerCampus(idTipoUsuario, idPais, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Torre> ObtenerTorres(int idTipoUsuario, int idCampus, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerTorres(idTipoUsuario, idCampus, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Piso> ObtenerPisos(int idTipoUsuario, int idTorre, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerPisos(idTipoUsuario, idTorre, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Zona> ObtenerZonas(int idTipoUsuario, int idPiso, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerZonas(idTipoUsuario, idPiso, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SubZona> ObtenerSubZonas(int idTipoUsuario, int idZona, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerSubZonas(idTipoUsuario, idZona, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<SiteRack> ObtenerSiteRacks(int idTipoUsuario, int idSubZona, bool insertarSeleccion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerSiteRacks(idTipoUsuario, idSubZona, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Ubicacion ObtenerUbicacion(int idPais, int? idCampus, int? idTorre, int? idPiso, int? idZona, int? idSubZona, int? idSiteRack)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacion(idPais, idCampus, idTorre, idPiso, idZona, idSubZona, idSiteRack);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarUbicacion(Ubicacion ubicacion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    negocio.GuardarUbicacion(ubicacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Ubicacion ObtenerUbicacionUsuario(int idUbicacion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacionUsuario(idUbicacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Ubicacion> ObtenerUbicaciones(int? idTipoUsuario, int? idPais, int? idCampus, int? idTorre, int? idPiso, int? idZona, int? idSubZona, int? idSiteRack)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicaciones(idTipoUsuario, idPais, idCampus, idTorre, idPiso, idZona, idSubZona, idSiteRack);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Ubicacion> BuscarPorPalabra(int? idTipoUsuario, int? idPais, int? idCampus, int? idTorre, int? idPiso, int? idZona,
            int? idSubZona, int? idSiteRack, string filtro)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.BuscarPorPalabra(idTipoUsuario, idPais, idCampus, idTorre, idPiso, idZona, idSubZona, idSiteRack, filtro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void ActualizarUbicacion(Ubicacion ub)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    negocio.ActualizarUbicacion(ub);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Ubicacion ObtenerUbicacionById(int idUbicacion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacionById(idUbicacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarUbicacion(int idUbicacion, bool habilitado)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    negocio.HabilitarUbicacion(idUbicacion, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Ubicacion> ObtenerUbicacionesGrupos(List<int> grupos)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacionesGrupos(grupos);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<int> ObtenerUbicacionesByIdUbicacion(int idUbicacion)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacionesByIdUbicacion(idUbicacion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Ubicacion> ObtenerUbicacionByRegionCode(string regionCode)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacionByRegionCode(regionCode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Ubicacion ObtenerUbicacionFiscal(int idColonia, string calle, string noExt, string noInt)
        {
            try
            {
                using (BusinessUbicacion negocio = new BusinessUbicacion())
                {
                    return negocio.ObtenerUbicacionFiscal(idColonia, calle, noExt, noInt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
