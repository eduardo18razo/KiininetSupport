using System;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Security.Cryptography;
using System.Text;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Arbol.Organizacion;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones;
using KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio;
using KiiniNet.Entities.Cat.Mascaras;
using KiiniNet.Entities.Cat.Operacion;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Cat.Usuario;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Operacion.Usuarios;
using KiiniNet.Entities.Parametros;

namespace KinniNet.Data.Help
{
    public class DataBaseModelContext : ObjectContext
    {
        private readonly EntityConnection _connection;
        private readonly string _connectionString;
        private const string ConnectionString = "name=connection";
        private const string ContainerName = "connection";

        public DataBaseModelContext() : base(ConnectionString, ContainerName)
        {
            try
            {
                //Sistema
                _parametrosTelefonos = CreateObjectSet<ParametrosTelefonos>();
                _parametroDatosAdicionales = CreateObjectSet<ParametroDatosAdicionales>();
                _aliasOrganizacion = CreateObjectSet<AliasOrganizacion>();
                _aliasUbicacion = CreateObjectSet<AliasUbicacion>();
                _tipoUsuario = CreateObjectSet<TipoUsuario>();
                _rol = CreateObjectSet<Rol>();
                _rolTipoUsuario = CreateObjectSet<RolTipoUsuario>();
                _rolTipoGrupo = CreateObjectSet<RolTipoGrupo>();
                _tipoGrupo = CreateObjectSet<TipoGrupo>();
                _subRol = CreateObjectSet<SubRol>();
                _grupoUsuario = CreateObjectSet<GrupoUsuario>();
                _subGrupoUsuario = CreateObjectSet<SubGrupoUsuario>();
                _colonia = CreateObjectSet<Colonia>();
                _municipio = CreateObjectSet<Municipio>();
                _estado = CreateObjectSet<Estado>();
                _tipoTelefono = CreateObjectSet<TipoTelefono>();
                _catalogos = CreateObjectSet<Catalogos>();
                _tipoArbolAcceso = CreateObjectSet<TipoArbolAcceso>();
                _tipoInfConsulta = CreateObjectSet<TipoInfConsulta>();
                _tipoDocumento = CreateObjectSet<TipoDocumento>();
                _tipoEncuesta = CreateObjectSet<TipoEncuesta>();
                _respuestaTipoEncuesta = CreateObjectSet<RespuestaTipoEncuesta>();
                _menu = CreateObjectSet<Menu>();
                _rolMenu = CreateObjectSet<RolMenu>();
                _estatusTicket = CreateObjectSet<EstatusTicket>();
                _estatusAsignacion = CreateObjectSet<EstatusAsignacion>();
                _tipoNotificacion = CreateObjectSet<TipoNotificacion>();
                _impacto = CreateObjectSet<Impacto>();
                _prioridad = CreateObjectSet<Prioridad>();
                _urgencia = CreateObjectSet<Urgencia>();
                _nivelUbicacion = CreateObjectSet<NivelUbicacion>();
                _nivelOrganizacion = CreateObjectSet<NivelOrganizacion>();
                _tipoCorreo = CreateObjectSet<TipoCorreo>();
                _tipoLink = CreateObjectSet<TipoLink>();
                _canal = CreateObjectSet<Canal>();
                _tipoNota = CreateObjectSet<TipoNota>();
                _diaFestivoDefault = CreateObjectSet<DiaFestivoDefault>();
                _diaFeriado = CreateObjectSet<DiaFeriado>();
                _diasFeriados = CreateObjectSet<DiasFeriados>();
                _diasFeriadosDetalle = CreateObjectSet<DiasFeriadosDetalle>();

                //Parametros
                _subRolEscalacionPermitida = CreateObjectSet<SubRolEscalacionPermitida>();
                _parametrosSla = CreateObjectSet<ParametrosSla>();
                _parametroCorreo = CreateObjectSet<ParametroCorreo>();
                _parametrosGenerales = CreateObjectSet<ParametrosGenerales>();
                _parametroPassword = CreateObjectSet<ParametroPassword>();
                _parametrosUsuario = CreateObjectSet<ParametrosUsuario>();
                _frecuenciaFechas = CreateObjectSet<FrecuenciaFecha>();
                _graficosDefault = CreateObjectSet<GraficosDefault>();
                _graficosFavoritos = CreateObjectSet<GraficosFavoritos>();

                //Ubicacion
                _pais = CreateObjectSet<Pais>();
                _campus = CreateObjectSet<Campus>();
                _torre = CreateObjectSet<Torre>();
                _piso = CreateObjectSet<Piso>();
                _zona = CreateObjectSet<Zona>();
                _subZona = CreateObjectSet<SubZona>();
                _siteRack = CreateObjectSet<SiteRack>();

                //Organizacion
                _holding = CreateObjectSet<Holding>();
                _compania = CreateObjectSet<Compania>();
                _direccion = CreateObjectSet<Direccion>();
                _subDireccion = CreateObjectSet<SubDireccion>();
                _gerencia = CreateObjectSet<Gerencia>();
                _subGerencia = CreateObjectSet<SubGerencia>();
                _jefatura = CreateObjectSet<Jefatura>();

                //Nodos
                _nivel1 = CreateObjectSet<Nivel1>();
                _nivel2 = CreateObjectSet<Nivel2>();
                _nivel3 = CreateObjectSet<Nivel3>();
                _nivel4 = CreateObjectSet<Nivel4>();
                _nivel5 = CreateObjectSet<Nivel5>();
                _nivel6 = CreateObjectSet<Nivel6>();
                _nivel7 = CreateObjectSet<Nivel7>();

                //Mascara
                _mascara = CreateObjectSet<Mascara>();
                _tipoMascara = CreateObjectSet<TipoMascara>();
                _campoMascara = CreateObjectSet<CampoMascara>();

                //Operacion
                _usuario = CreateObjectSet<Usuario>();
                _bitacoraAcceso = CreateObjectSet<BitacoraAcceso>();
                _ubicacion = CreateObjectSet<Ubicacion>();
                _organizacion = CreateObjectSet<Organizacion>();
                _domicilio = CreateObjectSet<Domicilio>();
                _correoUsuario = CreateObjectSet<CorreoUsuario>();
                _telefonoUsuario = CreateObjectSet<TelefonoUsuario>();
                _usuarioNotificacion = CreateObjectSet<UsuarioNotificacion>();
                _usuarioRol = CreateObjectSet<UsuarioRol>();
                _usuarioGrupo = CreateObjectSet<UsuarioGrupo>();
                _usuarioLinkPassword = CreateObjectSet<UsuarioLinkPassword>();
                _usuarioPassword = CreateObjectSet<UsuarioPassword>();
                _tipoCampoMascara = CreateObjectSet<TipoCampoMascara>();
                _arbolAcceso = CreateObjectSet<ArbolAcceso>();
                _informacionConsulta = CreateObjectSet<InformacionConsulta>();
                _informacionConsultaDatos = CreateObjectSet<InformacionConsultaDatos>();
                _informacionConsultaDocumentos = CreateObjectSet<InformacionConsultaDocumentos>();
                _informacionConsultaRate = CreateObjectSet<InformacionConsultaRate>();
                _sla = CreateObjectSet<Sla>();
                _slaDetalle = CreateObjectSet<SlaDetalle>();
                _encuesta = CreateObjectSet<Encuesta>();
                _encuestaPregunta = CreateObjectSet<EncuestaPregunta>();
                _inventarioArbolAcceso = CreateObjectSet<InventarioArbolAcceso>();
                _inventarioInfConsulta = CreateObjectSet<InventarioInfConsulta>();
                _grupoUsuarioInventarioArbol = CreateObjectSet<GrupoUsuarioInventarioArbol>();
                _hitConsulta = CreateObjectSet<HitConsulta>();
                _hitGrupoUsuario = CreateObjectSet<HitGrupoUsuario>();
                _respuestaEncuesta = CreateObjectSet<RespuestaEncuesta>();
                _slaEstimadoTicket = CreateObjectSet<SlaEstimadoTicket>();
                _slaEstimadoTicketDetalle = CreateObjectSet<SlaEstimadoTicketDetalle>();
                _ticket = CreateObjectSet<Ticket>();
                _ticketCorreo = CreateObjectSet<TicketCorreo>();
                _ticketGrupoUsuario = CreateObjectSet<TicketGrupoUsuario>();
                _ticketAsignacion = CreateObjectSet<TicketAsignacion>();
                _ticketEstatus = CreateObjectSet<TicketEstatus>();
                _ticketNotificacion = CreateObjectSet<TicketNotificacion>();
                _area = CreateObjectSet<Area>();
                _estatusTicketSubRolGeneral = CreateObjectSet<EstatusTicketSubRolGeneral>();
                _estatusAsignacionSubRolGeneral = CreateObjectSet<EstatusAsignacionSubRolGeneral>();
                _estatusTicketSubRolGeneralDefault = CreateObjectSet<EstatusTicketSubRolGeneralDefault>();
                _estatusAsignacionSubRolGeneralDefault = CreateObjectSet<EstatusAsignacionSubRolGeneralDefault>();
                _horarioSubGrupo = CreateObjectSet<HorarioSubGrupo>();
                _diaFestivoSubGrupo = CreateObjectSet<DiaFestivoSubGrupo>();
                _tiempoInformeArbol = CreateObjectSet<TiempoInformeArbol>();
                _smsService = CreateObjectSet<SmsService>();
                _correoService = CreateObjectSet<CorreoService>();
                _preguntaReto = CreateObjectSet<PreguntaReto>();
                _preTicket = CreateObjectSet<PreTicket>();
                _horario = CreateObjectSet<Horario>();
                _horarioDetalle = CreateObjectSet<HorarioDetalle>();
                _notaGeneral = CreateObjectSet<NotaGeneral>();
                _notaGeneralGrupo = CreateObjectSet<NotaGeneralGrupo>();
                _notaOpcionUsuario = CreateObjectSet<NotaOpcionUsuario>();
                _notaOpcionGrupo = CreateObjectSet<NotaOpcionGrupo>();
                _ticketConversacion = CreateObjectSet<TicketConversacion>();
                _ticketEvento = CreateObjectSet<TicketEvento>();
                _ticketEventoAsignacion = CreateObjectSet<TicketEventoAsignacion>();
                _ticketEventoConversacion = CreateObjectSet<TicketEventoConversacion>();
                _ticketEventoEstatus = CreateObjectSet<TicketEventoEstatus>();
                _conversacionArchivo = CreateObjectSet<ConversacionArchivo>();
                _frecuencia = CreateObjectSet<Frecuencia>();
                _mascaraSeleccionCatalogo = CreateObjectSet<MascaraSeleccionCatalogo>();
                _puesto = CreateObjectSet<Puesto>();
                _grupoUsuarioDefaultOpcion = CreateObjectSet<GrupoUsuarioDefaultOpcion>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataBaseModelContext(string connectionString)
            : base(ConnectionString, ContainerName)
        {
            _connectionString = connectionString;
        }

        public DataBaseModelContext(EntityConnection connection)
            : base(ConnectionString, ContainerName)
        {
            _connection = connection;
        }

        public static string DecryptedConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings["GastosConnection"].ToString()))
                    throw new Exception("No se encuentra cadena de conexion");

