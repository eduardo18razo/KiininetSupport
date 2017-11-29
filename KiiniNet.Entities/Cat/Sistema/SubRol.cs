using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class SubRol
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdRol { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public int? OrdenAsignacion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual Rol Rol { get; set; }

        [DataMember]
        public virtual List<SubGrupoUsuario> SubGrupoUsuario { get; set; }
        [DataMember]
        public virtual List<GrupoUsuarioInventarioArbol> GrupoUsuarioInventarioArbol { get; set; }
        [DataMember]
        public List<EstatusTicketSubRolGeneral> EstatusTicketSubRolGeneralSolicita { get; set; }
        [DataMember]
        public List<EstatusTicketSubRolGeneral> EstatusTicketSubRolGeneralPertenece { get; set; }
        [DataMember]
        public List<EstatusTicketSubRolGeneralDefault> EstatusTicketSubRolGeneralDefaultSolicita { get; set; }
        [DataMember]
        public List<EstatusTicketSubRolGeneralDefault> EstatusTicketSubRolGeneralDefaultPertenece { get; set; }

        [DataMember]
        public List<EstatusAsignacionSubRolGeneral> EstatusAsignacionSubRolGeneral { get; set; }
        
        [DataMember]
        public List<EstatusAsignacionSubRolGeneralDefault> EstatusAsignacionSubRolGeneralDefault { get; set; }
        [DataMember]
        public List<SubRolEscalacionPermitida> SubRolEscalacionPermitida { get; set; }
        [DataMember]
        public List<SubRolEscalacionPermitida> SubRolEscalacionPermitidaPermitido { get; set; }
    }
}
