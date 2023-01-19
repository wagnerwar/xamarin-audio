using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioRecorder.Interfaces;
[assembly: Xamarin.Forms.Dependency(typeof(AudioRecorder.Droid.PathService))]
namespace AudioRecorder.Droid
{
    public class PathService : IPathService
    {
        public string Sounds
        {
            get
            {
                return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic).AbsolutePath;
            }
        }
    }
}