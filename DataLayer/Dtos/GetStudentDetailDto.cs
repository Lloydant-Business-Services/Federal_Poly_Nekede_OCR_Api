using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Dtos
{
    public class GetStudentDetailDto : BaseModel
    {
        public string RegistrationNumber { get; set; }
        public string SessionName { get; set; }
        public string DepartmentName { get; set; }
        public string ProgrammeName { get; set; }
        public string LevelName { get; set; }
        public string SemesterName { get; set; }
        public string StudentName { get; set; }

        public List<ResultGradeDto> resultGradeList { get; set; }


       
    }

    public class ResultGradeDto
    {
        public string CourseCode { get; set; }

        public string CourseTitle { get; set; }

        public string Grade { get; set; }
    }
}
