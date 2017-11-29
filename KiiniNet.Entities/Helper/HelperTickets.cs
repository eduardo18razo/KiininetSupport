using System;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperTickets
    {
        public int IdTicket { get; set; }
        public int IdUsuario { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public int IdGrupoAsignado { get; set; }
        public int IdImpacto { get; set; }
        public DateTime FechaHora { get; set; }
        public int NumeroTicket { get; set; }
        public string NombreUsuario { get; set; }
        public int IdSubRolAsignado { get; set; }
        public string Tipificacion { get; set; }
        public string GrupoAsignado { get; set; }
        public string UsuarioAsignado { get; set; }
        public int IdNivelAsignado { get; set; }
        public string NivelUsuarioAsignado { get; set; }
        public string Impacto { get; set; }
        public EstatusTicket EstatusTicket { get; set; }
        public EstatusAsignacion EstatusAsignacion { get; set; }
        public int Total { get; set; }
        public bool TieneEncuesta { get; set; }
        public bool EsPropietario { get; set; }
        public bool CambiaEstatus { get; set; }
        public bool Asigna { get; set; }

        public bool DentroSla { get; set; }
        public bool RecienCerrado { get; set; }


        public string ImagenPrioridad { get; set; }
        public string ImagenSla { get; set; }
        public Usuario UsuarioSolicito { get; set; }
        public string TipoTicketDescripcion { get; set; }

        public string TipoTicketAbreviacion { get; set; }

        public string Canal { get; set; }
        public bool Vip { get; set; }

        public int IdSubRolActual
        {
            get { return IdNivelAsignado + 2; }
        }

    }
}
