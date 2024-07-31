using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace PokeApp.Presentation.API.Services
{
    public static class SanitizeInputService
    {
        public static string SanitizeInput(string input)
        {
            //string res = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(input));
            //return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(res));

            string encodedValue = System.Net.WebUtility.UrlEncode(input);
            return System.Net.WebUtility.UrlDecode(encodedValue);
        }
    }
}
