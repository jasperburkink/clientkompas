using AuthenticationPoC.Models;
using CVSModelPoC;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationPoC.ViewModels
{
    public class GebruikerViewModel : Gebruiker
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
    }
}
