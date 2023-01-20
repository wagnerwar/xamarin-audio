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
        private Audio AudioEmExecucao { get; set; }
        bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }
        public ObservableCollection<Audio> Items { get; }
        public Command GravarCommand { get; }
        public Command LimparCommand { get; }
        public Command LoadItemsCommand { get; }
        public Command CarregarAudiosCommand { get; }
        public Command<Audio> PlayAudioCommand { get; }
        public Command<Audio> DeleteAudioCommand { get; }
        public Command<Audio> DownloadAudioCommand { get; }
        public AudiosViewModel()
        {
            reprodutor = new AudioPlayer();
            reprodutor.FinishedPlaying += new EventHandler(FinalizarAudio);
            Items = new ObservableCollection<Audio>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            GravarCommand = new Command(OnGravar);
            DeleteAudioCommand = new Command<Audio>(DeleteArquivo);
            PlayAudioCommand = new Command<Audio>(Reproduzir);
            DownloadAudioCommand = new Command<Audio>(BaixarAudio);
            LimparCommand = new Command(async () => await LimparPasta());
            CarregarAudiosCommand = new Command(async () => await CarregarAudios());
            if (CarregarAudiosCommand.CanExecute(null))
            {
                CarregarAudiosCommand.Execute(null);
            }
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
            isLoading = true;
            Items.Clear();
            await Task.Delay(TimeSpan.FromSeconds(3));
            var items = await BancoService.GetItemsAsync(true);
            foreach (var item in items)
            {
                await ExibirImagemReproducaoAudio(item);
                Items.Add(item);
            }
            IsLoading = false;
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
                await ExecutarAudio(item);                
            }
            catch (Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }
            finally
            {
                //item.Reproducao = false;
                //await ExibirImagemReproducaoAudio(item);
            }
        }
        async Task ExecutarAudio(Audio item)
        {
            await Task.Run(() => TocarAudio(item));
        }
        void TocarAudio(Audio item)
        {
            String caminhoArquivo = DownloadAudio(item).Result;
            reprodutor.Play(caminhoArquivo);
            AudioEmExecucao = item;
        }
        void FinalizarAudio(object sender, EventArgs e)
        {
            try
            {
                if(AudioEmExecucao != null && AudioEmExecucao.Reproducao == true)
                {
                    AudioEmExecucao.Reproducao = false;
                    if (AudioEmExecucao.Reproducao)
                    {
                        AudioEmExecucao.ImagemReproducaoArquivo = AudioEmExecucao.ImagemReproduzindo;
                    }
                    else
                    {
                        AudioEmExecucao.ImagemReproducaoArquivo = AudioEmExecucao.ImagemReproduzir;
                    }
                    //AudioEmExecucao = null;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
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
        async Task<String> DownloadAudio(Audio item)
        {
            var res = Task.Run(() => ExecutarDownloadAudio(item)).Result;
            return res;
        }
        String ExecutarDownloadAudio(Audio item)
        {
            String caminho = GetCaminhoPasta();
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
            String caminhoArquivo = Path.Combine(caminho, String.Concat(item.Nome, ".wav"));
            File.WriteAllBytes(caminhoArquivo, item.Arquivo);
            return caminhoArquivo;
        }
        void BaixarAudio(Audio item)
        {
            try
            {
                String caminhoArquivo = DownloadAudio(item).Result;
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", "Audio baixdo com sucesso!!!");
            }
            catch(Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }            
        }
        async Task LimparPasta()
        {
            await Task.Run(() => Limpar());
        }
        void Limpar()
        {
            try
            {
                String caminho = GetCaminhoPasta();
                if (Directory.Exists(caminho))
                {
                    DirectoryInfo dir = new DirectoryInfo(caminho);
                    foreach (FileInfo fi in dir.GetFiles())
                    {
                        fi.Delete();
                    }
                }
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", "Pasta limpa com sucesso!!!");
            }
            catch(Exception ex)
            {
                MessagingCenter.Send<AudiosPage, String>(new AudiosPage(), "Mensagem", ex.Message);
            }
        }
    }
}
