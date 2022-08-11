using BusinessLayer.Interface;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class StudentCarryOverService : IStudentCarryOverService
    {
        private readonly FPNOOCRContext _context;

        public StudentCarryOverService(FPNOOCRContext context)
        {
            _context = context;

        }
    }
}
