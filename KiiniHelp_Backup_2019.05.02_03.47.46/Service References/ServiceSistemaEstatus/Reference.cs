﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSistemaEstatus {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSistemaEstatus.IServiceEstatus")]
    public interface IServiceEstatus {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/ObtenerEstatusTicket", ReplyAction="http://tempuri.org/IServiceEstatus/ObtenerEstatusTicketResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusTicket> ObtenerEstatusTicket(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/ObtenerEstatusAsignacion", ReplyAction="http://tempuri.org/IServiceEstatus/ObtenerEstatusAsignacionResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusAsignacion> ObtenerEstatusAsignacion(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/ObtenerEstatusTicketUsuario", ReplyAction="http://tempuri.org/IServiceEstatus/ObtenerEstatusTicketUsuarioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusTicket> ObtenerEstatusTicketUsuario(int idUsuario, int idGrupo, int idEstatusActual, bool esPropietario, System.Nullable<int> idSubRol, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/ObtenerEstatusTicketUsuarioPublico", ReplyAction="http://tempuri.org/IServiceEstatus/ObtenerEstatusTicketUsuarioPublicoResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusTicket> ObtenerEstatusTicketUsuarioPublico(int idTicket, int idGrupo, int idEstatusActual, bool esPropietario, System.Nullable<int> idSubRol, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/ObtenerEstatusAsignacionUsuario", ReplyAction="http://tempuri.org/IServiceEstatus/ObtenerEstatusAsignacionUsuarioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int estatusAsignacionActual, bool esPropietario, int subRolActual, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/HasComentarioObligatorio", ReplyAction="http://tempuri.org/IServiceEstatus/HasComentarioObligatorioResponse")]
        bool HasComentarioObligatorio(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, int estatusAsignar, bool esPropietario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceEstatus/HasCambioEstatusComentarioObligatorio", ReplyAction="http://tempuri.org/IServiceEstatus/HasCambioEstatusComentarioObligatorioResponse")]
        bool HasCambioEstatusComentarioObligatorio(System.Nullable<int> idUsuario, int idTicket, int estatusAsignar, bool esPropietario, bool publico);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceEstatusChannel : KiiniHelp.ServiceSistemaEstatus.IServiceEstatus, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceEstatusClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSistemaEstatus.IServiceEstatus>, KiiniHelp.ServiceSistemaEstatus.IServiceEstatus {
        
        public ServiceEstatusClient() {
        }
        
        public ServiceEstatusClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceEstatusClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceEstatusClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceEstatusClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusTicket> ObtenerEstatusTicket(bool insertarSeleccion) {
            return base.Channel.ObtenerEstatusTicket(insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusAsignacion> ObtenerEstatusAsignacion(bool insertarSeleccion) {
            return base.Channel.ObtenerEstatusAsignacion(insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusTicket> ObtenerEstatusTicketUsuario(int idUsuario, int idGrupo, int idEstatusActual, bool esPropietario, System.Nullable<int> idSubRol, bool insertarSeleccion) {
            return base.Channel.ObtenerEstatusTicketUsuario(idUsuario, idGrupo, idEstatusActual, esPropietario, idSubRol, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusTicket> ObtenerEstatusTicketUsuarioPublico(int idTicket, int idGrupo, int idEstatusActual, bool esPropietario, System.Nullable<int> idSubRol, bool insertarSeleccion) {
            return base.Channel.ObtenerEstatusTicketUsuarioPublico(idTicket, idGrupo, idEstatusActual, esPropietario, idSubRol, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.EstatusAsignacion> ObtenerEstatusAsignacionUsuario(int idUsuario, int idGrupo, int estatusAsignacionActual, bool esPropietario, int subRolActual, bool insertarSeleccion) {
            return base.Channel.ObtenerEstatusAsignacionUsuario(idUsuario, idGrupo, estatusAsignacionActual, esPropietario, subRolActual, insertarSeleccion);
        }
        
        public bool HasComentarioObligatorio(int idUsuario, int idGrupo, int idSubRol, int estatusAsignacionActual, int estatusAsignar, bool esPropietario) {
            return base.Channel.HasComentarioObligatorio(idUsuario, idGrupo, idSubRol, estatusAsignacionActual, estatusAsignar, esPropietario);
        }
        
        public bool HasCambioEstatusComentarioObligatorio(System.Nullable<int> idUsuario, int idTicket, int estatusAsignar, bool esPropietario, bool publico) {
            return base.Channel.HasCambioEstatusComentarioObligatorio(idUsuario, idTicket, estatusAsignar, esPropietario, publico);
        }
    }
}
