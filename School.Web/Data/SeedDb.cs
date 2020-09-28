using Microsoft.AspNetCore.Identity;
using School.Web.Data.Entities;
using School.Web.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace School.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Teacher");
            await _userHelper.CheckRoleAsync("Student");

            var user = await _userHelper.GetUserByEmailAsync("maria_frazao@hotmail.com");

            if (user == null)
            {
                user = new User
                {
                    FirstName = "Isabel",
                    LastName = "Frazão",
                    Email = "maria_frazao@hotmail.com",
                    UserName = "maria_frazao@hotmail.com",
                    EmailConfirmed = true
                };

                var result = await _userHelper.AddUserAsync(user, "123456");

                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");

            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }


            //if(!_context.Students.Any())
            //{
            //    this.AddStudents("Isabel");
            //    this.AddStudents("José");
            //    this.AddStudents("António");
            //    await _context.SaveChangesAsync();
            //}

            if (!_context.Teachers.Any())
            {
                this.AddTeachers("No Selection");

                await _context.SaveChangesAsync();
            }

            if (!_context.Courses.Any())
            {
                this.AddCourses("No Selection");

                await _context.SaveChangesAsync();
            }

            if (!_context.Schedules.Any())
            {
                this.AddSchedules("Day");
                this.AddSchedules("Night");

                await _context.SaveChangesAsync();
            }

            if (!_context.Classrooms.Any())
            {
                this.AddClassrooms("3.01");
                this.AddClassrooms("3.02");

                await _context.SaveChangesAsync();
            }
        }

        private void AddClassrooms(string room)
        {
            _context.Classrooms.Add(new Classroom
            {
                Room = room
            });
        }

        private void AddSchedules(string shift)
        {
            _context.Schedules.Add(new Schedule
            {
                Shift = shift
            });
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
                Name = name,
                CoordinatorId = 1
            });
        }

        private void AddTeachers(string name)
        {
            _context.Teachers.Add(new Teacher
            {
                FullName = name
            });
        }
    }
}
