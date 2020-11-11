﻿using System;
using System.Collections.Generic;
using System.Text;
using BaseLicense = Kit.License.License;
namespace Kit.Droid.Services
{
    public class License : BaseLicense
    {
        public License(string AppName) : base(new CustomMessageBoxService(),new DeviceInfo(),AppName)
        {

        }

        protected override async void OpenRegisterForm()
        {
            var page = Xamarin.Forms.Application.Current.MainPage;

            Forms.Pages.DeviceRegister login = new Forms.Pages.DeviceRegister(page.Background, this,new CustomMessageBoxService());
            await page.Navigation.PushModalAsync(login, true);
        }
    }
}