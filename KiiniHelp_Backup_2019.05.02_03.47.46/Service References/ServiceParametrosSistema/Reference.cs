﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceParametrosSistema {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceParametrosSistema.IServiceParametros")]
    public interface IServiceParametros {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerTelefonosParametrosIdTipoUsuario", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerTelefonosParametrosIdTipoUsuarioResp" +
            "onse")]
        System.Collections.Generic.List<KiiniNet.Entities.Operacion.Usuarios.TelefonoUsuario> ObtenerTelefonosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerCorreosParametrosIdTipoUsuario", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerCorreosParametrosIdTipoUsuarioRespon" +
            "se")]
        System.Collections.Generic.List<KiiniNet.Entities.Operacion.Usuarios.CorreoUsuario> ObtenerCorreosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerParametrosGenerales", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerParametrosGeneralesResponse")]
        KiiniNet.Entities.Parametros.ParametrosGenerales ObtenerParametrosGenerales();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerParametrosUsuario", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerParametrosUsuarioResponse")]
        KiiniNet.Entities.Parametros.ParametrosUsuario ObtenerParametrosUsuario(int idTipoUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerAliasOrganizacion", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerAliasOrganizacionResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.AliasOrganizacion> ObtenerAliasOrganizacion(int idTipoUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerAliasUbicacion", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerAliasUbicacionResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.AliasUbicacion> ObtenerAliasUbicacion(int idTipoUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerDatosAdicionales", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerDatosAdicionalesResponse")]
        KiiniNet.Entities.Parametros.ParametroDatosAdicionales ObtenerDatosAdicionales(int idTipoUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerParemtrosPassword", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerParemtrosPasswordResponse")]
        KiiniNet.Entities.Parametros.ParametroPassword ObtenerParemtrosPassword();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerParametrosGraficoDefault", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerParametrosGraficoDefaultResponse")]
        KiiniNet.Entities.Parametros.GraficosDefault ObtenerParametrosGraficoDefault(int idReporte);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerFrecuenciasFecha", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerFrecuenciasFechaResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.FrecuenciaFecha> ObtenerFrecuenciasFecha();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerColoresTop", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerColoresTopResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.ColoresTop> ObtenerColoresTop();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerColoresSla", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerColoresSlaResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.ColoresSla> ObtenerColoresSla();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceParametros/ObtenerArchivosPermitidos", ReplyAction="http://tempuri.org/IServiceParametros/ObtenerArchivosPermitidosResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.ArchivosPermitidos> ObtenerArchivosPermitidos();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceParametrosChannel : KiiniHelp.ServiceParametrosSistema.IServiceParametros, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceParametrosClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceParametrosSistema.IServiceParametros>, KiiniHelp.ServiceParametrosSistema.IServiceParametros {
        
        public ServiceParametrosClient() {
        }
        
        public ServiceParametrosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceParametrosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceParametrosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceParametrosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Operacion.Usuarios.TelefonoUsuario> ObtenerTelefonosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerTelefonosParametrosIdTipoUsuario(idTipoUsuario, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Operacion.Usuarios.CorreoUsuario> ObtenerCorreosParametrosIdTipoUsuario(int idTipoUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerCorreosParametrosIdTipoUsuario(idTipoUsuario, insertarSeleccion);
        }
        
        public KiiniNet.Entities.Parametros.ParametrosGenerales ObtenerParametrosGenerales() {
            return base.Channel.ObtenerParametrosGenerales();
        }
        
        public KiiniNet.Entities.Parametros.ParametrosUsuario ObtenerParametrosUsuario(int idTipoUsuario) {
            return base.Channel.ObtenerParametrosUsuario(idTipoUsuario);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.AliasOrganizacion> ObtenerAliasOrganizacion(int idTipoUsuario) {
            return base.Channel.ObtenerAliasOrganizacion(idTipoUsuario);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.AliasUbicacion> ObtenerAliasUbicacion(int idTipoUsuario) {
            return base.Channel.ObtenerAliasUbicacion(idTipoUsuario);
        }
        
        public KiiniNet.Entities.Parametros.ParametroDatosAdicionales ObtenerDatosAdicionales(int idTipoUsuario) {
            return base.Channel.ObtenerDatosAdicionales(idTipoUsuario);
        }
        
        public KiiniNet.Entities.Parametros.ParametroPassword ObtenerParemtrosPassword() {
            return base.Channel.ObtenerParemtrosPassword();
        }
        
        public KiiniNet.Entities.Parametros.GraficosDefault ObtenerParametrosGraficoDefault(int idReporte) {
            return base.Channel.ObtenerParametrosGraficoDefault(idReporte);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.FrecuenciaFecha> ObtenerFrecuenciasFecha() {
            return base.Channel.ObtenerFrecuenciasFecha();
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.ColoresTop> ObtenerColoresTop() {
            return base.Channel.ObtenerColoresTop();
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.ColoresSla> ObtenerColoresSla() {
            return base.Channel.ObtenerColoresSla();
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.ArchivosPermitidos> ObtenerArchivosPermitidos() {
            return base.Channel.ObtenerArchivosPermitidos();
        }
    }
}
