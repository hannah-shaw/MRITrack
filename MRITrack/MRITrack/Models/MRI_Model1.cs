using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MRITrack.Models
{
    public partial class MRI_Model1 : DbContext
    {
        public MRI_Model1()
            : base("name=MRI_Model1")
        {
        }

        public virtual DbSet<Appointments> Appointments { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointments>()
                .HasMany(e => e.Comments)
                .WithRequired(e => e.Appointments)
                .HasForeignKey(e => e.AppointmentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Doctors>()
                .HasMany(e => e.Appointments)
                .WithRequired(e => e.Doctors)
                .HasForeignKey(e => e.DoctorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Appointments)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
