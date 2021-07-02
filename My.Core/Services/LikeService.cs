using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using My.Core.Interfaces;
using My.Core.Models;

namespace My.Core.Services
{
    public class LikeService : ILikeService
    {
        private FileInfo _contentFile = new FileInfo("../My.Core/Data/BlogLikes.json");
        public List<BlogLike> BlogLikes { get; set; }
        private string _jsonString;
        private string[] _jsonArray;
        private JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };
        public async Task<List<BlogLike>> GetBlogLikes()
        {
            using (var reader = new StreamReader(_contentFile.Open(FileMode.OpenOrCreate, FileAccess.Read)))
            {
                BlogLikes = new List<BlogLike>();
                _jsonString = await reader.ReadToEndAsync();
                if (_jsonString != string.Empty) 
                {
                    _jsonArray = _jsonString.Split('}');
                    for (int i = 0; i < (_jsonArray.Length - 1); i++)
                    {
                        if (_jsonArray[i] != string.Empty) BlogLikes.Add(JsonSerializer.Deserialize<BlogLike>(_jsonArray[i] + '}'));                        
                    }
                }
            }
            return BlogLikes;
        }
        public async Task AddBlogLike(BlogLike like)
        {
            using (var writer = new StreamWriter(_contentFile.Open(FileMode.Append, FileAccess.Write)))
            {
                await writer.WriteLineAsync(JsonSerializer.Serialize(like, options));
                await writer.FlushAsync();
            }
        }
    }
}