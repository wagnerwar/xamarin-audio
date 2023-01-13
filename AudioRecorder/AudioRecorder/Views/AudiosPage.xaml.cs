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
    public partial class AudiosPage : ContentPage
    {
        public AudiosPage()
        {
            InitializeComponent();
            this.BindingContext = new AudiosViewModel();
        }
    }
}