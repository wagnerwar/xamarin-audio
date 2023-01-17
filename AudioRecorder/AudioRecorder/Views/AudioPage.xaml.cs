using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioRecorder.Models;
using AudioRecorder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;

namespace AudioRecorder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AudioPage : ContentPage
    {
        public AudioPage()
        {
            InitializeComponent();
            this.BindingContext = new AudioViewModel();
            MessagingCenter.Subscribe<AudioPage, String>(this, "Mensagem", (sender, a) =>
            {
                this.DisplayToastAsync(a, 3000);
            });
        }
    }
}