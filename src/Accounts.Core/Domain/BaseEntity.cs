using System;

namespace Accounts.Core.Domain
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}