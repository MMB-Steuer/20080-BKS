using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MMBServer
{
    internal static class Helper
    {
        public static string encryptPwd(string pwd)
        {
            byte[] hash;
            string salt = "_$J;:g+3TjrBQ!R:w36]sM5-$G#ps4Rpv?L&X@czdJ";
            pwd = pwd + salt;
            HashAlgorithm algorithm = SHA256.Create();
            hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash) sb.Append(b.ToString("X2"));

            return sb.ToString(); ;
        }

 


    }
}
