using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.ListPages;
using Project.Pages.ListPages;
using Project.Pages.MerchPages;
using Project.Pages.SuperPages;
using Project.SpecialClasses;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Command ViewActiveTickets { get; }
        public Command ViewCompletedTickets { get; }
        public Command ViewStores { get; }
        public Command SignOut { get; }
        private Page page;
        public Menu()
        {
            InitializeComponent();

            ViewActiveTickets = new Command(() => ViewATickets());
            ViewCompletedTickets = new Command(() => ViewCompTickets());
            ViewStores = new Command(() => ViewStoreList());
            SignOut = new Command(() => SignUserOut());
            BindingContext = this;
        }

        private async void SignUserOut()
        {
            await Navigation.PopToRootAsync();
        }

        private async void ViewStoreList()
        {
            Database.StoreDataAccess storeDataAccess = new Database.StoreDataAccess();
            if (storeDataAccess.StoreDetails.Count != 0)
            {
                page = new MerchStoreList();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert("No Stores", "Currently there are no stores stored, " +
                    "please try again later when your supervisor has added some.", "OK");
            }
            
        }

        private async void ViewCompTickets()
        {
            Database.CompletedTicketDataAccess CompletedicketDataAccess = 
                new Database.CompletedTicketDataAccess();
            if (CompletedicketDataAccess.CompletedTickets.Count != 0)
            {
                page = new CompletedTicketList();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert("No Tickets", "Currently there are no Completed Tickets stored, " +
                    "please try again later when you have completed an Active Ticket.", "OK");
            }
                
        }

        private async void ViewATickets()
        {
            Database.TicketDataAccess ticketDataAccess = new Database.TicketDataAccess();
            if (ticketDataAccess.Tickets.Count != 0)
            {
                page = new ActiveTicketsMerch();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert("No Tickets", "Currently there are no Tickets stored, " +
                    "please try again later when your supervisor has added some.", "OK");
            }
            
        }
    }
}