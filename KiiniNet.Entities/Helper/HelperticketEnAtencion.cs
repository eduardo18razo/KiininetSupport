using System;
using System.Collections.Generic;
using System.Configuration;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperTicketEnAtencion
    {
        public int IdTicket { get; set; }
        public string Tipificacion { get; set; }
        public string CorreoTicket { get; set; }
        public string FechaLevanto { get; set; }
        public string Impacto { get; set; }
        public bool DentroSla { get; set; }
        public int IdEstatusTicket { get; set; }
        public string DescripcionEstatusTicket { get; set; }
        public string ColorEstatus { get; set; }
        public int? IdNivelAsignacion { get; set; }

        public int IdEstatusAsignacion { get; set; }
        public string DescripcionEstatusAsignacion { get; set; }
        public bool EsPropietario { get; set; }
        public bool PuedeAsignar { get; set; }
        public int IdGrupoAsignado { get; set; }
        public string UsuarioAsignado { get; set; }

        public List<EstatusTicket> EstatusDisponibles { get; set; }

        public bool GrupoConSupervisor { get; set; }
        
        public HelperUsuario UsuarioLevanto { get; set; }
        public List<HelperConversacionDetalle> Conversaciones { get; set; }

        public List<HelperEvento> Eventos { get; set; }

    }
}
