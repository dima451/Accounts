using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Abstractions.Services;
using Accounts.Core.Domain.AccountsManagement;
using Accounts.WebHost.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Accounts.WebHost.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IEntityService<Account> _entityService;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;

        public AccountController(IEntityService<Account> entityService, ILogger<AccountController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _entityService = entityService;
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
                var account = _mapper.Map<Account>(request);

                var result = await _entityService.CreateAsync(account);
                
                if (String.IsNullOrEmpty(result))
                    return Ok();
                else
                    return BadRequest(result);
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
            var accounts = await _entityService.GetAllAsync();

            var result =  (_mapper.ProjectTo<AccountResponse>(accounts?.AsQueryable())).ToList();

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
            var account = await _entityService.GetByIdAsync(id);

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
                var account = _mapper.Map<Account>(request);

                var result = await _entityService.UpdateAsync(id, account);

                if (String.IsNullOrEmpty(result))
                    return Ok();
                else
                    return BadRequest(result);
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
                await _entityService.DeleteAsync(id);
                
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
