using System;
using System.Collections.Generic;

namespace OrangeCMS.Domain
{
    public class Roles
    {
        public static List<String> All = new List<string>(new[] {"Collector", "Standard", "Administrator"});

        public static string Collector = All[0];

        public static string Standard = All[1];

        public static string Administrator = All[2];
    }
}
