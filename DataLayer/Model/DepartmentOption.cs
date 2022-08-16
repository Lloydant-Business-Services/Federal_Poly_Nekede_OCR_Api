using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class DepartmentOption : BaseModel
    {
        public long? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
