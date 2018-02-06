using System.Configuration;
using System.Web;

namespace KinniNet.Business.Utils
{
    public static class BusinessVariables
    {
        public static class AliasOrganizaciones
        {
            public static string Nivel1 = "Nivel 1";
            public static string Nivel2 = "Nivel 2";
            public static string Nivel3 = "Nivel 3";
            public static string Nivel4 = "Nivel 4";
            public static string Nivel5 = "Nivel 5";
            public static string Nivel6 = "Nivel 6";
            public static string Nivel7 = "Nivel 7";
        }

        public static class AliasUbicaciones
        {
            public static string Nivel1 = "Nivel 1";
            public static string Nivel2 = "Nivel 2";
            public static string Nivel3 = "Nivel 3";
            public static string Nivel4 = "Nivel 4";
            public static string Nivel5 = "Nivel 5";
            public static string Nivel6 = "Nivel 6";
            public static string Nivel7 = "Nivel 7";
        }

        public static class Directorios
        {
            public static string RepositorioTemporalInformacionConsulta = ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioInfomracionConsultas"] + ConfigurationManager.AppSettings["CarpetaTemporal"];
            public static string RepositorioInformacionConsulta = ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioInfomracionConsultas"];
            public static string RepositorioInformacionConsultaHtml = ConfigurationManager.AppSettings["PathInformacionConsultaHtml"];
            public static string RepositorioTemporalMascara = ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"] + ConfigurationManager.AppSettings["CarpetaTemporal"];
            public static string RepositorioMascara = ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["RepositorioMascara"];
            public static string RepositorioTemporal = ConfigurationManager.AppSettings["Repositorio"] + ConfigurationManager.AppSettings["CarpetaTemporal"];
            public static string RepositorioRepositorio = ConfigurationManager.AppSettings["Repositorio"];
            public static string RepositorioCorreo = ConfigurationManager.AppSettings["RepositorioCorreos"];
            public static string Carpetaemporal = HttpRuntime.AppDomainAppPath + ConfigurationManager.AppSettings["CarpetaTemporal"];
            public static string CarpetaTemporalSitio = ConfigurationManager.AppSettings["siteUrlRemporal"];
            public static string OnlyTempFolder = ConfigurationManager.AppSettings["CarpetaTemporal"];
        }

        public static class ComboBoxCatalogo
        {
            public static int IndexSeleccione = 0;
            public static int IndexTodos = 1;
            public static int ValueSeleccione = 0;
            public static int ValueTodos = -1;
            public static string DescripcionSeleccione = "-";
            public static string DescripcionTodos = "==TODOS==";
            public static bool Habilitado = false;
        }

        public static class ParametrosMascaraCaptura
        {
            public static string PrefijoTabla = "FORM_";
            public static string PrefijoComandoInsertar = "USPINSERT";
            public static string PrefijoComandoActualizar = "USPUPDATE";
            public static string NombreCampoRandom = "CLAVEREGISTRO";
            public static string TipoCampoRandom = "NVARCHAR(20)";
            public static string CampoRandom = NombreCampoRandom + " " + TipoCampoRandom;
            public static string CaracteresCampoRandom = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            public static int LongitudRandom = 4;
            public static string PrefijoFechaInicio = "Inicial";
            public static string PrefijoFechaFin = "Final";

        }

        public static class ParametrosCatalogo
        {
            public static string PrefijoTabla = "USER_";
            public static string PrefijoComandoInsertar = "InsertCatalogoSistema";
            public static string PrefijoComandoActualizar = "UpdateCatalogoSistema";
            public static string PrefijoComandoHabilitar = "HabilitarCatalogoSistema";

        }
        public static class EnumImpacto
        {
            public const string Alto = "Alto";
            public const string Medio = "Medio";
            public const string Bajo = "Bajo";
        }
        public static class EnumeradoresKiiniNet
        {
            public enum EnumeradorNivelAsignacion
            {
                Supervisor = 1,
                PrimerNivel = 2,
                SegundoNivel = 3,
                TercerNivel = 4,
                CuartoNivel = 5,
            }



            public enum EnumTiposCampo
            {
                Texto = 1,
                TextoMultiLinea = 2,
                RadioBoton = 3,
                ListaDepledable = 4,
                CasillaDeVerificación = 5,
                NúmeroEntero = 6,
                NúmeroDecimal = 7,
                Logico = 8,
                SelecciónCascada = 9,
                Fecha = 10,
                FechaRango = 11,
                ExpresiónRegular = 12,
                AdjuntarArchivo = 13,
            }
            public enum EnumTiempoDuracion
            {
                //Meses = 1,
                Dias = 2,
                Horas = 3,
                Minutos = 4,

            }
            public enum EnumEstatusTicket
            {
                Abierto = 1,
                Cancelado = 2,
                ReTipificado = 3,
                ReAbierto = 4,
                Resuelto = 5,
                Cerrado = 6,
                EnEspera = 7
            }

