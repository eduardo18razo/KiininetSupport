using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class NotaGeneralGrupo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdNotaGeneral { get; set; }
        [DataMember]
        public int IdGrupoUsuario { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual NotaGeneral NotaGeneral { get; set; }
        [DataMember]
        public virtual GrupoUsuario GrupoUsuario { get; set; }
        
    }
}
