using AudioRecorder.ViewModels;
using AudioRecorder.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace AudioRecorder
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(AudioPage), typeof(AudioPage));
        }

    }
}
