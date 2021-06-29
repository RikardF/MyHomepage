using System.Net;
using My.Core.Interfaces;

namespace My.Core.Services
{
    public class DecoderEncoderService : IDecoderEncoderService
    {
        public string Decode(string input)
        {
            return WebUtility.UrlDecode(input);
        }

        public string Encode(string input)
        {
            return WebUtility.UrlEncode(input);
        }

        public string HTMLDecode(string input)
        {
            return WebUtility.HtmlDecode(WebUtility.UrlDecode(input));
        }

        public string HTMLEncode(string input)
        {
            return WebUtility.UrlEncode(WebUtility.HtmlEncode(input));
        }
    }
}