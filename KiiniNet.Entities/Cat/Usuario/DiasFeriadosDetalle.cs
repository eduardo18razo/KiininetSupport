using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class    DiasFeriadosDetalle
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdDiasFeriados { get; set; }
        [DataMember]
        public DateTime Dia { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual DiasFeriados DiasFeriados { get; set; }
    }
}
