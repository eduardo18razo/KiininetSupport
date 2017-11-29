using KiiniNet.Entities.Cat.Operacion;

namespace KiiniNet.Entities.Operacion
{
    public class InventarioInfConsulta
    {
        public int Id { get; set; }
        public int IdInventario { get; set; }
        public int IdInfConsulta { get; set; }
        public virtual InventarioArbolAcceso InventarioArbolAcceso { get; set; }
        public virtual InformacionConsulta InformacionConsulta { get; set; }
    }
}
