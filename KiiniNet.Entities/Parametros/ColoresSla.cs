using System.Runtime.Serialization;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ColoresSla
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public bool Dentro { get; set; }
    }
}
