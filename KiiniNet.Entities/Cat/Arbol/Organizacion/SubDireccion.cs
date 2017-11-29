using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Cat.Arbol.Organizacion
{
    [DataContract(IsReference = true)]
    public class SubDireccion
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual List<Operacion.Organizacion> Organizacion { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
    }
}
