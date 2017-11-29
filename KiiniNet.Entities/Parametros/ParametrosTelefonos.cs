using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ParametrosTelefonos
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdTipoTelefono { get; set; }
        [DataMember]
        public int NumeroTelefonos { get; set; }
        [DataMember]
        public int Obligatorios { get; set; }

        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }

        [DataMember]
        public virtual TipoTelefono TipoTelefono { get; set; }
    }
}
