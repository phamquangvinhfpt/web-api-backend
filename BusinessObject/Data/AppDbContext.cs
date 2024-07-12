using BusinessObject.Auditing;
using BusinessObject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObject.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicDetail> ClinicDetails { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DentalRecord> DentalRecords { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<FollowUpAppointment> FollowUpAppointments { get; set; }
        public DbSet<DentistDetail> DentistDetails { get; set; }
        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);
                optionsBuilder.UseSqlServer(ConnectionString());
            }
        }
        public string ConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build().GetConnectionString("DefaultConnection");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole<Guid>>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<Clinic>()
                .HasOne(c => c.Owner)
                .WithMany()
                .HasForeignKey(c => c.OwnerID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Clinic>()
                .HasMany(c => c.ClinicDetails)
                .WithOne(cd => cd.Clinic)
                .HasForeignKey(cd => cd.Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Dentist)
                .WithMany()
                .HasForeignKey(a => a.DentistID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Clinic)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.ClinicID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DentalRecord>()
                .HasOne(dr => dr.MedicalRecord)
                .WithOne(mr => mr.DentalRecord)
                .HasForeignKey<MedicalRecord>(mr => mr.DentalRecordId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClinicDetail>()
                .HasOne(coh => coh.Clinic)
                .WithMany(c => c.ClinicDetails)
                .HasForeignKey(coh => coh.ClinicID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Appointment>()
                .HasOne(a => a.DentalRecord)
                .WithOne(dr => dr.Appointment)
                .HasForeignKey<DentalRecord>(dr => dr.AppointmentID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MedicalRecord>()
                .HasOne(mr => mr.Appointment)
                .WithOne()
                .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<DentistDetail>()
                .HasOne(dq => dq.Dentist)
                .WithMany()
                .HasForeignKey(dq => dq.DentistId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<DentistDetail>()
                .HasOne(dq => dq.Clinic)
                .WithMany()
                .HasForeignKey(dq => dq.ClinicId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Prescription>()
                .HasOne(p => p.DentalRecord)
                .WithMany(dr => dr.Prescriptions)
                .HasForeignKey(p => p.DentalRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<AppUser>()
                .HasOne(u => u.DentistDetails)
                .WithOne(d => d.Dentist)
                .HasForeignKey<DentistDetail>(d => d.DentistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .Property(b => b.Title)
                .HasMaxLength(256);
            builder.Entity<Notification>()
                .Property(b => b.Message)
                .HasMaxLength(2048);
            builder.Entity<Notification>()
                .Property(b => b.Label)
                .HasConversion<string>()
                .HasColumnType("varchar(50)");
            builder.Entity<Notification>()
            .Property(b => b.IsRead)
            .HasDefaultValue(false);
            builder.Entity<Notification>()
                .Property(b => b.Url)
                .HasMaxLength(2048);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
        }

        // Audit
        public virtual async Task<int> SaveChangesAsync(Guid userId)
        {
            OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }

        private void OnBeforeSaveChanges(Guid userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = Enums.AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = Enums.AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.AuditType = Enums.AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }
    }
}