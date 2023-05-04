using AuthenticationPoC.Models;
using CVSModelPoC;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationPoC.ViewModels
{
    public class GebruikerViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Gebruikersnaam { get; set; }

        [Required]
        public string Voornaam { get; set; }

        [Required]
        public string Achternaam { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Wachtwoord { get; set; }
    }
}
