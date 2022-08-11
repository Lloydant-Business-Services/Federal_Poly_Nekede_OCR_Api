using BusinessLayer.Interface;
using DataLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultVettingController : ControllerBase
    {
        private readonly IResultVettingService _service;
        private readonly IHostEnvironment _hostingEnvironment;

        public ResultVettingController(IResultVettingService service, IHostEnvironment hostingEnvironment)
        {
            _service = service;
            _hostingEnvironment = hostingEnvironment;

        }

        [HttpPost("[action]")]
        public async Task<long> PostResultVet([FromForm] AddResultVetDto addResultVetDto)
        {
            var directory = Path.Combine("Resources", "ResultVet");
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, directory);
            return await _service.AddResultVetDocument(addResultVetDto, filePath, directory);
        }
    }
}
