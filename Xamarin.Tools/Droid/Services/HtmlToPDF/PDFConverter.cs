﻿using Android.Views;
using Plugin.Xamarin.Tools.Shared.Services;
using Plugin.Xamarin.Tools.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using static Plugin.Xamarin.Tools.Shared.Services.PDFToHtml;

namespace Plugin.Xamarin.Tools.Droid.Services.HtmlToPDF
{
    internal class PDFConverter : IPDFConverter
    {
        public void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml)
        {
            try
            {
                var webpage = new Android.Webkit.WebView(Android.App.Application.Context);
                webpage.Settings.JavaScriptEnabled = true;

#pragma warning disable CS0618 // Type or member is obsolete
                webpage.DrawingCacheEnabled = true;
#pragma warning restore CS0618 // Type or member is obsolete

                webpage.SetLayerType(LayerType.Software, null);
                webpage.Layout(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight);
                webpage.LoadData(_PDFToHtml.HTMLString, "text/html; charset=utf-8", "UTF-8");
                webpage.SetWebViewClient(new WebViewCallBack(_PDFToHtml));
            }
            catch(Exception ex)
            {
                SQLHelper.Log.LogMe(ex, "ConvertHTMLtoPDF");
                _PDFToHtml.Status = PDFEnum.Failed;
            }
        }
    }
}
