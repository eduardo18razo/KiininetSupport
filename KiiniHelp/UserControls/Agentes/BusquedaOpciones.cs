using System;
using System.Collections.Generic;
using System.Linq;
using KiiniHelp.ServiceArbolAcceso;
using KiiniHelp.ServiceArea;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KinniNet.Business.Utils;

namespace KiiniHelp.UserControls.Agentes
{
    public class BusquedaOpciones
    {
        private readonly ServiceAreaClient _servicioArea = new ServiceAreaClient();
        private readonly ServiceArbolAccesoClient _servicioArbol = new ServiceArbolAccesoClient();
        public BusquedaOpciones() { }
        public List<HelperArbolAcceso> GetOptions(int idUsuarioSolicita, int idUsuarioLevanta, int? idArea, bool insertarSeleccion, string keys)
        {
            List<HelperArbolAcceso> result = null;
            try
            {
                result = _servicioArbol.ObtenerOpcionesPermitidas(idUsuarioSolicita, idUsuarioLevanta, idArea, insertarSeleccion);
                if (result != null && keys != null && keys.Trim() != string.Empty)
                    result = result.Where(w => w.Descripcion.ToLower().Contains(keys.ToLower().Trim())).ToList();
                if (result == null)
                    result = new List<HelperArbolAcceso>();
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public List<Area> GetAreas(int idUsuarioSolicito)
        {
            List<Area> result = null;
            try
            {
                result = _servicioArea.ObtenerAreasUsuario(idUsuarioSolicito, false);
                if(result != null)
                    result.Insert(BusinessVariables.ComboBoxCatalogo.IndexSeleccione, new Area { Id = BusinessVariables.ComboBoxCatalogo.ValueTodos, Descripcion = "Categoría" });
            }
            catch (Exception)
            {
            }
            return result;

        }
    }
}