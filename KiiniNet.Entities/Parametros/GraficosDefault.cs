using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class GraficosDefault
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdMenu { get; set; }
        [DataMember]
        public int IdFrecuenciaFecha { get; set; }
        [DataMember]
        public int Periodo { get; set; }
        [DataMember]
        public bool Stack { get; set; }
        [DataMember]
        public bool Pareto { get; set; }
        [DataMember]
        public bool Pie { get; set; }
        [DataMember]
        public bool Geografico { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual FrecuenciaFecha FrecuenciaFecha { get; set; }
        [DataMember]
        public virtual Menu Menu { get; set; }
    }
}
