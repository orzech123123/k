using System.Collections.Generic;
using Kompostowanie.Records;

namespace Kompostowanie.ViewModels
{
    public class PryzmaIndexViewModel
    {
        public DoswiadczenieRecord Doswiadczenie { get; set; }
        public IEnumerable<PryzmaRecord> Pryzmy { get; set; }
    }
}