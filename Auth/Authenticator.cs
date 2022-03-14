using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
    public class Authenticator
    {
        public void Authenticate()
        {
            
        }

        public string ComputeSHA256HashAsBase64String(string stringToHash)
        {
            using (var hash = SHA256.Create())
            {
                Byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(stringToHash));
                return Convert.ToBase64String(result);
            }
        }

    }
}
