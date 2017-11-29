using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Mascaras
{
    [DataContract(IsReference = true)]
    public class TipoCampoMascara
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string DescripcionTexto { get; set; }
        [DataMember]
        public string LongitudMaximaPermitida { get; set; }

        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public string TipoDatoSql { get; set; }
        [DataMember]
        public bool Multiple { get; set; }
        [DataMember]
        public bool Checkbox { get; set; }
        [DataMember]
        public bool RadioButton { get; set; }
        [DataMember]
        public bool LongitudMinima { get; set; }
        [DataMember]
        public bool LongitudMaxima { get; set; }
        [DataMember]
        public bool Entero { get; set; }
        [DataMember]
        public bool Decimal { get; set; }
        [DataMember]
        public bool ValorMinimo { get; set; }
        [DataMember]
        public bool ValorMaximo { get; set; }
        [DataMember]
        public bool SimboloMoneda { get; set; }
        [DataMember]
        public bool Catalogo { get; set; }
        [DataMember]
        public bool Mask { get; set; }
        [DataMember]
        public bool UploadFile { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }
        [DataMember]
        public virtual List<CampoMascara> CampoMascara { get; set; }
    }
}
