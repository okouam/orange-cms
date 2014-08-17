﻿namespace OrangeCMS.Domain
{
    public class User 
    {
        public long Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public Client Client { get; set; }
    }
}
