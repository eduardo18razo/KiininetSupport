using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class InformacionConsulta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoInfConsulta { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
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
        public Usuario UsuarioAlta { get; set; }
        [DataMember]
        public Usuario UsuarioModifico { get; set; }
        [DataMember]
        public virtual TipoInfConsulta TipoInfConsulta { get; set; }

        [DataMember]
        public virtual List<InventarioInfConsulta> InventarioInfConsulta { get; set; }
        [DataMember]
        public virtual List<InformacionConsultaDatos> InformacionConsultaDatos { get; set; }
        [DataMember]
        public virtual List<InformacionConsultaDocumentos> InformacionConsultaDocumentos { get; set; }
        [DataMember]
        public virtual List<InformacionConsultaRate> InformacionConsultaRate { get; set; }

    }
}
