using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class RespuestaEncuesta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int? IdTicket { get; set; }
        [DataMember]
        public int? IdArbol { get; set; }
        [DataMember]
        public int IdEncuesta { get; set; }
        [DataMember]
        public int IdPregunta { get; set; }
        [DataMember]
        public int ValorRespuesta { get; set; }
        [DataMember]
        public decimal Ponderacion { get; set; }
        [DataMember]
        public virtual Ticket Ticket { get; set; }

        [DataMember]
        public virtual Encuesta Encuesta { get; set; }
        [DataMember]
        public virtual EncuestaPregunta EncuestaPregunta { get; set; }
        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }
    }
}
