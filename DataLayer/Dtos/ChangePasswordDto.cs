using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Dtos
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
