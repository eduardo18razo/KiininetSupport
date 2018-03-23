using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class CorreoService
    {
        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public Int64 IdTicketEvento { get; set; }
        [DataMember]
        public string Correo { get; set; }
        [DataMember]
        public string Contenido { get; set; }
        [DataMember]
        public DateTime? FechaSolicitud { get; set; }
        [DataMember]
        public DateTime? FechaEnvio { get; set; }
        [DataMember]
        public bool Enviado { get; set; }

        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual TicketEvento TicketEvento { get; set; }
    }
}
