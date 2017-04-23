using Kompostowanie.Extensions;
using Kompostowanie.Records;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kompostowanie.ViewModels
{
    public class PryzmaPomiarViewModel : IJsonViewModel
    {
        public PryzmaPomiarViewModel()
        {
            
        }

        public PryzmaPomiarViewModel(PryzmaPomiarRecord record)
        {
            PryzmaId = record.Pryzma.Id;
            PryzmaName = record.Pryzma.Nazwa;
            Typ = record.Typ;
            TypName = record.Typ.ToDescription();
            Value = record.Value;
        }

        [JsonProperty("pryzmaId")]
        public int PryzmaId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("typ")]
        public PryzmaPomiarTyp Typ { get; set; }
        
        [JsonProperty("typName")]
        public string TypName { get; set; }

        [JsonProperty("pryzmaName")]
        public string PryzmaName { get; set; }

        [JsonProperty("value")]
        public decimal Value { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = new NHibernateContractResolver()
            });
        }
    }
}