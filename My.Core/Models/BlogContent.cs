using System;
using System.Text.Json.Serialization;

namespace My.Core.Models
{
    public class BlogContent
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("dateAdded")]
        public DateTime DateAdded { get; set; }
        [JsonPropertyName("likes")]
        public int Likes { get; set; }
    }
}