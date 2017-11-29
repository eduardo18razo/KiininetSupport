using KiiniNet.Entities.Cat.Sistema;
using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperReportesTicket
    {
        public int IdTicket { get; set; }
        public int IdCanal { get; set; }
        public string Canal { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public string GrupoEspecialConsulta { get; set; }
        public string GrupoAtendedor { get; set; }
        public string GrupoMantenimiento { get; set; }
        public string GrupoOperacion { get; set; }
        public string GrupoDesarrollo { get; set; }

        public int IdOrganizacion { get; set; }
        public string Organizacion { get; set; }
        public int IdNivelOrganizacion { get; set; }

        public int IdUbicacion { get; set; }
        public string Ubicacion { get; set; }
        public int IdNivelUbicacion { get; set; }

        public int IdTipificacion { get; set; }
        public string Tipificacion { get; set; }

        public int IdServicioIncidente { get; set; }
        public string ServicioIncidente { get; set; }
        public string Prioridad { get; set; }
        public string Urgencia { get; set; }

        public string Impacto { get; set; }
        public int IdEstatus { get; set; }
        public string Estatus { get; set; }
        public bool DentroSla { get; set; }
        public string Sla { get; set; }
        public string FechaHora { get; set; }

        public string Respuesta { get; set; }
  
        
    }
}
