using System;
using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;
using Newtonsoft.Json;

namespace Kompostowanie.ViewModels
{
    public class PryzmaStartViewModel : IJsonViewModel
    {
        private PryzmaRecord pryzma;
        private readonly PryzmaStartRecord record;

        [JsonProperty("pryzmaId")]
        public int PryzmaId { get; set; }

        public void SetPryzma(PryzmaRecord p)
        {
            pryzma = p;
        }

        public PryzmaStartViewModel(PryzmaStartRecord pryzmaStart)
        {
            record = pryzmaStart;
            pryzma = pryzmaStart.Pryzma;
            PryzmaId = pryzma.Id;
        }

        public PryzmaStartViewModel(PryzmaRecord pryzma)
        {
            record = new PryzmaStartRecord();
            this.pryzma = pryzma;
            PryzmaId = pryzma.Id;
        }

        public PryzmaStartViewModel()
        {
            record = new PryzmaStartRecord();
            pryzma = new PryzmaRecord();
        }

        [JsonIgnore]
        public PryzmaStartRecord Record { get { return record;  } }

        [JsonIgnore]
        public int Id { get { return record.Id; } }

        [JsonProperty("nazwa")]
        public string Nazwa
        {
            get { return pryzma.Nazwa; }
        }

        [JsonProperty("skladnikiMasa")]
        public decimal SkladnikiMasa
        {
            get { return pryzma.MasaSwiezaSuma; }
        }

        [JsonProperty("wilgotnosc")]
        public decimal Wilgotnosc
        {
            get { return pryzma.Wilgotnosc; }
        }

        [JsonProperty("iloscWSmSuma")]
        public decimal IloscWSmSuma
        {
            get { return pryzma.IloscWSmSuma; }
        }

        [JsonProperty("masaNSuma")]
        public decimal MasaNSuma
        {
            get { return pryzma.MasaNSuma; }
        }

        [JsonProperty("ileZebranoNaProbki")]
        [Required(ErrorMessage = "Pole Ile zebrano na próbki jest wymagane.")]
        public decimal IleZebranoNaProbki
        {
            get { return record.IleZebranoNaProbki; }
            set { record.IleZebranoNaProbki = value; }
        }

        [JsonProperty("wysokoscOdBrzegu")]
        [Required(ErrorMessage = "Pole Wysokość od brzegu jest wymagane.")]
        public decimal WysokoscOdBrzegu
        {
            get { return record.WysokoscOdBrzegu; }
            set { record.WysokoscOdBrzegu = value; }
        }
        
        [JsonProperty("przeplyw")]
        [Required(ErrorMessage = "Pole Przepływ jest wymagane.")]
        public decimal Przeplyw
        {
            get { return record.Przeplyw; }
            set { record.Przeplyw = value; }
        }
        
        [JsonProperty("ph1")]
        [Required(ErrorMessage = "Pole Ph1 jest wymagane.")]
        public decimal Ph1
        {
            get { return record.Ph1; }
            set { record.Ph1 = value; }
        }
        
        [JsonProperty("ph2")]
        [Required(ErrorMessage = "Pole Ph2 jest wymagane.")]
        public decimal Ph2
        {
            get { return record.Ph2; }
            set { record.Ph2 = value; }
        }
        
        [JsonProperty("konduktywnosc1")]
        [Required(ErrorMessage = "Pole Konduktywnosc1 jest wymagane.")]
        public decimal Konduktywnosc1
        {
            get { return record.Konduktywnosc1; }
            set { record.Konduktywnosc1 = value; }
        }

        [JsonProperty("konduktywnosc2")]
        [Required(ErrorMessage = "Pole Konduktywnosc2 jest wymagane.")]
        public decimal Konduktywnosc2
        {
            get { return record.Konduktywnosc2; }
            set { record.Konduktywnosc2 = value; }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new NHibernateContractResolver()
            });
        }
    }
}