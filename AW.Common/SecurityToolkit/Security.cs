using System;
using System.Security.Cryptography;
using System.Text;

namespace AW.Common.SecurityToolkit
{
    public class Security 
    {
        public string GetSha256Hash(string input)
        {
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = SHA256.Create().ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
