namespace Jeremy.IdentityCenter.Database.Constants
{
    public static class TableName
    {
        private const string PreFix = "idc";

        public static readonly string IdcUsers = $"{PreFix}Users";
        public static readonly string IdcUserLogins = $"{PreFix}UserLogins";
        public static readonly string IdcUserClaims = $"{PreFix}UserClaims";
        public static readonly string IdcUserTokens = $"{PreFix}UserTokens";
        public static readonly string IdcUserRoles = $"{PreFix}UserRoles";
        public static readonly string IdcRoles = $"{PreFix}Roles";
        public static readonly string IdcRoleClaims = $"{PreFix}RoleClaims";
    }
}
