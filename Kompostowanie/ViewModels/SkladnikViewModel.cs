using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kompostowanie.ViewModels
{
    public class SkladnikViewModel : IJsonViewModel
    {
        private readonly SkladnikRecord record;

        [JsonIgnore]
        public int PryzmaId { get; set; }

        public SkladnikViewModel() : this(new SkladnikRecord())
        {
            
        }

        public SkladnikViewModel(SkladnikRecord record)
        {
            this.record = record;
        }

        [JsonIgnore]
        public SkladnikRecord Record { get { return record;  } }

        [JsonIgnore]
        public int Id { get { return record.Id; } }

        [Required(ErrorMessage = "Pole Składnik jest wymagane.")]
        [JsonProperty("skladnik")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Skladnik Skladnik
        {
            get { return record.Skladnik; }
            set { record.Skladnik = value; }
        }

        [Required(ErrorMessage = "Pole Sucha masa jest wymagane.")]
        [JsonProperty("suchaMasa")]
        public decimal SuchaMasa
        {
            get { return record.SuchaMasa; }
            set { record.SuchaMasa = value; }
        }

        [Required(ErrorMessage = "Pole Udział jest wymagane.")]
        [JsonProperty("udzial")]
        public decimal Udzial
        {
            get { return record.Udzial; }
            set { record.Udzial = value; }
        }

        [Required(ErrorMessage = "Pole Ilość w s. m. jest wymagane.")]
        [JsonProperty("iloscWSm")]
        public decimal IloscWSm
        {
            get { return record.IloscWSm; }
            set { record.IloscWSm = value; }
        }

        [Required(ErrorMessage = "Pole Masa świeża jest wymagane.")]
        [JsonProperty("masaSwieza")]
        public decimal MasaSwieza
        {
            get { return record.MasaSwieza; }
            set { record.MasaSwieza = value; }
        }
        
        [Required(ErrorMessage = "Pole Zawartość C jest wymagane.")]
        [JsonProperty("zawartoscC")]
        public decimal ZawartoscC
        {
            get { return record.ZawartoscC; }
            set { record.ZawartoscC = value; }
        }

        [Required(ErrorMessage = "Pole Zawartość N jest wymagane.")]
        [JsonProperty("zawartoscN")]
        public decimal ZawartoscN
        {
            get { return record.ZawartoscN; }
            set { record.ZawartoscN = value; }
        }

        [Required(ErrorMessage = "Pole Masa C jest wymagane.")]
        [JsonProperty("masaC")]
        public decimal MasaC
        {
            get { return record.MasaC; }
            set { record.MasaC = value; }
        }

        [Required(ErrorMessage = "Pole Masa N jest wymagane.")]
        [JsonProperty("masaN")]
        public decimal MasaN
        {
            get { return record.MasaN; }
            set { record.MasaN = value; }
        }
        
        [Required(ErrorMessage = "Pole C/N jest wymagane.")]
        [JsonProperty("cn")]
        public decimal Cn
        {
            get { return record.Cn; }
            set { record.Cn = value; }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new NHibernateContractResolver()
            });
        }
    }
}