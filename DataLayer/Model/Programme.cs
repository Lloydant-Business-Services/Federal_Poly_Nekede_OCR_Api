using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Model
{
    public class Programme : BaseModel
    {
        public string Name { get; set; }    
        public bool Active { get; set; }
    }
}
