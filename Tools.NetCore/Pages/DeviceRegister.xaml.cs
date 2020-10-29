﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tools.License;
using Tools.Services.Interfaces;

namespace Tools.NetCore.Pages
{
    /// <summary>
    /// Lógica de interacción para DeviceRegister.xaml
    /// </summary>
    public partial class DeviceRegister : Window
    {
        public DeviceRegisterModel Model { get; private set; }
        public DeviceRegister(Licence Licence, ICustomMessageBox CustomMesaggeBox)
        {
            this.Model = new DeviceRegisterModel(Licence, CustomMesaggeBox);
            this.DataContext = this.Model;
            InitializeComponent();
            this.TxtUsuario.DataContext = this.Model;

        }
        private void Hyperlink_RequestNavigate(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(License.Licence.LoginSite));
            e.Handled = true;
        }
        private async void LogIn(object sender, RoutedEventArgs e)
        {
            this.Model.Password = Pssword.Password;
            if (await this.Model.LogIn())
            {
                this.Close();
            }
        }
    }
}