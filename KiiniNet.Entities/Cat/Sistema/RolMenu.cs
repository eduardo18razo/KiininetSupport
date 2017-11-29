using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class RolMenu
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdRol { get; set; }
        [DataMember]
        public int IdMenu { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public virtual Rol Rol { get; set; }
        [DataMember]
        public virtual Menu Menu { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
    }
}
