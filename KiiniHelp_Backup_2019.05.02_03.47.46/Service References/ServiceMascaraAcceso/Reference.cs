﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceMascaraAcceso {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceMascaraAcceso.IServiceMascaras")]
    public interface IServiceMascaras {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/CrearMascara", ReplyAction="http://tempuri.org/IServiceMascaras/CrearMascaraResponse")]
        void CrearMascara(KiiniNet.Entities.Cat.Mascaras.Mascara mascara);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ActualizarMascara", ReplyAction="http://tempuri.org/IServiceMascaras/ActualizarMascaraResponse")]
        void ActualizarMascara(KiiniNet.Entities.Cat.Mascaras.Mascara mascara);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ObtenerMascaraCaptura", ReplyAction="http://tempuri.org/IServiceMascaras/ObtenerMascaraCapturaResponse")]
        KiiniNet.Entities.Cat.Mascaras.Mascara ObtenerMascaraCaptura(int idMascara);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ObtenerMascaraCapturaByIdTicket", ReplyAction="http://tempuri.org/IServiceMascaras/ObtenerMascaraCapturaByIdTicketResponse")]
        KiiniNet.Entities.Cat.Mascaras.Mascara ObtenerMascaraCapturaByIdTicket(int idTicket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ObtenerMascarasAcceso", ReplyAction="http://tempuri.org/IServiceMascaras/ObtenerMascarasAccesoResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Mascaras.Mascara> ObtenerMascarasAcceso(int idTipoMascara, bool sistema, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ObtenerCatalogoCampoMascara", ReplyAction="http://tempuri.org/IServiceMascaras/ObtenerCatalogoCampoMascaraResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.CatalogoGenerico> ObtenerCatalogoCampoMascara(int idCatalogo, bool insertarSeleccion, bool filtraHabilitados);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/Consulta", ReplyAction="http://tempuri.org/IServiceMascaras/ConsultaResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Mascaras.Mascara> Consulta(string descripcion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/HabilitarMascara", ReplyAction="http://tempuri.org/IServiceMascaras/HabilitarMascaraResponse")]
        void HabilitarMascara(int idMascara, bool habilitado);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ObtenerDatosMascara", ReplyAction="http://tempuri.org/IServiceMascaras/ObtenerDatosMascaraResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperMascaraData> ObtenerDatosMascara(int idMascara, int idTicket);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceMascaras/ObtenerReporteMascara", ReplyAction="http://tempuri.org/IServiceMascaras/ObtenerReporteMascaraResponse")]
        System.Data.DataTable ObtenerReporteMascara(int idMascara, System.Collections.Generic.Dictionary<string, System.DateTime> fechas);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceMascarasChannel : KiiniHelp.ServiceMascaraAcceso.IServiceMascaras, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceMascarasClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceMascaraAcceso.IServiceMascaras>, KiiniHelp.ServiceMascaraAcceso.IServiceMascaras {
        
        public ServiceMascarasClient() {
        }
        
        public ServiceMascarasClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceMascarasClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceMascarasClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceMascarasClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CrearMascara(KiiniNet.Entities.Cat.Mascaras.Mascara mascara) {
            base.Channel.CrearMascara(mascara);
        }
        
        public void ActualizarMascara(KiiniNet.Entities.Cat.Mascaras.Mascara mascara) {
            base.Channel.ActualizarMascara(mascara);
        }
        
        public KiiniNet.Entities.Cat.Mascaras.Mascara ObtenerMascaraCaptura(int idMascara) {
            return base.Channel.ObtenerMascaraCaptura(idMascara);
        }
        
        public KiiniNet.Entities.Cat.Mascaras.Mascara ObtenerMascaraCapturaByIdTicket(int idTicket) {
            return base.Channel.ObtenerMascaraCapturaByIdTicket(idTicket);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Mascaras.Mascara> ObtenerMascarasAcceso(int idTipoMascara, bool sistema, bool insertarSeleccion) {
            return base.Channel.ObtenerMascarasAcceso(idTipoMascara, sistema, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.CatalogoGenerico> ObtenerCatalogoCampoMascara(int idCatalogo, bool insertarSeleccion, bool filtraHabilitados) {
            return base.Channel.ObtenerCatalogoCampoMascara(idCatalogo, insertarSeleccion, filtraHabilitados);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Mascaras.Mascara> Consulta(string descripcion) {
            return base.Channel.Consulta(descripcion);
        }
        
        public void HabilitarMascara(int idMascara, bool habilitado) {
            base.Channel.HabilitarMascara(idMascara, habilitado);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperMascaraData> ObtenerDatosMascara(int idMascara, int idTicket) {
            return base.Channel.ObtenerDatosMascara(idMascara, idTicket);
        }
        
        public System.Data.DataTable ObtenerReporteMascara(int idMascara, System.Collections.Generic.Dictionary<string, System.DateTime> fechas) {
            return base.Channel.ObtenerReporteMascara(idMascara, fechas);
        }
    }
}
