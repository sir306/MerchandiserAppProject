using Project.Database;
using Project.DataClasses;
using Project.ListPages;
using Project.Pages.ListPages;
using Project.Pages.SuperPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Project.Pages.MerchPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MerchSelectedTicket : ContentPage
    {
        //data access
        private readonly CompletedTicketDataAccess completedTicketDataAccess;
        private readonly TicketDataAccess ticketDataAccess;
        //required assignments
        private readonly CompletedTickets completedTickets;
        private Style LabelStyle;
        private Style TitleLabelStyle;
        private Style FrameStyle;
        private Style EntryStyle;
        private Style TimePickStyle;
        private Style ButtonStyle;
        private Ticket TicketTask;
        private Entry entryComments;
        private TimePicker startTimePicker;
        private bool startTimeFlag = false;
        private TimePicker endTimePicker;
        private bool endTimeFlag = false;
        private TimeSpan totalTime;
        private DateTime completedDate;
        private Button TakePhoto;
        private Image displayImageTaken;
        private byte[] imageByte;
        private StackLayout imageStack;
        private Button completedBtn;
        private Stream StreamCon;

        public object PhotoPath { get; private set; }
        public Command TicketCompleted { get; }

        public MerchSelectedTicket(object Item)
        {
            InitializeComponent();
            SetLabelStyle();
            StylePage();
            TicketCompleted = new Command(() => TaskCompleted());
            BindingContext = Item;
            //set data access up here
            ticketDataAccess = new TicketDataAccess();
            completedTicketDataAccess = new CompletedTicketDataAccess();
            completedTickets = new CompletedTickets();
            //initial timespan
            totalTime = new TimeSpan(0);
            BuildLayout(Item);
        }
        private void TaskCompleted()
        {
            if(CheckValues() != false)
            {
                completedDate = DateTime.Now;
                completedTickets.BookedDate = TicketTask.BookedDate;
                completedTickets.StoreKey = TicketTask.StoreKey;
                completedTickets.Store = TicketTask.Store;
                completedTickets.Comments = entryComments.Text;
                completedTickets.StartTime = startTimePicker.Time.ToString("t");
                completedTickets.EndTime = endTimePicker.Time.ToString("t");
                completedTickets.CompletedTime = CalculateTimeSpent();
                completedTickets.CompletedDate = completedDate.Date.ToString("d");
                completedTickets.DisplayPhoto = imageByte;
                completedTicketDataAccess.AddNewTicket(completedTickets);
                completedTicketDataAccess.SaveAllTickets();
                ticketDataAccess.DeleteTicket(TicketTask);
                StreamCon.Dispose();
                DisplayAlert("Ticket Completed", "Ticket has been completed and removed from active tickets", "OK");
                Page page = new ActiveTicketsMerch();
                Navigation.PopAsync();
                Navigation.PopAsync();
                Navigation.PushAsync(page);
            }
            else
            {
                DisplayAlert("Task Incomplete", "You haven't filled in the required fields to complete this Ticket.", "OK");
            }
            
        }

        private string CalculateTimeSpent()
        {
            totalTime = endTimePicker.Time - startTimePicker.Time;
            return totalTime.ToString("t");
        }

        private bool CheckValues()
        {
            if(startTimeFlag == false) { startTimePicker.Focus(); return false; }
            if(endTimeFlag == false) { endTimePicker.Focus(); return false; }
            if(imageByte == null) { TakePhoto.Focus(); return false; }
            else { return true; }
        }

        private void BuildLayout(object item)
        {
            #region
            StackLayout stackLayout = new StackLayout() { BackgroundColor = Color.LightGray };
            //convert object into its ticket object
            TicketTask = (Ticket)item;
            Frame frame = new Frame() { Style = FrameStyle };
            Label title = new Label() { Text = "Current Selected Ticket Job", Style = TitleLabelStyle };
            StackLayout titleStack = new StackLayout() { Children = { title } };
            frame.Content = titleStack;
            //build pickers and assign them to assignments above 
            startTimePicker = new TimePicker() { Style = TimePickStyle};
            startTimePicker.PropertyChanged += StartTimeChanged;
            endTimePicker = new TimePicker() { Style = TimePickStyle };
            endTimePicker.PropertyChanged += EndTimeChanged;

            //build image related components
            imageStack = BuildImageStack();

            //build buttons
            completedBtn = BuildCompleteBtn();

            
            //entrys
            entryComments = new Entry() { Style = EntryStyle };
            //create labels required
            Label ticketNumber = new Label { Text = "Ticket Number: " + TicketTask.Id.ToString(), Style = LabelStyle };
            Label storeName = new Label { Text = "Store: " + TicketTask.Store, Style = LabelStyle };
            Label commentTitle = new Label { Text = "Enter Comments:", Style = LabelStyle };
            Label startTimeTitle = new Label { Text = "Select Start Time:", Style = LabelStyle };
            Label endTimeTitle = new Label { Text = "Select End Time:", Style = LabelStyle };
            //label line for seperation
            Label label = new Label { BackgroundColor = Color.Black, HeightRequest = 4, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label1 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label2 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label3 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label4 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label6 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label7 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            Label label8 = new Label { BackgroundColor = Color.Black, HeightRequest = 4, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };

            stackLayout.Children.Add(frame);
            stackLayout.Children.Add(label);
            stackLayout.Children.Add(ticketNumber);
            stackLayout.Children.Add(label1);
            stackLayout.Children.Add(storeName);
            stackLayout.Children.Add(label2);
            stackLayout.Children.Add(commentTitle);
            stackLayout.Children.Add(entryComments);
            stackLayout.Children.Add(label3);
            stackLayout.Children.Add(startTimeTitle);
            stackLayout.Children.Add(startTimePicker);
            stackLayout.Children.Add(label4);
            stackLayout.Children.Add(endTimeTitle);
            stackLayout.Children.Add(endTimePicker);
            stackLayout.Children.Add(label6);
            stackLayout.Children.Add(imageStack);
            stackLayout.Children.Add(label8);
            stackLayout.Children.Add(completedBtn);

            Content =  new ScrollView { Content = stackLayout }; 
        }
#endregion
        
        private void EndTimeChanged(object sender, PropertyChangedEventArgs e)
        {
            endTimeFlag = true;
        }

        private void StartTimeChanged(object sender, PropertyChangedEventArgs e)
        {
            startTimeFlag = true;
        }

        private StackLayout BuildImageStack()
        {
            StackLayout stack = new StackLayout();
            Label takePhotoTitle = new Label { Text = "Display Photo:", Style = LabelStyle };
            Label label7 = new Label { BackgroundColor = Color.Black, HeightRequest = 2, HorizontalOptions = LayoutOptions.FillAndExpand, Margin = new Thickness(0, 10, 0, 10) };
            string path = @"C:/Project/Project/blank.bmp";
            displayImageTaken = new Image
            {
                Source = path
            };
            TakePhoto = new Button { Text = "Take Photo", Command = new Command(async () => await TakePhotoAsync()), Style = ButtonStyle };
            //create grid to house photo
            var grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height = new GridLength(200, GridUnitType.Absolute)},
                    new RowDefinition { Height = GridLength.Auto},
                    new RowDefinition { Height = GridLength.Auto},
                }
            };
            grid.Children.Add(takePhotoTitle, 0, 0);
            grid.Children.Add(displayImageTaken, 0, 1);
            grid.Children.Add(label7, 0, 2);
            grid.Children.Add(TakePhoto, 0, 3);
            stack.Children.Add(grid);
            return stack;
        }

        private Button BuildCompleteBtn()
        {
            Button button = new Button { Text = "Task Completed", Style = ButtonStyle, Command = TicketCompleted };
            return button;
        }
        //photo taking region
        #region
        //take photo methods
        async Task TakePhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                // canceled
                if (photo == null)
                {
                    PhotoPath = null;
                    return;
                }
                else
                {
                    StreamCon = await photo.OpenReadAsync();
                    ConvertImageToByte(StreamCon);
                    //call turn bytes to stream to prevent stream from closing
                    displayImageTaken.Source = ImageSource.FromStream(() => TurnBytesToStream(imageByte));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        //save image stream to byte
        private void ConvertImageToByte(Stream stream)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                stream.CopyTo(memStream);
                imageByte = memStream.ToArray();
            }
        }
        //convert byte to stream for display
        private Stream TurnBytesToStream(byte[] bytes)
        {
            StreamCon = new MemoryStream(bytes);
            return StreamCon;
        }
        #endregion
        //style region
        #region 
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
        //styles
        
        private void StylePage()
        {
            //title layout
           TitleLabelStyle = new Style(typeof(Label))
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
            FrameStyle = new Style(typeof(Frame))
            {
                Setters =
                {
                    new Setter {Property = BackgroundColorProperty, Value = ColorConverters.FromHex("#2196F3") },
                }
            };
            //button style
            ButtonStyle = new Style(typeof(Button))
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
            EntryStyle = new Style(typeof(Entry))
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
            TimePickStyle = new Style(typeof(TimePicker))
            {
                Setters =
                {
                    new Setter {Property = TimePicker.FontSizeProperty, Value = Device.GetNamedSize(NamedSize.Medium, typeof(Picker)) },
                    new Setter {Property = TimePicker.TextColorProperty, Value = Color.Black},
                    new Setter {Property = View.HorizontalOptionsProperty, Value = LayoutOptions.FillAndExpand},
                    new Setter {Property = View.VerticalOptionsProperty, Value = LayoutOptions.CenterAndExpand },
                    new Setter {Property = View.MarginProperty, Value = new Thickness(30, 0, 30, 10)},
                    new Setter {Property = TimePicker.FontAttributesProperty, Value = FontAttributes.Bold}
                }
            };
            #endregion
        }
    }
}