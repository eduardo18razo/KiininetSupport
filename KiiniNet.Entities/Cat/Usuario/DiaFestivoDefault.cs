using System.Runtime.Serialization;

namespace KiiniNet.Entities.Cat.Usuario
{
    [DataContract(IsReference = true)]
    public class DiaFestivoDefault
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Dia { get; set; }
        [DataMember]
        public int Mes { get; set; }
        [DataMember]
        public bool Habilitado { get; set; }

        [DataMember]
        public string Descripcion
        {
            get { return DateFormat; }
            set { DateFormat = value; }
        }

        private string DateFormat
        {
            get; 
            set;
        }

    }
}
