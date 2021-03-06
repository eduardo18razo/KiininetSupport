﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class Usuario
    {
        #region Mapeo
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdUbicacion { get; set; }
        [DataMember]
        public int IdOrganizacion { get; set; }
        [DataMember]
        public string ApellidoPaterno { get; set; }
        [DataMember]
        public string ApellidoMaterno { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public bool DirectorioActivo { get; set; }
        [DataMember]
        public string NombreUsuario { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public int? IdPuesto { get; set; }
        [DataMember]
        public bool Vip { get; set; }
        [DataMember]
        public byte[] Foto { get; set; }
        [DataMember]
        public bool PersonaFisica { get; set; }
        [DataMember]
        public bool Autoregistro { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public bool Activo { get; set; }
        [DataMember]
        public int Tries { get; set; }
        [DataMember]
        public DateTime? FechaBloqueo { get; set; }
        [DataMember]
        public DateTime? FechaUpdate { get; set; }
        [DataMember]
        public DateTime? FechaAlta { get; set; }
        [DataMember]
        public DateTime? FechaActualizacion { get; set; }

        [DataMember]
        public bool Sistema { get; set; }
        [DataMember]
        public bool LevantaTickets { get; set; }
        [DataMember]
        public bool LevantaRecado { get; set; }

        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual Ubicacion Ubicacion { get; set; }
        [DataMember]
        public virtual Organizacion Organizacion { get; set; }
        [DataMember]
        public virtual List<CorreoUsuario> CorreoUsuario { get; set; }
        [DataMember]
        public virtual List<TelefonoUsuario> TelefonoUsuario { get; set; }
        [DataMember]
        public virtual List<UsuarioRol> UsuarioRol { get; set; }
        [DataMember]
        public virtual List<UsuarioGrupo> UsuarioGrupo { get; set; }
        [DataMember]
        public virtual List<HitConsulta> HitConsulta { get; set; }
        [DataMember]
        public virtual List<Ticket> TicketsLevantados { get; set; }
        [DataMember]
        public virtual List<Ticket> TicketsResueltos { get; set; }
        [DataMember]
        public virtual List<Ticket> TicketsSolicitados { get; set; }
        [DataMember]
        public virtual List<TicketEstatus> TicketEstatus { get; set; }
        [DataMember]
        public virtual List<TicketAsignacion> TicketAsigno { get; set; }
        [DataMember]
        public virtual List<TicketAsignacion> TicketAsignado { get; set; }
        [DataMember]
        public virtual Puesto Puesto { get; set; }
        [DataMember]
        public virtual List<UsuarioLinkPassword> UsuarioLinkPassword { get; set; }
        [DataMember]
        public virtual List<SmsService> SmsService { get; set; }
        [DataMember]
        public virtual List<PreguntaReto> PreguntaReto { get; set; }
        [DataMember]
        public virtual List<UsuarioPassword> UsuarioPassword { get; set; }
        [DataMember]
        public virtual List<PreTicket> PreTicketSolicitado { get; set; }
        [DataMember]
        public virtual List<PreTicket> PreTicketLevantado { get; set; }
        [DataMember]
        public virtual List<NotaGeneral> NotaGeneral { get; set; }
        [DataMember]
        public virtual List<NotaOpcionUsuario> NotaOpcionUsuario { get; set; }
        [DataMember]
        public virtual List<NotaOpcionGrupo> NotaOpcionGrupo { get; set; }
        [DataMember]
        public virtual List<TicketConversacion> TicketConversacion { get; set; }
        [DataMember]
        public virtual List<BitacoraAcceso> BitacoraAcceso { get; set; }


        [DataMember]
        public virtual List<InformacionConsulta> InformacionConsultaAlta { get; set; }
        [DataMember]
        public virtual List<InformacionConsulta> InformacionConsultaModifico { get; set; }

        [DataMember]
        public virtual List<InformacionConsultaDocumentos> InformacionConsultaDocumentosAlta { get; set; }
        [DataMember]
        public virtual List<InformacionConsultaDocumentos> InformacionConsultaDocumentosModifico { get; set; }
        [DataMember]
        public virtual List<Mascara> MascaraAlta { get; set; }
        [DataMember]
        public virtual List<Mascara> MascaraModifico { get; set; }
        [DataMember]
        public virtual List<InformacionConsultaRate> InformacionConsultaRate { get; set; }
        [DataMember]
        public virtual List<TicketEvento> TicketEvento { get; set; }
        [DataMember]
        public virtual List<UsuarioNotificacion> UsuarioNotificacion { get; set; }
        [DataMember]
        public virtual List<CorreoService> CorreoService { get; set; }

        [DataMember]
        public virtual List<TicketNotificacion> TicketNotificacion { get; set; }
        [DataMember]
        public virtual List<GraficosFavoritos> GraficosFavoritos { get; set; }
        [DataMember]
        public virtual List<Ticket> TicketsUltimoMovimiento { get; set; }
        [DataMember]
        public virtual List<Ticket> UltimosTicketsAsignados { get; set; }
        #endregion Mapeo

        [DataMember]
        public bool Administrador { get; set; }
        [DataMember]
        public bool Supervisor { get; set; }
        public string NombreCompleto { get { return Nombre + " " + ApellidoPaterno + " " + ApellidoMaterno; } }
        [DataMember]
        public string OrganizacionCompleta { get; set; }
        [DataMember]
        public string OrganizacionFinal { get; set; }
        [DataMember]
        public string UbicacionCompleta { get; set; }
        [DataMember]
        public string UbicacionFinal { get; set; }
        [DataMember]
        public string CorreoPrincipal { get; set; }
        [DataMember]
        public string TelefonoPrincipal { get; set; }
        [DataMember]
        public string FechaUltimoAccesoExito
        {
            get; set; 
        }
    }
}
