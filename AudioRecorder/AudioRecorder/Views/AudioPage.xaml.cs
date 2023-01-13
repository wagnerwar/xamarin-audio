using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioRecorder.Models;
using AudioRecorder.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AudioRecorder.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AudioPage : ContentPage
    {
        public AudioPage()
        {
            InitializeComponent();
            this.BindingContext = new AudioViewModel();
        }
    }
}