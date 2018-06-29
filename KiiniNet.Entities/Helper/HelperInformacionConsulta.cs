using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using KiiniNet.Entities.Operacion.Usuarios;

namespace KiiniNet.Entities.Helper
{
    public class HelperInformacionConsulta
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string Autor { get; set; }
        [DataMember]
        public DateTime Creacion { get; set; }
        [DataMember]
        public DateTime? UltEdicion { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public int MeGusta { get; set; }
        [DataMember]
        public int NoMeGusta { get; set; }
    }
}
