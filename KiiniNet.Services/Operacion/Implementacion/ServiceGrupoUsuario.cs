using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceGrupoUsuario : IServiceGrupoUsuario
    {
        public List<GrupoUsuario> ObtenerGruposUsuarioByIdTipoSubGrupo(int idTipoSubgrupo, bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioByIdTipoSubGrupo(idTipoSubgrupo, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioByIdRolTipoUsuario(int idRol, int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioByIdRol(int idRol, bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioByIdRol(idRol, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioTipoUsuario(int idTipoGrupo, int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioTipoUsuario(idTipoGrupo, idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void GuardarGrupoUsuario(GrupoUsuario grupoUsuario, Dictionary<int, int> horarios, Dictionary<int, List<DiaFestivoSubGrupo>> diasDescanso)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    negocio.GuardarGrupoUsuario(grupoUsuario, horarios, diasDescanso);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public GrupoUsuario ObtenerGrupoUsuarioById(int idGrupoUsuario)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGrupoUsuarioById(idGrupoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioSistema(int idTipoUsuario)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioSistema(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioNivel(int idtipoArbol, int? nivel1, int? nivel2, int? nivel3, int? nivel4, int? nivel5, int? nivel6, int? nivel7)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioNivel(idtipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<UsuarioGrupo> ObtenerGruposDeUsuario(int idUsuario)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposDeUsuario(idUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarGrupo(int idGrupo, bool habilitado)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    negocio.HabilitarGrupo(idGrupo, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioAll(int? idTipoUsuario, int? idTipoGrupo)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioAll(idTipoUsuario, idTipoGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarGrupo(GrupoUsuario gpo, Dictionary<int, int> horarios, Dictionary<int, List<DiaFestivoSubGrupo>> diasDescanso)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    negocio.ActualizarGrupo(gpo, horarios, diasDescanso);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HorarioSubGrupo> ObtenerHorariosByIdSubGrupo(int idSubGrupo)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerHorariosByIdSubGrupo(idSubGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DiaFestivoSubGrupo> ObtenerDiasByIdSubGrupo(int idSubGrupo)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerDiasByIdSubGrupo(idSubGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGrupos(bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGrupos(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposByIdUsuario(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposByIdUsuario(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposUsuarioResponsablesByGruposTipoServicio(int idUsuario, List<int> grupos, List<int> tipoServicio)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposUsuarioResponsablesByGruposTipoServicio(idUsuario, grupos, tipoServicio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<GrupoUsuario> ObtenerGruposAtencionByIdUsuario(int idUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGruposAtencionByIdUsuario(idUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public GrupoUsuario ObtenerGrupoDefaultRol(int idRol, int idTipoUsuario)
        {
            try
            {
                using (BusinessGrupoUsuario negocio = new BusinessGrupoUsuario())
                {
                    return negocio.ObtenerGrupoDefaultRol(idRol, idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
