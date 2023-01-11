using AudioRecorder.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace AudioRecorder.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}