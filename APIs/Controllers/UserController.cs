using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer;
using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APIs.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly string key;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
            key = _configuration.GetValue<string>("AppSettings:Key");
        }

        [HttpPost("Authenticate")]
        public async Task<UserDto> AuthenticateUser(UserDto dto) => await _userService.AuthenticateUser(dto, key);

        [HttpPost("[action]")]
        public async Task<bool> UpdatePasswordAfterReset(ChangePasswordDto changePasswordDto) => await _userService.UpdatePasswordAfterReset(changePasswordDto);



    }
}
