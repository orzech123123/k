using System.Linq;
using System.Web.Mvc;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class AeracjaController : AuthorizedController
    {
        private readonly IDoswiadczenieRepository doswiadczenieRepository;
        private readonly IPryzmaRepository pryzmaRepository;
        private readonly IAeracjaRepository aeracjaRepository;
        private readonly IAeracjaEntryRepository aeracjaEntryRepository;

        public AeracjaController()
        {
            doswiadczenieRepository = new DoswiadczenieRepository(UnitOfWork);
            pryzmaRepository = new PryzmaRepository(UnitOfWork);
            aeracjaRepository = new AeracjaRepository(UnitOfWork);
            aeracjaEntryRepository = new AeracjaEntryRepository(UnitOfWork);
        }

        public ActionResult Index(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            var viewModel = new DoswiadczenieAeracjaIndexViewModel
            {
                Doswiadczenie = doswiadczenieRecord,
                Aeracje = doswiadczenieRecord.Aeracje.ToList()
            };

            return View(viewModel);
        }

        public ActionResult Create(int id, int? aeracjaId = null)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            var aeracjaRecord = aeracjaId.HasValue ? aeracjaRepository.Get(aeracjaId.Value) : new AeracjaRecord
            {
                Doswiadczenie = doswiadczenieRecord
            };

            AeracjaViewModel viewModel;

            if (doswiadczenieRecord.Pryzmy.ToList().Any(p => aeracjaEntryRepository.GetByPryzmaAndAeracja(p.Id, aeracjaRecord.Id) != null))
            {
                viewModel = new AeracjaViewModel(aeracjaRecord)
                {
                    Aeracje = doswiadczenieRecord.Pryzmy.ToList()
                            .Select(p => new AeracjaEntryViewModel(aeracjaEntryRepository.GetByPryzmaAndAeracja(p.Id, aeracjaRecord.Id)))
                            .ToList()
                };
            }
            else
            {
                viewModel = new AeracjaViewModel(aeracjaRecord)
                {
                    Aeracje = doswiadczenieRecord.Pryzmy.Select(p => new AeracjaEntryViewModel(p, aeracjaRecord))
                };
            }

            ViewBag.Create = !aeracjaId.HasValue;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, int id, int aeracjaId)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            var aeracjaRecord = aeracjaRepository.Get(aeracjaId) ?? new AeracjaRecord { Doswiadczenie = doswiadczenieRecord };

            var viewModel = new AeracjaViewModel(aeracjaRecord);

            TryUpdateModel(viewModel);
            
            if (ModelState.IsValid)
            {
                viewModel.Aeracja.Doswiadczenie = doswiadczenieRecord;
                aeracjaRepository.Add(viewModel.Aeracja);

                foreach (var aeracjaEntry in viewModel.Aeracje)
                {
                    var pryzma = pryzmaRepository.Get(aeracjaEntry.PryzmaId);
                    aeracjaEntry.Record.Pryzma = pryzma;
                    aeracjaEntry.Record.Aeracja = aeracjaRecord;
                    aeracjaEntryRepository.Add(aeracjaEntry.Record);
                }

                return RedirectToAction("Index", new { id = doswiadczenieRecord.Id });
            }
            
            foreach (var aeracjaEntry in viewModel.Aeracje)
            {
                aeracjaEntry.SetPryzma(pryzmaRepository.Get(aeracjaEntry.PryzmaId));
                aeracjaEntry.SetAeracja(aeracjaRepository.Get(aeracjaRecord.Id));
            }
            viewModel.Aeracja = aeracjaRecord;

            UnitOfWork.Rollback();

            ViewBag.Create = true;
            return View(viewModel);
        }

        public ActionResult Delete(int id)
        {
            var aeracja = aeracjaRepository.Get(id);
            var doswiadczenieId = aeracja.Doswiadczenie.Id;

            foreach (var aeracjaAeracjaEntry in aeracja.AeracjaEntries)
                aeracjaEntryRepository.Remove(aeracjaAeracjaEntry);
            aeracjaRepository.Remove(aeracja);

            return RedirectToAction("Index", new {id = doswiadczenieId});
        }
    }
}