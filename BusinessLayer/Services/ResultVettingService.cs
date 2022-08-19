using BusinessLayer.Interface;
using DataLayer.Dtos;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
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
        private readonly IHostEnvironment _hostingEnvironment;
        public ResultVettingService(IConfiguration configuration, FPNOOCRContext context, IHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _context = context;
            baseUrl = _configuration.GetValue<string>("Url:root");
            _hostingEnvironment = hostingEnvironment;


        }
        public async Task<IEnumerable<DocumentPlaygroundDto>> GetPlaygroundDocument(long SessionId, long SemesterId, long ProgrammeId, long DepartmentId, long LevelId)
        {
            return await _context.OCR_VET_STORE.Where(x => x.SessionId == SessionId && x.SemesterId == SemesterId && x.ProgrammeId == ProgrammeId && x.DepartmentId == DepartmentId && x.LevelId == LevelId)
                .Select(f => new DocumentPlaygroundDto
                {
                    SessionId = SessionId,
                    SemesterId = SemesterId,
                    ProgrammeId = ProgrammeId, 
                    DepartmentId = DepartmentId,
                    LevelId = LevelId,
                    DocumentUrl = f.DocumentUrl,

                }).ToListAsync();
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
                var validateProgramme = await _context.PROGRAMME.Where(x => x.Id == resultVetDto.ProgrammeId).FirstOrDefaultAsync();
                var validateLevel = await _context.LEVEL.Where(x => x.Id == resultVetDto.LevelId).FirstOrDefaultAsync();
                var validateSemester = await _context.SEMESTER.Where(x => x.Id == resultVetDto.SemesterId).FirstOrDefaultAsync();
                if (validateSession == null)
                    throw new Exception("session not found");
                if (resultVetDto.ResultFile != null)
                {
                    string fileNamePrefix = validateSession.Name + "_" + validateSemester.Name + "_" + validateProgramme.Name + "_" + validateDepartment.Name + "_" + validateLevel.Name;
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
        public async Task<ExcelSheetUploadAggregation> ProcessSheetForDisplayAndManipulation(long departmentId, long programmeId, long sessionId, long semesterId, long levelId)
        {
            ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            try
            {
                var getIncubatorDocument = await _context.OCR_VET_STORE.Where(x => x.SessionId == sessionId && x.SemesterId == semesterId && x.LevelId == levelId && x.DepartmentId == departmentId).FirstOrDefaultAsync();
                var docFile = baseUrl + getIncubatorDocument.DocumentUrl;

                //string fileName = string.Format("{0}{1}", givenFileName + "_", extType);

                var directory = Path.Combine("Resources", "ResultVet");
                var directoryPreview = Path.Combine("Resources", "IncubatorView");
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                var fullPath = Path.Combine(directory, getIncubatorDocument.DocumentUrl);
               fullPath = getIncubatorDocument.DocumentUrl;

                FileInfo fileExists = new FileInfo(fullPath);
                var filePathTemp = Path.GetTempFileName();

               // FileStream file = fileExists.CopyTo();
                string path2 = Path.GetTempFileName();
                //var file = new FileInfo(fullPath);
                //FileStream file = null;
                //IFormFile file = (IFormFile)fileExists.Open;

                if (fileExists.Exists)
                {
                    //byte[] data = File.ReadAllBytes(filePath);
                    //string fileName = Path.GetFileName(filePath);
                    
                   // file = fileExists;

                }
                //filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directoryPreview);
                //var filePathTemp = Path.GetTempFileName();
                using (var stream = File.OpenRead(fullPath))
                {
                    //await file.(stream);
                    ExcelPackage package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        //two rows space from the top to allow for the headers
                        int totalRows = worksheet.Dimension.Rows;

                        for (int i = 2; i <= totalRows; i++)
                        {
                            StudentUploadModel studentDetail = new StudentUploadModel();
                            int serialNumber = Convert.ToInt32(worksheet.Cells[i, 1].Value);
                            studentDetail.Name = worksheet.Cells[i, 2].Value != null ? worksheet.Cells[i, 2].Value.ToString() : " ";
                            studentDetail.RegistrationNumber = worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString() : " ";
                            studentDetail.Course1 = worksheet.Cells[i, 4].Value != null ? worksheet.Cells[i, 4].Value.ToString() : " ";
                            studentDetail.Course2 = worksheet.Cells[i, 5].Value != null ? worksheet.Cells[i, 5].Value.ToString() : " ";
                            studentDetail.Course3 = worksheet.Cells[i, 6].Value != null ? worksheet.Cells[i, 6].Value.ToString() : " ";

                            // studentList.Add(studentDetail);
                        }

                        //if (studentList?.Count() > 0)
                        //{

                        //    uploadAggregation = await _service.ProcessStudentUpload(studentList, departmentId);
                        //}
                    }
                }
                //long size = file.Length;
             


                    throw new NullReferenceException("Invalid Upload Sheet");
            }
            catch (Exception ex) { throw ex; }
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
