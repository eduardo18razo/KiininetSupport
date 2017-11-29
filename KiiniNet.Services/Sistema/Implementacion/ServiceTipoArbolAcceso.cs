using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoArbolAcceso : IServiceTipoArbolAcceso
    {
        public List<TipoArbolAcceso> ObtenerTiposArbolAcceso(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoArbolAcceso negocio = new BusinessTipoArbolAcceso())
                {
                    return negocio.ObtenerTiposArbolAcceso(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TipoArbolAcceso> ObtenerTiposArbolAccesoByGruposTercero(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoArbolAcceso negocio = new BusinessTipoArbolAcceso())
                {
                    return negocio.ObtenerTiposArbolAccesoByGruposTercero(idUsuarioSolicita, idUsuarioLevanta, idArea, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TipoArbolAcceso> ObtenerTiposArbolAccesoByGrupos(List<int> grupos, bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoArbolAcceso negocio = new BusinessTipoArbolAcceso())
                {
                    return negocio.ObtenerTiposArbolAccesoByGrupos(grupos, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
