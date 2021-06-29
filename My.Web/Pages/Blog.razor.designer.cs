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
        [Inject]
        private IBlogService _blogService { get; set; }
        [Inject]
        private IDecoderEncoderService _decoderEncoderService { get; set; }
        protected List<BlogContent> BlogContents = new List<BlogContent>();
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadBlog();
        }

        private async Task LoadBlog()
        {
            BlogContents = await _blogService.ReadContent();
            foreach (BlogContent item in BlogContents)
            {
                item.Content = _decoderEncoderService.HTMLDecode(item.Content);
                item.Title = _decoderEncoderService.Decode(item.Title);
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
    }
}
