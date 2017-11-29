
using System;
using System.Text;

namespace KinniNet.Business.Utils
{
    public static  class SecurityUtils
    {
        public static string CreateShaHash(string cadena)
        {
            System.Security.Cryptography.SHA512Managed hashTool = new System.Security.Cryptography.SHA512Managed();
            Byte[] cadenaAsByte = Encoding.UTF8.GetBytes(cadena);
            Byte[] encryptedBytes = hashTool.ComputeHash(cadenaAsByte);
            hashTool.Clear();
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
