﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KiiniMaster.ServiceGrupoUsuario {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceGrupoUsuario.IServiceGrupoUsuario")]
    public interface IServiceGrupoUsuario {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioTipoUsuario", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioTipoUsuarioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioTipoUsuario(int idTipoGrupo, int idTipoUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioByIdTipoSubGrupo", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioByIdTipoSubGrupoRespo" +
            "nse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioByIdTipoSubGrupo(int idTipoSubgrupo, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioByIdRolTipoUsuario", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioByIdRolTipoUsuarioRes" +
            "ponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioByIdRolTipoUsuario(int idRol, int idTipoUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioByIdRol", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioByIdRolResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioByIdRol(int idRol, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/GuardarGrupoUsuario", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/GuardarGrupoUsuarioResponse")]
        void GuardarGrupoUsuario(KiiniNet.Entities.Cat.Usuario.GrupoUsuario grupoUsuario, System.Collections.Generic.Dictionary<int, int> horarios, System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.DiaFestivoSubGrupo>> diasDescanso);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGrupoUsuarioById", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGrupoUsuarioByIdResponse")]
        KiiniNet.Entities.Cat.Usuario.GrupoUsuario ObtenerGrupoUsuarioById(int idGrupoUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioSistema", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioSistemaResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioSistema(int idTipoUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioNivel", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioNivelResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioNivel(int idtipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposDeUsuario", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposDeUsuarioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Operacion.Usuarios.UsuarioGrupo> ObtenerGruposDeUsuario(int idUsuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/HabilitarGrupo", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/HabilitarGrupoResponse")]
        void HabilitarGrupo(int idGrupo, bool habilitado);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioAll", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioAllResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioAll(System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoGrupo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ActualizarGrupo", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ActualizarGrupoResponse")]
        void ActualizarGrupo(KiiniNet.Entities.Cat.Usuario.GrupoUsuario gpo, System.Collections.Generic.Dictionary<int, int> horarios, System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.DiaFestivoSubGrupo>> diasDescanso);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerHorariosByIdSubGrupo", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerHorariosByIdSubGrupoResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.HorarioSubGrupo> ObtenerHorariosByIdSubGrupo(int idSubGrupo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerDiasByIdSubGrupo", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerDiasByIdSubGrupoResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.DiaFestivoSubGrupo> ObtenerDiasByIdSubGrupo(int idSubGrupo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGrupos", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGrupos(bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposByIdUsuario", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposByIdUsuarioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposByIdUsuario(int idUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioResponsablesByGruposT" +
            "ipoServicio", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposUsuarioResponsablesByGruposT" +
            "ipoServicioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioResponsablesByGruposTipoServicio(int idUsuario, System.Collections.Generic.List<int> grupos, System.Collections.Generic.List<int> tipoServicio);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposAtencionByIdUsuario", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGruposAtencionByIdUsuarioResponse")]
        System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposAtencionByIdUsuario(int idUsuario, bool insertarSeleccion);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServiceGrupoUsuario/ObtenerGrupoDefaultRol", ReplyAction="http://tempuri.org/IServiceGrupoUsuario/ObtenerGrupoDefaultRolResponse")]
        KiiniNet.Entities.Cat.Usuario.GrupoUsuario ObtenerGrupoDefaultRol(int idRol, int idTipoUsuario);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceGrupoUsuarioChannel : KiiniMaster.ServiceGrupoUsuario.IServiceGrupoUsuario, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceGrupoUsuarioClient : System.ServiceModel.ClientBase<KiiniMaster.ServiceGrupoUsuario.IServiceGrupoUsuario>, KiiniMaster.ServiceGrupoUsuario.IServiceGrupoUsuario {
        
        public ServiceGrupoUsuarioClient() {
        }
        
        public ServiceGrupoUsuarioClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceGrupoUsuarioClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceGrupoUsuarioClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceGrupoUsuarioClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioTipoUsuario(int idTipoGrupo, int idTipoUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerGruposUsuarioTipoUsuario(idTipoGrupo, idTipoUsuario, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioByIdTipoSubGrupo(int idTipoSubgrupo, bool insertarSeleccion) {
            return base.Channel.ObtenerGruposUsuarioByIdTipoSubGrupo(idTipoSubgrupo, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioByIdRolTipoUsuario(int idRol, int idTipoUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerGruposUsuarioByIdRolTipoUsuario(idRol, idTipoUsuario, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioByIdRol(int idRol, bool insertarSeleccion) {
            return base.Channel.ObtenerGruposUsuarioByIdRol(idRol, insertarSeleccion);
        }
        
        public void GuardarGrupoUsuario(KiiniNet.Entities.Cat.Usuario.GrupoUsuario grupoUsuario, System.Collections.Generic.Dictionary<int, int> horarios, System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.DiaFestivoSubGrupo>> diasDescanso) {
            base.Channel.GuardarGrupoUsuario(grupoUsuario, horarios, diasDescanso);
        }
        
        public KiiniNet.Entities.Cat.Usuario.GrupoUsuario ObtenerGrupoUsuarioById(int idGrupoUsuario) {
            return base.Channel.ObtenerGrupoUsuarioById(idGrupoUsuario);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioSistema(int idTipoUsuario) {
            return base.Channel.ObtenerGruposUsuarioSistema(idTipoUsuario);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioNivel(int idtipoArbol, System.Nullable<int> nivel1, System.Nullable<int> nivel2, System.Nullable<int> nivel3, System.Nullable<int> nivel4, System.Nullable<int> nivel5, System.Nullable<int> nivel6, System.Nullable<int> nivel7) {
            return base.Channel.ObtenerGruposUsuarioNivel(idtipoArbol, nivel1, nivel2, nivel3, nivel4, nivel5, nivel6, nivel7);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Operacion.Usuarios.UsuarioGrupo> ObtenerGruposDeUsuario(int idUsuario) {
            return base.Channel.ObtenerGruposDeUsuario(idUsuario);
        }
        
        public void HabilitarGrupo(int idGrupo, bool habilitado) {
            base.Channel.HabilitarGrupo(idGrupo, habilitado);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioAll(System.Nullable<int> idTipoUsuario, System.Nullable<int> idTipoGrupo) {
            return base.Channel.ObtenerGruposUsuarioAll(idTipoUsuario, idTipoGrupo);
        }
        
        public void ActualizarGrupo(KiiniNet.Entities.Cat.Usuario.GrupoUsuario gpo, System.Collections.Generic.Dictionary<int, int> horarios, System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.DiaFestivoSubGrupo>> diasDescanso) {
            base.Channel.ActualizarGrupo(gpo, horarios, diasDescanso);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.HorarioSubGrupo> ObtenerHorariosByIdSubGrupo(int idSubGrupo) {
            return base.Channel.ObtenerHorariosByIdSubGrupo(idSubGrupo);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.DiaFestivoSubGrupo> ObtenerDiasByIdSubGrupo(int idSubGrupo) {
            return base.Channel.ObtenerDiasByIdSubGrupo(idSubGrupo);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGrupos(bool insertarSeleccion) {
            return base.Channel.ObtenerGrupos(insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposByIdUsuario(int idUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerGruposByIdUsuario(idUsuario, insertarSeleccion);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposUsuarioResponsablesByGruposTipoServicio(int idUsuario, System.Collections.Generic.List<int> grupos, System.Collections.Generic.List<int> tipoServicio) {
            return base.Channel.ObtenerGruposUsuarioResponsablesByGruposTipoServicio(idUsuario, grupos, tipoServicio);
        }
        
        public System.Collections.Generic.List<KiiniNet.Entities.Cat.Usuario.GrupoUsuario> ObtenerGruposAtencionByIdUsuario(int idUsuario, bool insertarSeleccion) {
            return base.Channel.ObtenerGruposAtencionByIdUsuario(idUsuario, insertarSeleccion);
        }
        
        public KiiniNet.Entities.Cat.Usuario.GrupoUsuario ObtenerGrupoDefaultRol(int idRol, int idTipoUsuario) {
            return base.Channel.ObtenerGrupoDefaultRol(idRol, idTipoUsuario);
        }
    }
}
