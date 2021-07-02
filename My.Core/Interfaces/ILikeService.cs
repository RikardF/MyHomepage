using System.Collections.Generic;
using System.Threading.Tasks;
using My.Core.Models;

namespace My.Core.Interfaces
{
    public interface ILikeService
    {
        Task<List<BlogLike>> GetBlogLikes();
        Task AddBlogLike(BlogLike like);
    }
}