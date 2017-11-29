using System;
using System.Collections.Generic;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperDetalleTicket
    {
        public int IdTicket { get; set; }
        public int IdTipoUsuarioLevanto { get; set; }
        public int IdEstatusTicket { get; set; }
        public string CveRegistro { get; set; }
        public int IdEstatusAsignacion { get; set; }
        public int IdUsuarioLevanto { get; set; }
        public string EstatusActual { get; set; }
        public string AsignacionActual { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool TieneEncuesta { get; set; }
        public List<HelperEstatusDetalle> EstatusDetalle { get; set; }
        public List<HelperAsignacionesDetalle> AsignacionesDetalle { get; set; }
        public List<HelperConversacionDetalle> ConversacionDetalle { get; set; }
    }
    [Serializable]
    public class HelperConversacionDetalle
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaHora { get; set; }

        public string Comentario { get; set; }
        public bool Privado { get; set; }
        public List<HelperConversacionArchivo> Archivo { get; set; }
    }
    [Serializable]
    public class HelperConversacionArchivo
    {
        public int IdConversacion { get; set; }
        public string Archivo { get; set; }
    }
    [Serializable]
    public class HelperAsignacionesDetalle
    {
        public string Descripcion { get; set; }
        public string UsuarioAsigno { get; set; }
        public string UsuarioAsignado { get; set; }
        public DateTime FechaMovimiento { get; set; }

    }
    [Serializable]
    public class HelperEstatusDetalle
    {
        public string Descripcion { get; set; }
        public string UsuarioMovimiento { get; set; }
        public string Comentarios { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
