using System;
using Microsoft.EntityFrameworkCore;
using EFTraining.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EFTraining.Data
{
    public class DigiBookDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DigiBookDbContext(DbContextOptions<DigiBookDbContext> options)
                    : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            #region Appointments
            modelBuilder.Entity<Appointment>()
                .ToTable("Appointment");
            modelBuilder.Entity<Appointment>()
                .Property( a => a.AppointmentId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Host)
                .WithMany( e=> e.Appointments);
            modelBuilder.Entity<Appointment>()
                .HasOne( e => e.Guest)
                .WithMany( g => g.Appointments);
            
            #endregion

            #region Employees
            modelBuilder.Entity<Employee>()
                .ToTable("Employee");
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeId)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Employee>()
                .HasMany( e => e.Appointments)
                .WithOne( a => a.Host);
            
            #endregion
            
             #region Contacts
            modelBuilder.Entity<Contact>()
                .ToTable("Contact");
            modelBuilder.Entity<Contact>()
                .Property(c => c.ContactId )
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Contact>()
                .HasMany( c => c.Appointments)
                .WithOne( a => a.Guest);
            
            #endregion

            #region Identity
         
            #endregion
        }

    }
}