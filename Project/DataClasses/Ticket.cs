using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;
using System.ComponentModel;
using Project.Database;

namespace Project.DataClasses
{
    class Ticket : INotifyPropertyChanged
    {
        private int _id;
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id
        {
            get { return _id; }
            set { this._id = value; OnPropertyChanged(nameof(Id)); }
        }
        private int _storeKey;
        public int StoreKey
        {
            get { return _storeKey; }
            set { _storeKey = value; SetMerchandiser(StoreKey); OnPropertyChanged(nameof(StoreKey)); }
        }
        private string _store;
        public string Store 
        {
            get { return _store; } 
            set { this._store = value; OnPropertyChanged(nameof(Store)); } 
        }

        private string _comments;
        [MaxLength(150)]
        public string Comments 
        { 
            get { return _comments; }
            set { this._comments = value; OnPropertyChanged(nameof(Comments)); }
        }

        private string _bookedDate;
        public string BookedDate
        {
            get { return _bookedDate; }
            set { _bookedDate = value; OnPropertyChanged(nameof(BookedDate)); }
        }

        private string _startTime;
        public string StartTime 
        { 
            get { return _startTime; }
            set { this._startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        private string _endTime;
        public string EndTime 
        { 
            get { return _endTime; }
            set { this._endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        private string _completedTime;
        public string CompletedTime 
        { 
            get { return _completedTime; }
            set { this._completedTime = value; OnPropertyChanged(nameof(CompletedTime)); }
        }

        private string _completedDate;
        public string CompletedDate
        {
            get { return _completedDate; }
            set { _completedDate = value; OnPropertyChanged(nameof(CompletedDate)); }
        }

        private byte[] _displayPhoto;
        public byte[] DisplayPhoto
        {
            get { return _displayPhoto; }
            set { this._displayPhoto = value; OnPropertyChanged(nameof(DisplayPhoto)); }
        }

        private string _merchName;
        public string MerchName
        {
            get { return _merchName; }
            set { this._merchName = value; OnPropertyChanged(nameof(MerchName)); }
        }
        private void SetMerchandiser(int storeKey)
        {
            if (storeKey != 0)
            {
                var storeData = new StoreDataAccess();
                MerchName = storeData.GetStoreDetails(storeKey).MerchandiserName.ToString();
            }
            else
            {
                return;
            }
        
        }
        
        public DateTime OrderRef
        {
            get { return DateTime.Parse(_bookedDate); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
