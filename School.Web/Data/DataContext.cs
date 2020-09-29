using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System.Linq;

namespace School.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<IEFPSubject> IEFPSubjects { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Field> Fields { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Subject>().Property(p => p.Professor).HasColumnType("decimal (18,2)");

            //Cascading Delete Rule

            var cascadeFKs = modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //UNIQUE

            modelBuilder.Entity<Student>().HasIndex(s => s.IdentificationNumber).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.TaxNumber).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.SSNumber).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.NHSNumber).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.Telephone).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.Email).IsUnique();

            modelBuilder.Entity<Teacher>().HasIndex(t => t.IdentificationNumber).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.TaxNumber).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.SSNumber).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.NHSNumber).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.Telephone).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(t => t.Email).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
