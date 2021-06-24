using System.Threading.Tasks;
using Accounts.Core.Domain.AccountsManagement;
using Microsoft.AspNetCore.Identity;

namespace Accounts.Services.Abstractions
{
    public interface IPasswordService
    {
        string Encrypt(string value);
        string Decrypt(string value);
        Task<IdentityResult> ValidateAsync(string password);
    }
}