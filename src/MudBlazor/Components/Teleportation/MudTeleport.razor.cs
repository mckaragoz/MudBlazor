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
            if (_mustUpdate)
            {
                await Update();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Update()
        {
            if (!Disabled)
            {
                await JSRuntime.InvokeVoidAsync("mudTeleport.teleport", _ref, To);
            }
        }

        public async ValueTask DisposeAsync()
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("mudTeleport.removeFromDOM", _ref);
            }
            catch (JSDisconnectedException) { }
            catch (TaskCanceledException) { }
            catch (InvalidOperationException) { /* it throws in the server side project */ }
        }
    }
}
