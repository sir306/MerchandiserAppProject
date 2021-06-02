using Project.Database;
using Project.DataClasses;
using Project.Pages.SuperPages;
using Project.Pages.UpdateDeleteListItem;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.ListPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreList : ContentPage
    {
        private ObservableCollection<StoreDetails> Items { get; set; }
        private readonly StoreDataAccess storeDataAccess;
        private readonly SQLiteConnection database;

        public StoreList()
        {
            InitializeComponent();
            storeDataAccess = new StoreDataAccess();
            BindingContext = storeDataAccess;
            Items = storeDataAccess.StoreDetails;
            StoreView.ItemsSource = Items;
            
        }
        private async void AddButton_Clicked(object sender, EventArgs args)
        {
            MerchandiserDataAccess dataAccess = new MerchandiserDataAccess();
            if(dataAccess.Merchandisers.Count != 0)
            {
                await Navigation.PushAsync(new AddStore());
            }
            else
            {
                await DisplayAlert("No Merchandisers", "You can't create a store till a merchandiser has been added. " +
                    "Go to the Merchandiser page and click the plus button to add one.", "OK");
            }
            
        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) { return; }
            else
            {
                StoreDetails item = Items[e.ItemIndex];
                await Navigation.PushAsync(new UpdateDeleteList("Store", item));
                //Deselect Item
                ((ListView)sender).SelectedItem = null;
            }    
            
        }
        //page reload handle
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var dbName = "StoreListDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);

            if (database == null)
            {
                new StoreDataAccess();
            }
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                Items = storeDataAccess.StoreDetails;
                StoreView.ItemsSource = Items;
            }
        }
    }
}
