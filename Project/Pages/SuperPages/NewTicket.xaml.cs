using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project.Database;
using Project.DataClasses;
using Project.ListPages;
//TODO fix clear button, date picker so only present and future dates can be chosen save button pops page after being added title/frame
namespace Project.Pages.SuperPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewTicket : ContentPage
    {
        private readonly StoreDataAccess storeDataAccess;
        private readonly TicketDataAccess dataAccess;
        private DateTime AssignedDate;
        private DateTime CurrentDateTime = DateTime.Now;

        public NewTicket()
        {
            InitializeComponent();
            dataAccess = new TicketDataAccess();
            storeDataAccess = new StoreDataAccess();
            BindingContext = dataAccess;
            
        }
        //save tickets
        private void SaveButton_Clicked(object sender, EventArgs args)
        {
            if(CheckValues() == true)
            {
                if (AssignedDate == DateTime.MinValue) { AssignedDate = CurrentDateTime; }
                Ticket newTicket = new Ticket()
                {
                    StoreKey = GetStoreId(),
                    Store = storePicker.SelectedItem.ToString(),
                    BookedDate = AssignedDate.ToString("d")
                };
                this.dataAccess.AddNewTicket(newTicket);
                this.dataAccess.SaveAllTickets();

                //call clear button to clear form to default
                DisplayAlert("Saved", "New Ticket has been added.", "Ok");
                ClearButton_Clicked(sender, args);
                Page page = new TicketListView();
                Navigation.PopAsync();
                Navigation.PopAsync();
                Navigation.PushAsync(page);
            }
            else
            {
                DisplayAlert("Invalid Input", "You didn't complete the form.", "Ok");
            }
        }

        //clear form
        private void ClearButton_Clicked(object sender, EventArgs args)
        {
            storePicker.SelectedItem = default;
            _ = new DateTime();
            DateTime tempDate = DateTime.Now;
            AssignedDate = tempDate;
        }

        //check not default values
        private bool CheckValues()
        {
            //check store
            if(storePicker.SelectedIndex == -1)
            {
                return false;
            }
            if(AssignedDate == null) 
            {
                AssignedDate = CurrentDateTime;
                return true;
            }
            else
            {
                return true;
            }
        }
        //ensure store list up to date
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //merch picker set up
            List<string> storeList = new List<string>();
            foreach (StoreDetails store in storeDataAccess.StoreDetails)
            {
                storeList.Add(store.StoreName);
            }
            storePicker.ItemsSource = storeList;
        }
        //get merch name from picker
        private int GetStoreId()
        {
            if (storePicker.SelectedIndex != -1)
            {
                var id = 0;
                foreach (StoreDetails store in storeDataAccess.StoreDetails)
                {
                    if (store.StoreName == storePicker.SelectedItem.ToString())
                    {
                        id = store.StoreId;
                    }
                }
                return id;
            }
            else
            {
                return -1;
            }
        }

        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            AssignedDate = e.NewDate.Date;
        }
    }
}