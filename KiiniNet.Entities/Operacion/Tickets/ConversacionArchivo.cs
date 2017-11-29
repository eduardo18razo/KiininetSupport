using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class ConversacionArchivo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTicketConversacion { get; set; }
        [DataMember]
        public string Archivo { get; set; }
        [DataMember]
        public virtual TicketConversacion TicketConversacion { get; set; }
    }
}
