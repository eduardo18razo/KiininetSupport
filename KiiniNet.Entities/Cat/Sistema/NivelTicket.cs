using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class NivelTicket
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }
    }
}
