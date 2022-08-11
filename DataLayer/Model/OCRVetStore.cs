using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class OCRVetStore : BaseModel
    {
        public virtual Session Session { get; set; }
        public virtual Department Department { get; set; }
        public virtual Programme Programme { get; set; }
        public long SessionId { get; set; }
        public long DepartmentId { get; set; }
        public long ProgrammeId { get; set; }
        public string DocumentUrl { get; set; }
        public DateTime DateAdded { get; set; }
        public bool Active { get; set; }
    }
}
