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
using Microsoft.AspNetCore.Mvc;

namespace My.Web.Pages
{
    public partial class WebAdministrationComponent : ComponentBase
    {
        [CascadingParameter]
        private ConnectionParams connectionParams { get; set; }
        [Inject]
        private NavigationManager _navManager { get; set; }
        [Inject]
        private IBlogService _blogService { get; set; }
        [Inject]
        private IDecoderEncoderService _decoderEncoderService { get; set; }
        protected List<BlogContent> BlogContents = new List<BlogContent>();
        [Inject]
        private IGuestbookService _guestbookService { get; set; }
        protected List<GuestbookContent> GuestbookContents = new List<GuestbookContent>();
        protected BlogContent NewBlogContent = new BlogContent();
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        protected override async Task OnInitializedAsync()
        {    
            if (connectionParams.UserRole != Core.Constants.Role.Admin) _navManager.NavigateTo("/");  
            await LoadLists(); 
        }

        private async Task LoadLists()
        {
            BlogContents = await _blogService.ReadContent();
            foreach (BlogContent item in BlogContents)
            {
                item.Title = _decoderEncoderService.Decode(item.Title);
                item.Content = _decoderEncoderService.HTMLDecode(item.Content);
            }
            GuestbookContents = await  _guestbookService.ReadContent();
            foreach (GuestbookContent item in GuestbookContents)
            {
                item.Name = _decoderEncoderService.Decode(item.Name);
                item.HtmlContent = _decoderEncoderService.HTMLDecode(item.HtmlContent);
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        protected async Task UpdateGuestbook()
        {
            foreach (GuestbookContent item in GuestbookContents)
            {
                item.HtmlContent = _decoderEncoderService.HTMLEncode(item.HtmlContent);
                item.Name = _decoderEncoderService.Encode(item.Name);
            }
            await _guestbookService.UpdateContent(GuestbookContents);
            await OnInitializedAsync();
        }

        protected async Task UpdateBlog()
        {
            foreach (BlogContent item in BlogContents)
            {
                item.Content = _decoderEncoderService.HTMLEncode(item.Content);
                item.Title = _decoderEncoderService.Encode(item.Title);
            }
            await _blogService.UpdateContent(BlogContents);
            await OnInitializedAsync();
        }

        protected async Task PostBlogContent()
        {
            NewBlogContent.Content = _decoderEncoderService.Encode(NewBlogContent.Content);
            NewBlogContent.Title = _decoderEncoderService.Encode(NewBlogContent.Title);
            NewBlogContent.DateAdded = DateTime.Now;
            NewBlogContent.Id = Guid.NewGuid().ToString();
            NewBlogContent.Likes = 0;
            await _blogService.WriteContent(NewBlogContent);
            NewBlogContent = new BlogContent();
            await OnInitializedAsync();
        }
    }
}
