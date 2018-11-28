using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoTelefono
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Extension { get; set; }
        [DataMember]
        public bool Confirmacion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public List<TelefonoUsuario> TelefonoUsuario { get; set; }
    }
}
