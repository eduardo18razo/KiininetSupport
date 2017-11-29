using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceImpactoUrgencia : IServiceImpactoUrgencia
    {
        public List<Prioridad> ObtenerPrioridad(bool insertarSeleccion)
        {
            try
            {
                using (BusinessImpactoUrgencia negocio = new BusinessImpactoUrgencia())
                {
                    return negocio.ObtenerPrioridad(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Urgencia> ObtenerUrgencia(bool insertarSeleccion)
        {
            try
            {
                using (BusinessImpactoUrgencia negocio = new BusinessImpactoUrgencia())
                {
                    return negocio.ObtenerUrgencia(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Impacto ObtenerImpactoById(int idImpacto)
        {
            try
            {
                using (BusinessImpactoUrgencia negocio = new BusinessImpactoUrgencia())
                {
                    return negocio.ObtenerImpactoById(idImpacto);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Impacto ObtenerImpactoByPrioridadUrgencia(int idPrioridad, int idUrgencia)
        {
            try
            {
                using (BusinessImpactoUrgencia negocio = new BusinessImpactoUrgencia())
                {
                    return negocio.ObtenerPrioridadByImpactoUrgencia(idPrioridad, idUrgencia);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Impacto> ObtenerAll(bool insertarSeleccion)
        {
            try
            {
                using (BusinessImpactoUrgencia negocio = new BusinessImpactoUrgencia())
                {
                    return negocio.ObtenerAll(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
