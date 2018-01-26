using System;

namespace KiiniNet.Entities.Operacion.Dashboard
{
    [Serializable]
    public class GraficoConteo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Total { get; set; }
    }
}
