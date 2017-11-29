using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Cat.Operacion
{
    [DataContract(IsReference = true)]
    public class GrupoUsuarioInventarioArbol
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdInventarioArbolAcceso { get; set; }
        [DataMember]
        public int IdRol { get; set; }
        [DataMember]
        public int IdGrupoUsuario { get; set; }
        [DataMember]
        public int? IdSubGrupoUsuario { get; set; }
        [DataMember]
        public virtual Rol Rol { get; set; }
        [DataMember]
        public virtual GrupoUsuario GrupoUsuario { get; set; }
        [DataMember]
        public virtual SubGrupoUsuario SubGrupoUsuario { get; set; }
        [DataMember]
        public virtual InventarioArbolAcceso InventarioArbolAcceso { get; set; }


    }
}
