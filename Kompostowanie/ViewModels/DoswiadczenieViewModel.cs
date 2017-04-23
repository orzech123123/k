using System;
using System.ComponentModel.DataAnnotations;
using Kompostowanie.Records;

namespace Kompostowanie.ViewModels
{
    public class DoswiadczenieViewModel
    {
        private readonly DoswiadczenieRecord record;

        public DoswiadczenieViewModel() : this(new DoswiadczenieRecord())
        {

        }

        public DoswiadczenieViewModel(DoswiadczenieRecord record)
        {
            this.record = record;
        }

        public DoswiadczenieRecord Record { get { return record;  } }

        public int Id { get { return record.Id; } }

        [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
        public string Nazwa
        {
            get { return record.Nazwa; }
            set { record.Nazwa = value; }
        }

        [Required(ErrorMessage = "Pole Cel jest wymagane.")]
        public string Cel
        {
            get { return record.Cel; }
            set { record.Cel = value; }
        }

        [Required(ErrorMessage = "Pole Data jest wymagane.")]
        public DateTime Data
        {
            get { return record.Data; }
            set { record.Data = value; }
        }
    }
}