            public enum EnumEstatusAsignacion
            {
                PorAsignar = 1,
                Asignado = 2,
                ReAsignado = 3,
                Escalado = 4,
                Autoasignado = 5
            }

            public enum EnumCanal
            {
                Portal = 1,
                Correo = 2,
                Chat = 3,
                MessengerFacebook = 4,
                Twiter = 5,
                Telefono = 6,

            }

            public enum EnumTipoNota
            {
                General = 1,
                Opcion = 2
            }
        }

        public static class Correo
        {
            public static string HotmailAccount = ConfigurationManager.AppSettings["HotmailAccount"];
            public static string HotmailPassword = ConfigurationManager.AppSettings["HotmailPassword"];
            public static string GmailAccount = ConfigurationManager.AppSettings["GmailAccount"];
            public static string GmailPassword = ConfigurationManager.AppSettings["GmailPassword"];
            public static string YahooAccount = ConfigurationManager.AppSettings["YahooAccount"];
            public static string YahooPassword = ConfigurationManager.AppSettings["YahooPassword"];
            public static string OtherSmtp = ConfigurationManager.AppSettings["OtherSmtp"];
            public static int OtherPort = int.Parse(ConfigurationManager.AppSettings["OtherPort"]);
            public static string OtherAccount = ConfigurationManager.AppSettings["OtherAccount"];
            public static string OtherPassword = ConfigurationManager.AppSettings["OtherPassword"];

        }

        //public static int[] IdsPublicos =
        //{
        //    (int) EnumTiposUsuario.ClienteInvitado,
        //    (int) EnumTiposUsuario.EmpleadoInvitado,
        //    (int) EnumTiposUsuario.ProveedorInvitado
        //};

        public enum EnumTiposUsuario
        {
            Operador = 1,
            Cliente = 2,
            Proveedor = 3,
            Empleado = 4
            
            //EmpleadoPersonaFisica = 7,
            //ClientaPersonaFisica = 8,
            //ProveedorPersonaFisica = 9,
            //NuestraInstitucion = 10
        }
        public enum EnumTipoObjeto
        {
            Tabla = 1,
            Store = 2
        }
        public enum EnumTiposDocumento
        {
            Word = 1,
            PowerPoint = 2,
            Excel = 3,
            Pdf = 4,
            Imagen = 5
        }
        public enum EnumTiposInformacionConsulta
        {
            EditorDeContenido = 1,
            DocumentoOffice = 2,
            DireccionWeb = 3,
            Servicio = 4
        }

        public enum EnumTiposGrupos
        {
            Administrador = 1,
            Usuario = 2,
            ConsultasEspeciales = 3,
            Agente = 4,
            ResponsableDeContenido = 5,
            ResponsableDeOperación = 6,
            ResponsableDeDesarrollo = 7,
            ResponsableDeCategoría = 8,
            AgenteUniversal = 9
        }

        public enum EnumRoles
        {
            Administrador = 1,
            Usuario = 2,
            ConsultasEspeciales = 3,
            Agente = 4,
            ResponsableDeContenido = 5,
            ResponsableDeOperación = 6,
            ResponsableDeDesarrollo = 7,
            ResponsableDeCategoría = 8,
            AgenteUniversal = 9
        }

        public enum EnumSubRoles
        {
            Autorizador = 1,
            Solicitante = 2,
            Supervisor = 3,
            PrimererNivel = 4,
            SegundoNivel = 5,
            TercerNivel = 6,
            CuartoNivel = 7,
        }

        public enum EnumTipoNivel
        {
            SubMenu = 1,
            OpcionTerminal = 2
        }

        public enum EnumTipoArbol
        {
            ConsultarInformacion = 1,
            SolicitarServicio = 2,
            ReportarProblemas = 3,
            IncidentesMonitoreo = 4
        }

        public enum EnumMenu
        {
            Dashboard = 1,
            MisTickets = 2,
            Admin = 3,
            Atención = 4,
            HelpCenter = 5,
            Tickets = 6,
            Consultas = 7,
            Servicio = 8,
            Incidentes = 9,
            View = 10,
            Chat = 11,
            CambiarContraseña = 12,
            SolicitudMantenimiento = 13,
            Notificaciones = 14,


        }

        public enum EnumTipoEncuesta
        {
            SiNo = 1,
            Calificacion = 2,
            OpcionMultiple = 3,
            PromotorScore = 4
        }

        public enum EnumTipoCorreo
        {
            AltaUsuario = 1,
            RecuperarCuenta = 2,
            GenerarTicket = 3,
            ResponderTicket = 4
        }

        public enum EnumTipoLink
        {
            Confirmacion = 1,
            Reset = 2
        }

        public enum EnumTipoTelefono
        {
            Casa = 1,
            Celular = 2,
            Oficina = 3
        }

        public enum EnumtServerImap
        {
            Hotmail = 1,
            Gmail = 2,
            Yahoo = 3,
            Other = 4
        }

        public enum EnumMensajes
        {

            Exito,
            Error,
            FaltaTipoUsuario,
            FaltaDescripcion,
            Actualizacion,

        }
    }
}
