using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Parametros
{
    public class SubRolEscalacionPermitida
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdSubRol { get; set; }
        [DataMember]
        public int IdEstatusAsignacion { get; set; }
        [DataMember]
        public int IdSubRolPermitido { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual SubRol SubRol { get; set; }

        [DataMember]
        public virtual SubRol SubRolPermitido { get; set; }

        [DataMember]
        public virtual EstatusAsignacion EstatusAsignacion { get; set; }
    }
}
