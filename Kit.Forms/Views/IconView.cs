﻿using Kit.Forms.Fonts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Kit.Forms.Views
{
    public class IconView : ContentView
    {
        public virtual string Icon => FontelloIcons.Ok;
        public virtual string Title => string.Empty;

        public static readonly BindableProperty ToolbarItemProperty =
            BindableProperty.Create(
            propertyName: nameof(ToolbarItem),
            returnType: typeof(ToolbarItem),
            declaringType: typeof(ToolbarItem),
            defaultValue: null);

        public ToolbarItem ToolbarItem
        {
            get => (ToolbarItem)GetValue(ToolbarItemProperty);
            set
            {
                SetValue(ToolbarItemProperty, value);
                OnPropertyChanged();
            }
        }

        public IconView() : base()
        {
        }
    }
}