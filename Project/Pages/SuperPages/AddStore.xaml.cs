using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Database;
using Project.DataClasses;
using Project.Pages.ListPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.SuperPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStore : ContentPage
    {
        private readonly MerchandiserDataAccess merchandiserDataAccess;
        private readonly StoreDataAccess storeDataAccess;
        public AddStore()
        {
            InitializeComponent();
            storeDataAccess = new StoreDataAccess();
            merchandiserDataAccess = new MerchandiserDataAccess();
            BindingContext = storeDataAccess;
        }
        //save store
        public void SaveButton_Clicked(object sender, EventArgs args)
        {
            if(CheckValues() == true)
            {
                StoreDetails newStore = new StoreDetails()
                {
                    StoreName = store.Text,
                    StoreManger = storeManager.Text,
                    StoreNumber = storeNumber.Text,
                    Address = address.Text,
                    MerchandiserKey = GetMerchId()
                };
                storeDataAccess.AddNewStore(newStore);
                storeDataAccess.SaveAllStoreDetails();
                ClearButton(sender, args);
                DisplayAlert("Store Added", "New Store has been added.", "OK");
                Page page = new StoreList();
                Navigation.PopAsync();
                Navigation.PopAsync();
                Navigation.PushAsync(page);
            }
            else
            {
                DisplayAlert("Invalid Input", "You didn't complete the form.", "Ok");
            }
        }
        //get merch key from picker
        private int GetMerchId()
        {
            if (merchPicker.SelectedIndex != -1)
            {
                var id = 0;
                foreach (Merchandiser merch in merchandiserDataAccess.Merchandisers)
                {
                    if (merch.Name == merchPicker.SelectedItem.ToString())
                    {
                        id = merch.MerchId;
                    }
                }
                return id;
            }
            else
            {
                return -1;
            } 
        }

        private void ClearButton(object sender, EventArgs args)
        {
            store.Text = null;
            storeManager.Text = null;
            storeNumber.Text = null;
            address.Text = null;
            merchPicker.SelectedItem = default;
        }

        private bool CheckValues()
        {
            //check store
            if (store.Text == null)
            {
                return false;
            }
            //check manager
            if (storeManager.Text == null)
            {
                return false;
            }
            //check store number
            if (storeNumber.Text == null)
            {
                return false;
            }
            //check address
            if (address.Text == null)
            {
                return false;
            }
            //check merch picker
            if (merchPicker.SelectedIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }
        //ensure merch list up to date
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //merch picker set up
            List<string> merchList = new List<string>();
            foreach (Merchandiser merch in merchandiserDataAccess.Merchandisers)
            {
                merchList.Add(merch.Name);
            }
            merchPicker.ItemsSource = merchList;
            
        }
    }
}