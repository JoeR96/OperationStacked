namespace OperationStacked.Entities
{
    public class UserRole : BaseEntity
    {
        public int UserId { get; set; }
        public User UserAccount { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}