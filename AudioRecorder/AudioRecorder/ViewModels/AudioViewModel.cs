using System;
using System.Collections.Generic;
using System.Text;
using AudioRecorder.Models;
using AudioRecorder.Views;
using Xamarin.Forms;

namespace AudioRecorder.ViewModels
{
    public class AudioViewModel : BaseViewModel
    {
        public Command IniciarGravacaoCommand { get; }
        public AudioViewModel()
        {
            IniciarGravacaoCommand = new Command(OnIniciarGravacao);
        }
        private async void OnIniciarGravacao()
        {
            MessagingCenter.Send<AudioPage, String>(new AudioPage(), "Mensagem", "Arquivo enviado");
        }
    }
}
