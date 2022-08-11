using BusinessLayer.Interface;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ProgrammeService : IProgrammeService
    {
        private readonly FPNOOCRContext _context;

        public ProgrammeService(FPNOOCRContext context)
        {
            _context = context;

        }

        public async Task<Boolean> AddProgrammeAsync(Programme programme)
        {
            _context.Add(programme);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
