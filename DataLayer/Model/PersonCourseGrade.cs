using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Model
{
    public class PersonCourseGrade : BaseModel
    {
        public string Grade { get; set; }
        public long CourseId { get; set; }
        public long StudentResultId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("StudentResultId")]
        public StudentResult StudentResult { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}
