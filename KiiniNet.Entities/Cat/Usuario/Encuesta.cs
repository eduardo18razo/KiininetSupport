using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class Encuesta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoEncuesta { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string TituloCliente { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool EsPonderacion { get; set; }
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
        public virtual TipoEncuesta TipoEncuesta { get; set; }
        [DataMember]
        public virtual List<InventarioArbolAcceso> InventarioArbolAcceso { get; set; }
        [DataMember]
        public virtual List<EncuestaPregunta> EncuestaPregunta { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }

        [DataMember]
        public virtual List<RespuestaEncuesta> RespuestaEncuesta { get; set; }

        [DataMember]

        public string Tipificacion { get; set; }
    }
}
