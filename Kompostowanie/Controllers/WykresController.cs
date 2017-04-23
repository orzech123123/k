using System.Linq;
using System.Web.Mvc;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class WykresController : AuthorizedController
    {
        private readonly IDoswiadczenieRepository doswiadczenieRepository;
        private readonly IPryzmaPomiarRepository pryzmaPomiarRepository;

        public WykresController()
        {
            doswiadczenieRepository = new DoswiadczenieRepository(UnitOfWork);
            pryzmaPomiarRepository = new PryzmaPomiarRepository(UnitOfWork);
        }

        private bool CanDraw(DoswiadczenieRecord doswiadczenie)
        {
            return doswiadczenie.Pomiary.Any();
        }

        public ActionResult Co2(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!CanDraw(doswiadczenieRecord))
            {
                Notifier.AddError("Brak możliwości rysowania wykresu - nie dodano żadnego pomiaru.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var viewModel = new WykresViewModel
            {
                LabelY = "Dwutlenek węgla [%]",
                Doswiadczenie = doswiadczenieRecord
            };

            FillData(viewModel, PryzmaPomiarTyp.Co2);

            return View("Wykres", viewModel);
        }

        public ActionResult Temperatura(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!CanDraw(doswiadczenieRecord))
            {
                Notifier.AddError("Brak możliwości rysowania wykresu - nie dodano żadnego pomiaru.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var viewModel = new WykresViewModel
            {
                LabelY = "Temperatura [C]",
                Doswiadczenie = doswiadczenieRecord
            };

            FillData(viewModel, PryzmaPomiarTyp.Temperatura);

            return View("Wykres", viewModel);
        }

        public ActionResult Przeplyw(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!CanDraw(doswiadczenieRecord))
            {
                Notifier.AddError("Brak możliwości rysowania wykresu - nie dodano żadnego pomiaru.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var viewModel = new WykresViewModel
            {
                LabelY = "Przepływ [l/min]",
                Doswiadczenie = doswiadczenieRecord
            };

            FillData(viewModel, PryzmaPomiarTyp.Przeplyw);

            return View("Wykres", viewModel);
        }


        public ActionResult O2(int id)
        {
            var doswiadczenieRecord = doswiadczenieRepository.Get(id);

            if (!CanDraw(doswiadczenieRecord))
            {
                Notifier.AddError("Brak możliwości rysowania wykresu - nie dodano żadnego pomiaru.");
                return RedirectToAction("Index", "Doswiadczenie");
            }

            var viewModel = new WykresViewModel
            {
                LabelY = "Stężenie tlenu [dm3]",
                Doswiadczenie = doswiadczenieRecord
            };

            FillData(viewModel, PryzmaPomiarTyp.O2);

            return View("Wykres", viewModel);
        }

        private void FillData(WykresViewModel viewModel, PryzmaPomiarTyp typ)
        {
            var pomiary = pryzmaPomiarRepository.QueryByDoswiadczenieAndTyp(viewModel.Doswiadczenie.Id, typ).ToList();

            var pomiaryByPryzma = pomiary.GroupBy(p => new { p.Pryzma.Id, p.Pryzma.Nazwa } ).ToList();

            viewModel.Pryzmy = pomiaryByPryzma.Select(pryzma => new PryzmaWykresViewModel
            {
                Pryzma = pryzma.Key.Nazwa,
                Pomiary = pryzma.Select(p => new PomiarWykresViewModel
                {
                    Dzien = p.DoswiadczeniePomiar.Dzien,
                    Value = p.Value
                })
                .OrderBy(p => p.Dzien)
                .ToList()
            })
            .OrderBy(p => p.Pryzma)
            .ToList();
        }
    }
}