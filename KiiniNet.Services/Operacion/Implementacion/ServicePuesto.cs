using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServicePuesto : IServicePuesto
    {
        public List<Puesto> ObtenerPuestosByTipoUsuario(int idTipoUsuario, bool insertarSeleccion)
        {
            try
            {
                using (BusinessPuesto negocio = new BusinessPuesto())
                {
                    return negocio.ObtenerPuestosByTipoUsuario(idTipoUsuario, insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Puesto ObtenerPuestoById(int idPuesto)
        {
            try
            {
                using (BusinessPuesto negocio = new BusinessPuesto())
                {
                    return negocio.ObtenerPuestoById(idPuesto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Guardar(Puesto puesto)
        {
            try
            {
                using (BusinessPuesto negocio = new BusinessPuesto())
                {
                    negocio.Guardar(puesto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Actualizar(int idPuesto, Puesto puesto)
        {
            try
            {
                using (BusinessPuesto negocio = new BusinessPuesto())
                {
                    negocio.Actualizar(idPuesto, puesto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Puesto> ObtenerPuestoConsulta(int? idTipoUsuario)
        {
            try
            {
                using (BusinessPuesto negocio = new BusinessPuesto())
                {
                    return negocio.ObtenerPuestoConsulta(idTipoUsuario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Habilitar(int idPuesto, bool habilitado)
        {
            try
            {
                using (BusinessPuesto negocio = new BusinessPuesto())
                {
                    negocio.Habilitar(idPuesto, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
