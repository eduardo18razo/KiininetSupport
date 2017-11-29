using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Cat.Mascaras
{
    [DataContract(IsReference = true)]
    public class MascaraSeleccionCatalogo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NombreCampoMascara { get; set; }
        [DataMember]
        public int IdRegistroCatalogo { get; set; }
        [DataMember]
        public int IdTicket { get; set; }
        [DataMember]
        public bool Seleccionado { get; set; }
        [DataMember]
        public virtual Ticket Ticket { get; set; }
    }
}
