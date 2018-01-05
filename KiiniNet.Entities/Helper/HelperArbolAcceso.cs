using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperArbolAcceso
    {
        public int Id { get; set; }
        public string TipoUsuario { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string DescripcionTipificacion { get; set; }
        public string Tipo { get; set; }
        public int Nivel { get; set; }
        public string Ruta { get; set; }
        public string DescripcionLarga { get; set; }

        public string Activo { get; set; }
        
    }
}
