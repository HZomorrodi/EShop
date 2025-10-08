using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Common.Extensions
{
    public static class PasswordHelper
    {
        //private static readonly string secretKey2 = "evicequffyxbdsqfqwxzbqweulwbkpkrlqfujmupkqqmwvyxyraidvqywtrensxw";
        private static readonly byte[] secretKey =
        [
            187,
            95,
            128,
            177,
            51,
            198,
            114,
            56,
            95,
            109,
            93,
            3,
            51,
            79,
            204,
            146,
            125,
            78,
            34,
            64,
            147,
            195,
            210,
            63,
            223,
            45,
            19,
            13,
            200,
            5,
            112,
            238,
            1,
            119,
            5,
            106,
            130,
            136,
            1,
            191,
            205,
            187,
            29,
            244,
            29,
            185,
            213,
            226,
            255,
            158,
            202,
            195,
            189,
            56,
            97,
            177,
            71,
            250,
            140,
            191,
            108,
            30,
            42,
            20
        ];

        public static string ToHash(this string input)
        {
            //var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey2);
            byte[] messageBytes = Encoding.UTF8.GetBytes(input);
            using HMACSHA256 hmac = new(secretKey);
            //using var hmac = new HMACSHA256(secretKeyBytes);
            byte[] hashedMessage = hmac.ComputeHash(messageBytes);
            return Convert.ToBase64String(hashedMessage);
        }

        private static void GenerateKey()
        {
            var numbers = new List<string>();
            for (int counter = 0; counter < 64; counter++)
            {
                var rnd = new Random().Next(0, 256);
                numbers.Add(rnd + ",");
            }

            var path = @"D:\a.txt";
            File.AppendAllLines(path, numbers);
        }
    }
}
