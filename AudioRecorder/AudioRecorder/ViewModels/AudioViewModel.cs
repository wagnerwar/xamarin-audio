using System;
using System.Collections.Generic;
using System.Text;
using AudioRecorder.Models;
using AudioRecorder.Views;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
namespace AudioRecorder.ViewModels
{
    public class AudioViewModel : BaseViewModel
    {
        private String imagemGravar = "icone_gravar.png";
        private String imagemGravando = "icone_stop.png";
        AudioRecorderService gravador;
        private string image;
        private bool gravando;
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }
        public bool Gravando
        {
            get => gravando;
            set => SetProperty(ref gravando, value);
        }
        public Command IniciarGravacaoCommand { get; }
        public AudioViewModel()
        {
            gravador = new AudioRecorderService
            {
                StopRecordingAfterTimeout = true,
                TotalAudioTimeout = TimeSpan.FromSeconds(15),
                AudioSilenceTimeout = TimeSpan.FromSeconds(2)
            };
            IniciarGravacaoCommand = new Command(OnIniciarGravacao);
            Gravando = false;
            Image = imagemGravar;
        }
        private async void OnIniciarGravacao()
        {                     
            await ProcessarGravacao();
            //await TratarFimGravacao();
        }
        private async Task TratarFimGravacao()
        {
            Image = imagemGravar;
            Gravando = false;
            // Recuperando arquivo do audio gravado
            var filePath = gravador.GetAudioFilePath();
            MessagingCenter.Send<AudioPage, String>(new AudioPage(), "Mensagem", "Arquivo enviado");
        }
        private async Task ProcessarGravacao()
        {
            Image = imagemGravando;
            try
            {
                if (!gravador.IsRecording)
                {
                    gravador.StopRecordingOnSilence = true;
                    if (Gravando == false)
                    {
                        Gravando = true;
                    }
                    //Começar gravação
                    var audioRecordTask = await gravador.StartRecording();
                    await audioRecordTask;
                }
                else
                {                   
                    //parar a gravação…
                    await gravador.StopRecording();
                    await TratarFimGravacao();
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<AudioPage, String>(new AudioPage(), "Mensagem", ex.Message);
            }
        }
        private async Task<bool> RecuperarEstadoGravacao()
        {
            if (Gravando == true)
            {
                return false;
            }
            return true;
            /*if (Image == imagemGravar)
            {
                return false;
            }else if(Image == imagemGravando && Gravando == true)
            {
                return true;
            }
            return false;*/
        }
    }
}
