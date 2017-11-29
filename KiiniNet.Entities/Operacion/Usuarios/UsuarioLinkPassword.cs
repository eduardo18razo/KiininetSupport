using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class UsuarioLinkPassword
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int IdTipoLink { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public Guid Link { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public bool Activo { get; set; }
        [DataMember]
        public virtual TipoLink TipoLink { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
