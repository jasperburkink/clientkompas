using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.CVS
{
    public class CVSDbContextInitialiser
    {
        private readonly ILogger<CVSDbContextInitialiser> _logger;
        private readonly CVSDbContext _context;

        public CVSDbContextInitialiser(ILogger<CVSDbContextInitialiser> logger, CVSDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task InitialiseAsync()
        {
            try
            {
                if (_context.Database.IsMySql())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }

        public async Task TrySeedAsync()
        {   
            // Default data
            // Seed, if necessary
            // TODO: Maybe only when debugging
            if (!_context.Clients.Any())
            {
                _context.Clients.Add(new Client
                {
                    Id = 1,
                    IdentificationNumber = 123456789,
                    FirstName = "Jan",
                    Initials = "J",
                    PrefixLastName = "",
                    LastName = "Jansen",
                    Gender = Gender.Men,
                    StreetName = "Dorpstraat",
                    HouseNumber = 1,
                    HouseNumberAddition = "a",
                    PostalCode = "1234AB",
                    Residence = "Amsterdam",
                    TelephoneNumber = "0623456789",
                    DateOfBirth = new DateOnly(1990, 5, 14),
                    EmailAddress = "a@b.com",
                    MaritalStatus = MaritalStatus.Unmarried,
                    DriversLicences =
                    {
                        new DriversLicence
                        {
                            Id = 1,
                            DriversLicenceCode = DriversLicenceEnum.B
                        }
                    },
                    EmergencyPeople =
                    {
                        new EmergencyPerson
                        {
                            Id = 1,
                            Name = "Piet Pietersen",
                            TelephoneNumber = "0123456789"
                        }
                    },
                    Diagnoses =
                    {
                        new Diagnosis
                        {
                            Id = 1,
                            Name = "ADHD"
                        },
                        new Diagnosis
                        {
                            Id = 2,
                            Name = "Autismespectrumstoornis (ASS)"
                        }
                    },
                    //BenefitForm = BenefitForm.Bijstand,
                    WorkingContracts =
                    {
                        new WorkingContract
                        {
                            Id = 1,
                            CompanyName = "SBICT",
                            Function = "Directeur",
                            ContractType = ContractType.Temporary,
                            FromDate = new DateOnly(2023, 1, 1),
                            ToDate = new DateOnly(2024, 1, 1)
                        }
                    },
                    Remarks = "Jan is een geweldig persoon om mee samen te werken."
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
