using System;
using System.Data;

namespace KiiniNet.Entities.Reportes
{
    [Serializable]
    public class ReporteInformacionConsulta
    {
        public int IdInformacionConsulta { get; set; }
        public DataTable GraficoBarras { get; set; }
        public DataTable GraficoPie { get; set; }
    }
}
