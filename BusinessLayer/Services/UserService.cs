using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BusinessLayer.Infrastructure;
using BusinessLayer.Interface;
using BusinessLayer.Services.Email.Interface;
using DataLayer.Dtos;
using DataLayer.Enums;
using DataLayer.Model;
using DataLayer.Model.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using RestSharp.Authenticators;

namespace BusinessLayer.Services
{
    public class UserService : Repository<User>, IUserService
    {
        //private readonly FPNOOCRContext _context;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly string baseUrl;
        private readonly string defualtPassword = "1234567";
        ResponseModel response = new ResponseModel();

        public UserService(FPNOOCRContext context, IConfiguration configuration, IEmailService emailService)
             : base(context)
        {
           // _context = context;
            _configuration = configuration;
            baseUrl = _configuration.GetValue<string>("Url:root");
            _emailService = emailService;



        }

        public async Task<UserDto> AuthenticateUser(UserDto dto, string injectkey)
        {
            PaymentCheck isPaymentSet = PaymentCheck.Disabled;
            var user = await _context.USER
               .Include(r => r.Role)
               .Include(r => r.Person)
               .Where(f => f.Active && f.Username == dto.UserName).Include(p => p.Person).FirstOrDefaultAsync();

            if (user == null)
                return null;
            if (!user.IsVerified)
                throw new NotFoundException("Account has not been verified!");
            if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                throw new NotFoundException("Inavlid Username/Password!"); ;
          
            var token = GenerateJSONWebToken(user);
            user.LastLogin = DateTime.UtcNow;
            _context.Update(user);
            await _context.SaveChangesAsync();
            //var token = tokenHandler.CreateToken(tokenDescriptor);
            //dto.Token = tokenHandler.WriteToken(token);
            dto.Token = token;
            dto.Password = null;
            dto.UserName = user.Username;
            dto.RoleName = user.Role.Name;
            dto.UserId = user.Id;
            dto.PersonId = user.PersonId;
            dto.FullName = user.Person.Surname + " " + user.Person.Firstname + " " + user.Person.Othername;
            dto.IsHOD = user.RoleId == 4 ? true : false;
            dto.Email = user.Person.Email;
            dto.IsEmailConfirmed = user.IsVerified;
            dto.IsPasswordUpdated = user.IsPasswordUpdated == null || user.IsPasswordUpdated == false ? false : true;
            dto.PaymentCheck = isPaymentSet;
            
            return dto;
        }


        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

   

     

        private string GenerateJSONWebToken(User userInfo)
        {
            var expiryDate = DateTime.Now.AddHours(48);
            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim(CustomClaim.USER_ID, userInfo.Id.ToString()),
                    new Claim(CustomClaim.USER_ROLE, userInfo.Role.Id.ToString()),
                    new Claim(CustomClaim.USER_NAME, userInfo.Username),
                    new Claim(CustomClaim.NAME, $"{userInfo.Person.Firstname}  {userInfo.Person.Othername}"),
                    new Claim(CustomClaim.TOKEN_EXPIRY_DATE, expiryDate.ToString("yyyy-MM-dd")),
                    new Claim(CustomClaim.TOKEN_ISSUANCE_DATE, DateTime.Now.ToString("yyyy-MM-dd"))
                });

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_configuration["AppSettings:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    
        private string GenerateToken()
        {
            Random generator = new Random();
            string token = generator.Next(0, 999999).ToString("D5");

            return token;
        }

        public async Task<bool> UpdatePasswordAfterReset(ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _context.USER.Where(f => f.Person.Email == changePasswordDto.Email).Include(x => x.Person).FirstOrDefaultAsync();
                if (user == null)
                    return false;
           
               
                    Utility.CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                    return true;
            

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }

}
