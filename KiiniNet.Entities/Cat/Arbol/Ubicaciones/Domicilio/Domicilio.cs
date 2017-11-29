using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio
{
    [DataContract(IsReference = true)]
    public class Domicilio
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdCampus { get; set; }
        [DataMember]
        public int IdColonia { get; set; }
        [DataMember]
        public string Calle { get; set; }
        [DataMember]
        public string NoExt { get; set; }
        [DataMember]
        public string NoInt { get; set; }
        
        [DataMember]
        public virtual Campus Campus { get; set; }
        [DataMember]
        public virtual Colonia Colonia { get; set; }
    }
}
