using System.Runtime.Serialization;

namespace KiiniNet.Entities.Parametros
{
    [DataContract(IsReference = true)]
    public class ParametroPassword
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Min { get; set; }
        [DataMember]
        public int Max { get; set; }
        [DataMember]
        public bool Letras { get; set; }
        [DataMember]
        public bool Numeros { get; set; }
        [DataMember]
        public bool Especiales { get; set; }
        [DataMember]
        public int Mayusculas { get; set; }
        [DataMember]
        public bool ValidaAnterior { get; set; }
        [DataMember]
        public int AlmacenAnterior { get; set; }
        [DataMember]
        public bool Fail { get; set; }
        [DataMember]
        public int TimeoutFail { get; set; }
        [DataMember]
        public int Tries { get; set; }
        [DataMember]
        public bool CaducaPassword { get; set; }
        [DataMember]
        public int TiempoCaducidad { get; set; }
    }
}
