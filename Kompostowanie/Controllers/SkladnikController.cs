using System.Linq;
using System.Web.Mvc;
using Kompostowanie.Extensions;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class SkladnikController : AuthorizedController
    {
        private readonly IPryzmaRepository pryzmaRepository;
        private readonly ISkladnikRepository skladnikRepository;

        public SkladnikController()
        {
            pryzmaRepository = new PryzmaRepository(UnitOfWork);
            skladnikRepository = new SkladnikRepository(UnitOfWork);
        }

        [HttpGet]
        public ActionResult Index(int id)
        {
            var pryzmaRecord = pryzmaRepository.Get(id);
            var skladnikRecords = skladnikRepository.QueryByPryzma(id).ToList();

            var viewModel = new SkladnikiViewModel(pryzmaRecord)
            {
                Skladniki = skladnikRecords.Select(s => new SkladnikViewModel(s))
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection, int id)
        {
            var pryzmaRecord = pryzmaRepository.Get(id);
            
            if (pryzmaRecord.Doswiadczenie.Started)
            {
                Notifier.AddError("Doświadczenie zostało wystartowane. Brak możliwości modyfikacji składników pryzmy.");
                return RedirectToAction("Index", new { id });
            }

            var viewModel = new SkladnikiViewModel(pryzmaRecord);
            TryUpdateModel(viewModel);

            ExtraValidation(pryzmaRecord, viewModel);
            
            if (ModelState.IsValid)
            {
                var skladnikRecords = skladnikRepository.QueryByPryzma(id).ToList();
                foreach (var skladnikRecord in skladnikRecords)
                    skladnikRepository.Remove(skladnikRecord);

                if(viewModel.Skladniki != null)
                    foreach (var skladnikViewModel in viewModel.Skladniki)
                    {
                        skladnikViewModel.Record.Pryzma = pryzmaRecord;
                        skladnikRepository.Add(skladnikViewModel.Record);
                    }

                return RedirectToAction("Index", "Pryzma", new { id = pryzmaRecord.Doswiadczenie.Id });
            }

            viewModel.Pryzma = pryzmaRecord;

            return View(viewModel);
            
        }

        private void ExtraValidation(PryzmaRecord pryzmaRecord, SkladnikiViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return;

            CheckDistinctSkladniki(viewModel);
            CheckSameSkladniki(pryzmaRecord, viewModel);
        }

        private void CheckDistinctSkladniki(SkladnikiViewModel viewModel)
        {
            var isNotDistinct = viewModel.Skladniki.Select(s => s.Skladnik).GroupBy(s => s).Any(g => g.Count() > 1);

            if (isNotDistinct)
            {
                Notifier.AddError("Pryzma nie może zawierać wielokrotnie tych samych składników");
                ModelState.AddModelError(string.Empty, string.Empty);
            }
        }

        private void CheckSameSkladniki(PryzmaRecord pryzmaRecord, SkladnikiViewModel viewModel)
        {
            var otherPryzma = pryzmaRepository
                .QueryByDoswiadczenieId(pryzmaRecord.Doswiadczenie.Id)
                .ToList()
                .FirstOrDefault(p => p.Id != pryzmaRecord.Id);

            if (otherPryzma == null || !otherPryzma.Skladniki.Any())
                return;

            var skladniki = viewModel.Skladniki.Select(s => s.Skladnik).ToList();

            var skladnikiOther = skladnikRepository.QueryByPryzma(otherPryzma.Id)
                .Select(s => s.Skladnik)
                .ToList();

            if (skladniki.OrderBy(s => s).SequenceEqual(skladnikiOther.OrderBy(s => s)))
                return;

            Notifier.AddError(
                string.Format("Pryzma musi zawierać tylko takie same składniki jak poprzednio dodana pryzma, tj. {0}",
                    string.Join(", ", skladnikiOther.Select(s => s.ToDescription()))));
            ModelState.AddModelError(string.Empty, string.Empty);
        }
    }
}