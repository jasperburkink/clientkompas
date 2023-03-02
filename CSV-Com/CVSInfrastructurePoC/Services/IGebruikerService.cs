using CVSModelPoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVSInfrastructurePoC.Services
{
    public interface IGebruikerService
    {
        public List<Gebruiker> GetGebruikers();
    }
}
