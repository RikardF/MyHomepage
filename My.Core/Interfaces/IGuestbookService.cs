using System.Collections.Generic;
using System.Threading.Tasks;
using My.Core.Models;

namespace My.Core.Interfaces
{
    public interface IGuestbookService
    {
        List<GuestbookContent> GuestbookContents { get; set; }
        Task WriteContent(GuestbookContent content);
        Task UpdateContent(List<GuestbookContent> guestbook);
        Task<List<GuestbookContent>> ReadContent();
    }
}