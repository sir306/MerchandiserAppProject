using Project.Models;
using Project.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Project
{
    public partial class MainPage : ContentPage
    {
        public MainPage() 
        {
            InitializeComponent();
            BindingContext = new MerchOrSuperModel();
        }
        private async void OnClickButton(object sender, EventArgs args)
        {
            string userType = (sender as Button).Text;
            var model = new SignInViewModel
            {
                UserType = userType
            };
            var page = new SignIn(userType)
            {
                BindingContext = model
            };
            await Navigation.PushAsync(page);
        }
    }
}
