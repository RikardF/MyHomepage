using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using My.Web.Services;
using My.Core.Interfaces;
using My.Core.Models;

namespace My.Web.Pages
{
    public partial class BlogComponent : ComponentBase
    {
        [CascadingParameter]
        private ConnectionParams connectionParams { get; set; }
        [Inject]
        private IBlogService _blogService { get; set; }
        [Inject]
        private IDecoderEncoderService _decoderEncoderService { get; set; }
        [Inject]
        private ILikeService _likeService { get; set; }
        protected List<BlogLike> Likes;
        protected List<BlogContent> BlogContents = new List<BlogContent>();
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Likes = await _likeService.GetBlogLikes();
            await LoadBlog();
        }

        private async Task LoadBlog()
        {
            BlogContents = await _blogService.ReadContent();
            foreach (BlogContent item in BlogContents)
            {
                item.Content = _decoderEncoderService.HTMLDecode(item.Content);
                item.Title = _decoderEncoderService.Decode(item.Title);
                item.Likes = Likes.Where(l => l.BlogContentId == item.Id).Count();
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected bool LikeButtonState(string Id)
        {
            if (Likes.Where(i => i.BlogContentId == Id).Count() > 0) return true;
            else return false;
        }

        protected async Task LikeContent(string id)
        {
            await _likeService.AddBlogLike(new BlogLike {
                UserIP = connectionParams.ConnectionIP,
                BlogContentId = id
            });
            await OnInitializedAsync();
        }
    }
}
