using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class InformacionConsultaDocumentos
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdInformacionConsulta { get; set; }
        [DataMember]
        public int IdTipoDocumento { get; set; }
        [DataMember]
        public string Archivo { get; set; }
        [DataMember]
        public DateTime? FechaAlta { get; set; }
        [DataMember]
        public int IdUsuarioAlta { get; set; }
        [DataMember]
        public DateTime? FechaModificacion { get; set; }
        [DataMember]
        public int? IdUsuarioModifico { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual TipoDocumento TipoDocumento { get; set; }
        [DataMember]
        public Usuario UsuarioAlta { get; set; }
        [DataMember]
        public Usuario UsuarioModifico { get; set; }
        [DataMember]
        public virtual InformacionConsulta InformacionConsulta { get; set; }

    }
}
