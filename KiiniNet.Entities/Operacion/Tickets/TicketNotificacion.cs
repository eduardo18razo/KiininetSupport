using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketNotificacion
    {

        [DataMember]
        public Int64 Id { get; set; }
        [DataMember]
        public int? IdSmsService { get; set; }

        [DataMember]
        public int IdTicket { get; set; }
        
        [DataMember]
        public int IdGrupoUsuario { get; set; }

        [DataMember]
        public int IdUsuario { get; set; }

        [DataMember]
        public string TelefonoUsuario { get; set; }
        [DataMember]
        public string CorreoUsuario { get; set; }

        [DataMember]
        public int IdTipoNotificacion { get; set; }

        [DataMember]
        public virtual DateTime FechaNotificacion { get; set; }

        [DataMember]
        public virtual GrupoUsuario GrupoUsuario { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual SmsService SmsService { get; set; }
        [DataMember]
        public virtual Ticket Ticket { get; set; }

        [DataMember]
        public virtual TipoNotificacion TipoNotificacion { get; set; }
    }
}
