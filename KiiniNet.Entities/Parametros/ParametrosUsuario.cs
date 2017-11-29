using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Parametros
{
    //ECL
    [DataContract(IsReference = true)]
    public class ParametrosUsuario
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdOrganizacion { get; set; }
        [DataMember]
        public int IdUbicacion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual Organizacion Organizacion { get; set; }
        [DataMember]
        public virtual Ubicacion Ubicacion { get; set; }
    }
}
