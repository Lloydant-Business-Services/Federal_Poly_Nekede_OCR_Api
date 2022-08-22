using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class OcrEvaluationDto
    {
        public List<StudentResultHeaderDto> StudentResultHeaderDto { get; set; }
        public List<StudentResultDto> StudentResultDto { get; set; }
    }
    public class StudentResultDto
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

        //carryovers
        public string CarryOverCourse1 { get; set; }
        public string CarryOverCourse2 { get; set; }
        public string CarryOverCourse3 { get; set; }
        public string CarryOverCourse4 { get; set; }
        public string CarryOverCourse5 { get; set; }
        public string CarryOverCourse6 { get; set; }
        public string CarryOverCourse7 { get; set; }
        public string CarryOverCourse8 { get; set; }
        public string CarryOverCourse9 { get; set; }
        public string CarryOverCourse10 { get; set; }
        public string CarryOverCourse11 { get; set; }
        public string CarryOverCourse12 { get; set; }
        public string CarryOverCourse13 { get; set; }
        public string CarryOverCourse14 { get; set; }
        public string GPABF { get; set; }
        public string Total { get; set; }
        public string GPA { get; set; }
        public string Remark { get; set; }
    }
    public class StudentResultHeaderDto
    {
        public string title { get; set; }
        public string dataIndex { get; set; }
        public string key { get; set; }
    }


}
