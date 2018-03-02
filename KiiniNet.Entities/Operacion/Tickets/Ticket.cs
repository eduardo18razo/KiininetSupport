using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Operacion.Tickets
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Ticket
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdTipoArbolAcceso { get; set; }
        [DataMember]
        public int IdArbolAcceso { get; set; }
        [DataMember]
        public int IdImpacto { get; set; }
        [DataMember]
        public int IdUsuarioLevanto { get; set; }
        [DataMember]
        public int IdUsuarioSolicito { get; set; }
        [DataMember]
        public int IdOrganizacion { get; set; }
        [DataMember]
        public int IdUbicacion { get; set; }
        [DataMember]
        public int? IdMascara { get; set; }
        [DataMember]
        public int? IdEncuesta { get; set; }
        [DataMember]
        public int? IdSlaEstimadoTicket { get; set; }
        [DataMember]
        public int IdEstatusTicket { get; set; }
        [DataMember]
        public int IdCanal { get; set; }
        [DataMember]
        public int? IdNivelTicket { get; set; }
        [DataMember]
        public int IdEstatusAsignacion { get; set; }
        [DataMember]
        public DateTime FechaHoraAlta { get; set; }
        [DataMember]
        public DateTime FechaHoraFinProceso { get; set; }
        [DataMember]
        public DateTime? FechaTermino { get; set; }
        [DataMember]
        public int? IdUsuarioResolvio { get; set; }
        [DataMember]
        public bool DentroSla { get; set; }
        [DataMember]
        public bool Random { get; set; }
        [DataMember]
        public string ClaveRegistro { get; set; }
        [DataMember]
        public bool EncuestaRespondida { get; set; }
        [DataMember]
        public bool EsTercero { get; set; }
        [DataMember]
        public bool Espera { get; set; }
        [DataMember]
        public string TiempoEspera { get; set; }
        [DataMember]
        public DateTime? FechaInicioEspera { get; set; }
        [DataMember]
        public DateTime? FechaFinEspera { get; set; }

        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual TipoArbolAcceso TipoArbolAcceso { get; set; }
        [DataMember]
        public virtual ArbolAcceso ArbolAcceso { get; set; }
        [DataMember]
        public virtual Impacto Impacto { get; set; }
        [DataMember]
        public virtual Usuario UsuarioLevanto { get; set; }
        [DataMember]
        public virtual Usuario UsuarioResolvio { get; set; }
        [DataMember]
        public virtual Usuario UsuarioSolicito { get; set; }
        [DataMember]
        public virtual Organizacion Organizacion { get; set; }
        [DataMember]
        public virtual Ubicacion Ubicacion { get; set; }
        [DataMember]
        public virtual Mascara Mascara { get; set; }
        [DataMember]
        public virtual Encuesta Encuesta { get; set; }
        [DataMember]
        public virtual SlaEstimadoTicket SlaEstimadoTicket { get; set; }
        [DataMember]
        public virtual EstatusTicket EstatusTicket { get; set; }
        [DataMember]
        public virtual Canal Canal { get; set; }
        [DataMember]
        public virtual NivelTicket NivelTicket { get; set; }
        [DataMember]
        public virtual EstatusAsignacion EstatusAsignacion { get; set; }
        [DataMember]
        public virtual List<RespuestaEncuesta> RespuestaEncuesta { get; set; }
        [DataMember]
        public virtual List<TicketGrupoUsuario> TicketGrupoUsuario { get; set; }
        [DataMember]
        public virtual List<TicketEstatus> TicketEstatus { get; set; }
        [DataMember]
        public virtual List<TicketAsignacion> TicketAsignacion { get; set; }
        [DataMember]
        public virtual List<TicketConversacion> TicketConversacion { get; set; }

        [DataMember]
        public virtual List<MascaraSeleccionCatalogo> MascaraSeleccionCatalogo { get; set; }
        [DataMember]
        public virtual List<TicketCorreo> TicketCorreo { get; set; }
        [DataMember]
        public virtual List<TicketEvento> TicketEvento { get; set; }

    }
}
