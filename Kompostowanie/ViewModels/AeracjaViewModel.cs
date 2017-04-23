using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;
using Newtonsoft.Json;

namespace Kompostowanie.ViewModels
{
    public class AeracjaViewModel : IJsonViewModel
    {
        public AeracjaViewModel()
        {
            
        }

        public AeracjaViewModel(AeracjaRecord record)
        {
            Aeracja = record;
        }

        [JsonIgnore]
        public AeracjaRecord Aeracja { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
        public string Nazwa
        {
            get { return Aeracja.Nazwa; }
            set { Aeracja.Nazwa = value; }
        }
        
        [JsonProperty("aeracje")]
        public IEnumerable<AeracjaEntryViewModel> Aeracje { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new NHibernateContractResolver()
            });
        }
    }
}