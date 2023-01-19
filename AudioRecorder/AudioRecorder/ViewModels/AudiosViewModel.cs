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
                
                String caminho = GetCaminhoPasta();
                if (!Directory.Exists(caminho))
                {
                    Directory.CreateDirectory(caminho);
                }
                String caminhoArquivo = Path.Combine(caminho, String.Concat(item.Nome, ".wav"));
                File.WriteAllBytes(caminhoArquivo, item.Arquivo);
                reprodutor.Play(caminhoArquivo);

                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", "Arquivo deletado com sucesso");
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }
        }
        public String GetCaminhoPasta()
        {
            String basePath = DependencyService.Get<IPathService>().Sounds;
            String caminho = Path.Combine(basePath, Pasta);
            return caminho;
        }
         
    }
}
