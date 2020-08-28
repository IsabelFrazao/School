using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //Check if the Database is created

            //if(!_context.Students.Any())
            //{
            //    this.AddStudents("Isabel");
            //    this.AddStudents("José");
            //    this.AddStudents("António");
            //    await _context.SaveChangesAsync();
            //}

            if (!_context.Teachers.Any())
            {
                this.AddTeachers("Not Selected");

                await _context.SaveChangesAsync();
            }

            if (!_context.Courses.Any())
            {
                this.AddCourses("Not Selected");
                
                await _context.SaveChangesAsync();
            }            
        }

        //private void AddStudents(string name)
        //{
        //    _context.Students.Add(new Student
        //    {
        //        FullName = "Isabel Frazão",
        //    });
        //}

        private void AddCourses(string name)
        {
            _context.Courses.Add(new Course
            {
                Name = "Not Selected",
                CoordinatorId = 1
            });
        }

        private void AddTeachers(string name)
        {
            _context.Teachers.Add(new Teacher
            {
                FullName = "Not Selected"
            });
        }
    }
}
