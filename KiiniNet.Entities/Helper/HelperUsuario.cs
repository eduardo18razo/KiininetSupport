using System.Collections.Generic;

namespace KiiniNet.Entities.Helper
{
    public class HelperUsuario
    {
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; }
        public string TipoUsuarioDescripcion { get; set; }
        public string TipoUsuarioColor { get; set; }
        public bool Vip { get; set; }
        public string FechaUltimoLogin { get; set; }
        public int NumeroTicketsAbiertos { get; set; }

        public List<HelperTicketsUsuario> TicketsAbiertos { get; set; }

        public string Puesto { get; set; }
        public List<string> Correos { get; set; }
        public List<string> Telefonos { get; set; }
        public string Organizacion { get; set; }
        public string Ubicacion { get; set; }

        public string Creado { get; set; }
        public string UltimaActualizacion { get; set; }
    }
}
