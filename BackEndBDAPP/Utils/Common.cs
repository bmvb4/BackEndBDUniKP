using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BackEndBDAPP.Utils
{
    public class Common
    {
        //Creating salt
        public static byte[] GetRandomSalt(int length) {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[length];
            random.GetNonZeroBytes(salt);
            return salt;
        }
        //Hashing password with salt
        public static byte[] SaltHashPassword(byte[] password, byte[] salt) {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] PlainTextWithSaltByte = new byte[password.Length + salt.Length];
            for (int i = 0; i < password.Length; i++)
                PlainTextWithSaltByte[i] = password[i];
            for (int i = 0; i < salt.Length; i++)
                PlainTextWithSaltByte[password.Length+i] = salt[i];
            return algorithm.ComputeHash(PlainTextWithSaltByte);
        }
    }
}
