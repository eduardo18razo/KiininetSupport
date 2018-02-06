using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Dashboard
{
    [Serializable]
    public class DashboardAgente
    {
        public int TotalTickets { get; set; }
        public int TicketsAbiertos { get; set; }
        public int TicketsCreados { get; set; }
        public int TicketsResuletos { get; set; }
        public double TicketsResuletosVsCreados { get; set; }
        public double TicketsResuletosVsReabiertos { get; set; }
        public int TotalTicketsReabiertos { get; set; }

        //Metricas
        public double PromedioPrimeraRespuestaSegundosActual { get; set; }
        public string PromedioPrimeraRespuestaActual { get; set; }
        public double PromedioPrimeraRespuestaSegundosAnterior { get; set; }
        public string PromedioPrimeraRespuestaAnterior { get; set; }
        public double DiferenciaPromedioRespuestaPorcentaje { get; set; }


        public double PromedioTiempoResolucionSegundosActual { get; set; }
        public string PromedioTiempoResolucionActual { get; set; }
        public double PromedioTiempoResolucionSegundosAnterior { get; set; }
        public string PromedioTiempoResolucionAnterior { get; set; }
        public double PromedioTiempoResolucionPorcentaje { get; set; }


        public double PromedioResolucionPrimercontactoSegundosActual { get; set; }
        public string PromedioResolucionPrimercontactoActual { get; set; }
        public double PromedioResolucionPrimercontactoSegundosAnterior { get; set; }
        public string PromedioResolucionPrimercontactoAnterior { get; set; }
        public double PromedioResolucionPrimercontactoPorcentaje { get; set; }

        public double PromedioIntervencionesAgenteActual { get; set; }
        public double PromedioIntervencionesAgenteAnterior { get; set; }
        public double PromedioIntervencionesAgentePorcentaje { get; set; }

        public double ClientesAtendidosActual { get; set; }
        public double ClientesAtendidosAnterior { get; set; }
        public double ClientesAtendidosPorcentaje { get; set; }

        [DataMember]
        public DataTable GraficoTicketsAbiertos { get; set; }
        [DataMember]
        public DataTable GraficoTicketsPrioridad { get; set; }
        [DataMember]
        public DataTable GraficoTicketsCanal { get; set; }
        [DataMember]
        public DataTable GraficoTicketsCreadosResueltos { get; set; }

        public List<HelperMetricas> GruposMetricas { get; set; } 

    }
}
