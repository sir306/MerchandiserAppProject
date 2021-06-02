using System;
using Project.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Project.Pages.SuperPages;
using Project.Database;
using System.IO;
using SQLite;
using Project.DataClasses;

namespace Project.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignIn : ContentPage
    {
        private Page page;

        public string UserType { get; set; }
        public SignIn(string user)
        {
            InitializeComponent();
            UserType = user;
        }
        public async void BackButton(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MainPage());
        }
        private async void SignInButton(object sender, EventArgs args)
        {
            if (UserType != "Merchandiser")
            {
                //var model = new AddTicket() ;//{ UserType = UserType };
                page = new SuperMenu(); //{ BindingContext = model };
            }
            else
            {

                page = new Menu();
            }
            await Navigation.PushAsync(page);
        }
        //database deletion
        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    var dbName = "TicketDatabase.db3";
        //    var path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
        //    var con = new SQLiteConnection(path);
        //    con.DropTable<Ticket>();
        //    dbName = "CompletedTicketDatabase.db3";
        //    path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
        //    con = new SQLiteConnection(path);
        //    con.DropTable<CompletedTickets>();
        //    dbName = "StoreDatabase.db3";
        //    path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
        //    con = new SQLiteConnection(path);
        //    con.DropTable<StoreDetails>();
        //    dbName = "MerchListDatabase.db3";
        //    path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal), dbName);
        //    con = new SQLiteConnection(path);
        //    con.DropTable<Merchandiser>();
        //}
    }
}