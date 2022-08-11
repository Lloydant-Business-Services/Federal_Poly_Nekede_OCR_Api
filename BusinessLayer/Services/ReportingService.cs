using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ReportingService : IReportingSevice
    {
        private readonly FPNOOCRContext _context;
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;

        public ReportingService(FPNOOCRContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        //public async Task<IEnumerable<GetInstructorDto>> GetInstructors(long departmentId, long sessionSemesterId)
        //{
        //    var activeSessionSemester = await GetActiveSessionSemester();
        //    return await _context.INSTRUCTOR_DEPARTMENT.Where(d => d.DepartmentId == departmentId && d.CourseAllocation.SessionSemesterId == sessionSemesterId)
        //        .Include(d => d.User)
        //        .ThenInclude(p => p.Person)
        //        .Include(d => d.Department)
        //        .ThenInclude(f => f.FacultySchool)
        //        .Include(c => c.CourseAllocation)
        //        .ThenInclude(c => c.Course)
        //        .Select(f => new GetInstructorDto
        //        {
        //            FullName = f.User.Person.Surname + " " + f.User.Person.Firstname + " " + f.User.Person.Othername,
        //            UserId = f.UserId,
        //            Email = f.User.Person.Email,
        //            Department = f.Department,
        //            CourseCode = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseCode : null,
        //            CourseTitle = f.CourseAllocation != null ? f.CourseAllocation.Course.CourseTitle : null,
        //            CourseId = f.CourseAllocation != null ? f.CourseAllocation.CourseId : 0
        //        })
        //        .Distinct()
        //        .ToListAsync();
        //}

       

    }
}
