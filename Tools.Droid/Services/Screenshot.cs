﻿using Android.Graphics;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Tools.Services.Interfaces;

namespace Tools.Droid.Services
{
    public class Screenshot : IScreenshot
    {
        public async Task<byte[]> Capture()
        {
            await Task.Yield();

            var rootView = CrossCurrentActivity.Current.Activity.Window.DecorView.RootView;
            using (var screenshot = Android.Graphics.Bitmap.CreateBitmap(
                rootView.Width,
                rootView.Height,
                Android.Graphics.Bitmap.Config.Argb8888))
            {
                var canvas = new Canvas(screenshot);
                rootView.Draw(canvas);

                using (var stream = new MemoryStream())
                {
                    screenshot.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 90, stream);
                    return stream.ToArray();
                }
            }
        }
    }
}