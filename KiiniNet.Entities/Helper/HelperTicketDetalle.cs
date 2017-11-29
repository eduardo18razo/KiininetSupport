using System;
using System.Collections.Generic;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperTicketDetalle
    {
        public int IdTicket { get; set; }
        public string Tipificacion { get; set; }
        public int IdUsuarioLevanto { get; set; }
        public string UsuarioLevanto { get; set; }
        public string CorreoUsuarioPrincipal { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public int IdImpacto { get; set; }
        public string Impacto { get; set; }
        public string DiferenciaSla { get; set; }
        public EstatusTicket EstatusTicket { get; set; }
        public EstatusAsignacion EstatusAsignacion { get; set; }

        public DateTime UltimaActualizacion { get; set; }

        public bool EsPropietario { get; set; }
        public bool CambiaEstatus { get; set; }

        public bool Asigna { get; set; }
        public bool Reasigna { get; set; }
        public bool Escala { get; set; }

        public int IdUsuarioAsignado { get; set; }
        public Usuario UsuarioAsignado { get; set; }
        public string NivelUsuarioAsignado { get; set; }
        public GrupoUsuario GrupoAsignado { get; set; }
        public Usuario DetalleUsuarioLevanto { get; set; }

        public List<HelperConversacionDetalle> ConversacionDetalle { get; set; }


    }
}
