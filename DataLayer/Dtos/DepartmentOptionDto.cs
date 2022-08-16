using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class DepartmentOptionDto : BaseModel
    {
        public long DepartmentId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
