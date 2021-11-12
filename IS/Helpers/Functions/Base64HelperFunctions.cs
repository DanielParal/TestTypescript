
using System;

namespace IS.Helpers.Functions
{
    public static class Base64HelperFunctions
    {
        public static string Base64Decode(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('_', '/').Replace('-', '+');
            switch (base64EncodedData.Length % 4)
            {
                case 2: base64EncodedData += "=="; break;
                case 3: base64EncodedData += "="; break;
            }
            var decode = Convert.FromBase64String(base64EncodedData);
            var decodedToken = System.Text.Encoding.Default.GetString(decode);
            return decodedToken;
        }
    }
}
