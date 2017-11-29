using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Operacion
{
    [DataContract(IsReference = true)]
    public class Organizacion 
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdHolding { get; set; }
        [DataMember]
        public int? IdCompania { get; set; }
        [DataMember]
        public int? IdDireccion { get; set; }
        [DataMember]
        public int? IdSubDireccion { get; set; }
        [DataMember]
        public int? IdGerencia { get; set; }
        [DataMember]
        public int? IdSubGerencia { get; set; }
        [DataMember]
        public int? IdJefatura { get; set; }
        [DataMember]
        public int IdNivelOrganizacion { get; set; }
        [DataMember]
        public bool Sistema { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        
        [DataMember]
        public virtual List<Entities.Operacion.Usuarios.Usuario> Usuario { get; set; }
        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual Holding Holding { get; set; }
        [DataMember]
        public virtual Compania Compania { get; set; }
        [DataMember]
        public virtual Direccion Direccion { get; set; }
        [DataMember]
        public virtual SubDireccion SubDireccion { get; set; }
        [DataMember]
        public virtual Gerencia Gerencia { get; set; }
        [DataMember]
        public virtual SubGerencia SubGerencia { get; set; }
        [DataMember]
        public virtual Jefatura Jefatura { get; set; }
        [DataMember]
        public virtual List<HitConsulta> HitConsulta { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }
        [DataMember]
        public virtual NivelOrganizacion NivelOrganizacion { get; set; }
        [DataMember]
        public virtual List<ParametrosUsuario> ParametrosUsuario { get; set; }

    }
}
