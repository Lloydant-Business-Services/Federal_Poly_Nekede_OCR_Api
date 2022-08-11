﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using DataLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly IRepository<Session> _repo;
        private readonly FPNOOCRContext _context;

        public SessionController(IRepository<Session> repo, FPNOOCRContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpPost]
        public async Task<long> AddSesion([FromBody] Session session) => await _repo.Insert(session);
        [HttpGet]
        public async Task<IEnumerable<Session>> GetAll()
        {
            return await _context.SESSION.Where(x => x.Active)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public Session GetById(long id) => _repo.GetById(id);

        [HttpDelete]
        public void Delete(long id) => _repo.Delete(id);
    }
}
