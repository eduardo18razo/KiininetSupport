using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class SubGrupoUsuario
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public int IdGrupoUsuario { get; set; }
        [DataMember]
        public int IdSubRol { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual GrupoUsuario GrupoUsuario { get; set; }
        [DataMember]
        public virtual SubRol SubRol { get; set; }
        [DataMember]
        public virtual List<UsuarioGrupo> UsuarioGrupo { get; set; }
        [DataMember]
        public virtual List<GrupoUsuarioInventarioArbol> GrupoUsuarioInventarioArbol { get; set; }
        [DataMember]
        public virtual List<TicketGrupoUsuario> TicketGrupoUsuario { get; set; }
        [DataMember]
        public virtual List<HorarioSubGrupo> HorarioSubGrupo { get; set; }
        [DataMember]
        public virtual List<DiaFestivoSubGrupo> DiaFestivoSubGrupo { get; set; }
    }
}
