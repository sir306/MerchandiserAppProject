using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Project.Models;
using Project.Pages;

namespace Project.SpecialClasses
{
    class MenuButton
    {
        public Page Destination { get; set; }

        public Button CreateButton(string textValue)
        {
            Button menuButton = new Button
            {
                Text = textValue,
                
            };
            menuButton.Clicked += new EventHandler(BtnClick);
            return menuButton;
        }

        private async void BtnClick(object sender, EventArgs args)
        {
            var currentPage = App.Current.MainPage;
            await currentPage.Navigation.PushAsync(Destination);
        }
        
    }
}
