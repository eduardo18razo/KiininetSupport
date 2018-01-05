using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Parametros
{
    public class ParametroDatosAdicionales
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int? IdMascara { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual Mascara Mascara { get; set; }
        
    }
}
