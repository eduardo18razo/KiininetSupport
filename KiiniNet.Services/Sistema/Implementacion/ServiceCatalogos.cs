using System;
using System.Collections.Generic;
using System.Data;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Helper;
using KiiniNet.Services.Sistema.Interface;
using KinniNet.Core.Sistema;

namespace KiiniNet.Services.Sistema.Implementacion
{
    
    public class ServiceCatalogos : IServiceCatalogos
    {
        public void CrearCatalogo(Catalogos catalogo, bool esMascara, List<CatalogoGenerico> registros)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.CrearCatalogo(catalogo, esMascara, registros);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarCatalogo(Catalogos catalogo, bool esMascara, List<CatalogoGenerico> registros)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.ActualizarCatalogo(catalogo, esMascara, registros);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Catalogos ObtenerCatalogo(int idCatalogo)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    return negocio.ObtenerCatalogo(idCatalogo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Catalogos> ObtenerCatalogos(bool insertarSeleccion)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    return negocio.ObtenerCatalogos(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Catalogos> ObtenerCatalogoConsulta(int? idCatalogo)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    return negocio.ObtenerCatalogoConsulta(idCatalogo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Catalogos> ObtenerCatalogosMascaraCaptura(bool insertarSeleccion)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    return negocio.ObtenerCatalogosMascaraCaptura(insertarSeleccion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Habilitar(int idCatalogo, bool habilitado)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.Habilitar(idCatalogo, habilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AgregarRegistroSistema(int idCatalogo, string descripcion)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.AgregarRegistroSistema(idCatalogo, descripcion);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarRegistroSistema(int idCatalogo, string descripcion, int idRegistro)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.ActualizarRegistroSistema(idCatalogo, descripcion, idRegistro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void HabilitarRegistroSistema(int idCatalogo, bool habilitado, int idRegistro)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.HabilitarRegistroSistema(idCatalogo, habilitado, idRegistro);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CatalogoGenerico> ObtenerRegistrosSistemaCatalogo(int idCatalogo, bool insertarSeleccion, bool filtroHabilitado)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    return negocio.ObtenerRegistrosSistemaCatalogo(idCatalogo, insertarSeleccion, filtroHabilitado);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable ObtenerRegistrosArchivosCatalogo(int idCatalogo)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    return negocio.ObtenerRegistrosArchivosCatalogo(idCatalogo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CrearCatalogoExcel(Catalogos catalogo, bool esMascara, string archivo, string hoja)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.CrearCatalogoExcel(catalogo, esMascara, archivo, hoja);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarCatalogoExcel(Catalogos cat, bool esMascara, string archivo, string hoja)
        {
            try
            {
                using (BusinessCatalogos negocio = new BusinessCatalogos())
                {
                    negocio.ActualizarCatalogoExcel(cat, esMascara, archivo, hoja);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
