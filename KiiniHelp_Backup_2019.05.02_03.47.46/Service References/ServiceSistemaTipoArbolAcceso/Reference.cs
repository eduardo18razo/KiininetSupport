﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSistemaTipoArbolAcceso {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSistemaTipoArbolAcceso.IServiceTipoArbolAcceso")]
    public interface IServiceTipoArbolAcceso {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoArbolAcceso/ObtenerTiposArbolAcceso", ReplyAction="http://tempuri.org/IServiceTipoArbolAcceso/ObtenerTiposArbolAccesoResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoArbolAcceso> ObtenerTiposArbolAcceso(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoArbolAcceso/ObtenerTiposArbolAccesoByGruposTercero" +
            "", ReplyAction="http://tempuri.org/IServiceTipoArbolAcceso/ObtenerTiposArbolAccesoByGruposTercero" +
            "Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoArbolAcceso> ObtenerTiposArbolAccesoByGruposTercero(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceTipoArbolAcceso/ObtenerTiposArbolAccesoByGrupos", ReplyAction="http://tempuri.org/IServiceTipoArbolAcceso/ObtenerTiposArbolAccesoByGruposRespons" +
            "e")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoArbolAcceso> ObtenerTiposArbolAccesoByGrupos(System.Collections.Generic.List<int> grupos, bool insertarSeleccion);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceTipoArbolAccesoChannel : KiiniHelp.ServiceSistemaTipoArbolAcceso.IServiceTipoArbolAcceso, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceTipoArbolAccesoClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSistemaTipoArbolAcceso.IServiceTipoArbolAcceso>, KiiniHelp.ServiceSistemaTipoArbolAcceso.IServiceTipoArbolAcceso {
        
        public ServiceTipoArbolAccesoClient() {
        }
        
        public ServiceTipoArbolAccesoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceTipoArbolAccesoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoArbolAccesoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceTipoArbolAccesoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoArbolAcceso> ObtenerTiposArbolAcceso(bool insertarSeleccion) {
            return base.Channel.ObtenerTiposArbolAcceso(insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoArbolAcceso> ObtenerTiposArbolAccesoByGruposTercero(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, bool insertarSeleccion) {
            return base.Channel.ObtenerTiposArbolAccesoByGruposTercero(idUsuarioSolicita, idUsuarioLevanta, idArea, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Sistema.TipoArbolAcceso> ObtenerTiposArbolAccesoByGrupos(System.Collections.Generic.List<int> grupos, bool insertarSeleccion) {
            return base.Channel.ObtenerTiposArbolAccesoByGrupos(grupos, insertarSeleccion);
        }
    }
}
