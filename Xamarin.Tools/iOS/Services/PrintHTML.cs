﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using Plugin.Xamarin.Tools.Shared.Services.Interfaces;
using UIKit;
using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Plugin.Xamarin.Tools.iOS.Services
{
    public class PrintHTML : IPrintHTML
    {
        public async Task<bool> Print(string HTML, string Printer)
        {
            try
            {
                if (Printer != "Microsoft Print to PDF")
                {
                    //if (Forms9Patch.ToPdfService.IsAvailable)
                    //{
                    //  await  Forms9Patch.PrintService.PrintAsync(HTML, "XCOMANDERA");
                    //}
                    //else
                    //    Acr.UserDialogs.UserDialogs.Instance.Toast("PDF Export is not available on this device", TimeSpan.FromSeconds(5));
                }
            }
            catch (Exception ex)
            {
                SQLHelper.Log.LogMe(ex, "Al guardar el archivo html");
                return false;
            }
            return false;
        }
    }
}
