using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class RolTipoUsuario
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdRol { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }

        [DataMember]
        public virtual Rol Rol { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual List<UsuarioRol> UsuarioRol { get; set; }
    }
}
