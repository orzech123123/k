using System.Web.Mvc;
using Kompostowanie.Helpers;
using Kompostowanie.Records;
using Kompostowanie.ViewModels;

namespace Kompostowanie.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository userRepository;

        public UserController()
        {
            userRepository = new UserRepository(UnitOfWork);
        }

        public UserSession UserSession
        {
            get { return (UserSession) Session["UserSession"];  }
            set { Session["UserSession"] = value; }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (SessionHelper.UserSession(Session) == null)
                return View(new UserViewModel());
            
            return RedirectToAction("Index", "Doswiadczenie");
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var viewModel = new UserViewModel();
            TryUpdateModel(viewModel);

            LoginExtraValidation(viewModel);

            if (ModelState.IsValid)
            {
                var userRecord = userRepository.GetByUsername(viewModel.Username);
                SessionHelper.SetUserSession(Session, new UserSession { UserId = userRecord.Id, Username = userRecord.Username });

                return RedirectToAction("Index", "Doswiadczenie");
            }

            return View(viewModel);
        }

        private void LoginExtraValidation(UserViewModel viewModel)
        {
            var user = userRepository.GetByUsername(viewModel.Username);

            if(user == null || user.Password != viewModel.Password)
                ModelState.AddModelError("Username", "Nie udało się zalogować. Sprawdź wprowadzane dane");
        }

        private void RegisterExtraValidation(UserViewModel viewModel)
        {
            if (userRepository.GetByUsername(viewModel.Username) != null)
                ModelState.AddModelError("Username", "Istnieje już użytkownik o takim loginie.");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Register(FormCollection collection)
        {
            var viewModel = new UserViewModel();
            TryUpdateModel(viewModel);

            RegisterExtraValidation(viewModel);

            if (ModelState.IsValid)
            {
                var userRecord = new UserRecord
                {
                    Username = viewModel.Username,
                    Password = viewModel.Password
                };

                userRepository.Add(userRecord);

                SessionHelper.SetUserSession(Session, null);
                
                return RedirectToAction("Login");
            }

            return View(viewModel);
        }

        public ActionResult Logout()
        {
            if(SessionHelper.UserSession(Session) == null)
                return RedirectToAction("Index", "Doswiadczenie");

            SessionHelper.SetUserSession(Session, null);

            return View("Login");
        }
    }
}