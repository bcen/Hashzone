using System;
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
            string sha1 = String.Empty;
            HashAlgorithm hashFunc = HashAlgorithm.Create(hashName);

            using (Stream stream = File.OpenRead(filePath))
                hashFunc.ComputeHash(stream);

            sha1 = BitConverter.ToString(hashFunc.Hash).Replace("-", String.Empty).ToLower();
            hashFunc.Clear();
            hashFunc = null;

            return sha1;
        }
    }
}
