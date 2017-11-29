using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class TelefonoUsuario
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoTelefono { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Numero { get; set; }
        [DataMember]
        public string Extension { get; set; }
        [DataMember]
        public bool Obligatorio { get; set; }
        [DataMember]
        public bool Confirmado { get; set; }
        
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual TipoTelefono TipoTelefono { get; set; }

    }
}
