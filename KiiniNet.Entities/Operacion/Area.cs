using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class Area
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public byte[] Imagen { get; set; }
        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public bool Sistema { get; set; }
        [DataMember]
        public DateTime FechaAlta { get; set; }
        [DataMember]
        public int IdUsuarioAlta { get; set; }
        [DataMember]
        public DateTime? FechaModificacion { get; set; }
        [DataMember]
        public int? IdUsuarioModifico { get; set; }

        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<ArbolAcceso> ArbolAcceso { get; set; }
    }
}
