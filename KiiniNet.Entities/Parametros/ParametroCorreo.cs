using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ParametroCorreo
    {

        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoCorreo { get; set; }
        [DataMember]
        public string Contenido { get; set; }
        [DataMember]
        public bool Predefinido { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        public virtual TipoCorreo TipoCorreo { get; set; }

    }
}
