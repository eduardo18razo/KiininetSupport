﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceDashboard {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceDashboard.IServiceDashboards")]
    public interface IServiceDashboards {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceDashboards/GetDashboardAdministrador", ReplyAction="http://tempuri.org/IServiceDashboards/GetDashboardAdministradorResponse")]
        KiiniNet.Entities.Operacion.Dashboard.DashboardAdministrador GetDashboardAdministrador();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceDashboards/GetDashboardAgente", ReplyAction="http://tempuri.org/IServiceDashboards/GetDashboardAgenteResponse")]
        KiiniNet.Entities.Operacion.Dashboard.DashboardAgente GetDashboardAgente(System.Nullable<int> idGrupo, System.Nullable<int> idUsuario);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceDashboardsChannel : KiiniHelp.ServiceDashboard.IServiceDashboards, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceDashboardsClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceDashboard.IServiceDashboards>, KiiniHelp.ServiceDashboard.IServiceDashboards {
        
        public ServiceDashboardsClient() {
        }
        
        public ServiceDashboardsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceDashboardsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceDashboardsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceDashboardsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public KiiniNet.Entities.Operacion.Dashboard.DashboardAdministrador GetDashboardAdministrador() {
            return base.Channel.GetDashboardAdministrador();
        }
        
        public KiiniNet.Entities.Operacion.Dashboard.DashboardAgente GetDashboardAgente(System.Nullable<int> idGrupo, System.Nullable<int> idUsuario) {
            return base.Channel.GetDashboardAgente(idGrupo, idUsuario);
        }
    }
}