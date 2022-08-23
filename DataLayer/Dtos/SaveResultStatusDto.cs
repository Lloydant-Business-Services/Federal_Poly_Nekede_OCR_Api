using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class SaveResultStatusDto
    {
        //public int SuccessfulSaves { get; set; }
        //public int FailedSaves { get; set; }
        public FailDetailDto SaveDetailDto { get; set; }
    }
    public class FailDetailDto
    {
        public string RegNumber { get; set; }
        public string Name { get; set; }
        public string Reason { get; set; }
        public bool Succeeded { get; set; } = true;
    }
    public class VerifyResultDto
    {
        public List<StudentResultHeaderDto> StudentResultHeaderDto { get; set; }
        public List<string> DataList { get; set; }
        public long DepartmentId { get; set; }
        public long ProgrammeId { get; set; }
        public long SessionId { get; set; }
        public long SemesterId { get; set; }
        public long LevelId { get; set; }
    }
}
