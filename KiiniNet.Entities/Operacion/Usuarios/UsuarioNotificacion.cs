using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class UsuarioNotificacion
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public Int64 IdTicketEvento { get; set; }
        [DataMember]
        public int Notificaciones { get; set; }
        [DataMember]
        public DateTime FechaActualizacion { get; set; }

        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual TicketEvento TicketEvento { get; set; }

    }
}
