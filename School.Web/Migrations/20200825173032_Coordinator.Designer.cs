﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using School.Web.Data;

namespace School.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20200825173032_Coordinator")]
    partial class Coordinator
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("School.Web.Data.Entities.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CourseId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Room");

                    b.Property<string>("Schedule");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BeginDate");

                    b.Property<int>("CoordinatorId");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Field");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CoordinatorId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Approval")
                        .IsRequired();

                    b.Property<int>("ClassId");

                    b.Property<string>("FinalGrade")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<int>("ClassId");

                    b.Property<int>("CourseId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(30);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Gender");

                    b.Property<string>("IdentificationNumber")
                        .HasMaxLength(30);

                    b.Property<string>("MaritalStatus");

                    b.Property<string>("NHSNumber")
                        .HasMaxLength(30);

                    b.Property<string>("Nationality")
                        .HasMaxLength(30);

                    b.Property<string>("PhotoUrl");

                    b.Property<string>("SSNumber")
                        .HasMaxLength(30);

                    b.Property<string>("Schedule");

                    b.Property<string>("TaxNumber")
                        .HasMaxLength(30);

                    b.Property<string>("Telephone")
                        .HasMaxLength(30);

                    b.Property<string>("ZipCode")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("CourseId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Component")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Credits")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<double>("Duration")
                        .HasMaxLength(50);

                    b.Property<string>("Field")
                        .IsRequired();

                    b.Property<string>("FieldCode")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("QEQLevel")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("QNQLevel")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ReferenceCode")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("WebCode");

                    b.HasKey("Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Teacher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .HasMaxLength(50);

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .HasMaxLength(30);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Gender");

                    b.Property<string>("IdentificationNumber")
                        .HasMaxLength(30);

                    b.Property<string>("MaritalStatus");

                    b.Property<string>("NHSNumber")
                        .HasMaxLength(30);

                    b.Property<string>("Nationality")
                        .HasMaxLength(30);

                    b.Property<string>("PhotoUrl");

                    b.Property<string>("SSNumber")
                        .HasMaxLength(30);

                    b.Property<string>("TaxNumber")
                        .HasMaxLength(30);

                    b.Property<string>("Telephone")
                        .HasMaxLength(30);

                    b.Property<string>("ZipCode")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Class", b =>
                {
                    b.HasOne("School.Web.Data.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("School.Web.Data.Entities.Course", b =>
                {
                    b.HasOne("School.Web.Data.Entities.Teacher", "Coordinator")
                        .WithMany()
                        .HasForeignKey("CoordinatorId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("School.Web.Data.Entities.Grade", b =>
                {
                    b.HasOne("School.Web.Data.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("School.Web.Data.Entities.Student", b =>
                {
                    b.HasOne("School.Web.Data.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("School.Web.Data.Entities.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
