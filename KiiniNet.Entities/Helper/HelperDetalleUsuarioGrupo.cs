using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperDetalleUsuarioGrupo
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string NombreUsuarioCompleto { get; set; }

        public string Supervisor { get; set; }
        public string PrimerNivel { get; set; }
        public string SegundoNivel { get; set; }
        public string TercerNivel { get; set; }
        public string CuartoNivel { get; set; }
        public string Activo { get; set; }
        
    }
}
