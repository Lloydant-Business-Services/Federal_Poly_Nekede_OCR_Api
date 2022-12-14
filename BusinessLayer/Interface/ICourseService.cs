using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ICourseService
    {
        Task<Course> AddCourse(AddCourseDto courseDto);
        Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto);
        Task<ResponseModel> DeleteCourse(long id);  
        Task<AddCourseDto> GetCourse(long id);
    }
}
