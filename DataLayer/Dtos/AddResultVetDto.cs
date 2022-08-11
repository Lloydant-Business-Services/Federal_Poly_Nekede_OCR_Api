using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class AddResultVetDto
    {
         public long SessionId { get; set; }
        public long DepartmentId { get; set; }
        public long ProgrammeId { get; set; }
        public IFormFile ResultFile { get; set; }
    }
}
