﻿using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IProgrammeService
    {
        Task<Boolean> AddProgrammeAsync(Programme programme);
    }
}
