﻿using CoreGraphics;
using Kit.Services;
using Kit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using WebKit;
using Kit.Enums;
using Kit.Forms.Services.XFPDF.Helpers.Enums;
using Kit.Forms.Services.XFPDF.Helpers.Interfaces;
using Kit.Forms.Services.XFPDF.Model;
using Kit.iOS.Services.HtmlToPDF;
using Xamarin.Forms;

[assembly: Dependency(typeof(PDFConverter))]
namespace Kit.iOS.Services.HtmlToPDF
{
    public class PDFConverter : IPDFConverter
    {
        public void ConvertHTMLtoPDF(PDFToHtml _PDFToHtml)
        {
            try
            {
                WKWebView webView = new WKWebView(new CGRect(0, 0, (int)_PDFToHtml.PageWidth, (int)_PDFToHtml.PageHeight), new WKWebViewConfiguration());
                webView.UserInteractionEnabled = false;
                webView.BackgroundColor = UIColor.White;
                webView.NavigationDelegate = new WebViewCallBack(_PDFToHtml);
                webView.LoadHtmlString(_PDFToHtml.HTMLString, null);
            }
            catch
            {
                _PDFToHtml.Status = PDFEnum.Failed;
            }
        }
    }
}
