using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class CampoCatalogo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdCatalogo { get; set; }
        [DataMember]
        public string Campo { get; set; }
        [DataMember]
        public string TipoCampo { get; set; }
        [DataMember]
        public virtual Catalogos Catalogos { get; set; } 
    }
}
