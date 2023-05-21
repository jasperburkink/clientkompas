using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Gebruiker
    {
        [Key]
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
