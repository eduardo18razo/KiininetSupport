using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class PreguntaReto
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Pregunta { get; set; }
        [DataMember]
        public string Respuesta { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
