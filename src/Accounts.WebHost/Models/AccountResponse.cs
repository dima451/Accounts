using System;

namespace Accounting.WebHost.Models
{
    public class AccountResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
    }
}