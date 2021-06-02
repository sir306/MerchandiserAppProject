using Project.Pages.SuperPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.SharedPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TappedImagePage : ContentPage
    {
        public int TicketID { get; set; }
        private readonly byte[] ByteArr;
        //public Image LoadedImage { get; set; }
        private Stream Stream;

        public TappedImagePage(Byte[] byteArr, int id)
        {
            InitializeComponent();
            TicketID = id;
            ByteArr = byteArr;
            BuildPage();
            DisplayAlert("Tap Image To Exit", "Tap the image to leave the page", "OK");
        }
        private Stream TurnBytesToStream(byte[] bytes)
        {
            Stream = new MemoryStream(bytes);
            return Stream;
        }
        public void EndImageStream()
        {
            Stream.Dispose();
        }
        // create page from scratch
        private void BuildPage()
        {
            Frame frame = new Frame()
            {
                Content = new Label 
                { 
                    Text = "Display Image of Ticket " + TicketID.ToString(), 
                    HorizontalTextAlignment = TextAlignment.Center,
                    Padding = new Thickness(0), Margin = new Thickness(0),
                    TextColor = Color.White, FontSize = 36
                },
                BackgroundColor = ColorConverters.FromHex("#2196F3")
            };
            ImageButton image = new ImageButton()
            {
                Source = ImageSource.FromStream(() => TurnBytesToStream(ByteArr)),
                Command = new Command(async () => await ImageButton_Clicked())
            };
            Content = new StackLayout()
            {
                Children =
                {
                    frame, image
                },
                BackgroundColor = Color.LightGray
            };
        }

        private Task ImageButton_Clicked()
        {
            //call method from previous page to end stream then pop
            EndImageStream();
            return Navigation.PopAsync();
            
        }
    }
}
