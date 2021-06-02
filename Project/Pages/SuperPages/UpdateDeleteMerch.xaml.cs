using Project.Database;
using Project.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.SuperPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDeleteMerch : ContentPage
    {
        private readonly MerchandiserDataAccess dataAccess;

        public UpdateDeleteMerch()
        {
            InitializeComponent();
            dataAccess = new MerchandiserDataAccess();
        }
        //save merch
        private void UpdateButton_Clicked(object sender, EventArgs args)
        {
            if (CheckValues() == true)
            {

                Merchandiser merch = (Merchandiser)BindingContext;
                merch.Name = name.Text;
                merch.ContactNum = contactNum.Text;
                this.dataAccess.SaveAllMerchandisers();
                DisplayAlert("Updated", "Merchandiser has been updated", "Ok");
                Page page = new MerchandiserList();
                Navigation.PopAsync();
                Navigation.PopAsync();
                Navigation.PushAsync(page);
            }
            else
            {
                DisplayAlert("Invalid Input", "You didn't complete the form.", "Ok");
            }
        }
        private void DeleteButton_Clicked(object sender, EventArgs args)
        {
            Merchandiser merch = (Merchandiser)BindingContext;
            dataAccess.DeleteMerch(merch);
            DisplayAlert("Merchandiser Deleted", "The Merchandiser has been deleted from record", "OK");
            Page page = new MerchandiserList();
            Navigation.PopAsync();
            Navigation.PopAsync();
            Navigation.PushAsync(page);
        }
        //check not default values
        private bool CheckValues()
        {
            //check store
            if (name.Text == null)
            {
                return false;
            }
            //check comments
            if (contactNum.Text == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
