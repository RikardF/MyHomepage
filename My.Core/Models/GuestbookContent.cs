using System;
using System.Text.Json.Serialization;

namespace My.Core.Models
{
    public class GuestbookContent
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("dateAdded")]
        public DateTime DateAdded { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("approved")]
        public bool Approved { get; set; }
        [JsonPropertyName("htmlContent")]
        public string HtmlContent { get; set; }
    }
}