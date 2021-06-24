using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Core.Entity.AccountingManagement;
using Accounting.WebHost.Models;
using Accounts.DataAcess.Data;
using Accounts.DataAcess.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accounts.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly EFRepository<Account> _repository;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(EFRepository<Account> repository, ILogger<AccountController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns>Account</returns>
        [HttpGet]
        public async Task<ActionResult<List<AccountResponse>>> GetAllAccounts()
        {
            var accounts = await _repository.GetAllAsync();

            var result = await _mapper.ProjectTo<AccountResponse>(accounts.AsQueryable()).ToListAsync();

            return result;
        }

        /// <summary>
        /// Get single account by key
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns>Account</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AccountResponse>> GetAccountById(Guid id)
        {
            var account = await _repository.GetByIdAsync(id);

            var result = _mapper.Map<AccountResponse>(account);

            return result;
        }

    }
}
