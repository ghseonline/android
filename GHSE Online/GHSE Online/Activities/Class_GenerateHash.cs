using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Java.Net;
using Java.Util;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace GHSE_Online
{
    public class generateHash
    {
        public static string SHA512StringHash(String input)
        {
            SHA512 shaM = new SHA512Managed();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaM.ComputeHash(Encoding.UTF8.GetBytes(input));
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();
            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            // Return the hexadecimal string.
            input = sBuilder.ToString();
            return (input);
        }
    }
}