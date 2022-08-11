using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Model
{
    public class StudentResult : BaseModel
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string Course1 { get; set; }
        public string Course2 { get; set; }
        public string Course3 { get; set; }
        public string Course4 { get; set; }
        public string Course5 { get; set; }
        public string Course6 { get; set; }
        public string Course7 { get; set; }
        public string Course8 { get; set; }
        public string Course9 { get; set; }
        public string Course10 { get; set; }
        public string Course11 { get; set; }
        public string Course12 { get; set; }
        public string Course13 { get; set; }
        public string Course14 { get; set; }
        public string GPABF { get; set; }
        public string Total { get; set; }
        public string GPA { get; set; }
        public string Remark { get; set; }
        public long ProgrammeId { get; set; }
        public long DepartmentId { get; set; }
        public long SessionId { get; set; }

        [ForeignKey("ProgrammeId")]
        public Programme Programme { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        [ForeignKey("SessionId")]
        public Session Session { get; set; }
        public DateTime DateAdded { get; set; }

    }

    public class StudentCarryOver : BaseModel
    {
        public string Course1 { get; set; }
        public string Course2 { get; set; }
        public string Course3 { get; set; }
        public string Course4 { get; set; }
        public string Course5 { get; set; }
        public string Course6 { get; set; }
        public string Course7 { get; set; }
        public string Course8 { get; set; }
        public string Course9 { get; set; }
        public string Course10 { get; set; }
        public string Course11 { get; set; }
        public string Course12 { get; set; }
        public string Course13 { get; set; }
        public string Course14 { get; set; }
        public long StudentResultId { get;set; }

        [ForeignKey("StudentResultId")]
        public StudentResult StudentResult { get; set; }

    }
}
