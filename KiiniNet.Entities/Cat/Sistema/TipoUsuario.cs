using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class TipoUsuario
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
        public bool EsResidente { get; set; }
        [DataMember]
        public bool EsMoral { get; set; }
        [DataMember]
        public int NumeroCorreos { get; set; }
        [DataMember]
        public int CorreosObligatorios { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }


        [DataMember]
        public virtual List<Entities.Operacion.Usuarios.Usuario> Usuario { get; set; }

        [DataMember]
        public virtual List<Ubicacion> Ubicacion { get; set; }
        [DataMember]
        public virtual List<Pais> Pais { get; set; }
        [DataMember]
        public virtual List<Campus> Campus { get; set; }
        [DataMember]
        public virtual List<Torre> Torre { get; set; }
        [DataMember]
        public virtual List<Piso> Piso { get; set; }
        [DataMember]
        public virtual List<Zona> Zona { get; set; }
        [DataMember]
        public virtual List<SubZona> SubZona { get; set; }
        [DataMember]
        public virtual List<SiteRack> SiteRack { get; set; }

        [DataMember]
        public virtual List<Organizacion> Organizacion { get; set; }
        [DataMember]
        public virtual List<Holding> Holding { get; set; }
        [DataMember]
        public virtual List<Compania> Compania { get; set; }
        [DataMember]
        public virtual List<Direccion> Direccion { get; set; }
        [DataMember]
        public virtual List<SubDireccion> SubDireccion { get; set; }
        [DataMember]
        public virtual List<Gerencia> Gerencia { get; set; }
        [DataMember]
        public virtual List<SubGerencia> SubGerencia { get; set; }
        [DataMember]
        public virtual List<Jefatura> Jefatura { get; set; }

        [DataMember]
        public virtual List<ParametrosTelefonos> ParametrosTelefonos { get; set; }
        [DataMember]
        public virtual List<RolTipoUsuario> RolTipoUsuario { get; set; }
        [DataMember]
        public virtual List<Nivel1> Nivel1 { get; set; }
        [DataMember]
        public virtual List<Nivel2> Nivel2 { get; set; }
        [DataMember]
        public virtual List<Nivel3> Nivel3 { get; set; }
        [DataMember]
        public virtual List<Nivel4> Nivel4 { get; set; }
        [DataMember]
        public virtual List<Nivel5> Nivel5 { get; set; }
        [DataMember]
        public virtual List<Nivel6> Nivel6 { get; set; }
        [DataMember]
        public virtual List<Nivel7> Nivel7 { get; set; }
        [DataMember]
        public virtual List<ArbolAcceso> ArbolAcceso { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }
        [DataMember]
        public virtual List<GrupoUsuario> GrupoUsuario { get; set; }
        [DataMember]
        public virtual List<Puesto> Puesto { get; set; }
        [DataMember]
        public virtual List<AliasOrganizacion> AliasOrganizacion { get; set; }
        [DataMember]
        public virtual List<AliasUbicacion> AliasUbicacion { get; set; }
        [DataMember]
        public virtual List<Frecuencia> Frecuencia { get; set; }
        [DataMember]
        public virtual List<RolMenu> RolMenu { get; set; }
        [DataMember]
        public virtual List<ParametrosUsuario> ParametrosUsuario { get; set; }
        [DataMember]
        public virtual List<ParametroDatosAdicionales> ParametroDatosAdicionales { get; set; }

        [DataMember]
        public virtual List<HitConsulta> HitConsulta { get; set; }
        [DataMember]
        public virtual List<GrupoUsuarioDefaultOpcion> GrupoUsuarioDefaultOpcion { get; set; }
    }
}
