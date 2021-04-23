using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Services
{
    public class RandomBarcodeGenerator
    {
        public string RandomString(int Length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[Length];
            Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var final = new string(stringChars);
            return final;
        }
    }
}
