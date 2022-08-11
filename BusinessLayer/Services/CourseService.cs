using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CourseService : ICourseService
    {
        private readonly IConfiguration _configuration;
        private readonly FPNOOCRContext _context;

        public CourseService(IConfiguration configuration, FPNOOCRContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ResponseModel> AddCourse(AddCourseDto courseDto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                var courseCodeSlug = Utility.GenerateSlug(courseDto.CourseCode);
                var courseTitleSlug = Utility.GenerateSlug(courseDto.CourseTitle);

                User user = await _context.USER.Where(s => s.Id == courseDto.UserId).FirstOrDefaultAsync();
                Course doesCourseExist = await _context.COURSE.Where(c => c.CourseCodeSlug == courseCodeSlug && c.CourseTitleSlug == courseTitleSlug).FirstOrDefaultAsync();
                if (user == null)
                    throw new NullReferenceException("User was not found");
                if (doesCourseExist != null)
                {
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    response.Message = "Course Already Added";
                    return response;
                }
                Course course = new Course()
                {
                    CourseTitle = courseDto.CourseTitle,
                    CourseCode = courseDto.CourseCode,
                    CourseCodeSlug = Utility.GenerateSlug(courseDto.CourseCode),
                    CourseTitleSlug = Utility.GenerateSlug(courseDto.CourseTitle),
                    DateCreated = DateTime.Now,
                    Active = true
                };
                _context.Add(course);
                await _context.SaveChangesAsync();
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "success";
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto)
        {
            try
            {
                ResponseModel response = new ResponseModel();
                Course course = await _context.COURSE.Where(c => c.Id == dto.Id).FirstOrDefaultAsync();
                if (course == null)
                    throw new NullReferenceException("Course not found");
                course.CourseTitle = dto.CourseTitle != null ? dto.CourseTitle : course.CourseTitle;
                course.CourseCode = dto.CourseCode != null ? dto.CourseCode : course.CourseCode;
                _context.Update(course);
                await _context.SaveChangesAsync();
                response.StatusCode = StatusCodes.Status200OK;
                response.Message = "success";
                return response;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
   
    }
}
