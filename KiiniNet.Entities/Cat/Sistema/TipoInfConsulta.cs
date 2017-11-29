using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoInfConsulta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool EsBaseDatos { get; set; }
        [DataMember]
        public bool EsDirectorio { get; set; }
        [DataMember]
        public bool EsUrl { get; set; }
        [DataMember]
        public bool EsServicio { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        
        [DataMember]
        public virtual List<InformacionConsulta> InformacionConsulta { get; set; }
    }
}
