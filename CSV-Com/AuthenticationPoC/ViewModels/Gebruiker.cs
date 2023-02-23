using System.ComponentModel.DataAnnotations;

namespace AuthenticationPoC.ViewModels
{
    public class Gebruiker
    {
        [Required]
        public string Gebruikersnaam { get; set; }

        //[Required]
        //public string Voornaam { get; set; }

        //[Required]
        //public string Voorletters { get; set; }

        //public string Tussenvoegsel { get; set; }

        //[Required]
        //public string Achternaam { get; set; }

        //public string Telefoon { get; set; }

        //public string Mobiel { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public string Wachtwoord { get; set; }
    }
}
