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
            int blockSize = 2 * 1024;
            int bytesRead;
            byte[] buffer = new byte[blockSize];
            HashAlgorithm hashFunc = HashAlgorithm.Create("SHA1");

            using (Stream stream = File.OpenRead(filePath))
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    hashFunc.TransformBlock(buffer, 0, bytesRead, null, 0);
                }
                hashFunc.TransformFinalBlock(buffer, 0, 0);
            }

            sha1 = BitConverter.ToString(hashFunc.Hash).ToLower().Replace("-", String.Empty);
            hashFunc.Clear();
            buffer = null;
            hashFunc = null;

            return sha1;
        }
    }
}
