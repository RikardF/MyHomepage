using System.Text.Json.Serialization;

namespace My.Core.Models
{
    public class BlogLike
    {
        [JsonPropertyName("blogContentId")]
        public string BlogContentId { get; set; }
        [JsonPropertyName("userIP")]
        public string UserIP { get; set; }
    }
}