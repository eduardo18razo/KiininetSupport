using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceArbolAcceso : IServiceArbolAcceso
    {
        public List<HelperArbolAcceso> ObtenerOpcionesPermitidas(int idUsuarioSolicita, int idUsuarioLevanta, int? idArea, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerOpcionesPermitidas(idUsuarioSolicita, idUsuarioLevanta, idArea, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool LevantaTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.LevantaTicket(idUsuarioLevanta, idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool RecadoTicketTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.RecadoTicketTicket(idUsuarioLevanta, idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel1> ObtenerNivel1ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel1ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel2> ObtenerNivel2ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel1, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel2ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel1, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel3> ObtenerNivel3ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel2, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel3ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel2, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel4> ObtenerNivel4ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel3, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel4ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel3, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel5> ObtenerNivel5ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel4, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel5ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel4, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel6> ObtenerNivel6ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel5, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel6ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel5, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel7> ObtenerNivel7ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel6, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel7ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel6, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EsNodoTerminalByGrupos(int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Nivel1> ObtenerNivel1(int idArea, int idTipoArbol, int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel1(idArea, idTipoArbol, idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Nivel2> ObtenerNivel2(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel1, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel2(idArea, idTipoArbol, idTipoUsuario, idNivel1, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Nivel3> ObtenerNivel3(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel2, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel3(idArea, idTipoArbol, idTipoUsuario, idNivel2, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Nivel4> ObtenerNivel4(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel3, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel4(idArea, idTipoArbol, idTipoUsuario, idNivel3, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Nivel5> ObtenerNivel5(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel4, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel5(idArea, idTipoArbol, idTipoUsuario, idNivel4, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Nivel6> ObtenerNivel6(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel5, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel6(idArea, idTipoArbol, idTipoUsuario, idNivel5, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<Nivel7> ObtenerNivel7(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel6, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerNivel7(idArea, idTipoArbol, idTipoUsuario, idNivel6, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool EsNodoTerminal(int idTipoUsuario, int idTipoArbol, int nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.EsNodoTerminal(idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void GuardarArbol(ArbolAcceso arbol)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    negocio.GuardarArbol(arbol);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ArbolAcceso> ObtenerArbolesAccesoByGruposUsuario(int idUsuario, int idTipoArbol, int idArea)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolesAccesoByUsuarioTipoArbol(idUsuario, idTipoArbol, idArea);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ArbolAcceso ObtenerArbolAcceso(int idArbol)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolAcceso(idArbol);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ArbolAcceso> ObtenerArbolesAccesoAll(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ArbolAcceso> ObtenerArbolesAccesoAllReporte(int? idArea, int? idTipoUsuario, int? idTipoArbol, int idTipoEncuesta,
            Dictionary<string, DateTime> fechas)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolesAccesoAllReporte(idArea, idTipoUsuario, idTipoArbol, idTipoEncuesta, fechas);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarArbol(int idArbol, bool habilitado)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    negocio.HabilitarArbol(idArbol, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizardArbol(int idArbolAcceso, ArbolAcceso arbolAcceso, string descripcion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    negocio.ActualizardArbol(idArbolAcceso, arbolAcceso, descripcion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ArbolAcceso> ObtenerArbolesAccesoTerminalAll(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolesAccesoTerminalAll(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalAllTipificacion(int? idArea, int? idTipoUsuario, int? idTipoArbol, int? nivel1,
            int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolesAccesoTerminalAllTipificacion(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalByIdUsuario(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso())
                {
                    return negocio.ObtenerArbolesAccesoTerminalByIdUsuario(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperArbolAcceso> ObtenerArbolesAccesoTerminalByGrupoUsuario(int idGrupo)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso(true))
                {
                    return negocio.ObtenerArbolesAccesoTerminalByGrupoUsuario(idGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperBusquedaArbolAcceso> BusquedaGeneral(int? idUsuario, string filter, List<int> tipoUsuario, int? idTipoArbol, int? idCategoria, int page, int pagesize)
        {
            try
            {
                using (BusinessArbolAcceso negocio = new BusinessArbolAcceso(true))
                {
                    return negocio.BusquedaGeneral(idUsuario, filter, tipoUsuario, idTipoArbol, idCategoria, page, pagesize);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
