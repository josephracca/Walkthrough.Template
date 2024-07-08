using Domain.Common;

namespace Domain
{
    public class User : BaseAuditEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string EmailAddress { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public Guid? Identifier { get; set; }
        public DateTime? IdentifierExpirationDate { get; set; }
    }
}
