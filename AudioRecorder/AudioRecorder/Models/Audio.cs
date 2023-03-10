using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace AudioRecorder.Models
{
    public class Audio : INotifyPropertyChanged
    {
        public readonly String ImagemReproduzir = "icone_play.png";
        public readonly String ImagemReproduzindo = "icone_pause.png";
        public int Id {
            get;set;
        }
        public String Nome { get; set; }
        public byte[] Arquivo { get; set; }
        private String imagemReproducaoArquivo { get; set; }
        public String ImagemReproducaoArquivo{
            get
            {
                return imagemReproducaoArquivo;
            }
            set
            {
                if(imagemReproducaoArquivo != value)
                {
                    imagemReproducaoArquivo = value;
                    NotifyPropertyChanged();
                }                
            }
        }
        public bool Reproducao {
            get; set;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
