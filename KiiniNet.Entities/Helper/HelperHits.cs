using System;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperHits
    {
        public int IdHit { get; set; }
        public int IdTipoArbolAcceso { get; set; }
        public int IdTipificacion { get; set; }
        public int? IdUsuario { get; set; }
        public int? IdUbicacion { get; set; }
        public int? IdOrganizacion { get; set; }
        public string TipoServicio { get; set; }
        public string Tipificacion { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoUsuarioAbreviacion { get; set; }
        public string TipoUsuarioColor { get; set; }
        public bool Vip { get; set; }
        public string Hora { get; set; }
        public string Ubicacion { get; set; }
        public string Organizacion { get; set; }
        public string FechaHora { get; set; }
        public int Total { get; set; }

        public string Rol { get; set; }
        public string Grupo { get; set; }
    }
}
