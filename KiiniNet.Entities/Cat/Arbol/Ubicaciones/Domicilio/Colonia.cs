using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio
{
    [DataContract(IsReference = true)]
    public class Colonia
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? IdMunicipio { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public int CP { get; set; }

        [DataMember]
        public virtual Municipio Municipio { get; set; }
        [DataMember]
        public virtual List<Domicilio> Domicilio { get; set; }

    }
}
