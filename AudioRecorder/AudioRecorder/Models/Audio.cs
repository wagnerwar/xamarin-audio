using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace AudioRecorder.Models
{
    public class Audio
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public byte[] Arquivo { get; set; }        
    }
}
