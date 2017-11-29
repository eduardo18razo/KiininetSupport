using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class UsuarioRol
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public int IdRolTipoUsuario { get; set; }

        [DataMember]
        public virtual RolTipoUsuario RolTipoUsuario { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
