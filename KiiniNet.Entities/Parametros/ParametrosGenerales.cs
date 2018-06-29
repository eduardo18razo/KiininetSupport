using System.Runtime.Serialization;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ParametrosGenerales
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string TamanoDeArchivo { get; set; }
        [DataMember]
        public string NumeroArchivo { get; set; }
        [DataMember]
        public bool LevantaTickets { get; set; }
        [DataMember]
        public int PeriodoDashboard { get; set; }
        [DataMember]
        public bool ValidaUsuario { get; set; }
        [DataMember]
        public string MensajeValidacion { get; set; }
        [DataMember]
        public bool StrongPassword { get; set; }
        [DataMember]
        public string FormularioPortal { get; set; }
        [DataMember]
        public int FrecuenciaDemonioSegundos { get; set; }
        [DataMember]
        public int FrecuenciaNotificacionMinutos { get; set; }
        [DataMember]
        public int MensajesNotificacion { get; set; }
        [DataMember]
        public int? DiasCierreTicket { get; set; }
    }
}
