using System;
using System.Collections.Generic;
using System.Data;

namespace KiiniNet.Entities.Helper.Reportes
{
    [Serializable]
    public class HelperReporteEncuesta
    {
        public int IdArbolAcceso { get; set; }
        public string Titulo { get; set; }
        public DataTable GraficoBarras { get; set; }
        public DataTable GraficoPie { get; set; }
        public List<DataTable> Preguntas { get; set; }
    }
}
