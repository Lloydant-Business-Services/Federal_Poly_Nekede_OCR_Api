using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class DocumentPlaygroundDto
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public long DepartmentId { get; set; }
        public long ProgrammeId { get; set; }
        public long LevelId { get; set; }
        public long SemesterId { get; set; }
        public string DocumentUrl { get; set; }
    }
}
