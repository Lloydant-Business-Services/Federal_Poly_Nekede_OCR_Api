using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentResultController : ControllerBase
    {
        private readonly IStudentResultService _service;
        private readonly FPNOOCRContext _context;

        public StudentResultController(IStudentResultService service, FPNOOCRContext context)
        {
            _service = service;
            _context = context;
        }
    }
}
