using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;
using System.ComponentModel;
using Project.Database;

namespace Project.DataClasses
{
    class StoreDetails : INotifyPropertyChanged
    {
        private int _storeId;
        [PrimaryKey, AutoIncrement, NotNull]
        public int StoreId
        {
            get { return _storeId; }
            set { _storeId = value; OnPropertyChanged(nameof(StoreId)); }
        }

        private string _storeName;
        [NotNull, DefaultValue(value: "Enter Store Name")]
        public string StoreName
        {
            get { return _storeName; }
            set { _storeName = value; OnPropertyChanged(nameof(_storeName)); }
        }

        private string _storeManger;
        [NotNull, DefaultValue(value: "Enter Store Managers Name")]
        public string StoreManger
        {
            get { return _storeManger; }
            set { _storeManger = value; OnPropertyChanged(nameof(StoreManger)); }
        }

        private string _storeNumber;
        [NotNull, DefaultValue(value: "Enter Store Number")]
        public string StoreNumber
        {
            get { return _storeNumber; }
            set { _storeNumber = value; OnPropertyChanged(nameof(StoreNumber)); }
        }

        private string _address;
        [NotNull, DefaultValue(value: "Enter Address")]
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        private int _merchandiserKey;
        public int MerchandiserKey
        {
            get { return _merchandiserKey; }
            set { _merchandiserKey = value; SetMerchName(MerchandiserKey); OnPropertyChanged(nameof(MerchandiserKey)); }
        }

        private void SetMerchName(int merchandiserKey)
        {
            if(merchandiserKey != 0)
            {
                var merchData = new MerchandiserDataAccess();
                MerchandiserName = merchData.GetMerch(merchandiserKey).Name;
            }
            else { return; }
        }

        private string _merchandiserName;
        public string MerchandiserName
        {
            get { return _merchandiserName; }
            set { _merchandiserName = value; OnPropertyChanged(nameof(MerchandiserName)); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
