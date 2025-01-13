using EntityLayer.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public partial class GlamOfficeDbContext : DbContext
    {
        public GlamOfficeDbContext()
            : base("name=GlamOfficeDB")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<DailySchedule> DailySchedules { get; set; }
        public virtual DbSet<Day> Days { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<GiftCard> GiftCards { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Reservation_has_Treatment> Reservation_has_Treatment { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<RewardPoint> RewardPoints { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Treatment> Treatments { get; set; }
        public virtual DbSet<TreatmentGroup> TreatmentGroups { get; set; }
        public virtual DbSet<WeeklySchedule> WeeklySchedules { get; set; }
        public virtual DbSet<WorkPosition> WorkPositions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(e => e.Firstname)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Lastname)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.Client)
                .HasForeignKey(e => e.Client_idClient);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Reviews)
                .WithOptional(e => e.Client)
                .HasForeignKey(e => e.Client_idClient);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.RewardPoints)
                .WithRequired(e => e.Client)
                .HasForeignKey(e => e.Client_idClient)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Day>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Day>()
                .HasMany(e => e.DailySchedules)
                .WithRequired(e => e.Day)
                .HasForeignKey(e => e.Day_idDay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Day>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.Day)
                .HasForeignKey(e => e.Day_idDay);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PIN)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Firstname)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Lastname)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.DailySchedules)
                .WithRequired(e => e.Employee)
                .HasForeignKey(e => e.Employee_idEmployee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Reservations)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_idEmployee);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Reviews)
                .WithOptional(e => e.Employee)
                .HasForeignKey(e => e.Employee_idEmployee);

            modelBuilder.Entity<GiftCard>()
                .Property(e => e.Value)
                .HasPrecision(10, 2);

            modelBuilder.Entity<GiftCard>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<GiftCard>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<GiftCard>()
                .Property(e => e.PromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<Receipt>()
                .Property(e => e.ReceiptNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Receipt>()
                .Property(e => e.Balance)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Reservation>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<Reservation>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.Receipts)
                .WithRequired(e => e.Reservation)
                .HasForeignKey(e => e.Reservation_idReservation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.Reservation_has_Treatment)
                .WithRequired(e => e.Reservation)
                .HasForeignKey(e => e.Reservation_idReservation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                .HasMany(e => e.Reviews)
                .WithOptional(e => e.Reservation)
                .HasForeignKey(e => e.Reservation_idReservation);

            modelBuilder.Entity<Review>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Reward>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Reward>()
                .Property(e => e.Threshold)
                .IsUnicode(false);

            modelBuilder.Entity<Reward>()
                .HasMany(e => e.RewardPoints)
                .WithRequired(e => e.Reward)
                .HasForeignKey(e => e.Reward_idReward)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RewardPoint>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Treatment>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Treatment>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Treatment>()
                .Property(e => e.DurationMinutes)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Treatment>()
                .HasMany(e => e.Reservation_has_Treatment)
                .WithRequired(e => e.Treatment)
                .HasForeignKey(e => e.Treatment_idTreatment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Treatment>()
                .HasMany(e => e.Reviews)
                .WithOptional(e => e.Treatment)
                .HasForeignKey(e => e.Treatment_idTreatment);

            modelBuilder.Entity<TreatmentGroup>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<WeeklySchedule>()
                .HasMany(e => e.Days)
                .WithOptional(e => e.WeeklySchedule)
                .HasForeignKey(e => e.WeeklySchedule_idWeeklySchedule);

            modelBuilder.Entity<WorkPosition>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<WorkPosition>()
                .Property(e => e.Description)
                .IsUnicode(false);
        }
    }
}
