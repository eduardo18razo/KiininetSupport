using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;

namespace KiiniNet.Entities.Operacion
{
    [DataContract(IsReference = true)]
    public class TiempoInformeArbol
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdArbol { get; set; }
        [DataMember]
        public int IdTipoGrupo { get; set; }
        [DataMember]
        public int IdGrupoUsuario { get; set; }
        [DataMember]
        public decimal Dias { get; set; }
        [DataMember]
        public decimal Horas { get; set; }
        [DataMember]
        public decimal Minutos { get; set; }
        [DataMember]
        public decimal Segundos { get; set; }
        [DataMember]
        public decimal TiempoNotificacion { get; set; }
        [DataMember]
        public int IdTipoNotificacion { get; set; }
        [DataMember]
        public bool AntesVencimiento { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }
        [DataMember]
        public virtual TipoGrupo TipoGrupo { get; set; }
        [DataMember]
        public virtual GrupoUsuario GrupoUsuario { get; set; }
        [DataMember]
        public virtual TipoNotificacion TipoNotificacion { get; set; }
    }
}
