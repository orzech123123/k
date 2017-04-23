using System.ComponentModel.DataAnnotations;

namespace Kompostowanie.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Pole nazwa użytkownika jest wymagane.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Pole Hasło jest wymagane.")]
        public string Password { get; set; }
    }
}