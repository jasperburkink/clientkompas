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
            // TODO: when running intergation tests it may crash sometime becasuse of respawn. Maybe don't use static ids.
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
                benefitForm1 = _context.BenefitForm.First();
                benefitForm2 = _context.BenefitForm.First();
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
                            Organization = new Organization
                            {
                                OrganizationName = "SBICT",
                                BIC = "12345678",
                                BTWNumber = "12",
                                IBANNumber = "13",
                                KVKNumber = "33012",
                                EmailAddress = "David@gmail.com",
                                Website = "www.sbict.nl",
                                PhoneNumber = "0316778822",
                                ContactPersonName = "David Visser",
                                ContactPersonFunction = "Stagair",
                                ContactPersonTelephoneNumber = "0633081490",
                                ContactPersonMobilephoneNumber = "0633083122",
                                ContactPersonEmailAddress = "Contact@gmail.com",
                                VisitAddress = Address.From("Nijverheidsstraat", 4, "", "6905DL", "Zevenaar"),
                                InvoiceAddress = Address.From("Nijverheidsstraat", 4, "", "6905DL", "Zevenaar"),
                                PostAddress = Address.From("Nijverheidsstraat", 4, "", "6905DL", "Zevenaar")
                            },
                            Function = "Directeur",
                            ContractType = ContractType.Temporary,
                            FromDate = new DateOnly(2023, 1, 1),
                            ToDate = new DateOnly(2024, 1, 1)
                        },
                        new WorkingContract
                        {
                            Id = 2,
                            Organization = new Organization
                            {
                                OrganizationName = "De Nederlandse regering",
                                BIC = "12345678",
                                BTWNumber = "12",
                                IBANNumber = "13",
                                KVKNumber = "15654",
                                EmailAddress = "Regering@gmail.com",
                                Website = "www.Rijksoverheid.nl",
                                PhoneNumber = "0316283661",
                                ContactPersonName = "Jan Pietje",
                                ContactPersonFunction = "Minister president",
                                ContactPersonTelephoneNumber = "0633033291",
                                ContactPersonMobilephoneNumber = "0633083333",
                                ContactPersonEmailAddress = "Contact@gmail.com",
                                VisitAddress = Address.From("Bezuidenhoutseweg", 67, "", "2233LK", "Den Haag"),
                                InvoiceAddress = Address.From("Bezuidenhoutseweg", 67, "", "2233LK", "Den Haag"),
                                PostAddress = Address.From("Bezuidenhoutseweg", 67, "", "2233LK", "Den Haag")
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
                            Organization = new Organization
                            {
                                OrganizationName = "DHL",
                                BIC = "12345678",
                                BTWNumber = "12",
                                IBANNumber = "13",
                                KVKNumber = "89246",
                                EmailAddress = "DHL@gmail.com",
                                Website = "www.Regering.nl",
                                PhoneNumber = "0316283661",
                                ContactPersonName = "Alexander de Croo",
                                ContactPersonFunction = "Magazijn medewerker",
                                ContactPersonTelephoneNumber = "0633081496",
                                ContactPersonMobilephoneNumber = "0633081496",
                                ContactPersonEmailAddress = "Contact@gmail.com",
                                VisitAddress = Address.From("Dorpstraat", 1, "b", "2233LK", "Rotterdam"),
                                InvoiceAddress = Address.From("Rotterdamseweg", 3, "AB", "2299KJ", "Rotterdamn"),
                                PostAddress = Address.From("Steenstraat", 33, "h", "3311ff", "Hilversum")
                            },
                            Function = "Magazijn medewerker",
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
