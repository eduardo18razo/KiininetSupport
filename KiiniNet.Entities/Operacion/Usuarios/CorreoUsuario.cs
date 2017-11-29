using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class CorreoUsuario
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Correo { get; set; }
        [DataMember]
        public bool Obligatorio { get; set; }
        
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
