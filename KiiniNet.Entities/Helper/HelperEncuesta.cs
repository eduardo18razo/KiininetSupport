using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperEncuesta
    {
        public int IdTicket { get; set; }
        public int IdEncuesta { get; set; }
        public int NumeroTicket { get; set; }
        public string Tipificacion { get; set; }
        public string Descripcion { get; set; }

        public bool Respondida { get; set; }

    }
}
