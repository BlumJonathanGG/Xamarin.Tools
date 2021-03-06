﻿using System;
using System.Net;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using FFImageLoading.Forms.Platform;
using Kit.Forms.Services;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using Java.Lang;
using Kit.Droid.Services;
using Kit.Forms.Services.Interfaces;

[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesPermission("android.permission.ACCESS_WIFI_STATE")]
[assembly: Dependency(typeof(MainActivity))]
namespace Kit.Droid.Services
{
    [MetaData(name: "android.support.FILE_PROVIDER_PATH", Resource = "@xml/paths")]
    [Activity(Label = "Kit.MainActivity", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges =
            ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
            ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden |
            ConfigChanges.Mcc | ConfigChanges.Mnc | ConfigChanges.Navigation
    )]
    public abstract class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if (!e.Flags.HasFlag(Android.Views.KeyEventFlags.SoftKeyboard) || keyCode == Keycode.Back)
            {
                char character = ((char)e.GetUnicodeChar(e.MetaState));
                if (Kit.Tools.Debugging)
                {
                    Log.Logger.Debug("Keyboard:{0}", character);
                }
                MessagingCenter.Send<object, char>(this, IKeyboardListenerService.Message, character);
            }
            return base.OnKeyUp(keyCode, e);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Shapes_Experimental", "DragAndDrop_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            CachedImageRenderer.InitImageViewHandler();
            CachedImageRenderer.Init(false);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this);
            Kit.Droid.Tools.Init(this, savedInstanceState);
            Forms9Patch.Droid.Settings.Initialize(this);
            Instance = this; //ImagePicker
            ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) => true;

            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            Serilog.Log.CloseAndFlush();
            base.OnDestroy();
        }
        public static Context GetAppContext()
        {
            return Android.App.Application.Context;
        }


    }
}