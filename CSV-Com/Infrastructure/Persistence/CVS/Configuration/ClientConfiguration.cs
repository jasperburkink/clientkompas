using Domain.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<Client>());
        }

        public void Configure(EntityTypeBuilder<Client> builder)
        {
            // NOTE: Don't use keys configurations etc. which you can solve with EF conventions. https://youtu.be/dK4Yb6-LxAk?t=2210

            /*
             * TODO: Maybe these foreign keys can be solved with conventions. Also maybe cascade delete is not wanted.
             * But it is needed in the current situation, because there are no repositories for diagnoses, driverslicences, etc.
             */
            builder.HasMany(c => c.Diagnoses)
                    .WithMany(dl => dl.Clients);

            builder.HasOne(c => c.MaritalStatus)
               .WithMany(d => d.Clients)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired(false);

            builder.HasMany(c => c.DriversLicences)
                .WithMany(dl => dl.Clients);

            builder.HasMany(c => c.BenefitForms)
                .WithMany(d => d.Clients)
                .UsingEntity(join => join.ToTable("ClientBenefitForm"));

            builder.HasMany(c => c.EmergencyPeople)
                .WithOne(ep => ep.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.WorkingContracts)
                .WithOne(wc => wc.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.FirstName)
                .HasMaxLength(ClientConstants.ClientFirstNameMaxLength)
                .IsRequired();

            builder.Property(c => c.PrefixLastName)
                .HasMaxLength(ClientConstants.ClientPrefixLastNameMaxLength)
                .IsRequired(false);

            builder.Property(c => c.LastName)
                .HasMaxLength(ClientConstants.ClientLastNameMaxLength)
                .IsRequired();

            builder.Property(c => c.Initials)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(c => c.FullName)
                .HasMaxLength(120);

            builder.HasIndex(c => c.FullName)
                .IsFullText();

            builder.OwnsOne(c => c.Address, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("StreetName")
                .HasMaxLength(100)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(7)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("HouseNumber")
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("HouseNumberAddition")
                .HasMaxLength(10)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("Residence")
                .HasMaxLength(100)
                .IsRequired();
            });

            builder.Property(c => c.TelephoneNumber)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(c => c.EmailAddress)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(c => c.DateOfBirth)
                .IsRequired();

            builder.Property(c => c.Remarks)
                .IsRequired(false);
        }
    }
}
