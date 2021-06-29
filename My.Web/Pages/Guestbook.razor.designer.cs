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
using My.Core.Models;
using My.Core.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net;
// using Microsoft.AspNetCore.Http;

namespace My.Web.Pages
{
    public partial class GuestbookComponent : ComponentBase
    {
        [CascadingParameter]
        private ConnectionParams connectionParams { get; set; }
        public bool PostSuccess { get; set; }
        [Inject]
        private NavigationManager _navigationManager { get; set; }
        [Inject]
        private IGuestbookService _guestbookService { get; set; }
        [Inject]
        private IDecoderEncoderService _decoderEncoderService { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        public bool ShowNew { get; set; }
        protected List<GuestbookContent> GuestbookContents = new List<GuestbookContent>();
        protected GuestbookContent NewContent = new GuestbookContent();
        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            await LoadGuestbook();
            PostSuccess = true;
            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/guestbookHub"))
            .Build();
            _hubConnection.On<GuestbookContent>("Updated", async (content) =>
            {
                content.HtmlContent = _decoderEncoderService.HTMLDecode(content.HtmlContent);
                content.Name = _decoderEncoderService.Decode(content.Name);
                GuestbookContents.Add(content);
                await InvokeAsync(() => StateHasChanged());
            });
            await _hubConnection.StartAsync();
        }

        private async Task LoadGuestbook()
        {
            GuestbookContents = await _guestbookService.ReadContent();
            foreach (GuestbookContent item in GuestbookContents)
            {
                item.HtmlContent = _decoderEncoderService.HTMLDecode(item.HtmlContent);
                item.Name = _decoderEncoderService.Decode(item.Name);
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public EventCallback Show()
        {
            ShowNew = !ShowNew;
            return EventCallback.Empty;
        }

        public async Task<EventCallback> PostGuestbookContent()
        {
            if (GuestbookContents.Where(i => i.Id == connectionParams.ConnectionIP).Where(d => d.DateAdded.AddHours(1) > DateTime.Now).Count() > 0)
            {
                PostSuccess = false;
                return EventCallback.Empty;
            }
            NewContent.Approved = true;
            NewContent.DateAdded = DateTime.Now;
            NewContent.Id = connectionParams.ConnectionIP;
            NewContent.HtmlContent = _decoderEncoderService.Encode(NewContent.HtmlContent);
            NewContent.Name = _decoderEncoderService.Encode(NewContent.Name);
            await _guestbookService.WriteContent(NewContent);
            await Send();
            NewContent = new GuestbookContent();
            Show();
            Reload();

            return EventCallback.Empty;
        }

        async Task Send() =>
        await _hubConnection.SendAsync("Send", NewContent);
    }
}
