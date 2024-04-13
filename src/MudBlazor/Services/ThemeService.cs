// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System;
using System.Threading.Tasks;
using MudBlazor.Interfaces;

namespace MudBlazor.Services
{
    public class ThemeService : IThemeService
    {
        public MudThemingProvider? Provider { get; internal set; }

        public void Attach(MudThemingProvider? provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            Provider = provider;
        }

        public void Detach()
        {
            Provider = null;
        }

        public void SetDarkMode(bool darkMode)
        {
            Provider?.SetDarkMode(darkMode);
        }

        public bool GetDarkMode()
        {
            return Provider?.GetDarkMode() ?? false;
        }

        public async Task<bool> SetSystemPreference()
        {
            if (Provider == null)
            {
                return false;
            }
            return await Provider.GetSystemPreference();
        }
    }
}
