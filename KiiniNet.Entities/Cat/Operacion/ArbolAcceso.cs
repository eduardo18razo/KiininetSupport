using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Cat.Arbol.Nodos;
using KiiniNet.Entities.Cat.Sistema;
using KiiniNet.Entities.Operacion;
using KiiniNet.Entities.Operacion.Tickets;

namespace KiiniNet.Entities.Cat.Operacion
{
    [DataContract(IsReference = true)]
    public class ArbolAcceso
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IdArea { get; set; }
        [DataMember]
        public int IdTipoUsuario { get; set; }
        [DataMember]
        public int IdTipoArbolAcceso { get; set; }
        [DataMember]
        public int? IdImpacto { get; set; }

        [DataMember]
        public int? IdNivel1 { get; set; }
        [DataMember]
        public int? IdNivel2 { get; set; }
        [DataMember]
        public int? IdNivel3 { get; set; }
        [DataMember]
        public int? IdNivel4 { get; set; }
        [DataMember]
        public int? IdNivel5 { get; set; }
        [DataMember]
        public int? IdNivel6 { get; set; }
        [DataMember]
        public int? IdNivel7 { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public bool Evaluacion { get; set; }
        [DataMember]
        public bool EsTerminal { get; set; }
        [DataMember]
        public bool Publico { get; set; }
        [DataMember]
        public bool Sistema { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public DateTime FechaAlta { get; set; }
        [DataMember]
        public int IdUsuarioAlta { get; set; }
        [DataMember]
        public int MeGusta { get; set; }
        [DataMember]
        public int NoMeGusta { get; set; }
        [DataMember]
        public int Visitas { get; set; }
        [DataMember]
        public DateTime FechaVisita { get; set; }

        [DataMember]
        public virtual TipoUsuario TipoUsuario { get; set; }
        [DataMember]
        public virtual Impacto Impacto { get; set; }

        [DataMember]
        public virtual Nivel1 Nivel1 { get; set; }
        [DataMember]
        public virtual Nivel2 Nivel2 { get; set; }
        [DataMember]
        public virtual Nivel3 Nivel3 { get; set; }
        [DataMember]
        public virtual Nivel4 Nivel4 { get; set; }
        [DataMember]
        public virtual Nivel5 Nivel5 { get; set; }
        [DataMember]
        public virtual Nivel6 Nivel6 { get; set; }
        [DataMember]
        public virtual Nivel7 Nivel7 { get; set; }

        [DataMember]
        public virtual TipoArbolAcceso TipoArbolAcceso { get; set; }
        [DataMember]
        public virtual List<InventarioArbolAcceso> InventarioArbolAcceso { get; set; }
        [DataMember]
        public virtual List<HitConsulta> HitConsulta { get; set; }
        [DataMember]
        public virtual List<Ticket> Ticket { get; set; }
        [DataMember]
        public virtual Area Area { get; set; }

        [DataMember]
        public virtual List<TiempoInformeArbol> TiempoInformeArbol { get; set; }
        [DataMember]
        public virtual List<RespuestaEncuesta> RespuestaEncuesta { get; set; }
        [DataMember]
        public virtual List<PreTicket> PreTicket { get; set; }
        [DataMember]
        public virtual List<NotaOpcionUsuario> NotaOpcionUsuario { get; set; }
        [DataMember]
        public virtual List<NotaOpcionGrupo> NotaOpcionGrupo { get; set; }
        [DataMember]
        public virtual List<Frecuencia> Frecuencia { get; set; }
        [DataMember]
        public string Tipificacion { get; set; }
        [DataMember]
        public int Nivel { get; set; }


        [DataMember]
        public int Promotores { get; set; }
        [DataMember]
        public int Neutros { get; set; }
        [DataMember]
        public int Detractores { get; set; }


        [DataMember]
        public int NumeroEncuestas { get; set; }
        [DataMember]
        public int NumeroPreguntasEncuesta { get; set; }
        [DataMember]
        public int CeroCinco { get; set; }
        [DataMember]
        public int SeisSiete { get; set; }
        [DataMember]
        public int OchoNueve { get; set; }
        [DataMember]
        public int Diez { get; set; }
        [DataMember]
        public double PromedioPonderado { get; set; }


        [DataMember]
        public int Pesimo { get; set; }
        [DataMember]
        public int Malo { get; set; }
        [DataMember]
        public int Regular { get; set; }
        [DataMember]
        public int Bueno { get; set; }
        [DataMember]
        public int Excelente { get; set; }

        [DataMember]
        public int Si { get; set; }
        [DataMember]
        public int No { get; set; }


    }
}
