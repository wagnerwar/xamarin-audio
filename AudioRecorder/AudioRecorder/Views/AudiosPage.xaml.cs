using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioRecorder.Models;
using AudioRecorder.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AudioRecorder.Views
{
    public partial class AudiosPage : ContentPage
    {
        AudiosViewModel _viewModel;
        public AudiosPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel = new AudiosViewModel();
            MessagingCenter.Subscribe<AudiosPage, String>(this, "Mensagem", (sender, a) =>
            {
                this.DisplayToastAsync(a, 3000);
            });
        }
    }
}