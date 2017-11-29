using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class Impacto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdPrioridad { get; set; }
        [DataMember]
        public int IdUrgencia { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual Prioridad Prioridad { get; set; }
        [DataMember]
        public virtual Urgencia Urgencia { get; set; }
        [DataMember]
        public virtual List<ArbolAcceso> ArbolAcceso { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }
    }
}
