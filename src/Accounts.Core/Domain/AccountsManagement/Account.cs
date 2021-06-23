namespace Accounting.Core.Entity.AccountingManagement
{
    public class Account : BaseEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }
}