using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.CVS.Configuration
{
    public interface ICVSEntityTypeConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}
