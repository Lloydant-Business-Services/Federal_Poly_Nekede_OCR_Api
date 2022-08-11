using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class DBInitializer
    {
        public async static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new FPNOOCRContext(serviceProvider.GetRequiredService<DbContextOptions<FPNOOCRContext>>());
            context.Database.EnsureCreated();
            // Look for any students.
            if (await context.ROLE.AnyAsync())
            {
                return;   // DB has been seeded
            }

            var roles = new Role[]
              {
                    new Role{ Active = true, Name = "Admin"},
                    new Role{ Active = true, Name = "Student"},
              };
            foreach (Role role in roles)
            {
                context.Add(role);
            }
            await context.SaveChangesAsync();

            var genders = new Gender[]
            {
                new Gender{ Active = true, Name = "Male"},
                new Gender{ Active = true, Name = "Female"},
            };
            foreach (Gender gender in genders)
            {
                context.Add(gender);
            }
            await context.SaveChangesAsync();
           
            var personTypes = new PersonType[]
               {
                new PersonType{ Active = true, Name = "Student"},
                new PersonType{ Active = true, Name = "Admin"},
               };
            foreach (PersonType personType in personTypes)
            {
                context.Add(personType);
            }
            await context.SaveChangesAsync();

            var sessions = new Session[]
               {
                new Session{ Active = true, Name = "2019/2020"},
                new Session{ Active = true, Name = "2020/2021"},
                new Session{ Active = true, Name = "2021/2022"},
               };
            foreach (Session session in sessions)
            {
                context.Add(session);
            }
            await context.SaveChangesAsync();
            var semesters = new Semester[]
               {
                new Semester{ Active = true, Name = "First Semester"},
                new Semester{ Active = true, Name = "Second Semester"},
                new Semester{ Active = true, Name = "Third Semester"},
               };
            foreach (Semester semester in semesters)
            {
                context.Add(semester);
            }
            await context.SaveChangesAsync();

            var levels = new Level[]
               {
                new Level{ Active = true, Name = "ND I"},
                new Level{ Active = true, Name = "ND II"},
                new Level{ Active = true, Name = "HND I"},
                new Level{ Active = true, Name = "HND II"},
               };
            foreach (Semester semester in semesters)
            {
                context.Add(semester);
            }
            await context.SaveChangesAsync();
           

            foreach (Role role in roles)
            {
                context.Add(role);
            }
            await context.SaveChangesAsync();

        }

    }
}
