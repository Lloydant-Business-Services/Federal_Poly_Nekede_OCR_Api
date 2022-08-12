using DataLayer.Dtos;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IDepartmentOptionService: IRepository<DepartmentOption>
    {
        Task<ResponseModel> AddDepartmentOption(DepartmentOptionDto model);
        Task<ResponseModel> UpdateDepartmentOption(DepartmentOptionDto model);
        Task<ResponseModel> DeleteDepartmentOption(long id);
        //Task<ResponseModel> GetDepartmentOptionsByDepartmentId(string departmentId, bool isAdmin);
    }
}
