using System;
using System.Linq;
using System.Web.Mvc;
using Kompostowanie.Extensions;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class PomiarController : AuthorizedController
    {
        private readonly IDoswiadczenieRepository doswiadczenieRepository;
        private readonly IDoswiadczeniePomiarRepository doswiadczeniePomiarRepository;
        private readonly IPryzmaPomiarRepository pryzmaPomiarRepository;
        private readonly IPryzmaRepository pryzmaRepository;

        public PomiarController()
        {
            doswiadczenieRepository = new DoswiadczenieRepository(UnitOfWork);
            doswiadczeniePomiarRepository = new DoswiadczeniePomiarRepository(UnitOfWork);
            pryzmaPomiarRepository = new PryzmaPomiarRepository(UnitOfWork);
            pryzmaRepository = new PryzmaRepository(UnitOfWork);
        }

        public ActionResult Index(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!doswiadczenieRecord.Started)
            {
                Notifier.AddError("Nie można modyfikować pomiarów nie wystartowanego doświadczenia. Rozpocznij doświadczenie, żeby modyfikować.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var viewModel = new DoswiadczeniePomiarIndexViewModel
            {
                Doswiadczenie = doswiadczenieRecord,
                Pomiary = doswiadczenieRecord.Pomiary.ToList()
            };

            return View(viewModel);
        }

        public ActionResult View(int id)
        {
            var doswiadczeniePomiarRecord = doswiadczeniePomiarRepository.Get(id);
            var viewModel = GetViewModel(doswiadczeniePomiarRecord.Doswiadczenie, doswiadczeniePomiarRecord);

            ViewBag.Create = false;

            return View("Create", viewModel);
        }

        [HttpGet]
        public ActionResult Create(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!doswiadczenieRecord.Started)
            {
                Notifier.AddError("Nie można modyfikować pomiarów nie wystartowanego doświadczenia. Rozpocznij doświadczenie, żeby modyfikować.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var viewModel = GetViewModel(doswiadczenieRecord);

            ViewBag.Create = true;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            var viewModel = GetViewModel(doswiadczenieRecord);

            TryUpdateModel(viewModel);

            if (ModelState.IsValid)
            {
                var daty = doswiadczenieRecord.Pomiary.Select(p => p.Data).ToList();
                Func<DateTime, bool> validDate = d => daty.Any() ? d > daty.Max(dd => dd) : d >= doswiadczenieRecord.DataStart;

                if (!validDate(viewModel.Data))
                {
                    Notifier.AddError("Data musi być większa lub równa niż data startowa doświadczenia lub gdy zostało już dodany chociaż jeden pomiar - większa od daty ostatniego pomiaru.");
                    ModelState.AddModelError("_FORM", string.Empty);
                }
            }

            if (ModelState.IsValid)
            {
                doswiadczeniePomiarRepository.Add(viewModel.Record);

                foreach (var pomiarViewModel in viewModel.Pomiary)
                    CreatePomiarRecord(viewModel, pomiarViewModel);

                foreach (var pomiarViewModel in viewModel.Przeplywy)
                    CreatePomiarRecord(viewModel, pomiarViewModel);

                foreach (var pomiarViewModel in viewModel.StanyLicznikow)
                    CreatePomiarRecord(viewModel, pomiarViewModel);

                return RedirectToAction("Index", new { id });
            }

            UnitOfWork.Rollback();
            ViewBag.Create = true;

            return View(viewModel);
        }

        private void CreatePomiarRecord(DoswiadczeniePomiarViewModel viewModel, PryzmaPomiarViewModel pomiarViewModel)
        {
            pryzmaPomiarRepository.Add(new PryzmaPomiarRecord
            {
                DoswiadczeniePomiar = viewModel.Record,
                Pryzma = pryzmaRepository.Get(pomiarViewModel.PryzmaId),
                Typ = pomiarViewModel.Typ,
                Value = pomiarViewModel.Value
            });
        }

        private DoswiadczeniePomiarViewModel GetViewModel(DoswiadczenieRecord doswiadczenieRecord, DoswiadczeniePomiarRecord pomiarRecord = null, bool nested = false)
        {
            DoswiadczeniePomiarViewModel viewModel;
            if (pomiarRecord == null)
                viewModel = new DoswiadczeniePomiarViewModel(new DoswiadczeniePomiarRecord
                {
                    Doswiadczenie = doswiadczenieRecord,
                    Data = !doswiadczenieRecord.Pomiary.Any() ? doswiadczenieRecord.DataStart.Value : doswiadczenieRecord.Pomiary.Max(p => p.Data).AddDays(1),
                    Dzien = !doswiadczenieRecord.Pomiary.Any() ? 0 : doswiadczenieRecord.Pomiary.Max(p => p.Dzien) + 1
                });
            else
                viewModel = new DoswiadczeniePomiarViewModel(pomiarRecord);

            if (viewModel.Dzien != 0 && !nested)
            {
                var firstPomiar = doswiadczenieRecord.Pomiary.OrderByDescending(p => p.Dzien).First();
                ViewBag.First = GetViewModel(firstPomiar.Doswiadczenie, firstPomiar, true);
            }

            foreach (var pryzmaRecord in doswiadczenieRecord.Pryzmy)
            {
                foreach (PryzmaPomiarTyp typ in Enum.GetValues(typeof(PryzmaPomiarTyp)))
                {
                    if (typ == PryzmaPomiarTyp.StanLicznikow || typ == PryzmaPomiarTyp.Przeplyw)
                        continue;

                    var pomiar = pryzmaPomiarRepository.GetByPryzmaAndTyp(pryzmaRecord.Id, viewModel.Record.Id, typ);
                    viewModel.Pomiary.Add(pomiar != null
                        ? new PryzmaPomiarViewModel(pomiar)
                        : new PryzmaPomiarViewModel
                        {
                            PryzmaId = pryzmaRecord.Id,
                            Typ = typ,
                            PryzmaName = pryzmaRecord.Nazwa,
                            TypName = typ.ToDescription()
                        });
                }

                var stanLicznikow = pryzmaPomiarRepository.GetByPryzmaAndTyp(pryzmaRecord.Id, viewModel.Record.Id, PryzmaPomiarTyp.StanLicznikow);
                viewModel.StanyLicznikow.Add(stanLicznikow != null
                    ? new PryzmaPomiarViewModel(stanLicznikow)
                    : new PryzmaPomiarViewModel
                    {
                        PryzmaId = pryzmaRecord.Id,
                        Typ = PryzmaPomiarTyp.StanLicznikow,
                        PryzmaName = pryzmaRecord.Nazwa,
                        TypName = PryzmaPomiarTyp.StanLicznikow.ToDescription()
                    });

                var przeplyw = pryzmaPomiarRepository.GetByPryzmaAndTyp(pryzmaRecord.Id, viewModel.Record.Id, PryzmaPomiarTyp.Przeplyw);
                viewModel.Przeplywy.Add(przeplyw != null
                    ? new PryzmaPomiarViewModel(przeplyw)
                    : new PryzmaPomiarViewModel
                    {
                        PryzmaId = pryzmaRecord.Id,
                        Typ = PryzmaPomiarTyp.Przeplyw,
                        PryzmaName = pryzmaRecord.Nazwa,
                        TypName = PryzmaPomiarTyp.Przeplyw.ToDescription()
                    });
            }

            return viewModel;
        }

        public ActionResult Delete(int id)
        {
            var doswiadczeniePomiar = doswiadczeniePomiarRepository.Get(id);

            if (!doswiadczeniePomiar.Doswiadczenie.Started)
            {
                Notifier.AddError("Nie można modyfikować pomiarów nie wystartowanego doświadczenia. Rozpocznij doświadczenie, żeby modyfikować.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var doswiadczenieId = doswiadczeniePomiar.Doswiadczenie.Id;

            if (doswiadczeniePomiar.Dzien != doswiadczeniePomiarRepository.Query().Max(p => p.Dzien))
                throw new Exception("Można usuwać tylko ostatni dzień pomiarów");

            foreach (var pryzmaPomiarRecord in doswiadczeniePomiar.Pomiary)
                pryzmaPomiarRepository.Remove(pryzmaPomiarRecord);
            doswiadczeniePomiarRepository.Remove(doswiadczeniePomiar);

            return RedirectToAction("Index", new { id = doswiadczenieId });
        }
    }
}