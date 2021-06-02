using Project.ListPages;
using Project.Pages;
using Project.Pages.ListPages;
using Project.Pages.SuperPages;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Project.Models
{
    class SuperMenuModel : SuperMenu
    {
        public Command AddTicket { get; }
        public Command AddNewStore { get; }
        public Command CurrentStores { get; }
        public Command CurrentTickets { get; }
        public string UserType { get; set; }
        private Page page;

        public SuperMenuModel()
        {
            AddTicket = new Command(() => AddNewTicket());
            AddNewStore = new Command(() => NewStore());
            CurrentStores = new Command(() => CurrentStoreList());
            CurrentTickets = new Command(() => CurrentTicketList());
        }

        private async void CurrentTicketList()
        {
            page = new TicketListView();
            //await Navigation.PushAsync(page);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async void CurrentStoreList()
        {
            page = new StoreList();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async void NewStore()
        {
            page = new AddStore();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }

        private async void AddNewTicket()
        {
            page = new NewTicket();
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}
