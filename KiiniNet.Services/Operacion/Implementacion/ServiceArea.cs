using System;
using System.Collections.Generic;
using KiiniNet.Entities.Operacion;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public  class ServiceArea : IServiceArea
    {
        public void GuardarAreaAndroid(Area descripcion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    negocio.GuardarAreaAndroid(descripcion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        
        public List<Area> ObtenerAreasUsuario(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreasUsuario(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Area> GetAll()
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreas(true);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public List<Area> ObtenerAreasUsuarioByRol(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreasUsuarioByRol(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Area> ObtenerAreasUsuarioTercero(int idUsuario, int idUsuarioTercero, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreasUsuarioTercero(idUsuario, idUsuarioTercero, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Area> ObtenerAreasUsuarioPublico(bool insertarSeleccion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreasUsuarioPublico(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Area> ObtenerAreasTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreasTipoUsuario(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Area> ObtenerAreas(bool insertarSeleccion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreas(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Guardar(Area area)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    negocio.Guardar(area);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Area ObtenerAreaById(int idArea)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreaById(idArea);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Actualizar(int idArea, Area area)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    negocio.Actualizar(idArea, area);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Habilitar(int idArea, bool habilitado)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    negocio.Habilitar(idArea, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Area> ObtenerAreaConsulta(string descripcion)
        {
            try
            {
                using (BusinessArea negocio = new BusinessArea())
                {
                    return negocio.ObtenerAreaConsulta(descripcion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
