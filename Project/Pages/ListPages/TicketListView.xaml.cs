using Project.Database;
using Project.DataClasses;
using Project.Pages.SuperPages;
using Project.Pages.UpdateDeleteListItem;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.ListPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TicketListView : ContentPage
    {
        private readonly TicketDataAccess dataAccess;
        private readonly SQLiteConnection database;
        
       
        public TicketListView()
        {
            InitializeComponent();
            dataAccess = new TicketDataAccess();
            BindingContext = dataAccess;
            //TicketView.ItemsSource = dataAccess.Tickets;
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
                TicketView.ItemsSource = dataAccess.Tickets;
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null){ return; }
            else
            {
                Ticket item = dataAccess.Tickets[e.ItemIndex];
                await Navigation.PushAsync(new UpdateDeleteList("Ticket", item));
                //Deselect Item
                ((ListView)sender).SelectedItem = null;
            }
            
        }
        
        private async void AddButton_Clicked(object sender, EventArgs args)
        {
            StoreDataAccess storeData = new StoreDataAccess();
            if(storeData.StoreDetails.Count != 0)
            {
                await Navigation.PushAsync(new NewTicket());
            }
            else
            {
                await DisplayAlert("No Stores", "You can't create a ticket till a store has been added. " +
                    "Go to the Store page and click the plus button to add one.", "OK");
            }
            
        }
    }
}
