using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CVS.Domain
{
    public class Client
    {
        public int ClientId { get; set; }

        public int BSNNummer { get; set; }

        public string Roepnaam { get; set; }

        public string Voorletters { get; set; }

        public string Tussenvoegsel { get; set; }

        public string Achternaam { get; set; }

        public string StraatNaam { get; set; }

        public int Huisnummer { get; set; }

        public string HuisnummerToevoeging { get; set; }

        public string Postcode { get; set; }

        public string Woonplaats { get; set; }

        public string Telefoonnummer { get; set; }

        public string Mobielnummer { get; set; }

        public string Emailadres { get; set; }                
    }
}
