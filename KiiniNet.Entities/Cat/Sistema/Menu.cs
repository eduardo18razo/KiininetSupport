using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Sistema
{
    [DataContract(IsReference = true)]
    public class Menu
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public int? IdPadre { get; set; }
        [DataMember]
        public int? Orden { get; set; }

        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<Menu> Menu1 { get; set; }
        [DataMember]
        public virtual Menu Menu2 { get; set; }
        [DataMember]
        public virtual List<RolMenu> RolMenu { get; set; }
        [DataMember]
        public virtual List<GraficosDefault> GraficosDefault { get; set; }
        [DataMember]
        public virtual List<GraficosFavoritos> GraficosFavoritos { get; set; }
    }
}
