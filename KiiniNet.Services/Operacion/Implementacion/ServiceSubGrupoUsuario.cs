using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceSubGrupoUsuario : IServiceSubGrupoUsuario
    {
        public List<SubGrupoUsuario> ObtenerSubGruposUsuarioByIdGrupo(int idGrupoUsuario)
        {
            try
            {
                using (BusinessSubGrupoUsuario negocio = new BusinessSubGrupoUsuario())
                {
                    return negocio.ObtenerSubGruposUsuarioByIdGrupo(idGrupoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<HelperSubGurpoUsuario> ObtenerSubGruposUsuario(int idGrupoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessSubGrupoUsuario negocio = new BusinessSubGrupoUsuario())
                {
                    return negocio.ObtenerSubGruposUsuario(idGrupoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public SubGrupoUsuario ObtenerSubGrupoUsuario(int idGrupoUsuario, int idSubGrupo)
        {
            try
            {
                using (BusinessSubGrupoUsuario negocio = new BusinessSubGrupoUsuario())
                {
                    return negocio.ObtenerSubGrupoUsuario(idGrupoUsuario, idSubGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
