using System;

namespace Accounting.Core.Entity
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}