namespace Jeremy.IdentityCenter.Database.Models
{
    public class IdentityUserAndUserRole<TUser, TUserRole>
    {
        public TUser User { get; set; }
        public TUserRole UserRole { get; set; }
    }
}
