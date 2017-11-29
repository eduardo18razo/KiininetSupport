using System;
using System.Collections.Generic;
using KiiniNet.Entities.Helper;
using KiiniNet.Entities.Operacion;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceNota : IServiceNota
    {
        public void CrearNotaGeneralUsuario(NotaGeneral notaGeneral)
        {
            try
            {
                using (BusinessNota negocio = new BusinessNota())
                {
                    negocio.CrearNotaGeneral(notaGeneral);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void CrearNotaOpcionUsuario(NotaOpcionUsuario notaOpcionUsuario)
        {
            try
            {
                using (BusinessNota negocio = new BusinessNota())
                {
                    negocio.CrearNotaOpcionUsuario(notaOpcionUsuario);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public void CrearNotaOpcionGrupo(NotaOpcionGrupo notaOpcionGrupo)
        {
            try
            {
                using (BusinessNota negocio = new BusinessNota())
                {
                    negocio.CrearNotaOpcionGrupo(notaOpcionGrupo);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<HelperNotasUsuario> ObtenerNotasUsuario(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessNota negocio = new BusinessNota())
                {
                    return negocio.ObtenerNotasUsuario(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<HelperNotasOpcion> ObtenerNotasOpcion(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessNota negocio = new BusinessNota())
                {
                    return negocio.ObtenerNotasOpcion(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public List<HelperNotasUsuario> ObtenerNotasGrupo(int idUsuario)
        {
            try
            {
                using (BusinessNota negocio = new BusinessNota())
                {
                    return negocio.ObtenerNotasUsuario(idUsuario, false);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
