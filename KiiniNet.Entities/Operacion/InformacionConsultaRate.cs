using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class InformacionConsultaRate
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdInformacionConsulta { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public bool MeGusta { get; set; }
        [DataMember]
        public bool NoMeGusta { get; set; }
        [DataMember]
        public DateTime FechaModificacion { get; set; }

        [DataMember]
        public virtual InformacionConsulta InformacionConsulta { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
