using Accounts.Core.Domain.AccountsManagement;
using Accounts.WebHost.Models;
using AutoMapper;

namespace Accounts.WebHost.Maps
{
    public class AccountMap : Profile   
    {
        public AccountMap()
        {
            CreateMap<Account, AccountResponse>();
            CreateMap<AccountRequest, Account>();
        }
    }
}