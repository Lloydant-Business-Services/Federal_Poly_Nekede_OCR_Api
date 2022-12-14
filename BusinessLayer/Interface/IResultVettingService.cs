using DataLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IResultVettingService
    {
        Task<long> AddResultVetDocument(AddResultVetDto resultVetDto, string filePath, string directory);
        Task<OcrEvaluationDto> ProcessSheetForDisplayAndManipulation(long departmentId, long programmeId, long sessionId, long semesterId, long levelId);
        Task<FailDetailDto> SavedAndVerifyResult(VerifyResultDto dto);
    }
}
