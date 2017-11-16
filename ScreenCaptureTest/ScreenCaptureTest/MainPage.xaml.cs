using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ScreenCaptureTest
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var stream = await SignatureView.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
           var data =  await DependencyService.Get<IScreenshotServicecs>().CaptureAsync(stream);
        }
    }
}
