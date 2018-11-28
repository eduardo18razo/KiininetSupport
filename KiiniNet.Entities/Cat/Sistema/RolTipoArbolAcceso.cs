using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class RolTipoArbolAcceso
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoArbolAcceso { get; set; }
        [DataMember]
        public int IdRol { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual Rol Rol { get; set; }
        [DataMember]
        public virtual TipoArbolAcceso TipoArbolAcceso { get; set; }
    }
}
