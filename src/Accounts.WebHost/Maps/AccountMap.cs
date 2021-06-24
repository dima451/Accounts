using Accounting.Core.Entity.AccountingManagement;
using Accounting.WebHost.Models;
using AutoMapper;

namespace Accounting.WebHost.Maps
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