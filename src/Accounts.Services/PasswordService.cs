using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Accounts.Core.Abstractions.Services;
using Accounts.Core.Domain.AccountsManagement;
using Accounts.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Accounts.Services
{
    public class PasswordService : IPasswordService
    {
        private int _requiredLength;

        public PasswordService()
        {
            _requiredLength = 6;
        }

        public string Decrypt(string value)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string value)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> ValidateAsync(string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (String.IsNullOrEmpty(password) || password.Length < _requiredLength)
            {
                errors.Add(new IdentityError
                {
                        Description = $"Minimal lenght password is {_requiredLength}"
                });
            }
            string pattern = "/(?=.*[0-9])(?=.*[!@#$%^&*])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&*]{6,}/g";

            if (!Regex.IsMatch(password, pattern))
            {
                errors.Add(new IdentityError
                {
                        Description = "Password should has numbers, letters and special symbols"
                });
            }

            var result = errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());

            return await Task.FromResult(result);
        }
    }
}