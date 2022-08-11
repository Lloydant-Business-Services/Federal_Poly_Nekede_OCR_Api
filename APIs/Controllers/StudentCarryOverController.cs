using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCarryOverController : ControllerBase
    {
        private readonly IStudentCarryOverService _service;
        private readonly FPNOOCRContext _context;

        public StudentCarryOverController(IStudentCarryOverService service, FPNOOCRContext context)
        {
            _service = service;
            _context = context;
        }
    }
}
