using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoLink
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<UsuarioLinkPassword> UsuarioLinkPassword { get; set; }
        [DataMember]
        public virtual List<SmsService> SmsService { get; set; }
    }
}
