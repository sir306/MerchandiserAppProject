using Project.Database;
using Project.DataClasses;
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

namespace Project.Pages.SuperPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchandiserList : ContentPage
    {
        private List<Merchandiser> merchandisers;
        public ObservableCollection<string> Items { get; set; }
        private readonly MerchandiserDataAccess dataAccess;
        private readonly SQLiteConnection database;

        private Page page;

        public MerchandiserList()
        {
            InitializeComponent();
            dataAccess = new MerchandiserDataAccess();
            this.BindingContext = this.dataAccess;
            merchandisers = dataAccess.Merchandisers.ToList();
            MerchandisersView.ItemsSource = merchandisers;
        }
        //add button clicked
        private async void AddButton_Clicked(object sender, EventArgs args)
        {
            Page page = new AddMerch();
            await Navigation.PushAsync(page);
        }
        //save 
        private void SaveButton_Clicked(object sender, EventArgs args)
        {
            this.dataAccess.SaveAllMerchandisers();
        }
        //remove 
        private void RemoveButton_Clicked(object sender, EventArgs args)
        {
            if (this.MerchandisersView.SelectedItem is Merchandiser currentMerchandisers)
            {
                this.dataAccess.DeleteMerch(currentMerchandisers);
            }
        }
        //remove all 
        private async void RemoveAllButton_Clicked(object sender, EventArgs args)
        {
            if (this.dataAccess.Merchandisers.Any())
            {
                var result = await DisplayAlert("Confirm Delete", "Are you sure you want to delete all Merchandiser data?", "OK", "Cancel");
                if (result == true)
                {
                    this.dataAccess.DeleteAllMerchandisers();
                    this.BindingContext = this.dataAccess;
                }
            }
        }
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
            {
                return;
            }

            page = new UpdateDeleteMerch();
            var item = e.Item;
            page.BindingContext = item;
            ToPage(page);

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        private async void ToPage(Page page)
        {
            await Navigation.PushAsync(page);
        }

        //page reload handle
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var dbName = "MerchListDatabase.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);

            if (database == null)
            {
                new MerchandiserDataAccess();
            }
            using (SQLiteConnection conn = new SQLiteConnection(path))
            {
                merchandisers = conn.Table<Merchandiser>().ToList();
                MerchandisersView.ItemsSource = merchandisers;
            }
        }
    }
}
