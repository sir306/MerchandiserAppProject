using Project.Database;
using Project.DataClasses;
using Project.ListPages;
using Project.Pages.ListPages;
using Project.Pages.SuperPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.UpdateDeleteListItem
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateDeleteList : ContentPage
    {
        private new readonly Label Title;
        private Style LabelStyle;
        private StoreDetails UpdateStoreDetails;
        private Ticket TicketUpdate;
        readonly StoreDataAccess dataAccess;
        readonly TicketDataAccess ticketDataAccess;
        private Entry SNum;
        private Entry SName;
        private Entry SMName;
        private Entry Addy;
        private Picker MerchPick;
        private Picker StorePicker;
        private DatePicker BookedDate;
        private int CurrentPickerIndex = -1;
        public List<string> merchList = new List<string>();
        public List<string> storeList = new List<string>();
        private readonly MerchandiserDataAccess merchandiserDataAccess;

        private bool DeleteConfirmed = false;
        public UpdateDeleteList(string pageType, object Item)
        {
            InitializeComponent();
            BindingContext = Item;
            SetLabelStyle();
            merchandiserDataAccess = new MerchandiserDataAccess();
            dataAccess = new StoreDataAccess();
            ticketDataAccess = new TicketDataAccess();
            string titleMsg = "Update or Delete Selected " + pageType;
            Frame frame = new Frame();
            Label title = new Label() {Text = titleMsg };
            Title = title;
            StackLayout titleStack = new StackLayout() { Children = { Title } };
            frame.Content = titleStack;
            if (pageType == "Store")
            {
                UpdateStoreDetails = (StoreDetails)Item;
                StoreUDLItem(frame);
            }
            if (pageType == "Ticket")
            {
                TicketUpdate = (Ticket)Item;
                TicketUDLItem(frame);
            }
            StylePage();
        }
        //button commands and related methods
        private async void SaveButton_Clicked(object sender, EventArgs args)
        {
            if (StoreCheckValues() == true)
            {
                UpdateStoreDetails.StoreName = SName.Text;
                UpdateStoreDetails.StoreManger = SMName.Text;
                UpdateStoreDetails.StoreNumber = SNum.Text;
                UpdateStoreDetails.Address = Addy.Text;
                UpdateStoreDetails.MerchandiserKey = GetMerchId();
                
                dataAccess.SaveStoreDetails(UpdateStoreDetails);
                await DisplayAlert("Store Update", "Store has been updated.", "Ok");
                Page page = new StoreList();
                await Navigation.PopAsync();
                await Navigation.PopAsync();
                await Navigation.PushAsync(page);
            }
            else
            {
                await DisplayAlert("Invalid Input", "You didn't complete the form.", "Ok");
            }
        }
        //delete methods and alerts
        private async void DeleteClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Confirm Data Deletion", "Are you sure you want to delete this?", "Yes", "No");
            if (answer) { DeleteConfirmed = true; }
            if (DeleteConfirmed != false)
            {
                dataAccess.DeleteStoreDetails(UpdateStoreDetails);
                Page page = new StoreList();
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PushAsync(page);
                DeleteConfirmed = false;
            }
            
        }
        private bool StoreCheckValues()
        {
            if (SName == null) { return false; }
            if (SMName == null) { return false; }
            if (SNum == null) { return false; }
            if (Addy == null) { return false; }
            else{ return true;}
        }
        //get merch key from picker
        private int GetMerchId()
        {
            if (MerchPick.SelectedIndex != -1)
            {
                var id = 0;
                foreach (Merchandiser merch in merchandiserDataAccess.Merchandisers)
                {
                    if (merch.Name == MerchPick.SelectedItem.ToString())
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
        //call to create page based on pageType store
        private void StoreUDLItem(Frame frame)
        {

            //create data entrys
            SName = new Entry();
            SName.SetBinding(Entry.TextProperty, new Binding("StoreName"));

            SMName = new Entry();
            SMName.SetBinding(Entry.TextProperty, new Binding("StoreManger"));

            SNum = new Entry();
            SNum.SetBinding(Entry.TextProperty, new Binding("StoreNumber"));

            Addy = new Entry();
            Addy.SetBinding(Entry.TextProperty, new Binding("Address"));

            //merch picker set up
            var i = 0;
            foreach (Merchandiser merch in merchandiserDataAccess.Merchandisers)
            {
                merchList.Add(merch.Name);
                if(merch.MerchId == UpdateStoreDetails.MerchandiserKey) { CurrentPickerIndex = i; }
                i++;
            }
            MerchPick = new Picker
            {
                ItemsSource = merchList,
                SelectedIndex = CurrentPickerIndex
            };

            //create labels
            Label sNameLabel = new Label { Text = "Current Store Name:" };
            sNameLabel.Style = LabelStyle;
            Label mNameLabel = new Label { Text = "Current Store Manager:" };
            mNameLabel.Style = LabelStyle;
            Label sNumberLabel = new Label { Text = "Current Store Number:" };
            sNumberLabel.Style = LabelStyle;
            Label addressLabel = new Label { Text = "Current Store Address:" };
            addressLabel.Style = LabelStyle;
            Label merchLabel = new Label { Text = "Current Assigned Merchandiser:" };
            merchLabel.Style = LabelStyle;
            Label label = new Label { BackgroundColor = Color.Black, HeightRequest = 3, HorizontalOptions = LayoutOptions.FillAndExpand };
            //create stacklayout boxes to nest labels and entrys side by side
            StackLayout nameLayout = new StackLayout();
            nameLayout.Children.Add(sNameLabel);
            nameLayout.Children.Add(SName);
            nameLayout.Children.Add(label);

            StackLayout managerLayout = new StackLayout();
            managerLayout.Children.Add(mNameLabel);
            managerLayout.Children.Add(SMName);
            managerLayout.Children.Add(label);
            
            StackLayout numberLayout = new StackLayout();
            numberLayout.Children.Add(sNumberLabel);
            numberLayout.Children.Add(SNum);
            numberLayout.Children.Add(label);

            StackLayout addressLayout = new StackLayout();
            addressLayout.Children.Add(addressLabel);
            addressLayout.Children.Add(Addy);
            addressLayout.Children.Add(label);

            StackLayout merchLayout = new StackLayout();
            merchLayout.Children.Add(merchLabel);
            merchLayout.Children.Add(MerchPick);
            merchLayout.Children.Add(label);

            //save button
            Button Save = new Button { Text = "Save" };
            Save.Clicked += new EventHandler(SaveButton_Clicked);
            //delete button
            Button Delete = new Button { Text = "Delete" };
            Delete.Clicked += new EventHandler(DeleteClicked);
            // put it all together
            StackLayout content = new StackLayout
            {
                Children =
                {
                    frame, nameLayout, managerLayout, numberLayout, addressLayout, merchLayout,
                    Save, Delete 
                }
            };
            Content = new ScrollView { Content = content};
        }

        //call to create page based on pageType ticket
        private void TicketUDLItem(Frame frame)
        {
            
            //make pickers
            var i = 0;
            foreach (StoreDetails store in dataAccess.StoreDetails)
            {
                storeList.Add(store.StoreName);
                if (store.StoreName == TicketUpdate.Store) { CurrentPickerIndex = i; }
                i++;
            }
            StorePicker = new Picker
            {
                ItemsSource = storeList,
                SelectedIndex = CurrentPickerIndex
            };
            Label DateLabel = new Label
            {
                Style = LabelStyle
            };
            BookedDate = new DatePicker();
            BookedDate.DateSelected += CurrentDateSelected;
            void CurrentDateSelected(object sender, DateChangedEventArgs e)
            {
                DateLabel.Text = e.NewDate.Date.ToString("d");
            }
            BookedDate.Date = Convert.ToDateTime(TicketUpdate.BookedDate);

            //create labels
            Label sNameLabel = new Label { Text = "Current Store Name:" };
            sNameLabel.Style = LabelStyle;
            Label bookedLabel = new Label { Text = "Current Booked Date:" };
            bookedLabel.Style = LabelStyle;
            Label label = new Label { BackgroundColor = Color.Black, HeightRequest = 3, HorizontalOptions = LayoutOptions.FillAndExpand , HorizontalTextAlignment = TextAlignment.Center};

            //create stacklayout boxes to nest labels and entrys side by side
            StackLayout nameLayout = new StackLayout();
            nameLayout.Children.Add(sNameLabel);
            nameLayout.Children.Add(StorePicker);
            nameLayout.Children.Add(label);

            StackLayout dateLayout = new StackLayout();
            dateLayout.Children.Add(bookedLabel);
            dateLayout.Children.Add(DateLabel);
            dateLayout.Children.Add(BookedDate);
            dateLayout.Children.Add(label);

            //save button
            Button Save = new Button { Text = "Save" };
            Save.Clicked += new EventHandler(SaveButton_Ticket);
            //delete button
            Button Delete = new Button { Text = "Delete" };
            Delete.Clicked += new EventHandler(DeleteTicket);
            Content = new StackLayout
            {
                Children =
                {
                    frame, nameLayout, dateLayout, Save, Delete  
                }
            };
        }
        //seperate method for styling label indiviually
        private void SetLabelStyle()
        {
            //label style
            LabelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) },
                    new Setter {Property = Label.TextColorProperty, Value = Color.Black},
                    new Setter {Property = Label.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter {Property = Label.VerticalOptionsProperty, Value = LayoutOptions.CenterAndExpand },
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Start},
                    new Setter {Property = Label.MarginProperty, Value = new Thickness(30, 0, 30, 0)},
                    new Setter {Property = Label.FontAttributesProperty, Value = FontAttributes.Bold}
                }
            };
        }
        //ticket save
        private async void SaveButton_Ticket(object sender, EventArgs args)
        {
            TicketUpdate.StoreKey = GetStoreId();
            TicketUpdate.Store = StorePicker.SelectedItem.ToString();
            TicketUpdate.BookedDate = BookedDate.Date.ToString("d");
            ticketDataAccess.SaveTicket(TicketUpdate);
            await DisplayAlert("Ticket Update", "Ticket has been updated.", "Ok");
            Page page = new TicketListView();
            await Navigation.PopAsync();
            await Navigation.PopAsync();
            await Navigation.PushAsync(page);
        }

        private int GetStoreId()
        {
            if (StorePicker.SelectedIndex != -1)
            {
                var id = 0;
                foreach (StoreDetails store in dataAccess.StoreDetails)
                {
                    if (store.StoreName == StorePicker.SelectedItem.ToString())
                    {
                        id = store.StoreId;
                    }
                }
                return id;
            }
            else
            {
                return 0;
            }
        }

        //delete ticket
        private async void DeleteTicket(object sender, EventArgs args)
        {
            var answer = await DisplayAlert("Confirm Data Deletion", "Are you sure you want to delete this?", "Yes", "No");
            if (answer) { DeleteConfirmed = true; }
            if (DeleteConfirmed != false)
            {
                ticketDataAccess.DeleteTicket(TicketUpdate);
                Page page = new TicketListView();
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PushAsync(page);
                DeleteConfirmed = false;
            }
        }

        private void StylePage()
        {
            //styles
            //stacklayout style
            //title layout
            Title.Style = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter {Property = Label.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Button)) },
                    new Setter {Property = Label.TextColorProperty, Value = Color.White},
                    new Setter {Property = Label.HorizontalOptionsProperty, Value = LayoutOptions.CenterAndExpand},
                    new Setter {Property = Label.VerticalOptionsProperty, Value = LayoutOptions.Start },
                    new Setter {Property = Label.HorizontalTextAlignmentProperty, Value = TextAlignment.Center},
                    new Setter {Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Start },
                    new Setter {Property = Label.MarginProperty, Value = new Thickness(0)},
                    new Setter {Property = Label.PaddingProperty, Value = 0 },
                    new Setter {Property = Label.FontAttributesProperty, Value = FontAttributes.Bold},
                    new Setter {Property = Label.FontSizeProperty, Value = 36 }
                }
            };
            var titleFrame = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter {Property = Frame.BackgroundColorProperty, Value = ColorConverters.FromHex("#2196F3") },
                }
            };
            //button style
            var buttonStyle = new Style(typeof(Button))
            {
                Setters =
                {
                    new Setter {Property = Button.BackgroundColorProperty, Value = Color.LightCyan},
                    new Setter {Property = Button.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Large, typeof(Button))},
                    new Setter {Property = Button.TextColorProperty, Value = Color.Black},
                    new Setter {Property = Button.FontAttributesProperty, Value = FontAttributes.Bold},
                    new Setter {Property = Button.BorderColorProperty, Value = Color.Black},
                    new Setter {Property = Button.MarginProperty, Value = 15},
                    new Setter {Property = Button.CornerRadiusProperty, Value = 20},
                    new Setter {Property = Button.BorderWidthProperty, Value = 2},
                }
            };
            //entry style
            var entryStyle = new Style(typeof(Entry))
            {
                Setters =
                {
                    new Setter {Property = Entry.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)) },
                    new Setter {Property = Entry.TextColorProperty, Value = Color.Black},
                    new Setter {Property = Entry.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter {Property = Entry.VerticalOptionsProperty, Value = LayoutOptions.CenterAndExpand },
                    new Setter {Property = Entry.HorizontalTextAlignmentProperty, Value = TextAlignment.Start},
                    new Setter {Property = Entry.MarginProperty, Value = new Thickness(30, 0, 30, 0)},
                    new Setter {Property = Entry.FontAttributesProperty, Value = FontAttributes.Bold}
                }
            };
            //picker style
            var pickerStyle = new Style(typeof(Picker))
            {
                Setters =
                {
                    new Setter {Property = Picker.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Picker)) },
                    new Setter {Property = Picker.TextColorProperty, Value = Color.Black},
                    new Setter {Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter {Property = View.VerticalOptionsProperty, Value = LayoutOptions.CenterAndExpand },
                    new Setter {Property = Picker.HorizontalTextAlignmentProperty, Value = TextAlignment.Start},
                    new Setter {Property = View.MarginProperty, Value = new Thickness(30, 0, 30, 0)},
                    new Setter {Property = Picker.FontAttributesProperty, Value = FontAttributes.Bold}
                }
            };
            var datePickerStyle = new Style(typeof(DatePicker))
            {
                Setters =
                {
                    new Setter {Property = DatePicker.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Picker)) },
                    new Setter {Property = DatePicker.TextColorProperty, Value = Color.Black},
                    new Setter {Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter {Property = View.VerticalOptionsProperty, Value = LayoutOptions.CenterAndExpand },
                    new Setter {Property = View.MarginProperty, Value = new Thickness(30, 0, 30, 10)},
                    new Setter {Property = DatePicker.FontAttributesProperty, Value = FontAttributes.Bold}
                }
            };
            //create resource dictionary
            Resources = new ResourceDictionary
            {
                entryStyle,
                buttonStyle,
                titleFrame,
                pickerStyle,
                datePickerStyle
            };
        }
    }
}