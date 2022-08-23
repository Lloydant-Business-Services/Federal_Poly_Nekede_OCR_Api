using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly FPNOOCRContext _context;
        private readonly ICourseService _service;

        public CourseController(ICourseService service, FPNOOCRContext context)
        {
            _context = context;
            _service = service;
        }

        [HttpPost("[action]")]
        public async Task<Course> AddCourse(AddCourseDto courseDto) => await _service.AddCourse(courseDto);

        [HttpPost]
        public async Task<ResponseModel> UpdateCourseDetail(AddCourseDto dto) => await _service.UpdateCourseDetail(dto);

        [HttpPost("[action]")]
        public async Task<ResponseModel> DeleteCourse(long id) => await _service.DeleteCourse(id);

        [HttpGet("{id}")]
        public async Task<AddCourseDto> GetCourse(long id) => await _service.GetCourse(id);

    }
}
