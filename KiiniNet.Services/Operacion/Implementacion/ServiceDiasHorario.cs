using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Services.Operacion.Interface;
using KinniNet.Core.Operacion;

namespace KiiniNet.Services.Operacion.Implementacion
{
    public class ServiceDiasHorario : IServiceDiasHorario
    {
        #region Horarios
        public List<Horario> ObtenerHorarioDefault(bool insertarSeleccion)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerHorarioSistema(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Horario> ObtenerHorarioConsulta(string filtro)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerHorarioConsulta(filtro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CrearHorario(Horario horario)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.CrearHorario(horario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarHorario(Horario horario)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.ActualizarHorario(horario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Horario ObtenerHorarioById(int idHorario)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerHorarioById(idHorario);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarHorario(int idHorario, bool habilitado)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.HabilitarHorario(idHorario, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        

        #endregion Horarios

        #region Dias Feriados

        public List<DiasFeriados> ObtenerDiasFeriadosConsulta(string filtro)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiasFeriadosConsulta(filtro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<DiaFestivoDefault> ObtenerDiasDefault(bool insertarSeleccion)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiasDefault(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AgregarDiaFeriado(DiaFeriado dia)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.AgregarDiaFeriado(dia);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DiaFeriado ObtenerDiaByFecha(DateTime fecha)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiaByFecha(fecha);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DiaFeriado ObtenerDiaFeriado(int id)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiaFeriado(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DiaFeriado> ObtenerDiasFeriados(bool insertarSeleccion)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiasFeriados(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarDiasFestivos(DiasFeriados item)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.ActualizarDiasFestivos(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CrearDiasFestivos(DiasFeriados item)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.CrearDiasFestivos(item);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DiasFeriados> ObtenerDiasFeriadosUser(bool insertarSeleccion)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiasFeriadosUser(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DiasFeriados ObtenerDiasFeriadosUserById(int id)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    return negocio.ObtenerDiasFeriadosUserById(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarDiasFestivos(int idDiasFestivos, bool habilitado)
        {
            try
            {
                using (BusinessDiasHorario negocio = new BusinessDiasHorario())
                {
                    negocio.HabilitarDiasFestivos(idDiasFestivos, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Dias Feriados
    }
}
