﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSitemaTipoNota {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSitemaTipoNota.IServiceTipoNota")]
    public interface IServiceTipoNota {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoNota/ObtenerTipoNotas", ReplyAction="http://tempuri.org/IServiceTipoNota/ObtenerTipoNotasResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoNota> ObtenerTipoNotas(bool insertarSeleccion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceTipoNotaChannel : KiiniHelp.ServiceSitemaTipoNota.IServiceTipoNota, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceTipoNotaClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSitemaTipoNota.IServiceTipoNota>, KiiniHelp.ServiceSitemaTipoNota.IServiceTipoNota {
        
        public ServiceTipoNotaClient() {
        }
        
        public ServiceTipoNotaClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceTipoNotaClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoNotaClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoNotaClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoNota> ObtenerTipoNotas(bool insertarSeleccion) {
            return base.Channel.ObtenerTipoNotas(insertarSeleccion);
        }
    }
}
