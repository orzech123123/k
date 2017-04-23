using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;
using Newtonsoft.Json;

namespace Kompostowanie.ViewModels
{
    public class DoswiadczenieStopViewModel : IJsonViewModel
    {
        public DoswiadczenieStopViewModel()
        {
            
        }

        public DoswiadczenieStopViewModel(DoswiadczenieRecord record)
        {
            Doswiadczenie = record;
        }

        [JsonIgnore]
        public DoswiadczenieRecord Doswiadczenie { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "Pole Data zakończenia jest wymagane.")]
        public DateTime? DataStop
        {
            get { return Doswiadczenie.DataStop; }
            set { Doswiadczenie.DataStop = value; }
        }
        
        [JsonProperty("pryzmy")]
        public IEnumerable<PryzmaStopViewModel> Pryzmy { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                ContractResolver = new NHibernateContractResolver()
            });
        }
    }
}