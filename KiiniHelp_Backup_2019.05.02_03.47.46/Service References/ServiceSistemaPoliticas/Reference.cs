﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceSistemaPoliticas {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceSistemaPoliticas.IServicePoliticas")]
    public interface IServicePoliticas {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/ObtenerEstatusAsignacionSubRolGeneralDefault" +
            "", ReplyAction="http://tempuri.org/IServicePoliticas/ObtenerEstatusAsignacionSubRolGeneralDefault" +
            "Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusAsignacionSubRolGeneralDefault> ObtenerEstatusAsignacionSubRolGeneralDefault();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/ObtenerEstatusTicketSubRolGeneralDefault", ReplyAction="http://tempuri.org/IServicePoliticas/ObtenerEstatusTicketSubRolGeneralDefaultResp" +
            "onse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusTicketSubRolGeneralDefault> ObtenerEstatusTicketSubRolGeneralDefault();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/ObtenerEstatusAsignacionSubRolGeneral", ReplyAction="http://tempuri.org/IServicePoliticas/ObtenerEstatusAsignacionSubRolGeneralRespons" +
            "e")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusAsignacionSubRolGeneral> ObtenerEstatusAsignacionSubRolGeneral();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/ObtenerEstatusTicketSubRolGeneral", ReplyAction="http://tempuri.org/IServicePoliticas/ObtenerEstatusTicketSubRolGeneralResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusTicketSubRolGeneral> ObtenerEstatusTicketSubRolGeneral();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/HabilitarEstatusAsignacionSubRolGeneralDefau" +
            "lt", ReplyAction="http://tempuri.org/IServicePoliticas/HabilitarEstatusAsignacionSubRolGeneralDefau" +
            "ltResponse")]
        void HabilitarEstatusAsignacionSubRolGeneralDefault(int idAsignacion, bool habilitado);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/HabilitarEstatusTicketSubRolGeneralDefault", ReplyAction="http://tempuri.org/IServicePoliticas/HabilitarEstatusTicketSubRolGeneralDefaultRe" +
            "sponse")]
        void HabilitarEstatusTicketSubRolGeneralDefault(int idAsignacion, bool habilitado);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/HabilitarEstatusAsignacionSubRolGeneral", ReplyAction="http://tempuri.org/IServicePoliticas/HabilitarEstatusAsignacionSubRolGeneralRespo" +
            "nse")]
        void HabilitarEstatusAsignacionSubRolGeneral(int idAsignacion, bool habilitado);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServicePoliticas/HabilitarEstatusTicketSubRolGeneral", ReplyAction="http://tempuri.org/IServicePoliticas/HabilitarEstatusTicketSubRolGeneralResponse")]
        void HabilitarEstatusTicketSubRolGeneral(int idAsignacion, bool habilitado);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServicePoliticasChannel : KiiniHelp.ServiceSistemaPoliticas.IServicePoliticas, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicePoliticasClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceSistemaPoliticas.IServicePoliticas>, KiiniHelp.ServiceSistemaPoliticas.IServicePoliticas {
        
        public ServicePoliticasClient() {
        }
        
        public ServicePoliticasClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicePoliticasClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicePoliticasClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicePoliticasClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusAsignacionSubRolGeneralDefault> ObtenerEstatusAsignacionSubRolGeneralDefault() {
            return base.Channel.ObtenerEstatusAsignacionSubRolGeneralDefault();
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusTicketSubRolGeneralDefault> ObtenerEstatusTicketSubRolGeneralDefault() {
            return base.Channel.ObtenerEstatusTicketSubRolGeneralDefault();
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusAsignacionSubRolGeneral> ObtenerEstatusAsignacionSubRolGeneral() {
            return base.Channel.ObtenerEstatusAsignacionSubRolGeneral();
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Parametros.EstatusTicketSubRolGeneral> ObtenerEstatusTicketSubRolGeneral() {
            return base.Channel.ObtenerEstatusTicketSubRolGeneral();
        }
        
        public void HabilitarEstatusAsignacionSubRolGeneralDefault(int idAsignacion, bool habilitado) {
            base.Channel.HabilitarEstatusAsignacionSubRolGeneralDefault(idAsignacion, habilitado);
        }
        
        public void HabilitarEstatusTicketSubRolGeneralDefault(int idAsignacion, bool habilitado) {
            base.Channel.HabilitarEstatusTicketSubRolGeneralDefault(idAsignacion, habilitado);
        }
        
        public void HabilitarEstatusAsignacionSubRolGeneral(int idAsignacion, bool habilitado) {
            base.Channel.HabilitarEstatusAsignacionSubRolGeneral(idAsignacion, habilitado);
        }
        
        public void HabilitarEstatusTicketSubRolGeneral(int idAsignacion, bool habilitado) {
            base.Channel.HabilitarEstatusTicketSubRolGeneral(idAsignacion, habilitado);
        }
    }
}