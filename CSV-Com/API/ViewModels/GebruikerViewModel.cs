using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class GebruikerViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Voornaam { get; set; }

        [Required]
        public string Voorletters { get; set; }

        public string Tussenvoegsel { get; set; }

        [Required]
        public string Achternaam { get; set; }

        [Required]
        public string Email { get; set; }

        public string Telefoon { get; set; }

        public string Mobiel { get; set; }
    }
}
