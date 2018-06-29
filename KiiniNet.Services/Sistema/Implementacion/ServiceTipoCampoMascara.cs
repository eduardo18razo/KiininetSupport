using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    public class ServiceTipoCampoMascara : IServiceTipoCampoMascara
    {
        public List<TipoCampoMascara> ObtenerTipoCampoMascara(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoCampoMascara negocio = new BusinessTipoCampoMascara())
                {
                    return negocio.ObtenerTipoCampoMascara(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TipoCampoMascara TipoCampoMascaraId(int idTipoCampo)
        {
            try
            {
                using (BusinessTipoCampoMascara negocio = new BusinessTipoCampoMascara())
                {
                    return negocio.TipoCampoMascaraId(idTipoCampo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<TipoMascara> ObtenerTipoMascara(bool insertarSeleccion)
        {
            try
            {
                using (BusinessTipoCampoMascara negocio = new BusinessTipoCampoMascara())
                {
                    return negocio.ObtenerTipoMascara(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
