using System.Collections.Generic;
using Kompostowanie.Records;

namespace Kompostowanie.ViewModels
{
    public class DoswiadczenieAeracjaIndexViewModel
    {
        public DoswiadczenieRecord Doswiadczenie { get; set; }
        public IEnumerable<AeracjaRecord> Aeracje { get; set; }
    }
}