using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ResultVettingService : IResultVettingService
    {
        private readonly IConfiguration _configuration;
        private readonly FPNOOCRContext _context;
        private readonly string baseUrl;
        public ResultVettingService(IConfiguration configuration, FPNOOCRContext context)
        {
            _configuration = configuration;
            _context = context;
            baseUrl = _configuration.GetValue<string>("Url:root");

        }

        public async Task<long> AddResultVetDocument(AddResultVetDto resultVetDto, string filePath, string directory)
        {
            if (resultVetDto?.SessionId > 0 && resultVetDto?.DepartmentId > 0)
            {
                var isAlreadyUploaded = await _context.OCR_VET_STORE
                    .Where(f => f.SessionId == resultVetDto.SessionId && f.DepartmentId == resultVetDto.DepartmentId).FirstOrDefaultAsync();
                if (isAlreadyUploaded != null)
                    throw new NullReferenceException("Already uploaded");
                var docLink = string.Empty;

                var validateSession = await _context.SESSION.Where(x => x.Id == resultVetDto.SessionId).FirstOrDefaultAsync();
                var validateDepartment = await _context.DEPARTMENT.Where(x => x.Id == resultVetDto.DepartmentId).FirstOrDefaultAsync();
                if (validateSession == null)
                    throw new Exception("session not found");
                if (resultVetDto.ResultFile != null)
                {
                    string fileNamePrefix = validateSession.Name + "_" + validateDepartment.Name + "_";
                    docLink = await SaveResultSheetForVetting(resultVetDto.ResultFile, filePath, directory, fileNamePrefix);
                }

                OCRVetStore resultVetStore = new OCRVetStore()
                {
                    SessionId = resultVetDto.SessionId,
                    DocumentUrl = docLink,
                    DepartmentId = resultVetDto.DepartmentId,
                    ProgrammeId = resultVetDto.ProgrammeId,
                    DateAdded = DateTime.Now,
                    Active = true
                };
                _context.Add(resultVetStore);
                var created = await _context.SaveChangesAsync();
                if (created > 0)
                    return StatusCodes.Status200OK;
            }
            else
            {
                throw new NullReferenceException("Enter session and department");
            }
            return 0;
        }

        public async Task<string> SaveResultSheetForVetting(IFormFile file, string filePath, string directory, string givenFileName)
        {
            var noteUrl = string.Empty;

            var validFileSize = (1024 * 1024);//1mb
            List<string> validFileExtension = new List<string>();
            validFileExtension.Add(".xlx");
            validFileExtension.Add(".xlsx");

            if (file.Length > 0)
            {
                var extType = Path.GetExtension(file.FileName);
                var fileSize = file.Length;
                if (fileSize <= validFileSize)
                {
                    if (validFileExtension.Contains(extType))
                    {
                        string fileName = string.Format("{0}{1}", givenFileName + "_", extType);
                        //create file path if it doesnt exist
                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }
                        var fullPath = Path.Combine(filePath, fileName);
                        noteUrl = Path.Combine(directory, fileName);
                        //Delete if file exist
                        FileInfo fileExists = new FileInfo(fullPath);
                        if (fileExists.Exists)
                        {
                            fileExists.Delete();
                        }

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        return noteUrl = noteUrl.Replace('\\', '/');
                    }
                    else
                    {
                        throw new BadImageFormatException("Invalid file type...Accepted formats are xlsx, xlx");
                    }
                }
            }
            return noteUrl;
        }

    }
}
