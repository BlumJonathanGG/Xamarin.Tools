﻿using System;
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

[assembly: UsesFeature("android.hardware.camera", Required = false)]
//[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
//[assembly: UsesPermission("android.permission.READ_PHONE_STATE")]
//[assembly: UsesPermission("android.permission.ACCESS_WIFI_STATE")]
//[assembly: UsesPermission("android.permission.BLUETOOTH")]

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

        // Field, property, and method for Picture Picker
        //public static readonly int PickImageId = 1000;
        public static MainActivity Instance;
        //public TaskCompletionSource<Tuple<byte[], ImageSource>> PickImageTaskCompletionSource { set; get; }
        //protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        //{
        //    base.OnActivityResult(requestCode, resultCode, intent);

        //    if (requestCode == PickImageId)
        //    {
        //        if ((resultCode == Result.Ok) && (intent != null))
        //        {
        //            Android.Net.Uri uri = intent.Data;
        //            Stream stream = ContentResolver.OpenInputStream(uri);
        //            byte[] bits = null;
        //            try
        //            {
        //                using (MemoryStream memoryStream = new MemoryStream())
        //                {
        //                    stream.CopyTo(memoryStream);
        //                    bits = memoryStream.ToArray();
        //                }
        //                Tuple<byte[], ImageSource> tuple = new Tuple<byte[], ImageSource>(bits,
        //                    ImageSource.FromStream(() => new MemoryStream(bits)));
        //                PickImageTaskCompletionSource.SetResult(tuple);
        //            }
        //            catch (Exception ex)
        //            {
        //                Log.Logger.Error(ex, "Al obtener la imagen despues de ser abierta");
        //            }
        //        }
        //        else
        //        {
        //            PickImageTaskCompletionSource.SetResult(null);
        //        }
        //    }
        //}

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
        }

        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}
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
        protected bool IsServiceRunning(Type serviceClass)
        {
            var service_class = Java.Lang.Class.FromType(serviceClass);
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
#pragma warning disable CS0618 // Type or member is obsolete
            foreach (var service in manager.GetRunningServices(Integer.MaxValue))
#pragma warning restore CS0618 // Type or member is obsolete
            {
                if (service_class.Name == service.Service.ClassName)
                {
                    return true;
                }
            }
            return false;
        }
        public static Context GetAppContext()
        {
            return Android.App.Application.Context;
        }
    }
}