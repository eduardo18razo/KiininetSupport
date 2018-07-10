using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace KiiniNet.Entities.Helper.Reportes
{
    [Serializable]
    public class HelperReporteEncuesta
    {
        public int IdArbolAcceso { get; set; }
        public string Titulo { get; set; }
        public DataTable GraficoBarras { get; set; }
        public DataTable GraficoPie { get; set; }
    }
}
