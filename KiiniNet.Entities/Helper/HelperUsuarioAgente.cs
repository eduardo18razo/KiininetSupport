using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperUsuarioAgente
    {
        public int IdUsuario { get; set; }
        public int IdSubRol { get; set; }
        public string DescripcionSubRol { get; set; }
        public string NombreUsuario { get; set; }
    }
}
