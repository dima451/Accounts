using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Core.Abstractions.Repository;
using Accounts.Core.Abstractions.Services;
using Accounts.Core.Domain.AccountsManagement;
using Accounts.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Accounts.Services
{
    public class AccountService : IEntityService<Account>
    {
        private readonly IRepository<Account> _repository;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public AccountService(IRepository<Account> repository, IPasswordService passwordService, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _passwordService = passwordService;
        }
        public async Task<string> CreateAsync(Account value)
        {
            string result = String.Empty;

            var existingAccount = (await _repository.GetWithIncludeAsync(c =>
                    c.Domain.ToLower() == value.Domain.ToLower() && c.UserName == value.UserName)).ToList();

            if (existingAccount.Count > 0)
                result += "Account with same name and domain exists.";

            result += await ValidatePassword(value);
            
            if (!String.IsNullOrEmpty(result))
                return result;

            value.Password = _passwordService.Encrypt(value.Password);
            await _repository.CreateAsync(value);

            return result;
        }

        public  async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAsync(params string[] filters)
        {
            var accounts = await _repository.GetAllAsync();

            if (!String.IsNullOrEmpty(filters.FirstOrDefault()))
            {
                accounts = accounts.Where(c => c.Domain == filters.First()).ToList();
            }

            foreach (var account in accounts)
            {
                account.Password = _passwordService.Decrypt(account.Password);
            }

            accounts = accounts.OrderBy(c => c.Domain).ThenBy(c => c.UserName).ToList();

            return accounts;
        }

        public async Task<Account> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);

            result.Password = _passwordService.Decrypt(result.Password);

            return result;
        }

        public async Task<string> UpdateAsync(Guid id, Account value)
        {
            string result = String.Empty;

            var existingAccount = (await _repository.GetWithIncludeAsync(c =>
                    c.Domain.ToLower() == value.Domain.ToLower() && c.UserName == value.UserName && c.Id != id)).ToList();

            if (existingAccount.Count > 0)
                result+="Account with same name and domain exists.";

            result += await ValidatePassword(value);

            if (!String.IsNullOrEmpty(result))
                return result;

            value.Password = _passwordService.Encrypt(value.Password);

            await _repository.UpdateAsync(id, value);

            return result;
        }

        private async Task<string> ValidatePassword(Account account)
        {
            string result = String.Empty;
            
            var passResult = await _passwordService.ValidateAsync(account.Password);

            if (!passResult.Succeeded)
            {
                foreach (var passResultError in passResult.Errors)
                {

                    result += $"\n{passResultError.Description}";
                }
            }

            return result;
        }
    }
}