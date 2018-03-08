﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniHelp.ServiceArbolAcceso {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceArbolAcceso.IServiceArbolAcceso")]
    public interface IServiceArbolAcceso {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerOpcionesPermitidas", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerOpcionesPermitidasResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerOpcionesPermitidas(int idUsuarioSolicita, int idUsuarioLevanta, System.Nullable<int> idArea, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/LevantaTicket", ReplyAction="http://tempuri.org/IServiceArbolAcceso/LevantaTicketResponse")]
        bool LevantaTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/RecadoTicketTicket", ReplyAction="http://tempuri.org/IServiceArbolAcceso/RecadoTicketTicketResponse")]
        bool RecadoTicketTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel1ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel1ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel1> ObtenerNivel1ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel2ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel2ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel2> ObtenerNivel2ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel1, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel3ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel3ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel3> ObtenerNivel3ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel2, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel4ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel4ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel4> ObtenerNivel4ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel3, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel5ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel5ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel5> ObtenerNivel5ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel4, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel6ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel6ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel6> ObtenerNivel6ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel5, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel7ByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel7ByGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel7> ObtenerNivel7ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel6, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/EsNodoTerminalByGrupos", ReplyAction="http://tempuri.org/IServiceArbolAcceso/EsNodoTerminalByGruposResponse")]
        bool EsNodoTerminalByGrupos(int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel1", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel1Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel1> ObtenerNivel1(int idArea, int idTipoArbol, int idTipoUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel2", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel2Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel2> ObtenerNivel2(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel1, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel3", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel3Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel3> ObtenerNivel3(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel2, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel4", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel4Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel4> ObtenerNivel4(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel3, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel5", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel5Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel5> ObtenerNivel5(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel4, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel6", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel6Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel6> ObtenerNivel6(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel5, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel7", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerNivel7Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel7> ObtenerNivel7(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel6, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/EsNodoTerminal", ReplyAction="http://tempuri.org/IServiceArbolAcceso/EsNodoTerminalResponse")]
        bool EsNodoTerminal(int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/GuardarArbol", ReplyAction="http://tempuri.org/IServiceArbolAcceso/GuardarArbolResponse")]
        void GuardarArbol(KiiniNet.Entities.Cat.Operacion.ArbolAcceso arbol);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoByGruposUsuario", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoByGruposUsuarioRespons" +
            "e")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Operacion.ArbolAcceso> ObtenerArbolesAccesoByGruposUsuario(int idUsuario, int idTipoArbol, int idArea);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolAcceso", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolAccesoResponse")]
        KiiniNet.Entities.Cat.Operacion.ArbolAcceso ObtenerArbolAcceso(int idArbol);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoAll", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoAllResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Operacion.ArbolAcceso> ObtenerArbolesAccesoAll(System.Nullable<int> idArea, System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/HabilitarArbol", ReplyAction="http://tempuri.org/IServiceArbolAcceso/HabilitarArbolResponse")]
        void HabilitarArbol(int idArbol, bool habilitado);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ActualizardArbol", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ActualizardArbolResponse")]
        void ActualizardArbol(int idArbolAcceso, KiiniNet.Entities.Cat.Operacion.ArbolAcceso arbolAcceso, string descripcion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalAll", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalAllResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Operacion.ArbolAcceso> ObtenerArbolesAccesoTerminalAll(System.Nullable<int> idArea, System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalAllTipificacio" +
            "n", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalAllTipificacio" +
            "nResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerArbolesAccesoTerminalAllTipificacion(System.Nullable<int> idArea, System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalByIdUsuario", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalByIdUsuarioRes" +
            "ponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerArbolesAccesoTerminalByIdUsuario(int idUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalByGrupoUsuario" +
            "", ReplyAction="http://tempuri.org/IServiceArbolAcceso/ObtenerArbolesAccesoTerminalByGrupoUsuario" +
            "Response")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerArbolesAccesoTerminalByGrupoUsuario(int idGrupo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceArbolAcceso/BusquedaGeneral", ReplyAction="http://tempuri.org/IServiceArbolAcceso/BusquedaGeneralResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperBusquedaArbolAcceso> BusquedaGeneral(System.Nullable<int> idUsuario, string filter, System.Collections.Generic.List<int> tipoUsuario, int page, int pagesize);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceArbolAccesoChannel : KiiniHelp.ServiceArbolAcceso.IServiceArbolAcceso, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceArbolAccesoClient : System.ServiceModel.ClientBase<KiiniHelp.ServiceArbolAcceso.IServiceArbolAcceso>, KiiniHelp.ServiceArbolAcceso.IServiceArbolAcceso {
        
        public ServiceArbolAccesoClient() {
        }
        
        public ServiceArbolAccesoClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceArbolAccesoClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceArbolAccesoClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceArbolAccesoClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerOpcionesPermitidas(int idUsuarioSolicita, int idUsuarioLevanta, System.Nullable<int> idArea, bool insertarSeleccion) {
            return base.Channel.ObtenerOpcionesPermitidas(idUsuarioSolicita, idUsuarioLevanta, idArea, insertarSeleccion);
        }
        
        public bool LevantaTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.LevantaTicket(idUsuarioLevanta, idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public bool RecadoTicketTicket(int idUsuarioLevanta, int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.RecadoTicketTicket(idUsuarioLevanta, idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel1> ObtenerNivel1ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel1ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel2> ObtenerNivel2ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel1, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel2ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel1, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel3> ObtenerNivel3ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel2, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel3ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel2, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel4> ObtenerNivel4ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel3, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel4ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel3, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel5> ObtenerNivel5ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel4, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel5ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel4, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel6> ObtenerNivel6ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel5, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel6ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel5, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel7> ObtenerNivel7ByGrupos(int idUsuarioSolicita, int idUsuarioLevanta, int idArea, int idTipoArbolAcceso, int idNivel6, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel7ByGrupos(idUsuarioSolicita, idUsuarioLevanta, idArea, idTipoArbolAcceso, idNivel6, insertarSeleccion);
        }
        
        public bool EsNodoTerminalByGrupos(int idArea, int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.EsNodoTerminalByGrupos(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel1> ObtenerNivel1(int idArea, int idTipoArbol, int idTipoUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel1(idArea, idTipoArbol, idTipoUsuario, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel2> ObtenerNivel2(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel1, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel2(idArea, idTipoArbol, idTipoUsuario, idNivel1, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel3> ObtenerNivel3(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel2, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel3(idArea, idTipoArbol, idTipoUsuario, idNivel2, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel4> ObtenerNivel4(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel3, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel4(idArea, idTipoArbol, idTipoUsuario, idNivel3, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel5> ObtenerNivel5(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel4, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel5(idArea, idTipoArbol, idTipoUsuario, idNivel4, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel6> ObtenerNivel6(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel5, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel6(idArea, idTipoArbol, idTipoUsuario, idNivel5, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Arbol.Nodos.Nivel7> ObtenerNivel7(int idArea, int idTipoArbol, int idTipoUsuario, int idNivel6, bool insertarSeleccion) {
            return base.Channel.ObtenerNivel7(idArea, idTipoArbol, idTipoUsuario, idNivel6, insertarSeleccion);
        }
        
        public bool EsNodoTerminal(int idTipoUsuario, int idTipoArbol, int nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.EsNodoTerminal(idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public void GuardarArbol(KiiniNet.Entities.Cat.Operacion.ArbolAcceso arbol) {
            base.Channel.GuardarArbol(arbol);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Operacion.ArbolAcceso> ObtenerArbolesAccesoByGruposUsuario(int idUsuario, int idTipoArbol, int idArea) {
            return base.Channel.ObtenerArbolesAccesoByGruposUsuario(idUsuario, idTipoArbol, idArea);
        }
        
        public KiiniNet.Entities.Cat.Operacion.ArbolAcceso ObtenerArbolAcceso(int idArbol) {
            return base.Channel.ObtenerArbolAcceso(idArbol);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Operacion.ArbolAcceso> ObtenerArbolesAccesoAll(System.Nullable<int> idArea, System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.ObtenerArbolesAccesoAll(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public void HabilitarArbol(int idArbol, bool habilitado) {
            base.Channel.HabilitarArbol(idArbol, habilitado);
        }
        
        public void ActualizardArbol(int idArbolAcceso, KiiniNet.Entities.Cat.Operacion.ArbolAcceso arbolAcceso, string descripcion) {
            base.Channel.ActualizardArbol(idArbolAcceso, arbolAcceso, descripcion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Operacion.ArbolAcceso> ObtenerArbolesAccesoTerminalAll(System.Nullable<int> idArea, System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.ObtenerArbolesAccesoTerminalAll(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerArbolesAccesoTerminalAllTipificacion(System.Nullable<int> idArea, System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.ObtenerArbolesAccesoTerminalAllTipificacion(idArea, idTipoUsuario, idTipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerArbolesAccesoTerminalByIdUsuario(int idUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerArbolesAccesoTerminalByIdUsuario(idUsuario, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperArbolAcceso> ObtenerArbolesAccesoTerminalByGrupoUsuario(int idGrupo) {
            return base.Channel.ObtenerArbolesAccesoTerminalByGrupoUsuario(idGrupo);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Helper.HelperBusquedaArbolAcceso> BusquedaGeneral(System.Nullable<int> idUsuario, string filter, System.Collections.Generic.List<int> tipoUsuario, int page, int pagesize) {
            return base.Channel.BusquedaGeneral(idUsuario, filter, tipoUsuario, page, pagesize);
        }
    }
}
