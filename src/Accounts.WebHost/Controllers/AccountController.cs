using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounting.Core.Entity.AccountingManagement;
using Accounting.Core.Repository;
using Accounting.WebHost.Models;
using Accounts.Core.Services;
using Accounts.DataAcess.Data;
using Accounts.DataAcess.Repositories;
using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Accounts.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRepository<Account> _repository;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(IRepository<Account> repository, ILogger<AccountController> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Create new account
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateAccount(AccountRequest request)
        {
            try
            {
                var existingAccount = (await _repository.GetWithIncludeAsync(c =>
                        c.Domain.ToLower() == request.Domain.ToLower() && c.UserName == request.UserName)).ToList();

                if (existingAccount.Count > 0)
                    return BadRequest("Account with same name and domain exists.");

                var passMessage = PasswordService.ValidatePassword(request.Password);

                if (!String.IsNullOrEmpty(passMessage))
                {
                    return BadRequest(passMessage);
                }

                var account = _mapper.Map<Account>(request);

                await _repository.CreateAsync(account);

                return Ok();
            }
            catch (Exception e)
            {
                var message = e.Message + e.InnerException?.Message;

                _logger.LogError(message);

                return BadRequest($"Cannot create account. {message}");
            }
        }

        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <param name="id">Account id</param>
        /// <returns>Account</returns>
        [HttpGet]
        public async Task<ActionResult<List<AccountResponse>>> GetAllAccounts(string domain = null)
        {
            var accounts = await _repository.GetAllAsync();

            if (domain != null)
            {
               accounts = accounts.Where(c => c.Domain == domain).ToList();
            }

            var result =  (_mapper.ProjectTo<AccountResponse>(accounts?.AsQueryable())).ToList();

            result = result.OrderBy(c => c.Domain).ThenBy(c => c.UserName).ToList();

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


        /// <summary>
        /// Delete account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAccountAsync(Guid id, AccountRequest request)
        {
            try
            {
                var existingAccount = (await _repository.GetWithIncludeAsync(c =>
                        c.Domain.ToLower() == request.Domain.ToLower() && c.UserName == request.UserName && c.Id != id)).ToList();


                if (existingAccount.Count > 0)
                    return Forbid("Account with same name and domain exists.");

                var account = _mapper.Map<Account>(request);

                await _repository.UpdateAsync(id, account);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest($"Cannot delete account. {e.Message}");
            }
        }

        /// <summary>
        /// Delete account by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAccountAsync(Guid id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest($"Cannot delete account. {e.Message}");
            }
        }



    }
}
