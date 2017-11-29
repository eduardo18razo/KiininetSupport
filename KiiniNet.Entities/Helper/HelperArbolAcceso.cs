using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperArbolAcceso
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Ruta { get; set; }
        public string DescripcionLarga { get; set; }
        public string Categoria { get; set; }
    }
}
