using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using My.Core.Interfaces;
using My.Core.Models;
using System.Linq;
using System;
using System.Net;

namespace My.Core.Services
{
    public class GuestbookService : IGuestbookService
    {
        public List<GuestbookContent> GuestbookContents { get; set; }
        private GuestbookContent _tempContent { get; set; }
        private FileInfo _contentFile = new FileInfo("../My.Core/Data/GuestbookContent.json");
        private string _jsonString;
        private string[] _jsonArray;
        private JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        public async Task WriteContent(GuestbookContent content)
        {
            using (var writer = new StreamWriter(_contentFile.Open(FileMode.Append, FileAccess.Write)))
            {
                await writer.WriteLineAsync(JsonSerializer.Serialize(content, options));
                await writer.FlushAsync();
            }
            
        }

        public async Task UpdateContent(List<GuestbookContent> guestbook)
        {
            using(var writer = new StreamWriter(_contentFile.Open(FileMode.Open, FileAccess.Write)))
            {
                foreach (GuestbookContent content in guestbook)
                {
                    await writer.WriteLineAsync(JsonSerializer.Serialize(content, options));
                }
                await writer.FlushAsync();
            }
        }

        public async Task<List<GuestbookContent>> ReadContent()
        {
            using (var reader = new StreamReader(_contentFile.Open(FileMode.OpenOrCreate, FileAccess.Read)))
            {
                GuestbookContents = new List<GuestbookContent>();
                _jsonString = await reader.ReadToEndAsync();
                if (_jsonString != string.Empty) 
                {
                    _jsonArray = _jsonString.Split('}');
                    for (int i = 0; i < (_jsonArray.Length - 1); i++)
                    {
                        if (_jsonArray[i] != string.Empty) _tempContent = JsonSerializer.Deserialize<GuestbookContent>(_jsonArray[i] + '}');
                        GuestbookContents.Add(_tempContent);                 
                    }
                }
            }
            
            return GuestbookContents;
        }
    }
}