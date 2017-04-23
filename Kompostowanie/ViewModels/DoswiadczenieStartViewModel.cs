using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;
using Newtonsoft.Json;

namespace Kompostowanie.ViewModels
{
    public class DoswiadczenieStartViewModel : IJsonViewModel
    {
        public DoswiadczenieStartViewModel()
        {
            
        }

        public DoswiadczenieStartViewModel(DoswiadczenieRecord record)
        {
            Doswiadczenie = record;
        }

        [JsonIgnore]
        public DoswiadczenieRecord Doswiadczenie { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Pole Data startowa jest wymagane.")]
        public DateTime? DataStart
        {
            get { return Doswiadczenie.DataStart; }
            set { Doswiadczenie.DataStart = value; }
        }
        
        [JsonProperty("pryzmy")]
        public IEnumerable<PryzmaStartViewModel> Pryzmy { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new NHibernateContractResolver()
            });
        }
    }
}