using Codeifier.OrangeCMS.Domain.Providers;
using Codeifier.OrangeCMS.Repositories;
using OrangeCMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace OrangeCMS.Application.Providers
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IDbContextScope dbContextScope;
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 1000;
        public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 1;
        public const int PBKDF2_INDEX = 2;

        public IdentityProvider(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
        }

        public virtual User User
        {
            get
            {
                using (var dbContext = dbContextScope.CreateDbContext())
                {
                    return dbContext.Users.First();
                }
            }
        }

        public User Authenticate(string username, string password)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                var user = dbContext.Users.FirstOrDefault(x => x.UserName == username);
                if (user == null) return null;
                return ValidatePassword(password, user.Password)? user : null;
            }
        }

        public string CreateHash(string password)
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);
            var hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return PBKDF2_ITERATIONS + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        public bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[ITERATION_INDEX]);
            var salt = Convert.FromBase64String(split[SALT_INDEX]);
            var hash = Convert.FromBase64String(split[PBKDF2_INDEX]);
            var testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }
        
        private bool SlowEquals(IList<byte> a, IList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;
            for (var i = 0; i < a.Count && i < b.Count; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) {IterationCount = iterations};
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
