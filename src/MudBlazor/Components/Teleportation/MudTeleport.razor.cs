using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor.Extensions;
using MudBlazor.Utilities;

namespace MudBlazor
{
    public partial class MudTeleport : MudComponentBase, IAsyncDisposable
    {
        [Inject] IJSRuntime JSRuntime { get; set; }

        [Parameter] public string To { get; set; }
        [Parameter] public bool Disabled { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        private ElementReference _ref;

        private string _to;
        private bool? _disabled;
        private bool _mustUpdate = true;

        private IJSObjectReference _module;

        protected override void OnInitialized()
        {
            _to = To;
            _disabled = Disabled;
        }

        protected override void OnParametersSet()
        {
            // if `To` or `Disabled` has changed we must update the teleport
            if (!To.Equals(_to) || !Disabled.Equals(_disabled))
            {
                _to = To;
                _disabled = Disabled;
                _mustUpdate = true;
            }
            else
            {
                _mustUpdate = false;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/MudBlazor/TScript/teleport.js");
            }

            if (_mustUpdate)
            {
                await Update();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Update()
        {
            if (_module is not null && !Disabled)
            {
                await _module.InvokeVoidAsync("teleport", _ref, To);
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                if (_module is not null)
                {
                    // FIXME: for some reason a tooltip inside a dialog keeps visible even if the
                    // dialog was closed. The only way to remove it from the DOM is using JSInterop
                    // since setting Disabled to true does not works. Probably there's a better way
                    // to handle it.
                    await _module.InvokeVoidAsync("removeFromDOM", _ref);
                    await _module.DisposeAsync();
                }
            }
            catch (JSDisconnectedException) { }
            catch (TaskCanceledException) { }
            // GC.SuppressFinalize(this);
        }
    }
}
