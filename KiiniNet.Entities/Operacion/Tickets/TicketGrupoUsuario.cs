using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class TicketGrupoUsuario
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTicket { get; set; }
        [DataMember]
        public int IdGrupoUsuario { get; set; }
        [DataMember]
        public int? IdSubGrupoUsuario { get; set; }

        [DataMember]
        public virtual Ticket Ticket { get; set; }
        [DataMember]
        public virtual GrupoUsuario GrupoUsuario { get; set; }
        [DataMember]
        public virtual SubGrupoUsuario SubGrupoUsuario { get; set; }
    }
}
