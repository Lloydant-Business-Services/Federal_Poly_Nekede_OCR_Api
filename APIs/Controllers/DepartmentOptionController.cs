using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DataLayer.Dtos;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentOptionController : ControllerBase
    {
        private readonly IDepartmentOptionService _service;
        private readonly FPNOOCRContext _context;

        public DepartmentOptionController(IDepartmentOptionService service, FPNOOCRContext context)
        {
            _service = service;
            _context = context;

        }

        [HttpPost("[action]")]
        public async Task<ResponseModel> AddDepartmentOption(DepartmentOptionDto model) => await _service.AddDepartmentOption(model);

        [HttpGet("[action]")]
        public async Task<IEnumerable<DepartmentOptionDto>> GetDepartmentOptions()
        {
            return await _context.DEPARTMENT_OPTION.Where(a => a.Active).Include(f => f.Name)
                .Select(f => new DepartmentOptionDto
                {
                    Name = f.Name,
                    DepartmentId = (long)f.DepartmentId,
                    Id = f.Id,
                    Active = f.Active,
                })
                .OrderBy(a => a.Name) 
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public DepartmentOption GetById(long id) => _service.GetById(id);

        [HttpPost("[action]")]
        public async Task<ResponseModel> UpdateDepartmentOption(DepartmentOptionDto model) => await _service.UpdateDepartmentOption(model);

        [HttpPost("[action]")]
        public async Task<ResponseModel> DeleteDepartmentOption(long id) => await _service.DeleteDepartmentOption(id);
    }
}
