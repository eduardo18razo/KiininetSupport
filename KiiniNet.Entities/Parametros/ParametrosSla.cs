                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               using System.Runtime.Serialization;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ParametrosSla
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int MinimoDias { get; set; }
        [DataMember]
        public int MinimoHoras { get; set; }
        [DataMember]
        public int MinimoMinutos { get; set; }
        [DataMember]
        public int MinimoSegundos { get; set; }
        [DataMember]
        public int MaximoDias { get; set; }
        [DataMember]
        public int MaximoHoras { get; set; }
        [DataMember]
        public int MaximoMinutos { get; set; }
        [DataMember]
        public int MaximoSegundos { get; set; }
    }
}
