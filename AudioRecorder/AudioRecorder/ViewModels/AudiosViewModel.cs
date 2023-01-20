using System;
using System.Collections.Generic;
using System.Text;
using AudioRecorder.Models;
using AudioRecorder.Views;
using Xamarin.Forms;
using AudioRecorder.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using AudioRecorder.Interfaces;
using System.IO;

namespace AudioRecorder.ViewModels
{
    public class AudiosViewModel : BaseViewModel
    {
        private String Pasta = "gravados";
      
        private AudioPlayer reprodutor;
        public ObservableCollection<Audio> Items { get; }
        public Command GravarCommand { get; }
        public Command LoadItemsCommand { get; }
        public Command<Audio> PlayAudioCommand { get; }
        public Command<Audio> DeleteAudioCommand { get; }
        public Command<Audio> DownloadAudioCommand { get; }
        public AudiosViewModel()
        {
            reprodutor = new AudioPlayer();
            Items = new ObservableCollection<Audio>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            GravarCommand = new Command(OnGravar);
            DeleteAudioCommand = new Command<Audio>(DeleteArquivo);
            PlayAudioCommand = new Command<Audio>(Reproduzir);
        }
        private async void OnGravar()
        {
            await Shell.Current.GoToAsync(nameof(AudioPage));
        }
        private async Task ExecuteLoadItemsCommand()
        {            
            IsBusy = true;                     
            try
            {
                await CarregarAudios();
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async Task CarregarAudios()
        {
            Items.Clear();
            await Task.Delay(TimeSpan.FromSeconds(2));
            var items = await BancoService.GetItemsAsync(true);
            foreach (var item in items)
            {
                await ExibirImagemReproducaoAudio(item);
                Items.Add(item);
            }
        }
        private async void DeleteArquivo(Audio item)
        {
            try
            {
                await BancoService.DeleteItemAsync(item.Id.ToString());
                await CarregarAudios();
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", "Arquivo deletado com sucesso");
            }
            catch(Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }
        }
        private async void Reproduzir(Audio item)
        {
            try
            {
                item.Reproducao = true;
                await ExibirImagemReproducaoAudio(item);
                await Task.Delay(TimeSpan.FromSeconds(3));
                await ExecutarAudio(item);                
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }
            finally
            {
                item.Reproducao = false;
                await ExibirImagemReproducaoAudio(item);
            }
        }
        async Task ExecutarAudio(Audio item)
        {
            await Task.Run(() => TocarAudio(item));
        }
        void TocarAudio(Audio item)
        {
            String caminho = GetCaminhoPasta();
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
            String caminhoArquivo = Path.Combine(caminho, String.Concat(item.Nome, ".wav"));
            File.WriteAllBytes(caminhoArquivo, item.Arquivo);
            reprodutor.Play(caminhoArquivo);
        }
        public String GetCaminhoPasta()
        {
            String basePath = PathService.Sounds;
            String caminho = Path.Combine(basePath, Pasta);
            return caminho;
        }
        public async Task ExibirImagemReproducaoAudio(Audio item)
        {            
            if (item.Reproducao)
            {
                item.ImagemReproducaoArquivo = item.ImagemReproduzindo;
            }
            else
            {
                item.ImagemReproducaoArquivo = item.ImagemReproduzir;
            }                        
        }
    }
}
