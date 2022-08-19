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
        Task<ExcelSheetUploadAggregation> ProcessSheetForDisplayAndManipulation(long departmentId, long programmeId, long sessionId, long semesterId, long levelId);
    }
}
