﻿using CVSModelPoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVSInfrastructurePoC.Repositories
{
    public interface IGebruikerRepository : IDisposable
    {
        Task<List<Gebruiker>> GetGebruikersAsync();

        Task InsertGebruikerAsync(Gebruiker gebruiker);

        Task SaveAsync();

        Task<Gebruiker> GetGebruikerByEmailAsync(string email);

        Task<Gebruiker> GetGebruikerAsync(int id);

        Task UpdateGebruikerAsync(Gebruiker gebruiker);
    }
}
