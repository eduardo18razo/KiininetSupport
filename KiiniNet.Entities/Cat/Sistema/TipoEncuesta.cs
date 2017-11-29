using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoEncuesta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<Encuesta> Encuesta { get; set; }
        [DataMember]
        public virtual List<RespuestaTipoEncuesta> RespuestaTipoEncuesta { get; set; }
    }
}
