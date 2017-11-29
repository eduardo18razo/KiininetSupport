using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class EstatusTicketSubRolGeneralDefault
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdRolSolicita { get; set; }
        [DataMember]
        public int? IdSubRolSolicita { get; set; }
        [DataMember]
        public int IdRolPertenece { get; set; }
        [DataMember]
        public int? IdSubRolPertenece { get; set; }
        [DataMember]
        public int? IdEstatusTicketActual { get; set; }
        [DataMember]
        public int? IdEstatusTicketAccion { get; set; }
        [DataMember]
        public bool? TieneSupervisor { get; set; }
        [DataMember]
        public bool? Propietario { get; set; }
        [DataMember]
        public bool? LevantaTicket { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        
        [DataMember]
        public EstatusTicket EstatusTicketActual { get; set; }
        [DataMember]
        public EstatusTicket EstatusTicketAccion { get; set; }
        [DataMember]
        public GrupoUsuario GrupoUsuario { get; set; }
        [DataMember]
        public Rol RolSolicita { get; set; }
        [DataMember]
        public SubRol SubSolicita { get; set; }
        [DataMember]
        public Rol RolPertenece { get; set; }
        [DataMember]
        public SubRol SubRolPertenece { get; set; }
    }
}
