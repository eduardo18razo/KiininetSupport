﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSla {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSla.IServiceSla")]
    public interface IServiceSla {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceSla/ObtenerSla", ReplyAction="http://tempuri.org/IServiceSla/ObtenerSlaResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.Sla> ObtenerSla(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceSla/ObtenerSlaById", ReplyAction="http://tempuri.org/IServiceSla/ObtenerSlaByIdResponse")]
        KiiniNet.Entities.Cat.Usuario.Sla ObtenerSlaById(int idSla);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceSla/Guardar", ReplyAction="http://tempuri.org/IServiceSla/GuardarResponse")]
        void Guardar(KiiniNet.Entities.Cat.Usuario.Sla sla);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceSlaChannel : KiiniHelp.ServiceSla.IServiceSla, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceSlaClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSla.IServiceSla>, KiiniHelp.ServiceSla.IServiceSla {
        
        public ServiceSlaClient() {
        }
        
        public ServiceSlaClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceSlaClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSlaClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceSlaClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.Sla> ObtenerSla(bool insertarSeleccion) {
            return base.Channel.ObtenerSla(insertarSeleccion);
        }
        
        public KiiniNet.Entities.Cat.Usuario.Sla ObtenerSlaById(int idSla) {
            return base.Channel.ObtenerSlaById(idSla);
        }
        
        public void Guardar(KiiniNet.Entities.Cat.Usuario.Sla sla) {
            base.Channel.Guardar(sla);
        }
    }
}