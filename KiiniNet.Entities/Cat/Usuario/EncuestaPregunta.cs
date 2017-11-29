using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class EncuestaPregunta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdEncuesta { get; set; }
        [DataMember]
        public string Pregunta { get; set; }
        [DataMember]
        public virtual Encuesta Encuesta { get; set; }
        [DataMember]
        public decimal Ponderacion { get; set; }
        [DataMember]
        public List<RespuestaEncuesta> RespuestaEncuesta { get; set; } 
    }
}
