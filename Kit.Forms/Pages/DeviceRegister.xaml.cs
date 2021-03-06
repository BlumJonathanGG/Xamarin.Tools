﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kit.Dialogs;
using Kit.License;
using Kit.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kit.Forms.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceRegister
    {
        public DeviceRegisterModel Model { get; private set; }

        public DeviceRegister(BlumAPI.License Licence, IDialogs Dialogs)
        {
            this.Model = new DeviceRegisterModel(Licence, Dialogs);
            InitializeComponent();
            this.LockModal();
        }

        private void MailChanged(object sender, EventArgs e)
        {
            if (Btn is null)
            {
                return;
            }
            if (string.IsNullOrEmpty(this.Model.UserName) || string.IsNullOrEmpty(this.Model.Password))
            {
                this.Btn.TextColor = Color.Gray;
            }
            else
            {
                this.Btn.TextColor = Color.White;
            }
        }

        private void PasswordChanged(object sender, EventArgs e)
        {
            MailChanged(sender, e);
        }

        private void Registrarse(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri(BlumAPI.License.LoginSite));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private async void LogIn(object sender, EventArgs e)
        {
            if (await this.Model.LogIn())
            {
                await this.Close();
            }
        }
    }
}