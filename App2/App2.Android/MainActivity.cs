using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Java.IO;
using Android.Content;
using System.IO;

namespace App2.Droid
{
    [Activity(Label = "App2", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        //public static File ApplicationOperationPath;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            
            var  DirPath = Android.App.Application.Context.GetExternalFilesDir(null).AbsolutePath;
            var textPath = Path.Combine(DirPath, "FGSerialNumbers.txt");
            var DuplicatetextPath = Path.Combine(DirPath, "DuplicateFGSerialNumbers.txt");
            if (!System.IO.File.Exists(textPath))
            {
                var value=System.IO.File.Create(textPath);
                using (var str = new StreamReader(value))
                {
                   // Datalist = str.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
                //using (var str = System.IO.File.AppendText(textPath))
                //{
                //    str.WriteLine(str.NewLine);

                //}
            }
            if(!System.IO.File.Exists(DuplicatetextPath))
            {
                var value = System.IO.File.Create(DuplicatetextPath);
                using (var str = new StreamReader(value))
                {
                    // Datalist = str.ReadToEnd().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                }
            }

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}