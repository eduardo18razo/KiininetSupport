using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperNotasOpcion
    {
        public int Id { get; set; }
        public int IdTipoNota { get; set; }
        public string TipoNota { get; set; }
        public int IdArbol { get; set; }
        public string Nombre { get; set; }
        public string Contenido { get; set; }
        public bool Habilitado { get; set; }

    }
}
