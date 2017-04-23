using System;
using System.Linq;
using System.Web.Mvc;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class DoswiadczenieController : AuthorizedController
    {
        private readonly IDoswiadczenieRepository doswiadczenieRepository;
        private readonly IPryzmaRepository pryzmaRepository;
        private readonly IPryzmaStartRepository pryzmaStartRepository;
        private readonly IPryzmaStopRepository pryzmaStopRepository;
        private readonly ISkladnikRepository skladnikRepository;
        private readonly IDoswiadczeniePomiarRepository doswiadczeniePomiarRepository;
        private readonly IPryzmaPomiarRepository pryzmaPomiarRepository;
        private readonly IAeracjaRepository aeracjaRepository;
        private readonly IAeracjaEntryRepository aeracjaEntryRepository;

        public DoswiadczenieController()
        {
            doswiadczenieRepository = new DoswiadczenieRepository(UnitOfWork);
            pryzmaRepository = new PryzmaRepository(UnitOfWork);
            skladnikRepository = new SkladnikRepository(UnitOfWork);
            pryzmaStartRepository = new PryzmaStartRepository(UnitOfWork);
            pryzmaStopRepository = new PryzmaStopRepository(UnitOfWork);
            doswiadczeniePomiarRepository = new DoswiadczeniePomiarRepository(UnitOfWork);
            pryzmaPomiarRepository = new PryzmaPomiarRepository(UnitOfWork);
            aeracjaRepository = new AeracjaRepository(UnitOfWork);
            aeracjaEntryRepository = new AeracjaEntryRepository(UnitOfWork);
        }

        public ActionResult Index()
        {
            var doswiadczenieRecords = doswiadczenieRepository.Query()
                .OrderByDescending(d => d.Data)
                .ToList();

            return View(doswiadczenieRecords);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var viewModel = new DoswiadczenieViewModel
            {
                Data = DateTime.Now
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            var viewModel = new DoswiadczenieViewModel();

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                doswiadczenieRepository.Add(viewModel.Record);
                return RedirectToAction("Index");
            }
            
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            
            foreach (var aeracja in doswiadczenieRecord.Aeracje.ToList())
            {
                foreach (var aeracjaAeracjaEntry in aeracja.AeracjaEntries)
                    aeracjaEntryRepository.Remove(aeracjaAeracjaEntry);
                aeracjaRepository.Remove(aeracja);
            }

            foreach (var doswiadczeniePomiar in doswiadczenieRecord.Pomiary)
            {
                foreach (var pryzmaPomiarRecord in doswiadczeniePomiar.Pomiary)
                    pryzmaPomiarRepository.Remove(pryzmaPomiarRecord);
                doswiadczeniePomiarRepository.Remove(doswiadczeniePomiar);
            }
            
            foreach (var pryzmaRecord in doswiadczenieRecord.Pryzmy.ToList())
            {
                var pryzmaStartRecord = pryzmaStartRepository.GetByPryzma(pryzmaRecord.Id);
                pryzmaStartRepository.Remove(pryzmaStartRecord);
                var pryzmaStopRecord = pryzmaStopRepository.GetByPryzma(pryzmaRecord.Id);
                pryzmaStopRepository.Remove(pryzmaStopRecord);

                foreach (var skladnikRecord in pryzmaRecord.Skladniki.ToList())
                {
                    skladnikRepository.Remove(skladnikRecord);
                    pryzmaRecord.Skladniki.Remove(skladnikRecord);
                }

                pryzmaRepository.Remove(pryzmaRecord);
                doswiadczenieRecord.Pryzmy.Remove(pryzmaRecord);
            }

            doswiadczenieRepository.Remove(doswiadczenieRecord);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            var viewModel = new DoswiadczenieViewModel(doswiadczenieRecord);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection, int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            var viewModel = new DoswiadczenieViewModel(doswiadczenieRecord);

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            UnitOfWork.Rollback();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Start(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!doswiadczenieRecord.Pryzmy.Any() || doswiadczenieRecord.Pryzmy.ToList().Any(p => !p.Skladniki.Any()))
            {
                Notifier.AddError("Nie można wystartować doświadczenia. Brak pryzm lub nie wszystkie pryzmy mają wypełnione składniki.");
                return RedirectToAction("Index");
            }

            DoswiadczenieStartViewModel viewModel;

            if (doswiadczenieRecord.Pryzmy.ToList().Any(p => pryzmaStartRepository.GetByPryzma(p.Id) != null))
            {
                viewModel = new DoswiadczenieStartViewModel(doswiadczenieRecord)
                {
                    Pryzmy = doswiadczenieRecord.Pryzmy.ToList()
                            .Select(p => new PryzmaStartViewModel(pryzmaStartRepository.GetByPryzma(p.Id)))
                            .ToList()
                };
            }
            else
            {
                viewModel = new DoswiadczenieStartViewModel(doswiadczenieRecord)
                {
                    Pryzmy = doswiadczenieRecord.Pryzmy.Select(p => new PryzmaStartViewModel(p))
                };
            }

            if (doswiadczenieRecord.DataStart == null)
            {
                doswiadczenieRecord.DataStart = DateTime.Now > doswiadczenieRecord.Data ? DateTime.Now : doswiadczenieRecord.Data;
                UnitOfWork.Rollback();
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Start(FormCollection collection, int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            
            var viewModel = new DoswiadczenieStartViewModel(doswiadczenieRecord);

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                // ReSharper disable once PossibleInvalidOperationException
                if(viewModel.DataStart.Value < viewModel.Doswiadczenie.Data)
                    ModelState.AddModelError("DataStart", string.Format("Data startowa doświadczenia nie może być mniejsza niż data założenia doświadczenia, tj. {0}", viewModel.Doswiadczenie.Data.ToString("yyyy-MM-dd")));
            }

            if (ModelState.IsValid && !doswiadczenieRecord.Started)
            {
                foreach (var pryzmaStart in viewModel.Pryzmy)
                {
                    var pryzma = pryzmaRepository.Get(pryzmaStart.PryzmaId);
                    pryzmaStart.Record.Pryzma = pryzma;
                    pryzmaStartRepository.Add(pryzmaStart.Record);
                }
                
                doswiadczenieRecord.Started = true;

                return RedirectToAction("Index");
            }
            
            if (doswiadczenieRecord.Started)
                Notifier.AddError("Nie można wystartować już wystartowanego doświadczenia.");

            foreach (var pryzmaStart in viewModel.Pryzmy)
            {
                pryzmaStart.SetPryzma(pryzmaRepository.Get(pryzmaStart.PryzmaId));
            }
            viewModel.Doswiadczenie = doswiadczenieRecord;

            UnitOfWork.Rollback();

            return View(viewModel);
        }
        
        [HttpGet]
        public ActionResult Stop(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);
            
            DoswiadczenieStopViewModel viewModel;

            if (doswiadczenieRecord.Pryzmy.ToList().Any(p => pryzmaStopRepository.GetByPryzma(p.Id) != null))
            {
                viewModel = new DoswiadczenieStopViewModel(doswiadczenieRecord)
                {
                    Pryzmy = doswiadczenieRecord.Pryzmy.ToList()
                            .Select(p => new PryzmaStopViewModel(pryzmaStopRepository.GetByPryzma(p.Id)))
                            .ToList()
                };
            }
            else
            {
                viewModel = new DoswiadczenieStopViewModel(doswiadczenieRecord)
                {
                    Pryzmy = doswiadczenieRecord.Pryzmy.Select(p => new PryzmaStopViewModel(p))
                };
            }

            if (doswiadczenieRecord.DataStop == null)
            {
                doswiadczenieRecord.DataStop = DateTime.Now;
                UnitOfWork.Rollback();
            }

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Stop(FormCollection collection, int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            var viewModel = new DoswiadczenieStopViewModel(doswiadczenieRecord);

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                // ReSharper disable once PossibleInvalidOperationException
                if (viewModel.DataStop.Value < viewModel.Doswiadczenie.DataStart)
                    ModelState.AddModelError("DataStop", string.Format("Data zakończenia doświadczenia nie może być mniejsza niż data rozpoczęcia doświadczenia, tj. {0}", viewModel.Doswiadczenie.DataStart.Value.ToString("yyyy-MM-dd")));
            }

            if (ModelState.IsValid && !doswiadczenieRecord.Stopped)
            {
                foreach (var pryzmaStop in viewModel.Pryzmy)
                {
                    var pryzma = pryzmaRepository.Get(pryzmaStop.PryzmaId);
                    pryzmaStop.Record.Pryzma = pryzma;
                    pryzmaStopRepository.Add(pryzmaStop.Record);
                }

                doswiadczenieRecord.Stopped = true;
                doswiadczenieRecord.DataStop = viewModel.DataStop;

                return RedirectToAction("Index");
            }

            if (doswiadczenieRecord.Stopped)
                Notifier.AddError("Nie można zakończyć już zakończonego doświadczenia.");

            foreach (var pryzmaStart in viewModel.Pryzmy)
            {
                pryzmaStart.SetPryzma(pryzmaRepository.Get(pryzmaStart.PryzmaId));
            }
            viewModel.Doswiadczenie = doswiadczenieRecord;

            UnitOfWork.Rollback();

            return View(viewModel);
        }
    }
}