using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiiniNet.Entities.Operacion.Dashboard
{
    [Serializable]
    public class HelperMetricas
    {
        public int IdGrupo { get; set; }
        public string DescripcionGrupo { get; set; }
        public int TotalActual { get; set; }
        public int TotalAnterior { get; set; }
        public int TotalPorcentaje { get; set; }

        public int TotalAbiertosActual { get; set; }
        public int TotalAbiertosAnterior { get; set; }
        public int TotalAbiertosPorcentaje { get; set; }

        public int TotalImpactoAltoActual { get; set; }
        public int TotalImpactoAltoAnterior { get; set; }
        public int TotalImpactoAltoPorcentaje { get; set; }
    }
}
