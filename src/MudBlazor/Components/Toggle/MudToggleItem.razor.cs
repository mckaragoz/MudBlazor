﻿// Copyright (c) MudBlazor 2021
// MudBlazor licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor.Utilities;

namespace MudBlazor
{

#nullable enable
    public partial class MudToggleItem<T> : MudComponentBase
    {
        private bool _selected;

        protected string Classes => new CssBuilder("mud-toggle-item")
            .AddClass($"mud-theme-{Parent?.Color.ToDescriptionString()}", _selected && string.IsNullOrEmpty(Parent?.SelectedClass))
            .AddClass(Parent?.SelectedClass, _selected && !string.IsNullOrEmpty(Parent?.SelectedClass))
            .AddClass($"mud-toggle-item-{Parent?.Color.ToDescriptionString()}")
            .AddClass("mud-ripple", Parent?.DisableRipple == false)
            .AddClass($"mud-border-{Parent?.Color.ToDescriptionString()} border-solid")
            .AddClass("border-r border-b", Parent?.ShowBorder == true)
            .AddClass("border-l", Parent?.ShowBorder == true && (Parent?.Vertical == true || Parent?.IsFirstItem(this) == true || Parent?.RightToLeft == true))
            .AddClass("border-t", Parent?.ShowBorder == true && (Parent?.Vertical == false || Parent?.IsFirstItem(this) == true))
            .AddClass("rounded-l-xl", Parent is { Rounded: true, Vertical: false } && Parent?.IsFirstItem(this) == true)
            .AddClass("rounded-t-xl", Parent is { Rounded: true, Vertical: true } && Parent?.IsFirstItem(this) == true)
            .AddClass("rounded-r-xl", Parent is { Rounded: true, Vertical: false } && Parent?.IsLastItem(this) == true)
            .AddClass("rounded-b-xl", Parent is { Rounded: true, Vertical: true } && Parent?.IsLastItem(this) == true)
            .AddClass(ItemPadding)
            .AddClass("mud-toggle-item-vertical", Parent?.Vertical == true)
            .AddClass(Class)
            .Build();
        
        protected string TextClassName => new CssBuilder()
            .AddClass(Parent?.TextClass)
            .Build();
        
        protected string IconClassName => new CssBuilder()
            .AddClass(Parent?.IconClass)
            .AddClass("me-2", Parent?.ShowText)
            .Build();

        protected string ItemPadding
        {
            get
            {
                if (Parent?.Vertical == true)
                {
                    if (Parent?.Rounded == true)
                        if (Parent?.IsFirstItem(this) == true)
                            return Parent?.Dense==true ? "px-1 pt-2 pb-1" : "px-2 pt-3 pb-2";
                        else if (Parent?.IsLastItem(this) == true)
                            return Parent?.Dense==true ? "px-1 pt-1 pb-2" : "px-2 pt-2 pb-3";
                        else
                            return Parent?.Dense==true ? "px-1 py-1" : "px-2 py-2";
                    // not rounded 
                    return Parent?.Dense==true ? "px-1 py-1" : "px-2 py-2";
                }
                // horizontal
                if (Parent?.Rounded == true)
                    if (Parent?.IsFirstItem(this) == true)
                        return Parent?.Dense==true ? "ps-2 pe-1 py-1" : "ps-3 pe-2 py-2";
                    else if (Parent?.IsLastItem(this) == true)
                        return Parent?.Dense==true ? "ps-1 pe-2 py-1" : "ps-2 pe-3 py-2";
                    else
                        return Parent?.Dense==true ? "px-1 py-1" : "px-2 py-2";
                // not rounded 
                return Parent?.Dense==true ? "px-1 py-1" : "px-2 py-2";
            }
        }

        [CascadingParameter]
        public MudToggleGroup<T>? Parent { get; set; }

        [Parameter]
        [Category(CategoryTypes.List.Behavior)]
        public T? Value { get; set; }

        [Parameter]
        [Category(CategoryTypes.List.Appearance)]
        public string? Icon { get; set; }
        
        [Parameter]
        [Category(CategoryTypes.List.Appearance)]
        public string? SelectedIcon { get; set; }

        private string? CurrentIcon => IsSelected ? SelectedIcon ?? Icon : Icon;
        
        [Parameter]
        [Category(CategoryTypes.List.Appearance)]
        public string? Text { get; set; }

        [Parameter]
        [Category(CategoryTypes.List.Appearance)]
        public RenderFragment? ChildContent { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent?.Register(this);
        }

        public void SetSelected(bool selected)
        {
            _selected = selected;
            StateHasChanged();
        }

        protected internal bool IsSelected => _selected;

        protected async Task HandleOnClickAsync()
        {
            if (Parent is not null)
            {
                await Parent.ToggleItemAsync(this);
            }
        }

        protected internal bool IsEmpty => string.IsNullOrEmpty(Text) && Value is null;
        
    }
}
