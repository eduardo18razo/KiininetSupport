using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;

namespace KiiniNet.Entities.Cat.Operacion
{
    [DataContract(IsReference = true)]
    public class InventarioArbolAcceso
    {
        public int Id { get; set; }

        [DataMember]
        public int IdArbolAcceso { get; set; }
        [DataMember]
        public int? IdMascara { get; set; }
        [DataMember]
        public int? IdSla { get; set; }
        
        [DataMember]
        public int? IdEncuesta { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }
        [DataMember]
        public virtual Mascara Mascara { get; set; }
        [DataMember]
        public virtual Sla Sla { get; set; }
        
       
        [DataMember]
        public virtual Encuesta Encuesta { get; set; }
        [DataMember]
        public virtual List<InventarioInfConsulta> InventarioInfConsulta { get; set; } 

        [DataMember]
        public virtual List<GrupoUsuarioInventarioArbol> GrupoUsuarioInventarioArbol { get; set; } 
    }
}

