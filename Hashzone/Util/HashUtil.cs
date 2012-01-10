using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Hashzone.Util
{
    public class HashUtil
    {
        public static string HashFile(string filePath)
        {
            return HashFile(filePath, "SHA1");
        }

        public static string HashFile(string filePath, string hashName)
        {
            string hashmsg = String.Empty;
            HashAlgorithm hashFunc = HashAlgorithm.Create(hashName);

            using (Stream stream = File.OpenRead(filePath))
                hashFunc.ComputeHash(stream);

            hashmsg = BitConverter.ToString(hashFunc.Hash).Replace("-", String.Empty).ToLower();
            hashFunc.Clear();
            hashFunc = null;

            return hashmsg;
        }

        public static string HashString(string src, string hashName)
        {
            string hashmsg = String.Empty;
            
            using (HashAlgorithm hashFunc = HashAlgorithm.Create(hashName))
            {
                hashFunc.ComputeHash(Encoding.UTF8.GetBytes(src.ToCharArray()));
                hashmsg = BitConverter.ToString(hashFunc.Hash).Replace("-", String.Empty).ToLower();
            }

            return hashmsg;
        }
    }
}
