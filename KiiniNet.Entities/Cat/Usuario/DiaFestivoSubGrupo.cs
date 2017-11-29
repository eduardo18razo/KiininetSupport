using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class DiaFestivoSubGrupo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdSubGrupoUsuario { get; set; }
        [DataMember]
        public int IdDiasFeriados { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public virtual SubGrupoUsuario SubGrupoUsuario { get; set; }
        [DataMember]
        public virtual DiasFeriados DiasFeriados { get; set; }
    }
}
