﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSistemaTipoDocumento {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSistemaTipoDocumento.IServiceTipoDocumento")]
    public interface IServiceTipoDocumento {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoDocumento/ObtenerTipoDocumentos", ReplyAction="http://tempuri.org/IServiceTipoDocumento/ObtenerTipoDocumentosResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoDocumento> ObtenerTipoDocumentos(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoDocumento/ObtenerTiposDocumentoId", ReplyAction="http://tempuri.org/IServiceTipoDocumento/ObtenerTiposDocumentoIdResponse")]
        KiiniNet.Entities.Cat.Sistema.TipoDocumento ObtenerTiposDocumentoId(int idTipoDocumento);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceTipoDocumentoChannel : KiiniHelp.ServiceSistemaTipoDocumento.IServiceTipoDocumento, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceTipoDocumentoClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSistemaTipoDocumento.IServiceTipoDocumento>, KiiniHelp.ServiceSistemaTipoDocumento.IServiceTipoDocumento {
        
        public ServiceTipoDocumentoClient() {
        }
        
        public ServiceTipoDocumentoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceTipoDocumentoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoDocumentoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoDocumentoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoDocumento> ObtenerTipoDocumentos(bool insertarSeleccion) {
            return base.Channel.ObtenerTipoDocumentos(insertarSeleccion);
        }
        
        public KiiniNet.Entities.Cat.Sistema.TipoDocumento ObtenerTiposDocumentoId(int idTipoDocumento) {
            return base.Channel.ObtenerTiposDocumentoId(idTipoDocumento);
        }
    }
}
