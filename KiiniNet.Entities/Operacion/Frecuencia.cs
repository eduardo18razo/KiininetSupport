using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class Frecuencia
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdTipoArbolAcceso { get; set; }
        [DataMember]
        public int IdArbolAcceso { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public int NumeroVisitas { get; set; }
        [DataMember]
        public DateTime UltimaVisita { get; set; }

        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual TipoArbolAcceso TipoArbolAcceso { get; set; }
        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }

    }
}
