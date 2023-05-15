using System.ComponentModel.DataAnnotations;

namespace CVSModelPoC
{
    public class Gebruiker
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Voornaam { get; set; }

        //[Required]
        //public string Voorletters { get; set; }

        //public string Tussenvoegsel { get; set; }

        [Required]
        public string Achternaam { get; set; }

        [Required]
        public string Email { get; set; }

        //public static implicit operator Gebruiker(void v)
        //{
        //    throw new NotImplementedException();
        //}

        //public string Telefoon { get; set; }

        //public string Mobiel { get; set; }
    }
}