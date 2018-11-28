using System.Runtime.Serialization;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ArchivosPermitidos
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NombreArchivo { get; set; }
        [DataMember]
        public string Extensiones { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
    }
}
