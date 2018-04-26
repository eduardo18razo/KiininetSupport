using System;
using System.Collections.Generic;

namespace KiiniNet.Entities.Helper
{
    [Serializable]
    public class HelperDetalleTicket
    {
        public int IdTicket { get; set; }
        public int IdTipoUsuarioLevanto { get; set; }
        public int IdNivelAsignado { get; set; }
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
        public byte[] Foto { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaHora { get; set; }
        public int IdUsuario { get; set; }
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

    [Serializable]
    public class HelperEvento
    {
        public Int64 IdEvento { get; set; }
        public int IdUsuarioGenero { get; set; }


        public byte[] Foto { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaHoraEvento { get; set; }
        public string FechaHoraEventoFormato { get; set; }
        public List<HelperMovimientoEvento> Movimientos { get; set; }
    }

    [Serializable]
    public class HelperMovimientoEvento
    {
        public Int64 IdMovimiento { get; set; }
        public byte[] Foto { get; set; }
        public bool EsMovimientoEstatusTicket { get; set; }
        public bool EsMovimientoAsignacion { get; set; }
        public bool EsMovimientoConversacion { get; set; }

        public int? IdEstatus { get; set; }
        public string DescripcionEstatus { get; set; }

        public int? IdEstatusAnterior { get; set; }
        public string DescripcionEstatusAnterior { get; set; }

        public int? IdUsuarioAsigno { get; set; }
        public string NombreCambioEstatus { get; set; }
        public int? IdUsuarioAsignado { get; set; }
        public string NombreUsuarioAsignado { get; set; }

        public string Comentarios { get; set; }

        public bool ComentarioPublico { get; set; }
        public string Conversacion { get; set; }
    }

}
