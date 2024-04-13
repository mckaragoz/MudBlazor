// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace MudBlazor
{
    public interface IThemeService
    {
        public void Attach(MudThemingProvider provider);
        public void Detach();
        public void SetDarkMode(bool darkMode);
        public bool GetDarkMode();
        public Task<bool> SetSystemPreference();
    }
}
