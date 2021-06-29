using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using My.Web.Pages;
using My.Core.Models;
using Microsoft.AspNetCore.Http;
using My.Core.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using My.Core.Services;

namespace My.Web.Layouts
{
    public partial class MyLayoutComponent : LayoutComponentBase
    {
        [Inject]
        private UserRoleService _userRoleService { get; set; }
        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }
        protected RadzenBody body0;
        protected RadzenSidebar sidebar0;
        [Parameter]
        public ConnectionParams connectionParams { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            connectionParams = _userRoleService.DetermineUserRole(_httpContextAccessor.HttpContext.Connection?.RemoteIpAddress.ToString());
            
            await base.SetParametersAsync(parameters);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) await SidebarToggle0Click(null);
            base.OnAfterRender(firstRender);
        }

        protected async System.Threading.Tasks.Task SidebarToggle0Click(dynamic args)
        {
            await InvokeAsync(() => { sidebar0.Toggle(); });

            await InvokeAsync(() => { body0.Toggle(); });
        }




    }
}
