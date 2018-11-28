using System.Runtime.Serialization;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ColoresTop
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public int Orden { get; set; }
    }
}
