﻿using System;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Operacion.Usuarios
{
    [DataContract(IsReference = true)]
    public class UsuarioPassword
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdUsuario { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public virtual Usuario Usuario { get; set; }
    }
}
