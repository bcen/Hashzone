using System;
using System.IO;
using System.Security.Cryptography;

namespace Hashzone.Util
{
    public class HashUtil
    {
        public static string HashFile(string filePath)
        {
            string sha1 = String.Empty;
            HashAlgorithm hashFunc = HashAlgorithm.Create("SHA1");

            using (Stream stream = new BufferedStream(File.OpenRead(filePath)))
                hashFunc.ComputeHash(stream);

            sha1 = BitConverter.ToString(hashFunc.Hash).ToLower().Replace("-", String.Empty);
            hashFunc.Clear();
            hashFunc = null;

            return sha1;
        }
    }
}
