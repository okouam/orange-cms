namespace CodeKinden.OrangeCMS.Domain.Models
{
    public class User 
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public bool IsAdministrator => Role.Equals(Roles.Administrator);

        public bool IsStandard => Role.Equals(Roles.Standard);

        public bool IsSystem => Role.Equals(Roles.System);
    }
}
