﻿using Microsoft.EntityFrameworkCore;
using School.Web.Data.Entities;
using System.Linq;
using Class = School.Web.Data.Entities.Class;
using Course = School.Web.Data.Entities.Course;
using Subject = School.Web.Data.Entities.Subject;
using Teacher = School.Web.Data.Entities.Teacher;

namespace School.Web.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<IEFPSubject> IEFPSubjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }

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


            base.OnModelCreating(modelBuilder);
        }
    }
}
