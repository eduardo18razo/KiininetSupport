using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class Rol
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool EsUnico { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual List<RolTipoUsuario> RolTipoUsuario { get; set; }
        [DataMember]
        public virtual List<RolTipoGrupo> RolTipoGrupo { get; set; }
        [DataMember]
        public virtual List<SubRol> SubRol { get; set; }
        [DataMember]
        public virtual List<UsuarioGrupo> UsuarioGrupo { get; set; }
        [DataMember]
        public virtual List<GrupoUsuarioInventarioArbol> GrupoUsuarioInventarioArbol { get; set; }
        [DataMember]
        public virtual List<RolMenu> RolMenu { get; set; }

        [DataMember]
        public virtual List<EstatusAsignacionSubRolGeneral> EstatusAsignacionSubRolGeneral { get; set; }

        [DataMember]
        public virtual List<EstatusAsignacionSubRolGeneralDefault> EstatusAsignacionSubRolGeneralDefault { get; set; }

        [DataMember]
        public virtual List<EstatusTicketSubRolGeneral> EstatusTicketSubRolGeneralSolicita { get; set; }
        [DataMember]
        public virtual List<EstatusTicketSubRolGeneral> EstatusTicketSubRolGeneralPertenece { get; set; }

        [DataMember]
        public virtual List<EstatusTicketSubRolGeneralDefault> EstatusTicketSubRolGeneralDefaultSolicita { get; set; }
        [DataMember]
        public virtual List<EstatusTicketSubRolGeneralDefault> EstatusTicketSubRolGeneralDefaultPertenece { get; set; }
        [DataMember]
        public virtual List<HitGrupoUsuario> HitGrupoUsuario { get; set; } 
    }
}
