using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.ComponentModel;

namespace Project.DataClasses
{
    class Merchandiser : INotifyPropertyChanged
    {
        private int _merchId;
        [PrimaryKey, AutoIncrement]
        public int MerchId
        {
            get { return _merchId; }
            set { _merchId = value; OnPropertyChanged(nameof(MerchId)); }
        }

        private string _name;
        [NotNull]
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _contactNum;
        [NotNull]
        public string ContactNum
        {
            get { return _contactNum; }
            set { _contactNum = value; OnPropertyChanged(nameof(ContactNum)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}