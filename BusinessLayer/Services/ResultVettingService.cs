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
                    LevelId = resultVetDto.LevelId,
                    SemesterId = resultVetDto.SemesterId,
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
        public async Task<OcrEvaluationDto> ProcessSheetForDisplayAndManipulation(long departmentId, long programmeId, long sessionId, long semesterId, long levelId)
        {
            OcrEvaluationDto ocrEvaluationDto = new OcrEvaluationDto();
            List<StudentResultDto> headerList = new List<StudentResultDto>();
            List<StudentResultDto> dataList = new List<StudentResultDto>();
            //ExcelSheetUploadAggregation uploadAggregation = new ExcelSheetUploadAggregation();
            try
            {
                var getIncubatorDocument = await _context.OCR_VET_STORE.Where(x => x.SessionId == sessionId && x.SemesterId == semesterId && x.LevelId == levelId && x.DepartmentId == departmentId).FirstOrDefaultAsync();
                var docFile = baseUrl + getIncubatorDocument?.DocumentUrl;

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
                if (fileExists.Exists)
                {
                    //byte[] data = File.ReadAllBytes(filePath);
                    //string fileName = Path.GetFileName(filePath);
                    
                   // file = fileExists;

                }
               
                using (var stream = File.OpenRead(fullPath))
                {
                    ExcelPackage package = new ExcelPackage(stream);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        //two rows space from the top to allow for the headers
                        int totalRows = worksheet.Dimension.Rows;
                        int totalColumns = worksheet.Dimension.Columns;
                        StudentResultHeaderDto headerDetail = new StudentResultHeaderDto();
                        var listHeader = ResolveHeaderData(worksheet, totalColumns);
                        ocrEvaluationDto.StudentResultHeaderDto = listHeader;

                        for (int i = 2; i <= totalRows; i++)
                        {
                            StudentResultDto studentDetail = new StudentResultDto();
                            //string serialNumber = "S/N";
                            studentDetail.RegistrationNumber = worksheet.Cells[i, 2].Value != null ? worksheet.Cells[i, 2].Value.ToString() : "-"; ;
                            studentDetail.Name = worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString() : "-";
                            //courses
                            studentDetail.Course1 = worksheet.Cells[i, 4].Value != null ? worksheet.Cells[i, 4].Value.ToString() : "-";
                            studentDetail.Course2 = worksheet.Cells[i, 5].Value != null ? worksheet.Cells[i, 5].Value.ToString() : "-";
                            studentDetail.Course3 = worksheet.Cells[i, 6].Value != null ? worksheet.Cells[i, 6].Value.ToString() : "-";
                            studentDetail.Course4 = worksheet.Cells[i, 7].Value != null ? worksheet.Cells[i, 7].Value.ToString() : "-";
                            studentDetail.Course5 = worksheet.Cells[i, 8].Value != null ? worksheet.Cells[i, 8].Value.ToString() : "-";
                            studentDetail.Course6 = worksheet.Cells[i, 9].Value != null ? worksheet.Cells[i, 9].Value.ToString() : "-";
                            studentDetail.Course7 = worksheet.Cells[i, 10].Value != null ? worksheet.Cells[i, 10].Value.ToString() : "-";
                            studentDetail.Course8 = worksheet.Cells[i, 11].Value != null ? worksheet.Cells[i, 11].Value.ToString() : "-";
                            studentDetail.Course9 = worksheet.Cells[i, 12].Value != null ? worksheet.Cells[i, 12].Value.ToString() : "-";
                            studentDetail.Course10 = worksheet.Cells[i, 13].Value != null ? worksheet.Cells[i, 13].Value.ToString() : "-";
                            studentDetail.Course11 = worksheet.Cells[i, 14].Value != null ? worksheet.Cells[i, 14].Value.ToString() : "-";
                            studentDetail.Course12 = worksheet.Cells[i, 15].Value != null ? worksheet.Cells[i, 15].Value.ToString() : "-";
                            studentDetail.Course13 = worksheet.Cells[i, 16].Value != null ? worksheet.Cells[i, 16].Value.ToString() : "-";
                            studentDetail.Course14 = worksheet.Cells[i, 17].Value != null ? worksheet.Cells[i, 17].Value.ToString() : "-";



                            //carry over courses
                            studentDetail.CarryOverCourse1 = worksheet.Cells[i, 18].Value != null ? worksheet.Cells[i, 18].Value.ToString() : "-";
                            studentDetail.CarryOverCourse2 = worksheet.Cells[i, 19].Value != null ? worksheet.Cells[i, 19].Value.ToString() : "-";
                            studentDetail.CarryOverCourse3 = worksheet.Cells[i, 20].Value != null ? worksheet.Cells[i, 20].Value.ToString() : "-";
                            studentDetail.CarryOverCourse4 = worksheet.Cells[i, 21].Value != null ? worksheet.Cells[i, 21].Value.ToString() : "-";
                            studentDetail.CarryOverCourse5 = worksheet.Cells[i, 22].Value != null ? worksheet.Cells[i, 22].Value.ToString() : "-";
                            studentDetail.CarryOverCourse6 = worksheet.Cells[i, 23].Value != null ? worksheet.Cells[i, 23].Value.ToString() : "-";
                            studentDetail.CarryOverCourse7 = worksheet.Cells[i, 24].Value != null ? worksheet.Cells[i, 24].Value.ToString() : "-";
                            studentDetail.CarryOverCourse8 = worksheet.Cells[i, 25].Value != null ? worksheet.Cells[i, 25].Value.ToString() : "-";
                            studentDetail.CarryOverCourse9 = worksheet.Cells[i, 26].Value != null ? worksheet.Cells[i, 26].Value.ToString() : "-";
                            studentDetail.CarryOverCourse10 = worksheet.Cells[i, 27].Value != null ? worksheet.Cells[i, 27].Value.ToString() : "-";
                            studentDetail.CarryOverCourse11 = worksheet.Cells[i, 28].Value != null ? worksheet.Cells[i, 28].Value.ToString() : "-";
                            studentDetail.CarryOverCourse12 = worksheet.Cells[i, 29].Value != null ? worksheet.Cells[i, 29].Value.ToString() : "-";
                            studentDetail.CarryOverCourse13 = worksheet.Cells[i, 30].Value != null ? worksheet.Cells[i, 30].Value.ToString() : "-";
                            studentDetail.CarryOverCourse14 = worksheet.Cells[i, 31].Value != null ? worksheet.Cells[i, 31].Value.ToString() : "-";
                            studentDetail.GPABF = worksheet.Cells[i, totalColumns-3].Value != null ? worksheet.Cells[i, totalColumns-3].Value.ToString() : "-";
                            studentDetail.Total = worksheet.Cells[i, totalColumns-2].Value != null ? worksheet.Cells[i, totalColumns-2].Value.ToString() : "-";
                            studentDetail.GPA = worksheet.Cells[i, totalColumns-1].Value != null ? worksheet.Cells[i, totalColumns-1].Value.ToString() : "-";
                            studentDetail.Remark = worksheet.Cells[i, totalColumns].Value != null ? worksheet.Cells[i, totalColumns].Value.ToString() : "-";

                            //have a method thatb recieves studemt details and returns it updtated and corrected
                            if(i == 1)
                            {
                                headerList.Add(studentDetail);
                                //var splitItem = studentDetail.sp
                            }
                            else
                            {
                                //var dd = 
                                dataList.Add(DataEvaluation(studentDetail));
                            }
                            
                        }

                        //ocrEvaluationDto.StudentResultHeaderDto = headerList;
                        ocrEvaluationDto.StudentResultDto = dataList;
                    }
                }
                //long size = file.Length;
                return ocrEvaluationDto;
             
            throw new NullReferenceException("Invalid Upload Sheet");
            }
            catch (Exception ex) { throw ex; }
        }
        public List<StudentResultHeaderDto> ResolveHeaderData(ExcelWorksheet worksheet, int totalColumns)
        {
            List<StudentResultHeaderDto> headerList = new List<StudentResultHeaderDto>();

            if(worksheet != null)
            {
                StudentResultHeaderDto headerDto = new StudentResultHeaderDto();
                StudentResultDto studentDetail = new StudentResultDto();
                studentDetail.RegistrationNumber = worksheet.Cells[1, 2].Value != null ? worksheet.Cells[1, 2].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.RegistrationNumber,
                    key = studentDetail.RegistrationNumber.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.RegistrationNumber.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);
                //headerDto = new StudentResultHeaderDto();
                studentDetail.Name = worksheet.Cells[1, 3].Value != null ? worksheet.Cells[1, 3].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Name,
                    key = studentDetail.Name.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Name.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                //courses
                studentDetail.Course1 = worksheet.Cells[1, 4].Value != null ? worksheet.Cells[1, 4].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course1,
                    key = studentDetail.Course1.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course1.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);
                
                studentDetail.Course2 = worksheet.Cells[1, 5].Value != null ? worksheet.Cells[1, 5].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course2,
                    key = studentDetail.Course2.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course2.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course3 = worksheet.Cells[1, 6].Value != null ? worksheet.Cells[1, 6].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course3,
                    key = studentDetail.Course3.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course3.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course4 = worksheet.Cells[1, 7].Value != null ? worksheet.Cells[1, 7].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course4,
                    key = studentDetail.Course4.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course4.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course5 = worksheet.Cells[1, 8].Value != null ? worksheet.Cells[1, 8].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course5,
                    key = studentDetail.Course5.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course5.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course6 = worksheet.Cells[1, 9].Value != null ? worksheet.Cells[1, 9].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course6,
                    key = studentDetail.Course6.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course6.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course7 = worksheet.Cells[1, 10].Value != null ? worksheet.Cells[1, 10].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course7,
                    key = studentDetail.Course7.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course7.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course8 = worksheet.Cells[1, 11].Value != null ? worksheet.Cells[1, 11].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course8,
                    key = studentDetail.Course8.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course8.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course9 = worksheet.Cells[1, 12].Value != null ? worksheet.Cells[1, 12].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course9,
                    key = studentDetail.Course9.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course9.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course10 = worksheet.Cells[1, 13].Value != null ? worksheet.Cells[1, 13].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course10,
                    key = studentDetail.Course10.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course10.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course11 = worksheet.Cells[1, 14].Value != null ? worksheet.Cells[1, 14].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course11,
                    key = studentDetail.Course11.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course11.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course12 = worksheet.Cells[1, 15].Value != null ? worksheet.Cells[1, 15].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course12,
                    key = studentDetail.Course12.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course12.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course13 = worksheet.Cells[1, 16].Value != null ? worksheet.Cells[1, 16].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course13,
                    key = studentDetail.Course13.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course13.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Course14 = worksheet.Cells[1, 17].Value != null ? worksheet.Cells[1, 17].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Course14,
                    key = studentDetail.Course14.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Course14.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);



                //carry over courses
                studentDetail.CarryOverCourse1 = worksheet.Cells[1, 18].Value != null ? worksheet.Cells[1, 18].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse1,
                    key = studentDetail.CarryOverCourse1.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse1.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse2 = worksheet.Cells[1, 19].Value != null ? worksheet.Cells[1, 19].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse2,
                    key = studentDetail.CarryOverCourse2.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse2.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse3 = worksheet.Cells[1, 20].Value != null ? worksheet.Cells[1, 20].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse3,
                    key = studentDetail.CarryOverCourse3.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse3.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse4 = worksheet.Cells[1, 21].Value != null ? worksheet.Cells[1, 21].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse4,
                    key = studentDetail.CarryOverCourse4.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse4.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse5 = worksheet.Cells[1, 22].Value != null ? worksheet.Cells[1, 22].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse5,
                    key = studentDetail.CarryOverCourse5.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse5.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse6 = worksheet.Cells[1, 23].Value != null ? worksheet.Cells[1, 23].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse6,
                    key = studentDetail.CarryOverCourse6.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse6.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse7 = worksheet.Cells[1, 24].Value != null ? worksheet.Cells[1, 24].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse7,
                    key = studentDetail.CarryOverCourse7.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse7.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse8 = worksheet.Cells[1, 25].Value != null ? worksheet.Cells[1, 25].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse8,
                    key = studentDetail.CarryOverCourse8.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse8.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse9 = worksheet.Cells[1, 26].Value != null ? worksheet.Cells[1, 26].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse9,
                    key = studentDetail.CarryOverCourse9.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse9.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse10 = worksheet.Cells[1, 27].Value != null ? worksheet.Cells[1, 27].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse10,
                    key = studentDetail.CarryOverCourse10.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse10.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse11 = worksheet.Cells[1, 28].Value != null ? worksheet.Cells[1, 28].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse11,
                    key = studentDetail.CarryOverCourse11.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse11.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse12 = worksheet.Cells[1, 29].Value != null ? worksheet.Cells[1, 29].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse12,
                    key = studentDetail.CarryOverCourse12.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse12.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse13 = worksheet.Cells[1, 30].Value != null ? worksheet.Cells[1, 30].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse13,
                    key = studentDetail.CarryOverCourse13.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse13.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.CarryOverCourse14 = worksheet.Cells[1, 31].Value != null ? worksheet.Cells[1, 31].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.CarryOverCourse14,
                    key = studentDetail.CarryOverCourse14.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.CarryOverCourse14.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.GPABF = worksheet.Cells[1, totalColumns - 3].Value != null ? worksheet.Cells[1, totalColumns - 3].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.GPABF,
                    key = studentDetail.GPABF.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.GPABF.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Total = worksheet.Cells[1, totalColumns - 2].Value != null ? worksheet.Cells[1, totalColumns - 2].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Total,
                    key = studentDetail.Total.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Total.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.GPA = worksheet.Cells[1, totalColumns - 1].Value != null ? worksheet.Cells[1, totalColumns - 1].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.GPA,
                    key = studentDetail.GPA.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.GPA.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

                studentDetail.Remark = worksheet.Cells[1, totalColumns].Value != null ? worksheet.Cells[1, totalColumns].Value.ToString() : "-";
                headerDto = new StudentResultHeaderDto()
                {
                    title = studentDetail.Remark,
                    key = studentDetail.Remark.ToLower().Replace(" ", ""),
                    dataIndex = studentDetail.Remark.ToLower().Replace(" ", "")
                };
                headerList.Add(headerDto);

            }
            return headerList;

        }
        public StudentResultDto DataEvaluation(StudentResultDto rowData)
        {
            if(rowData != null)
            {
                //StudentResultDto studentResult = new StudentResultDto();
                //if(rowData.Course1 != null)
                rowData.Course1 = EvaluateCourseGrades(rowData.Course1);
                rowData.Course2 = EvaluateCourseGrades(rowData.Course2);
                rowData.Course3 = EvaluateCourseGrades(rowData.Course3);
                rowData.Course4 = EvaluateCourseGrades(rowData.Course4);
                rowData.Course5 = EvaluateCourseGrades(rowData.Course5);
                rowData.Course6 = EvaluateCourseGrades(rowData.Course6);
                rowData.Course7 = EvaluateCourseGrades(rowData.Course7);
                rowData.Course8 = EvaluateCourseGrades(rowData.Course8);
                rowData.Course9 = EvaluateCourseGrades(rowData.Course9);
                rowData.Course10 = EvaluateCourseGrades(rowData.Course10);
                rowData.Course11 = EvaluateCourseGrades(rowData.Course11);
                rowData.Course12 = EvaluateCourseGrades(rowData.Course12);
                rowData.Course13 = EvaluateCourseGrades(rowData.Course13);
                rowData.Course14 = EvaluateCourseGrades(rowData.Course14);

                rowData.CarryOverCourse1 = EvaluateCourseGrades(rowData.CarryOverCourse1);
                rowData.CarryOverCourse2 = EvaluateCourseGrades(rowData.CarryOverCourse2);
                rowData.CarryOverCourse3 = EvaluateCourseGrades(rowData.CarryOverCourse3);
                rowData.CarryOverCourse4 = EvaluateCourseGrades(rowData.CarryOverCourse4);
                rowData.CarryOverCourse5 = EvaluateCourseGrades(rowData.CarryOverCourse5);
                rowData.CarryOverCourse6 = EvaluateCourseGrades(rowData.CarryOverCourse6);
                rowData.CarryOverCourse7 = EvaluateCourseGrades(rowData.CarryOverCourse7);
                rowData.CarryOverCourse8 = EvaluateCourseGrades(rowData.CarryOverCourse8);
                rowData.CarryOverCourse9 = EvaluateCourseGrades(rowData.CarryOverCourse9);
                rowData.CarryOverCourse10 = EvaluateCourseGrades(rowData.CarryOverCourse10);
                rowData.CarryOverCourse11 = EvaluateCourseGrades(rowData.CarryOverCourse11);
                rowData.CarryOverCourse12 = EvaluateCourseGrades(rowData.CarryOverCourse12);
                rowData.CarryOverCourse13 = EvaluateCourseGrades(rowData.CarryOverCourse13);
                rowData.CarryOverCourse14 = EvaluateCourseGrades(rowData.CarryOverCourse14);


            }
            return rowData;
        }

        public string EvaluateCourseGrades(string data)
        {
            if (data == null || data.Length > 3)
                return "-";
            //var returnData = data.Contains()
            var match = data.IndexOfAny("ABCDEF".ToCharArray());
            if (match != -1)
                return data;
            return "-";
        }
        public StudentResultDto HeaderEvaluation(StudentResultDto rowData)
        {
            if (rowData != null)
            {
                //StudentResultDto studentResult = new StudentResultDto();
                //if(rowData.Course1 != null)
                rowData.RegistrationNumber = rowData.RegistrationNumber;
                rowData.Name = rowData.Name;
                rowData.Course1 = EvaluateCourseGrades(rowData.Course1);
                rowData.Course2 = EvaluateCourseGrades(rowData.Course2);
                rowData.Course3 = EvaluateCourseGrades(rowData.Course3);
                rowData.Course4 = EvaluateCourseGrades(rowData.Course4);
                rowData.Course5 = EvaluateCourseGrades(rowData.Course5);
                rowData.Course6 = EvaluateCourseGrades(rowData.Course6);
                rowData.Course7 = EvaluateCourseGrades(rowData.Course7);
                rowData.Course8 = EvaluateCourseGrades(rowData.Course8);
                rowData.Course9 = EvaluateCourseGrades(rowData.Course9);
                rowData.Course10 = EvaluateCourseGrades(rowData.Course10);
                rowData.Course11 = EvaluateCourseGrades(rowData.Course11);
                rowData.Course12 = EvaluateCourseGrades(rowData.Course12);
                rowData.Course13 = EvaluateCourseGrades(rowData.Course13);
                rowData.Course14 = EvaluateCourseGrades(rowData.Course14);

                rowData.CarryOverCourse1 = EvaluateCourseGrades(rowData.CarryOverCourse1);
                rowData.CarryOverCourse2 = EvaluateCourseGrades(rowData.CarryOverCourse2);
                rowData.CarryOverCourse3 = EvaluateCourseGrades(rowData.CarryOverCourse3);
                rowData.CarryOverCourse4 = EvaluateCourseGrades(rowData.CarryOverCourse4);
                rowData.CarryOverCourse5 = EvaluateCourseGrades(rowData.CarryOverCourse5);
                rowData.CarryOverCourse6 = EvaluateCourseGrades(rowData.CarryOverCourse6);
                rowData.CarryOverCourse7 = EvaluateCourseGrades(rowData.CarryOverCourse7);
                rowData.CarryOverCourse8 = EvaluateCourseGrades(rowData.CarryOverCourse8);
                rowData.CarryOverCourse9 = EvaluateCourseGrades(rowData.CarryOverCourse9);
                rowData.CarryOverCourse10 = EvaluateCourseGrades(rowData.CarryOverCourse10);
                rowData.CarryOverCourse11 = EvaluateCourseGrades(rowData.CarryOverCourse11);
                rowData.CarryOverCourse12 = EvaluateCourseGrades(rowData.CarryOverCourse12);
                rowData.CarryOverCourse13 = EvaluateCourseGrades(rowData.CarryOverCourse13);
                rowData.CarryOverCourse14 = EvaluateCourseGrades(rowData.CarryOverCourse14);


            }
            return rowData;
        }
        public string EvaluateHeaderColumn(string data)
        {
            if (data == null || data.Length > 3)
                return "-";
            //var returnData = data.Contains()
            var match = data.IndexOfAny("ABCDEF".ToCharArray());
            if (match != -1)
                return data;
            return "-";
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
