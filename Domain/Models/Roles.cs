using System.Collections.Generic;

namespace CodeKinden.OrangeCMS.Domain.Models
{
    public class Roles
    {
        public static List<string> All = new List<string>(new[] { "Standard", "Administrator", "System"});
        
        public static string Standard = All[0];

        public static string Administrator = All[1];

        public static string System = All[2];

        public static string FromEnum(Role role)
        {
            return All[(int) role];
        }
    }

    public enum Role
    {
        Standard = 0,
        Administrator = 1,
        System = 2
    }
}
