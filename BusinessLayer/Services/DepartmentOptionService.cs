using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessLayer.Services
{
    public class DepartmentOptionService : Repository<DepartmentOption>, IDepartmentOptionService
    {
        private readonly IConfiguration _configuration;
        public readonly string baseUrl;
        ResponseModel response = new ResponseModel();
        public DepartmentOptionService(FPNOOCRContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
            baseUrl = configuration.GetValue<string>("Url:root");
        }

        public async Task<ResponseModel> AddDepartmentOption(DepartmentOptionDto model)
        {
            try
            {
                DepartmentOption deptOption = new DepartmentOption();
                //var dpo_slug = Utility.GenerateSlug(model.Name);
                var doesExist = await _context.DEPARTMENT_OPTION.Where(dpo => dpo.Id == model.Id).FirstOrDefaultAsync();

               // var doesExist = await _context.DEPARTMENT_OPTION.Where(f => f.Name == dpo_slug).FirstOrDefaultAsync();

                if (doesExist != null)
                {
                    response.StatusCode = StatusCodes.Status208AlreadyReported;
                    response.Message = "Department Option Already Added";
                    return response;
                }
                deptOption.Name = model.Name;
                deptOption.Active = true;
                deptOption.DepartmentId = model.DepartmentId;
                _context.Add(deptOption);
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<ResponseModel> DeleteDepartmentOption(long id)
        {
            try
            {
                DepartmentOption deptOpt = await _context.DEPARTMENT_OPTION.Where(f => f.Id == id).FirstOrDefaultAsync();
                if (deptOpt != null)
                {
                    deptOpt.Active = deptOpt.Active ? false : true;
                    _context.Update(deptOpt);
                    await _context.SaveChangesAsync();
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Message = "Deleted";

                }
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async Task<ResponseModel> UpdateDepartmentOption(DepartmentOptionDto model)
        {
            try
            {
                DepartmentOption deptOption = await _context.DEPARTMENT_OPTION.Where(f => f.Id == model.Id).FirstOrDefaultAsync();
                if (deptOption == null)
                {
                    throw new NullReferenceException("Department Option not found");
                }
                else
                {
                    deptOption.Name = model.Name;
                    deptOption.Active = true;
                    if (model.DepartmentId > 0)
                    {
                        deptOption.DepartmentId = model.DepartmentId;
                    }
                    _context.Update(deptOption);
                    await _context.SaveChangesAsync();
                }
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<DepartmentOptionDto>> GetDepartmentOptionsByDepartmentId(long DepartmentId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await _context.DEPARTMENT_OPTION.Where(d => d.DepartmentId == DepartmentId).Select(d => new DepartmentOptionDto
                {
                    Name = d.Name,
                    Id = d.Id,
                    Active = d.Active,
                })
                  .OrderBy(d => d.Name)
                  .ToListAsync();
            }
            else
            {
                return await _context.DEPARTMENT_OPTION.Where(d => d.DepartmentId == DepartmentId && d.Active)
                    .Select(d => new DepartmentOptionDto
                    {
                        Name = d.Name,
                        Id = d.Id,
                        Active = d.Active,
                    })
                    .OrderBy(d => d.Name)
                    .ToListAsync();
            }
        }
    }
}
