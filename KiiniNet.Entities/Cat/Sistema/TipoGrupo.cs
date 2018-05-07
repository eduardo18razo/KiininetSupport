using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoGrupo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual List<GrupoUsuario> GrupoUsuario { get; set; }
        [DataMember]
        public virtual List<RolTipoGrupo> RolTipoGrupo { get; set; }
        [DataMember]
        public virtual List<SubRol> TipoSubGrupo { get; set; }
        [DataMember]
        public virtual List<TiempoInformeArbol> TiempoInformeArbol { get; set; }
        [DataMember]
        public virtual List<GrupoUsuarioDefaultOpcion> GrupoUsuarioDefaultOpcion { get; set; }
    }
}
