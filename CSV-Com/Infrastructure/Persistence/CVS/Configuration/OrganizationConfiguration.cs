using Domain.Constants;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>, ICVSEntityTypeConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            Configure(modelBuilder.Entity<Organization>());
        }

        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.Property(c => c.OrganizationName)
                .HasMaxLength(OrganizationConstants.OrganizationNameMaxLength)
                .IsRequired();

            builder.HasIndex(c => c.OrganizationName)
                .IsFullText();

            builder.OwnsOne(c => c.VisitAddress, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("VisitStreetName")
                .HasMaxLength(OrganizationConstants.StreetNameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("VisitPostalCode")
                .HasMaxLength(OrganizationConstants.PostalCodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("VisitHouseNumber")
                .HasMaxLength(OrganizationConstants.HouseNumberMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("VisitHouseNumberAddition")
                .HasMaxLength(OrganizationConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("VisitResidence")
                .HasMaxLength(OrganizationConstants.ResidenceMaxLength)
                .IsRequired();
            });

            builder.OwnsOne(c => c.InvoiceAddress, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("InvoiceStreetName")
                .HasMaxLength(OrganizationConstants.StreetNameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("InvoicePostalCode")
                .HasMaxLength(OrganizationConstants.PostalCodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("InvoiceHouseNumber")
                .HasMaxLength(OrganizationConstants.HouseNumberMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("InvoiceHouseNumberAddition")
                .HasMaxLength(OrganizationConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("InvoiceResidence")
                .HasMaxLength(OrganizationConstants.ResidenceMaxLength)
                .IsRequired();
            });

            builder.OwnsOne(c => c.PostAddress, a =>
            {
                a.Property(p => p.StreetName)
                .HasColumnName("PostStreetName")
                .HasMaxLength(OrganizationConstants.StreetNameMaxLength)
                .IsRequired();

                a.Property(p => p.PostalCode)
                .HasColumnName("PostPostalCode")
                .HasMaxLength(OrganizationConstants.PostalCodeMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumber)
                .HasColumnName("PostHouseNumber")
                .HasMaxLength(OrganizationConstants.HouseNumberMaxLength)
                .IsRequired();

                a.Property(p => p.HouseNumberAddition)
                .HasColumnName("PostHouseNumberAddition")
                .HasMaxLength(OrganizationConstants.HouseNumberAdditionMaxLength)
                .IsRequired(false);

                a.Property(p => p.Residence)
                .HasColumnName("PostResidence")
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

            builder.HasIndex(c => c.KVKNumber)
                .IsUnique();

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
