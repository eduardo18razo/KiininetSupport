using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class PreTicketCorreo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Guid { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string ApellidoPaterno { get; set; }
        [DataMember]
        public string ApellidoMaterno { get; set; }
        [DataMember]
        public string Correo { get; set; }
        [DataMember]
        public string Asunto { get; set; }
        [DataMember]
        public string Comentario { get; set; }
        [DataMember]
        public string ArchivoAdjunto { get; set; }
        [DataMember]
        public bool Confirmado { get; set; }
        [DataMember]
        public DateTime FechaSolicito { get; set; }
        [DataMember]
        public DateTime? FechaConfirmo { get; set; }
    }
}
