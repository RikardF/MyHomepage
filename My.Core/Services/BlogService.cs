using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using My.Core.Interfaces;
using My.Core.Models;
using System.Linq;
using System;

namespace My.Core.Services
{
    public class BlogService : IBlogService
    {
        public List<BlogContent> BlogContents { get; set; }
        private FileInfo _contentFile = new FileInfo("../My.Core/Data/BlogContent.json");
        private string _jsonString;
        private string[] _jsonArray;
        private JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        public async Task WriteContent(BlogContent content)
        {
            using (var writer = new StreamWriter(_contentFile.Open(FileMode.Append, FileAccess.Write)))
            {
                await writer.WriteLineAsync(JsonSerializer.Serialize(content, options));
                await writer.FlushAsync();
            }
            
        }

        public async Task<List<BlogContent>> ReadContent()
        {
            using (var reader = new StreamReader(_contentFile.Open(FileMode.OpenOrCreate, FileAccess.Read)))
            {
                BlogContents = new List<BlogContent>();
                _jsonString = await reader.ReadToEndAsync();
                if (_jsonString != string.Empty) 
                {
                    _jsonArray = _jsonString.Split('}');
                    for (int i = 0; i < (_jsonArray.Length - 1); i++)
                    {
                        if (_jsonArray[i] != string.Empty) BlogContents.Add(JsonSerializer.Deserialize<BlogContent>(_jsonArray[i] + '}'));                        
                    }
                }
            }
            
            return BlogContents;
        }

        public async Task UpdateContent(List<BlogContent> blogContents)
        {
            using (var writer = new StreamWriter(_contentFile.Open(FileMode.Open, FileAccess.Write)))
            {
                foreach (BlogContent item in blogContents)
                {
                    await writer.WriteLineAsync(JsonSerializer.Serialize(item, options));
                }
                await writer.FlushAsync();
            }
        }
    }
}