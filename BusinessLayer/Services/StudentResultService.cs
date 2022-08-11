using BusinessLayer.Interface;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class StudentResultService : IStudentResultService
    {
        private readonly FPNOOCRContext _context;

        public StudentResultService(FPNOOCRContext context)
        {
            _context = context;

        }
        
    }
}
