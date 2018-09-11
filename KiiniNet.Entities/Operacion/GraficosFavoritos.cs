using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class GraficosFavoritos
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public int IdMenu { get; set; }
        [DataMember]
        public string TipoUsuario { get; set; }
        [DataMember]
        public string Categoria { get; set; }
        [DataMember]
        public string GrupoAtencion { get; set; }
        [DataMember]
        public string Agentes { get; set; }
        [DataMember]
        public string Canal { get; set; }
        [DataMember]
        public string TipoTicket { get; set; }
        [DataMember]
        public string EstatusTicket { get; set; }
        [DataMember]
        public string Prioridad { get; set; }
        [DataMember]
        public string Sla { get; set; }
        [DataMember]
        public string Vip { get; set; }
        [DataMember]
        public string Organizacion { get; set; }
        [DataMember]
        public string Estados { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual Menu Menu { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
