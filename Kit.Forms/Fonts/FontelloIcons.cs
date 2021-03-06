﻿using System;
using System.Collections.Generic;
using System.Text;
using Kit.Forms.Fonts;
using Xamarin.Forms;

[assembly: ExportFont("kiticons_4.ttf", Alias = FontelloIcons.Font)]
namespace Kit.Forms.Fonts
{
    public static class FontelloIcons
    {
        public const string Ok = "\uE803";
        public const string Cross = "\uE804";
        public const string Camera = "\uE800";
        public const string RightArrow = "\uF105";
        public const string ThreeDots = "\uF0C9";
        public const string Font = "KitFontIcons";
    }
}
