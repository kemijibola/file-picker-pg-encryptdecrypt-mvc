using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICMS.Lite.Repository.Utilities
{
    public class HashUtil
    {
        public static string Hash(string wordToHash)
        {
            string key = "";
            System.Security.Cryptography.SHA512 hashtool = new System.Security.Cryptography.SHA512Managed();
            Byte[] strHash = System.Text.Encoding.UTF8.GetBytes(string.Concat(wordToHash));
            Byte[] encryptedHash = hashtool.ComputeHash(strHash);
            hashtool.Clear();
            StringBuilder hexString = new StringBuilder();
            for (int i = 0; i < encryptedHash.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", encryptedHash[i]));
            }
            key = hexString.ToString();
            return key;
        }

        private static string key = "12345";
        public static string DecryptStringValue(string value)
        {
            var decrypted = new CrytoLibrary.SymmetricEncryption(CrytoLibrary.SymmetricEncryption.EncryptionType.DES)
                .Decrypt(value, key);

            return decrypted;
        }

        public static string EncryptStringValue(string value)
        {
            var encrypted = new CrytoLibrary.SymmetricEncryption(CrytoLibrary.SymmetricEncryption.EncryptionType.DES)
                .Encrypt(value, key);

            return encrypted;
        }
    }
}
