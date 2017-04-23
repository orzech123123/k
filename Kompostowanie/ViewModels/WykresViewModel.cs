using System.Collections.Generic;
using Kompostowanie.Records;

namespace Kompostowanie.ViewModels
{
    public class WykresViewModel
    {
        public WykresViewModel()
        {
            LabelX = "Czas kompostowania [dni]";
            Pryzmy = new List<PryzmaWykresViewModel>();
        }

        public IList<PryzmaWykresViewModel> Pryzmy { get; set; }
        public DoswiadczenieRecord Doswiadczenie { get; set; }
        public string LabelX { get; set; }
        public string LabelY { get; set; }
    }

    public class PryzmaWykresViewModel
    {
        public PryzmaWykresViewModel()
        {
            Pomiary = new List<PomiarWykresViewModel>();
        }

        public string Pryzma { get; set; }
        public IList<PomiarWykresViewModel> Pomiary { get; set; }
    }

    public class PomiarWykresViewModel
    {
        public int Dzien { get; set; }
        public decimal Value { get; set; }
    }
}