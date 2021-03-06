﻿using System;
using System.Collections.Generic;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;
using KiiniNet.Services.Parametros.Interface;
using KinniNet.Core.Parametros;

namespace KiiniNet.Services.Parametros.Implementacion
{
    public class ServiceParametros : IServiceParametros
    {

        public List<TelefonoUsuario> ObtenerTelefonosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerTelefonosParametrosIdTipoUsuario(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CorreoUsuario> ObtenerCorreosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerCorreosParametrosIdTipoUsuario(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ParametrosGenerales ObtenerParametrosGenerales()
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerParametrosGenerales();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ParametrosUsuario ObtenerParametrosUsuario(int idTipoUsuario)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerParametrosUsuario(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AliasOrganizacion> ObtenerAliasOrganizacion(int idTipoUsuario)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerAliasOrganizacion(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<AliasUbicacion> ObtenerAliasUbicacion(int idTipoUsuario)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerAliasUbicacion(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ParametroDatosAdicionales ObtenerDatosAdicionales(int idTipoUsuario)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerDatosAdicionales(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ParametroPassword ObtenerParemtrosPassword()
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerParemtrosPassword();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public GraficosDefault ObtenerParametrosGraficoDefault(int idReporte)
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerParametrosGraficoDefault(idReporte);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<FrecuenciaFecha> ObtenerFrecuenciasFecha()
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerFrecuenciasFecha();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ColoresTop> ObtenerColoresTop()
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerColoresTop();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ColoresSla> ObtenerColoresSla()
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerColoresSla();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ArchivosPermitidos> ObtenerArchivosPermitidos()
        {
            try
            {
                using (BusinessParametros negocio = new BusinessParametros())
                {
                    return negocio.ObtenerArchivosPermitidos();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
