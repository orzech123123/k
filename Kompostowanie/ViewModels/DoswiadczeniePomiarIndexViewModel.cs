using System.Collections.Generic;
using Kompostowanie.Records;

namespace Kompostowanie.ViewModels
{
    public class DoswiadczeniePomiarIndexViewModel
    {
        public DoswiadczenieRecord Doswiadczenie { get; set; }
        public IEnumerable<DoswiadczeniePomiarRecord> Pomiary { get; set; }
    }
}