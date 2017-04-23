using System.Collections.Generic;
using Kompostowanie.Records;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Kompostowanie.ViewModels
{
    public class SkladnikiViewModel : IJsonViewModel
    {
        public SkladnikiViewModel()
        {
            
        }

        public SkladnikiViewModel(PryzmaRecord record)
        {
            Pryzma = record;
        }

        [JsonIgnore]
        public PryzmaRecord Pryzma { get; set; }

        [JsonProperty("skladniki")]
        public IEnumerable<SkladnikViewModel> Skladniki { get; set; }
        
        [Required(ErrorMessage = "Pole WilgotnoscOstateczna jest wymagane.")]
        [JsonProperty("wilgotnoscOstateczna")]
        public decimal WilgotnoscOstateczna
        {
            get { return Pryzma.WilgotnoscOstateczna; }
            set { Pryzma.WilgotnoscOstateczna = value; }
        }

        [Required(ErrorMessage = "Pole SuchaMasa jest wymagane.")]
        [JsonProperty("suchaMasa")]
        public decimal SuchaMasa
        {
            get { return Pryzma.SuchaMasa; }
            set { Pryzma.SuchaMasa = value; }
        }

        [Required(ErrorMessage = "Pole Wilgotnosc jest wymagane.")]
        [JsonProperty("wilgotnosc")]
        public decimal Wilgotnosc
        {
            get { return Pryzma.Wilgotnosc; }
            set { Pryzma.Wilgotnosc = value; }
        }

        [Required(ErrorMessage = "Pole UdzialSuma jest wymagane.")]
        [JsonProperty("udzialSuma")]
        public decimal UdzialSuma
        {
            get { return Pryzma.UdzialSuma; }
            set { Pryzma.UdzialSuma = value; }
        }

        [Required(ErrorMessage = "Pole IloscWSmSuma jest wymagane.")]
        [JsonProperty("iloscWSmSuma")]
        public decimal IloscWSmSuma
        {
            get { return Pryzma.IloscWSmSuma; }
            set { Pryzma.IloscWSmSuma = value; }
        }

        [Required(ErrorMessage = "Pole MasaSwiezaSuma jest wymagane.")]
        [JsonProperty("masaSwiezaSuma")]
        public decimal MasaSwiezaSuma
        {
            get { return Pryzma.MasaSwiezaSuma; }
            set { Pryzma.MasaSwiezaSuma = value; }
        }

        [Required(ErrorMessage = "Pole MasaCSuma jest wymagane.")]
        [JsonProperty("masaCSuma")]
        public decimal MasaCSuma
        {
            get { return Pryzma.MasaCSuma; }
            set { Pryzma.MasaCSuma = value; }
        }
        
        [Required(ErrorMessage = "Pole MasaNSuma jest wymagane.")]
        [JsonProperty("masaNSuma")]
        public decimal MasaNSuma
        {
            get { return Pryzma.MasaNSuma; }
            set { Pryzma.MasaNSuma = value; }
        }

        [Required(ErrorMessage = "Pole Cn jest wymagane.")]
        [JsonProperty("cn")]
        public decimal Cn
        {
            get { return Pryzma.Cn; }
            set { Pryzma.Cn = value; }
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