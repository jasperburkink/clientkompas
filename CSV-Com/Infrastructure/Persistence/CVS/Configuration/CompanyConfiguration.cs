using Domain.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Organization>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<Organization>());
        }

        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.HasMany(c => c.WorkingContracts)
                .WithOne(wc => wc.CompanyName)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.CompanyName)
                .HasMaxLength(OrganizationConstants.CompanyNameMaxLength)
                .IsRequired();

            builder.OwnsOne(c => c.VisitAddress, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("StreetName")
                .HasMaxLength(OrganizationConstants.StreetNameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(OrganizationConstants.PostalCodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("HouseNumber")
                .HasMaxLength(OrganizationConstants.HouseNumberMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("HouseNumberAddition")
                .HasMaxLength(OrganizationConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("Residence")
                .HasMaxLength(OrganizationConstants.ResidenceMaxLength)
                .IsRequired();
            });

            builder.OwnsOne(c => c.InvoiceAddress, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("StreetName")
                .HasMaxLength(OrganizationConstants.StreetNameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(OrganizationConstants.PostalCodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("HouseNumber")
                .HasMaxLength(OrganizationConstants.HouseNumberMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("HouseNumberAddition")
                .HasMaxLength(OrganizationConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("Residence")
                .HasMaxLength(OrganizationConstants.ResidenceMaxLength)
                .IsRequired();
            });

            builder.OwnsOne(c => c.PostAddress, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("StreetName")
                .HasMaxLength(OrganizationConstants.StreetNameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("PostalCode")
                .HasMaxLength(OrganizationConstants.PostalCodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("HouseNumber")
                .HasMaxLength(OrganizationConstants.HouseNumberMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("HouseNumberAddition")
                .HasMaxLength(OrganizationConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("Residence")
                .HasMaxLength(OrganizationConstants.ResidenceMaxLength)
                .IsRequired();
            });

            builder.Property(c => c.ContactPersonName)
                .HasMaxLength(OrganizationConstants.ContactPersonNameMaxLength)
                .IsRequired();

            builder.Property(c => c.ContactPersonFunction)
                .HasMaxLength(OrganizationConstants.ContactPersonFunctionMaxLength)
                .IsRequired();

            builder.Property(c => c.ContactPersonTelephoneNumber)
                .HasMaxLength(OrganizationConstants.ContactPersonTelephoneNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.ContactPersonMobilephoneNumber)
                .HasMaxLength(OrganizationConstants.ContactPersonMobilephoneNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.ContactPersonEmailAddress)
                .HasMaxLength(OrganizationConstants.ContactPersonEmailAddressMaxLength)
                .IsRequired();

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(OrganizationConstants.PhoneNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.Website)
                .HasMaxLength(OrganizationConstants.WebsiteMaxLength)
                .IsRequired();

            builder.Property(c => c.EmailAddress)
                .HasMaxLength(OrganizationConstants.EmailAddressMaxLength)
                .IsRequired();

            builder.Property(c => c.KVKNumber)
                .HasMaxLength(OrganizationConstants.KVKNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.BTWNumber)
                .HasMaxLength(OrganizationConstants.BTWNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.IBANNumber)
                .HasMaxLength(OrganizationConstants.IBANNumberMaxLength)
                .IsRequired();

            builder.Property(c => c.BIC)
                .HasMaxLength(OrganizationConstants.BICMaxLength)
                .IsRequired();
        }
    }
}
