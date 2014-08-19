﻿using OrangeCMS.Domain;

namespace OrangeCMS.Application.Providers
{
    public interface IIdentityProvider
    {
        User User { get; }

        User Authenticate(string username, string password); 
    }
}