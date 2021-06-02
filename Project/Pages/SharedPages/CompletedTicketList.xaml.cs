using Project.Database;
using Project.DataClasses;
using Project.Pages.SharedPages;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.SuperPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompletedTicketList : ContentPage
    {
        private ObservableCollection<CompletedTickets> Items { get; set; }
        private readonly CompletedTicketDataAccess completedTicketDataAccess;

        public CompletedTicketList()
        {
            InitializeComponent();
            completedTicketDataAccess = new CompletedTicketDataAccess();
            Items = completedTicketDataAccess.CompletedTickets;
            //order observable list
            Items = new ObservableCollection<CompletedTickets>(Items.OrderBy(x => x.OrderRef.Date));
            TicketView.ItemsSource = Items;
            BindingContext = Items;
        }
        //due to load times images need to be loaded through tapped item and close when done viewing
        private async void TicketView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var byteArr = Items[e.ItemIndex].DisplayPhoto;
            Page page = new TappedImagePage(byteArr, Items[e.ItemIndex].Id);
            await Navigation.PushAsync(page);
        }
        
    }
}