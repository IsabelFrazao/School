﻿using Microsoft.AspNetCore.Identity;
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

            if (!_context.Fields.Any())
            {
                this.AddFields("Áudiovisuais e Produção dos Media");
                this.AddFields("Ciências Informáticas");
                this.AddFields("Eletrónica e Automação");

                await _context.SaveChangesAsync();
            }

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

        private void AddFields(string area)
        {
            _context.Fields.Add(new Field
            {
                Area = area,
                isActive = true
            });
        }

        private void AddClassrooms(string room)
        {
            _context.Classrooms.Add(new Classroom
            {
                Room = room,
                isActive = true
            });
        }

        private void AddSchedules(string shift)
        {
            _context.Schedules.Add(new Schedule
            {
                Shift = shift,
                isActive = true
            });
        }

        private void AddTeachers(string name)
        {
            _context.Teachers.Add(new Teacher
            {
                FullName = name,
                Gender = "No Selection",
                DateOfBirth = DateTime.Now,
                Address = "No Selection",
                ZipCode = "No Selection",
                City = "No Selection",
                IdentificationNumber = "No Selection",
                TaxNumber = "No Selection",
                SSNumber = "No Selection",
                NHSNumber = "No Selection",
                MaritalStatus = "No Selection",
                Nationality = "No Selection",
                Telephone = "No Selection",
                Email = "No Selection",
                isActive = true,
                User = null
            });
        }

        private void AddCourses(string name)
        {
            _context.Courses.Add(new Course
            {
                FieldId = 1,
                Name = name,                
                Description = "No Selection",
                CoordinatorId = 1,
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now,
                SchoolYear = "No Selection",
                isActive = true
            });
        }        
    }
}
