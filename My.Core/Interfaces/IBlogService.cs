using System.Collections.Generic;
using System.Threading.Tasks;
using My.Core.Models;

namespace My.Core.Interfaces
{
    public interface IBlogService
    {
        List<BlogContent> BlogContents { get; set; }
        Task WriteContent(BlogContent blogContent);
        Task<List<BlogContent>> ReadContent();
        Task UpdateContent(List<BlogContent> blogContents);
    }
}