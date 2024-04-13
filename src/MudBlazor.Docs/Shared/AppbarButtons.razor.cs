// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Docs.Enums;
using MudBlazor.Docs.Extensions;
using MudBlazor.Docs.Models;
using MudBlazor.Docs.Services;
using MudBlazor.Docs.Services.Notifications;
using MudBlazor.Docs.Services.UserPreferences;

namespace MudBlazor.Docs.Shared;

public partial class AppbarButtons
{
    [Inject] private INotificationService NotificationService { get; set; }
    [Inject] private LayoutService LayoutService { get; set; }
    private IDictionary<NotificationMessage, bool> _messages = null;
    private bool _newNotificationsAvailable = false;
    private DarkLightMode _darkModeStatus = DarkLightMode.System;

    private async Task MarkNotificationAsRead()
    {
        await NotificationService.MarkNotificationsAsRead();
        _newNotificationsAvailable = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _newNotificationsAvailable = await NotificationService.AreNewNotificationsAvailable();
            _messages = await NotificationService.GetNotifications();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public async Task ToggleDarkMode()
    {
        switch (_darkModeStatus)
        {
            case DarkLightMode.System:
                ThemeService.SetDarkMode(false);
                _darkModeStatus = DarkLightMode.Light;
                break;
            case DarkLightMode.Light:
                ThemeService.SetDarkMode(true);
                _darkModeStatus = DarkLightMode.Dark;
                break;
            case DarkLightMode.Dark:
                await ThemeService.SetSystemPreference();
                _darkModeStatus = DarkLightMode.System;
                break;
        }
    }

}
