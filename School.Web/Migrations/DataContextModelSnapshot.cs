﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using School.Web.Data;

namespace School.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Room")
                        .IsRequired();

                    b.Property<string>("Schedule")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("BeginDate");

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Field")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

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

            modelBuilder.Entity("School.Web.Data.Entities.Professor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("MaritalStatus")
                        .IsRequired();

                    b.Property<string>("NHSNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("SSNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Professors");
                });

            modelBuilder.Entity("School.Web.Data.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Gender")
                        .IsRequired();

                    b.Property<string>("IdentificationNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("MaritalStatus")
                        .IsRequired();

                    b.Property<string>("NHSNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("SSNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Schedule")
                        .IsRequired();

                    b.Property<string>("TaxNumber")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("ZipCode")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("Id");

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

            modelBuilder.Entity("School.Web.Data.Entities.Grade", b =>
                {
                    b.HasOne("School.Web.Data.Entities.Class", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
