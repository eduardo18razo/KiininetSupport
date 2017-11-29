using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [DataContract(IsReference = true)]
    public class PreTicket
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string ClaveRegistro { get; set; }
        [DataMember]
        public int IdUsuarioSolicito { get; set; }
        [DataMember]
        public int IdArbol { get; set; }
        [DataMember]
        public int IdUsuarioAtendio { get; set; }
        [DataMember]
        public DateTime FechaHora { get; set; }
        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }
        [DataMember]
        public virtual Usuario UsuarioSolicito { get; set; }
        [DataMember]
        public virtual Usuario UsuarioLevanta { get; set; }
    }
}
