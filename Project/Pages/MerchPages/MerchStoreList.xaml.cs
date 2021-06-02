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

namespace Project.Pages.MerchPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchStoreList : ContentPage
    {
        private ObservableCollection<StoreDetails> Items { get; set; }
        private readonly StoreDataAccess storeDataAccess;
        private readonly SQLiteConnection database;

        public MerchStoreList()
        {
            InitializeComponent();
            storeDataAccess = new StoreDataAccess();
            this.BindingContext = this.storeDataAccess;
            Items = storeDataAccess.StoreDetails;
            StoreViewMerch.ItemsSource = Items;

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
                StoreViewMerch.ItemsSource = Items;
            }
        }
    }
}