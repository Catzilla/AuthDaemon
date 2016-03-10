using System;
using System.Security.Cryptography;
using System.Text;

namespace AuthDaemon.Net.Security
{
    public class MD5Hash
    {
        public byte[] GetHash(byte[] login, byte[] authData, byte[] key)
        {
            return new HMACMD5(authData).ComputeHash(key);
        }
        public byte[] GetKey(byte[] login, byte[] hash, byte[] key)
        {
            var loginmd5 = new HMACMD5(login);

            byte[] hash02 = new byte[key.Length + hash.Length];

            Buffer.BlockCopy(hash, 0, hash02, 0, hash.Length);
            Buffer.BlockCopy(key, 0, hash02, hash.Length, key.Length);

            return loginmd5.ComputeHash(hash02);
        }
    }
}
