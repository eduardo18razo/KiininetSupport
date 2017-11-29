using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class SlaDetalle
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdSla { get; set; }
        [DataMember]
        public int IdSubRol { get; set; }
        [DataMember]
        public decimal Dias { get; set; }
        [DataMember]
        public decimal Horas { get; set; }
        [DataMember]
        public decimal Minutos { get; set; }
        [DataMember]
        public decimal Segundos { get; set; }
        [DataMember]
        public decimal TiempoProceso { get; set; }
        [DataMember]
        public virtual Sla Sla { get; set; }
    }
}
