using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class SmsService
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public int IdTipoLink { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Mensaje { get; set; }
        [DataMember]
        public bool Enviado { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual TipoLink TipoLink { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual List<TicketNotificacion> TicketNotificacion { get; set; }
    }
}
