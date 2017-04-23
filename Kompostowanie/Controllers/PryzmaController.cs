using System.Linq;
using System.Web.Mvc;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class PryzmaController : AuthorizedController
    {
        private readonly IPryzmaRepository pryzmaRepository;
        private readonly IDoswiadczenieRepository doswiadczenieRepository;
        private readonly ISkladnikRepository skladnikRepository;
        private readonly IPryzmaStartRepository pryzmaStartRepository;

        public PryzmaController()
        {
            skladnikRepository = new SkladnikRepository(UnitOfWork);
            pryzmaRepository = new PryzmaRepository(UnitOfWork);
            doswiadczenieRepository = new DoswiadczenieRepository(UnitOfWork);
            pryzmaStartRepository = new PryzmaStartRepository(UnitOfWork);
        }

        public ActionResult Index(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            var pryzmaRecords = pryzmaRepository.QueryByDoswiadczenieId(id).ToList();

            var viewModel = new PryzmaIndexViewModel
            {
                Doswiadczenie = doswiadczenieRecord,
                Pryzmy = pryzmaRecords
            };

            return View(viewModel);
        }

        public ActionResult Delete(int id)
        {
            var pryzmaRecord = pryzmaRepository.Get(id);
            
            if (pryzmaRecord.Doswiadczenie.Started)
            {
                Notifier.AddError("Doświadczenie zostało wystartowane. Brak możliwości usunięcia pryzm.");
                return RedirectToAction("Index", new { id = pryzmaRecord.Doswiadczenie.Id });
            }

            foreach (var skladnikRecord in pryzmaRecord.Skladniki.ToList())
            {
                skladnikRepository.Remove(skladnikRecord);
                pryzmaRecord.Skladniki.Remove(skladnikRecord);
            }

            foreach (var pryzmaStartRecord in pryzmaRecord.Starts.ToList())
            {
                pryzmaStartRepository.Remove(pryzmaStartRecord);
                pryzmaRecord.Starts.Remove(pryzmaStartRecord);
            }

            pryzmaRepository.Remove(pryzmaRecord);

            return RedirectToAction("Index", new { id = pryzmaRecord.Doswiadczenie.Id });
        }
        
        [HttpGet]
        public ActionResult Create(int id)
        {
            var pryzmaCount = pryzmaRepository.QueryByDoswiadczenieId(id).Count();
            var doswiadczenie = doswiadczenieRepository.Get(id);
            
            if (doswiadczenie.Started)
            {
                Notifier.AddError("Doświadczenie zostało wystartowane. Brak możliwości dodawania pryzm.");
                return RedirectToAction("Index", new { id });
            }

            if (pryzmaCount >= 8)
            {
                Notifier.AddError(string.Format("Doświadczenie może mieć maksymalnie 8 pryzm. Doświadczenie {0} posiada już 8 pryzm.", doswiadczenie.Nazwa));
                return RedirectToAction("Index", new {id});
            }

            var viewModel = new PryzmaViewModel
            {
                DoswiadczenieId = id
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var viewModel = new PryzmaViewModel();

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                viewModel.Record.Doswiadczenie = doswiadczenieRepository.Get(viewModel.DoswiadczenieId);
                pryzmaRepository.Add(viewModel.Record);

                return RedirectToAction("Index", new { id = viewModel.DoswiadczenieId });
            }

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var pryzmaRecord = pryzmaRepository.Get(id);
            var viewModel = new PryzmaViewModel(pryzmaRecord)
            {
                DoswiadczenieId = pryzmaRecord.Doswiadczenie.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var pryzmaRecord = pryzmaRepository.Get(id);
            var viewModel = new PryzmaViewModel(pryzmaRecord);

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", new { pryzmaRecord.Doswiadczenie.Id });
            }

            UnitOfWork.Rollback();

            return View(viewModel);
        }
    }
}