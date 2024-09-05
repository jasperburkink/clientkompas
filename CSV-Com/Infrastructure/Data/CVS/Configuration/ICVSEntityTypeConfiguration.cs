using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.CVS.Configuration
{
    public interface ICVSEntityTypeConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
