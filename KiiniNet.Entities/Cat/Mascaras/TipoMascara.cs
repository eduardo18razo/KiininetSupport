using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Mascaras
{
    [DataContract(IsReference = true)]
    public class TipoMascara
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string TablaLlave { get; set; }
        [DataMember]
        public string CampoLlave { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<Mascara> Mascara { get; set; }
    }
}
