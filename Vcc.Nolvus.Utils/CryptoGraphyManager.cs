using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Vcc.Nolvus.Utils
{
    public static class CryptographyManager
    {
        public static string EncryptString(string value)
        {
            string Key = "1F2CAB925HFBBCAE589632FABC547892";
            string IV = "ACtEDg70CcU=";

            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.Key = Convert.FromBase64String(Key);
            TripleDES.IV = Convert.FromBase64String(IV);
            TripleDES.Mode = CipherMode.ECB;
            TripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = TripleDES.CreateEncryptor(TripleDES.Key, TripleDES.IV);

            byt = Encoding.UTF8.GetBytes(value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());


        }

        public static string DecryptString(string value)
        {
            string Key = "1F2CAB925HFBBCAE589632FABC547892";
            string IV = "ACtEDg70CcU=";

            TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
            TripleDES.Key = Convert.FromBase64String(Key);
            TripleDES.IV = Convert.FromBase64String(IV);
            TripleDES.Mode = CipherMode.ECB;
            TripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            ct = TripleDES.CreateDecryptor(TripleDES.Key, TripleDES.IV);

            byt = Convert.FromBase64String(value);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
