using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class InformacionConsultaDatos
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdInformacionConsulta { get; set; }
        [DataMember]
        public string Datos { get; set; }
        [DataMember]
        public string Busqueda { get; set; }
        [DataMember]
        public string Tags { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual InformacionConsulta InformacionConsulta { get; set; }
    }
}
