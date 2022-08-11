﻿using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserService : IRepository<User>
    {

        Task<UserDto> AuthenticateUser(UserDto dto, string injectkey);
        //Task<long> PostUser(AddUserDto userDto);
        Task<bool> UpdatePasswordAfterReset(ChangePasswordDto changePasswordDto);


    }
}
