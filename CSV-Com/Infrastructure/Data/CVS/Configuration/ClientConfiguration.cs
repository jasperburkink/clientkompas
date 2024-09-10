using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.CVS.Configuration
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

            builder.HasMany(c => c.WorkingContracts)
                .WithOne(wc => wc.Client)
                .HasForeignKey(wc => wc.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.FirstName)
                .HasMaxLength(ClientConstants.FirstNameMaxLength)
                .IsRequired();

            builder.Property(c => c.PrefixLastName)
                .HasMaxLength(ClientConstants.PrefixLastNameMaxLength)
                .IsRequired(false);

            builder.Property(c => c.LastName)
                .HasMaxLength(ClientConstants.LastNameMaxLength)
                .IsRequired();

            builder.Property(c => c.Initials)
                .HasMaxLength(ClientConstants.InitialsMaxLength)
                .IsRequired();

            builder.Property(c => c.FullName)
                .HasMaxLength(ClientConstants.FullnameMaxLength);

            builder.HasIndex(c => c.FullName)
                .IsFullText();

            builder.OwnsOne(c => c.Address, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("StreetName")
                .HasMaxLength(AddressConstants.StreetnameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(AddressConstants.PostalcodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("HouseNumber")
                .HasMaxLength(AddressConstants.HouseNumberMaxValue)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("HouseNumberAddition")
                .HasMaxLength(AddressConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("Residence")
                .HasMaxLength(AddressConstants.ResidenceMaxLength)
                .IsRequired();
            });

            builder.Property(c => c.TelephoneNumber)
                .HasMaxLength(ClientConstants.TelephoneNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.EmailAddress)
                .HasMaxLength(ClientConstants.EmailAddressMaxLength)
                .IsRequired();

            builder.Property(c => c.DateOfBirth)
                .IsRequired();

            builder.Property(c => c.Remarks)
                .HasMaxLength(ClientConstants.RemarksMaxLength)
                .IsRequired(false);

            builder.OwnsMany(c => c.EmergencyPeople, a =>
            {
                a.Property(e => e.Name)
                 .HasColumnName("Name")
                 .HasMaxLength(EmergencyPersonConstants.NameMaxLength)
                 .IsRequired();

                a.Property(e => e.TelephoneNumber)
                .HasColumnName("TelephoneNumber")
                .HasMaxLength(EmergencyPersonConstants.TelephoneNumberMaxLength)
                .IsRequired();
            });
        }
    }
}
