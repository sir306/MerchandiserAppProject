using Project.Database;
using Project.DataClasses;
using SQLite;
using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.MerchPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActiveTicketsMerch : ContentPage
    {
        private readonly TicketDataAccess dataAccess;
        private readonly SQLiteConnection database;

        public ActiveTicketsMerch()
        {
            InitializeComponent();
            dataAccess = new TicketDataAccess();
            BindingContext = dataAccess;
            MerchTicketView.ItemsSource = dataAccess.Tickets;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var dbName = "TicketDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
            if (database == null)
            {
                new TicketDataAccess();
            }
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                MerchTicketView.ItemsSource = dataAccess.Tickets; 
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) { return; }
            else
            {
                Ticket item = dataAccess.Tickets[e.ItemIndex];
                await Navigation.PushAsync(new MerchSelectedTicket(item));
                //Deselect Item
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
