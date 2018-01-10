
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.IdentityModel.Protocols;

namespace EFTraining.Data
{
        public class DigiBookDbContextFactory : IDesignTimeDbContextFactory<DigiBookDbContext>
    {
        public DigiBookDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DigiBookDbContext>();
             
            optionsBuilder.UseSqlServer(connectionString: "Server=tcp:tmgsql01.database.windows.net,1433;Initial Catalog=DigiBookDb;Persist Security Info=False;User ID=tmgsql01sa;Password=Bl@st123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new DigiBookDbContext(optionsBuilder.Options);
        }
    }
}