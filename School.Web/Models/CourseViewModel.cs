﻿using School.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace School.Web.Models
{
    public class CourseViewModel : Course
    {
        public string[] Fields = new[] { "Áudiovisuais e Produção dos Media", "Ciências Informáticas", "Eletrónica e Automação" };

        public IEnumerable<Teacher> Teachers { get; set; }

        public IEnumerable<Class> Classes { get; set; }

        public IEnumerable<IEFPSubject> IEFPSubjects { get; set; }

        public IEnumerable<Subject> Subjects { get; set; }
    }
}