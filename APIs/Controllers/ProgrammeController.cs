using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgrammeController : ControllerBase
    {
        private readonly IProgrammeService _service;
        private readonly FPNOOCRContext _context;

        public ProgrammeController(IProgrammeService service, FPNOOCRContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost("[action]")]
        public async Task<Boolean> AddProgrammeAsync(Programme programme) => await _service.AddProgrammeAsync(programme);

        [HttpGet("[action]")]
        public async Task<IEnumerable<Programme>> GetAllProgramme()
        {
            return await _context.PROGRAMME.Where(x => x.Active).ToListAsync();
        }
    }
}
