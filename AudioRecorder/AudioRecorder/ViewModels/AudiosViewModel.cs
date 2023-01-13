using System;
using System.Collections.Generic;
using System.Text;
using AudioRecorder.Models;
using AudioRecorder.Views;
using Xamarin.Forms;

namespace AudioRecorder.ViewModels
{
    public class AudiosViewModel : BaseViewModel
    {
        public Command GravarCommand { get; }
        public AudiosViewModel()
        {
            GravarCommand = new Command(OnGravar);
        }
        private async void OnGravar()
        {
            await Shell.Current.GoToAsync(nameof(AudioPage));
        }
    }
}
