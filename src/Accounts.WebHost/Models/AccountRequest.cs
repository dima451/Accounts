﻿using System;

namespace Accounting.WebHost.Models
{
    public class AccountRequest
    {
        public string UserName { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
    }
}