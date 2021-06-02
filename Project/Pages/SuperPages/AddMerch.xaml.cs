using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project.Database;
using Project.DataClasses;

namespace Project.Pages.SuperPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddMerch : ContentPage
    {
        private readonly MerchandiserDataAccess dataAccess;
        public AddMerch()
        {
            InitializeComponent();
            dataAccess = new MerchandiserDataAccess();
            this.BindingContext = this.dataAccess;
        }
        //save merch
        private async void SaveButton_Clicked(object sender, EventArgs args)
        {
            if (CheckValues() == true)
            {
                Merchandiser newMerch = new Merchandiser()
                {
                    Name = name.Text,
                    ContactNum = contactNum.Text,
                };
                this.dataAccess.AddNewMerch(newMerch);
                this.dataAccess.SaveAllMerchandisers();
                //call clear button to clear form to default
                ClearButton_Clicked(sender, args);
                await DisplayAlert("Merchandiser Added", "Merchandiser has been added", "Ok");
                await Navigation.PopAsync();
                await Navigation.PopAsync();
                await Navigation.PushAsync(new MerchandiserList());
            }
            else
            {
                await DisplayAlert("Invalid Input", "You didn't complete the form.", "Ok");
            }
        }

        //clear form
        private void ClearButton_Clicked(object sender, EventArgs args)
        {
            name.Text = null;
            contactNum.Text = null;
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

