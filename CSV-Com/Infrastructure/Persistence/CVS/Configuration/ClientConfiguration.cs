using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            // NOTE: Don't use keys configurations etc. which you can solve with EF conventions. https://youtu.be/dK4Yb6-LxAk?t=2210

            /*
             * TODO: Maybe these foreign keys can be solved with conventions. Also maybe cascade delete is not wanted.
             * But it is needed in the current situation, because there are no repositories for diagnoses, driverslicences, etc.
             */
            builder.HasMany(c => c.Diagnoses)
                .WithOne(d => d.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.DriversLicences)
                .WithOne(dl => dl.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.BenefitForm)
                .WithMany(d => d.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.EmergencyPeople)
                .WithOne(ep => ep.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.WorkingContracts)
                .WithOne(wc => wc.Client)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.PrefixLastName)
                .HasMaxLength(10);

            builder.Property(c => c.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.StreetName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Residence)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
