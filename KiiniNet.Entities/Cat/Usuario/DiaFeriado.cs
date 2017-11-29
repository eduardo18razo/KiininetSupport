using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class DiaFeriado
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public DateTime? FechaAlta { get; set; }
        [DataMember]
        public int? IdUsuarioAlta { get; set; }
        [DataMember]
        public DateTime? FechaModificacion { get; set; }
        [DataMember]
        public int? IdUsuarioModifico { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
    }
}
