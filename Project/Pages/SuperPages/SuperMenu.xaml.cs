using Project.ListPages;
using Project.Models;
using Project.Pages.ListPages;
using Project.Pages.SuperPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuperMenu : ContentPage
    {
        public Command ViewActiveTickets { get; }
        public Command ViewMerchandiserList { get; }
        public Command ViewCompletedTickets { get; }
        public Command ViewStoreDetails { get; }
        public Command SignOut { get; }

        private Page page;

        public SuperMenu()
        {
            InitializeComponent();

            ViewActiveTickets = new Command(() => ViewATickets());
            ViewMerchandiserList = new Command(() => ViewMerchList());
            ViewCompletedTickets = new Command(() => ViewCompTickets());
            ViewStoreDetails = new Command(() => ViewStoreList());
            SignOut = new Command(() => SignUserOut());
            BindingContext = this;
        }

        private async void SignUserOut()
        {
            await Navigation.PopToRootAsync();
        }

        private async void ViewStoreList()
        {
            page = new StoreList();
            await Navigation.PushAsync(page);
        }

        private async void ViewCompTickets()
        {
            page = new CompletedTicketList();
            var model = new SuperMenuModel();
            page.BindingContext = model;
            await Navigation.PushAsync(page);
        }

        private async void ViewMerchList()
        {
            page = new MerchandiserList();
            await Navigation.PushAsync(page);
        }

        private async void ViewATickets()
        {
            page = new TicketListView();
            await Navigation.PushAsync(page);
        }
    }
}