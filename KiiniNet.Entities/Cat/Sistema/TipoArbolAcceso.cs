using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoArbolAcceso
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Abreviacion { get; set; }
        [DataMember]
        public string Color { get; set; }
        [DataMember]
        public string ColorGrafico { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<ArbolAcceso> ArbolAcceso { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }
        [DataMember]
        public virtual List<HitConsulta> HitConsulta { get; set; }
        [DataMember]
        public virtual List<Frecuencia> Frecuencia { get; set; }
        [DataMember]
        public virtual List<GrupoUsuarioDefaultOpcion> GrupoUsuarioDefaultOpcion { get; set; }
        [DataMember]
        public virtual List<RolTipoArbolAcceso> RolTipoArbolAcceso { get; set; }
    }
}
