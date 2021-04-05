﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FFImageLoading.Forms.Platform;
using Plugin.CurrentActivity;
using Plugin.Permissions;
using Kit.Sql;
using Xamarin.Forms;

[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]
[assembly: UsesPermission("android.permission.READ_PHONE_STATE")]
[assembly: UsesPermission("android.permission.ACCESS_WIFI_STATE")]
[assembly: UsesPermission("android.permission.BLUETOOTH")]

namespace Kit.Droid.Services
{
    [Activity(Label = "Kit.MainActivity", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {

        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;
        public static MainActivity Instance;
        public TaskCompletionSource<Tuple<byte[], ImageSource>> PickImageTaskCompletionSource { set; get; }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (intent != null))
                {
                    Android.Net.Uri uri = intent.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);
                    byte[] bits = null;
                    try
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bits = memoryStream.ToArray();
                        }
                        Tuple<byte[], ImageSource> tuple = new Tuple<byte[], ImageSource>(bits,
                            ImageSource.FromStream(() => new MemoryStream(bits)));
                        PickImageTaskCompletionSource.SetResult(tuple);
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex, "Al obtener la imagen despues de ser abierta");
                    }
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Expander_Experimental", "Brush_Experimental", "Shapes_Experimental", "DragAndDrop_Experimental");
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
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
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnDestroy()
        {
            Serilog.Log.CloseAndFlush();
            base.OnDestroy();
        }
    }
}