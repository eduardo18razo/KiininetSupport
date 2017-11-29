using System;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class NotaOpcionGrupo
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoNota { get; set; }
        [DataMember]
        public int IdArbolAcceso { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Contenido { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public bool Aprobada { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual TipoNota TipoNota { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }
    }
}
