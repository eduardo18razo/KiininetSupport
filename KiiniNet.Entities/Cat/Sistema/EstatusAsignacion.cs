using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KiiniNet.Entities.Operacion.Tickets;
using KiiniNet.Entities.Parametros;

namespace KiiniNet.Entities.Cat.Sistema
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class EstatusAsignacion
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string ColorGrafico { get; set; }
        [DataMember]
        public int Orden { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public List<Ticket> Ticket { get; set; }
        [DataMember]
        public virtual List<TicketAsignacion> TicketAsignacion { get; set; }
        [DataMember]
        public virtual List<EstatusAsignacionSubRolGeneral> EstatusAsignacionSubRolGeneralActual { get; set; }
        [DataMember]
        public virtual List<EstatusAsignacionSubRolGeneral> EstatusAsignacionSubRolGeneralAccion { get; set; }
        [DataMember]
        public virtual List<EstatusAsignacionSubRolGeneralDefault> EstatusAsignacionSubRolGeneralDefaultActual { get; set; }
        [DataMember]
        public virtual List<EstatusAsignacionSubRolGeneralDefault> EstatusAsignacionSubRolGeneralDefaultAccion { get; set; }
        [DataMember]
        public List<SubRolEscalacionPermitida> SubRolEscalacionPermitida { get; set; }

    }
}
