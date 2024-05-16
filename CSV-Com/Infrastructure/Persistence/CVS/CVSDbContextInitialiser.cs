using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
            BenefitForm benefitForm1, benefitForm2;
            int benefitForm1Id = 1, benefitForm2Id = 2;

            if (!_context.BenefitForm.Any())
            {
                benefitForm1 = new BenefitForm
                {
                    Id = 1,
                    Name = "Bijstand"
                };
                _context.BenefitForm.Add(benefitForm1);

                benefitForm2 = new BenefitForm
                {
                    Id = 2,
                    Name = "WIA"
                };
                _context.BenefitForm.Add(benefitForm2);

                await _context.SaveChangesAsync();
            }
            else
            {
                benefitForm1 = _context.BenefitForm.First(ms => ms.Id.Equals(benefitForm1Id));
                benefitForm2 = _context.BenefitForm.First(ms => ms.Id.Equals(benefitForm2Id));
            }

            MaritalStatus martitalStatus;
            var maritalStatusId = 1;

            if (!_context.MaritalStatus.Any(ms => ms.Id.Equals(maritalStatusId)))
            {
                martitalStatus = new MaritalStatus
                {
                    Id = maritalStatusId,
                    Name = "Unmarried"
                };

                _context.MaritalStatus.Add(martitalStatus);
                await _context.SaveChangesAsync();
            }
            else
            {
                martitalStatus = _context.MaritalStatus.First(ms => ms.Id.Equals(maritalStatusId));
            }

            if (!_context.Clients.Any())
            {
                _context.Clients.Add(new Client
                {
                    Id = 1,
                    FirstName = "Jan",
                    Initials = "J",
                    PrefixLastName = "",
                    LastName = "Jansen",
                    Gender = Gender.Man,
                    Address = Address.From("Dorpstraat", 1, "a", "1234AB", "Amsterdam"),
                    TelephoneNumber = "0623456789",
                    DateOfBirth = new DateOnly(1990, 5, 14),
                    EmailAddress = "a@b.com",
                    MaritalStatus = martitalStatus,
                    DriversLicences =
                    {
                        new DriversLicence
                        {
                            Id = 1,
                            Category = "B",
                            Description = "Auto"
                        },
                          new DriversLicence
                        {
                            Id = 2,
                            Category = "A",
                            Description = "Motor"
                        }
                    },
                    Diagnoses =
                    {
                        new Diagnosis
                        {
                            Id = 1,
                            Name = "Dyslexia"
                        },
                        new Diagnosis
                        {
                            Id = 2,
                            Name = "Depression"
                        }
                    },
                    EmergencyPeople =
                    {
                        new EmergencyPerson
                        {
                            Id = 1,
                            Name = "Piet Pietersen",
                            TelephoneNumber = "0123456789"
                        },
                        new EmergencyPerson
                        {
                            Id = 2,
                            Name = "Frans Duits",
                            TelephoneNumber = "098765321"
                        }
                    },
                    BenefitForms =
                    {
                        benefitForm1
                    },
                    WorkingContracts =
                    {
                        new WorkingContract
                        {
                            Id = 1,
                            CompanyName = new Organization
                            {
                                CompanyName = "SBICT",
                            },
                            Function = "Directeur",
                            ContractType = ContractType.Temporary,
                            FromDate = new DateOnly(2023, 1, 1),
                            ToDate = new DateOnly(2024, 1, 1)
                        },
                        new WorkingContract
                        {
                            Id = 2,
                            CompanyName = new Organization
                            {
                            CompanyName = "De Nederlandse regering",
                            },
                            Function = "Minister president",
                            ContractType = ContractType.Permanent,
                            FromDate = new DateOnly(2010, 1, 1),
                            ToDate = new DateOnly(2023, 1, 1)
                        }
                    },
                    Remarks = "Jan is een geweldig persoon om mee samen te werken."
                });

                _context.Clients.Add(new Client
                {
                    Id = 2,
                    FirstName = "Prince-Fritz-Cruene-August-Willem-Jan-Hendrik-Dick",
                    Initials = "P.F.C.A.W.J.H.D",
                    PrefixLastName = "van den",
                    LastName = "Heuvel tot Beichlingen, gezegd Bartolotti Rijnders",
                    Gender = Gender.NonBinary,
                    Address = Address.From("Kerkstraat", 2, "", "1234AB", "Rotterdam"),
                    TelephoneNumber = "0623456789",
                    DateOfBirth = new DateOnly(1950, 12, 1),
                    EmailAddress = "b@a.com",
                    MaritalStatus = martitalStatus,
                    EmergencyPeople =
                    {
                        new EmergencyPerson
                        {
                            Id = 3,
                            Name = "Jan Pietersen",
                            TelephoneNumber = "0123456789"
                        }
                    },
                    BenefitForms =
                    {
                        benefitForm1,
                        benefitForm2
                    },
                    WorkingContracts =
                    {
                        new WorkingContract
                        {
                            Id = 3,
                            CompanyName = new Organization
                            {
                            CompanyName = "De Nederlandse regering",
                            },
                            Function = "Minister president",
                            ContractType = ContractType.Permanent,
                            FromDate = new DateOnly(1980, 1, 1),
                            ToDate = new DateOnly(2000, 1, 1)
                        }
                    },
                    Remarks = "Deze persoon heeft wel een heel erg lange naam."
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
