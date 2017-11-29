using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio
{
    [DataContract(IsReference = true)]
    public class Municipio
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? IdEstado { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public virtual Estado Estado { get; set; }
        [DataMember]
        public virtual List<Colonia> Colonia { get; set; }
    }
}
