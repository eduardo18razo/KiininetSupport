using System;
using System.Collections.Generic;
using System.Data;

namespace KiiniNet.Entities.Operacion.Dashboard
{
    [Serializable]
    public class DashboardAdministrador
    {
        public int UsuariosRegistrados { get; set; }
        public int UsuariosActivos { get; set; }
        public int TicketsCreados { get; set; }
        public int Operadores { get; set; }
        public DataTable GraficoTicketsCreadosCanal { get; set; }
        public DataTable GraficoUsuariosRegistrados { get; set; }
        public DataTable GraficoAlmacenamiento { get; set; }
        public long TotalArchivos { get; set; }
        public int Categorias { get; set; }
        public int Articulos { get; set; }
        public int Formularios { get; set; }
        public int Catalogos { get; set; }
        public List<GraficoConteo> OperadorRol { get; set; }
        public int Organizacion { get; set; }
        public int Ubicacion { get; set; }
        public int Puestos { get; set; }

        public int Grupos { get; set; }
        public int Horarios { get; set; }
        public int Feriados { get; set; }

    }
}