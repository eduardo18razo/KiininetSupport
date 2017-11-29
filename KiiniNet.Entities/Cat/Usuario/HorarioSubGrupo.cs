using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class HorarioSubGrupo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdSubGrupoUsuario { get; set; }
        [DataMember]
        public int IdHorario { get; set; }
        [DataMember]
        public int Dia { get; set; }
        [DataMember]
        public string HoraInicio { get; set; }
        [DataMember]
        public string HoraFin { get; set; }
        [DataMember]
        public virtual SubGrupoUsuario SubGrupoUsuario { get; set; }
        [DataMember]
        public virtual Horario Horario { get; set; }
    }
}