                var toD = Convert.FromBase64String(ConfigurationManager.ConnectionStrings["GastosConnection"].ToString());

                var denc = ProtectedData.Unprotect(toD, null, DataProtectionScope.LocalMachine);
                string cadena = Encoding.ASCII.GetString(denc).Replace("&quot;", "\"");
                //System.Xml.Linq.XDocument x1 = System.Xml.Linq.XDocument.Parse(x);
                return cadena;
            }
        }

        public ObjectSet<ParametrosTelefonos> ParametrosTelefonos
        {
            get
            {
                return _parametrosTelefonos;
            }
        }
        public ObjectSet<AliasOrganizacion> AliasOrganizacion
        {
            get
            {
                return _aliasOrganizacion;
            }
        }

        public ObjectSet<AliasUbicacion> AliasUbicacion
        {
            get
            {
                return _aliasUbicacion;
            }
        }

        private readonly ObjectSet<ParametrosTelefonos> _parametrosTelefonos;
        private readonly ObjectSet<ParametroDatosAdicionales> _parametroDatosAdicionales;
        private readonly ObjectSet<AliasOrganizacion> _aliasOrganizacion;
        private readonly ObjectSet<AliasUbicacion> _aliasUbicacion;

        #region Operativo
        public ObjectSet<Usuario> Usuario
        {
            get
            {
                return _usuario;
            }
        }
        public ObjectSet<BitacoraAcceso> BitacoraAcceso
        {
            get
            {
                return _bitacoraAcceso;
            }
        }

        public ObjectSet<Ubicacion> Ubicacion
        {
            get
            {
                return _ubicacion;
            }
        }

        public ObjectSet<Organizacion> Organizacion
        {
            get
            {
                return _organizacion;
            }
        }

        public ObjectSet<Domicilio> Domicilio
        {
            get
            {
                return _domicilio;
            }
        }

        public ObjectSet<CorreoUsuario> CorreoUsuario
        {
            get
            {
                return _correoUsuario;
            }
        }

        public ObjectSet<TelefonoUsuario> TelefonoUsuario
        {
            get
            {
                return _telefonoUsuario;
            }
        }

        public ObjectSet<UsuarioNotificacion> UsuarioNotificacion
        {
            get
            {
                return _usuarioNotificacion;
            }
        }

        public ObjectSet<UsuarioRol> UsuarioRol
        {
            get
            {
                return _usuarioRol;
            }
        }

        public ObjectSet<UsuarioGrupo> UsuarioGrupo
        {
            get
            {
                return _usuarioGrupo;
            }
        }

        public ObjectSet<UsuarioLinkPassword> UsuarioLinkPassword
        {
            get
            {
                return _usuarioLinkPassword;
            }
        }

        public ObjectSet<UsuarioPassword> UsuarioPassword
        {
            get
            {
                return _usuarioPassword;
            }
        }

        public ObjectSet<TipoCampoMascara> TipoCampoMascara
        {
            get
            {
                return _tipoCampoMascara;
            }
        }

        public ObjectSet<Catalogos> Catalogos
        {
            get
            {
                return _catalogos;
            }
        }

        public ObjectSet<ArbolAcceso> ArbolAcceso
        {
            get
            {
                return _arbolAcceso;
            }
        }

        public ObjectSet<InformacionConsulta> InformacionConsulta
        {
            get
            {
                return _informacionConsulta;
            }
        }

        public ObjectSet<InformacionConsultaDatos> InformacionConsultaDatos
        {
            get
            {
                return _informacionConsultaDatos;
            }
        }

        public ObjectSet<InformacionConsultaDocumentos> InformacionConsultaDocumentos
        {
            get
            {
                return _informacionConsultaDocumentos;
            }
        }
        public ObjectSet<InformacionConsultaRate> InformacionConsultaRate
        {
            get
            {
                return _informacionConsultaRate;
            }
        }

        public ObjectSet<Sla> Sla
        {
            get
            {
                return _sla;
            }
        }

        public ObjectSet<SlaDetalle> SlaDetalle
        {
            get
            {
                return _slaDetalle;
            }
        }


        public ObjectSet<Encuesta> Encuesta
        {
            get
            {
                return _encuesta;
            }
        }

        public ObjectSet<EncuestaPregunta> EncuestaPregunta
        {
            get
            {
                return _encuestaPregunta;
            }
        }
        public ObjectSet<InventarioArbolAcceso> InventarioArbolAcceso
        {
            get
            {
                return _inventarioArbolAcceso;
            }
        }

        public ObjectSet<InventarioInfConsulta> InventarioInfConsulta
        {
            get
            {
                return _inventarioInfConsulta;
            }
        }

        public ObjectSet<GrupoUsuarioInventarioArbol> GrupoUsuarioInventarioArbol
        {
            get
            {
                return _grupoUsuarioInventarioArbol;
            }
        }

        public ObjectSet<HitConsulta> HitConsulta
        {
            get
            {
                return _hitConsulta;
            }
        }

        public ObjectSet<HitGrupoUsuario> HitGrupoUsuario
        {
            get
            {
                return _hitGrupoUsuario;
            }
        }

        public ObjectSet<RespuestaEncuesta> RespuestaEncuesta
        {
            get
            {
                return _respuestaEncuesta;
            }
        }

        public ObjectSet<SlaEstimadoTicket> SlaEstimadoTicket
        {
            get
            {
                return _slaEstimadoTicket;
            }
        }

        public ObjectSet<SlaEstimadoTicketDetalle> SlaEstimadoTicketDetalle
        {
            get
            {
                return _slaEstimadoTicketDetalle;
            }
        }

        public ObjectSet<Ticket> Ticket
        {
            get
            {
                return _ticket;
            }
        }

        public ObjectSet<TicketCorreo> TicketCorreo
        {
            get
            {
                return _ticketCorreo;
            }
        }

        public ObjectSet<TicketGrupoUsuario> TicketGrupoUsuario
        {
            get
            {
                return _ticketGrupoUsuario;
            }
        }

        public ObjectSet<Area> Area
        {
            get
            {
                return _area;
            }
        }

        public ObjectSet<EstatusTicketSubRolGeneral> EstatusTicketSubRolGeneral
        {
            get
            {
                return _estatusTicketSubRolGeneral;
            }
        }

        public ObjectSet<EstatusAsignacionSubRolGeneral> EstatusAsignacionSubRolGeneral
        {
            get
            {
                return _estatusAsignacionSubRolGeneral;
            }
        }

        public ObjectSet<EstatusTicketSubRolGeneralDefault> EstatusTicketSubRolGeneralDefault
        {
            get
            {
                return _estatusTicketSubRolGeneralDefault;
            }
        }

        public ObjectSet<EstatusAsignacionSubRolGeneralDefault> EstatusAsignacionSubRolGeneralDefault
        {
            get
            {
                return _estatusAsignacionSubRolGeneralDefault;
            }
        }

        public ObjectSet<TicketAsignacion> TicketAsignacion
        {
            get
            {
                return _ticketAsignacion;
            }
        }
        public ObjectSet<TicketEstatus> TicketEstatus
        {
            get
            {
                return _ticketEstatus;
            }
        }
        public ObjectSet<TicketNotificacion> TicketNotificacion
        {
            get
            {
                return _ticketNotificacion;
            }
        }

        public ObjectSet<HorarioSubGrupo> HorarioSubGrupo
        {
            get
            {
                return _horarioSubGrupo;
            }
        }
        public ObjectSet<DiaFestivoSubGrupo> DiaFestivoSubGrupo
        {
            get
            {
                return _diaFestivoSubGrupo;
            }
        }

        public ObjectSet<TiempoInformeArbol> TiempoInformeArbol
        {
            get
            {
                return _tiempoInformeArbol;
            }
        }

        public ObjectSet<CorreoService> CorreoService
        {
            get
            {
                return _correoService;
            }
        }

        public ObjectSet<SmsService> SmsService
        {
            get
            {
                return _smsService;
            }
        }

        public ObjectSet<PreguntaReto> PreguntaReto
        {
            get
            {
                return _preguntaReto;
            }
        }

        public ObjectSet<PreTicket> PreTicket
        {
            get
            {
                return _preTicket;
            }
        }

        public ObjectSet<NotaGeneral> NotaGeneral
        {
            get
            {
                return _notaGeneral;
            }
        }
        public ObjectSet<NotaGeneralGrupo> NotaGeneralGrupo
        {
            get
            {
                return _notaGeneralGrupo;
            }
        }
        public ObjectSet<NotaOpcionUsuario> NotaOpcionUsuario
        {
            get
            {
                return _notaOpcionUsuario;
            }
        }
        public ObjectSet<NotaOpcionGrupo> NotaOpcionGrupo
        {
            get
            {
                return _notaOpcionGrupo;
            }
        }
        public ObjectSet<TicketConversacion> TicketConversacion
        {
            get
            {
                return _ticketConversacion;
            }
        }
        public ObjectSet<TicketEvento> TicketEvento
        {
            get
            {
                return _ticketEvento;
            }
        }
        public ObjectSet<TicketEventoAsignacion> TicketEventoAsignacion
        {
            get
            {
                return _ticketEventoAsignacion;
            }
        }
        public ObjectSet<TicketEventoConversacion> TicketEventoConversacion
        {
            get
            {
                return _ticketEventoConversacion;
            }
        }
        public ObjectSet<TicketEventoEstatus> TicketEventoEstatus
        {
            get
            {
                return _ticketEventoEstatus;
            }
        }

        public ObjectSet<ConversacionArchivo> ConversacionArchivo
        {
            get
            {
                return _conversacionArchivo;
            }
        }
        public ObjectSet<Frecuencia> Frecuencia
        {
            get
            {
                return _frecuencia;
            }
        }
        public ObjectSet<MascaraSeleccionCatalogo> MascaraSeleccionCatalogo
        {
            get
            {
                return _mascaraSeleccionCatalogo;
            }
        }



        private readonly ObjectSet<Usuario> _usuario;
        private readonly ObjectSet<BitacoraAcceso> _bitacoraAcceso;
        private readonly ObjectSet<Ubicacion> _ubicacion;
        private readonly ObjectSet<Organizacion> _organizacion;
        private readonly ObjectSet<Domicilio> _domicilio;
        private readonly ObjectSet<CorreoUsuario> _correoUsuario;
        private readonly ObjectSet<TelefonoUsuario> _telefonoUsuario;
        private readonly ObjectSet<UsuarioNotificacion> _usuarioNotificacion;
        private readonly ObjectSet<UsuarioRol> _usuarioRol;
        private readonly ObjectSet<UsuarioGrupo> _usuarioGrupo;
        private readonly ObjectSet<UsuarioLinkPassword> _usuarioLinkPassword;
        private readonly ObjectSet<UsuarioPassword> _usuarioPassword;
        private readonly ObjectSet<TipoCampoMascara> _tipoCampoMascara;
        private readonly ObjectSet<Catalogos> _catalogos;
        private readonly ObjectSet<ArbolAcceso> _arbolAcceso;
        private readonly ObjectSet<InformacionConsulta> _informacionConsulta;
        private readonly ObjectSet<InformacionConsultaDatos> _informacionConsultaDatos;
        private readonly ObjectSet<InformacionConsultaDocumentos> _informacionConsultaDocumentos;
        private readonly ObjectSet<InformacionConsultaRate> _informacionConsultaRate;
        private readonly ObjectSet<Sla> _sla;
        private readonly ObjectSet<SlaDetalle> _slaDetalle;

        private readonly ObjectSet<Encuesta> _encuesta;
        private readonly ObjectSet<EncuestaPregunta> _encuestaPregunta;
        private readonly ObjectSet<InventarioArbolAcceso> _inventarioArbolAcceso;
        private readonly ObjectSet<InventarioInfConsulta> _inventarioInfConsulta;
        private readonly ObjectSet<GrupoUsuarioInventarioArbol> _grupoUsuarioInventarioArbol;
        private readonly ObjectSet<HitConsulta> _hitConsulta;
        private readonly ObjectSet<HitGrupoUsuario> _hitGrupoUsuario;
        private readonly ObjectSet<RespuestaEncuesta> _respuestaEncuesta;
        private readonly ObjectSet<SlaEstimadoTicket> _slaEstimadoTicket;
        private readonly ObjectSet<SlaEstimadoTicketDetalle> _slaEstimadoTicketDetalle;
        private readonly ObjectSet<Ticket> _ticket;
        private readonly ObjectSet<TicketCorreo> _ticketCorreo;
        private readonly ObjectSet<TicketGrupoUsuario> _ticketGrupoUsuario;
        private readonly ObjectSet<TicketAsignacion> _ticketAsignacion;
        private readonly ObjectSet<TicketEstatus> _ticketEstatus;
        private readonly ObjectSet<TicketNotificacion> _ticketNotificacion;
        private readonly ObjectSet<Area> _area;
        private readonly ObjectSet<EstatusTicketSubRolGeneral> _estatusTicketSubRolGeneral;
        private readonly ObjectSet<EstatusAsignacionSubRolGeneral> _estatusAsignacionSubRolGeneral;
        private readonly ObjectSet<EstatusTicketSubRolGeneralDefault> _estatusTicketSubRolGeneralDefault;
        private readonly ObjectSet<EstatusAsignacionSubRolGeneralDefault> _estatusAsignacionSubRolGeneralDefault;
        private readonly ObjectSet<HorarioSubGrupo> _horarioSubGrupo;
        private readonly ObjectSet<DiaFestivoSubGrupo> _diaFestivoSubGrupo;
        private readonly ObjectSet<TiempoInformeArbol> _tiempoInformeArbol;

        private readonly ObjectSet<CorreoService> _correoService;
        private readonly ObjectSet<SmsService> _smsService;
        private readonly ObjectSet<PreguntaReto> _preguntaReto;
        private readonly ObjectSet<PreTicket> _preTicket;
        private readonly ObjectSet<NotaGeneral> _notaGeneral;
        private readonly ObjectSet<NotaGeneralGrupo> _notaGeneralGrupo;
        private readonly ObjectSet<NotaOpcionUsuario> _notaOpcionUsuario;
        private readonly ObjectSet<NotaOpcionGrupo> _notaOpcionGrupo;
        private readonly ObjectSet<TicketConversacion> _ticketConversacion;
        private readonly ObjectSet<TicketEvento> _ticketEvento;
        private readonly ObjectSet<TicketEventoAsignacion> _ticketEventoAsignacion;
        private readonly ObjectSet<TicketEventoConversacion> _ticketEventoConversacion;
        private readonly ObjectSet<TicketEventoEstatus> _ticketEventoEstatus;
        private readonly ObjectSet<ConversacionArchivo> _conversacionArchivo;
        private readonly ObjectSet<Frecuencia> _frecuencia;
        private readonly ObjectSet<MascaraSeleccionCatalogo> _mascaraSeleccionCatalogo;

        #region Mascara
        public ObjectSet<Mascara> Mascara
        {
            get
            {
                return _mascara;
            }
        }
        public ObjectSet<TipoMascara> TipoMascara
        {
            get
            {
                return _tipoMascara;
            }
        }

        public ObjectSet<CampoMascara> CampoMascara
        {
            get
            {
                return _campoMascara;
            }
        }

        private readonly ObjectSet<Mascara> _mascara;
        private readonly ObjectSet<TipoMascara> _tipoMascara;
        private readonly ObjectSet<CampoMascara> _campoMascara;
        #endregion Mascara


        #endregion Operativo

        #region catalogos

        #region Systema

        public ObjectSet<TipoUsuario> TipoUsuario
        {
            get
            {
                return _tipoUsuario;
            }
        }

        public ObjectSet<Pais> Pais
        {
            get
            {
                return _pais;
            }
        }

        public ObjectSet<Colonia> Colonia
        {
            get
            {
                return _colonia;
            }
        }

        public ObjectSet<Municipio> Municipio
        {
            get
            {
                return _municipio;
            }
        }

        public ObjectSet<Estado> Estado
        {
            get
            {
                return _estado;
            }
        }

        public ObjectSet<TipoTelefono> TipoTelefono
        {
            get
            {
                return _tipoTelefono;
            }
        }

        public ObjectSet<Rol> Rol
        {
            get
            {
                return _rol;
            }
        }

        public ObjectSet<RolTipoUsuario> RolTipoUsuario
        {
            get
            {
                return _rolTipoUsuario;
            }
        }

        public ObjectSet<RolTipoGrupo> RolTipoGrupo
        {
            get
            {
                return _rolTipoGrupo;
            }
        }

        public ObjectSet<TipoGrupo> TipoGrupo
        {
            get
            {
                return _tipoGrupo;
            }
        }

        public ObjectSet<SubRol> SubRol
        {
            get
            {
                return _subRol;
            }
        }

        public ObjectSet<TipoArbolAcceso> TipoArbolAcceso
        {
            get
            {
                return _tipoArbolAcceso;
            }
        }

        public ObjectSet<TipoInfConsulta> TipoInfConsulta
        {
            get
            {
                return _tipoInfConsulta;
            }
        }

        public ObjectSet<TipoDocumento> TipoDocumento
        {
            get
            {
                return _tipoDocumento;
            }
        }
        public ObjectSet<TipoEncuesta> TipoEncuesta
        {
            get
            {
                return _tipoEncuesta;
            }
        }

        public ObjectSet<RespuestaTipoEncuesta> RespuestaTipoEncuesta
        {
            get
            {
                return _respuestaTipoEncuesta;
            }
        }

        public ObjectSet<Menu> Menu
        {
            get
            {
                return _menu;
            }
        }

        public ObjectSet<RolMenu> RolMenu
        {
            get
            {
                return _rolMenu;
            }
        }

        public ObjectSet<EstatusTicket> EstatusTicket
        {
            get
            {
                return _estatusTicket;
            }
        }

        public ObjectSet<EstatusAsignacion> EstatusAsignacion
        {
            get
            {
                return _estatusAsignacion;
            }
        }

        public ObjectSet<TipoNotificacion> TipoNotificacion
        {
            get
            {
                return _tipoNotificacion;
            }
        }

        public ObjectSet<Impacto> Impacto
        {
            get
            {
                return _impacto;
            }
        }
        public ObjectSet<Prioridad> Prioridad
        {
            get
            {
                return _prioridad;
            }
        }
        public ObjectSet<Urgencia> Urgencia
        {
            get
            {
                return _urgencia;
            }
        }

        public ObjectSet<NivelUbicacion> NivelUbicacion
        {
            get
            {
                return _nivelUbicacion;
            }
        }
        public ObjectSet<NivelOrganizacion> NivelOrganizacion
        {
            get
            {
                return _nivelOrganizacion;
            }
        }

        public ObjectSet<TipoCorreo> TipoCorreo
        {
            get
            {
                return _tipoCorreo;
            }
        }

        public ObjectSet<TipoLink> TipoLink
        {
            get
            {
                return _tipoLink;
            }
        }

        public ObjectSet<Canal> Canal
        {
            get
            {
                return _canal;
            }
        }

        public ObjectSet<Horario> Horario
        {
            get
            {
                return _horario;
            }
        }
        public ObjectSet<HorarioDetalle> HorarioDetalle
        {
            get
            {
                return _horarioDetalle;
            }
        }

        public ObjectSet<TipoNota> TipoNota
        {
            get
            {
                return _tipoNota;
            }
        }

        public ObjectSet<DiaFestivoDefault> DiaFestivoDefault
        {
            get
            {
                return _diaFestivoDefault;
            }
        }
        public ObjectSet<DiaFeriado> DiaFeriado
        {
            get
            {
                return _diaFeriado;
            }
        }

        public ObjectSet<DiasFeriados> DiasFeriados
        {
            get
            {
                return _diasFeriados;
            }
        }

        public ObjectSet<DiasFeriadosDetalle> DiasFeriadosDetalle
        {
            get
            {
                return _diasFeriadosDetalle;
            }
        }


        private readonly ObjectSet<TipoUsuario> _tipoUsuario;
        private readonly ObjectSet<Pais> _pais;
        private readonly ObjectSet<Colonia> _colonia;
        private readonly ObjectSet<Municipio> _municipio;
        private readonly ObjectSet<Estado> _estado;
        private readonly ObjectSet<TipoTelefono> _tipoTelefono;
        private readonly ObjectSet<Rol> _rol;
        private readonly ObjectSet<RolTipoUsuario> _rolTipoUsuario;
        private readonly ObjectSet<RolTipoGrupo> _rolTipoGrupo;
        private readonly ObjectSet<TipoGrupo> _tipoGrupo;
        private readonly ObjectSet<SubRol> _subRol;
        private readonly ObjectSet<TipoArbolAcceso> _tipoArbolAcceso;
        private readonly ObjectSet<TipoInfConsulta> _tipoInfConsulta;
        private readonly ObjectSet<TipoDocumento> _tipoDocumento;
        private readonly ObjectSet<TipoEncuesta> _tipoEncuesta;
        private readonly ObjectSet<RespuestaTipoEncuesta> _respuestaTipoEncuesta;
        private readonly ObjectSet<Menu> _menu;
        private readonly ObjectSet<RolMenu> _rolMenu;

        private readonly ObjectSet<EstatusTicket> _estatusTicket;
        private readonly ObjectSet<EstatusAsignacion> _estatusAsignacion;
        private readonly ObjectSet<TipoNotificacion> _tipoNotificacion;

        private readonly ObjectSet<Impacto> _impacto;
        private readonly ObjectSet<Prioridad> _prioridad;
        private readonly ObjectSet<Urgencia> _urgencia;

        private readonly ObjectSet<NivelUbicacion> _nivelUbicacion;
        private readonly ObjectSet<NivelOrganizacion> _nivelOrganizacion;
        private readonly ObjectSet<TipoCorreo> _tipoCorreo;
        private readonly ObjectSet<TipoLink> _tipoLink;
        private readonly ObjectSet<Canal> _canal;
        private readonly ObjectSet<Horario> _horario;
        private readonly ObjectSet<HorarioDetalle> _horarioDetalle;
        private readonly ObjectSet<TipoNota> _tipoNota;
        private readonly ObjectSet<DiaFestivoDefault> _diaFestivoDefault;
        private readonly ObjectSet<DiaFeriado> _diaFeriado;
        private readonly ObjectSet<DiasFeriados> _diasFeriados;
        private readonly ObjectSet<DiasFeriadosDetalle> _diasFeriadosDetalle;

        #endregion Systema

        public ObjectSet<SubRolEscalacionPermitida> SubRolEscalacionPermitida
        {
            get
            {
                return _subRolEscalacionPermitida;
            }
        }
        public ObjectSet<ParametrosSla> ParametrosSla
        {
            get
            {
                return _parametrosSla;
            }
        }
        public ObjectSet<ParametroCorreo> ParametroCorreo
        {
            get
            {
                return _parametroCorreo;
            }
        }
        public ObjectSet<ParametrosGenerales> ParametrosGenerales
        {
            get
            {
                return _parametrosGenerales;
            }
        }
        public ObjectSet<ParametroPassword> ParametroPassword
        {
            get
            {
                return _parametroPassword;
            }
        }
        public ObjectSet<ParametrosUsuario> ParametrosUsuario
        {
            get
            {
                return _parametrosUsuario;
            }
        }
        public ObjectSet<FrecuenciaFecha> FrecuenciaFecha
        {
            get
            {
                return _frecuenciaFechas;
            }
        }
        public ObjectSet<GraficosDefault> GraficosDefault
        {
            get
            {
                return _graficosDefault;
            }
        }
        public ObjectSet<GraficosFavoritos> GraficosFavoritos
        {
            get
            {
                return _graficosFavoritos;
            }
        }

        public ObjectSet<ParametroDatosAdicionales> ParametroDatosAdicionales
        {
            get
            {
                return _parametroDatosAdicionales;
            }
        }

        private readonly ObjectSet<SubRolEscalacionPermitida> _subRolEscalacionPermitida;
        private readonly ObjectSet<ParametrosSla> _parametrosSla;
        private readonly ObjectSet<ParametroCorreo> _parametroCorreo;
        private readonly ObjectSet<ParametrosGenerales> _parametrosGenerales;
        private readonly ObjectSet<ParametroPassword> _parametroPassword;
        private readonly ObjectSet<ParametrosUsuario> _parametrosUsuario;
        private readonly ObjectSet<FrecuenciaFecha> _frecuenciaFechas;
        private readonly ObjectSet<GraficosDefault> _graficosDefault;
        private readonly ObjectSet<GraficosFavoritos> _graficosFavoritos;

        #region Usuario

        public ObjectSet<GrupoUsuario> GrupoUsuario
        {
            get
            {
                return _grupoUsuario;
            }
        }

        public ObjectSet<SubGrupoUsuario> SubGrupoUsuario
        {
            get
            {
                return _subGrupoUsuario;
            }
        }

        public ObjectSet<Puesto> Puesto
        {
            get
            {
                return _puesto;
            }
        }

        public ObjectSet<GrupoUsuarioDefaultOpcion> GrupoUsuarioDefaultOpcion
        {
            get
            {
                return _grupoUsuarioDefaultOpcion;
            }
        }

        private readonly ObjectSet<GrupoUsuario> _grupoUsuario;
        private readonly ObjectSet<GrupoUsuarioDefaultOpcion> _grupoUsuarioDefaultOpcion;

        private readonly ObjectSet<SubGrupoUsuario> _subGrupoUsuario;
        private readonly ObjectSet<Puesto> _puesto;


        #region Ubicacion
        public ObjectSet<Campus> Campus
        {
            get
            {
                return _campus;
            }
        }

        public ObjectSet<Torre> Torre
        {
            get
            {
                return _torre;
            }
        }

        public ObjectSet<Piso> Piso
        {
            get
            {
                return _piso;
            }
        }

        public ObjectSet<Zona> Zona
        {
            get
            {
                return _zona;
            }
        }

        public ObjectSet<SubZona> SubZona
        {
            get
            {
                return _subZona;
            }
        }

        public ObjectSet<SiteRack> SiteRack
        {
            get
            {
                return _siteRack;
            }
        }

        private readonly ObjectSet<Campus> _campus;
        private readonly ObjectSet<Torre> _torre;
        private readonly ObjectSet<Piso> _piso;
        private readonly ObjectSet<Zona> _zona;
        private readonly ObjectSet<SubZona> _subZona;
        private readonly ObjectSet<SiteRack> _siteRack;
        #endregion Ubicacion

        #region Organizacion
        public ObjectSet<Holding> Holding
        {
            get
            {
                return _holding;
            }
        }

        public ObjectSet<Compania> Compañia
        {
            get
            {
                return _compania;
            }
        }

        public ObjectSet<Direccion> Direccion
        {
            get
            {
                return _direccion;
            }
        }

        public ObjectSet<SubDireccion> SubDireccion
        {
            get
            {
                return _subDireccion;
            }
        }

        public ObjectSet<Gerencia> Gerencia
        {
            get
            {
                return _gerencia;
            }
        }

        public ObjectSet<SubGerencia> SubGerencia
        {
            get
            {
                return _subGerencia;
            }
        }

        public ObjectSet<Jefatura> Jefatura
        {
            get
            {
                return _jefatura;
            }
        }

        private readonly ObjectSet<Holding> _holding;
        private readonly ObjectSet<Compania> _compania;
        private readonly ObjectSet<Direccion> _direccion;
        private readonly ObjectSet<SubDireccion> _subDireccion;
        private readonly ObjectSet<Gerencia> _gerencia;
        private readonly ObjectSet<SubGerencia> _subGerencia;
        private readonly ObjectSet<Jefatura> _jefatura;

        #endregion Organizacion


        #region ArbolesAcceso

        public ObjectSet<Nivel1> Nivel1
        {
            get
            {
                return _nivel1;
            }
        }

        public ObjectSet<Nivel2> Nivel2
        {
            get
            {
                return _nivel2;
            }
        }
        public ObjectSet<Nivel3> Nivel3
        {
            get
            {
                return _nivel3;
            }
        }
        public ObjectSet<Nivel4> Nivel4
        {
            get
            {
                return _nivel4;
            }
        }
        public ObjectSet<Nivel5> Nivel5
        {
            get
            {
                return _nivel5;
            }
        }
        public ObjectSet<Nivel6> Nivel6
        {
            get
            {
                return _nivel6;
            }
        }
        public ObjectSet<Nivel7> Nivel7
        {
            get
            {
                return _nivel7;
            }
        }
        private readonly ObjectSet<Nivel1> _nivel1;
        private readonly ObjectSet<Nivel2> _nivel2;
        private readonly ObjectSet<Nivel3> _nivel3;
        private readonly ObjectSet<Nivel4> _nivel4;
        private readonly ObjectSet<Nivel5> _nivel5;
        private readonly ObjectSet<Nivel6> _nivel6;
        private readonly ObjectSet<Nivel7> _nivel7;
        #endregion ArbolesAcceso

        #endregion Usuario

        #endregion catalogos

    }
}
