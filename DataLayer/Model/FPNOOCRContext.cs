using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.Model
{
    public class FPNOOCRContext:DbContext
    {
        public FPNOOCRContext(DbContextOptions<FPNOOCRContext> options)
        : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(f => f.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }

        public DbSet<Gender> GENDER { get; set; }
        public DbSet<Person> PERSON { get; set; }
        public DbSet<User> USER { get; set; }
        public DbSet<Role> ROLE { get; set; }
        public DbSet<Session> SESSION { get; set; }
        public DbSet<Level> LEVEL { get; set; }
        public DbSet<Semester> SEMESTER { get; set; }
        public DbSet<SessionSemester> SESSION_SEMESTER { get; set; }
        public DbSet<FacultySchool> FACULTY_SCHOOL { get; set; }
        public DbSet<Department> DEPARTMENT { get; set; }
        public DbSet<Course> COURSE { get; set; }
        public DbSet<StudentResult> STUDENT_RESULT { get; set; }
        public DbSet<StudentCarryOver> STUDENT_CARRY_OVER { get; set; }
        public DbSet<Programme> PROGRAMME{get; set; }
        public DbSet<OCRVetStore> OCR_VET_STORE{get; set; }
        public DbSet<DepartmentOption> DEPARTMENT_OPTION { get; set; }
        public DbSet<PersonCourseGrade> PERSON_COURSE_GRADE { get; set; }


        

    }
}
