﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSistemaTipoEncuesta {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSistemaTipoEncuesta.IServiceTipoEncuesta")]
    public interface IServiceTipoEncuesta {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoEncuesta/ObtenerTiposEncuesta", ReplyAction="http://tempuri.org/IServiceTipoEncuesta/ObtenerTiposEncuestaResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoEncuesta> ObtenerTiposEncuesta(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoEncuesta/TipoEncuestaId", ReplyAction="http://tempuri.org/IServiceTipoEncuesta/TipoEncuestaIdResponse")]
        KiiniNet.Entities.Cat.Sistema.TipoEncuesta TipoEncuestaId(int idTipoEncuesta);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceTipoEncuestaChannel : KiiniHelp.ServiceSistemaTipoEncuesta.IServiceTipoEncuesta, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceTipoEncuestaClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSistemaTipoEncuesta.IServiceTipoEncuesta>, KiiniHelp.ServiceSistemaTipoEncuesta.IServiceTipoEncuesta {
        
        public ServiceTipoEncuestaClient() {
        }
        
        public ServiceTipoEncuestaClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceTipoEncuestaClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoEncuestaClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoEncuestaClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoEncuesta> ObtenerTiposEncuesta(bool insertarSeleccion) {
            return base.Channel.ObtenerTiposEncuesta(insertarSeleccion);
        }
        
        public KiiniNet.Entities.Cat.Sistema.TipoEncuesta TipoEncuestaId(int idTipoEncuesta) {
            return base.Channel.TipoEncuestaId(idTipoEncuesta);
        }
    }
}
