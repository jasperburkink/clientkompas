using Domain.CVS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.CVS
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Gebruiker> GebruikerRepository { get; }

        IRepository<Cliënt> CliëntRepository { get; }

        public void Save();
    }
}
