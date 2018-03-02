using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperTicketsUsuario
    {
        public int IdTicket { get; set; }
        public string Tipificacion { get; set; }

        public int IdEstatusTicket { get; set; }
        public string DescripcionEstatusTicket { get; set; }

        public DateTime FechaCreacion { get; set; }
        public string FechaCreacionFormato { get; set; }

        public bool PuedeVer { get; set; }
    }
}
