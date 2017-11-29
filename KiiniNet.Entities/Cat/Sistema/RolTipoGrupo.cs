using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class RolTipoGrupo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdRol { get; set; }
        [DataMember]
        public int IdTipoGrupo { get; set; }
        [DataMember]
        public virtual Rol Rol { get; set; }
        
        [DataMember]
        public virtual TipoGrupo TipoGrupo { get; set; }
    }
}
