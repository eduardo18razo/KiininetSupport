﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSistemaDomicilio {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSistemaDomicilio.IServiceDomicilioSistema")]
    public interface IServiceDomicilioSistema {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceDomicilioSistema/ObtenerColoniasCp", ReplyAction="http://tempuri.org/IServiceDomicilioSistema/ObtenerColoniasCpResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Colonia> ObtenerColoniasCp(int cp, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceDomicilioSistema/ObtenerDetalleColonia", ReplyAction="http://tempuri.org/IServiceDomicilioSistema/ObtenerDetalleColoniaResponse")]
        KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Colonia ObtenerDetalleColonia(int idColonia);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceDomicilioSistema/ObtenerEstados", ReplyAction="http://tempuri.org/IServiceDomicilioSistema/ObtenerEstadosResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Estado> ObtenerEstados(bool insertarSeleccion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceDomicilioSistemaChannel : KiiniHelp.ServiceSistemaDomicilio.IServiceDomicilioSistema, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceDomicilioSistemaClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSistemaDomicilio.IServiceDomicilioSistema>, KiiniHelp.ServiceSistemaDomicilio.IServiceDomicilioSistema {
        
        public ServiceDomicilioSistemaClient() {
        }
        
        public ServiceDomicilioSistemaClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceDomicilioSistemaClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceDomicilioSistemaClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceDomicilioSistemaClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Colonia> ObtenerColoniasCp(int cp, bool insertarSeleccion) {
            return base.Channel.ObtenerColoniasCp(cp, insertarSeleccion);
        }
        
        public KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Colonia ObtenerDetalleColonia(int idColonia) {
            return base.Channel.ObtenerDetalleColonia(idColonia);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Ubicaciones.Domicilio.Estado> ObtenerEstados(bool insertarSeleccion) {
            return base.Channel.ObtenerEstados(insertarSeleccion);
        }
    }
}
