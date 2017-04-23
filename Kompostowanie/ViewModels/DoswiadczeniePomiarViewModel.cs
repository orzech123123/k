using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Kompostowanie.Records;
using Newtonsoft.Json;

namespace Kompostowanie.ViewModels
{
    public class DoswiadczeniePomiarViewModel : IJsonViewModel
    {
        private readonly DoswiadczeniePomiarRecord record;

        public DoswiadczeniePomiarViewModel(DoswiadczeniePomiarRecord record)
        {
            this.record = record;
            Pomiary = new List<PryzmaPomiarViewModel>();
            StanyLicznikow = new List<PryzmaPomiarViewModel>();
            Przeplywy = new List<PryzmaPomiarViewModel>();
        }

        public DoswiadczeniePomiarViewModel()
        {
            record = new DoswiadczeniePomiarRecord();
            Pomiary = new List<PryzmaPomiarViewModel>();
            StanyLicznikow = new List<PryzmaPomiarViewModel>();
            Przeplywy = new List<PryzmaPomiarViewModel>();
        }

        [JsonProperty("dataStart")]
        public string DataStart
        {
            get
            {
                return record.Doswiadczenie.DataStart.Value.ToString("yyyy-MM-dd");
            }
        }

        [JsonProperty("pomiary")]
        public IList<PryzmaPomiarViewModel> Pomiary { get; set; }

        [JsonProperty("stanyLicznikow")]
        public IList<PryzmaPomiarViewModel> StanyLicznikow { get; set; }
        [JsonProperty("przeplywy")]
        public IList<PryzmaPomiarViewModel> Przeplywy { get; set; }

        [JsonIgnore]
        public DoswiadczeniePomiarRecord Record { get { return record; } }

        [Required(ErrorMessage = "Pole Data jest wymagane.")]
        public DateTime Data
        {
            get { return record.Data; }
            set { record.Data = value; }
        }

        [JsonProperty("data")]
        public string DataString
        {
            get { return record.Data.ToString("yyyy-MM-dd"); }
        }

        [Required(ErrorMessage = "Pole Dzień jest wymagane.")]
        [JsonProperty("dzien")]
        public int Dzien
        {
            get { return record.Dzien; }
            set { record.Dzien = value; }
        }

        [Required(ErrorMessage = "Pole Godzina jest wymagane.")]
        [JsonProperty("godzina")]
        public decimal Godzina
        {
            get { return record.Godzina; }
            set { record.Godzina = value; }
        }

        [Required(ErrorMessage = "Pole CzasComputed jest wymagane.")]
        [JsonProperty("czasComputed")]
        public decimal CzasComputed
        {
            get { return record.CzasComputed; }
            set { record.CzasComputed = value; }
        }

        [Required(ErrorMessage = "Pole DzienComputed jest wymagane.")]
        [JsonProperty("dzienComputed")]
        public decimal DzienComputed
        {
            get { return record.DzienComputed; }
            set { record.DzienComputed = value; }
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