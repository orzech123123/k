using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;

namespace Kompostowanie.ViewModels
{
    public class PryzmaViewModel
    {
        private readonly PryzmaRecord record;

        public int DoswiadczenieId { get; set; }

        public PryzmaViewModel() : this(new PryzmaRecord())
        {
            
        }

        public PryzmaViewModel(PryzmaRecord record)
        {
            this.record = record;
        }

        public PryzmaRecord Record { get { return record;  } }

        public int Id { get { return record.Id; } }

        [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
        public string Nazwa
        {
            get { return record.Nazwa; }
            set { record.Nazwa = value; }
        }
    }
}